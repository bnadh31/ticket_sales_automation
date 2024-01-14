using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp;
using ZXing;


namespace Airline_management_project
{
    public partial class BiletMenusu : Form
    {
        public BiletMenusu()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("server=172.20.10.2;database=otomasyon_veritabani;UID=SA;password=reallyStrongPwd123");

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            if (textBox1.Text == "")
            {
                MessageBox.Show("Lütfen sorgulama için doğru bir TC/Pasaport no giriniz.");
                baglanti.Close();
            }
            else
            {
                SqlDataAdapter komut = new SqlDataAdapter("SELECT biletler.bilet_id, biletler.koltuk_no, biletler.sefer_id,biletler.bilet_turu ,musteri_bilgileri.tc_pas_no, musteri_bilgileri.ad, musteri_bilgileri.soyad FROM biletler,musteri_bilgileri WHERE biletler.musteri_id = musteri_bilgileri.musteri_id AND musteri_bilgileri.tc_pas_no = '" + textBox1.Text + "'", baglanti);
                DataTable dt = new DataTable();
                DataView dv = new DataView();
                AnaForm main_page = new AnaForm();
                komut.Fill(dt);
                dv = dt.DefaultView;
                dataGridView3.DataSource = dv;
                baglanti.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            textBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            if (textBox2.Text == "")
            {
                MessageBox.Show("Lütfen sorgulama için doğru bir TC/Pasaport no giriniz.");
                baglanti.Close();
            }
            else
            {
                SqlDataAdapter komut = new SqlDataAdapter("SELECT biletler.bilet_id, biletler.koltuk_no, biletler.sefer_id,biletler.bilet_turu ,musteri_bilgileri.tc_pas_no, musteri_bilgileri.ad, musteri_bilgileri.soyad " +
                    "FROM biletler,musteri_bilgileri " +
                    "WHERE biletler.musteri_id = musteri_bilgileri.musteri_id AND musteri_bilgileri.tc_pas_no = '" + textBox2.Text + "'", baglanti);
                DataTable dt = new DataTable();
                DataView dv = new DataView();
                AnaForm main_page = new AnaForm();
                komut.Fill(dt);
                dv = dt.DefaultView;
                dataGridView1.DataSource = dv;
                baglanti.Close();

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Lütfen bileti seçiniz.");
                baglanti.Close();
            }
            else
            {
                SqlCommand komut2 = new SqlCommand("SELECT check_in.check_in_id FROM check_in, biletler, musteri_bilgileri WHERE musteri_bilgileri.musteri_id = biletler.musteri_id AND biletler.bilet_id = check_in.bilet_id AND biletler.bilet_id = check_in.bilet_id AND biletler.bilet_id = @bilet_id", baglanti);
                komut2.Parameters.AddWithValue("@bilet_id",dataGridView1.CurrentRow.Cells[0].Value.ToString());
                SqlDataReader reader = komut2.ExecuteReader();
                if (reader.HasRows)
                {
                    MessageBox.Show("Bu bilete zaten Check-In yapılmıştır.");
                    reader.Close();
                    baglanti.Close();
                }
                else
                {
                    reader.Close();
                    DateTime bugununTarihi = DateTime.Today;
                    string bilet_id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    SqlCommand komut = new SqlCommand("INSERT INTO check_in(check_in_tarihi, bilet_id)VALUES (@Tarih,@bilet_id)", baglanti);
                    komut.Parameters.AddWithValue("@Tarih", bugununTarihi);
                    komut.Parameters.AddWithValue("@bilet_id", bilet_id);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Check-In işlemi başarıyla gerçekleşti.");
                    baglanti.Close();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            if (textBox3.Text == "")
            {
                MessageBox.Show("Lütfen sorgulama için doğru bir TC/Pasaport no giriniz.");
                baglanti.Close();
            }
            else
            {
                SqlDataAdapter komut = new SqlDataAdapter("SELECT biletler.bilet_id,check_in.check_in_id,biletler.koltuk_no,biletler.bilet_turu, musteri_bilgileri.ad,musteri_bilgileri.soyad FROM biletler, check_in, musteri_bilgileri WHERE musteri_bilgileri.musteri_id = biletler.musteri_id AND biletler.bilet_id = check_in.bilet_id AND musteri_bilgileri.tc_pas_no = '"+textBox3.Text+"'", baglanti);
                DataTable dt = new DataTable();
                DataView dv = new DataView();
                AnaForm main_page = new AnaForm();
                komut.Fill(dt);
                dv = dt.DefaultView;
                dataGridView2.DataSource = dv;
                baglanti.Close();
                button6.Visible = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            if (dataGridView2.CurrentRow == null)
            {
                MessageBox.Show("Lütfen satır bilet seçimi yapınız.");
                baglanti.Close();
            }
            else
            {
                SaveFileDialog file = new SaveFileDialog();
                PrintDialog PD = new PrintDialog();
                file.Filter = "PDF Dosyaları(*.pdf) |*.pdf";
                file.Title = "Biniş Kartı Kaydet";
                file.OverwritePrompt = false;
                file.FileName = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                if (file.ShowDialog() == DialogResult.OK)
                {
                    System.Drawing.Bitmap bitmap = Properties.Resources.TourTic_Logo;
                    MemoryStream ms = new MemoryStream();
                    SqlCommand komut1 = new SqlCommand("SELECT musteri_bilgileri.ad,musteri_bilgileri.soyad, seferler.kalkis_yeri, seferler.varis_yeri, seferler.kalkis_saati,biletler.koltuk_no,biletler.bilet_turu FROM biletler,musteri_bilgileri,seferler WHERE seferler.sefer_id = biletler.sefer_id AND musteri_bilgileri.musteri_id = biletler.musteri_id AND bilet_id = @bilet_id", baglanti);
                    komut1.Parameters.AddWithValue("@bilet_id", dataGridView2.CurrentRow.Cells[0].Value.ToString());
                    SqlDataReader reader2 = komut1.ExecuteReader();
                    reader2.Read();
                    string vCardData = $"BEGIN:VCARD\n" +
                               $"VERSION:2.1\n" +
                               $"N:{reader2["ad"]};;;\n" +
                               $"SN;CELL:{reader2["soyad"]}\n" +
                               $"KY:{reader2["kalkis_yeri"]}\n" +
                               $"KN:{reader2["koltuk_no"]}\n" +
                               $"BiletID:{dataGridView2.CurrentRow.Cells[0].Value}\n" +
                               $"END:VCARD";
                    BarcodeWriter writer1 = new BarcodeWriter();
                    writer1.Format = BarcodeFormat.QR_CODE;
                    writer1.Options = new ZXing.Common.EncodingOptions
                    {
                        Width = 300,
                        Height = 300,
                        Margin = 0
                    };
                    Bitmap qrCode = writer1.Write(vCardData);
                    FileStream dosya = File.Open(file.FileName, FileMode.Create);
                    iTextSharp.text.Document pdf = new iTextSharp.text.Document(PageSize.A4, 15f, 15f, 75f, 75f);
                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdf, dosya);
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    iTextSharp.text.Image resim = iTextSharp.text.Image.GetInstance(ms.GetBuffer());
                    iTextSharp.text.Image qr = iTextSharp.text.Image.GetInstance(qrCode, System.Drawing.Imaging.ImageFormat.Png);
                    resim.ScalePercent(27f);
                    pdf.AddAuthor("TourTick A.S.");
                    pdf.AddCreator("TourTick A.S.");
                    pdf.AddCreationDate();
                    pdf.Open();
                    Paragraph paragraf = new Paragraph("Yolcu Adi : " + reader2["ad"].ToString());
                    Paragraph paragraf1 = new Paragraph("Yolcu Soyadi : " + reader2["soyad"].ToString());
                    Paragraph paragraf2 = new Paragraph("Sefer No : " + dataGridView2.CurrentRow.Cells[0].Value.ToString());
                    Paragraph paragraf3 = new Paragraph("Koltuk No : " + reader2["koltuk_no"].ToString());
                    Paragraph paragraf4 = new Paragraph("Bilet Tipi: " + reader2["bilet_turu"].ToString());
                    Paragraph paragraf5 = new Paragraph("Kalkis Yeri : " + reader2["kalkis_yeri"].ToString());
                    Paragraph paragraf6 = new Paragraph("Varis Yeri : " + reader2["varis_yeri"].ToString());
                    Paragraph paragraf7 = new Paragraph("Kalkis Saati : " + reader2["kalkis_saati"].ToString());
                    paragraf.Alignment = Element.ALIGN_CENTER;
                    paragraf1.Alignment = Element.ALIGN_CENTER;
                    paragraf2.Alignment = Element.ALIGN_CENTER;
                    paragraf3.Alignment = Element.ALIGN_CENTER;
                    paragraf4.Alignment = Element.ALIGN_CENTER;
                    paragraf5.Alignment = Element.ALIGN_CENTER;
                    paragraf6.Alignment = Element.ALIGN_CENTER;
                    paragraf7.Alignment = Element.ALIGN_CENTER;
                    resim.Alignment = Element.ALIGN_CENTER;
                    qr.Alignment = Element.ALIGN_CENTER;
                    reader2.Close();
                    pdf.Add(resim);
                    pdf.Add(paragraf);
                    pdf.Add(paragraf1);
                    pdf.Add(paragraf2);
                    pdf.Add(paragraf3);
                    pdf.Add(paragraf4);
                    pdf.Add(paragraf5);
                    pdf.Add(paragraf6);
                    pdf.Add(paragraf7);
                    pdf.Add(qr);
                    pdf.Close();
                    MessageBox.Show("Yazdırma Başarılı");
                    baglanti.Close();
                }
            }
        }
    }
}
