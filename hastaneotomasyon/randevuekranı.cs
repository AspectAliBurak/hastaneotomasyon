using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace hastaneotomasyon
{
    public partial class randevuekranı : Form
    {
        SqlConnection baglanti = new SqlConnection(
            @"Data Source=DESKTOP-6VSBRMJ\SQLEXPRESS;
              Initial Catalog=hastane;
              Integrated Security=True;
              TrustServerCertificate=True");

        public randevuekranı()
        {
            InitializeComponent();

            cmbPolikinlik.SelectedIndexChanged += cmbPolikinlik_SelectedIndexChanged;
            cmbDoktor.SelectedIndexChanged += cmbDoktor_SelectedIndexChanged;
        }

        private void randevuekranı_Load(object sender, EventArgs e)
        {
            baglanti.Open();

            string query = "SELECT id, adi FROM polikinlikler";
            SqlDataAdapter da = new SqlDataAdapter(query, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);

            var row = dt.NewRow();
            row["id"] = 0;
            row["adi"] = "Tümü";
            dt.Rows.InsertAt(row, 0);

            cmbPolikinlik.DisplayMember = "adi";
            cmbPolikinlik.ValueMember = "id";
            cmbPolikinlik.DataSource = dt;

            cmbDoktor.Items.Add("Tümü");
            cmbDoktor.SelectedIndex = 0;

            DateTime secilenTarih = dateTimePicker1.Value.Date; 


        

            baglanti.Close();
        }

        private void cmbPolikinlik_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Otomatik tetiklenen olay
        }

        private void cmbPolikinlik_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int polikinlikid = Convert.ToInt32(cmbPolikinlik.SelectedValue);

            string query;
            if (polikinlikid > 0)
                query = "SELECT id, adi + ' ' + soyadi AS DoktorAd " +
                        "FROM doktortablosu3 " +
                        "WHERE polikinlik_id = @polikinlik_id " +
                        "ORDER BY 2";
            else
                query = "SELECT id, adi + ' ' + soyadi AS DoktorAd " +
                        "FROM doktortablosu3 " +
                        "ORDER BY 2";

            SqlDataAdapter da = new SqlDataAdapter(query, baglanti);
            da.SelectCommand.Parameters.AddWithValue("@polikinlik_id", polikinlikid);

            DataTable dt = new DataTable();
            da.Fill(dt);

            var row = dt.NewRow();
            row["id"] = 0;
            row["DoktorAd"] = "Tümü";
            dt.Rows.InsertAt(row, 0);

            cmbDoktor.DisplayMember = "DoktorAd";
            cmbDoktor.ValueMember = "id";
            cmbDoktor.DataSource = dt;
            cmbDoktor.SelectedIndex = 0;
        }

        

        private void cmbDoktor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Listele();
        }

        private void Listele()
        {
            int doktorId = Convert.ToInt32(cmbDoktor.SelectedValue);
            int polikinlikId = Convert.ToInt32(cmbPolikinlik.SelectedValue);
            DateTime secilenTarih = dateTimePicker1.Value.Date;

            string query = @"
        SET DATEFORMAT DMY;
        SELECT
            s.saat,
            CASE WHEN r.ran_id IS NULL THEN 'MÜSAİT' ELSE 'DOLU' END AS Durum,
            h.adi + ' ' + h.soyadi   AS Hasta,
            d.adi + ' ' + d.soyadi   AS Doktor,
            p.adi                    AS Poliklinik,
            r.aciklama, 
            r.ran_id
        FROM saatler s
        LEFT JOIN randevular r
          ON s.saat           = r.ran_saat
         AND r.ran_tarih     = @tarih
         AND (@polikinlikId = 0 OR r.polikinlik_id = @polikinlikId)
         AND (@doktorId     = 0 OR r.doktor_id       = @doktorId)
        LEFT JOIN hastabilgi h ON r.hasta_id      = h.hasta_id
        LEFT JOIN doktortablosu3 d ON r.doktor_id  = d.id
        LEFT JOIN polikinlikler p ON r.polikinlik_id = p.id;
    ";



            SqlDataAdapter da = new SqlDataAdapter(query, baglanti);
            da.SelectCommand.Parameters.AddWithValue("@tarih", secilenTarih);
            da.SelectCommand.Parameters.AddWithValue("@doktorId", doktorId);
            da.SelectCommand.Parameters.AddWithValue("@polikinlikId", polikinlikId);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvSaatler.DataSource = dt;
            dgvSaatler.ClearSelection();
            dgvSaatler.Columns["ran_id"].Visible = false;
            dgvSaatler.Columns["aciklama"].HeaderText = "Not";
        }

        private void dgvSaatler_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0) return;

            int poliklinikId = Convert.ToInt32(cmbPolikinlik.SelectedValue);
            int doktorId = Convert.ToInt32(cmbDoktor.SelectedValue);
            DateTime secilenTarih = dateTimePicker1.Value;
            string secilenSaat = dgvSaatler
                                 .Rows[e.RowIndex]
                                 .Cells["saat"]
                                 .Value
                                 .ToString();

            string ranId = dgvSaatler
                           .Rows[e.RowIndex]
                           .Cells["ran_id"]
                           .Value
                           ?.ToString();

            var frm = new randevuayarla(poliklinikId,
                                       doktorId,
                                       secilenTarih,
                                       secilenSaat);
            frm.randevuId = ranId; // Güncelleme için


            if (frm.ShowDialog() == DialogResult.OK)
            {
                Listele();
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            Listele();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dgvSaatler_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //private void cmbTarih_SelectedIndexChanged_1(object sender, EventArgs e)
        //{

        //}
    }
}