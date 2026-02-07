using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace TeacherDashboard.Controls
{
    public partial class CommunicationControl : UserControl
    {
        // Theme Colors
        private Color primaryColor = Color.FromArgb(173, 22, 37); // VSIT Red
        private Color bgColor = Color.White;
        private Color cardBg = Color.White;
        private Color borderColor = Color.FromArgb(220, 220, 220);

        // UI Components
        private DataGridView dgvNotices;
        private DataTable dtNotices;
        
        // Detail Controls
        private Label lblSelectedSubject;
        private Label lblSelectedMeta;
        private TextBox txtSelectedBody;
        private Panel pnlBottomDetail;

        public CommunicationControl()
        {
            InitializeComponent();
            SetupData();
            SetupStrictNonOverlappingLayout();
            if (dgvNotices.Rows.Count > 0) UpdateDetailDisplay(0);
        }

        private void SetupData()
        {
            dtNotices = new DataTable();
            dtNotices.Columns.Add("Date");
            dtNotices.Columns.Add("Type");
            dtNotices.Columns.Add("Subject");
            dtNotices.Columns.Add("From");

            dtNotices.Rows.Add("Feb 03", "MEETING", "Urgent Faculty Meeting: Exam Duty Allocation", "Principal Office");
            dtNotices.Rows.Add("Feb 04", "ADMIN", "Internal Marks Deadline - Semester IV", "Exam Dept");
            dtNotices.Rows.Add("Feb 05", "MEETING", "Research Committee: Weekly Review", "HOD - BSc IT");
            dtNotices.Rows.Add("Feb 06", "NOTICE", "Maintenance: IT Lab 3 Internet Downtime", "Sys Admin");
        }

        private void SetupStrictNonOverlappingLayout()
        {
            // 1. CLEAR AND RESET
            this.Controls.Clear();
            this.BackColor = bgColor;
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(0);

            // 2. MAIN FLOW
            FlowLayoutPanel flpMain = new FlowLayoutPanel() {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = bgColor
            };
            this.Controls.Add(flpMain);

            // --- SECTION 1: HEADER ---
            Panel pnlHeader = new Panel() { Width = 1100, Height = 80, BackColor = Color.White };
            Label lblHeaderTitle = new Label() { 
                Text = "📩 OFFICE COMMUNICATION & MEETING HUB", 
                Font = new Font("Segoe UI", 20, FontStyle.Bold), 
                ForeColor = Color.FromArgb(173, 22, 37), 
                Location = new Point(30, 22), 
                AutoSize = true 
            };
            pnlHeader.Controls.Add(lblHeaderTitle);
            Panel pnlRedLine = new Panel() { Dock = DockStyle.Bottom, Height = 4, BackColor = primaryColor };
            pnlHeader.Controls.Add(pnlRedLine);
            flpMain.Controls.Add(pnlHeader);

            // --- SECTION 2: LIST OF NOTICES (DATA GRID) ---
            flpMain.Controls.Add(CreateSectionLabel("ADMIN BROADCASTS & MEETING LIST (Click to View)"));
            
            Panel pnlGridWrap = new Panel() { Width = 1000, Height = 250, BackColor = cardBg, Margin = new Padding(30, 0, 0, 20), Padding = new Padding(1) };
            dgvNotices = CreateStyledGrid(dtNotices);
            dgvNotices.CellClick += (s, e) => { if (e.RowIndex >= 0) UpdateDetailDisplay(e.RowIndex); };
            pnlGridWrap.Controls.Add(dgvNotices);
            flpMain.Controls.Add(pnlGridWrap);

            // --- SECTION 3: DETAILED VIEW ---
            flpMain.Controls.Add(CreateSectionLabel("MESSAGE / MEETING CONTENT"));

            pnlBottomDetail = new Panel() { 
                Width = 1000, 
                Height = 350, 
                BackColor = cardBg, 
                Margin = new Padding(30, 0, 0, 50), 
                Padding = new Padding(25) 
            };
            pnlBottomDetail.Paint += (s, e) => DrawBorder(e.Graphics, pnlBottomDetail.ClientRectangle);

            lblSelectedSubject = new Label() { 
                Text = "Subject Label", 
                Font = new Font("Segoe UI", 16, FontStyle.Bold), 
                ForeColor = Color.FromArgb(40, 40, 40), 
                Dock = DockStyle.Top, 
                Height = 40 
            };
            
            lblSelectedMeta = new Label() { 
                Text = "Meta Data Label", 
                Font = new Font("Segoe UI", 10, FontStyle.Italic), 
                ForeColor = primaryColor, 
                Dock = DockStyle.Top, 
                Height = 30 
            };

            txtSelectedBody = new TextBox() { 
                Multiline = true, 
                ReadOnly = true, 
                BackColor = cardBg, 
                ForeColor = Color.FromArgb(40, 40, 40), 
                BorderStyle = BorderStyle.None, 
                Font = new Font("Segoe UI", 11), 
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Vertical
            };

            pnlBottomDetail.Controls.Add(txtSelectedBody);
            pnlBottomDetail.Controls.Add(lblSelectedMeta);
            pnlBottomDetail.Controls.Add(lblSelectedSubject);
            
            flpMain.Controls.Add(pnlBottomDetail);

            this.Resize += (s, e) => {
                int targetWidth = this.Width - 60;
                pnlHeader.Width = this.Width;
                pnlGridWrap.Width = targetWidth;
                pnlBottomDetail.Width = targetWidth;
            };
        }
        private void UpdateDetailDisplay(int idx)
        {
            DataRow row = dtNotices.Rows[idx];
            string subject = row["Subject"].ToString();
            string from = row["From"].ToString();
            string date = row["Date"].ToString();
            string type = row["Type"].ToString();

            lblSelectedSubject.Text = subject.ToUpper();
            lblSelectedMeta.Text = $"📢 {type} | FROM: {from} | DATE: {date}";

            if (type == "MEETING")
            {
                txtSelectedBody.Text = "OFFICIAL MEETING INVITATION\r\n" +
                                       "--------------------------------------------------\r\n" +
                                       "📌 VENUE: Faculty Conference Hall (Level 2)\r\n" +
                                       "⏰ TIME: 11:30 AM Sharp\r\n" +
                                       "--------------------------------------------------\r\n\r\n" +
                                       "Agenda:\r\n" +
                                       "1. Finalization of Internal Examination schedules.\r\n" +
                                       "2. Review of student attendance for Semester IV.\r\n" +
                                       "3. Preparation for upcoming Institutional Audit.\r\n\r\n" +
                                       "Your physical presence is mandatory. Please mark your attendance at the entrance.";
            }
            else
            {
                txtSelectedBody.Text = "ADMINISTRATIVE NOTICE\r\n" +
                                       "--------------------------------------------------\r\n\r\n" +
                                       "Instructions:\r\n" +
                                       "Please be advised that the deadline for uploading internal marks is approaching. \r\n" +
                                       "Ensure all data is cross-verified for accuracy before final submission into the system.\r\n\r\n" +
                                       "Contact the Exam Department for any credential-related issues.\r\n\r\n" +
                                       "Regards,\r\n" + from;
            }
        }

        private Label CreateSectionLabel(string text)
        {
            return new Label() { 
                Text = "──  " + text, 
                Font = new Font("Segoe UI", 10, FontStyle.Bold), 
                ForeColor = primaryColor, 
                Width = 1000, 
                Height = 40, 
                TextAlign = ContentAlignment.BottomLeft,
                Margin = new Padding(30, 20, 0, 10)
            };
        }

        private DataGridView CreateStyledGrid(DataTable dt)
        {
            DataGridView d = new DataGridView() { 
                Dock = DockStyle.Fill, 
                DataSource = dt, 
                BackgroundColor = Color.White, 
                BorderStyle = BorderStyle.None, 
                ForeColor = Color.FromArgb(40, 40, 40), 
                GridColor = Color.FromArgb(220, 220, 220), 
                RowTemplate = { Height = 40 }, 
                ColumnHeadersHeight = 45, 
                AllowUserToAddRows = false, 
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false,
                MultiSelect = false
            };
            d.ColumnHeadersDefaultCellStyle.BackColor = primaryColor; 
            d.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; 
            d.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            d.DefaultCellStyle.BackColor = Color.White; 
            d.DefaultCellStyle.SelectionBackColor = primaryColor;
            d.DefaultCellStyle.SelectionForeColor = Color.White;
            d.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            return d;
        }

        private void DrawBorder(Graphics g, Rectangle r) {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen pen = new Pen(borderColor, 1)) g.DrawRectangle(pen, r.X, r.Y, r.Width - 1, r.Height - 1);
        }

        // InitializeComponent is in Designer.cs
    }
}
