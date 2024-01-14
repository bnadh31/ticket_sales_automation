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
    public partial class GirisYap : Form
    {
        public AnaForm main_page = new AnaForm();
        public Form sifreOlustur = new Form();
        Label sifrel = new Label();
        TextBox sifreb = new TextBox();
        Button butonb = new Button();

        public GirisYap()
        {
            InitializeComponent();            
            sifre_box.PasswordChar = '●';
            sifre_box.MaxLength = 20;
        }
        SqlConnection baglanti = new SqlConnection("server=172.20.10.2;database=otomasyon_veritabani;UID=SA;password=reallyStrongPwd123");
        String tc, sifre;

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sifre_box.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            tc = tc_box.Text;
            sifre = sifre_box.Text; 


            try
            {
                String sorgu = "SELECT * FROM personeller WHERE tc_no = '" + tc + "' AND sifre = '" + sifre + "'";
                SqlCommand komut = new SqlCommand("SELECT * FROM personeller WHERE tc_no = '" + tc + "' AND sifre = '" + sifre + "'",baglanti);
                SqlDataAdapter sda = new SqlDataAdapter(sorgu,baglanti);
                DataTable dtable = new DataTable();
                sda.Fill(dtable);
                SqlDataReader reader = komut.ExecuteReader();
                if (dtable.Rows.Count > 0)
                {
                    tc = tc_box.Text;
                    sifre = sifre_box.Text;
                    reader.Read();
                    main_page.personel_ad = reader["ad"].ToString();
                    main_page.personel_soyad = reader["soyad"].ToString();
                    main_page.personel_tc_no = reader["tc_no"].ToString();
                    main_page.personel_dogum_tarihi = (DateTime)reader["dogum_tarihi"];
                    main_page.personel_gorev = reader["gorev"].ToString();
                    main_page.personel_cinsiyet = reader["cinsiyet"].ToString();
                    main_page.personel_tecrube = reader["tecrube"].ToString();
                    reader.Close();
                    reader.Dispose();
                    main_page.Show();
                    this.Hide();
                    baglanti.Close();
                }
                else if(sifre_box.Text =="")
                {
                    MessageBox.Show("Lütfen şifre giriniz.");
                    baglanti.Close();
                    return;
                }
                TcDogruMu();
            }
            catch(FormatException)
            {
                MessageBox.Show("Giriş Başarısız.");
            }
            finally
            {
                baglanti.Close();
            }
                        
        }
        public bool TcDogruMu()
        {
            string kimlikno = tc_box.Text;
            kimlikno = kimlikno.Trim();
            if (kimlikno.Length != 11)
            {
                MessageBox.Show("T.C. Kimlik Numarasını Eksik Girdiniz.");
                tc_box.Focus();
                return false;
            }
            else
            {
                int[] sayilar = new int[11];
                for (int i = 0; i < kimlikno.Length; i++)
                {
                    sayilar[i] = Int32.Parse(kimlikno[i].ToString());
                }
                int toplam = 0;
                for (int i = 0; i < kimlikno.Length - 1; i++)
                {
                    toplam += sayilar[i];
                }
                if (toplam.ToString()[1].ToString() == sayilar[10].ToString() & sayilar[10] % 2 == 0)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("T.C. Kimlik No Hatalıdır.");
                    tc_box.Focus();
                    return false;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut3 = new SqlCommand("SELECT * FROM personeller WHERE tc_no = @tc_box", baglanti);
            komut3.Parameters.AddWithValue("@tc_box", tc_box.Text);
            SqlDataReader reader3 = komut3.ExecuteReader();
            if(!reader3.HasRows)
            {
                MessageBox.Show("Personel Bulunamadı.");
                baglanti.Close();
                reader3.Close();
                return;
            }
            if (tc_box.Text == "")
            {
                MessageBox.Show("Lütfen TC nizi ilgili kutucuğa giriniz.");
                return;
            }
            
            sifreOlustur.Width = 379;
            sifreOlustur.Height = 330;
            sifreOlustur.BackColor = Color.Blue;
            Label sifre = new Label();
            TextBox idb = new TextBox();
            Button buton = new Button();
            sifre.Location = new Point(114, 52);
            idb.Location = new Point(114, 72);
            idb.Width = 145;
            buton.Height = 45;
            buton.Location = new Point(114, 112);
            buton.Text = "Giriş";
            buton.Font = new Font("Microsoft YaHei", 12f, FontStyle.Bold);
            buton.BackColor = Color.White;
            buton.ForeColor = Color.Black;
            sifre.Text = "Personel No";
            sifre.Size = new Size(20, 20);
            sifre.Font = new Font("Microsoft YaHei", 10.125f, FontStyle.Bold);
            sifre.ForeColor = Color.White;
            sifre.Width = 140;
            sifreOlustur.Controls.Add(sifre);
            sifreOlustur.Controls.Add(idb);
            sifreOlustur.Controls.Add(buton);
            reader3.Close();
            baglanti.Close();
            
            buton.Click += (s, ev) =>
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("SELECT * FROM personeller WHERE personal_id = @idb",baglanti);
                komut.Parameters.AddWithValue("@idb", idb.Text);
                SqlDataReader reader = komut.ExecuteReader();
                if(!reader.HasRows)
                {
                    MessageBox.Show("Personel numarası geçersiz.");
                    baglanti.Close();
                    reader.Close();
                    return;
                }
                else
                {
                    idb.Visible = false;
                    sifre.Visible = false;
                    buton.Visible = false;
                    sifrel.Location = new Point(114, 52);
                    sifreb.Location = new Point(114, 72);
                    sifreb.Width = 145;
                    butonb.Height = 45;
                    butonb.Location = new Point(114, 132);
                    butonb.Text = "Ata";
                    butonb.Font = new Font("Microsoft YaHei", 12f, FontStyle.Bold);
                    butonb.BackColor = Color.White;
                    butonb.ForeColor = Color.Black;
                    sifrel.Text = "Şifrenizi Giriniz.";
                    sifrel.Size = new Size(20, 20);
                    sifrel.Font = new Font("Microsoft YaHei", 10.125f, FontStyle.Bold);
                    sifrel.ForeColor = Color.White;
                    sifrel.Width = 140;
                    sifreb.PasswordChar = '●';
                    sifreb.MaxLength = 20;
                    sifreOlustur.Controls.Add(sifrel);
                    sifreOlustur.Controls.Add(sifreb);
                    sifreOlustur.Controls.Add(butonb);
                    reader.Close();
                    butonb.Click += Butonb_Click;
                }
            };
            sifreOlustur.ShowDialog();
        }

        private void Butonb_Click(object sender, EventArgs e)
        {
            SqlCommand komut1 = new SqlCommand("UPDATE personeller set sifre = @sifre WHERE tc_no = @tc_box", baglanti);
            komut1.Parameters.AddWithValue("@tc_box",tc_box.Text.ToString());
            komut1.Parameters.AddWithValue("@sifre", sifreb.Text.ToString());
            komut1.ExecuteNonQuery();
            MessageBox.Show("Şifre Başarıyla Kaydedildi.");
            sifreOlustur.Close();
            baglanti.Close();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            tc = tc_box.Text;
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM personeller WHERE tc_no = '" + tc + "'", baglanti);
            SqlCommand komut = new SqlCommand("SELECT * FROM personeller WHERE tc_no = '" + tc + "'", baglanti);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            SqlDataReader reader = komut.ExecuteReader();
            reader.Read();
            if (tc_box.Text == "")
            {
                MessageBox.Show("Lütfen TC nizi ilgili kutucuğa giriniz.");
                baglanti.Close();
                reader.Close();
                return;
            }
            else if(!reader.IsDBNull(reader.GetOrdinal("sifre")))
            {
                MessageBox.Show("Zaten bir şifreye sahipsiniz.");
                baglanti.Close();
                reader.Close();
                return;
            }
            else if (reader["sifre"] == DBNull.Value && reader["gorev"].ToString() == "Supervisor" || reader["gorev"].ToString() == "Gise Personeli")
            {
                reader.Close();
                Form sifreOlustur = new Form();
                sifreOlustur.Width = 379;
                sifreOlustur.Height = 330;
                sifreOlustur.BackColor = Color.Blue;
                Label sifre = new Label();
                TextBox sifreb = new TextBox();
                Button buton = new Button();
                sifre.Location = new Point(114, 52);
                sifreb.Location = new Point(114, 72);
                sifreb.Width = 145;
                buton.Height = 45;
                buton.Location = new Point(114, 112);
                buton.Text = "Ata";
                buton.Font = new Font("Microsoft YaHei", 12f, FontStyle.Bold);
                buton.BackColor = Color.White;
                buton.ForeColor = Color.Black;
                sifre.Text = "Şifrenizi Giriniz.";
                sifre.Size = new Size(20, 20);
                sifre.Font = new Font("Microsoft YaHei", 10.125f, FontStyle.Bold);
                sifre.ForeColor = Color.White;
                sifre.Width = 140;
                sifreb.PasswordChar = '●';
                sifreb.MaxLength = 20;
                sifreOlustur.Controls.Add(sifre);
                sifreOlustur.Controls.Add(sifreb);
                sifreOlustur.Controls.Add(buton);
                buton.Click += (s, ev) =>
                {
                    SqlCommand komut1 = new SqlCommand("UPDATE personeller set sifre = @sifre", baglanti);
                    komut1.Parameters.AddWithValue("@sifre", sifreb.Text);
                    komut1.ExecuteNonQuery();
                    MessageBox.Show("Şifre Başarıyla Kaydedildi.");
                    sifreOlustur.Close();
                };
                sifreOlustur.ShowDialog();


            }
            else if (reader["gorev"].ToString() != "Supervisor" && reader["gorev"].ToString() != "Gise Personeli")
            {
                MessageBox.Show("Bu sisteme giriş yetkiniz bulunmamaktadır.");
            }
            else if (reader["sifre"] != DBNull.Value)
            {
                MessageBox.Show("Sisteme ait şifreniz zaten bulunmaktadır.");
            }
            else if (dt.Rows.Count < 0)
            {
                MessageBox.Show("Personel kaydınız bulunmamaktadır.");
            }
            
            
            baglanti.Close();
        }
    }
}
