using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TeacherDashboard.Controls
{
    public partial class GradesControl : UserControl
    {
        private DataGridView dgvGrades;

        public GradesControl()
        {
            SetupStrictLayout();
            LoadGrades();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitle = new Label() { Text = "STUDENT GRADE TRACKER", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            // 2. Grade Grid (Top)
            dgvGrades = new DataGridView() { 
                Dock = DockStyle.Top, 
                Height = 300,
                BackgroundColor = Color.FromArgb(28, 28, 28), 
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 40,
                ReadOnly = true,
                AllowUserToAddRows = false,
                GridColor = Color.FromArgb(45, 45, 45)
            };
            dgvGrades.DefaultCellStyle.BackColor = Color.White;
            dgvGrades.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvGrades.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            pnlScroll.Controls.Add(dgvGrades);

            // 3. Middle Section: Performance Highlights (Filling the blank)
            Label lblAnalyticTitle = new Label() { Text = "PERFORMANCE MILESTONES", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblAnalyticTitle);

            FlowLayoutPanel flpMilestones = new FlowLayoutPanel() { Dock = DockStyle.Top, Height = 130, Padding = new Padding(0, 5, 0, 10), WrapContents = false };
            flpMilestones.Controls.Add(CreateMilestoneCard("Avg. Score", "78.4%", "↑ 2.1% Semester", Color.FromArgb(46, 204, 113)));
            flpMilestones.Controls.Add(CreateMilestoneCard("Top Subject", "Data Science", "Avg: 85/100", Color.FromArgb(52, 152, 219)));
            flpMilestones.Controls.Add(CreateMilestoneCard("Readiness", "92%", "Credits Met", Color.FromArgb(173, 22, 37)));
            pnlScroll.Controls.Add(flpMilestones);

            // 4. Bottom Section: Grade Distribution (New Related Feature)
            Label lblDistTitle = new Label() { Text = "GRADE DISTRIBUTION SUMMARY", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblDistTitle);

            FlowLayoutPanel flpDist = new FlowLayoutPanel() { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(0, 5, 0, 30) };
            string[] dists = { "Grade O: 12 Students", "Grade A+: 25 Students", "Grade A: 40 Students", "Grade B: 15 Students", "Grade F: 02 Students" };
            foreach (var d in dists)
            {
                Label l = new Label() { Text = "📊 " + d, ForeColor = Color.LightGray, Font = new Font("Segoe UI", 9), Size = new Size(200, 25) };
                flpDist.Controls.Add(l);
            }
            pnlScroll.Controls.Add(flpDist);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 1); // Docks First (Top)
            this.Controls.SetChildIndex(pnlScroll, 0); // Docks Last (Fill)
        }

        private Panel CreateMilestoneCard(string title, string val, string sub, Color accent)
        {
            Panel p = new Panel() { Size = new Size(240, 110), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(15, 15), AutoSize = true };
            Label lblV = new Label() { Text = val, Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 40), AutoSize = true };
            Label lblS = new Label() { Text = sub, Font = new Font("Segoe UI", 8, FontStyle.Italic), ForeColor = Color.DimGray, Location = new Point(15, 75), Size = new Size(210, 30) };
            p.Controls.AddRange(new Control[] { l, lblT, lblV, lblS });
            return p;
        }

        private void LoadGrades()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Student Name");
            dt.Columns.Add("Subject");
            dt.Columns.Add("Internal (20)");
            dt.Columns.Add("External (80)");
            dt.Columns.Add("Total (100)");
            dt.Columns.Add("Grade");

            dt.Rows.Add("Rahul Sharma", "Data Science", "18", "72", "90", "A+");
            dt.Rows.Add("Priya Patel", "Networking", "15", "65", "80", "A");
            dt.Rows.Add("Amit Kumar", "Java", "12", "55", "67", "B");
            dt.Rows.Add("Sonal Gupta", "Cyber Security", "19", "78", "97", "O");
            dt.Rows.Add("Vikas Singh", "DBMS", "16", "60", "76", "A");

            dgvGrades.DataSource = dt;
            dgvGrades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
