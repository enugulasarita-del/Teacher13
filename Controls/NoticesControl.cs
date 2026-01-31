using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public partial class NoticesControl : UserControl
    {
        private DataGridView dgvHistory;

        public NoticesControl()
        {
            SetupStrictLayout();
            LoadData();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitle = new Label() { Text = "OFFICIAL NOTICE BOARD", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            // 2. Urgent Notices (Top - Interactive Horizontal Scroll)
            Label lblUrgent = new Label() { Text = "URGENT ANNOUNCEMENTS", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.FromArgb(241, 196, 15), Dock = DockStyle.Top, Height = 40 };
            pnlScroll.Controls.Add(lblUrgent);

            FlowLayoutPanel flpUrgent = new FlowLayoutPanel() { Dock = DockStyle.Top, Height = 160, WrapContents = false, AutoScroll = true, Padding = new Padding(0, 5, 0, 10) };
            flpUrgent.Controls.Add(CreateNoticeCard("Exam Deadline", "Final Year Projects must be submitted by Feb 10th.", Color.FromArgb(231, 76, 60)));
            flpUrgent.Controls.Add(CreateNoticeCard("Holiday Update", "College will remain closed on Jan 31st.", Color.FromArgb(52, 152, 219)));
            flpUrgent.Controls.Add(CreateNoticeCard("Staff Meeting", "Internal Dept meeting scheduled for tomorrow @ 11 AM.", Color.FromArgb(46, 204, 113)));
            pnlScroll.Controls.Add(flpUrgent);

            // 3. Pinned Reminders (Middle - Related Feature)
            Label lblPinned = new Label() { Text = "PINNED REMINDERS", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Gray, Dock = DockStyle.Top, Height = 30, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblPinned);

            FlowLayoutPanel flpPinned = new FlowLayoutPanel() { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(0, 5, 0, 20) };
            string[] pinnedItems = { "✔ Submit Monthly Attendance by 5th", "✔ Upload Unit Test 2 marks", "✔ Update Faculty Profile for NBA Audit", "✔ Renew Library Membership" };
            foreach (var item in pinnedItems)
            {
                Label l = new Label() { Text = item, ForeColor = Color.LightGray, Font = new Font("Segoe UI", 9), Size = new Size(350, 25) };
                flpPinned.Controls.Add(l);
            }
            pnlScroll.Controls.Add(flpPinned);

            // 4. History Notice List (Bottom)
            Label lblHistory = new Label() { Text = "ALL PREVIOUS NOTICES", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblHistory);

            dgvHistory = new DataGridView() { 
                Dock = DockStyle.Top, 
                Height = 300, 
                BackgroundColor = Color.FromArgb(28, 28, 28), 
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 40,
                GridColor = Color.FromArgb(45, 45, 45)
            };
            this.dgvHistory.DefaultCellStyle.BackColor = Color.White;
            dgvHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            pnlScroll.Controls.Add(dgvHistory);

            // Force Strict Docking Priority (Outer)
            this.Controls.SetChildIndex(pnlHeader, 1);
            this.Controls.SetChildIndex(pnlScroll, 0);

            // Force Strict Docking Priority (Inside Scroll Panel)
            pnlScroll.Controls.SetChildIndex(lblUrgent, 5);
            pnlScroll.Controls.SetChildIndex(flpUrgent, 4);
            pnlScroll.Controls.SetChildIndex(lblPinned, 3);
            pnlScroll.Controls.SetChildIndex(flpPinned, 2);
            pnlScroll.Controls.SetChildIndex(lblHistory, 1);
            pnlScroll.Controls.SetChildIndex(dgvHistory, 0);
        }

        private Panel CreateNoticeCard(string title, string content, Color accent)
        {
            Panel card = new Panel() { Size = new Size(320, 140), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Panel side = new Panel() { Dock = DockStyle.Left, Width = 6, BackColor = accent };
            
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.White, Location = new Point(20, 15), AutoSize = true };
            Label lblC = new Label() { Text = content, Font = new Font("Segoe UI", 9), ForeColor = Color.FromArgb(180, 180, 180), Location = new Point(20, 45), Size = new Size(280, 70) };
            
            card.Controls.AddRange(new Control[] { side, lblT, lblC });
            return card;
        }

        private void LoadData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Date");
            dt.Columns.Add("Category");
            dt.Columns.Add("Subject");
            dt.Rows.Add("2026-01-25", "Event", "Annual Sports Day Registration");
            dt.Rows.Add("2026-01-20", "Library", "New Journals added to CS Section");
            dt.Rows.Add("2026-01-18", "Exam", "Backlog Results Published");
            dgvHistory.DataSource = dt;
            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
