using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace TeacherDashboard.Controls
{
    public partial class AdminAnnouncementsControl : UserControl
    {
        private DataTable dtAnnouncements;
        private DataView dvAnnouncements; // For filtering
        private DataGridView dgvAnnouncements;
        private TextBox txtTitle, txtContent;
        private ComboBox cmbTarget, cmbType;
        private ComboBox cmbFilterTarget; // Filter dropdown
        private DateTimePicker dtpSchedule;
        private Button btnSend;
        private Panel pnlLeft, pnlRight;

        public AdminAnnouncementsControl()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            InitializeData();
            SetupLayout();
        }

        private void InitializeData()
        {
            dtAnnouncements = new DataTable();
            dtAnnouncements.Columns.Add("Title");
            dtAnnouncements.Columns.Add("Target");
            dtAnnouncements.Columns.Add("Type");
            dtAnnouncements.Columns.Add("Date", typeof(DateTime));
            dtAnnouncements.Columns.Add("Status");

            // Mock Data
            dtAnnouncements.Rows.Add("Faculty Meeting", "All Teachers", "Urgent", DateTime.Now.AddDays(1), "Scheduled");
            dtAnnouncements.Rows.Add("Exam Schedules Released", "Exam Dept", "Info", DateTime.Now, "Sent");
            dtAnnouncements.Rows.Add("Holiday Notice", "All Teachers", "General", DateTime.Now.AddDays(-2), "Sent");
            dtAnnouncements.Rows.Add("CSE Review", "Department: CSE", "Meeting", DateTime.Now.AddDays(2), "Scheduled");
            dtAnnouncements.Rows.Add("ECE Lab Update", "Department: ECE", "Info", DateTime.Now.AddDays(-1), "Sent");
            dtAnnouncements.Rows.Add("B.Sc IT Syllabus Update", "Department: B.Sc. IT", "Info", DateTime.Now.AddDays(-3), "Sent");
            dtAnnouncements.Rows.Add("Placement Drive - TCS", "Department: B.Sc. IT", "Placement", DateTime.Now.AddDays(5), "Scheduled");
            dtAnnouncements.Rows.Add("Data Science Workshop", "Department: B.Sc. Data Science", "Workshop", DateTime.Now.AddDays(3), "Scheduled");
            dtAnnouncements.Rows.Add("BMS Guest Lecture", "Department: BMS/BBI", "Event", DateTime.Now.AddDays(4), "Scheduled");
            dtAnnouncements.Rows.Add("BBI Finance Seminar", "Department: BMS/BBI", "Seminar", DateTime.Now.AddDays(-5), "Sent");
            dtAnnouncements.Rows.Add("HOD Monthly Meet", "HODs Only", "Confidential", DateTime.Now.AddDays(1), "Scheduled");
            dtAnnouncements.Rows.Add("Exam Paper Submission", "Examination Cell", "Deadline", DateTime.Now.AddDays(2), "Scheduled");
            dtAnnouncements.Rows.Add("Cultural Fest Planning", "All Teachers", "Event", DateTime.Now.AddDays(10), "Scheduled");
            dtAnnouncements.Rows.Add("Library Stock Check", "All Teachers", "Info", DateTime.Now.AddDays(-10), "Sent");
            dtAnnouncements.Rows.Add("Result Analysis - Sem 3", "Department: CSE", "Report", DateTime.Now.AddDays(-4), "Sent");

            dvAnnouncements = new DataView(dtAnnouncements);
        }

        private void SetupLayout()
        {
            TableLayoutPanel master = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2, BackColor = Color.White };
            master.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60)); // History
            master.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40)); // Compose
            master.RowStyles.Add(new RowStyle(SizeType.Absolute, 70)); // Header
            master.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            this.Controls.Add(master);

            // HEADER
            Panel pnlHeader = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(173, 22, 37) };
            master.SetColumnSpan(pnlHeader, 2);
            Label lblTitle = new Label() { Text = "📢 ANNOUNCEMENTS CENTER", Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(20, 0, 0, 0) };
            pnlHeader.Controls.Add(lblTitle);
            master.Controls.Add(pnlHeader, 0, 0);

            // LEFT: HISTORY W/ FILTER
            pnlLeft = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(20) };
            
            // Toolbar
            Panel pnlToolbar = new Panel() { Dock = DockStyle.Top, Height = 40 };
            Label lblHist = new Label() { Text = "PAST ANNOUNCEMENTS", ForeColor = Color.Gray, Font = new Font("Segoe UI", 12, FontStyle.Bold), AutoSize = true, Location = new Point(0, 5) };
            
            cmbFilterTarget = new ComboBox() { Width = 200, Height = 30, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.RoyalBlue, Font = new Font("Segoe UI", 9), Location = new Point(250, 2), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbFilterTarget.Items.Add("Filter: All");
            cmbFilterTarget.Items.AddRange(new string[] { "All Teachers", "Department: CSE", "Department: ECE", "Department: BMS/BBI", "HODs Only", "Examination Cell" });
            cmbFilterTarget.SelectedIndex = 0;
            cmbFilterTarget.SelectedIndexChanged += FilterAnnouncements;

            pnlToolbar.Controls.Add(lblHist);
            pnlToolbar.Controls.Add(cmbFilterTarget);

            dgvAnnouncements = new DataGridView() { 
                Dock = DockStyle.Fill, 
                DataSource = dvAnnouncements, // Use DataView
                BackgroundColor = Color.White, 
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 40,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 45 }
            };
            dgvAnnouncements.DefaultCellStyle.BackColor = Color.White;
            dgvAnnouncements.DefaultCellStyle.ForeColor = Color.RoyalBlue;
            dgvAnnouncements.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(64, 64, 64);
            dgvAnnouncements.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAnnouncements.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            // Add Delete Button Column
            DataGridViewButtonColumn btnDel = new DataGridViewButtonColumn() { HeaderText = "Action", Text = "🗑️", UseColumnTextForButtonValue = true, FlatStyle = FlatStyle.Flat, Width = 60 };
            btnDel.DefaultCellStyle.BackColor = Color.FromArgb(231, 76, 60);
            btnDel.DefaultCellStyle.ForeColor = Color.White;
            dgvAnnouncements.Columns.Add(btnDel);

            dgvAnnouncements.CellClick += (s, e) => {
                if(e.RowIndex >= 0 && e.ColumnIndex == dgvAnnouncements.Columns.Count - 1) {
                    // Safe Delete for DataView
                    DataRowView drv = (DataRowView)dgvAnnouncements.Rows[e.RowIndex].DataBoundItem;
                    if(drv != null) {
                        string title = drv["Title"].ToString();
                        if(MessageBox.Show($"Delete announcement '{title}'?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                           drv.Row.Delete(); 
                    }
                }
            };

            pnlLeft.Controls.Add(dgvAnnouncements);
            pnlLeft.Controls.Add(pnlToolbar);
            master.Controls.Add(pnlLeft, 0, 1);

            // RIGHT: COMPOSE
            pnlRight = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 245, 245), Padding = new Padding(20) };
            Label lblComp = new Label() { Text = "CREATE NEW ANNOUNCEMENT", ForeColor = Color.FromArgb(241, 196, 15), Font = new Font("Segoe UI", 12, FontStyle.Bold), Dock = DockStyle.Top, Height = 40 };
            Panel pnlForm = new Panel() { Dock = DockStyle.Fill, AutoScroll = true };
            
            int y = 0;
            txtTitle = CreateInput(pnlForm, "Title / Subject *", ref y);
            
            // Target Audience
            Label lT = new Label() { Text = "Target Audience *", ForeColor = Color.Gray, Location = new Point(0, y), AutoSize = true };
            cmbTarget = new ComboBox() { Location = new Point(0, y + 25), Width = 300, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.RoyalBlue, Font = new Font("Segoe UI", 10) };
            cmbTarget.Items.AddRange(new string[] { "All Teachers", "Department: CSE", "Department: ECE", "Department: BMS/BBI", "HODs Only", "Examination Cell" });
            pnlForm.Controls.AddRange(new Control[] { lT, cmbTarget });
            y += 70;

            // Type
            Label lType = new Label() { Text = "Announcement Type", ForeColor = Color.Gray, Location = new Point(0, y), AutoSize = true };
            cmbType = new ComboBox() { Location = new Point(0, y + 25), Width = 300, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.RoyalBlue, Font = new Font("Segoe UI", 10) };
            cmbType.Items.AddRange(new string[] { "General Info", "Urgent Alert", "Meeting Request", "Holiday Notice" });
            pnlForm.Controls.AddRange(new Control[] { lType, cmbType });
            y += 70;

            // Content
            Label lC = new Label() { Text = "Message Content *", ForeColor = Color.Gray, Location = new Point(0, y), AutoSize = true };
            txtContent = new TextBox() { Location = new Point(0, y + 25), Width = 300, Height = 120, Multiline = true, BackColor = Color.White, ForeColor = Color.RoyalBlue, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10) };
            pnlForm.Controls.AddRange(new Control[] { lC, txtContent });
            y += 160;

            // Schedule
            Label lD = new Label() { Text = "Schedule For (Optional)", ForeColor = Color.Gray, Location = new Point(0, y), AutoSize = true };
            dtpSchedule = new DateTimePicker() { Location = new Point(0, y + 25), Width = 200, Format = DateTimePickerFormat.Short };
            pnlForm.Controls.AddRange(new Control[] { lD, dtpSchedule });
            y += 70;

            btnSend = new Button() { Text = "🚀 SEND ANNOUNCEMENT", Width = 300, Height = 45, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(0, y + 10) };
            btnSend.Click += SendAnnouncement;
            pnlForm.Controls.Add(btnSend);

            pnlRight.Controls.Add(pnlForm);
            pnlRight.Controls.Add(lblComp);
            master.Controls.Add(pnlRight, 1, 1);
        }

        private void FilterAnnouncements(object sender, EventArgs e)
        {
            if (cmbFilterTarget.SelectedIndex <= 0) // "Filter: All"
            {
                dvAnnouncements.RowFilter = "";
            }
            else
            {
                string target = cmbFilterTarget.Text;
                // Filter by exact match on the "Target" column
                dvAnnouncements.RowFilter = $"Target = '{target}'";
            }
        }

        private TextBox CreateInput(Panel p, string label, ref int y)
        {
            Label l = new Label() { Text = label, ForeColor = Color.Gray, Location = new Point(0, y), AutoSize = true };
            TextBox t = new TextBox() { Location = new Point(0, y + 25), Width = 300, BackColor = Color.White, ForeColor = Color.RoyalBlue, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10) };
            p.Controls.AddRange(new Control[] { l, t });
            y += 70;
            return t;
        }

        private void SendAnnouncement(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(cmbTarget.Text)) {
                MessageBox.Show("Please fill all required fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string status = dtpSchedule.Value.Date > DateTime.Now.Date ? "Scheduled" : "Sent";
            string msg = status == "Scheduled" ? $"Announcement Scheduled for {dtpSchedule.Value.ToShortDateString()}" : "Announcement Sent Successfully!";
            
            dtAnnouncements.Rows.Add(txtTitle.Text, cmbTarget.Text, cmbType.Text, dtpSchedule.Value, status);
            MessageBox.Show(msg + "\n\n📧 Notification emails dispatched.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // Clear
            txtTitle.Clear();
            txtContent.Clear();
            cmbTarget.SelectedIndex = -1;
        }
    }
}
