namespace hastaneotomasyon
{
    partial class Hastane_Otomasyonu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Hastane_Otomasyonu));
            menuStrip1 = new MenuStrip();
            anaSayfaToolStripMenuItem = new ToolStripMenuItem();
            hastaİşlemleriToolStripMenuItem = new ToolStripMenuItem();
            doktorİşlemleriToolStripMenuItem = new ToolStripMenuItem();
            mnuRandevuIslemleri = new ToolStripMenuItem();
            polikinlikİşlemleriToolStripMenuItem = new ToolStripMenuItem();
            adminİşlemleriToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { anaSayfaToolStripMenuItem, hastaİşlemleriToolStripMenuItem, doktorİşlemleriToolStripMenuItem, mnuRandevuIslemleri, polikinlikİşlemleriToolStripMenuItem, adminİşlemleriToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(905, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // anaSayfaToolStripMenuItem
            // 
            anaSayfaToolStripMenuItem.Name = "anaSayfaToolStripMenuItem";
            anaSayfaToolStripMenuItem.Size = new Size(89, 24);
            anaSayfaToolStripMenuItem.Text = "Ana Sayfa";
            // 
            // hastaİşlemleriToolStripMenuItem
            // 
            hastaİşlemleriToolStripMenuItem.Name = "hastaİşlemleriToolStripMenuItem";
            hastaİşlemleriToolStripMenuItem.Size = new Size(121, 24);
            hastaİşlemleriToolStripMenuItem.Text = "Hasta İşlemleri";
            hastaİşlemleriToolStripMenuItem.Click += hastaİşlemleriToolStripMenuItem_Click;
            // 
            // doktorİşlemleriToolStripMenuItem
            // 
            doktorİşlemleriToolStripMenuItem.Name = "doktorİşlemleriToolStripMenuItem";
            doktorİşlemleriToolStripMenuItem.Size = new Size(129, 24);
            doktorİşlemleriToolStripMenuItem.Text = "Doktor İşlemleri";
            doktorİşlemleriToolStripMenuItem.Click += doktorİşlemleriToolStripMenuItem_Click;
            // 
            // mnuRandevuIslemleri
            // 
            mnuRandevuIslemleri.Name = "mnuRandevuIslemleri";
            mnuRandevuIslemleri.Size = new Size(140, 24);
            mnuRandevuIslemleri.Text = "Randevu İşlemleri";
            mnuRandevuIslemleri.Click += mnuRandevuIslemleri_Click;
            // 
            // polikinlikİşlemleriToolStripMenuItem
            // 
            polikinlikİşlemleriToolStripMenuItem.Name = "polikinlikİşlemleriToolStripMenuItem";
            polikinlikİşlemleriToolStripMenuItem.Size = new Size(141, 24);
            polikinlikİşlemleriToolStripMenuItem.Text = "Polikinlik İşlemleri";
            polikinlikİşlemleriToolStripMenuItem.Click += polikinlikİşlemleriToolStripMenuItem_Click;
            // 
            // adminİşlemleriToolStripMenuItem
            // 
            adminİşlemleriToolStripMenuItem.Name = "adminİşlemleriToolStripMenuItem";
            adminİşlemleriToolStripMenuItem.Size = new Size(137, 24);
            adminİşlemleriToolStripMenuItem.Text = "Yönetim İşlemleri";
            adminİşlemleriToolStripMenuItem.Click += adminİşlemleriToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 465);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(905, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            statusStrip1.ItemClicked += statusStrip1_ItemClicked;
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(0, 16);
            toolStripStatusLabel1.Click += toolStripStatusLabel1_Click;
            // 
            // Hastane_Otomasyonu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(905, 487);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "Hastane_Otomasyonu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hastane Otomasyonu";
            WindowState = FormWindowState.Maximized;
            Load += Hastane_Otomasyonu_Load;
            Shown += Hastane_Otomasyonu_Shown;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem anaSayfaToolStripMenuItem;
        private ToolStripMenuItem hastaİşlemleriToolStripMenuItem;
        private ToolStripMenuItem doktorİşlemleriToolStripMenuItem;
        private ToolStripMenuItem mnuRandevuIslemleri;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripMenuItem polikinlikİşlemleriToolStripMenuItem;
        private ToolStripMenuItem adminİşlemleriToolStripMenuItem;
    }
}