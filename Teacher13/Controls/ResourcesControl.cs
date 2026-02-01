using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TeacherDashboard.Controls
{
    public partial class ResourcesControl : UserControl
    {
        public ResourcesControl()
        {
            InitializeComponent();
            SetupStrictLayout();
            LoadResources();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblMain = new Label() { Text = "RESOURCE LIBRARY", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblMain);
            this.Controls.Add(pnlHeader);

            // 2. Toolbar
            Panel pnlToolbar = new Panel() { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(20, 10, 20, 10) };
            Button btnUploadNew = new Button() { 
                Text = "📤 UPLOAD NEW FILE", 
                Location = new Point(20, 12), 
                Size = new Size(180, 35), 
                BackColor = Color.FromArgb(173, 22, 37), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnUploadNew.FlatAppearance.BorderSize = 0;
            pnlToolbar.Controls.Add(btnUploadNew);
            this.Controls.Add(pnlToolbar);

            // 3. Content Splitter
            SplitContainer split = new SplitContainer() { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, SplitterDistance = 350, Padding = new Padding(20) };
            this.Controls.Add(split);

            this.dgvResources = new DataGridView() { 
                Dock = DockStyle.Fill, 
                BackgroundColor = Color.FromArgb(28, 28, 28), 
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 40,
                AllowUserToAddRows = false,
                GridColor = Color.FromArgb(45, 45, 45)
            };
            this.dgvResources.DefaultCellStyle.BackColor = Color.White;
            this.dgvResources.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            this.dgvResources.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            split.Panel1.Controls.Add(this.dgvResources);

            // 4. Storage Analytics (Replacing Chart)
            Panel pnlStats = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(18, 18, 18), Padding = new Padding(20) };
            Label lblStatTitle = new Label() { Text = "STORAGE & FILE INSIGHTS", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlStats.Controls.Add(lblStatTitle);

            FlowLayoutPanel flpResStats = new FlowLayoutPanel() { Dock = DockStyle.Fill };
            flpResStats.Controls.Add(CreateResBox("Total Files", "154", "Across 12 classes", Color.FromArgb(173, 22, 37)));
            flpResStats.Controls.Add(CreateResBox("Recent Uploads", "08", "In the last 7 days", Color.FromArgb(46, 204, 113)));
            flpResStats.Controls.Add(CreateResBox("Storage Used", "4.2 GB", "Of 10 GB limit", Color.FromArgb(52, 152, 219)));
            flpResStats.Controls.Add(CreateResBox("Dormant Files", "12", "Not accessed in 6 mo", Color.FromArgb(241, 196, 15)));

            pnlStats.Controls.Add(flpResStats);
            split.Panel2.Controls.Add(pnlStats);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 2);  // Docks First (Top)
            this.Controls.SetChildIndex(pnlToolbar, 1); // Docks Second (Top)
            this.Controls.SetChildIndex(split, 0);      // Docks Last (Fill)
        }

        private Panel CreateResBox(string title, string val, string sub, Color accent)
        {
            Panel p = new Panel() { Size = new Size(220, 100), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(15, 15), AutoSize = true };
            Label lblV = new Label() { Text = val, Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 38), AutoSize = true };
            Label lblS = new Label() { Text = sub, Font = new Font("Segoe UI", 8, FontStyle.Italic), ForeColor = Color.DimGray, Location = new Point(15, 72), Size = new Size(190, 30) };
            p.Controls.AddRange(new Control[] { l, lblT, lblV, lblS });
            return p;
        }

        private void LoadResources()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Type");
            dt.Columns.Add("File Name");
            dt.Columns.Add("Subject");

            dt.Rows.Add("PDF", "C Programming Notes.pdf", "Science");
            dt.Rows.Add("PPTX", "Cloud Basics.pptx", "IT");
            dt.Rows.Add("ZIP", "Sample Projects.zip", "IT");

            if (this.dgvResources != null)
            {
                this.dgvResources.DataSource = dt;
                this.dgvResources.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }
    }
}
