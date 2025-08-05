using BastamaExcelExport;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace hastaneotomasyon
{
    public partial class polikinlikislemleri : Form
    {
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-6VSBRMJ\SQLEXPRESS;Initial Catalog=hastane;Integrated Security=True;TrustServerCertificate=True");
        SqlDataAdapter da;
        DataTable dt;
        public polikinlikislemleri()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void polikinlikislemleri_Load(object sender, EventArgs e)
        {
            da = new SqlDataAdapter("SELECT * FROM polikinlikler", baglanti);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);

            dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            dataGridView1.Columns["adi"].HeaderText = "Poliklinik Adı";
            dataGridView1.Columns["id"].Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            int secilenid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);
            if (secilenid == null)
            {
                MessageBox.Show("hata");
                baglanti.Close();
                return;
            }
            DialogResult sonuc = MessageBox.Show("Seçilen kişiyi silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo);
            if (sonuc == DialogResult.Yes)
            {
                SqlCommand sil = new SqlCommand("DELETE from polikinlikler WHERE id = @id", baglanti);
                sil.Parameters.AddWithValue("@id", secilenid);
                sil.ExecuteNonQuery();
                baglanti.Close();
            }
            baglanti.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string adi = textBox1.Text;



            if (string.IsNullOrEmpty(adi))
            {
                MessageBox.Show("Lütfen boş bırakmayın!!");
                return;
            }
            baglanti.Open();
            SqlCommand aynimi = new SqlCommand("Select count(*) from polikinlikler where adi = @adi", baglanti);
            aynimi.Parameters.AddWithValue("@adi", adi);
            int aynimisayi = (int)aynimi.ExecuteScalar();

            if (aynimisayi > 0)
            {
                MessageBox.Show("Yazılan polikinlik zaten mevcuttur!!");
                baglanti.Close();
                return;
            }
            baglanti.Close();

            baglanti.Open();
            SqlCommand ekle = new SqlCommand("INSERT INTO polikinlikler (adi) values (@adi)", baglanti);
            ekle.Parameters.AddWithValue("@adi", adi);
            ekle.ExecuteNonQuery();
            MessageBox.Show("Polikinlik başarıyla eklendi");
            baglanti.Close();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dt.Clear();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilialan = dataGridView1.SelectedCells[0].RowIndex;
            string polikinlik = dataGridView1.Rows[secilialan].Cells[1].Value.ToString();
            textBox1.Text = polikinlik;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            string adi = textBox1.Text;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);

            if (string.IsNullOrEmpty(adi))
            {
                MessageBox.Show("Lütfen boş bırakmayın!!");
                return;
            }
            baglanti.Open();
            SqlCommand aynimi = new SqlCommand("Select count(*) from polikinlikler where adi = @adi", baglanti);
            aynimi.Parameters.AddWithValue("@adi", adi);
            int aynimisayi = (int)aynimi.ExecuteScalar();

            if (aynimisayi > 0)
            {
                MessageBox.Show("Yazılan polikinlik zaten mevcuttur!!");
                baglanti.Close();
                return;
            }
            baglanti.Close();

            baglanti.Open();
            SqlCommand degistir = new SqlCommand("UPDATE polikinlikler SET adi = @adi WHERE id = @id", baglanti);
            degistir.Parameters.AddWithValue("@adi", adi);
            degistir.Parameters.AddWithValue("@id", id);
            degistir.ExecuteNonQuery();
            MessageBox.Show("Polikinlik başarıyla değiştirildi");
            baglanti.Close();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string masaustuYolu = "C:\\Users\\Alibu\\OneDrive\\Masaüstü\\exceltablosu";
            string tarihSaat = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string dosyaAdi = $"Poliklinik_islemleri_{tarihSaat}";

            using (EbsExcelExport excelExporter = new EbsExcelExport(true))
            {
                excelExporter.ExportToExcel(dataGridView1, masaustuYolu, dosyaAdi, true);
            }
        }
    }
}
