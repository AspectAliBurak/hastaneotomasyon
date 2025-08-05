using BastamaExcelExport;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace hastaneotomasyon
{
    public partial class doktorislemleri : Form
    {
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-6VSBRMJ\SQLEXPRESS;Initial Catalog=hastane;Integrated Security=True;TrustServerCertificate=True");

        SqlDataAdapter da;
        DataTable dt;
        public doktorislemleri()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void doktorislemleri_Load(object sender, EventArgs e)
        {
            da = new SqlDataAdapter(@"
    SELECT d.id, d.adi, d.soyadi, d.tcno, d.no, d.email, 
           p.adi AS polikinlik_adi
    FROM doktortablosu3 d
    JOIN polikinlikler p ON d.polikinlik_id = p.id", baglanti);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);

            dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            dataGridView1.Columns["adi"].HeaderText = "Adı";
            dataGridView1.Columns["soyadi"].HeaderText = "Soyadı";
            dataGridView1.Columns["tcno"].HeaderText = "TC Kimlik No";
            dataGridView1.Columns["no"].HeaderText = "Telefon Numarası";
            dataGridView1.Columns["email"].HeaderText = "e-mail";
            dataGridView1.Columns["polikinlik_adi"].HeaderText = "Poliklinik Adı";


            dataGridView1.Columns["id"].Visible = false;


            SqlDataAdapter daPoliklinik = new SqlDataAdapter("SELECT id, adi FROM polikinlikler", baglanti);
            DataTable dtPoliklinik = new DataTable();
            daPoliklinik.Fill(dtPoliklinik);
            comboBox1.DataSource = dtPoliklinik;
            comboBox1.DisplayMember = "adi";
            comboBox1.ValueMember = "id";
            comboBox1.SelectedIndex = -1;







        }

        private void button5_Click(object sender, EventArgs e)
        {

            try
            {
                string adi = textBox1.Text;
                string soyadi = textBox2.Text;
                long tcno = long.Parse(textBox3.Text);
                long no = long.Parse(textBox4.Text);
                string email = textBox5.Text;
                int polikinlik = Convert.ToInt32(comboBox1.SelectedValue);
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);

                if (textBox3.Text.Length != 11 || textBox4.Text.Length != 10)
                {
                    MessageBox.Show("Lütfen numaraya 10, tcno kısmına 11 hane giriniz");
                    return;

                }

                if (string.IsNullOrWhiteSpace(adi) || string.IsNullOrWhiteSpace(soyadi) || string.IsNullOrWhiteSpace(email) || comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen boş alan bırakmayın!!");
                    return;
                }

                baglanti.Open();
                SqlCommand aynimi = new SqlCommand("SELECT COUNT(*) FROM doktortablosu3 where (email = @email or no = @no or tcno = @tcno)", baglanti);
                aynimi.Parameters.AddWithValue("tcno", tcno);
                aynimi.Parameters.AddWithValue("no", no);
                aynimi.Parameters.AddWithValue("email", email);
                aynimi.Parameters.AddWithValue("@id", id);

                int sonuc = (int)aynimi.ExecuteScalar();

                if (sonuc > 0)
                {
                    MessageBox.Show("Girdiğiniz tc no, numara veya email zaten kayıtlı bulunuyor!!");
                    baglanti.Close();
                    return;
                }

                baglanti.Close();

                baglanti.Open();
                SqlCommand ekle = new SqlCommand("INSERT INTO doktortablosu3 (adi,soyadi,polikinlik_id,tcno,no,email) VALUES (@adi, @soyadi, @polikinlik_id, @tcno, @no, @email)", baglanti);
                ekle.Parameters.AddWithValue("@adi", adi);
                ekle.Parameters.AddWithValue("@soyadi", soyadi);
                ekle.Parameters.AddWithValue("@polikinlik_id", polikinlik);
                ekle.Parameters.AddWithValue("@tcno", tcno);
                ekle.Parameters.AddWithValue("@no", no);
                ekle.Parameters.AddWithValue("@email", email);

                ekle.ExecuteNonQuery();
                baglanti.Close();

                MessageBox.Show("Kayıt başarıyla eklendi.");
            }
            catch (FormatException)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz ve TC No, No alanlarına sadece rakam giriniz!");
            }
        }
        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int secilialan = dataGridView1.SelectedCells[0].RowIndex;
            string adi = dataGridView1.Rows[secilialan].Cells[1].Value.ToString();
            string soyadi = dataGridView1.Rows[secilialan].Cells[2].Value.ToString();
            string polikinlikAdi = dataGridView1.Rows[secilialan].Cells["polikinlik_adi"].Value.ToString();
            string tcno = dataGridView1.Rows[secilialan].Cells[3].Value.ToString();
            string no = dataGridView1.Rows[secilialan].Cells[4].Value.ToString();
            string email = dataGridView1.Rows[secilialan].Cells[5].Value.ToString();

            comboBox1.Text = polikinlikAdi;

            textBox1.Text = adi;
            textBox2.Text = soyadi;
            textBox3.Text = tcno;
            textBox4.Text = no;
            textBox5.Text = email;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            dt.Clear();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek istediğiniz en az bir satır seçin.");
                return;
            }

            DialogResult sonuc = MessageBox.Show("Seçili kişileri silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo);
            if (sonuc != DialogResult.Yes)
                return;

            baglanti.Open();

            foreach (DataGridViewRow satir in dataGridView1.SelectedRows)
            {
                int secilenId = Convert.ToInt32(satir.Cells["id"].Value);

                SqlCommand sil = new SqlCommand("DELETE FROM doktortablosu3 WHERE id = @id", baglanti);
                sil.Parameters.AddWithValue("@id", secilenId);
                sil.ExecuteNonQuery();
            }

            baglanti.Close();
            MessageBox.Show("Seçilen kayıt(lar) silindi.");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand arayici = new SqlCommand("SELECT * FROM doktortablosu3 WHERE adi LIKE @arama OR soyadi LIKE @arama OR tcno LIKE @arama OR email LIKE @arama OR no LIKE @arama", baglanti);
            arayici.Parameters.AddWithValue("@arama", "%" + textBox7.Text + "%");

            SqlDataAdapter da = new SqlDataAdapter(arayici);


            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;


            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string adi = textBox1.Text;
                string soyadi = textBox2.Text;
                long tcno = long.Parse(textBox3.Text);
                long no = long.Parse(textBox4.Text);
                string email = textBox5.Text;
                string polikinlik = comboBox1.Text;

                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);

                if (textBox3.Text.Length != 11 || textBox4.Text.Length != 10)
                {
                    MessageBox.Show("Lütfen numaraya 10, tcno kısmına 11 hane giriniz");
                    return;

                }

                if (string.IsNullOrWhiteSpace(adi) || string.IsNullOrWhiteSpace(soyadi) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(polikinlik))
                {
                    MessageBox.Show("Lütfen boş alan bırakmayın!!");
                    return;
                }

                baglanti.Open();
                SqlCommand aynimi1 = new SqlCommand("SELECT COUNT(*) FROM doktortablosu3 where (adi = @adi and soyadi = @soyadi and polikinlik_id = @polikinlik_id and email = @email and no = @no and tcno = @tcno) AND id = @id", baglanti);
                int polikinlikId = Convert.ToInt32(comboBox1.SelectedValue);

                aynimi1.Parameters.AddWithValue("@adi", adi);
                aynimi1.Parameters.AddWithValue("@soyadi", soyadi);
                aynimi1.Parameters.AddWithValue("tcno", tcno);
                aynimi1.Parameters.AddWithValue("no", no);
                aynimi1.Parameters.AddWithValue("email", email);
                aynimi1.Parameters.AddWithValue("@id", id);
                aynimi1.Parameters.AddWithValue("@polikinlik_id", polikinlikId);

                int sonuc2 = (int)aynimi1.ExecuteScalar();

                if (sonuc2 > 0)
                {
                    MessageBox.Show("Herhangi bir değişiklik yapmadınız!!");
                    baglanti.Close();
                    return;
                }
                baglanti.Close();



                baglanti.Open();
                SqlCommand aynimi = new SqlCommand("SELECT COUNT(*) FROM doktortablosu3 where (email = @email or no = @no or tcno = @tcno) AND id != @id", baglanti);
                aynimi.Parameters.AddWithValue("tcno", tcno);
                aynimi.Parameters.AddWithValue("no", no);
                aynimi.Parameters.AddWithValue("email", email);
                aynimi.Parameters.AddWithValue("@id", id);

                int sonuc = (int)aynimi.ExecuteScalar();

                if (sonuc > 0)
                {
                    MessageBox.Show("Girdiğiniz tc no, numara veya email zaten kayıtlı bulunuyor!!");
                    baglanti.Close();
                    return;
                }
                baglanti.Close();



                baglanti.Open();
                SqlCommand guncelle = new SqlCommand("UPDATE doktortablosu3 SET adi = @adi, soyadi = @soyadi, polikinlik_id = @polikinlik_id, email = @email, tcno = @tcno, no = @no WHERE id = @id", baglanti);


                guncelle.Parameters.AddWithValue("@adi", adi);
                guncelle.Parameters.AddWithValue("@soyadi", soyadi);
                guncelle.Parameters.AddWithValue("@tcno", tcno);
                guncelle.Parameters.AddWithValue("@no", no);
                guncelle.Parameters.AddWithValue("@email", email);
                guncelle.Parameters.AddWithValue("@id", id);
                guncelle.Parameters.AddWithValue("@polikinlik_id", polikinlikId);



                guncelle.ExecuteNonQuery();
                baglanti.Close();

                MessageBox.Show("Güncelleme başarılı!");


            }
            catch (FormatException)
            {
                MessageBox.Show("Boş yer bırakmayın ve Tc no, numara kısmına sadece sayı girişi yapın!!");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            comboBox1.SelectedIndex = -1;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            string masaustuYolu = "C:\\Users\\Alibu\\OneDrive\\Masaüstü\\exceltablosu";
            string tarihSaat = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string dosyaAdi = $"Doktor_islemleri_{tarihSaat}";

            using (EbsExcelExport excelExporter = new EbsExcelExport(true))
            {
                excelExporter.ExportToExcel(dataGridView1, masaustuYolu, dosyaAdi, true);
            }
        }
    }
}

