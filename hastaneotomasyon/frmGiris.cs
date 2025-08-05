using Microsoft.Data.SqlClient;
using System;
using System.Windows.Forms;

namespace hastaneotomasyon
{
    public partial class frmGiris : Form
    {
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-6VSBRMJ\SQLEXPRESS;Initial Catalog=hastane;Integrated Security=True;TrustServerCertificate=True");

        public frmGiris()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.BackColor = ColorTranslator.FromHtml("#1F1F1F");

            textBox1.BackColor = Color.FromArgb(30, 30, 30);
            textBox1.ForeColor = Color.White;

            textBox2.BackColor = Color.FromArgb(30, 30, 30);
            textBox2.ForeColor = Color.White;



            textBox1.PlaceholderText = "Kullanýcý adý";
            textBox2.PlaceholderText = "Þifre";

            //textBox1.Text = "admin";
            //textBox2.Text = "123456";
            //btnGiris.PerformClick();

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            string adi = textBox1.Text;
            string sifre = textBox2.Text;

            baglanti.Open();

            SqlCommand komut = new SqlCommand("SELECT YetkiSeviyesi FROM kullanicitablosu WHERE kullaniciadi = @kullaniciadi AND sifre = @sifre", baglanti);
            komut.Parameters.AddWithValue("@kullaniciadi", adi);
            komut.Parameters.AddWithValue("@sifre", sifre);

            SqlDataReader dr = komut.ExecuteReader();

            if (dr.Read())
            {
                kullanici.kullaniciadi = adi;
                kullanici.yetkiseviyesi = Convert.ToInt32(dr["yetkiseviyesi"]);

                this.DialogResult = DialogResult.OK;
               
            }
            else
            {
                MessageBox.Show("Hatalý kullanýcý adý veya þifre");
            }

            baglanti.Close();
        }
    }
}
