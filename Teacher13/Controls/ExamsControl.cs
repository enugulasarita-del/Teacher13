using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TeacherDashboard.Controls
{
    public partial class ExamsControl : UserControl
    {
        public ExamsControl()
        {
            InitializeComponent();
            SetupStrictLayout();
            LoadExams();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitleText = new Label() { Text = "EXAMS & GRADING HUB", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitleText);
            this.Controls.Add(pnlHeader);

            // 2. Action Bar
            Panel pnlToolbar = new Panel() { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(20, 10, 20, 10) };
            Button btnCreate = new Button() { 
                Text = "➕ CREATE NEW EXAM", 
                Location = Point.Empty, // Will be managed by flow if needed, but here it's manual
                Size = new Size(180, 35), 
                BackColor = Color.FromArgb(173, 22, 37), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnCreate.Location = new Point(20,12);
            btnCreate.FlatAppearance.BorderSize = 0;
            pnlToolbar.Controls.Add(btnCreate);
            this.Controls.Add(pnlToolbar);

            // 3. Content Splitter
            SplitContainer split = new SplitContainer() { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, SplitterDistance = 350 };
            this.Controls.Add(split);

            // 4. Grid
            this.dgvExams = new DataGridView() { 
                Dock = DockStyle.Fill, 
                BackgroundColor = Color.FromArgb(28, 28, 28), 
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 40,
                AllowUserToAddRows = false,
                GridColor = Color.FromArgb(45, 45, 45)
            };
            this.dgvExams.DefaultCellStyle.BackColor = Color.White;
            this.dgvExams.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            this.dgvExams.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            split.Panel1.Controls.Add(this.dgvExams);

            // 5. Exam Readiness (Replacing Chart)
            Panel pnlStats = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(18, 18, 18), Padding = new Padding(20) };
            Label lblGraphTitle = new Label() { Text = "EXAM PREPARATION STATUS", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlStats.Controls.Add(lblGraphTitle);

            FlowLayoutPanel flpExamStats = new FlowLayoutPanel() { Dock = DockStyle.Fill };
            flpExamStats.Controls.Add(CreateExamStatBox("Syllabus Completed", "94%", Color.FromArgb(46, 204, 113)));
            flpExamStats.Controls.Add(CreateExamStatBox("Papers Verified", "12/15", Color.FromArgb(52, 152, 219)));
            flpExamStats.Controls.Add(CreateExamStatBox("Seating Arrg.", "Done", Color.FromArgb(173, 22, 37)));
            flpExamStats.Controls.Add(CreateExamStatBox("Hall Tickets", "Sent", Color.FromArgb(241, 196, 15)));

            pnlStats.Controls.Add(flpExamStats);
            split.Panel2.Controls.Add(pnlStats);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 2);  // Docks First (Top)
            this.Controls.SetChildIndex(pnlToolbar, 1); // Docks Second (Top)
            this.Controls.SetChildIndex(split, 0);      // Docks Last (Fill)
        }

        private Panel CreateExamStatBox(string title, string val, Color accent)
        {
            Panel p = new Panel() { Size = new Size(220, 100), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(15, 20), AutoSize = true };
            Label lblV = new Label() { Text = val, Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 45), AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lblT, lblV });
            return p;
        }

        private void LoadExams()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Exam Title");
            dt.Columns.Add("Class");
            dt.Columns.Add("Date");
            dt.Columns.Add("Status");

            dt.Rows.Add("Mid-Term Exam", "FY-BSCIT", "2026-02-15", "Scheduled");
            dt.Rows.Add("Internal Assessment", "SY-BSCIT", "2026-02-10", "Scheduled");
            dt.Rows.Add("Final Practicals", "TY-BSCIT", "2026-03-20", "Pending");

            if (this.dgvExams != null)
            {
                this.dgvExams.DataSource = dt;
                this.dgvExams.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }
    }
}
