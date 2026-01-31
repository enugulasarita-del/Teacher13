using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public partial class CalendarControl : UserControl
    {
        public CalendarControl()
        {
            SetupStrictLayout();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitleText = new Label() { Text = "INTERACTIVE ACADEMIC PLANNER", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitleText);
            this.Controls.Add(pnlHeader);

            // 2. Main Content Splitter
            SplitContainer mainSplit = new SplitContainer() { Dock = DockStyle.Fill, SplitterDistance = 350, Padding = new Padding(20) };
            this.Controls.Add(mainSplit);

            // Left Side: Calendar & Legend (Fixing the overlap)
            Panel pnlLeft = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(15) };
            mainSplit.Panel1.Controls.Add(pnlLeft);

            MonthCalendar cal = new MonthCalendar() { Dock = DockStyle.Top };
            pnlLeft.Controls.Add(cal);
            
            Label lblLegend = new Label() { Text = "PLANNER LEGEND", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Gray, Dock = DockStyle.Top, Height = 40, Padding = new Padding(0, 15, 0, 5) };
            pnlLeft.Controls.Add(lblLegend);
            
            // Adding in reverse order to dock correctly below the labels
            pnlLeft.Controls.Add(CreateLegendItem("• Submission Deadlines", Color.FromArgb(46, 204, 113)));
            pnlLeft.Controls.Add(CreateLegendItem("• Faculty Meetings", Color.FromArgb(52, 152, 219)));
            pnlLeft.Controls.Add(CreateLegendItem("• Exams & Quizzes", Color.FromArgb(231, 76, 60)));

            // Filling the void below legends
            Label lblProgress = new Label() { Text = "SEMESTER PROGRESS", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Gray, Dock = DockStyle.Top, Height = 40, Padding = new Padding(0, 20, 0, 5) };
            pnlLeft.Controls.Add(lblProgress);
            
            Panel pnlProg = new Panel() { Dock = DockStyle.Top, Height = 10, BackColor = Color.FromArgb(45, 45, 45), Margin = new Padding(0, 5, 0, 5) };
            Panel pnlFill = new Panel() { Dock = DockStyle.Left, Width = 180, BackColor = Color.FromArgb(46, 204, 113) };
            pnlProg.Controls.Add(pnlFill);
            pnlLeft.Controls.Add(pnlProg);
            pnlLeft.Controls.Add(new Label() { Text = "65% Coverage Completed", ForeColor = Color.DimGray, Font = new Font("Segoe UI", 8), Dock = DockStyle.Top, Height = 25 });

            // Right Side: Agenda
            Panel pnlRight = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(28, 28, 28), Padding = new Padding(25) };
            Label lblDayTitle = new Label() { Text = "TODAY'S SCHEDULE", Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 50 };
            pnlRight.Controls.Add(lblDayTitle);
            
            FlowLayoutPanel flpEvents = new FlowLayoutPanel() { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false };
            pnlRight.Controls.Add(flpEvents);
            
            flpEvents.Controls.Add(CreateEventRow("09:00 AM", "Morning Briefing", "Principal's Office", Color.FromArgb(52, 152, 219)));
            flpEvents.Controls.Add(CreateEventRow("11:30 AM", "DBMS Project Viva", "Lab 402", Color.FromArgb(173, 22, 37)));
            flpEvents.Controls.Add(CreateEventRow("02:00 PM", "Seminar: AI Trends", "Auditorium", Color.FromArgb(46, 204, 113)));
            flpEvents.Controls.Add(CreateEventRow("04:30 PM", "Student Counseling", "Cabin 12", Color.FromArgb(241, 196, 15)));
            
            mainSplit.Panel2.Controls.Add(pnlRight);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 1); // Docks First (Top)
            this.Controls.SetChildIndex(mainSplit, 0); // Docks Last (Fill)
        }

        private Control CreateLegendItem(string text, Color color)
        {
            return new Label() { Text = text, ForeColor = color, Font = new Font("Segoe UI", 9), Dock = DockStyle.Top, Height = 25 };
        }

        private Panel CreateEventRow(string time, string title, string loc, Color accent)
        {
            Panel p = new Panel() { Size = new Size(550, 75), BackColor = Color.FromArgb(38, 38, 38), Margin = new Padding(0, 0, 0, 15) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label lblT = new Label() { Text = time, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.Gray, Location = new Point(15, 12), AutoSize = true };
            Label lblS = new Label() { Text = title, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 35), AutoSize = true };
            Label lblL = new Label() { Text = "📍 " + loc, Font = new Font("Segoe UI", 8), ForeColor = Color.DimGray, Location = new Point(400, 30), AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lblT, lblS, lblL });
            return p;
        }
    }
}
