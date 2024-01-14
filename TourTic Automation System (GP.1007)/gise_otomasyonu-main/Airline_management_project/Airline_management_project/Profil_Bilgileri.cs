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

namespace Airline_management_project
{
    public partial class Profil_Bilgileri : Form
    {
        
        public byte[] resimBytes;
        public string personel_ad { get; set; }
        public string personel_soyad { get; set; }
        public DateTime personel_dogum_tarihi { get; set; }
        public string personel_dogum_tarih;
        public string personel_tc_no { get; set; }
        public string personel_gorev { get; set; }
        public string personel_cinsiyet { get; set; }
        public string personel_tecrube { get; set; }
        DateTime dogum_tarihi_arama;
        public byte[] cv;



        SqlConnection baglanti = new SqlConnection("server=172.20.10.2;database=otomasyon_veritabani;UID=SA;password=reallyStrongPwd123");
        public Profil_Bilgileri()
        {
            InitializeComponent();
        }

        private void Profil_Bilgileri_Load(object sender, EventArgs e)
        {
            if(personel_gorev !="Supervisor")
            {
                panel2.Visible = false;
                button3.Visible = false;
            }
            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT * FROM personeller WHERE tc_no = @tc_no", baglanti);
            komut.Parameters.AddWithValue("@tc_no", personel_tc_no);
            SqlDataReader reader = komut.ExecuteReader();
            reader.Read();
            if(!reader.HasRows)
            {
                MessageBox.Show("Kullanıcı Bulunamadı. Program Kapatılacak.");
                Application.Exit();
            }
            label12.Text = reader["ad"].ToString() + " " + reader["soyad"].ToString();
            dogum_tarihi_arama = (DateTime)reader["dogum_tarihi"];
            label1.Text = dogum_tarihi_arama.ToString("dd-MM-yyyy");
            label9.Text = reader["gorev"].ToString();
            label10.Text = reader["tc_no"].ToString();
            label2.Text = reader["cinsiyet"].ToString();
            label6.Text = reader["tecrube"].ToString();
            byte[] fotograf = ((byte[])reader["fotoğraf"]);
            cv = ((byte[])reader["cv"]);
            MemoryStream ms = new MemoryStream(fotograf);
            Image resim_arama = Image.FromStream(ms);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            if(resim_arama.Width <= pictureBox1.Width && resim_arama.Height <= pictureBox1.Height)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            }
            pictureBox1.Image = resim_arama;
            reader.Close();
            baglanti.Close();
            // label12.Text = personel_ad + " " + personel_soyad;
            //personel_dogum_tarih = personel_dogum_tarihi.ToString("dd-MM-yyyy");
            //label1.Text = personel_dogum_tarih;
            //label10.Text = personel_tc_no;
            //label9.Text = personel_gorev;
            //label2.Text = personel_cinsiyet;
            //label6.Text = personel_tecrube;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            if (textBox1.Text == "")
            {
                MessageBox.Show("Lütfen arama için TC no giriniz.");
                baglanti.Close();
            }
            else
            {
                SqlCommand komut = new SqlCommand("SELECT * FROM personeller WHERE tc_no = @tc_no", baglanti);
                komut.Parameters.AddWithValue("@tc_no", textBox1.Text.ToString());
                SqlDataReader reader = komut.ExecuteReader();
                if (reader.Read())
                {
                    label12.Text = reader["ad"].ToString() + " " + reader["soyad"].ToString();
                    dogum_tarihi_arama = (DateTime)reader["dogum_tarihi"];
                    label1.Text = dogum_tarihi_arama.ToString("dd-MM-yyyy");
                    label9.Text = reader["gorev"].ToString();
                    label10.Text = reader["tc_no"].ToString();
                    label2.Text = reader["cinsiyet"].ToString();
                    label6.Text = reader["tecrube"].ToString();
                    byte[] fotograf = ((byte[])reader["fotoğraf"]);
                    cv = ((byte[])reader["cv"]);
                    reader.Close();
                    MemoryStream ms = new MemoryStream(fotograf);
                    Image resim_arama = Image.FromStream(ms);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    if (resim_arama.Width <= pictureBox1.Width && resim_arama.Height <= pictureBox1.Height)
                    {
                        pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                    }
                    pictureBox1.Image = resim_arama;
                    baglanti.Close();
                }
                else
                {
                    MessageBox.Show("Bu TC numarasına ait profil bulunamadı");
                    baglanti.Close();
                }
            }

        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Dosyaları | *.pdf";
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string dosyaYolu = saveFileDialog.FileName;
                File.WriteAllBytes(dosyaYolu,cv);
            }
        }
    }
}
