using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace hastaneotomasyon
{
    public partial class randevuayarla : Form
    {
        SqlConnection baglanti = new SqlConnection(
            @"Data Source=DESKTOP-6VSBRMJ\SQLEXPRESS;
              Initial Catalog=hastane;
              Integrated Security=True;
              TrustServerCertificate=True");

        public string randevuId = null;
        private int? seciliPolikinlikId;
        private int? seciliDoktorId;
        private DateTime? seciliTarih;
        private string seciliSaat;

        public randevuayarla(int? polId, int? doktorId, DateTime? tarih, string saat)
        {
            InitializeComponent();

            seciliPolikinlikId = polId;
            seciliDoktorId = doktorId;
            seciliTarih = tarih;
            seciliSaat = saat;

            comboBox1.SelectionChangeCommitted += ComboBox1_SelectionChangeCommitted;
        }

        public randevuayarla() : this(null, null, null, null) { }

        private void randevuayarla_Load(object sender, EventArgs e)
        {
            baglanti.Open();

            var daSaat = new SqlDataAdapter(
                "SELECT saat FROM saatler ORDER BY saat", baglanti);
            var dtSaat = new DataTable();
            daSaat.Fill(dtSaat);
            comboBox2.DisplayMember = "saat";
            comboBox2.ValueMember = "saat";
            comboBox2.DataSource = dtSaat;

            var daHasta = new SqlDataAdapter(
                "SELECT hasta_id, adi + ' ' + soyadi AS HastaAd FROM hastabilgi ORDER BY HastaAd",
                baglanti);
            var dtHasta = new DataTable();
            daHasta.Fill(dtHasta);
            comboBox4.DisplayMember = "HastaAd";
            comboBox4.ValueMember = "hasta_id";
            comboBox4.DataSource = dtHasta;

            var daPolik = new SqlDataAdapter(
                "SELECT id, adi FROM polikinlikler ORDER BY adi", baglanti);
            var dtPolik = new DataTable();
            daPolik.Fill(dtPolik);
            comboBox1.DisplayMember = "adi";
            comboBox1.ValueMember = "id";
            comboBox1.DataSource = dtPolik;

            

            if (!string.IsNullOrEmpty(randevuId))
            {
                LoadExistingRandevu();
            }
            else
            {
                // Yeni randevu: varsayılan tarih/saat/poliklinik ata
                if (seciliTarih.HasValue)
                    dateTimePicker1.Value = seciliTarih.Value;

                if (!string.IsNullOrEmpty(seciliSaat))
                {
                    int idx = comboBox2.FindStringExact(seciliSaat);
                    if (idx >= 0)
                        comboBox2.SelectedIndex = idx;
                }

                if (seciliPolikinlikId.HasValue)
                    comboBox1.SelectedValue = seciliPolikinlikId.Value;
            }

            PopulateDoctors(
              Convert.ToInt32(comboBox1.SelectedValue),
              seciliDoktorId);

            baglanti.Close();
        }

        private void ComboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PopulateDoctors(Convert.ToInt32(comboBox1.SelectedValue));
        }

        
        private void PopulateDoctors(int polikinlikId, int? doktorId = null)
        {
            var daDoktor = new SqlDataAdapter(@"
                SELECT id, adi + ' ' + soyadi AS DoktorAd
                  FROM doktortablosu3
                 WHERE polikinlik_id = @pid
              ORDER BY DoktorAd", baglanti);
            daDoktor.SelectCommand.Parameters.AddWithValue("@pid", polikinlikId);

            var dtDoktor = new DataTable();
            daDoktor.Fill(dtDoktor);

            comboBox3.DisplayMember = "DoktorAd";
            comboBox3.ValueMember = "id";
            comboBox3.DataSource = dtDoktor;

            if (doktorId.HasValue)
                comboBox3.SelectedValue = doktorId.Value;
        }

       
        private void LoadExistingRandevu()
        {
            using (var cmd = new SqlCommand(@"
                SELECT ran_tarih, ran_saat, polikinlik_id, doktor_id, hasta_id, aciklama
                  FROM randevular
                 WHERE ran_id = @rid", baglanti))
            {
                cmd.Parameters.AddWithValue("@rid", randevuId);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        dateTimePicker1.Value = dr.GetDateTime(dr.GetOrdinal("ran_tarih"));
                        comboBox2.SelectedValue = dr["ran_saat"].ToString();
                        comboBox1.SelectedValue = dr.GetInt32(dr.GetOrdinal("polikinlik_id"));
                        comboBox4.SelectedValue = dr.GetInt32(dr.GetOrdinal("hasta_id"));
                        seciliDoktorId = dr.GetInt32(dr.GetOrdinal("doktor_id"));

                        
                        if (!dr.IsDBNull(dr.GetOrdinal("aciklama")))
                            textBox8.Text = dr["aciklama"].ToString();
                        else
                            textBox8.Text = "";
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string seciliPolikinlikId = comboBox1.SelectedValue?.ToString();
            string seciliDoktorId = comboBox3.SelectedValue?.ToString();
            string seciliTarih = dateTimePicker1.Value.ToString("dd.MM.yyyy");
            string secilinot = textBox8.Text;
            string seciliSaat = comboBox2.SelectedValue?.ToString();
            if (
                string.IsNullOrWhiteSpace(seciliPolikinlikId) ||
                string.IsNullOrWhiteSpace(seciliDoktorId) ||
                string.IsNullOrWhiteSpace(seciliTarih) ||
                string.IsNullOrWhiteSpace(seciliSaat)
            )
            {
                MessageBox.Show("Lütfen tüm alanları seçip doldurun.");
                return;
            }


            var sorgu = @"
                SELECT COUNT(*) FROM randevular
                 WHERE polikinlik_id = @polId
                   AND doktor_id     = @dokId
                   AND ran_tarih     = @tarih
                   AND ran_saat      = @saat";

            using (var cmd = new SqlCommand(sorgu, baglanti))
            {
                cmd.Parameters.AddWithValue("@polId", comboBox1.SelectedValue);
                cmd.Parameters.AddWithValue("@dokId", comboBox3.SelectedValue);
                cmd.Parameters.AddWithValue("@tarih", dateTimePicker1.Value.Date);
                cmd.Parameters.AddWithValue("@saat", comboBox2.SelectedValue);

                baglanti.Open();
                int adet = (int)cmd.ExecuteScalar();
                baglanti.Close();

                if (adet > 0)
                {
                    MessageBox.Show("Bu saatte bu doktor için zaten bir randevu var.");
                    return;
                }

                using (var insert = new SqlCommand(@"
                    INSERT INTO randevular
                       (polikinlik_id, doktor_id, ran_tarih, ran_saat, hasta_id, aciklama)
                    VALUES
                       (@polId, @dokId, @tarih, @saat, @hastaId, @aciklama)", baglanti))
                {
                    insert.Parameters.AddWithValue("@polId", comboBox1.SelectedValue);
                    insert.Parameters.AddWithValue("@dokId", comboBox3.SelectedValue);
                    insert.Parameters.AddWithValue("@tarih", dateTimePicker1.Value.Date);
                    insert.Parameters.AddWithValue("@saat", comboBox2.SelectedValue);
                    insert.Parameters.AddWithValue("@hastaId", comboBox4.SelectedValue);
                    insert.Parameters.AddWithValue("@aciklama", textBox8.Text);


                    baglanti.Open();
                    insert.ExecuteNonQuery();
                    baglanti.Close();
                }

                MessageBox.Show("Randevu başarıyla kaydedildi.");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(randevuId))
            {
                MessageBox.Show("Silmek için geçerli bir randevu ID bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult onay = MessageBox.Show(
                "Bu randevuyu silmek istediğinize emin misiniz?",
                "Silme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (onay != DialogResult.Yes) return;

            string sorgu = "DELETE FROM randevular WHERE ran_id = @id";

            using (SqlCommand cmd = new SqlCommand(sorgu, baglanti))
            {
                cmd.Parameters.AddWithValue("@id", randevuId);

                try
                {
                    baglanti.Open();
                    int sonuc = cmd.ExecuteNonQuery();
                    baglanti.Close();

                    if (sonuc > 0)
                    {
                        MessageBox.Show("Randevu başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Randevu silinemedi. Belirtilen ID veritabanında bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (baglanti.State == ConnectionState.Open)
                        baglanti.Close();
                }
            }

        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(randevuId))
            {
                MessageBox.Show("Randevu ID eksik. Lütfen geçerli bir randevu seçin.");
                return;
            }

            if (comboBox1.SelectedValue == null ||
                comboBox3.SelectedValue == null ||
                comboBox2.SelectedValue == null ||
                comboBox4.SelectedValue == null)
            {
                MessageBox.Show("Lütfen tüm alanları seçin.");
                return;
            }

            try
            {
                baglanti.Open();

                string query = @"
            UPDATE randevular
            SET polikinlik_id = @PoliklinikId,
                doktor_id     = @DoktorId,
                ran_tarih     = @Tarih,
                ran_saat      = @Saat,
                hasta_id      = @HastaId,
                aciklama = @aciklama
            WHERE ran_id = @ran_id";


                SqlCommand aynimi1 = new SqlCommand(@"
    SELECT COUNT(*) 
      FROM randevular 
     WHERE polikinlik_id = @PoliklinikId 
       AND doktor_id     = @DoktorId 
       AND ran_tarih     = @Tarih 
       AND ran_saat      = @Saat 
       AND aciklama      = @aciklama
       AND hasta_id      = @HastaId 
       AND ran_id        = @ran_id", baglanti);

                aynimi1.Parameters.AddWithValue("@PoliklinikId", comboBox1.SelectedValue);
                aynimi1.Parameters.AddWithValue("@DoktorId", comboBox3.SelectedValue);
                aynimi1.Parameters.AddWithValue("@Tarih", dateTimePicker1.Value.Date);
                aynimi1.Parameters.AddWithValue("@Saat", comboBox2.SelectedValue);
                aynimi1.Parameters.AddWithValue("@HastaId", comboBox4.SelectedValue);
                aynimi1.Parameters.AddWithValue("@ran_id", randevuId);
                aynimi1.Parameters.AddWithValue("@aciklama", textBox8.Text);


                int sonuc1 = (int)aynimi1.ExecuteScalar();

                if (sonuc1 > 0)
                {
                    MessageBox.Show("Herhangi bir değişiklik yapmadınız!!");
                    baglanti.Close();
                    return;
                }

                using (SqlCommand cmd = new SqlCommand(query, baglanti))
                {
                    cmd.Parameters.AddWithValue("@PoliklinikId", comboBox1.SelectedValue);
                    cmd.Parameters.AddWithValue("@DoktorId", comboBox3.SelectedValue);
                    cmd.Parameters.AddWithValue("@Tarih", dateTimePicker1.Value.Date);
                    cmd.Parameters.AddWithValue("@Saat", comboBox2.SelectedValue);
                    cmd.Parameters.AddWithValue("@HastaId", comboBox4.SelectedValue);
                    cmd.Parameters.AddWithValue("@ran_id", randevuId);
                    cmd.Parameters.AddWithValue("@aciklama", textBox8.Text);


                    int affectedRows = cmd.ExecuteNonQuery();

                    if (affectedRows > 0)
                    {
                        MessageBox.Show("Randevu başarıyla güncellendi.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Güncellenecek randevu bulunamadı.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
            }
        }

    }
}