namespace hastaneotomasyon
{
    partial class randevuekranı
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvSaatler = new DataGridView();
            label2 = new Label();
            cmbPolikinlik = new ComboBox();
            cmbDoktor = new ComboBox();
            label3 = new Label();
            label4 = new Label();
            groupBox1 = new GroupBox();
            dateTimePicker1 = new DateTimePicker();
            btnAra = new Button();
            groupBox2 = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)dgvSaatler).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // dgvSaatler
            // 
            dgvSaatler.AllowUserToAddRows = false;
            dgvSaatler.AllowUserToDeleteRows = false;
            dgvSaatler.AllowUserToResizeColumns = false;
            dgvSaatler.AllowUserToResizeRows = false;
            dgvSaatler.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSaatler.Dock = DockStyle.Fill;
            dgvSaatler.Location = new Point(3, 23);
            dgvSaatler.MultiSelect = false;
            dgvSaatler.Name = "dgvSaatler";
            dgvSaatler.ReadOnly = true;
            dgvSaatler.RowHeadersVisible = false;
            dgvSaatler.RowHeadersWidth = 51;
            dgvSaatler.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSaatler.Size = new Size(953, 349);
            dgvSaatler.TabIndex = 0;
            dgvSaatler.CellContentClick += dgvSaatler_CellContentClick;
            dgvSaatler.CellDoubleClick += dgvSaatler_CellDoubleClick;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(501, 39);
            label2.Name = "label2";
            label2.Size = new Size(26, 20);
            label2.TabIndex = 4;
            label2.Text = "Yıl";
            // 
            // cmbPolikinlik
            // 
            cmbPolikinlik.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPolikinlik.FormattingEnabled = true;
            cmbPolikinlik.Location = new Point(97, 35);
            cmbPolikinlik.Name = "cmbPolikinlik";
            cmbPolikinlik.Size = new Size(151, 28);
            cmbPolikinlik.TabIndex = 5;
            cmbPolikinlik.SelectedIndexChanged += cmbPolikinlik_SelectedIndexChanged;
            cmbPolikinlik.SelectionChangeCommitted += cmbPolikinlik_SelectionChangeCommitted;
            // 
            // cmbDoktor
            // 
            cmbDoktor.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDoktor.FormattingEnabled = true;
            cmbDoktor.Location = new Point(324, 35);
            cmbDoktor.Name = "cmbDoktor";
            cmbDoktor.Size = new Size(151, 28);
            cmbDoktor.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(14, 39);
            label3.Name = "label3";
            label3.Size = new Size(72, 20);
            label3.TabIndex = 7;
            label3.Text = "Poliklinik";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(263, 39);
            label4.Name = "label4";
            label4.Size = new Size(58, 20);
            label4.TabIndex = 8;
            label4.Text = "Doktor";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dateTimePicker1);
            groupBox1.Controls.Add(btnAra);
            groupBox1.Controls.Add(cmbDoktor);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(cmbPolikinlik);
            groupBox1.Dock = DockStyle.Top;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(959, 85);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            groupBox1.Text = "Arama Kriterleri";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(542, 36);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(249, 27);
            dateTimePicker1.TabIndex = 10;
            // 
            // btnAra
            // 
            btnAra.BackColor = Color.BlueViolet;
            btnAra.FlatStyle = FlatStyle.Flat;
            btnAra.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnAra.ForeColor = Color.White;
            btnAra.Location = new Point(807, 36);
            btnAra.Name = "btnAra";
            btnAra.Size = new Size(94, 29);
            btnAra.TabIndex = 9;
            btnAra.Text = "Ara";
            btnAra.UseVisualStyleBackColor = false;
            btnAra.Click += btnAra_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dgvSaatler);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(0, 85);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(959, 375);
            groupBox2.TabIndex = 10;
            groupBox2.TabStop = false;
            groupBox2.Text = "Liste";
            // 
            // randevuekranı
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(959, 460);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            MaximizeBox = false;
            Name = "randevuekranı";
            Text = "randevuekranı";
            Load += randevuekranı_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSaatler).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvSaatler;
        private ComboBox comboBox1;
        private Label label1;
        private Label label2;
        private ComboBox cmbPolikinlik;
        private ComboBox cmbDoktor;
        private Label label3;
        private Label label4;
        private GroupBox groupBox1;
        private Button btnAra;
        private GroupBox groupBox2;
        private DateTimePicker dateTimePicker1;
    }
}