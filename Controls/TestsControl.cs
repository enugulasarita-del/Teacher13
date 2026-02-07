using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Drawing.Drawing2D;

namespace TeacherDashboard.Controls
{
    public partial class TestsControl : UserControl
    {
        // Theme Colors
        private Color primaryColor = Color.FromArgb(173, 22, 37); // VSIT Red
        private Color bgColor = Color.White;
        private Color cardBg = Color.White;
        private Color borderColor = Color.FromArgb(220, 220, 220);

        // UI Components
        private DataGridView dgvInvigilation;
        private DataGridView dgvPaperCorrection;
        private DataTable dtInvigilation, dtCorrection;
        private Label lblTotalDuties, lblPapersSet, lblToCorrect;

        public TestsControl()
        {
            InitializeComponent();
            SetupData();
            SetupLayout();
        }

        private void SetupData()
        {
            // 1. Invigilation Duties
            dtInvigilation = new DataTable();
            dtInvigilation.Columns.Add("Date");
            dtInvigilation.Columns.Add("Room/Lab");
            dtInvigilation.Columns.Add("Session");
            dtInvigilation.Columns.Add("Duty Role");
            dtInvigilation.Columns.Add("Reporting");

            dtInvigilation.Rows.Add("10 Feb 2026", "Room 302", "Morning", "Senior Invigilator", "08:30 AM");
            dtInvigilation.Rows.Add("12 Feb 2026", "Lab 101", "Afternoon", "Internal Supervisor", "01:00 PM");
            dtInvigilation.Rows.Add("15 Feb 2026", "Audit Hall", "Morning", "Invigilator", "08:30 AM");

            // 2. Paper Correction / Moderation Tasks
            dtCorrection = new DataTable();
            dtCorrection.Columns.Add("Subject Code");
            dtCorrection.Columns.Add("Subject Name");
            dtCorrection.Columns.Add("Total Papers");
            dtCorrection.Columns.Add("Completed");
            dtCorrection.Columns.Add("Deadline");
            dtCorrection.Columns.Add("Task Type");

            dtCorrection.Rows.Add("IT-401", "Information Security", "65", "40", "20 Feb", "Primary Evaluator");
            dtCorrection.Rows.Add("CS-202", "C++ Programming", "58", "0", "22 Feb", "Moderator");
            dtCorrection.Rows.Add("BS-105", "Management Science", "120", "110", "18 Feb", "Primary Evaluator");
        }

        private void SetupLayout()
        {
            this.Controls.Clear();
            this.BackColor = bgColor;
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 10);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 75, BackColor = Color.White };
            Label lblTitle = new Label() { Text = "👨‍🏫 TEACHER EXAMINATION PORTFOLIO", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.FromArgb(173, 22, 37), Location = new Point(30, 20), AutoSize = true };
            pnlHeader.Controls.Add(lblTitle);
            Panel pnlAccent = new Panel() { Dock = DockStyle.Bottom, Height = 3, BackColor = primaryColor };
            pnlHeader.Controls.Add(pnlAccent);
            this.Controls.Add(pnlHeader);

            // 2. Scrollable Container
            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25, 30, 25, 25) };
            this.Controls.Add(pnlScroll);

            FlowLayoutPanel flpMain = new FlowLayoutPanel() { FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = true, Dock = DockStyle.Top };
            pnlScroll.Controls.Add(flpMain);

            // --- SECTION: SUMMARY STATS ---
            flpMain.Controls.Add(CreateSectionHeader("EXAM DUTY SUMMARY & ACCOUNTABILITY"));
            TableLayoutPanel tlpStats = new TableLayoutPanel() { Width = 1000, Height = 130, ColumnCount = 3, Margin = new Padding(0, 0, 0, 30) };
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));

            lblTotalDuties = CreateValLabel("03", Color.FromArgb(52, 152, 219));
            lblPapersSet = CreateValLabel("02", Color.FromArgb(241, 196, 15));
            lblToCorrect = CreateValLabel("150+", Color.FromArgb(46, 204, 113));

            tlpStats.Controls.Add(CreateStatCard("INVIGILATION DUTIES", lblTotalDuties, Color.FromArgb(52, 152, 219)), 0, 0);
            tlpStats.Controls.Add(CreateStatCard("PAPER SETTING TASKS", lblPapersSet, Color.FromArgb(241, 196, 15)), 1, 0);
            tlpStats.Controls.Add(CreateStatCard("TOTAL BUNDLES TO EVALUATE", lblToCorrect, Color.FromArgb(46, 204, 113)), 2, 0);
            flpMain.Controls.Add(tlpStats);

            // --- SECTION: DETAILED ROLES ---
            flpMain.Controls.Add(CreateSectionHeader("MY DESIGNATED ROLES & RESPONSIBILITIES"));
            TableLayoutPanel tlpRoles = new TableLayoutPanel() { Width = 1000, Height = 100, ColumnCount = 2, Margin = new Padding(0, 0, 0, 30) };
            tlpRoles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tlpRoles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            tlpRoles.Controls.Add(CreateRoleBar("PRIMARY RESPONSIBILITY", "Paper Setter & Head Evaluator (B.Sc IT)", primaryColor), 0, 0);
            tlpRoles.Controls.Add(CreateRoleBar("SECONDARY RESPONSIBILITY", "Senior Supervisor (Main Hall)", Color.FromArgb(60, 60, 60)), 1, 0);
            flpMain.Controls.Add(tlpRoles);

            // --- SECTION: INVIGILATION SCHEDULE ---
            flpMain.Controls.Add(CreateSectionHeader("MY INVIGILATION / SUPERVISION SCHEDULE"));
            Panel pnlGrid1 = new Panel() { Width = 1000, Height = 220, BackColor = cardBg, Padding = new Padding(1), Margin = new Padding(0, 0, 0, 35) };
            dgvInvigilation = CreateStyledGrid(dtInvigilation);
            pnlGrid1.Controls.Add(dgvInvigilation);
            flpMain.Controls.Add(pnlGrid1);

            // --- SECTION: PAPER EVALUATION ---
            flpMain.Controls.Add(CreateSectionHeader("PAPER CORRECTION & MODERATION TRACKER"));
            Panel pnlGrid2 = new Panel() { Width = 1000, Height = 220, BackColor = cardBg, Padding = new Padding(1), Margin = new Padding(0, 0, 0, 35) };
            dgvPaperCorrection = CreateStyledGrid(dtCorrection);
            pnlGrid2.Controls.Add(dgvPaperCorrection);
            flpMain.Controls.Add(pnlGrid2);

            // ACTION BAR
            flpMain.Controls.Add(CreateSectionHeader("QUICK ACCESS PORTALS"));
            FlowLayoutPanel flpActions = new FlowLayoutPanel() { Width = 1000, Height = 60, FlowDirection = FlowDirection.LeftToRight };
            flpActions.Controls.Add(CreateActionButton("📜 Download Duty Slip", Color.FromArgb(41, 128, 185)));
            flpActions.Controls.Add(CreateActionButton("📤 Submit Question Paper", Color.FromArgb(39, 174, 96)));
            flpActions.Controls.Add(CreateActionButton("✍️ Mark Online Attendance", Color.FromArgb(230, 126, 34)));
            flpMain.Controls.Add(flpActions);

            // Dynamic Resizing
            pnlScroll.Resize += (s, e) => {
                int w = Math.Max(800, pnlScroll.Width - 70);
                flpMain.Width = pnlScroll.Width;
                foreach (Control c in flpMain.Controls) if (c is Panel || c is TableLayoutPanel || c is FlowLayoutPanel) c.Width = w;
            };
        }

        private Panel CreateRoleBar(string head, string val, Color accent)
        {
            Panel p = new Panel() { Dock = DockStyle.Fill, BackColor = cardBg, Margin = new Padding(0, 0, 15, 0), Padding = new Padding(15, 10, 15, 10) };
            p.Paint += (s, e) => DrawBorder(e.Graphics, p.ClientRectangle);
            Label l1 = new Label() { Text = head, ForeColor = accent, Font = new Font("Segoe UI", 8, FontStyle.Bold), Dock = DockStyle.Top, AutoSize = true };
            Label l2 = new Label() { Text = val, ForeColor = Color.FromArgb(40, 40, 40), Font = new Font("Segoe UI", 11, FontStyle.Bold), Dock = DockStyle.Top, Padding = new Padding(0, 5, 0, 0), AutoSize = true };
            p.Controls.Add(l2); p.Controls.Add(l1);
            return p;
        }

        private Panel CreateStatCard(string title, Label val, Color accent) {
            Panel p = new Panel() { Dock = DockStyle.Fill, BackColor = cardBg, Margin = new Padding(0, 0, 15, 0) };
            p.Paint += (s, e) => DrawBorder(e.Graphics, p.ClientRectangle);
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label head = new Label() { Text = title, ForeColor = Color.Gray, Font = new Font("Segoe UI", 8, FontStyle.Bold), Location = new Point(15, 12), AutoSize = true };
            val.Location = new Point(15, 38); p.Controls.AddRange(new Control[] { l, head, val });
            return p;
        }

        private Label CreateValLabel(string text, Color c) => new Label() { Text = text, ForeColor = c, Font = new Font("Segoe UI", 20, FontStyle.Bold), AutoSize = true };

        private Button CreateActionButton(string text, Color c) {
            Button b = new Button() { Text = text, Width = 230, Height = 45, BackColor = c, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Margin = new Padding(0, 0, 15, 0), Cursor = Cursors.Hand };
            b.FlatAppearance.BorderSize = 0; return b;
        }

        private DataGridView CreateStyledGrid(DataTable dt) {
            DataGridView d = new DataGridView() { 
                Dock = DockStyle.Fill, 
                DataSource = dt, 
                BackgroundColor = Color.White, 
                BorderStyle = BorderStyle.None, 
                ForeColor = Color.FromArgb(40, 40, 40), 
                GridColor = Color.FromArgb(220, 220, 220), 
                RowTemplate = { Height = 40 }, 
                ColumnHeadersHeight = 45, 
                AllowUserToAddRows = false, 
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = false,
                ReadOnly = true, // FIXED: Now it's a Read-Only Schedule, not an editable table
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, 
                EnableHeadersVisualStyles = false, 
                RowHeadersVisible = false, 
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            
            d.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(173, 22, 37); 
            d.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; 
            d.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            d.DefaultCellStyle.BackColor = Color.White; 
            d.DefaultCellStyle.SelectionBackColor = Color.FromArgb(173, 22, 37); 
            d.DefaultCellStyle.SelectionForeColor = Color.White;
            
            return d;
        }

        private Label CreateSectionHeader(string text) => new Label() { Text = "──  " + text, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = primaryColor, AutoSize = true, Margin = new Padding(0, 10, 0, 15) };

        private void DrawBorder(Graphics g, Rectangle r) {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen pen = new Pen(borderColor, 1)) g.DrawRectangle(pen, r.X, r.Y, r.Width - 1, r.Height - 1);
        }

        private void InitializeComponent() {
            this.SuspendLayout();
            this.Name = "TestsControl";
            this.Size = new Size(1100, 1100);
            this.ResumeLayout(false);
        }
    }
}
