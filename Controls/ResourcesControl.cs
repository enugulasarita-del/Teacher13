using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Drawing.Drawing2D;

namespace TeacherDashboard.Controls
{
    public partial class ResourcesControl : UserControl
    {
        // Theme Colors
        private Color primaryColor = Color.FromArgb(173, 22, 37); // VSIT Red
        private Color bgColor = Color.White;
        private Color cardBg = Color.White;
        private Color borderColor = Color.FromArgb(220, 220, 220);
        private Color textColor = Color.RoyalBlue;

        // UI Components
        private ComboBox cmbDept, cmbDiv, cmbType;
        private TextBox txtTitle, txtLink;
        private DataGridView dgvResources;
        private DataTable dtResources;

        public ResourcesControl()
        {
            InitializeComponent();
            SetupData();
            SetupLayout();
        }

        private void SetupData()
        {
            dtResources = new DataTable();
            dtResources.Columns.Add("Type");
            dtResources.Columns.Add("Title");
            dtResources.Columns.Add("Department");
            dtResources.Columns.Add("Division");
            dtResources.Columns.Add("Date Shared");

            // Dummy Data
            dtResources.Rows.Add("📄 Note", "Java Exception Handling.pdf", "B.Sc IT", "Div A", "02/02/2026");
            dtResources.Rows.Add("📊 PPT", "Database Normalization.pptx", "B.Sc CS", "Div B", "01/02/2026");
            dtResources.Rows.Add("📄 Note", "Discrete Mathematics - Set Theory", "All", "All", "30/01/2026");
            dtResources.Rows.Add("📊 PPT", "Computer Networking Overview", "B.Sc IT", "Div C", "28/01/2026");
        }

        private void SetupLayout()
        {
            this.Controls.Clear();
            this.BackColor = bgColor;
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 10);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 75, BackColor = Color.White };
            Label lblTitle = new Label() { Text = "📚  STUDY RESOURCES & CONTENT", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = primaryColor, Location = new Point(30, 20), AutoSize = true };
            pnlHeader.Controls.Add(lblTitle);
            Panel pnlAccent = new Panel() { Dock = DockStyle.Bottom, Height = 3, BackColor = primaryColor };
            pnlHeader.Controls.Add(pnlAccent);
            this.Controls.Add(pnlHeader);

            // 2. Scrollable Container
            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            FlowLayoutPanel flpMain = new FlowLayoutPanel() { FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = true, Dock = DockStyle.Top };
            pnlScroll.Controls.Add(flpMain);

            // --- SECTION: UPLOAD / SHARE ---
            flpMain.Controls.Add(CreateSectionHeader("SHARE NEW RESOURCE (PPT / NOTES)"));
            Panel pnlUpload = new Panel() { Width = 1000, Height = 220, BackColor = cardBg, Padding = new Padding(20), Margin = new Padding(0, 0, 0, 30) };
            pnlUpload.Paint += (s, e) => DrawBorder(e.Graphics, pnlUpload.ClientRectangle);
            
            TableLayoutPanel tlpEntry = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 2 };
            tlpEntry.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpEntry.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpEntry.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpEntry.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));

            txtTitle = AddTextbox(tlpEntry, "Resource Title / Subject", 0, 0, 2);
            cmbType = AddDropdown(tlpEntry, "Content Type", new string[] { "📊 PPT", "📄 Note", "🔗 Link", "📁 Other" }, 2, 0);
            
            cmbDept = AddDropdown(tlpEntry, "Target Department", new string[] { "All", "B.Sc IT", "B.Sc CS", "BMS", "B.Com" }, 0, 1);
            cmbDiv = AddDropdown(tlpEntry, "Target Division", new string[] { "All", "Div A", "Div B", "Div C" }, 1, 1);
            
            txtLink = AddTextbox(tlpEntry, "Upload Link / File Path", 2, 1, 1);

            Button btnShare = new Button() { 
                Text = "📤  SHARE RESOURCE", 
                Dock = DockStyle.Fill, 
                BackColor = primaryColor, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(10, 20, 0, 10)
            };
            btnShare.FlatAppearance.BorderSize = 0;
            btnShare.Click += (s, e) => ShareNewResource();
            tlpEntry.Controls.Add(btnShare, 3, 1);

            pnlUpload.Controls.Add(tlpEntry);
            flpMain.Controls.Add(pnlUpload);

            // --- SECTION: RESOURCE REPOSITORY ---
            flpMain.Controls.Add(CreateSectionHeader("REPOSITORY (RECENTLY SHARED)"));
            
            // Grid Filters
            Panel pnlGridFilters = new Panel() { Width = 1000, Height = 60, Margin = new Padding(0, 0, 0, 10) };
            Label lblF = new Label() { Text = "FILTER BY:", ForeColor = primaryColor, Font = new Font("Segoe UI", 8, FontStyle.Bold), Location = new Point(0, 2), AutoSize = true };
            
            ComboBox fDept = new ComboBox() { Name = "fDept", Location = new Point(0, 22), Width = 180, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.RoyalBlue, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            fDept.Items.AddRange(new string[] { "All Departments", "B.Sc IT", "B.Sc CS", "BMS", "B.Com" });
            fDept.SelectedIndex = 0;
            
            ComboBox fDiv = new ComboBox() { Name = "fDiv", Location = new Point(190, 22), Width = 130, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.RoyalBlue, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            fDiv.Items.AddRange(new string[] { "All Divisions", "Div A", "Div B", "Div C" });
            fDiv.SelectedIndex = 0;

            TextBox txtGridSearch = new TextBox() { Name = "txtGridSearch", Location = new Point(330, 22), Width = 250, BackColor = Color.White, ForeColor = Color.RoyalBlue, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 11) };
            Label lblS = new Label() { Text = "SEARCH CONTENT:", ForeColor = primaryColor, Font = new Font("Segoe UI", 8, FontStyle.Bold), Location = new Point(330, 2), AutoSize = true };

            // Live Filter Logic
            EventHandler filterHandler = (s, e) => {
                string deptFilter = fDept.SelectedIndex == 0 ? "" : fDept.Text;
                string divFilter = fDiv.SelectedIndex == 0 ? "" : fDiv.Text;
                string searchFilter = txtGridSearch.Text.ToLower();

                DataView dv = dtResources.DefaultView;
                string rowFilter = "";
                if (!string.IsNullOrEmpty(deptFilter)) rowFilter += $"Department = '{deptFilter}'";
                if (!string.IsNullOrEmpty(divFilter)) {
                    if (!string.IsNullOrEmpty(rowFilter)) rowFilter += " AND ";
                    rowFilter += $"Division = '{divFilter}'";
                }
                if (!string.IsNullOrEmpty(searchFilter)) {
                    if (!string.IsNullOrEmpty(rowFilter)) rowFilter += " AND ";
                    rowFilter += $"(Title LIKE '%{searchFilter}%' OR Type LIKE '%{searchFilter}%')";
                }
                dv.RowFilter = rowFilter;
            };

            fDept.SelectedIndexChanged += filterHandler;
            fDiv.SelectedIndexChanged += filterHandler;
            txtGridSearch.TextChanged += filterHandler;

            pnlGridFilters.Controls.AddRange(new Control[] { lblF, fDept, fDiv, lblS, txtGridSearch });
            flpMain.Controls.Add(pnlGridFilters);

            Panel pnlGridWrap = new Panel() { Width = 1000, Height = 450, BackColor = cardBg, Padding = new Padding(1), Margin = new Padding(0, 0, 0, 50) };
            dgvResources = new DataGridView() { 
                Dock = DockStyle.Fill, 
                DataSource = dtResources,
                BackgroundColor = Color.White, 
                BorderStyle = BorderStyle.None,
                ForeColor = Color.RoyalBlue,
                GridColor = Color.FromArgb(220, 220, 220),
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 45 },
                ColumnHeadersHeight = 50,
                EnableHeadersVisualStyles = false
            };
            dgvResources.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            dgvResources.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvResources.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvResources.DefaultCellStyle.BackColor = Color.White;
            dgvResources.DefaultCellStyle.SelectionBackColor = primaryColor;

            pnlGridWrap.Controls.Add(dgvResources);
            flpMain.Controls.Add(pnlGridWrap);

            this.Resize += (s, e) => {
                int targetW = Math.Max(800, this.Width - 100);
                flpMain.Width = this.Width;
                pnlUpload.Width = targetW;
                pnlGridFilters.Width = targetW;
                pnlGridWrap.Width = targetW;
            };
        }

        private void ShareNewResource()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text)) {
                MessageBox.Show("Please enter a title for the resource.", "Entry Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dtResources.Rows.InsertAt(dtResources.NewRow(), 0);
            dtResources.Rows[0][0] = cmbType.Text;
            dtResources.Rows[0][1] = txtTitle.Text;
            dtResources.Rows[0][2] = cmbDept.Text;
            dtResources.Rows[0][3] = cmbDiv.Text;
            dtResources.Rows[0][4] = DateTime.Now.ToShortDateString();

            txtTitle.Clear();
            txtLink.Clear();
            MessageBox.Show("Resource shared successfully with " + cmbDept.Text + " " + cmbDiv.Text, "Content Posted", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private ComboBox AddDropdown(TableLayoutPanel p, string label, string[] items, int col, int row)
        {
            Panel wrap = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(10, 5, 10, 5) };
            Label lbl = new Label() { Text = label, ForeColor = primaryColor, Font = new Font("Segoe UI", 8, FontStyle.Bold), Dock = DockStyle.Top };
            ComboBox cb = new ComboBox() { Dock = DockStyle.Top, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.RoyalBlue, DropDownStyle = ComboBoxStyle.DropDownList };
            cb.Items.AddRange(items);
            cb.SelectedIndex = 0;
            wrap.Controls.AddRange(new Control[] { cb, lbl });
            p.Controls.Add(wrap, col, row);
            return cb;
        }

        private TextBox AddTextbox(TableLayoutPanel p, string label, int col, int row, int colSpan)
        {
            Panel wrap = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(10, 5, 10, 5) };
            Label lbl = new Label() { Text = label, ForeColor = primaryColor, Font = new Font("Segoe UI", 8, FontStyle.Bold), Dock = DockStyle.Top };
            TextBox txt = new TextBox() { Dock = DockStyle.Top, BackColor = Color.White, ForeColor = Color.RoyalBlue, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10) };
            wrap.Controls.AddRange(new Control[] { txt, lbl });
            p.Controls.Add(wrap, col, row);
            if (colSpan > 1) p.SetColumnSpan(wrap, colSpan);
            return txt;
        }

        private Label CreateSectionHeader(string text)
        {
            return new Label() { Text = "──  " + text, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = primaryColor, AutoSize = true, Margin = new Padding(0, 10, 0, 15) };
        }

        private void DrawBorder(Graphics g, Rectangle r)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen p = new Pen(borderColor, 1)) {
                g.DrawRectangle(p, r.X, r.Y, r.Width - 1, r.Height - 1);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "ResourcesControl";
            this.Size = new Size(1100, 800);
            this.ResumeLayout(false);
        }
    }
}
