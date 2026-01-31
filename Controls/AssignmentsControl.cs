using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TeacherDashboard.Controls
{
    public partial class AssignmentsControl : UserControl
    {
        public AssignmentsControl()
        {
            InitializeComponent();
            SetupStrictLayout();
            LoadAssignments();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblMain = new Label() { Text = "ASSIGNMENTS TRACKER", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblMain);
            this.Controls.Add(pnlHeader);

            // 2. Toolbar
            Panel pnlToolbar = new Panel() { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(20, 10, 20, 10) };
            Button btnAdd = new Button() { 
                Text = "📝 POST NEW ASSIGNMENT", 
                Location = new Point(20, 12), 
                Size = new Size(220, 35), 
                BackColor = Color.FromArgb(173, 22, 37), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            pnlToolbar.Controls.Add(btnAdd);
            this.Controls.Add(pnlToolbar);

            // 3. Content
            SplitContainer split = new SplitContainer() { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, SplitterDistance = 350 };
            this.Controls.Add(split);

            this.dgvAssignments = new DataGridView() { 
                Dock = DockStyle.Fill, 
                BackgroundColor = Color.FromArgb(28, 28, 28), 
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 40,
                AllowUserToAddRows = false,
                GridColor = Color.FromArgb(45, 45, 45)
            };
            this.dgvAssignments.DefaultCellStyle.BackColor = Color.White;
            this.dgvAssignments.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            this.dgvAssignments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            split.Panel1.Controls.Add(this.dgvAssignments);

            // 4. Submission Rate Section (Replacing Chart)
            Panel pnlStats = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(18, 18, 18), Padding = new Padding(20) };
            Label lblChartTitle = new Label() { Text = "SUBMISSION METRICS", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlStats.Controls.Add(lblChartTitle);

            FlowLayoutPanel flpMetric = new FlowLayoutPanel() { Dock = DockStyle.Fill };
            flpMetric.Controls.Add(CreateMetricCard("Active Assignments", "12", "Due this week", Color.FromArgb(241, 196, 15)));
            flpMetric.Controls.Add(CreateMetricCard("Late Submissions", "45", "Across all subjects", Color.FromArgb(231, 76, 60)));
            flpMetric.Controls.Add(CreateMetricCard("Average Score", "7.5/10", "Based on graded items", Color.FromArgb(46, 204, 113)));
            flpMetric.Controls.Add(CreateMetricCard("Pending Grading", "150", "Student uploads", Color.FromArgb(52, 152, 219)));

            pnlStats.Controls.Add(flpMetric);
            split.Panel2.Controls.Add(pnlStats);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 2);  // Docks First (Top)
            this.Controls.SetChildIndex(pnlToolbar, 1); // Docks Second (Top)
            this.Controls.SetChildIndex(split, 0);      // Docks Last (Fill)
        }

        private Panel CreateMetricCard(string title, string val, string sub, Color accent)
        {
            Panel p = new Panel() { Size = new Size(240, 100), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(15, 15), AutoSize = true };
            Label lblV = new Label() { Text = val, Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 38), AutoSize = true };
            Label lblS = new Label() { Text = sub, Font = new Font("Segoe UI", 8, FontStyle.Italic), ForeColor = Color.DimGray, Location = new Point(15, 72), AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lblT, lblV, lblS });
            return p;
        }

        private void LoadAssignments()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Subject");
            dt.Columns.Add("Topic");
            dt.Columns.Add("Due Date");
            dt.Columns.Add("Submissions");

            dt.Rows.Add("Data Science", "Linear Regression Lab", "2026-02-05", "45/60");
            dt.Rows.Add("Networking", "Packet Tracing Quiz", "2026-02-03", "12/60");
            dt.Rows.Add("Java", "Multithreading MCQ", "2026-02-08", "0/60");

            if (this.dgvAssignments != null)
            {
                this.dgvAssignments.DataSource = dt;
                this.dgvAssignments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }
    }
}
