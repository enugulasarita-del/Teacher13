using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TeacherDashboard.Controls
{
    public partial class ClassesControl : UserControl
    {
        private FlowLayoutPanel flpClasses;

        public ClassesControl()
        {
            InitializeComponent();
            SetupStrictLayout();
            LoadMockClasses();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitle = new Label() { Text = "MY ACADEMIC CLASSES", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            // 2. Section: Active Classes (Top)
            Label lblClasses = new Label() { Text = "ACTIVE CLASS SESSIONS", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlScroll.Controls.Add(lblClasses);

            flpClasses = new FlowLayoutPanel() { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(0, 5, 0, 20), WrapContents = true };
            pnlScroll.Controls.Add(flpClasses);

            // 3. Section: Performance Insights (Middle - Filling the blank space)
            Label lblStatsTitle = new Label() { Text = "SUBJECT PERFORMANCE OVERVIEW", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblStatsTitle);

            FlowLayoutPanel flpStats = new FlowLayoutPanel() { Dock = DockStyle.Top, Height = 120, Padding = new Padding(0, 5, 0, 10) };
            flpStats.Controls.Add(CreateCompactStat("Avg. Score", "74%", Color.FromArgb(46, 204, 113)));
            flpStats.Controls.Add(CreateCompactStat("Resources", "124", Color.FromArgb(52, 152, 219)));
            flpStats.Controls.Add(CreateCompactStat("Quiz Avg.", "8.2/10", Color.FromArgb(241, 196, 15)));
            pnlScroll.Controls.Add(flpStats);

            // 4. Section: Updates & Activities (Bottom - Filling remaining space)
            Label lblUpdates = new Label() { Text = "RECENT UPDATES & ANNOUNCEMENTS", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblUpdates);

            FlowLayoutPanel flpUpdates = new FlowLayoutPanel() { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(0, 5, 0, 30) };
            flpUpdates.Controls.Add(CreateUpdateCard("Operating Systems", "Lab Manual for Unit 4 uploaded.", "2 hours ago", Color.FromArgb(173, 22, 37)));
            flpUpdates.Controls.Add(CreateUpdateCard("Java Programming", "Project deadline extended to Feb 15.", "Yesterday", Color.FromArgb(52, 152, 219)));
            flpUpdates.Controls.Add(CreateUpdateCard("Cloud Computing", "AWS Guest Lecture tomorrow @ 2 PM.", "Jan 30", Color.FromArgb(46, 204, 113)));
            pnlScroll.Controls.Add(flpUpdates);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 1); // Docks First (Top)
            this.Controls.SetChildIndex(pnlScroll, 0); // Docks Last (Fill)
        }

        private Panel CreateCompactStat(string title, string val, Color accent)
        {
            Panel p = new Panel() { Size = new Size(180, 80), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 0) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 4, BackColor = accent };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 8), ForeColor = Color.Gray, Location = new Point(15, 15), AutoSize = true };
            Label lblV = new Label() { Text = val, Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 35), AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lblT, lblV });
            return p;
        }

        private Panel CreateUpdateCard(string subject, string details, string time, Color accent)
        {
            Panel p = new Panel() { Size = new Size(320, 100), BackColor = Color.FromArgb(38, 38, 38), Margin = new Padding(0, 10, 20, 10) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label lblS = new Label() { Text = subject, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 10), AutoSize = true };
            Label lblD = new Label() { Text = details, Font = new Font("Segoe UI", 9), ForeColor = Color.FromArgb(180, 180, 180), Location = new Point(15, 35), Size = new Size(290, 40) };
            Label lblT = new Label() { Text = time, Font = new Font("Segoe UI", 8, FontStyle.Italic), ForeColor = Color.DimGray, Location = new Point(15, 75), AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lblS, lblD, lblT });
            return p;
        }

        private Label lblStatHead() => new Label() { Dock = DockStyle.Top, Height = 10 };

        private void LoadMockClasses()
        {
            flpClasses.Controls.Add(new ClassCard { ClassName = "FY-BSCIT", Subject = "Operating Systems", StudentsCount = "60 Students", AccentColor = Color.FromArgb(173, 22, 37) });
            flpClasses.Controls.Add(new ClassCard { ClassName = "SY-BSCIT", Subject = "Java Programming", StudentsCount = "55 Students", AccentColor = Color.FromArgb(52, 152, 219) });
            flpClasses.Controls.Add(new ClassCard { ClassName = "TY-BSCIT", Subject = "Cloud Computing", StudentsCount = "50 Students", AccentColor = Color.FromArgb(46, 204, 113) });
            flpClasses.Controls.Add(new ClassCard { ClassName = "FY-BMM", Subject = "Communication Skills", StudentsCount = "45 Students", AccentColor = Color.FromArgb(241, 196, 15) });
        }
    }
}
