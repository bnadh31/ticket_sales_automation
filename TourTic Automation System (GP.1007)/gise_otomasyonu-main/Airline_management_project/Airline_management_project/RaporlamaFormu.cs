using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using CrystalDecisions.Shared;
using System.IO;


namespace Airline_management_project
{
    public partial class RaporlamaFormu : Form
    {
        public RaporlamaFormu()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("server=172.20.10.2;database=otomasyon_veritabani;UID=SA;password=reallyStrongPwd123");
        ReportDocument rapor = new ReportDocument();
        ReportDocument rapor2 = new ReportDocument();
        ReportDocument rapor3 = new ReportDocument();
        SqlDataAdapter sa;

        private void RaporlamaFormu_Load(object sender, EventArgs e)
        {
            //SATIŞ RAPORU GÖRÜNTÜLEME
            TableLogOnInfo crTableLogonInfo = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            rapor.Load(@"C:\\Users\\gokhanpc\\Desktop\\gise_otomasyonu-main\\Airline_management_project\\Airline_management_project\\Raporlama.rpt");
            crConnectionInfo.ServerName = "172.20.10.2";
            crConnectionInfo.DatabaseName = "otomasyon_veritabani";
            crConnectionInfo.UserID = "SA";
            crConnectionInfo.Password = "reallyStrongPwd123";
            foreach (Table crTable in rapor.Database.Tables)
            {
                crTableLogonInfo = crTable.LogOnInfo;
                crTableLogonInfo.ConnectionInfo = crConnectionInfo;
                crTable.ApplyLogOnInfo(crTableLogonInfo);
            }
            
            crystalReportViewer1.ReportSource = rapor;
            //GÜN SONU RAPORU GÖRÜNTÜLEME
            rapor2.Load(@"C:\\Users\\gokhanpc\\Desktop\\gise_otomasyonu-main\\Airline_management_project\\Airline_management_project\\Raporlama2.rpt");
            foreach (Table crTable in rapor2.Database.Tables)
            {
                crTableLogonInfo = crTable.LogOnInfo;
                crTableLogonInfo.ConnectionInfo = crConnectionInfo;
                crTable.ApplyLogOnInfo(crTableLogonInfo);
            }
            crystalReportViewer2.ReportSource = rapor2;

            //FİLO RAPORU GÖRÜNTÜLEME
            rapor3.Load(@"C:\\Users\\gokhanpc\\Desktop\\gise_otomasyonu-main\\Airline_management_project\\Airline_management_project\\Raporlama3.rpt");
            foreach (Table crTable in rapor3.Database.Tables)
            {
                crTableLogonInfo = crTable.LogOnInfo;
                crTableLogonInfo.ConnectionInfo = crConnectionInfo;
                crTable.ApplyLogOnInfo(crTableLogonInfo);
            }
            crystalReportViewer3.ReportSource = rapor3;
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
