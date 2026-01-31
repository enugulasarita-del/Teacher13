using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public partial class ReportsControl : UserControl
    {
        public ReportsControl()
        {
            SetupStrictLayout();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitleText = new Label() { Text = "ACADEMIC REPORTS & ANALYTICS", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitleText);
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            // 2. Quick Summary Row (Top)
            Label lblStats = new Label() { Text = "SYSTEM-WIDE REPORT STATUS", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlScroll.Controls.Add(lblStats);

            FlowLayoutPanel flpQuickStats = new FlowLayoutPanel() { Dock = DockStyle.Top, Height = 100, AutoSize = false, Padding = new Padding(0, 5, 0, 10) };
            flpQuickStats.Controls.Add(CreateStatBox("Total Reports", "24", Color.FromArgb(173, 22, 37)));
            flpQuickStats.Controls.Add(CreateStatBox("Pending Audits", "3", Color.FromArgb(241, 196, 15)));
            flpQuickStats.Controls.Add(CreateStatBox("Last Generated", "Today", Color.FromArgb(46, 204, 113)));
            pnlScroll.Controls.Add(flpQuickStats);

            // 3. Custom Report Generator (Middle - Filling the gap)
            Panel pnlCustom = new Panel() { Dock = DockStyle.Top, Height = 220, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(25), Margin = new Padding(0, 20, 0, 0) };
            Label lblCustom = new Label() { Text = "GENERATE CUSTOM ANALYTICS", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlCustom.Controls.Add(lblCustom);
            
            pnlCustom.Controls.Add(new Label() { Text = "Class:", ForeColor = Color.LightGray, Location = new Point(25, 85), AutoSize = true });
            pnlCustom.Controls.Add(new ComboBox() { Location = new Point(100, 82), Width = 150 });
            
            pnlCustom.Controls.Add(new Label() { Text = "Type:", ForeColor = Color.LightGray, Location = new Point(280, 85), AutoSize = true });
            pnlCustom.Controls.Add(new ComboBox() { Location = new Point(340, 82), Width = 150 });
            
            Button btnGen = new Button() { 
                Text = "⚡ GENERATE NOW", 
                Location = new Point(25, 140), 
                Size = new Size(200, 40), 
                BackColor = Color.FromArgb(173, 22, 37), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnGen.FlatAppearance.BorderSize = 0;
            pnlCustom.Controls.Add(btnGen);
            pnlScroll.Controls.Add(pnlCustom);

            // 4. Report Gallery (Bottom)
            Label lblGallery = new Label() { Text = "AVAILABLE REPORT TEMPLATES", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.FromArgb(241, 196, 15), Dock = DockStyle.Top, Height = 40, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblGallery);

            FlowLayoutPanel flpReports = new FlowLayoutPanel() { Dock = DockStyle.Top, AutoSize = true, WrapContents = true, Padding = new Padding(0, 10, 0, 30) };
            string[] reportTypes = { "Student Performance", "Attendance Summary", "Exam Outcome", "Subject Progress", "Class Result Sheet", "Internal Audit" };
            foreach (var report in reportTypes) { flpReports.Controls.Add(CreateReportCard(report)); }
            pnlScroll.Controls.Add(flpReports);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 1); // Docks First (Top)
            this.Controls.SetChildIndex(pnlScroll, 0); // Docks Last (Fill)
        }

        private Panel CreateStatBox(string title, string val, Color accent)
        {
            Panel p = new Panel() { Size = new Size(220, 85), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 0) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(15, 15), AutoSize = true };
            Label lblV = new Label() { Text = val, Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 35), AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lblT, lblV });
            return p;
        }

        private Panel CreateReportCard(string title)
        {
            Panel card = new Panel() { Size = new Size(260, 150), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.White, Location = new Point(18, 18), Width = 230 };
            Label lblD = new Label() { Text = "Compiles semester data into a ready-to-print PDF format.", Font = new Font("Segoe UI", 8), ForeColor = Color.FromArgb(160, 160, 160), Location = new Point(18, 55), Size = new Size(230, 40) };
            Button btn = new Button() { Text = "DOWNLOAD PDF", Font = new Font("Segoe UI", 8, FontStyle.Bold), BackColor = Color.FromArgb(45, 45, 45), ForeColor = Color.LightGray, FlatStyle = FlatStyle.Flat, Location = new Point(18, 105), Size = new Size(120, 30) };
            btn.FlatAppearance.BorderSize = 0;
            card.Controls.AddRange(new Control[] { lblT, lblD, btn });
            return card;
        }

    }
}
