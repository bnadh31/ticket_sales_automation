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
    public partial class PersonelFormu : Form
    {
        byte[] fotoBytes;
        byte[] fotoBytes2;
        string dosyaYolu;
        public string dosyaYolu2;
        public string tur;
        string birim;
        public PersonelFormu()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("server=172.20.10.2;database=otomasyon_veritabani;UID=SA;password=reallyStrongPwd123");
        private void PersonelFormu_Load(object sender, EventArgs e)
        {
            dataGridView3.AutoGenerateColumns = true;
            SqlDataAdapter komut = new SqlDataAdapter("SELECT * FROM personeller", baglanti);
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            komut.Fill(dt);
            dv = dt.DefaultView;
            dataGridView3.DataSource = dv;
            dataGridView4.DataSource = dv;
            dataGridView3.Columns["cv"].Visible = false;
            dataGridView3.Columns["fotoğraf"].Visible = false;
            dataGridView4.Columns["cv"].Visible = false;
            dataGridView4.Columns["fotoğraf"].Visible = false;
            dataGridView4.Columns["sifre"].Visible = false;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            dateTimePicker3.Format = DateTimePickerFormat.Custom;
            dateTimePicker3.CustomFormat = "yyyy-MM-dd";
            panel1.Visible = false;
            dataGridView3.Columns["calistigi_birim"].Visible = false;
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Fotoğraf Yükleme";
            openFileDialog1.Filter = "Resim Dosyaları | *.jpg; *.jpeg; *.png; *.gif";
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                dosyaYolu = openFileDialog1.FileName;
                FileInfo fileInfo = new FileInfo(dosyaYolu);
                Image fotograf = Image.FromFile(dosyaYolu);
                long ByteSize = fileInfo.Length;
                long sizeMb = ByteSize / 1024;
                int width = fotograf.Width;
                int height = fotograf.Height;
                if (sizeMb > 50)
                {
                    MessageBox.Show("Yüklediğiniz dosya 25 MB'tan düşük olmalıdır.");
                    return;
                }
                if (width > 250 || height > 280)
                {
                    MessageBox.Show("Yüklediğiniz dosya 250 x 280 boyutunda ve ya altında olmalıdır.");
                    return;
                }
                button9.Visible = false;
                button11.Visible = true;
                fotoBytes = File.ReadAllBytes(dosyaYolu);
                label45.Text = dosyaYolu;
                label45.Visible = true;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            dosyaYolu = "";
            label45.Text = "";
            label45.Visible = false;
            button9.Visible = true;
            button11.Visible = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            openFileDialog2.Title = "CV Yükleme";
            openFileDialog2.Filter = "PDF Dosyaları | *.pdf";
            openFileDialog2.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                dosyaYolu2 = openFileDialog2.FileName;
                FileInfo fileInfo = new FileInfo(dosyaYolu2);
                
                if(fileInfo.Length > 25 * 1024 * 1024)
                {
                    MessageBox.Show("Yüklediğiniz dosya 25 MB'tan düşük olmalıdır.");
                }
                button10.Visible = false;
                button12.Visible = true;
                fotoBytes2 = File.ReadAllBytes(dosyaYolu2);
                label46.Text = dosyaYolu2;
                label46.Visible = true;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            dosyaYolu2 = "";
            label46.Text = "";
            label46.Visible = false;
            button10.Visible = true;
            button12.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut1 = new SqlCommand("SELECT * FROM personeller WHERE tc_no = @tc_no", baglanti);
            komut1.Parameters.AddWithValue("@tc_no", textBox7.Text.ToString());
            SqlDataReader reader = komut1.ExecuteReader();
            if (reader.HasRows)
            {
                MessageBox.Show("Bu TC numarasına ait personel kaydı daha önce yapılmıştır.");
                reader.Close();
                baglanti.Close();
            }
            else
            {
                try
                {
                    reader.Close();
                    SqlCommand komut = new SqlCommand("INSERT INTO personeller(ad, soyad, tc_no, dogum_tarihi, gorev,fotoğraf,tecrube,cinsiyet,cv) VALUES(@ad,@soyad,@tc_no,@dogum_tarihi,@gorev,@fotograf,@tecrube,@cinsiyet,@cv)", baglanti);
                    komut.Parameters.AddWithValue("@ad", textBox1.Text.ToString());
                    komut.Parameters.AddWithValue("@soyad", textBox2.Text.ToString());
                    komut.Parameters.AddWithValue("@tc_no", textBox7.Text.ToString());
                    komut.Parameters.AddWithValue("@dogum_tarihi", dateTimePicker1.Value.ToString());
                    komut.Parameters.AddWithValue("@gorev", comboBox2.Text.ToString());
                    komut.Parameters.AddWithValue("@fotograf", fotoBytes);
                    komut.Parameters.AddWithValue("@tecrube", dateTimePicker3.Value.ToString());
                    komut.Parameters.AddWithValue("@cinsiyet", comboBox3.Text.ToString());
                    komut.Parameters.AddWithValue("@cv", fotoBytes2);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Personel Başarıyla Eklendi.");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox7.Text = "";
                    comboBox3.Text = "";
                    comboBox2.Text = "";
                    dateTimePicker3.Text = "";
                    dateTimePicker1.Text = "";
                    baglanti.Close();
                }
                catch (OutOfMemoryException hata)
                {
                    MessageBox.Show("Görsel yükleme başarısız. Hata kodu : \n\n" + hata);
                    baglanti.Close();
                }
                catch (Exception sql)
                {
                    MessageBox.Show("Bir hata ile karşılaşıldı. Lütfen tekrar deneyiniz.\n\n" + sql);
                    baglanti.Close();
                }
                
            }
        }
        private void PersonelFormu_FormClosing(object sender, FormClosingEventArgs e)
        {
            dataGridView3.Columns["cv"].Visible = false;
            dataGridView3.Columns["fotoğraf"].Visible = false;
            dataGridView3.Columns["sifre"].Visible = false;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlDataAdapter komut = new SqlDataAdapter("SELECT * FROM personeller WHERE tc_no = '" + textBox5.Text + "'", baglanti);
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            AnaForm main_page = new AnaForm();
            komut.Fill(dt);
            dv = dt.DefaultView;
            dataGridView1.DataSource = dv;
            dataGridView1.Columns["cv"].Visible = false;
            dataGridView1.Columns["fotoğraf"].Visible = false;
            dataGridView1.Columns["sifre"].Visible = false;
            baglanti.Close();
            button4.Visible = true;
            baglanti.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("DELETE FROM personeller WHERE tc_no = @tc_no", baglanti);
                komut.Parameters.AddWithValue("@tc_no", textBox5.Text.ToString());
                komut.ExecuteNonQuery();
                MessageBox.Show("Personel başarıyla silindi.");
                baglanti.Close();
            }
            catch (Exception sql)
            {
                MessageBox.Show("Personel silinemedi. Hata kodu : \n\n" + sql);
                baglanti.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            try
            {
                SqlCommand komut = new SqlCommand("UPDATE personeller SET ad = @ad ,soyad = @soyad, tc_no = @tc_no, " +
                    "dogum_tarihi = @dogum_tarihi, gorev = @gorev, tecrube = @tecrube, cinsiyet = @cinsiyet WHERE tc_no = @tc_no", baglanti);

                // SqlCommand oluşturma
                komut.Parameters.AddWithValue("@ad", textBox17.Text.ToString());
                komut.Parameters.AddWithValue("@soyad", textBox9.Text.ToString());
                komut.Parameters.AddWithValue("@tc_no", textBox3.Text.ToString());
                komut.Parameters.AddWithValue("@dogum_tarihi", dateTimePicker4.Value.ToString());
                komut.Parameters.AddWithValue("@gorev", comboBox4.Text.ToString());
                komut.Parameters.AddWithValue("@tecrube", dateTimePicker2.Value.ToString());
                komut.Parameters.AddWithValue("@cinsiyet", comboBox1.Text.ToString());
                int kontrol = komut.ExecuteNonQuery();
                if (kontrol > 0)
                {
                    MessageBox.Show("Güncelleme işlemi başarılı !");

                }
                else
                {
                    MessageBox.Show("Güncelleme işlemi başarısız oldu.");
                }
                baglanti.Close();
            }
            catch (Exception sql)
            {
                MessageBox.Show("Güncelleme hatası ! Hata kodu : \n\n" + sql);
                baglanti.Close();
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlDataAdapter komut = new SqlDataAdapter("SELECT * FROM personeller WHERE tc_no = '" + textBox6.Text + "'", baglanti);
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            AnaForm main_page = new AnaForm();
            komut.Fill(dt);
            dv = dt.DefaultView;
            dataGridView2.DataSource = dv;
            dataGridView2.Columns["cv"].Visible = false;
            dataGridView2.Columns["fotoğraf"].Visible = false;
            button6.Visible = true;
            panel2.Visible = true;
            SqlCommand komut2 = new SqlCommand("SELECT * FROM personeller WHERE tc_no = @tc_no", baglanti);
            komut2.Parameters.AddWithValue("@tc_no", textBox6.Text);
            SqlDataReader reader = komut2.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                textBox17.Text = reader["ad"].ToString();
                textBox9.Text = reader["soyad"].ToString();
                textBox3.Text = reader["tc_no"].ToString();
                dateTimePicker4.Text = reader["dogum_tarihi"].ToString();
                comboBox4.Text = reader["gorev"].ToString();
                comboBox1.Text = reader["cinsiyet"].ToString();
                dateTimePicker2.Text = reader["tecrube"].ToString();
                reader.Close();
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Hatalı Giriş Yaptınız");
                baglanti.Close();
            }

            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            panel1.Visible = true;
            dataGridView4.AutoGenerateColumns = true;
            SqlDataAdapter komut = new SqlDataAdapter("SELECT * FROM seferler WHERE sefer_id = '" + textBox12.Text + "'", baglanti);
            SqlCommand komut2 = new SqlCommand("SELECT * FROM seferler WHERE sefer_id = @sefer_id", baglanti);
            komut2.Parameters.AddWithValue("@sefer_id", textBox12.Text);
            SqlDataReader reader = komut2.ExecuteReader();
            reader.Read();
            textBox10.Text = reader["personel_1_id"].ToString();
            textBox11.Text = reader["personel_2_id"].ToString();
            textBox8.Text = reader["personel_3_id"].ToString();
            textBox14.Text = reader["personel_4_id"].ToString();
            textBox15.Text = reader["personel_5_id"].ToString();
            textBox14.Text = reader["personel_6_id"].ToString();
            reader.Close();
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            komut.Fill(dt);
            dv = dt.DefaultView;
            dataGridView4.DataSource = dv;
            dataGridView4.Columns["fiyat"].Visible = false;
            dataGridView4.Columns["kalkis_saati"].Visible = false;
            dataGridView4.Columns["varis_saati"].Visible = false;
            dataGridView4.Columns["fiyat"].Visible = false;
            button8.Visible = true;
            baglanti.Close();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            
            label23.Visible = true;
            label40.Visible = true;
            textBox15.Visible = true;
            textBox13.Visible = true;
            tur = "Uçak";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            
            label23.Visible = false;
            label40.Visible = false;
            textBox15.Visible = false;
            textBox13.Visible = false;
            tur = "Otobüs";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            
            label23.Visible = true;
            textBox15.Visible = true;
            label40.Visible = false;
            textBox13.Visible = false;
            tur = "Tren";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut2 = new SqlCommand("UPDATE seferler SET personel_1_id = @personel1, personel_2_id = @personel2, personel_3_id = @personel3,personel_4_id = @personel4 ,personel_5_id = @personel5 ,personel_6_id = @personel6 WHERE sefer_id = @sefer_id", baglanti);
            komut2.Parameters.AddWithValue("@sefer_id", textBox12.Text.ToString());
            komut2.Parameters.AddWithValue("@personel1",textBox10.Text.ToString());
            komut2.Parameters.AddWithValue("@personel2", textBox11.Text.ToString());
            komut2.Parameters.AddWithValue("@personel3", textBox8.Text.ToString());
            komut2.Parameters.AddWithValue("@personel4", textBox14.Text.ToString());
            komut2.Parameters.AddWithValue("@personel5", textBox15.Text.ToString());
            komut2.Parameters.AddWithValue("@personel6", textBox13.Text.ToString());
            if(textBox12.Text == "" || textBox12.Text == "" || textBox12.Text == "" || textBox12.Text == "" || textBox12.Text == "" || textBox12.Text == "")
            {
                MessageBox.Show("Lütfen bütün personelleri giriniz");
                baglanti.Close();
                return;
            }
            else
            {
                komut2.ExecuteNonQuery();
                MessageBox.Show("Personeller başarıyla sefere eklendi.");
                baglanti.Close();
            }
            
        }

        private void button14_Click(object sender, EventArgs e)
        {
            
            dataGridView4.AutoGenerateColumns = true;
            SqlDataAdapter komut = new SqlDataAdapter("SELECT * FROM personeller", baglanti);
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            komut.Fill(dt);
            dv = dt.DefaultView;
            dataGridView4.DataSource = dv;
            dataGridView4.Columns["cv"].Visible = false;
            dataGridView4.Columns["fotoğraf"].Visible = false;
            dataGridView4.Columns["tecrube"].Visible = false;
            dataGridView4.Columns["dogum_tarihi"].Visible = false;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            
            dataGridView4.AutoGenerateColumns = true;
            SqlDataAdapter komut = new SqlDataAdapter("SELECT * FROM seferler", baglanti);
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            komut.Fill(dt);
            dv = dt.DefaultView;
            dataGridView4.DataSource = dv;
            dataGridView4.Columns["fiyat"].Visible = false;
            dataGridView4.Columns["kalkis_saati"].Visible = false;
            dataGridView4.Columns["varis_saati"].Visible = false;
            dataGridView4.Columns["fiyat"].Visible = false;
        }
    }
}

