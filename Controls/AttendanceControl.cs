using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TeacherDashboard.Controls
{
    public partial class AttendanceControl : UserControl
    {
        public AttendanceControl()
        {
            InitializeComponent();
            SetupStrictLayout();
            LoadMockAttendance();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37), Padding = new Padding(20, 0, 20, 0) };
            Label lblMainTitle = new Label() { Text = "ATTENDANCE TRACKER", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(20, 18) };
            pnlHeader.Controls.Add(lblMainTitle);
            this.Controls.Add(pnlHeader);

            // 2. Action Bar
            Panel pnlActions = new Panel() { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(20, 10, 20, 10) };
            Label lblD = new Label() { Text = "DATE:", Location = new Point(25, 22), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.White };
            DateTimePicker dtp = new DateTimePicker() { Location = new Point(75, 18), Width = 150 };
            Label lblC = new Label() { Text = "CLASS:", Location = new Point(250, 22), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.White };
            ComboBox cmb = new ComboBox() { Location = new Point(310, 18), Width = 120 };
            cmb.Items.AddRange(new string[] { "FY-BSCIT", "SY-BSCIT", "TY-BSCIT" });
            cmb.SelectedIndex = 0;
            Button btnSave = new Button() { Text = "✔ SAVE", Location = new Point(450, 12), Size = new Size(120, 35), BackColor = Color.FromArgb(173, 22, 37), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnSave.FlatAppearance.BorderSize = 0;
            pnlActions.Controls.AddRange(new Control[] { lblD, dtp, lblC, cmb, btnSave });
            this.Controls.Add(pnlActions);

            // 3. Grid (Primary)
            this.dgvAttendance = new DataGridView() { 
                Dock = DockStyle.Top, 
                Height = 300,
                BackgroundColor = Color.FromArgb(28, 28, 28), 
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 40,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(45, 45, 45)
            };
            this.dgvAttendance.DefaultCellStyle.BackColor = Color.White;
            this.dgvAttendance.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            this.dgvAttendance.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.Controls.Add(this.dgvAttendance);

            // 4. Fill Bottom with Insights and Defaulters
            Panel pnlBottom = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(18, 18, 18), Padding = new Padding(25) };
            this.Controls.Add(pnlBottom);

            Label lblStatHead = new Label() { Text = "MONTHLY ATTENDANCE INSIGHTS", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlBottom.Controls.Add(lblStatHead);

            FlowLayoutPanel flpStats = new FlowLayoutPanel() { Dock = DockStyle.Top, Height = 120, WrapContents = false };
            flpStats.Controls.Add(CreateInsightBox("Avg. Attendance", "88%", Color.FromArgb(46, 204, 113)));
            flpStats.Controls.Add(CreateInsightBox("Defaulter Count", "12", Color.FromArgb(231, 76, 60)));
            flpStats.Controls.Add(CreateInsightBox("Top Class", "TY-BSCIT", Color.FromArgb(52, 152, 219)));
            pnlBottom.Controls.Add(flpStats);

            Label lblDefHead = new Label() { Text = "DEFAULTERS WATCHLIST (Below 75%)", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(231, 76, 60), Dock = DockStyle.Top, Height = 30, Margin = new Padding(0, 20, 0, 0) };
            pnlBottom.Controls.Add(lblDefHead);

            FlowLayoutPanel flpDefaulters = new FlowLayoutPanel() { Dock = DockStyle.Fill, AutoScroll = true };
            string[] defaulters = { "Amit Mishra (64%)", "Suresh Raina (72%)", "Deepak Hooda (68%)", "Krunal Pandya (70%)" };
            foreach (var d in defaulters)
            {
                Label l = new Label() { Text = "⚠ " + d, ForeColor = Color.LightGray, Font = new Font("Segoe UI", 9), Size = new Size(250, 25), Margin = new Padding(0, 5, 0, 0) };
                flpDefaulters.Controls.Add(l);
            }
            pnlBottom.Controls.Add(flpDefaulters);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 3);    // Docks First (Top)
            this.Controls.SetChildIndex(pnlActions, 2);   // Docks Second (Top)
            this.Controls.SetChildIndex(dgvAttendance, 1);// Docks Third (Top)
            this.Controls.SetChildIndex(pnlBottom, 0);    // Docks Last (Fill)
        }

        private Panel CreateInsightBox(string title, string val, Color accent)
        {
            Panel p = new Panel() { Size = new Size(200, 100), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(15, 20), AutoSize = true };
            Label lblV = new Label() { Text = val, Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 45), AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lblT, lblV });
            return p;
        }

        private void LoadMockAttendance()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Roll No");
            dt.Columns.Add("Student Name");
            dt.Columns.Add("Status", typeof(bool));

            dt.Rows.Add("1", "Rahul Sharma", true);
            dt.Rows.Add("2", "Priya Patel", true);
            dt.Rows.Add("3", "Amit Mishra", false);
            dt.Rows.Add("4", "Sneha Rao", true);
            dt.Rows.Add("5", "Vikram Singh", true);

            if (this.dgvAttendance != null)
            {
                this.dgvAttendance.DataSource = dt;
                this.dgvAttendance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }
    }
}
