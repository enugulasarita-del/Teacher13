using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public class PlacementControl : UserControl
    {
        public PlacementControl()
        {
            SetupStrictLayout();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitle = new Label() { Text = "COLLEGE PLACEMENT CELL", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            // Placement Stats
            Label lblStats = new Label() { Text = "PLACEMENT PACKAGE SUMMARY", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlScroll.Controls.Add(lblStats);

            FlowLayoutPanel flpStats = new FlowLayoutPanel() { Dock = DockStyle.Top, Height = 120, WrapContents = false };
            flpStats.Controls.Add(CreateStatCard("Highest Package", "₹ 18.5 LPA", Color.FromArgb(46, 204, 113)));
            flpStats.Controls.Add(CreateStatCard("Average Package", "₹ 6.2 LPA", Color.FromArgb(52, 152, 219)));
            flpStats.Controls.Add(CreateStatCard("Placement %", "84%", Color.FromArgb(241, 196, 15)));
            pnlScroll.Controls.Add(flpStats);

            // Upcoming Companies
            Label lblCompanies = new Label() { Text = "UPCOMING CAMPUS DRIVES", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblCompanies);

            DataGridView dgvCompanies = new DataGridView() { 
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
            dgvCompanies.DefaultCellStyle.BackColor = Color.White;
            dgvCompanies.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvCompanies.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCompanies.Columns.Add("Date", "Drive Date");
            dgvCompanies.Columns.Add("Company", "Company Name");
            dgvCompanies.Columns.Add("Role", "Role Offered");
            dgvCompanies.Columns.Add("Eligibility", "Min. CGPA");

            dgvCompanies.Rows.Add("Feb 12, 2026", "Microsoft", "SDE Intern", "8.5 CGPA");
            dgvCompanies.Rows.Add("Feb 20, 2026", "TCS Digital", "System Engineer", "7.0 CGPA");
            dgvCompanies.Rows.Add("Mar 05, 2026", "Accenture", "Associate Dev", "No Criteria");
            dgvCompanies.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            pnlScroll.Controls.Add(dgvCompanies);

            // Force Strict Docking Priority (Outer)
            this.Controls.SetChildIndex(pnlHeader, 1);
            this.Controls.SetChildIndex(pnlScroll, 0);

            // Force Strict Docking Priority (Inside Scroll)
            pnlScroll.Controls.SetChildIndex(lblStats, 3);
            pnlScroll.Controls.SetChildIndex(flpStats, 2);
            pnlScroll.Controls.SetChildIndex(lblCompanies, 1);
            pnlScroll.Controls.SetChildIndex(dgvCompanies, 0);
        }

        private Panel CreateStatCard(string title, string val, Color accent)
        {
            Panel p = new Panel() { Size = new Size(220, 100), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(15, 15), AutoSize = true };
            Label lblV = new Label() { Text = val, Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 40), AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lblT, lblV });
            return p;
        }
    }
}
