using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Drawing.Drawing2D;

namespace TeacherDashboard.Controls
{
    public partial class ReportsControl : UserControl
    {
        // Theme Colors
        private Color primaryColor = Color.FromArgb(173, 22, 37); // VSIT Red
        private Color bgColor = Color.FromArgb(18, 18, 18);
        private Color cardBg = Color.FromArgb(30, 30, 33);
        private Color borderColor = Color.FromArgb(45, 45, 48);

        // UI Components
        private ComboBox cmbCategory, cmbDept, cmbSem, cmbDiv;
        private DateTimePicker dtpStart, dtpEnd;
        private DataGridView dgvReports;
        private Label lblTotalReports, lblPendingAudits, lblStudentFlags;
        
        // Data Store
        private DataTable dtReports;
        private Random rnd = new Random();

        public ReportsControl()
        {
            InitializeComponent();
            SetupData();
            SetupLayout();
        }

        private void SetupData()
        {
            dtReports = new DataTable();
            dtReports.Columns.Add("REF #");
            dtReports.Columns.Add("REPORT NAME");
            dtReports.Columns.Add("CATEGORY");
            dtReports.Columns.Add("DATE GENERATED");
            dtReports.Columns.Add("FILE STATUS");

            // Initial Dummy Data
            dtReports.Rows.Add("REP-001", "Attendance_Monthly_Jan", "Attendance", DateTime.Now.AddDays(-5).ToShortDateString(), "✅ Archived");
            dtReports.Rows.Add("REP-002", "Performance_Unit_Test_1", "Academic", DateTime.Now.AddDays(-10).ToShortDateString(), "✅ Archived");
            dtReports.Rows.Add("REP-003", "Syllabus_Audit_BScIT", "Syllabus Audit", DateTime.Now.AddDays(-2).ToShortDateString(), "⚠️ Pending");
        }

        private void SetupLayout()
        {
            this.Controls.Clear();
            this.BackColor = bgColor;
            this.Dock = DockStyle.Fill;

            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 75, BackColor = Color.FromArgb(25, 25, 25) };
            Label lblTitle = new Label() { Text = "📊  REPORTS & INSIGHTS ENGINE", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, Location = new Point(30, 20), AutoSize = true };
            pnlHeader.Controls.Add(lblTitle);
            Panel pnlAccent = new Panel() { Dock = DockStyle.Bottom, Height = 3, BackColor = primaryColor };
            pnlHeader.Controls.Add(pnlAccent);
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            FlowLayoutPanel flpMain = new FlowLayoutPanel() { FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = true, Dock = DockStyle.Top };
            pnlScroll.Controls.Add(flpMain);

            // 1. STATS CARDS
            flpMain.Controls.Add(CreateSectionLabel("SYSTEM OVERVIEW"));
            TableLayoutPanel tlpStats = new TableLayoutPanel() { Width = 1000, Height = 100, ColumnCount = 4, Margin = new Padding(0, 0, 0, 20) };
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));

            lblTotalReports = new Label() { Text = "0", ForeColor = Color.White, Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize = true };
            lblPendingAudits = new Label() { Text = "1", ForeColor = Color.White, Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize = true };
            lblStudentFlags = new Label() { Text = "12", ForeColor = Color.White, Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize = true };
            
            tlpStats.Controls.Add(CreateStatCard("TOTAL GENERATED", lblTotalReports, Color.FromArgb(52, 152, 219)), 0, 0);
            tlpStats.Controls.Add(CreateStatCard("PENDING SYNC", lblPendingAudits, Color.FromArgb(241, 196, 15)), 1, 0);
            tlpStats.Controls.Add(CreateStatCard("STUDENT FLAGS", lblStudentFlags, Color.FromArgb(231, 76, 60)), 2, 0);
            tlpStats.Controls.Add(CreateStatCard("SYSTEM STATUS", new Label() { Text = "ONLINE", ForeColor = Color.FromArgb(46, 204, 113), Font = new Font("Segoe UI", 12, FontStyle.Bold), AutoSize = true }, Color.FromArgb(46, 204, 113)), 3, 0);
            flpMain.Controls.Add(tlpStats);

            // 2. REPORT GENERATOR
            flpMain.Controls.Add(CreateSectionLabel("REPORT BUILDER TOOL"));
            Panel pnlBuilder = new Panel() { Width = 1000, Height = 260, BackColor = cardBg, Padding = new Padding(25), Margin = new Padding(0, 0, 0, 20) };
            pnlBuilder.Paint += (s, e) => DrawBorder(e.Graphics, pnlBuilder.ClientRectangle);
            
            TableLayoutPanel tlpFields = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 3 };
            tlpFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            tlpFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            tlpFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));

            cmbCategory = AddDropdown(tlpFields, "Report Category", new string[] { "Attendance", "Academic Performance", "Syllabus Audit", "Behavioral Log" }, 0, 0);
            cmbDept = AddDropdown(tlpFields, "Department", new string[] { "All", "B.Sc IT", "B.Sc CS", "BMS", "B.Com" }, 1, 0);
            cmbSem = AddDropdown(tlpFields, "Semester", new string[] { "Sem I", "Sem II", "Sem III", "Sem IV", "Sem V", "Sem VI" }, 2, 0);
            cmbDiv = AddDropdown(tlpFields, "Division", new string[] { "All Divisions", "Div A", "Div B", "Div C" }, 0, 1);

            Panel pnlDates = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(0, 5, 20, 0) };
            Label lblD = new Label() { Text = "Date Range", ForeColor = Color.Gray, Font = new Font("Segoe UI", 8, FontStyle.Bold), Dock = DockStyle.Top };
            dtpStart = new DateTimePicker() { Width = 135, Format = DateTimePickerFormat.Short, Location = new Point(0, 25) };
            dtpEnd = new DateTimePicker() { Width = 135, Format = DateTimePickerFormat.Short, Location = new Point(145, 25) };
            pnlDates.Controls.AddRange(new Control[] { lblD, dtpStart, dtpEnd });
            tlpFields.Controls.Add(pnlDates, 1, 1);

            Panel pnlActions = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(0, 20, 20, 0) };
            Button btnGenerate = new Button() { Text = "🚀  GENERATE NOW", Size = new Size(150, 45), BackColor = primaryColor, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand };
            btnGenerate.Click += (s, e) => GenerateNewReport();
            
            Button btnClear = new Button() { Text = "🔄 RESET", Size = new Size(120, 45), Location = new Point(160, 0), BackColor = Color.FromArgb(60, 60, 60), ForeColor = Color.Silver, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            pnlActions.Controls.AddRange(new Control[] { btnGenerate, btnClear });
            tlpFields.Controls.Add(pnlActions, 2, 1);

            pnlBuilder.Controls.Add(tlpFields);
            flpMain.Controls.Add(pnlBuilder);

            // 3. RECENT REPORTS TABLE
            flpMain.Controls.Add(CreateSectionLabel("RECENTLY GENERATED REPORTS"));
            Panel pnlGridWrap = new Panel() { Width = 1000, Height = 350, BackColor = cardBg, Padding = new Padding(1) };
            dgvReports = new DataGridView() { 
                Dock = DockStyle.Fill, 
                DataSource = dtReports,
                BackgroundColor = Color.FromArgb(30, 30, 30), 
                BorderStyle = BorderStyle.None,
                ForeColor = Color.White,
                GridColor = Color.FromArgb(50, 50, 50),
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 40 },
                ColumnHeadersHeight = 45,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                EnableHeadersVisualStyles = false
            };
            dgvReports.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgvReports.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReports.DefaultCellStyle.BackColor = Color.FromArgb(32, 33, 36);
            dgvReports.DefaultCellStyle.SelectionBackColor = primaryColor;

            pnlGridWrap.Controls.Add(dgvReports);
            flpMain.Controls.Add(pnlGridWrap);

            UpdateStatsUI();

            this.Resize += (s, e) => {
                int w = this.Width - 80;
                flpMain.Width = this.Width;
                tlpStats.Width = w;
                pnlBuilder.Width = w;
                pnlGridWrap.Width = w;
            };
        }

        private void GenerateNewReport()
        {
            string refId = "REP-" + rnd.Next(100, 999).ToString();
            string name = cmbCategory.Text + "_" + cmbSem.Text + "_" + DateTime.Now.ToString("MMM");
            string cat = cmbCategory.Text;
            string date = DateTime.Now.ToShortDateString();
            string status = rnd.Next(0, 2) == 0 ? "✅ Archived" : "⚠️ Pending";

            dtReports.Rows.InsertAt(dtReports.NewRow(), 0);
            dtReports.Rows[0][0] = refId;
            dtReports.Rows[0][1] = name;
            dtReports.Rows[0][2] = cat;
            dtReports.Rows[0][3] = date;
            dtReports.Rows[0][4] = status;

            UpdateStatsUI();
            MessageBox.Show($"New '{cat}' report generated successfully for {cmbDept.Text} ({cmbSem.Text}).", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateStatsUI()
        {
            // 1. Total Reports
            lblTotalReports.Text = dtReports.Rows.Count.ToString();

            // 2. Pending Audits (Calculated from Status column)
            int pending = 0;
            foreach (DataRow row in dtReports.Rows) {
                if (row["FILE STATUS"].ToString().Contains("Pending")) pending++;
            }
            lblPendingAudits.Text = pending.ToString();

            // 3. Student Flags (Dynamic Mock Logic)
            // Let's assume Academic Performance reports indicate flagging needs
            int performanceReports = dtReports.AsEnumerable().Count(r => r.Field<string>("CATEGORY") == "Academic Performance");
            int flags = (performanceReports * 4) + rnd.Next(2, 6); 
            lblStudentFlags.Text = flags.ToString();

            // 4. Status Auto-Styling
            lblPendingAudits.ForeColor = pending > 0 ? Color.FromArgb(241, 196, 15) : Color.White;
            lblStudentFlags.ForeColor = flags > 10 ? Color.FromArgb(231, 76, 60) : Color.White;
        }

        private ComboBox AddDropdown(TableLayoutPanel p, string label, string[] items, int col, int row)
        {
            Panel wrap = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(0, 5, 20, 0) };
            Label lbl = new Label() { Text = label, ForeColor = Color.Gray, Font = new Font("Segoe UI", 8, FontStyle.Bold), Dock = DockStyle.Top };
            ComboBox cb = new ComboBox() { Dock = DockStyle.Top, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(45, 45, 48), ForeColor = Color.White, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cb.Items.AddRange(items);
            cb.SelectedIndex = 0;
            wrap.Controls.AddRange(new Control[] { cb, lbl });
            p.Controls.Add(wrap, col, row);
            return cb;
        }

        private Panel CreateStatCard(string title, Control valCtrl, Color accent)
        {
            Panel p = new Panel() { BackColor = cardBg, Dock = DockStyle.Fill, Margin = new Padding(0, 0, 15, 0) };
            p.Paint += (s, e) => DrawBorder(e.Graphics, p.ClientRectangle);
            Panel line = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label head = new Label() { Text = title, ForeColor = Color.Gray, Font = new Font("Segoe UI", 8, FontStyle.Bold), Location = new Point(15, 12), AutoSize = true };
            valCtrl.Location = new Point(15, 30);
            p.Controls.AddRange(new Control[] { line, head, valCtrl });
            return p;
        }

        private Label CreateSectionLabel(string text)
        {
            return new Label() { Text = "──  " + text, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = primaryColor, AutoSize = true, Margin = new Padding(0, 20, 0, 15) };
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
            this.Name = "ReportsControl";
            this.Size = new Size(1100, 800);
            this.ResumeLayout(false);
        }
    }
}
