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
    public partial class TrenSatis : Form
    {
        SqlConnection baglanti = new SqlConnection("server=172.20.10.2;database=otomasyon_veritabani;UID=SA;password=reallyStrongPwd123");
        int musteri_id;
        public TrenSatis()
        {
            InitializeComponent();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Lütfen Bilgileri Eksiksiz Doldurunuz.");
                }
                else
                {
                    SqlCommand komut = new SqlCommand("SELECT * FROM musteri_bilgileri WHERE tc_pas_no = @tc_pas", baglanti);
                    komut.Parameters.AddWithValue("@tc_pas", textBox1.Text);
                    SqlDataReader reader = komut.ExecuteReader();
                    reader.Read();
                    if (!reader.HasRows)
                    {
                        MessageBox.Show("T.C kimlik no bulunamadı. Lütfen önce kayıt yapınız.");
                    }
                    musteri_id = int.Parse(reader["musteri_id"].ToString());
                    textBox2.Text = reader["ad"].ToString();
                    textBox3.Text = reader["soyad"].ToString();
                    textBox4.Text = reader["telefon_no"].ToString();
                    comboBox5.Text = reader["uyruk"].ToString();
                    panel3.Visible = true;
                    label19.Visible = true;
                    textBox2.Enabled = false;
                    textBox3.Enabled = false;
                    if (comboBox5.Text == "Türkiye")
                    {
                        checkBox1.Checked = false;
                    }
                    else
                    {
                        checkBox1.Checked = true;
                    }
                    comboBox5.Enabled = false;
                    comboBox5.Visible = true;
                    button6.Visible = true;
                    reader.Close();
                    baglanti.Close();
                }

            }
            catch (InvalidOperationException an)
            {
                MessageBox.Show("TC girişinde bir hata tespit edildi.\n\n" + an);
                baglanti.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (TcDogruMu(t: true))
            {
                baglanti.Open();
                try
                {
                    SqlCommand komut2 = new SqlCommand("SELECT * FROM musteri_bilgileri WHERE tc_pas_no = @tc", baglanti);
                    SqlCommand komut = new SqlCommand("INSERT INTO musteri_bilgileri (tc_pas_no, ad, soyad, telefon_no, uyruk, tekerlekli_sandalye) VALUES (@tc ,@ad,@soyad,@tel_no,@uyruk,@engel)", baglanti);
                    komut.Parameters.AddWithValue("@tc", textBox1.Text);
                    komut2.Parameters.AddWithValue("@tc", textBox1.Text);
                    komut.Parameters.AddWithValue("@ad", textBox2.Text);
                    komut.Parameters.AddWithValue("@soyad", textBox3.Text);
                    komut.Parameters.AddWithValue("@tel_no", textBox4.Text);
                    SqlDataReader reader = komut2.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        MessageBox.Show("Bu TC/Pasaport no zaten kayıtlı.");
                        baglanti.Close();
                    }
                    else
                    {
                        reader.Close();
                        if (checkBox1.Checked == false)
                        {
                            comboBox5.Text = "Türkiye";
                        }
                        if (checkBox2.Checked == true)
                        {
                            komut.Parameters.AddWithValue("@engel", 1);
                        }

                        else
                        {
                            komut.Parameters.AddWithValue("@engel", 0);

                        }
                        komut.Parameters.AddWithValue("@uyruk", comboBox5.Text);

                        int rowsAffected = komut.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Bilgiler Başarıyla Kaydedildi.");
                        }
                        else
                        {
                            MessageBox.Show("Bilgiler Kaydedilemedi");

                        }
                        baglanti.Close();
                    }
                }

                catch (SqlException a)
                {
                    MessageBox.Show("Baglanti Hatasi : " + a);
                    baglanti.Close();
                }
            }
        }
        public bool TcDogruMu(bool t = true)
        {

            string kimlikno = textBox1.Text;

            kimlikno = kimlikno.Trim();
            if (kimlikno.Length != 11)
            {
                MessageBox.Show("T.C. Kimlik Numarasını Eksik Girdiniz.");
                textBox1.Focus();
                return false;

            }
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

                return t;
            }
            else
            {
                MessageBox.Show("T.C. Kimlik No Hatalıdır.");

                textBox1.Focus();
                return false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //TC Sorgulama Bölümü
            SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM musteri_bilgileri", baglanti);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            dataGridView3.DataSource = dt;
            DataView dv = dt.DefaultView;
            DataView dv2 = dt.DefaultView;
            dv.RowFilter = "tc_pas_no = '" + textBox5.Text + "'";
            dataGridView3.DataSource = dv;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                label27.Text = "Evet";
            }
            else
            {
                label27.Text = "Hayır";
            }
            label22.Text = textBox1.Text;
            label26.Text = textBox2.Text;
            label48.Text = textBox3.Text;
            tabControl1.TabPages[1].Enabled = true;
            tabControl1.SelectedIndex = 1;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                comboBox5.Visible = true;
                label19.Visible = true;
            }
            else
            {
                comboBox5.Visible = false;
                label19.Visible = false;
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex == 0)
            {
                button10.Visible = false;
                button1.Visible = true;
                panel3.Visible = true;
                comboBox5.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                checkBox1.Checked = false;
                checkBox2.Checked = false;
            }
            if (comboBox6.SelectedIndex == 1)
            {
                button10.Visible = true;
                button1.Visible = false;
                panel3.Visible = false;
            }
        }

        private void TrenSatis_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            label19.Visible = false;
            comboBox5.Visible = false;
            tabControl1.SelectedIndex = 0;
            panel3.Visible = false;
            yerler_ComboBox();
            radioButton1.Checked = true;
            gidis_tarih.MinDate = DateTime.Today;
            donus_tarih.MinDate = DateTime.Today;
            baglanti.Close();
            button6.Visible = false;
            panel1.Visible = false;
            button13.Visible = true;
            tabControl1.TabPages[3].Enabled = false;
            tabControl1.TabPages[2].Enabled = false;
            tabControl1.TabPages[1].Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label6.Visible = true;
            donus_tarih.Visible = true;
            dataGridView2.Visible = true;
            label17.Visible = true;
            label18.Visible = true;
            panel1.Visible = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label6.Visible = false;
            donus_tarih.Visible = false;
            dataGridView2.Visible = false;
            label17.Visible = false;
            label18.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            seferleri_getir();
            baglanti.Close();
        }
        void seferleri_getir()
        {
            //Seferleri görüntüleme bölümü.

            SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM seferler WHERE ulasim_turu = 'Tren'", baglanti);
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            ad.Fill(dt);
            ad.Fill(dt2);
            dataGridView1.DataSource = dt;
            dataGridView2.DataSource = dt;
            DataView dv = dt.DefaultView;
            DataView dv2 = dt2.DefaultView;
            dv.RowFilter = "kalkis_yeri LIKE '" + comboBox1.Text + "' AND varis_yeri LIKE '" + comboBox2.Text + "' AND ulasim_turu LIKE 'Tren' AND kalkis_tarihi = '" + gidis_tarih.Text + "'";
            if (radioButton2.Checked == true)
            {
                dv2.RowFilter = "kalkis_yeri LIKE '" + comboBox2.Text + "' AND varis_yeri LIKE '" + comboBox1.Text + "' AND ulasim_turu LIKE 'Tren' AND kalkis_tarihi = '" + donus_tarih.Text + "'";
                dataGridView2.DataSource = dv2;
                dataGridView1.DataSource = dv;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[13].Visible = false;
                dataGridView1.Columns[14].Visible = false;
                dataGridView1.Columns[15].Visible = false;
                dataGridView2.Columns[2].Visible = false;
                dataGridView2.Columns[10].Visible = false;
                dataGridView2.Columns[11].Visible = false;
                dataGridView2.Columns[12].Visible = false;
                dataGridView2.Columns[13].Visible = false;
                dataGridView2.Columns[14].Visible = false;
                dataGridView2.Columns[15].Visible = false;
                label8.Text = comboBox1.Text + " - ";
                label9.Text = comboBox2.Text;
                label18.Text = comboBox2.Text + " - ";
                label17.Text = comboBox1.Text;
            }
            else
            {
                dataGridView1.DataSource = dv;
                dataGridView1.Visible = true;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[13].Visible = false;
                dataGridView1.Columns[14].Visible = false;
                dataGridView1.Columns[15].Visible = false;
                label8.Text = comboBox1.Text + " - ";
                label9.Text = comboBox2.Text;
                label18.Text = comboBox2.Text + " - ";
                label17.Text = comboBox1.Text;
            }
        }
        void yerler_ComboBox()
        {
            //Combobox doldurma bölümü.
            string query2 = "SELECT seferler.kalkis_yeri FROM seferler WHERE ulasim_turu = 'Tren'";
            SqlCommand komut2 = new SqlCommand(query2, baglanti);
            SqlDataReader reader = komut2.ExecuteReader();
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            while (reader.Read())
            {
                list.Add(reader["kalkis_yeri"].ToString());
            }
            reader.Close();
            list = list.Distinct().ToList();
            list.Sort();
            comboBox1.DataSource = list;
            string query = "SELECT seferler.varis_yeri FROM seferler WHERE ulasim_turu = 'Tren'";
            SqlCommand komut = new SqlCommand(query, baglanti);
            SqlDataReader reader1 = komut.ExecuteReader();
            while (reader1.Read())
            {
                list2.Add(reader1["varis_yeri"].ToString());
            }
            reader1.Close();
            list2 = list2.Distinct().ToList();
            list2.Sort();
            comboBox2.DataSource = list2;
        }
        float fiyat1, fiyat2;
        private void button4_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            try
            {

                if (gidis_tarih.Value == DateTimePicker.MinimumDateTime || gidis_tarih.Value == DateTimePicker.MinimumDateTime)
                {
                    MessageBox.Show("Lütfen tarih seçimi yapınız.");
                    baglanti.Close();
                }
                else
                {
                    if (dataGridView1 != null && dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Cells[0].Value != null && radioButton1.Checked == true)
                    {
                        SqlCommand komut = new SqlCommand("SELECT biletler.musteri_id FROM biletler WHERE sefer_id = @sefer_id", baglanti);
                        komut.Parameters.AddWithValue("@sefer_id", dataGridView1.CurrentRow.Cells[0].Value.ToString());
                        SqlDataReader reader = komut.ExecuteReader();
                        if (reader.Read())
                        {
                            if (int.Parse(reader["musteri_id"].ToString()) == musteri_id)
                            {
                                MessageBox.Show("Aynı uçaktan sadece bir bilet alınabilir.");

                                baglanti.Close();
                                return;
                            }
                            tabControl1.TabPages[2].Enabled = true;
                            tabControl1.SelectedIndex = 2;
                            label31.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                            label32.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                            label29.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                            fiyat1 = float.Parse(dataGridView1.CurrentRow.Cells[9].Value.ToString());
                            fiyat2 = float.Parse(dataGridView2.CurrentRow.Cells[9].Value.ToString());
                            label46.Text = (fiyat1 + fiyat2).ToString() + " TL";
                            baglanti.Close();
                            panel4.Visible = true;
                            button_ile(label31.Text);
                            if (radioButton2.Checked == true)
                            {
                                panel4.Visible = true;
                                label46.Text = (fiyat1 + fiyat2).ToString() + " TL";
                                label37.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                                label38.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
                                label35.Text = dataGridView2.CurrentRow.Cells[5].Value.ToString();
                                panel6.Visible = true;
                                panel5.Visible = true;
                                button_ile2();
                            }
                            else
                            {
                                panel6.Visible = false;
                                label46.Text = fiyat1.ToString() + " TL";
                            }
                            baglanti.Close();
                        }
                        else
                        {
                            tabControl1.TabPages[2].Enabled = true;
                            tabControl1.SelectedIndex = 2;
                            label31.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                            label32.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                            label29.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                            fiyat1 = float.Parse(dataGridView1.CurrentRow.Cells[9].Value.ToString());
                            fiyat2 = float.Parse(dataGridView2.CurrentRow.Cells[9].Value.ToString());
                            label46.Text = (fiyat1 + fiyat2).ToString() + " TL";
                            baglanti.Close();
                            button_ile(label31.Text);
                            if (radioButton2.Checked == true)
                            {
                                label46.Text = (fiyat1 + fiyat2).ToString() + " TL";
                                label37.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                                label38.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
                                label35.Text = dataGridView2.CurrentRow.Cells[5].Value.ToString();
                                panel6.Visible = true;
                                button_ile2();
                            }
                            else
                            {
                                panel6.Visible = false;
                                label46.Text = fiyat1.ToString() + " TL";
                            }
                            baglanti.Close();

                        }
                    }
                    else if (dataGridView2 != null && dataGridView2.CurrentRow != null && dataGridView2.CurrentRow.Cells[0].Value != null && radioButton2.Checked == true)

                    {
                        SqlCommand komut = new SqlCommand("SELECT biletler.musteri_id FROM biletler WHERE sefer_id = @sefer_id", baglanti);
                        komut.Parameters.AddWithValue("@sefer_id", dataGridView1.CurrentRow.Cells[0].Value.ToString());
                        komut.Parameters.AddWithValue("@sefer_id2", dataGridView2.CurrentRow.Cells[0].Value.ToString());
                        SqlDataReader reader = komut.ExecuteReader();
                        if (reader.Read())
                        {
                            if (int.Parse(reader["musteri_id"].ToString()) == musteri_id)
                            {
                                MessageBox.Show("Aynı uçaktan sadece bir bilet alınabilir.");
                                baglanti.Close();
                                return;
                            }
                            tabControl1.TabPages[2].Enabled = true;
                            tabControl1.SelectedIndex = 2;
                            label31.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                            label32.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                            label29.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                            fiyat1 = float.Parse(dataGridView1.CurrentRow.Cells[9].Value.ToString());
                            fiyat2 = float.Parse(dataGridView2.CurrentRow.Cells[9].Value.ToString());
                            label46.Text = (fiyat1 + fiyat2).ToString() + " TL";
                            baglanti.Close();
                            button_ile(label31.Text);
                            if (radioButton2.Checked == true)
                            {
                                label46.Text = (fiyat1 + fiyat2).ToString() + " TL";
                                label37.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                                label38.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
                                label35.Text = dataGridView2.CurrentRow.Cells[5].Value.ToString();
                                panel6.Visible = true;
                                button_ile2();
                            }
                            else
                            {
                                panel6.Visible = false;
                                label46.Text = fiyat1.ToString() + " TL";
                            }
                            baglanti.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Lütfen sefer seçimi yapnız.");
                        baglanti.Close();
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("İlerleme hatası. Hata Kodu : \n\n" + ex);
            }
        }
        void button_ile(string ucus_no)
        {
            int koltukNo = 1;
            int koltukHarf = 0;
            List<string> koltuk_numaralari = new List<string>();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT ulasim_araclari.koltuk_sayisi FROM ulasim_araclari, seferler WHERE seferler.arac_id = ulasim_araclari.arac_id AND seferler.sefer_id = @sefer_id", baglanti);
            komut.Parameters.AddWithValue("@sefer_id", ucus_no);
            SqlDataReader reader = komut.ExecuteReader();
            reader.Read();
            int koltuk_sayisi = int.Parse(reader["koltuk_sayisi"].ToString());
            reader.Close();
            int taraf_koltuk_sayisi = koltuk_sayisi / 4;
            
            char[] koltukHarfd = { 'A', 'B', 'C', 'D', 'E', 'F' };
            SqlCommand komut1 = new SqlCommand("SELECT biletler.koltuk_no FROM biletler WHERE sefer_id = @sefer_id", baglanti);
            komut1.Parameters.AddWithValue("@sefer_id", ucus_no);
            koltuk_numaralari.Clear();
            using (SqlDataReader reader2 = komut1.ExecuteReader())
            {
                while (reader2.Read())
                {
                    koltuk_numaralari.Add(reader2["koltuk_no"].ToString());
                }
                for (int i = 0; i < taraf_koltuk_sayisi; i++)
                {
                    for (int j = 0; j <= 4; j++)
                    {
                        if (koltukHarf >= 4)
                        {
                            koltukHarf = 0;
                        }
                        if (j == 2)
                        {
                            continue;
                        }
                        Button koltuk = new Button();
                        koltuk.Height = koltuk.Width = 35;
                        koltuk.Top = 30 + (i * 40);
                        koltuk.Left = 5 + (j * 40);
                        koltuk.Text = koltukHarfd[koltukHarf].ToString() + koltukNo.ToString();
                        if (koltuk_numaralari.Contains(koltuk.Text))
                        {
                            koltuk.Font = new Font(koltuk.Font.FontFamily, 8, FontStyle.Regular);
                            koltuk.BackColor = Color.Red;
                            koltuk.Enabled = false;
                        }
                        else
                        {
                            koltuk.Font = new Font(koltuk.Font.FontFamily, 8, FontStyle.Regular);
                            koltuk.BackColor = Color.Green;
                            koltuk.Enabled = true;
                            koltuk.Click += Koltuk_Click; ;
                        }
                        panel4.Controls.Add(koltuk);
                        koltukHarf++;
                    }
                    koltukNo++;
                }
                reader2.Close();
                baglanti.Close();
            }
        }
        Button tiklanan;

        Button sonTiklanan = null;

        private void Koltuk_Click(object sender, EventArgs e)
        {
            tiklanan = sender as Button;
            label16.Text = tiklanan.Text;
            if (sonTiklanan != null && sonTiklanan != tiklanan)
            {
                sonTiklanan.BackColor = Color.Green;
            }
            tiklanan.BackColor = Color.Yellow;
            sonTiklanan = tiklanan;
            label50.Text = label16.Text;
        }

        void button_ile2()
        {
            int koltukNo = 1;
            int koltukHarf = 0;
            List<string> koltuk_numaralari = new List<string>();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT ulasim_araclari.koltuk_sayisi FROM ulasim_araclari, seferler WHERE seferler.arac_id = ulasim_araclari.arac_id AND seferler.sefer_id = @sefer_id", baglanti);
            komut.Parameters.AddWithValue("@sefer_id", label37.Text);
            SqlDataReader reader = komut.ExecuteReader();
            reader.Read();
            int koltuk_sayisi = int.Parse(reader["koltuk_sayisi"].ToString());
            reader.Close();
            int taraf_koltuk_sayisi = koltuk_sayisi / 4;
            
            char[] koltukHarfd = { 'A', 'B', 'C', 'D', 'E', 'F' };
            SqlCommand komut1 = new SqlCommand("SELECT biletler.koltuk_no FROM biletler WHERE sefer_id = @sefer_id", baglanti);
            komut1.Parameters.AddWithValue("@sefer_id", label37.Text);
            koltuk_numaralari.Clear();
            using (SqlDataReader reader2 = komut1.ExecuteReader())
            {
                while (reader2.Read())
                {
                    koltuk_numaralari.Add(reader2["koltuk_no"].ToString());
                }
                for (int i = 0; i < taraf_koltuk_sayisi; i++)
                {
                    for (int j = 0; j <= 4; j++)
                    {
                        if (koltukHarf >= 4)
                        {
                            koltukHarf = 0;
                        }
                        if (j == 2)
                        {
                            continue;
                        }
                        Button koltuk = new Button();
                        koltuk.Height = koltuk.Width = 35;
                        koltuk.Top = 30 + (i * 40);
                        koltuk.Left = 5 + (j * 40);
                        koltuk.Text = koltukHarfd[koltukHarf].ToString() + koltukNo.ToString();
                        if (koltuk_numaralari.Contains(koltuk.Text))
                        {
                            koltuk.Font = new Font(koltuk.Font.FontFamily, 8, FontStyle.Regular);
                            koltuk.BackColor = Color.Red;
                            koltuk.Enabled = false;
                        }
                        else
                        {
                            koltuk.Font = new Font(koltuk.Font.FontFamily, 8, FontStyle.Regular);
                            koltuk.BackColor = Color.Green;
                            koltuk.Enabled = true;
                            koltuk.Click += Koltuk_Click1; ;
                        }
                        panel5.Controls.Add(koltuk);
                        koltukHarf++;


                    }
                    koltukNo++;
                }
                reader2.Close();
                baglanti.Close();
            }
        }
        Button tiklanan2;
        Button sonTiklanan2 = null;

        private void button12_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            button13.Visible = true;
            button12.Enabled = false;
            button13.Enabled = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            button12.Enabled = true;
            button13.Enabled = false;
            label50.Text = label16.Text = "";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
            button14.Visible = true;
            button15.Enabled = false;
            button14.Enabled = true;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            panel5.Visible = true;
            button15.Enabled = true;
            button14.Enabled = false;
            label43.Text = label56.Text = "";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages[3].Enabled = true;
            tabControl1.SelectedIndex = 3;
        }
        string durum;
        private void button8_Click(object sender, EventArgs e)
        {
            durum = "Nakit";
            button8.BackColor = Color.Green;
            button9.BackColor = Color.White;
            button8.Enabled = false;
            button9.Enabled = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            durum = "Kredi Kartı";
            button9.BackColor = Color.Green;
            button8.BackColor = Color.White;
            button9.Enabled = false;
            button8.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AnaForm main_page = new AnaForm();
            DateTime odeme_tarihi = DateTime.Now;
            string odeme_tarihi_2 = odeme_tarihi.ToString("yyyy-MM-dd");

            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("INSERT INTO biletler(koltuk_no, fiyat, sefer_id, musteri_id, odeme_tarihi, odeme_turu, bilet_turu) " +
                    "VALUES(@koltuk_no, @fiyat, @sefer_id, @musteri_id, @odeme_tarihi, @odeme_turu, @bilet_turu)", baglanti);
                komut.Parameters.AddWithValue("@koltuk_no", label50.Text);
                komut.Parameters.AddWithValue("@fiyat", fiyat1);
                komut.Parameters.AddWithValue("@sefer_id", label31.Text);
                komut.Parameters.AddWithValue("@musteri_id", musteri_id);
                komut.Parameters.AddWithValue("@odeme_tarihi", odeme_tarihi_2.ToString());
                komut.Parameters.AddWithValue("@odeme_turu", durum);
                komut.Parameters.AddWithValue("@bilet_turu", "Tren");
                SqlCommand komut2 = new SqlCommand("INSERT INTO biletler(koltuk_no, fiyat, sefer_id, musteri_id, odeme_tarihi, odeme_turu, bilet_turu) " +
                    "VALUES(@koltuk_no, @fiyat, @sefer_id, @musteri_id, @odeme_tarihi, @odeme_turu, @bilet_turu)", baglanti);
                komut2.Parameters.AddWithValue("@koltuk_no", label43.Text);
                komut2.Parameters.AddWithValue("@fiyat", fiyat2);
                komut2.Parameters.AddWithValue("@sefer_id", label37.Text);
                komut2.Parameters.AddWithValue("@musteri_id", musteri_id);
                komut2.Parameters.AddWithValue("@odeme_tarihi", odeme_tarihi_2.ToString());
                komut2.Parameters.AddWithValue("@odeme_turu", durum);
                komut2.Parameters.AddWithValue("@bilet_turu", "Tren");

                if (radioButton2.Checked == true)
                {
                    int rowsAffected = komut.ExecuteNonQuery();
                    int rowsAffected2 = komut2.ExecuteNonQuery();
                    if (rowsAffected > 0 && rowsAffected2 > 0)
                    {
                        MessageBox.Show("Satın alım başarıyla tamamlandı.");
                        baglanti.Close();
                        this.Close();
                        closing();
                    }
                    else
                    {
                        MessageBox.Show("Satın alım başarısız oldu.");

                    }
                    baglanti.Close();
                }
                else
                {
                    int rowsAffected = komut.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Satın alım başarıyla tamamlandı.");
                        baglanti.Close();
                        this.Close();
                        closing();
                    }
                    else
                    {
                        MessageBox.Show("Satın alım başarısız oldu.");
                        baglanti.Close();
                    }
                    baglanti.Close();
                }

            }
            catch (SqlException sql)
            {
                MessageBox.Show(sql.ToString());
                baglanti.Close();
            }
        }

        private void Koltuk_Click1(object sender, EventArgs e)
        {
            tiklanan2 = sender as Button;
            label56.Text = tiklanan2.Text;
            if (sonTiklanan2 != null && sonTiklanan2 != tiklanan2)
            {
                sonTiklanan2.BackColor = Color.Green;
            }
            tiklanan2.BackColor = Color.Yellow;
            sonTiklanan2 = tiklanan2;
            label43.Text = label56.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            closing();
        }
        void closing()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            panel4.Controls.Clear();
            panel5.Controls.Clear();

        }
    }
}
