using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public partial class EventsControl : UserControl
    {
        private DataGridView dgvEvents;

        public EventsControl()
        {
            SetupStrictLayout();
            LoadEvents();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitle = new Label() { Text = "COLLEGE EVENTS & SEMINARS", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            // 2. Events Grid (Top)
            dgvEvents = new DataGridView() { 
                Dock = DockStyle.Top, 
                Height = 250,
                BackgroundColor = Color.FromArgb(28, 28, 28), 
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 40,
                AllowUserToAddRows = false,
                GridColor = Color.FromArgb(45, 45, 45)
            };
            dgvEvents.DefaultCellStyle.BackColor = Color.White;
            dgvEvents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(173, 22, 37);
            dgvEvents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            pnlScroll.Controls.Add(dgvEvents);

            // 3. Middle Section: Upcoming Deadlines (Filling the blank)
            Label lblDeadlines = new Label() { Text = "EVENT REGISTRATION DEADLINES", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblDeadlines);

            FlowLayoutPanel flpDeadlines = new FlowLayoutPanel() { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(0, 5, 0, 20) };
            string[] deadlines = { "• Tech Summit Registration: Ends in 2 days", "• Cultural Fest Auditions: Feb 10th", "• Research Paper Submission: Feb 15th" };
            foreach (var d in deadlines)
            {
                Label l = new Label() { Text = d, ForeColor = Color.FromArgb(241, 196, 15), Font = new Font("Segoe UI", 9, FontStyle.Italic), Size = new Size(400, 25) };
                flpDeadlines.Controls.Add(l);
            }
            pnlScroll.Controls.Add(flpDeadlines);

            // 4. Bottom Section: Featured Event (Filling remaining space)
            Label lblFeatured = new Label() { Text = "⭐ FEATURED EVENT OF THE MONTH", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.FromArgb(241, 196, 15), Dock = DockStyle.Top, Height = 40, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblFeatured);
            
            Panel poster = new Panel() { Dock = DockStyle.Top, Height = 250, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(30) };
            Label lblP1 = new Label() { Text = "Global Tech Summit 2026", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(30, 30) };
            Label lblP2 = new Label() { Text = "3-day exploration of AI and Robotics with industry leaders.", Font = new Font("Segoe UI", 11), ForeColor = Color.FromArgb(180, 180, 180), AutoSize = true, Location = new Point(30, 75) };
            Label lblP3 = new Label() { Text = "📅 15th - 18th Feb | 📍 Grand Auditorium", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(241, 196, 15), AutoSize = true, Location = new Point(30, 110) };
            
            Button btnReg = new Button() { Text = "REGISTER STUDENTS", BackColor = Color.FromArgb(173, 22, 37), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(200, 40), Location = new Point(30, 160), Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnReg.FlatAppearance.BorderSize = 0;
            
            poster.Controls.AddRange(new Control[] { lblP1, lblP2, lblP3, btnReg });
            pnlScroll.Controls.Add(poster);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 1); // Docks First (Top)
            this.Controls.SetChildIndex(pnlScroll, 0); // Docks Last (Fill)
        }

        private void LoadEvents()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Date");
            dt.Columns.Add("Event Name");
            dt.Columns.Add("Venue");
            dt.Columns.Add("Organizer");

            dt.Rows.Add("2026-02-15", "Global Tech Summit 2026", "Main Auditorium", "IT Dept");
            dt.Rows.Add("2026-02-20", "AI & Ethics Workshop", "Seminar Hall B", "Research Cell");
            dt.Rows.Add("2026-02-28", "Annual Cultural Fest", "College Ground", "Student Council");

            dgvEvents.DataSource = dt;
            dgvEvents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
