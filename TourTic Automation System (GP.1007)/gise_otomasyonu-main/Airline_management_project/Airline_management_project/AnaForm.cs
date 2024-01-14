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
    public partial class AnaForm : Form
    {
        public string personel_ad { get; set; }
        public string checkin_personel { get; set; }
        public string personel_soyad { get; set; }
        public DateTime personel_dogum_tarihi { get; set; }
        public string personel_tc_no { get; set; }
        public string personel_gorev { get; set; }
        public string personel_cinsiyet { get; set; }
        public string personel_tecrube { get; set; }

        public AnaForm()
        {
            InitializeComponent();
            
        }
        
        UcakSatis USatis = new UcakSatis();
        SeferIslemleri seferMenu = new SeferIslemleri();
        UlasimAraclari ulasimAraclari = new UlasimAraclari();
        PersonelFormu personelFormu = new PersonelFormu();
        Profil_Bilgileri profilFormu = new Profil_Bilgileri();
        OtobusSatis otobusFormu = new OtobusSatis();
        BiletMenusu BiletMenusu = new BiletMenusu();
        TrenSatis TSatis = new TrenSatis();
        RaporlamaFormu Rapor = new RaporlamaFormu();
        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void biletİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AnaForm_Load(object sender, EventArgs e)
        {
            label2.Text = personel_ad + " " + personel_soyad;
            if(personel_gorev != "Supervisor")
            {
                button8.Enabled = false;
                button9.Enabled = false;
                button10.Enabled = false;
                button12.Enabled = false;
                
            }
            
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            Button buton = new Button();


            panel2.Controls.Add(buton);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            checkin_personel = label2.Text;
            BiletMenusu.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            USatis.ShowDialog();
        }

        private void button10_Click_2(object sender, EventArgs e)
        {
            seferMenu.ShowDialog();
            
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ulasimAraclari.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            personelFormu.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            profilFormu.personel_ad = personel_ad;
            profilFormu.personel_soyad = personel_soyad;
            profilFormu.personel_tc_no = personel_tc_no;
            profilFormu.personel_dogum_tarihi = personel_dogum_tarihi;
            profilFormu.personel_gorev = personel_gorev;
            profilFormu.personel_cinsiyet = personel_cinsiyet;
            profilFormu.personel_tecrube = personel_tecrube;

            profilFormu.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GirisYap girisFormu = new GirisYap();
            this.Close();
            girisFormu.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            otobusFormu.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.DarkGray;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TSatis.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Rapor.ShowDialog();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(51, 153, 255);
        }
    }
}
