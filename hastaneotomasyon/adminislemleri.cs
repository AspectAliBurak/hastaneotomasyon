using BastamaExcelExport;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using BastamaExcelExport;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace hastaneotomasyon
{
    public partial class adminislemleri : Form
    {

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-6VSBRMJ\SQLEXPRESS;Initial Catalog=hastane;Integrated Security=True;TrustServerCertificate=True");
        SqlDataAdapter da;
        DataTable dt;
        public adminislemleri()
        {
            InitializeComponent();
        }

        private void adminislemleri_Load(object sender, EventArgs e)
        {

            this.BackColor = ColorTranslator.FromHtml("#F5F5F5");




            da = new SqlDataAdapter("SELECT * FROM kullanicitablosu", baglanti);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);

            dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            dataGridView1.Columns["kullaniciadi"].HeaderText = "Kullanici Adı";
            dataGridView1.Columns["sifre"].HeaderText = "Şifre";
            dataGridView1.Columns["tcno"].HeaderText = "TC Kimlik No";
            dataGridView1.Columns["yetkiseviyesi"].HeaderText = "Yetki Seviyesi";



            SqlDataAdapter yetkilerDa = new SqlDataAdapter("SELECT DISTINCT yetkiseviyesi FROM kullanicitablosu", baglanti);
            DataTable yetkiDt = new DataTable();
            yetkilerDa.Fill(yetkiDt);

            comboBox1.DataSource = yetkiDt;
            comboBox1.DisplayMember = "yetkiseviyesi";
            comboBox1.ValueMember = "yetkiseviyesi";

        }





        private void button1_Click(object sender, EventArgs e)
        {
            string adi = textBox1.Text;
            string tcno = textBox2.Text;
            string sifre = textBox3.Text;

            int yetkiseviyesi = Convert.ToInt32(comboBox1.SelectedValue);


            if (string.IsNullOrWhiteSpace(adi) || string.IsNullOrWhiteSpace(tcno) || string.IsNullOrWhiteSpace(sifre) || comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Lütfen boş alan bırakmayın");
                return;
            }

            if (textBox2.Text.Length != 11)
            {
                MessageBox.Show("Lütfen tcno kısmına 11 hane giriniz");
                return;
            }

            baglanti.Open();
            SqlCommand aynimi = new SqlCommand("Select count(*) from kullanicitablosu where kullaniciadi = @kullaniciadi or tcno = @tcno", baglanti);
            aynimi.Parameters.AddWithValue("@kullaniciadi", adi);
            aynimi.Parameters.AddWithValue("@tcno", tcno);
            int aynimisayi = (int)aynimi.ExecuteScalar();

            if (aynimisayi > 0)
            {
                MessageBox.Show("TC no ve kullanıcı adı benzersiz olmalıdır!!");
                baglanti.Close();
                return;
            }
            baglanti.Close();

            baglanti.Open();
            SqlCommand kaydet = new SqlCommand("INSERT INTO kullanicitablosu (kullaniciadi,tcno,sifre,yetkiseviyesi) VALUES (@kullaniciadi,@tcno,@sifre,@yetkiseviyesi)", baglanti);
            kaydet.Parameters.AddWithValue("@kullaniciadi", adi);
            kaydet.Parameters.AddWithValue("@tcno", tcno);
            kaydet.Parameters.AddWithValue("@sifre", sifre);
            kaydet.Parameters.AddWithValue("@yetkiseviyesi", yetkiseviyesi);
            kaydet.ExecuteNonQuery();
            baglanti.Close();

            MessageBox.Show("Kayıt başarıyla eklendi.");



        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilialan = dataGridView1.SelectedCells[0].RowIndex;
            string kullaniciadi = dataGridView1.Rows[secilialan].Cells[0].Value.ToString();
            string sifre = dataGridView1.Rows[secilialan].Cells[1].Value.ToString();
            string yetkiseviyesi = dataGridView1.Rows[secilialan].Cells[2].Value.ToString();
            string tcno = dataGridView1.Rows[secilialan].Cells[3].Value.ToString();



            textBox1.Text = kullaniciadi;
            textBox2.Text = tcno;
            textBox3.Text = sifre;
            comboBox1.SelectedValue = yetkiseviyesi;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            long secilentcno = Convert.ToInt64(dataGridView1.CurrentRow.Cells["tcno"].Value);
            if (secilentcno == null)
            {
                MessageBox.Show("hata");
                baglanti.Close();
                return;

            }
            baglanti.Close();
            baglanti.Open();
            DialogResult sonuc = MessageBox.Show("Seçilen kişiyi silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo);
            if (sonuc == DialogResult.Yes)
            {
                SqlCommand sil = new SqlCommand("DELETE from kullanicitablosu WHERE tcno = @tcno", baglanti);
                sil.Parameters.AddWithValue("@tcno", secilentcno);
                sil.ExecuteNonQuery();
                MessageBox.Show("Kişi silindi");
                baglanti.Close();

            }
            baglanti.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string adi = textBox1.Text;
            string tcno = textBox2.Text;
            string sifre = textBox3.Text;

            if ((string.IsNullOrWhiteSpace(adi) || string.IsNullOrWhiteSpace(tcno) || string.IsNullOrWhiteSpace(sifre) || comboBox1.SelectedValue == null))
            {

                MessageBox.Show("Lütfen boş bırakmayın!!");
                return;
            }

            if (textBox2.Text.Length != 11)
            {
                MessageBox.Show("Lütfen tcno kısmına 11 hane giriniz");
                return;
            }


            int yetkiseviyesi = Convert.ToInt32(comboBox1.SelectedValue);
            int secilenSatir = dataGridView1.SelectedCells[0].RowIndex;
            int id = Convert.ToInt32(dataGridView1.Rows[secilenSatir].Cells["id"].Value);


           
            baglanti.Open();
            SqlCommand aynimi1 = new SqlCommand("SELECT COUNT(*) FROM kullanicitablosu where (yetkiseviyesi = @yetkiseviyesi and sifre = @sifre and kullaniciadi = @kullaniciadi and tcno = @tcno) AND id = @id", baglanti);
            aynimi1.Parameters.AddWithValue("@kullaniciadi", adi);
            aynimi1.Parameters.AddWithValue("@tcno", tcno);
            aynimi1.Parameters.AddWithValue("@yetkiseviyesi", yetkiseviyesi);
            aynimi1.Parameters.AddWithValue("@sifre", sifre);
            aynimi1.Parameters.AddWithValue("@id", id);
            int sonuc1 = (int)aynimi1.ExecuteScalar();
            if (sonuc1 > 0)
            {
                MessageBox.Show("Herhangi bir değişiklik yapmadınız!!");
                baglanti.Close();
                return;
            }
            baglanti.Close();


            baglanti.Open();
            SqlCommand aynimi = new SqlCommand("SELECT COUNT(*) FROM kullanicitablosu where (kullaniciadi = @kullaniciadi or tcno = @tcno) AND id != @id", baglanti);
            aynimi.Parameters.AddWithValue("@kullaniciadi", adi);
            aynimi.Parameters.AddWithValue("@tcno", tcno);
            aynimi.Parameters.AddWithValue("@id", id);

            int sonuc = (int)aynimi.ExecuteScalar();

            if (sonuc > 0)
            {
                MessageBox.Show("TC no ve kullanıcı adı benzersiz olmalıdır!!");
                baglanti.Close();
                return;
            }
            baglanti.Close();

            baglanti.Open();
            SqlCommand guncelle = new SqlCommand("UPDATE kullanicitablosu SET kullaniciadi = @kullaniciadi, tcno = @tcno, yetkiseviyesi = @yetkiseviyesi, sifre = @sifre WHERE id = @id", baglanti);

            int polikinlikId = Convert.ToInt32(comboBox1.SelectedValue);

            guncelle.Parameters.AddWithValue("@kullaniciadi", adi);
            guncelle.Parameters.AddWithValue("@tcno", tcno);
            guncelle.Parameters.AddWithValue("@yetkiseviyesi", yetkiseviyesi);
            guncelle.Parameters.AddWithValue("@sifre", sifre);
            guncelle.Parameters.AddWithValue("@id", id);

            guncelle.ExecuteNonQuery();
            baglanti.Close();

            MessageBox.Show("Güncelleme başarılı!");

        }


        private void button5_Click(object sender, EventArgs e)
        {
            ButonClass.Yenile(dataGridView1, da, dt);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string masaustuYolu = "C:\\Users\\Alibu\\OneDrive\\Masaüstü\\exceltablosu";
            string tarihSaat = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string dosyaAdi = $"Admin_islemleri_{tarihSaat}";

            using (EbsExcelExport excelExporter = new EbsExcelExport(true))
            {
                excelExporter.ExportToExcel(dataGridView1, masaustuYolu, dosyaAdi, true);
            }
        }
    }
}
