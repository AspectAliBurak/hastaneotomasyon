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

namespace hastaneotomasyon
{
    public partial class hastabilgi : Form
    {

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-6VSBRMJ\SQLEXPRESS;Initial Catalog=hastane;Integrated Security=True;TrustServerCertificate=True");
        SqlDataAdapter da;
        DataTable dt;
        public hastabilgi()
        {
            InitializeComponent();
        }

        private void hastabilgi_Load(object sender, EventArgs e)
        {
            da = new SqlDataAdapter("SELECT * FROM hastabilgi", baglanti);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);

            dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            dataGridView1.Columns["adi"].HeaderText = "Adı";
            dataGridView1.Columns["soyadi"].HeaderText = "Soyadı";
            dataGridView1.Columns["tel_no"].HeaderText = "Telefon Numarası";
            dataGridView1.Columns["tc_no"].HeaderText = "TC Kimlik No";
            dataGridView1.Columns["email"].HeaderText = "E-mail";

            dataGridView1.Columns["hasta_id"].Visible = false;
        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string adi = textBox1.Text;
                string soyadi = textBox2.Text;
                long no = long.Parse(textBox3.Text);
                long tcno = long.Parse(textBox4.Text);
                string email = textBox5.Text;
                int hasta_id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["hasta_id"].Value);


                if (textBox3.Text.Length != 10 || textBox4.Text.Length != 11)
                {
                    MessageBox.Show("Lütfen numaraya 10, tcno kısmına 11 hane giriniz");
                    return;

                }

                if (string.IsNullOrWhiteSpace(adi) || string.IsNullOrWhiteSpace(soyadi))

                {

                    MessageBox.Show("Lütfen boş yer bırakmayınız!!");
                    return;
                }

                baglanti.Open();
                SqlCommand aynimi1 = new SqlCommand("SELECT COUNT(*) FROM hastabilgi where (adi = @adi and soyadi = @soyadi and email = @email and tel_no = @tel_no and tc_no = @tc_no) AND hasta_id = @hasta_id", baglanti);

                aynimi1.Parameters.AddWithValue("@adi", adi);
                aynimi1.Parameters.AddWithValue("@soyadi", soyadi);
                aynimi1.Parameters.AddWithValue("@email", email);
                aynimi1.Parameters.AddWithValue("@tel_no", no);
                aynimi1.Parameters.AddWithValue("@tc_no", tcno);
                aynimi1.Parameters.AddWithValue("@hasta_id", hasta_id);
                int sonuc1 = (int)aynimi1.ExecuteScalar();
                if (sonuc1 > 0)
                {
                    MessageBox.Show("Herhangi bir değişiklik yapmadınız!!");
                    baglanti.Close();
                    return;
                }
                baglanti.Close();


                baglanti.Open();
                SqlCommand aynimi = new SqlCommand("SELECT COUNT(*) FROM hastabilgi where (tel_no = @tel_no or tc_no = @tc_no) AND hasta_id != @hasta_id", baglanti);
                aynimi.Parameters.AddWithValue("@tel_no", no);
                aynimi.Parameters.AddWithValue("@tc_no", tcno);
                aynimi.Parameters.AddWithValue("@hasta_id", hasta_id);
                int sonuc = (int)aynimi.ExecuteScalar();


                if (sonuc > 0)
                {
                    MessageBox.Show("TC no ve Numara benzersiz olmalıdır!!");
                    baglanti.Close();
                    return;
                }
                baglanti.Close();


                baglanti.Open();
                if (string.IsNullOrWhiteSpace(email))
                {
                    baglanti.Close();
                }
                else
                {
                    SqlCommand bosluk = new SqlCommand("SELECT COUNT(*) FROM hastabilgi WHERE email = @email AND hasta_id != @hasta_id", baglanti);
                    bosluk.Parameters.AddWithValue("@email", email);
                    bosluk.Parameters.AddWithValue("@hasta_id", hasta_id);

                    int boslukvarmi = (int)bosluk.ExecuteScalar();

                    if (boslukvarmi > 0)
                    {
                        MessageBox.Show("Mail bilgileri zaten kayıtlıdır!!");
                        baglanti.Close();
                        return;
                    }
                    baglanti.Close();
                }


                baglanti.Open();
                SqlCommand guncelle = new SqlCommand("UPDATE hastabilgi SET adi = @adi, soyadi = @soyadi, email = @email, tc_no = @tc_no, tel_no = @tel_no WHERE hasta_id = @hasta_id", baglanti);

                guncelle.Parameters.AddWithValue("@adi", adi);
                guncelle.Parameters.AddWithValue("@soyadi", soyadi);
                guncelle.Parameters.AddWithValue("@email", email);
                guncelle.Parameters.AddWithValue("@tel_no", no);
                guncelle.Parameters.AddWithValue("@tc_no", tcno);
                guncelle.Parameters.AddWithValue("@hasta_id", hasta_id);


                guncelle.ExecuteNonQuery();
                baglanti.Close();

                MessageBox.Show("Güncelleme başarılı!");


            }
            catch (FormatException)
            {
                MessageBox.Show("Boş yer bırakmayın ve Tc no, numara kısmına sadece sayı girişi yapın!!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand arayici = new SqlCommand("SELECT * FROM hastabilgi WHERE adi LIKE @arama OR soyadi LIKE @arama OR tc_no LIKE @arama OR email LIKE @arama OR tel_no LIKE @arama", baglanti);
            arayici.Parameters.AddWithValue("@arama", "%" + textBox7.Text + "%");

            SqlDataAdapter da = new SqlDataAdapter(arayici);


            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;


            baglanti.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için en az bir satır seçin.");
                return;
            }

            DialogResult sonuc = MessageBox.Show("Seçilen kişileri silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo);
            if (sonuc != DialogResult.Yes)
                return;


            baglanti.Open();

            foreach (DataGridViewRow satir in dataGridView1.SelectedRows)
            {
                if (satir.Cells["hasta_id"].Value != null)
                {
                    int hasta_id = Convert.ToInt32(satir.Cells["hasta_id"].Value);
                    SqlCommand sil = new SqlCommand("DELETE FROM hastabilgi WHERE hasta_id = @hasta_id", baglanti);
                    sil.Parameters.AddWithValue("@hasta_id", hasta_id);
                    sil.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Seçilen kayıtlar silindi.");
        }





        private void button4_Click(object sender, EventArgs e)
        {
            ButonClass.Yenile(dataGridView1, da, dt);


        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string adi = textBox1.Text;
                string soyadi = textBox2.Text;
                long no = long.Parse(textBox3.Text);
                long tcno = long.Parse(textBox4.Text);
                string email = textBox5.Text;


                if (textBox3.Text.Length != 10 || textBox4.Text.Length != 11)
                {
                    MessageBox.Show("Lütfen numaraya 10, tcno kısmına 11 hane giriniz");
                    return;
                }

                if (string.IsNullOrWhiteSpace(adi) || string.IsNullOrWhiteSpace(soyadi))
                {
                    MessageBox.Show("Lütfen boş yer bırakmayınız!!");
                    return;

                }


                baglanti.Open();
                SqlCommand aynimi = new SqlCommand("SELECT COUNT(*) FROM hastabilgi WHERE tel_no = @tel_no or tc_no = @tc_no", baglanti);

                aynimi.Parameters.AddWithValue("@tel_no", no);
                aynimi.Parameters.AddWithValue("@tc_no", tcno);
                int aynimisayi = (int)aynimi.ExecuteScalar();

                if (aynimisayi >= 1)
                {
                    MessageBox.Show("TC no ve numara bilgileri zaten kayıtlıdır!!");
                    baglanti.Close();
                    return;

                }
                baglanti.Close();

                baglanti.Open();
                if (string.IsNullOrWhiteSpace(email))
                {
                    baglanti.Close();
                }
                else
                {
                    SqlCommand bosluk = new SqlCommand("SELECT COUNT(*) FROM hastabilgi WHERE email = @email", baglanti);
                    bosluk.Parameters.AddWithValue("@email", email);

                    int boslukvarmi = (int)bosluk.ExecuteScalar();

                    if (boslukvarmi > 0)
                    {
                        MessageBox.Show("Mail bilgileri zaten kayıtlıdır!!");
                        baglanti.Close();
                        return;
                    }
                    baglanti.Close();
                }


                baglanti.Open();
                SqlCommand ekle = new SqlCommand("INSERT INTO hastabilgi (adi, soyadi, tc_no, tel_no, email) VALUES (@adi, @soyadi, @tc_no, @tel_no, @email)", baglanti);
                ekle.Parameters.AddWithValue("@adi", adi);
                ekle.Parameters.AddWithValue("@soyadi", soyadi);
                ekle.Parameters.AddWithValue("@tc_no", tcno);
                ekle.Parameters.AddWithValue("@tel_no", no);
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilialan = dataGridView1.SelectedCells[0].RowIndex;
            string adi = dataGridView1.Rows[secilialan].Cells[1].Value.ToString();
            string soyadi = dataGridView1.Rows[secilialan].Cells[2].Value.ToString();
            string no = dataGridView1.Rows[secilialan].Cells[3].Value.ToString();
            string tcno = dataGridView1.Rows[secilialan].Cells[4].Value.ToString();
            string email = dataGridView1.Rows[secilialan].Cells[5].Value.ToString();

            textBox1.Text = adi;
            textBox2.Text = soyadi;
            textBox3.Text = no;
            textBox4.Text = tcno;
            textBox5.Text = email;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string masaustuYolu = "C:\\Users\\Alibu\\OneDrive\\Masaüstü\\exceltablosu";
            string tarihSaat = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string dosyaAdi = $"Hasta_islemleri_{tarihSaat}";

            using (EbsExcelExport excelExporter = new EbsExcelExport(true))
            {
                excelExporter.ExportToExcel(dataGridView1, masaustuYolu, dosyaAdi, true);
            }
        }
    }
}
