using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public partial class CommunicationControl : UserControl
    {
        public CommunicationControl()
        {
            InitializeComponent();
            SetupStrictLayout();
            LoadMessages();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblMainTitle = new Label() { Text = "COMMUNICATION HUB", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblMainTitle);
            this.Controls.Add(pnlHeader);

            // 2. Toolbar & Category Explorer
            Panel pnlToolbar = new Panel() { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(20, 10, 20, 10) };
            Button btnNew = new Button() { Text = "✉ SEND NEW MESSAGE", Location = new Point(20, 12), Size = new Size(180, 35), BackColor = Color.FromArgb(173, 22, 37), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnNew.FlatAppearance.BorderSize = 0;
            
            FlowLayoutPanel flpCats = new FlowLayoutPanel() { Location = new Point(220, 15), Size = new Size(600, 40) };
            string[] cats = { "INBOX", "SENT", "URGENT", "ARCHIVE" };
            foreach (var cat in cats)
            {
                Button b = new Button() { Text = cat, AutoSize = true, FlatStyle = FlatStyle.Flat, ForeColor = Color.LightGray, Font = new Font("Segoe UI", 8, FontStyle.Bold), Margin = new Padding(10, 0, 0, 0) };
                b.FlatAppearance.BorderSize = 0;
                flpCats.Controls.Add(b);
            }
            
            pnlToolbar.Controls.AddRange(new Control[] { btnNew, flpCats });
            this.Controls.Add(pnlToolbar);

            // 3. Main Splitter
            SplitContainer mainSplit = new SplitContainer() { Dock = DockStyle.Fill, SplitterDistance = 400, Padding = new Padding(20) };
            this.Controls.Add(mainSplit);

            // Left: Inbox List + Pinned
            Panel pnlInbox = new Panel() { Dock = DockStyle.Fill };
            Label lblIn = new Label() { Text = "RECENT CONVERSATIONS", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.DimGray, Dock = DockStyle.Top, Height = 30 };
            pnlInbox.Controls.Add(lblIn);

            this.dgvMessages = new DataGridView() { 
                Dock = DockStyle.Top, 
                Height = 350,
                BackgroundColor = Color.FromArgb(28, 28, 28), 
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 40,
                GridColor = Color.FromArgb(45, 45, 45)
            };
            this.dgvMessages.DefaultCellStyle.BackColor = Color.White;
            mainSplit.Panel1.Controls.Add(pnlInbox);
            pnlInbox.Controls.Add(this.dgvMessages);

            // Right: Reader + Quick Reply Placeholder
            Panel pnlReader = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(25) };
            Label lblMsgHead = new Label() { Text = "MESSAGE PREVIEW", Font = new Font("Segoe UI", 12, FontStyle.Bold), Dock = DockStyle.Top, Height = 40, ForeColor = Color.White };
            pnlReader.Controls.Add(lblMsgHead);
            
            Panel pnlContent = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(28, 28, 28), Padding = new Padding(20) };
            Label lblContent = new Label() { 
                Text = "Select a message to view details.\n\nYou can quick-reply or flag important communications here.\n\nNote: Attachments are scanned for security before download.", 
                Font = new Font("Segoe UI", 10), 
                Dock = DockStyle.Fill, 
                ForeColor = Color.FromArgb(180, 180, 180) 
            };
            pnlContent.Controls.Add(lblContent);
            pnlReader.Controls.Add(pnlContent);

            // Related Feature: Quick Send Panel at bottom of reader
            Panel pnlQuickSend = new Panel() { Dock = DockStyle.Bottom, Height = 120, BackColor = Color.FromArgb(45, 45, 45), Padding = new Padding(15) };
            pnlQuickSend.Controls.Add(new TextBox() { Multiline = true, Dock = DockStyle.Fill, Text = "Type a quick reply...", ForeColor = Color.Gray });
            Button btnSend = new Button() { Text = "SEND ➔", Dock = DockStyle.Right, Width = 80, BackColor = Color.FromArgb(173, 22, 37), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            pnlQuickSend.Controls.Add(btnSend);
            pnlReader.Controls.Add(pnlQuickSend);

            mainSplit.Panel2.Controls.Add(pnlReader);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 2);  // Docks First (Top)
            this.Controls.SetChildIndex(pnlToolbar, 1); // Docks Second (Top)
            this.Controls.SetChildIndex(mainSplit, 0);  // Docks Last (Fill)
        }

        private void LoadMessages()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Date");
            dt.Columns.Add("From/To");
            dt.Columns.Add("Subject");

            dt.Rows.Add("2026-01-31", "Principal's Office", "Faculty Meeting");
            dt.Rows.Add("2026-01-30", "Exam Dept", "Marksheets Ready");
            dt.Rows.Add("2026-01-28", "Student Council", "Event Request");

            if (this.dgvMessages != null)
            {
                this.dgvMessages.DataSource = dt;
                this.dgvMessages.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }
    }
}
