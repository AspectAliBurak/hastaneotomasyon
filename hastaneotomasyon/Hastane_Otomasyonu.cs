using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hastaneotomasyon
{
    public partial class Hastane_Otomasyonu : Form
    {
        public Hastane_Otomasyonu()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-6VSBRMJ\SQLEXPRESS;Initial Catalog=hastane;Integrated Security=True;TrustServerCertificate=True");

        private void Hastane_Otomasyonu_Load(object sender, EventArgs e)
        {

        }

        private void Hastane_Otomasyonu_Shown(object sender, EventArgs e)
        {
            string yazankullaniciadi = kullanici.kullaniciadi;
            statusStrip1.Items.Add($"Sürüm 1.0.0 | Giriş yapan: {yazankullaniciadi}");

            menuStrip1.Enabled = false;
            var frmGiris = new frmGiris();
            if (frmGiris.ShowDialog() == DialogResult.OK)
            {
                frmGiris.Close();
                menuStrip1.Enabled = true;
            }
            else
                this.Close();
        }

        private void hastaListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }



        private void polikinlikİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            polikinlikislemleri frm = new polikinlikislemleri();
            frm.Show();
        }

        private void adminİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (kullanici.yetkiseviyesi == 3)
            {

                adminislemleri frm = new adminislemleri();
                frm.Show();
            }
            else
            {

                MessageBox.Show("Yetki seviyeniz dışındadır!!");
            }
        }

        private void hastaİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hastabilgi frm = new hastabilgi();
            frm.Show();
        }

        private void doktorİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doktorislemleri frm = new doktorislemleri();
            frm.Show();
        }

        private void mnuRandevuIslemleri_Click(object sender, EventArgs e)
        {
            randevuekranı frm = new randevuekranı();
            frm.Show();
        }
    }
}
