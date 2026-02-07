using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Drawing.Drawing2D;

namespace TeacherDashboard.Controls
{
    public partial class FacultyLeaveControl : UserControl
    {
        // Theme Colors
        private Color primaryColor = Color.FromArgb(173, 22, 37); // VSIT Red
        private Color bgColor = Color.White;
        private Color cardBg = Color.White;
        private Color borderColor = Color.FromArgb(220, 220, 220);

        // UI Components
        private ComboBox cmbLeaveType;
        private DateTimePicker dtpStart, dtpEnd;
        private TextBox txtReason;
        private DataGridView dgvLeaveHistory;
        private DataTable dtLeaveHistory;
        private Label lblSickBalance, lblCasualBalance, lblEarnedBalance;
        private NumericUpDown numDaysRequested;

        // Initial Yearly Quotas
        private const int MAX_SICK = 10;
        private const int MAX_CASUAL = 5;
        private const int MAX_EARNED = 15;

        public FacultyLeaveControl()
        {
            InitializeComponent();
            SetupDataStructures();
            SetupLayout();
            SetupEvents();
            UpdateBalanceDisplay();
            SyncDaysFromDates();
        }

        private void SetupDataStructures()
        {
            dtLeaveHistory = new DataTable();
            dtLeaveHistory.Columns.Add("ID");
            dtLeaveHistory.Columns.Add("Type");
            dtLeaveHistory.Columns.Add("Start");
            dtLeaveHistory.Columns.Add("End");
            dtLeaveHistory.Columns.Add("Days");
            dtLeaveHistory.Columns.Add("Reason");
            dtLeaveHistory.Columns.Add("Status");

            // Dummy Approved History (These will be deducted from the quota automatically)
            dtLeaveHistory.Rows.Add("LV-101", "Sick Leave", "05/01/2026", "06/01/2026", "2 Days", "Fever", "Approved");
            dtLeaveHistory.Rows.Add("LV-105", "Casual Leave", "15/01/2026", "15/01/2026", "1 Day", "Personal", "Approved");
        }

        private void UpdateBalanceDisplay()
        {
            int usedSick = 0, usedCasual = 0, usedEarned = 0;

            foreach (DataRow row in dtLeaveHistory.Rows)
            {
                if (row["Status"].ToString() == "Approved")
                {
                    string type = row["Type"].ToString();
                    string daysStr = row["Days"].ToString().Replace(" Days", "").Replace(" Day", "").Trim();
                    if (int.TryParse(daysStr, out int d))
                    {
                        if (type == "Sick Leave") usedSick += d;
                        else if (type == "Casual Leave") usedCasual += d;
                        else if (type == "Earned Leave") usedEarned += d;
                    }
                }
            }

            int curSick = Math.Max(0, MAX_SICK - usedSick);
            int curCasual = Math.Max(0, MAX_CASUAL - usedCasual);
            int curEarned = Math.Max(0, MAX_EARNED - usedEarned);

            lblSickBalance.Text = curSick.ToString("D2") + " DAYS";
            lblCasualBalance.Text = curCasual.ToString("D2") + " DAYS";
            lblEarnedBalance.Text = curEarned.ToString("D2") + " DAYS";
            
            lblSickBalance.Parent?.Refresh();
            lblCasualBalance.Parent?.Refresh();
            lblEarnedBalance.Parent?.Refresh();
        }

        private void SetupLayout()
        {
            this.Controls.Clear();
            this.BackColor = bgColor;
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 10);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 75, BackColor = Color.White };
            Label lblTitle = new Label() { Text = "🗓  FACULTY LEAVE & QUOTA MANAGEMENT", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = primaryColor, Location = new Point(30, 20), AutoSize = true };
            pnlHeader.Controls.Add(lblTitle);
            Panel pnlAccent = new Panel() { Dock = DockStyle.Bottom, Height = 3, BackColor = primaryColor };
            pnlHeader.Controls.Add(pnlAccent);
            this.Controls.Add(pnlHeader);

            // 2. Content
            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25, 30, 25, 25) };
            this.Controls.Add(pnlScroll);

            FlowLayoutPanel flpMain = new FlowLayoutPanel() { FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = true, Dock = DockStyle.Top };
            pnlScroll.Controls.Add(flpMain);

            // --- QUOTA CARDS ---
            flpMain.Controls.Add(CreateSectionHeader("CURRENT LEAVE QUOTA (BALANCES)"));
            TableLayoutPanel tlpStats = new TableLayoutPanel() { Width = 1000, Height = 130, ColumnCount = 3, Margin = new Padding(0, 0, 0, 30) };
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));

            lblSickBalance = CreateValLabel("08 DAYS", Color.FromArgb(52, 152, 219));
            lblCasualBalance = CreateValLabel("04 DAYS", Color.FromArgb(241, 196, 15));
            lblEarnedBalance = CreateValLabel("12 DAYS", Color.FromArgb(46, 204, 113));

            tlpStats.Controls.Add(CreateStatCard("SICK LEAVE BALANCE", lblSickBalance, Color.FromArgb(52, 152, 219)), 0, 0);
            tlpStats.Controls.Add(CreateStatCard("CASUAL LEAVE BALANCE", lblCasualBalance, Color.FromArgb(241, 196, 15)), 1, 0);
            tlpStats.Controls.Add(CreateStatCard("EARNED LEAVE BALANCE", lblEarnedBalance, Color.FromArgb(46, 204, 113)), 2, 0);
            flpMain.Controls.Add(tlpStats);

            // --- APPLICATION FORM ---
            flpMain.Controls.Add(CreateSectionHeader("APPLICATION FOR LEAVE (MANUAL DAYS ENTRY)"));
            Panel pnlForm = new Panel() { Width = 1000, Height = 280, BackColor = cardBg, Padding = new Padding(20), Margin = new Padding(0, 0, 0, 40) };
            pnlForm.Paint += (s, e) => DrawBorder(e.Graphics, pnlForm.ClientRectangle);
            
            TableLayoutPanel tlpForm = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 2 };
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));

            cmbLeaveType = AddDropdown(tlpForm, "Leave Type", new string[] { "Sick Leave", "Casual Leave", "Earned Leave", "Duty Leave" }, 0, 0);
            dtpStart = AddDatePicker(tlpForm, "Start Date", 1, 0);
            dtpEnd = AddDatePicker(tlpForm, "End Date", 2, 0);
            
            // DAY SELECTION BOX (REPLACED LABEL WITH NUMERIC UPDOWN)
            Panel wDays = new Panel() { Dock = DockStyle.Top, Height = 65, Padding = new Padding(5) };
            Label lDays = new Label() { Text = "Duration (Days)", ForeColor = Color.Gray, Font = new Font("Segoe UI", 8, FontStyle.Bold), Dock = DockStyle.Top };
            numDaysRequested = new NumericUpDown() { 
                Dock = DockStyle.Top, 
                Minimum = 1, 
                Maximum = 365, 
                Value = 1, 
                BackColor = Color.White, 
                ForeColor = Color.FromArgb(46, 204, 113), 
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = HorizontalAlignment.Center
            };
            wDays.Controls.AddRange(new Control[] { numDaysRequested, lDays });
            tlpForm.Controls.Add(wDays, 3, 0);

            txtReason = AddTextbox(tlpForm, "Reason for Leave", 0, 1, 2);
            
            Button btnSubmit = new Button() { 
                Text = "📤 SUBMIT & UPDATE QUOTA", 
                Dock = DockStyle.Top, 
                Height = 45, 
                BackColor = primaryColor, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Font = new Font("Segoe UI", 10, FontStyle.Bold), 
                Margin = new Padding(10, 20, 0, 0) 
            };
            btnSubmit.Click += (s, e) => SubmitLeave();
            tlpForm.Controls.Add(btnSubmit, 2, 1);
            tlpForm.SetColumnSpan(btnSubmit, 2);

            pnlForm.Controls.Add(tlpForm);
            flpMain.Controls.Add(pnlForm);

            // --- HISTORY TABLE ---
            flpMain.Controls.Add(CreateSectionHeader("MY LEAVE APPLICATION HISTORY"));
            Panel pnlGridWrap = new Panel() { Width = 1000, Height = 350, BackColor = cardBg, Padding = new Padding(1) };
            dgvLeaveHistory = CreateStyledGrid(dtLeaveHistory);
            pnlGridWrap.Controls.Add(dgvLeaveHistory);
            flpMain.Controls.Add(pnlGridWrap);

            pnlScroll.Resize += (s, e) => {
                int w = Math.Max(800, pnlScroll.Width - 70);
                flpMain.Width = pnlScroll.Width;
                foreach (Control c in flpMain.Controls) if (c is Panel || c is TableLayoutPanel) c.Width = w;
            };
        }

        private void SetupEvents()
        {
            dtpStart.ValueChanged += (s, e) => SyncDaysFromDates();
            dtpEnd.ValueChanged += (s, e) => SyncDaysFromDates();
            
            // If user manually changes days, update the end date automatically
            numDaysRequested.ValueChanged += (s, e) => SyncDatesFromDays();
        }

        private void SyncDaysFromDates()
        {
            TimeSpan ts = dtpEnd.Value.Date - dtpStart.Value.Date;
            int days = ts.Days + 1;
            
            if (days > 0) {
                numDaysRequested.Value = Math.Min(numDaysRequested.Maximum, (decimal)days);
                numDaysRequested.ForeColor = Color.FromArgb(46, 204, 113);
            } else {
                numDaysRequested.ForeColor = Color.Red;
            }
        }

        private void SyncDatesFromDays()
        {
            int days = (int)numDaysRequested.Value;
            if (days > 0) {
                dtpEnd.Value = dtpStart.Value.AddDays(days - 1);
                numDaysRequested.ForeColor = Color.FromArgb(46, 204, 113);
            }
        }

        private void SubmitLeave()
        {
            if (string.IsNullOrWhiteSpace(txtReason.Text)) {
                MessageBox.Show("Please provide a reason.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int days = (int)numDaysRequested.Value;
            if (days <= 0) {
                MessageBox.Show("Invalid number of days selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Calculation for early availability check
            UpdateBalanceDisplay(); // Refresh to get current truth
            string type = cmbLeaveType.Text;
            int currentBalance = 0;
            if (type == "Sick Leave") currentBalance = int.Parse(lblSickBalance.Text.Replace(" DAYS", ""));
            else if (type == "Casual Leave") currentBalance = int.Parse(lblCasualBalance.Text.Replace(" DAYS", ""));
            else if (type == "Earned Leave") currentBalance = int.Parse(lblEarnedBalance.Text.Replace(" DAYS", ""));

            if (days > currentBalance) {
                MessageBox.Show($"Insufficient {type} balance! You only have {currentBalance} days left.", "Quota Exhausted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Record in Table - This is the SINGLE SOURCE OF TRUTH
            dtLeaveHistory.Rows.InsertAt(dtLeaveHistory.NewRow(), 0);
            dtLeaveHistory.Rows[0][0] = "LV-" + new Random().Next(100, 999);
            dtLeaveHistory.Rows[0][1] = type;
            dtLeaveHistory.Rows[0][2] = dtpStart.Value.ToShortDateString();
            dtLeaveHistory.Rows[0][3] = dtpEnd.Value.ToShortDateString();
            dtLeaveHistory.Rows[0][4] = days.ToString() + (days == 1 ? " Day" : " Days");
            dtLeaveHistory.Rows[0][5] = txtReason.Text;
            dtLeaveHistory.Rows[0][6] = "Approved"; // Automark as Approved for this dummy interactive demo

            // TRIGGER STATS UPDATE BASED ON THE TABLE
            UpdateBalanceDisplay();
            
            txtReason.Clear();
            MessageBox.Show($"Application for {days} Day(s) Approved and Deducted from Quota.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private Label CreateValLabel(string text, Color c) => new Label() { Text = text, ForeColor = Color.RoyalBlue, Font = new Font("Segoe UI", 20, FontStyle.Bold), AutoSize = true };

        private Panel CreateStatCard(string title, Label val, Color accent) {
            Panel p = new Panel() { Dock = DockStyle.Fill, BackColor = cardBg, Margin = new Padding(0, 0, 15, 0) };
            p.Paint += (s, e) => DrawBorder(e.Graphics, p.ClientRectangle);
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label head = new Label() { Text = title, ForeColor = Color.Gray, Font = new Font("Segoe UI", 8, FontStyle.Bold), Location = new Point(15, 12), AutoSize = true };
            val.Location = new Point(15, 40);
            p.Controls.AddRange(new Control[] { l, head, val });
            return p;
        }

        private TextBox AddTextbox(TableLayoutPanel p, string label, int col, int row, int colSpan) {
            Panel wrap = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(10, 5, 10, 5) };
            Label lbl = new Label() { Text = label, ForeColor = Color.FromArgb(173, 22, 37), Font = new Font("Segoe UI", 8, FontStyle.Bold), Dock = DockStyle.Top };
            TextBox txt = new TextBox() { Dock = DockStyle.Top, BackColor = Color.White, ForeColor = Color.RoyalBlue, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 11), Multiline = row > 0, Height = 35 };
            wrap.Controls.AddRange(new Control[] { txt, lbl });
            p.Controls.Add(wrap, col, row); if (colSpan > 1) p.SetColumnSpan(wrap, colSpan);
            return txt;
        }

        private ComboBox AddDropdown(TableLayoutPanel p, string label, string[] items, int col, int row) {
            Panel wrap = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(10, 5, 10, 5) };
            Label lbl = new Label() { Text = label, ForeColor = Color.FromArgb(173, 22, 37), Font = new Font("Segoe UI", 8, FontStyle.Bold), Dock = DockStyle.Top };
            ComboBox cb = new ComboBox() { Dock = DockStyle.Top, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.RoyalBlue, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cb.Items.AddRange(items); cb.SelectedIndex = 0;
            wrap.Controls.AddRange(new Control[] { cb, lbl });
            p.Controls.Add(wrap, col, row);
            return cb;
        }

        private DateTimePicker AddDatePicker(TableLayoutPanel p, string label, int col, int row) {
            Panel wrap = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(10, 5, 10, 5) };
            Label lbl = new Label() { Text = label, ForeColor = Color.FromArgb(173, 22, 37), Font = new Font("Segoe UI", 8, FontStyle.Bold), Dock = DockStyle.Top };
            DateTimePicker dtp = new DateTimePicker() { Dock = DockStyle.Top, Format = DateTimePickerFormat.Short };
            wrap.Controls.AddRange(new Control[] { dtp, lbl });
            p.Controls.Add(wrap, col, row);
            return dtp;
        }

        private DataGridView CreateStyledGrid(DataTable dt) {
            DataGridView d = new DataGridView() { Dock = DockStyle.Fill, DataSource = dt, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, ForeColor = Color.FromArgb(40, 40, 40), GridColor = Color.FromArgb(220, 220, 220), RowTemplate = { Height = 40 }, ColumnHeadersHeight = 45, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, EnableHeadersVisualStyles = false };
            d.ColumnHeadersDefaultCellStyle.BackColor = primaryColor; d.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; d.DefaultCellStyle.BackColor = Color.White; d.DefaultCellStyle.SelectionBackColor = primaryColor;
            return d;
        }

        private Label CreateSectionHeader(string text) => new Label() { Text = "──  " + text, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = primaryColor, AutoSize = true, Margin = new Padding(0, 10, 0, 15) };

        private void DrawBorder(Graphics g, Rectangle r) {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen pen = new Pen(borderColor, 1)) g.DrawRectangle(pen, r.X, r.Y, r.Width - 1, r.Height - 1);
        }

        private void InitializeComponent() {
            this.SuspendLayout();
            this.Name = "FacultyLeaveControl";
            this.Size = new Size(1100, 1100);
            this.ResumeLayout(false);
        }
    }
}
