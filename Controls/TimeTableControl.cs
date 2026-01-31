using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public partial class TimeTableControl : UserControl
    {
        public TimeTableControl()
        {
            InitializeComponent();
            SetupStrictLayout();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblMain = new Label() { Text = "FACULTY MASTER SCHEDULE", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblMain);
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(20) };
            this.Controls.Add(pnlScroll);

            // 2. Timetable Grid
            Panel pnlGrid = new Panel() { Dock = DockStyle.Top, Height = 400, BackColor = Color.FromArgb(28, 28, 28), Padding = new Padding(10) };
            pnlScroll.Controls.Add(pnlGrid);
            DrawInnerTable(pnlGrid);

            // 3. Status Summary (Filling the Space)
            Label lblStatus = new Label() { Text = "TODAY'S HIGHLIGHTS", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Silver, Dock = DockStyle.Top, Height = 40, Padding = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblStatus);

            FlowLayoutPanel flpHighlights = new FlowLayoutPanel() { Dock = DockStyle.Top, AutoSize = true, WrapContents = true };
            pnlScroll.Controls.Add(flpHighlights);

            flpHighlights.Controls.Add(CreateHighlightCard("Current Session", "Introduction to DBMS", "Lab 402", Color.FromArgb(46, 204, 113)));
            flpHighlights.Controls.Add(CreateHighlightCard("Next Session", "Digital Marketing 101", "Room 305", Color.FromArgb(52, 152, 219)));
            flpHighlights.Controls.Add(CreateHighlightCard("Afternoon", "Faculty Meeting", "Conference Room", Color.FromArgb(173, 22, 37)));

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 1); // Docks First (Top)
            this.Controls.SetChildIndex(pnlScroll, 0); // Docks Last (Fill)
        }

        private void DrawInnerTable(Panel parent)
        {
            string[] days = { "Time", "Mon", "Tue", "Wed", "Thu", "Fri" };
            string[] times = { "09-10", "10-11", "11-12", "12-01", "01-02" };
            
            TableLayoutPanel tlp = new TableLayoutPanel() { Dock = DockStyle.Fill };
            tlp.ColumnCount = 6;
            tlp.RowCount = 6;
            tlp.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            
            for (int i = 0; i < 6; i++) tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6f));
            for (int i = 0; i < 6; i++) tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6f));

            for (int i = 0; i < 6; i++) tlp.Controls.Add(new Label { Text = days[i], Font = new Font("Segoe UI", 9, FontStyle.Bold), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.FromArgb(44, 62, 80), ForeColor = Color.White }, i, 0);

            for (int r = 1; r < 6; r++)
            {
                tlp.Controls.Add(new Label { Text = times[r-1], Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8, FontStyle.Bold), BackColor = Color.FromArgb(32, 33, 36), ForeColor = Color.LightGray }, 0, r);
                for (int c = 1; c < 6; c++)
                {
                    string sub = (r + c) % 3 == 0 ? "Lecture" : ((r + c) % 3 == 1 ? "Lab" : "Research");
                    tlp.Controls.Add(new Label { Text = sub, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8), ForeColor = Color.White }, c, r);
                }
            }
            parent.Controls.Add(tlp);
        }

        private Panel CreateHighlightCard(string phase, string title, string room, Color color)
        {
            Panel p = new Panel() { Size = new Size(300, 100), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = color };
            Label lp = new Label() { Text = phase, Font = new Font("Segoe UI", 8, FontStyle.Bold), Location = new Point(15, 15), ForeColor = color, AutoSize = true };
            Label lt = new Label() { Text = title, Font = new Font("Segoe UI", 11, FontStyle.Bold), Location = new Point(15, 35), Width = 270, ForeColor = Color.White };
            Label lr = new Label() { Text = "📍 " + room, Font = new Font("Segoe UI", 9), Location = new Point(15, 65), ForeColor = Color.Gray, AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lp, lt, lr });
            return p;
        }
    }
}
