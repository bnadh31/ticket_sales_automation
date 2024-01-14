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

namespace Airline_management_project
{
    public partial class SeferIslemleri : Form
    {
        SqlConnection baglanti = new SqlConnection("server=172.20.10.2;database=otomasyon_veritabani;UID=SA;password=reallyStrongPwd123");
        object deger;
        public SeferIslemleri()
        {
            InitializeComponent();
        }

        private void SeferIslemleri_Load(object sender, EventArgs e)
        {
            SqlDataAdapter komut = new SqlDataAdapter("SELECT * FROM seferler",baglanti);
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            komut.Fill(dt);
            dv = dt.DefaultView;
            dataGridView3.DataSource = dv;
            dataGridView3.Columns[10].Visible = false;
            dataGridView3.Columns[11].Visible = false;
            dataGridView3.Columns[12].Visible = false;
            dataGridView3.Columns[13].Visible = false;
            dataGridView3.Columns[14].Visible = false;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            dateTimePicker2.Format = DateTimePickerFormat.Time;
            dateTimePicker2.ShowUpDown = true;
            dateTimePicker3.Format = DateTimePickerFormat.Time;
            dateTimePicker3.ShowUpDown = true;
            dateTimePicker4.Format = DateTimePickerFormat.Custom;
            dateTimePicker4.CustomFormat = "yyyy-MM-dd";
            dateTimePicker6.Format = DateTimePickerFormat.Custom;
            dateTimePicker6.CustomFormat = "yyyy-MM-dd";
            dateTimePicker8.Format = DateTimePickerFormat.Custom;
            dateTimePicker8.CustomFormat = "yyyy-MM-dd";
            panel2.Visible = false;
            button7.Visible = false;
        }
        string tur;
        private void button1_Click(object sender, EventArgs e)
        {
            
            baglanti.Open();
            try
            {
                if (radioButton3.Checked == true)
                {
                    tur = "Uçak";
                }
                else if (radioButton4.Checked == true)
                {
                    tur = "Otobüs";
                }
                else if (radioButton5.Checked)
                {
                    tur = "Tren";
                }

                SqlCommand komut = new SqlCommand("INSERT INTO seferler(arac_id, ulasim_turu, kalkis_yeri, kalkis_tarihi, kalkis_saati, varis_yeri, " +
                    "varis_tarihi, varis_saati, fiyat) VALUES(@arac_id,@turu,@kalkis_yeri,@kalkis_tarihi,@kalkis_saati,@varis_yeri,@varis_tarihi,@varis_saati,@fiyat)",baglanti);
                komut.Parameters.AddWithValue("@arac_id", textBox1.Text.ToString());
                komut.Parameters.AddWithValue("@kalkis_yeri", textBox2.Text);
                komut.Parameters.AddWithValue("@kalkis_tarihi", dateTimePicker1.Value.ToString());
                komut.Parameters.AddWithValue("@kalkis_saati", dateTimePicker2.Value.ToString());
                komut.Parameters.AddWithValue("@varis_yeri", textBox4.Text);
                komut.Parameters.AddWithValue("@varis_tarihi", dateTimePicker3.Value.ToString());
                komut.Parameters.AddWithValue("@varis_saati", dateTimePicker4.Value.ToString());
                komut.Parameters.AddWithValue("@fiyat", textBox3.Text.ToString());
                komut.Parameters.AddWithValue("@turu", tur.ToString());
                SqlCommand komut2 = new SqlCommand("INSERT INTO biletler('Boş', '0', '', musteri_id, odeme_tarihi, odeme_turu, bilet_turu) " +
                   "VALUES(@koltuk_no, @fiyat, @sefer_id, @musteri_id, @odeme_tarihi, @odeme_turu, @bilet_turu)", baglanti);
                komut.ExecuteNonQuery();
                MessageBox.Show("Sefer Başarıyla Eklendi.");
                baglanti.Close();
               
            }
            catch(Exception sql)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı. Lütfen tekrar deneyiniz.\n\n" + sql);
                baglanti.Close();
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlDataAdapter komut = new SqlDataAdapter("SELECT * FROM seferler WHERE sefer_id = '"+textBox5.Text+"'", baglanti);
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            komut.Fill(dt);
            dv = dt.DefaultView;
            dataGridView1.DataSource = dv;
            dataGridView1.Columns["personel_1_id"].Visible = false;
            dataGridView1.Columns["personel_2_id"].Visible = false;
            dataGridView1.Columns["personel_3_id"].Visible = false;
            dataGridView1.Columns["personel_4_id"].Visible = false;
            dataGridView1.Columns["personel_5_id"].Visible = false;
            dataGridView1.Columns["personel_6_id"].Visible = false;
            button4.Visible = true;
            baglanti.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("DELETE FROM seferler WHERE sefer_id = @sefer_id", baglanti);
                SqlCommand komut2 = new SqlCommand("SELECT biletler.sefer_id FROM biletler,seferler WHERE seferler.sefer_id = biletler.sefer_id AND seferler.sefer_id = @sefer_id",baglanti);
                komut2.Parameters.AddWithValue("@sefer_id",deger.ToString());
                komut.Parameters.AddWithValue("@sefer_id", deger.ToString());
                using (SqlDataReader reader = komut2.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        MessageBox.Show("Bilet alınmış bir sefer silinemez");
                        baglanti.Close();
                        return;
                    }
                    reader.Close(); 
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Sefer başarıyla silindi.");
                    baglanti.Close();
                }
            }
            catch(Exception sql)
            {
                MessageBox.Show("Sefer silinemedi. Hata kodu : \n\n" + sql);
                baglanti.Close();

            }
            
        }
        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
                DataGridViewRow seciliSatir = dataGridView1.Rows[e.RowIndex];
                deger = seciliSatir.Cells[0].Value;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlDataAdapter komut = new SqlDataAdapter("SELECT * FROM seferler WHERE sefer_id = '" + textBox6.Text + "'", baglanti);
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            komut.Fill(dt);
            dv = dt.DefaultView;
            dataGridView2.DataSource = dv;
            button6.Visible = true;
            panel2.Visible = true;
            SqlCommand komut2 = new SqlCommand("SELECT * FROM seferler WHERE sefer_id = @sefer_id", baglanti);
            komut2.Parameters.AddWithValue("@sefer_id", textBox6.Text);
            SqlDataReader reader = komut2.ExecuteReader();
            reader.Read();

            textBox8.Text = reader["arac_id"].ToString();
            comboBox1.Text = reader["ulasim_turu"].ToString();
            textBox10.Text = reader["kalkis_yeri"].ToString();
            dateTimePicker8.Text = reader["kalkis_tarihi"].ToString();
            dateTimePicker7.Text = reader["kalkis_saati"].ToString();
            textBox9.Text = reader["varis_yeri"].ToString();
            dateTimePicker6.Text = reader["varis_tarihi"].ToString();
            dateTimePicker5.Text = reader["varis_saati"].ToString();
            textBox7.Text = reader["fiyat"].ToString();
            tur = reader["ulasim_turu"].ToString();
            reader.Close();
            
            baglanti.Close();
            
            
        }

        private void button6_Click(object sender, EventArgs e)
        {

            baglanti.Open();
            try
            {
                SqlCommand komut = new SqlCommand("UPDATE seferler SET arac_id = @arac_id ,ulasim_turu = @turu, kalkis_yeri = @kalkis_yeri, " +
                    "kalkis_tarihi = @kalkis_tarihi, kalkis_saati = @kalkis_saati, " +
                    "varis_yeri = @varis_yeri, varis_tarihi = @varis_tarihi, " +
                    "varis_saati = @varis_saati, fiyat = @fiyat WHERE sefer_id = @sefer_id", baglanti);

                // SqlCommand oluşturma
                komut.Parameters.AddWithValue("@arac_id", textBox8.Text.ToString());
                komut.Parameters.AddWithValue("@sefer_id", textBox6.Text.ToString());
                komut.Parameters.AddWithValue("@kalkis_yeri", textBox10.Text.ToString());
                komut.Parameters.AddWithValue("@kalkis_tarihi", dateTimePicker8.Value);
                komut.Parameters.AddWithValue("@kalkis_saati", dateTimePicker7.Value);
                komut.Parameters.AddWithValue("@varis_yeri", textBox9.Text.ToString());
                komut.Parameters.AddWithValue("@varis_tarihi", dateTimePicker6.Value);
                komut.Parameters.AddWithValue("@varis_saati", dateTimePicker5.Value);
                komut.Parameters.AddWithValue("@fiyat", textBox7.Text.ToString());
                komut.Parameters.AddWithValue("@turu", tur.ToString());
                int kontrol = komut.ExecuteNonQuery();
                if (kontrol>0)
                {
                    MessageBox.Show("Güncelleme işlemi başarılı !");

                }
                else
                {
                    MessageBox.Show("Güncelleme işlemi başarısız oldu.");
                }
                baglanti.Close();
            }
            catch(Exception sql)
            {
                MessageBox.Show("Güncelleme hatası ! Hata kodu : \n\n"+sql);
                baglanti.Close();
            }
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            label1.Text = "Araç Listesi";
            SqlDataAdapter komut = new SqlDataAdapter("SELECT * FROM ulasim_araclari", baglanti);
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            komut.Fill(dt);
            dv = dt.DefaultView;
            dataGridView3.DataSource = dv;
            button7.Visible = false;
            button8.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            label1.Text = "Sefer Listesi";
            SqlDataAdapter komut = new SqlDataAdapter("SELECT * FROM seferler", baglanti);
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            komut.Fill(dt);
            dv = dt.DefaultView;
            dataGridView3.DataSource = dt;
            button8.Visible = false;
            button7.Visible = true;
        }
    }
}
