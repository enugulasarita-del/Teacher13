using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data;

namespace TeacherDashboard.Controls
{
    public partial class AdminLeaveRequestsControl : UserControl
    {
        private Color primaryColor = Color.FromArgb(173, 22, 37); // VSIT Red
        private Color acceptColor = Color.FromArgb(46, 204, 113); // Green for Approve
        private Color rejectColor = Color.FromArgb(231, 76, 60);  // Red for Reject
        private Color cardBg = Color.White;
        private Color lightGray = Color.FromArgb(245, 245, 245);

        private DataTable dtRequests;
        private DataGridView dgvRequests;

        public AdminLeaveRequestsControl()
        {
            InitializeComponent();
            SetupData();
            SetupLayout();
        }

        private void SetupData()
        {
            dtRequests = new DataTable();
            dtRequests.Columns.Add("ReqID");
            dtRequests.Columns.Add("Faculty");
            dtRequests.Columns.Add("Dept");
            dtRequests.Columns.Add("Type");
            dtRequests.Columns.Add("Dates");
            dtRequests.Columns.Add("Days");
            dtRequests.Columns.Add("Reason");
            dtRequests.Columns.Add("Status");

            // Pending Requests
            dtRequests.Rows.Add("REQ-089", "Prof. Anita Sharma", "IT", "Sick Leave", "10 Feb - 11 Feb", "2", "High Fever", "Pending");
            dtRequests.Rows.Add("REQ-090", "Mr. Amit Verma", "CS", "Casual Leave", "15 Feb", "1", "Family Function", "Pending");
            dtRequests.Rows.Add("REQ-091", "Dr. Rajesh Kumar", "IT", "Duty Leave", "12 Feb", "1", "University Exam Duty", "Pending");
            
            // Historical
            dtRequests.Rows.Add("REQ-085", "Ms. Priya Singh", "BMS", "Earned Leave", "01 Feb - 05 Feb", "5", "Vacation", "Approved");
            dtRequests.Rows.Add("REQ-082", "Mr. Rohan Das", "CS", "Sick Leave", "20 Jan", "1", "Migraine", "Rejected");
        }

        private void SetupLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 120, BackColor = Color.White, Padding = new Padding(30, 25, 30, 0) };
            
            FlowLayoutPanel tlpHead = new FlowLayoutPanel() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = true };
            
            Label lblTitle = new Label() { Text = "✈️ FACULTY LEAVE MANAGEMENT", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = primaryColor, AutoSize = true, Margin = new Padding(0, 0, 0, 5) };
            Label lblSubtitle = new Label() { Text = "Review and approve leave applications from teaching and non-teaching staff", Font = new Font("Segoe UI", 11), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(4, 0, 0, 0) };
            
            tlpHead.Controls.Add(lblTitle);
            tlpHead.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(tlpHead);

            Panel pnlAccent = new Panel() { Dock = DockStyle.Bottom, Height = 4, BackColor = primaryColor };
            pnlHeader.Controls.Add(pnlAccent);
            
            // 2. Stats Panel
            Panel pnlStats = new Panel() { Dock = DockStyle.Top, Height = 120, BackColor = lightGray, Padding = new Padding(30, 20, 30, 10) };
            TableLayoutPanel tlpStats = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 3 };
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));

            tlpStats.Controls.Add(CreateStatCard("PENDING REQUESTS", "3", Color.Orange), 0, 0);
            tlpStats.Controls.Add(CreateStatCard("APPROVED THIS MONTH", "12", acceptColor), 1, 0);
            tlpStats.Controls.Add(CreateStatCard("REJECTED THIS MONTH", "2", rejectColor), 2, 0);
            
            pnlStats.Controls.Add(tlpStats);

            // 3. Grid
            Panel pnlGrid = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(30) };
            dgvRequests = new DataGridView() {
                Dock = DockStyle.Fill,
                DataSource = dtRequests,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeight = 50,
                RowTemplate = { Height = 45 },
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(230, 230, 230)
            };

            dgvRequests.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            dgvRequests.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRequests.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvRequests.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvRequests.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 235, 238);
            dgvRequests.DefaultCellStyle.SelectionForeColor = primaryColor;

            // Action Buttons
            DataGridViewButtonColumn btnApprove = new DataGridViewButtonColumn() { 
                Name = "Approve", HeaderText = "Approve", Text = "✔", UseColumnTextForButtonValue = true, Width = 60, FlatStyle = FlatStyle.Flat 
            };
            btnApprove.DefaultCellStyle.BackColor = acceptColor;
            btnApprove.DefaultCellStyle.ForeColor = Color.White;

            DataGridViewButtonColumn btnReject = new DataGridViewButtonColumn() { 
                Name = "Reject", HeaderText = "Reject", Text = "✖", UseColumnTextForButtonValue = true, Width = 60, FlatStyle = FlatStyle.Flat 
            };
            btnReject.DefaultCellStyle.BackColor = rejectColor;
            btnReject.DefaultCellStyle.ForeColor = Color.White;

            dgvRequests.Columns.Add(btnApprove);
            dgvRequests.Columns.Add(btnReject);
            dgvRequests.CellClick += HandleActions;

            pnlGrid.Controls.Add(dgvRequests);

            // Add to Main Controls in Reverse Order (Bottom-up) to fix Docking Overlap
            this.Controls.Add(pnlGrid);   // Fill (Bottom z-order)
            this.Controls.Add(pnlStats);  // Top (Middle z-order)
            this.Controls.Add(pnlHeader); // Top (Top z-order)
        }

        private Panel CreateStatCard(string title, string value, Color color)
        {
            Panel p = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0, 0, 20, 0) };
            p.Paint += (s, e) => ControlPaint.DrawBorder(e.Graphics, p.ClientRectangle, Color.LightGray, ButtonBorderStyle.Solid);
            
            Panel bar = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = color };
            Label lT = new Label() { Text = title, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(15, 15), AutoSize = true };
            Label lV = new Label() { Text = value, ForeColor = Color.Black, Font = new Font("Segoe UI", 20, FontStyle.Bold), Location = new Point(15, 40), AutoSize = true };
            
            p.Controls.Add(bar);
            p.Controls.Add(lT);
            p.Controls.Add(lV);
            return p;
        }

        private void HandleActions(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string status = dtRequests.Rows[e.RowIndex]["Status"].ToString();
            if (status != "Pending") return; // Can only act on Pending

            if (dgvRequests.Columns[e.ColumnIndex].Name == "Approve")
            {
                if (MessageBox.Show("Approve this leave request?", "Confirm Approval", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dtRequests.Rows[e.RowIndex]["Status"] = "Approved";
                    MessageBox.Show("Leave Request Approved.", "Success");
                }
            }
            else if (dgvRequests.Columns[e.ColumnIndex].Name == "Reject")
            {
                if (MessageBox.Show("Reject this leave request?", "Confirm Rejection", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    dtRequests.Rows[e.RowIndex]["Status"] = "Rejected";
                     MessageBox.Show("Leave Request Rejected.", "Updated");
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "AdminLeaveRequestsControl";
            this.Size = new Size(1100, 800);
            this.ResumeLayout(false);
        }
    }
}
