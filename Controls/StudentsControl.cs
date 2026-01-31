using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TeacherDashboard.Controls
{
    public partial class StudentsControl : UserControl
    {
        public StudentsControl()
        {
            InitializeComponent();
            SetupStrictLayout();
            LoadMockStudents();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitleText = new Label() { Text = "STUDENT DIRECTORY", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitleText);
            this.Controls.Add(pnlHeader);

            // 2. Toolbar
            Panel pnlToolbar = new Panel() { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(20, 10, 20, 10) };
            Label lblS = new Label() { Text = "SEARCH:", Location = new Point(25, 22), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.White };
            TextBox txtS = new TextBox() { Location = new Point(100, 18), Width = 250, Font = new Font("Segoe UI", 10) };
            Button btnE = new Button() { Text = "📥 EXPORT DATA", Location = new Point(370, 14), Size = new Size(130, 32), BackColor = Color.FromArgb(45, 45, 45), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnE.FlatAppearance.BorderSize = 0;
            pnlToolbar.Controls.AddRange(new Control[] { lblS, txtS, btnE });
            this.Controls.Add(pnlToolbar);

            // 3. Grid (Primary Content)
            this.dgvStudents = new DataGridView() { 
                Dock = DockStyle.Top, 
                Height = 300,
                BackgroundColor = Color.FromArgb(28, 28, 28), 
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 40,
                GridColor = Color.FromArgb(45, 45, 45),
                Margin = new Padding(20)
            };
            this.dgvStudents.DefaultCellStyle.BackColor = Color.White;
            this.dgvStudents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(173, 22, 37);
            this.dgvStudents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.Controls.Add(this.dgvStudents);

            // 4. Fill bottom with stats and related features
            Panel pnlBottom = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(18, 18, 18), Padding = new Padding(25) };
            this.Controls.Add(pnlBottom);

            Label lblDist = new Label() { Text = "DEMOGRAPHICS & PERFORMANCE HIGHLIGHTS", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlBottom.Controls.Add(lblDist);

            FlowLayoutPanel flpStats = new FlowLayoutPanel() { Dock = DockStyle.Top, Height = 120, WrapContents = false };
            flpStats.Controls.Add(CreateStudentBox("Total Enrolled", "210", "Active Students", Color.FromArgb(46, 204, 113)));
            flpStats.Controls.Add(CreateStudentBox("New Admissions", "15", "Last 30 days", Color.FromArgb(52, 152, 219)));
            flpStats.Controls.Add(CreateStudentBox("At Risk", "08", "Critical Status", Color.FromArgb(173, 22, 37)));
            flpStats.Controls.Add(CreateStudentBox("Outstanding", "32", "Top Performers", Color.FromArgb(241, 196, 15)));
            pnlBottom.Controls.Add(flpStats);

            Label lblInsight = new Label() { Text = "CLASS DISTRIBUTION INSIGHTS", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.DimGray, Dock = DockStyle.Top, Height = 30, Margin = new Padding(0, 20, 0, 0) };
            pnlBottom.Controls.Add(lblInsight);

            FlowLayoutPanel flpInsights = new FlowLayoutPanel() { Dock = DockStyle.Fill, AutoScroll = true };
            string[] insights = { "FY-BSCIT: 60 Students (Capacity: 90%)", "SY-BSCIT: 55 Students (Capacity: 82%)", "TY-BSCIT: 50 Students (Capacity: 75%)", "FY-BMM: 45 Students (Capacity: 68%)" };
            foreach (var insight in insights)
            {
                Label l = new Label() { Text = "• " + insight, ForeColor = Color.LightGray, Font = new Font("Segoe UI", 9), Size = new Size(300, 25), Margin = new Padding(0, 5, 0, 0) };
                flpInsights.Controls.Add(l);
            }
            pnlBottom.Controls.Add(flpInsights);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 3);  // Docks First (Top)
            this.Controls.SetChildIndex(pnlToolbar, 2); // Docks Second (Top)
            this.Controls.SetChildIndex(dgvStudents, 1);// Docks Third (Top)
            this.Controls.SetChildIndex(pnlBottom, 0);  // Docks Last (Fill)
        }

        private Panel CreateStudentBox(string title, string val, string sub, Color accent)
        {
            Panel p = new Panel() { Size = new Size(220, 100), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(15, 15), AutoSize = true };
            Label lblV = new Label() { Text = val, Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 38), AutoSize = true };
            Label lblS = new Label() { Text = sub, Font = new Font("Segoe UI", 8, FontStyle.Italic), ForeColor = Color.DimGray, Location = new Point(15, 72), Size = new Size(190, 30) };
            p.Controls.AddRange(new Control[] { l, lblT, lblV, lblS });
            return p;
        }

        private void LoadMockStudents()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            dt.Columns.Add("Class");
            dt.Columns.Add("Email");
            dt.Columns.Add("Parent Contact");

            dt.Rows.Add("S101", "Rahul Sharma", "TY-BSCIT", "rahul.s@example.com", "9876543210");
            dt.Rows.Add("S102", "Priya Patel", "SY-BSCIT", "priya.p@example.com", "9876543211");
            dt.Rows.Add("S103", "Amit Mishra", "FY-BSCIT", "amit.m@example.com", "9876543212");
            dt.Rows.Add("S104", "Sneha Rao", "TY-BSCIT", "sneha.r@example.com", "9876543213");
            dt.Rows.Add("S105", "Vikram Singh", "SY-BSCIT", "vikram.s@example.com", "9876543214");

            if (this.dgvStudents != null)
            {
                this.dgvStudents.DataSource = dt;
                this.dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }
    }
}
