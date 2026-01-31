using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public class ProjectMentorshipControl : UserControl
    {
        public ProjectMentorshipControl()
        {
            SetupStrictLayout();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitle = new Label() { Text = "PROJECT MENTORSHIP & VIVA", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            // Project Groups
            Label lblGroups = new Label() { Text = "MY MENTEE GROUPS (FINAL YEAR)", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlScroll.Controls.Add(lblGroups);

            FlowLayoutPanel flpGroups = new FlowLayoutPanel() { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(0, 5, 0, 20) };
            flpGroups.Controls.Add(CreateGroupCard("Smart Agriculture IoT", "Group 04", "85% Built", Color.FromArgb(46, 204, 113)));
            flpGroups.Controls.Add(CreateGroupCard("Secure Blockchain Voting", "Group 12", "60% Built", Color.FromArgb(52, 152, 219)));
            flpGroups.Controls.Add(CreateGroupCard("ML Health Diagnostic", "Group 08", "Pending Viva", Color.FromArgb(241, 196, 15)));
            pnlScroll.Controls.Add(flpGroups);

            // Upcoming Viva Sessions
            Label lblViva = new Label() { Text = "UPCOMING EXTERNAL VIVA SESSIONS", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblViva);

            DataGridView dgvViva = new DataGridView() { 
                Dock = DockStyle.Top, 
                Height = 250, 
                BackgroundColor = Color.FromArgb(28, 28, 28), 
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 40,
                ReadOnly = true,
                AllowUserToAddRows = false,
                GridColor = Color.FromArgb(45, 45, 45)
            };
            dgvViva.DefaultCellStyle.BackColor = Color.White;
            dgvViva.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(173, 22, 37);
            dgvViva.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvViva.Columns.Add("Date", "Date");
            dgvViva.Columns.Add("Group", "Project Group");
            dgvViva.Columns.Add("External", "External Examiner");
            dgvViva.Columns.Add("Venue", "Lab Venue");

            dgvViva.Rows.Add("Feb 15, 2026", "Group 04 - IoT", "Dr. Satish K.", "Lab 402");
            dgvViva.Rows.Add("Feb 16, 2026", "Group 12 - Blockchain", "Prof. Megha R.", "Lab 501");
            dgvViva.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            pnlScroll.Controls.Add(dgvViva);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 1); // Docks First (Top)
            this.Controls.SetChildIndex(pnlScroll, 0); // Docks Last (Fill)
        }

        private Panel CreateGroupCard(string title, string id, string status, Color accent)
        {
            Panel p = new Panel() { Size = new Size(240, 110), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 15), Size = new Size(210, 40) };
            Label lblI = new Label() { Text = id, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(15, 60), AutoSize = true };
            Label lblS = new Label() { Text = status, Font = new Font("Segoe UI", 9, FontStyle.Italic), ForeColor = accent, Location = new Point(15, 80), AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lblT, lblI, lblS });
            return p;
        }
    }
}
