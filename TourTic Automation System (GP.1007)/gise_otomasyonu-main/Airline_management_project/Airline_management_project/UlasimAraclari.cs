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
    public partial class UlasimAraclari : Form
    {
        public UlasimAraclari()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("server=172.20.10.2;database=otomasyon_veritabani;UID=SA;password=reallyStrongPwd123");
        string tur;
        object deger;
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
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("Lütfen bütün bilgileri doldurunuz.");
                    baglanti.Close();
                }
                else
                {
                    SqlCommand komut = new SqlCommand("INSERT INTO ulasim_araclari(arac_modelli,ulasim_tur,koltuk_sayisi) VALUES(@arac_modeli,@ulasim_tur,@koltuk_sayisi)", baglanti);
                    komut.Parameters.AddWithValue("@arac_modeli", textBox1.Text.ToString());
                    komut.Parameters.AddWithValue("@koltuk_Sayisi", textBox2.Text);
                    komut.Parameters.AddWithValue("@ulasim_tur", tur.ToString());
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Araç Başarıyla Eklendi.");
                    baglanti.Close();
                }
            }
            catch (Exception sql)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı. Lütfen tekrar deneyiniz.\n\n" + sql);
                baglanti.Close();
            }
        }
        private void UlasimAraclari_Load(object sender, EventArgs e)
        {
            SqlDataAdapter komut = new SqlDataAdapter("SELECT * FROM ulasim_araclari", baglanti);
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            komut.Fill(dt);
            dv = dt.DefaultView;
            dataGridView3.DataSource = dv;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlDataAdapter komut = new SqlDataAdapter("SELECT * FROM ulasim_araclari WHERE arac_id = '" + textBox5.Text + "'", baglanti);
            SqlCommand komut2 = new SqlCommand("SELECT seferler.arac_id FROM ulasim_araclari,seferler WHERE seferler.arac_id = ulasim_araclari.arac_id AND seferler.arac_id = @arac_id", baglanti);
            komut2.Parameters.AddWithValue("@arac_id", textBox5.Text);
            SqlDataReader reader = komut2.ExecuteReader();
            if (reader.HasRows)
            {
                MessageBox.Show("Bir seferde bulunan araç silinemez.");
                baglanti.Close();
                reader.Close();
                return;
            }
            else
            {
                reader.Close();
                DataTable dt = new DataTable();
                DataView dv = new DataView();
                komut.Fill(dt);
                dv = dt.DefaultView;
                dataGridView1.DataSource = dv;
                baglanti.Close();
                button4.Visible = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                if(dataGridView1.CurrentRow.Cells[0].Value.ToString() == "" || (dataGridView1.CurrentRow.Cells[0].Value == null))
                {
                    MessageBox.Show("Lütfen satır seçiniz.");
                    baglanti.Close();
                    return;

                }
                SqlCommand komut = new SqlCommand("DELETE FROM ulasim_araclari WHERE arac_id = @arac_id", baglanti);
                komut.Parameters.AddWithValue("@arac_id", dataGridView1.CurrentRow.Cells[0].Value.ToString());
                komut.ExecuteNonQuery();
                MessageBox.Show("Araç başarıyla silindi.");
                baglanti.Close();
            }
            catch (Exception sql)
            {
                MessageBox.Show("Araç silinemedi. Hata kodu : \n\n" + sql);
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
            SqlDataAdapter komut = new SqlDataAdapter("SELECT * FROM ulasim_araclari WHERE arac_id = '" + textBox6.Text + "'", baglanti);
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            komut.Fill(dt);
            dv = dt.DefaultView;
            dataGridView2.DataSource = dv;
            button6.Visible = true;
            panel2.Visible = true;
            SqlCommand komut2 = new SqlCommand("SELECT * FROM ulasim_araclari WHERE arac_id = @arac_id", baglanti);
            komut2.Parameters.AddWithValue("@arac_id", textBox6.Text);
            SqlDataReader reader = komut2.ExecuteReader();
            reader.Read();
            textBox4.Text = reader["koltuk_sayisi"].ToString();
            comboBox1.Text = reader["ulasim_tur"].ToString();
            textBox3.Text = reader["arac_modelli"].ToString();
            tur = reader["ulasim_tur"].ToString();
            reader.Close();
            baglanti.Close();
        }
        private void button6_Click(object sender, EventArgs e)
        {

            baglanti.Open();
            try
            {
                SqlCommand komut = new SqlCommand("UPDATE ulasim_araclari SET arac_modelli = @arac_model, ulasim_tur = @turu, koltuk_sayisi = @koltuk_sayisi WHERE arac_id = @arac_id", baglanti);
                komut.Parameters.AddWithValue("@arac_id", textBox6.Text.ToString());
                komut.Parameters.AddWithValue("@arac_model", textBox3.Text.ToString());
                komut.Parameters.AddWithValue("@koltuk_sayisi", textBox4.Text.ToString());
                komut.Parameters.AddWithValue("@turu", tur.ToString());
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
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            textBox3.Text = "";
            textBox4.Text = "";
            comboBox1.Text = "";
            textBox6.Text = "";
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
