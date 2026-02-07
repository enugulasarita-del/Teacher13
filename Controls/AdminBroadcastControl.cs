using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace TeacherDashboard.Controls
{
    public partial class AdminBroadcastControl : UserControl
    {
        // Theme Colors
        private Color primaryColor = Color.FromArgb(173, 22, 37); // VSIT Red
        private Color darkColor = Color.RoyalBlue;
        private Color blueColor = Color.FromArgb(41, 128, 185);
        private Color lightGray = Color.FromArgb(245, 245, 245);

        // UI Components
        private DateTimePicker dtpDate;
        private ComboBox cmbType;
        private TextBox txtSubject;
        private ComboBox cmbFrom;
        private ComboBox cmbTo;
        private TextBox txtMessage;
        private RadioButton rbNormal;
        private RadioButton rbUrgent;
        private DataGridView dgvRecent;
        private DataTable dtRecent;

        public AdminBroadcastControl()
        {
            InitializeComponent();
            SetupData();
            SetupLayout();
        }

        private void SetupData()
        {
            dtRecent = new DataTable();
            dtRecent.Columns.Add("Date");
            dtRecent.Columns.Add("Type");
            dtRecent.Columns.Add("Subject");
            dtRecent.Columns.Add("From");
            dtRecent.Columns.Add("Status");

            // Sample rows to show:
            dtRecent.Rows.Add("Feb 03", "MEETING", "Urgent Faculty Meeting: Exam Duty Allocation", "Principal Office", "Sent (03 Feb, 10:00 AM)");
            dtRecent.Rows.Add("Feb 04", "ADMIN", "Internal Marks Deadline - Semester IV", "Exam Dept", "Sent (04 Feb, 11:30 AM)");
            dtRecent.Rows.Add("Feb 05", "MEETING", "Research Committee: Weekly Review", "HOD - BSc IT", "Sent (05 Feb, 02:15 PM)");
            dtRecent.Rows.Add("Feb 06", "NOTICE", "Maintenance: IT Lab 3 Internet Downtime", "Sys Admin", "Sent (06 Feb, 09:45 AM)");
        }

        private void SetupLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            // 2. Scrollable Container (Add first so header can sit on top)
            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(0) };
            this.Controls.Add(pnlScroll);

            // 1. Fixed Header (Add second so it's on top)
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 120, BackColor = Color.White, Padding = new Padding(30, 20, 30, 0) };
            
            FlowLayoutPanel tlpHead = new FlowLayoutPanel() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = true };
            
            Label lblTitle = new Label() { Text = "📢 GLOBAL ANNOUNCER", Font = new Font("Segoe UI", 26, FontStyle.Bold), ForeColor = primaryColor, AutoSize = true, Margin = new Padding(0, 0, 0, 5) };
            Label lblSubtitle = new Label() { Text = "Dispatch official notices, meetings, and urgent alerts to the faculty", Font = new Font("Segoe UI", 12), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(4, 0, 0, 0) };
            
            tlpHead.Controls.Add(lblTitle);
            tlpHead.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(tlpHead);

            Panel pnlAccent = new Panel() { Dock = DockStyle.Bottom, Height = 5, BackColor = primaryColor };
            pnlHeader.Controls.Add(pnlAccent);
            this.Controls.Add(pnlHeader);

            // Fix Docking Z-Order
            pnlHeader.BringToFront();
            pnlScroll.SendToBack();

            FlowLayoutPanel flpMain = new FlowLayoutPanel() { 
                Dock = DockStyle.Top, 
                FlowDirection = FlowDirection.TopDown, 
                WrapContents = false, 
                AutoSize = true, 
                Padding = new Padding(30, 40, 30, 50),
                BackColor = lightGray
            };
            pnlScroll.Controls.Add(flpMain);

            // FORM SECTION (Centered Container)
            Panel pnlFormContainer = new Panel() { 
                Width = 950, 
                AutoSize = true, 
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.Transparent, 
                Padding = new Padding(0, 0, 0, 30) 
            };
            flpMain.Controls.Add(pnlFormContainer);

            Panel pnlForm = new Panel() { 
                Width = 950,
                BackColor = Color.White, 
                Padding = new Padding(30), 
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                MinimumSize = new Size(800, 500)
            };
            pnlForm.Paint += (s, e) => ControlPaint.DrawBorder(e.Graphics, pnlForm.ClientRectangle, Color.FromArgb(220, 220, 220), ButtonBorderStyle.Solid);
            pnlFormContainer.Controls.Add(pnlForm);

            // The following lines are added based on the user's instruction, assuming 'master' is intended to be declared and used here.
            // However, 'master' is not declared in the original context, so this might be an incomplete snippet or intended for a different control.
            // For the purpose of faithful reproduction, it's inserted as provided.
            TableLayoutPanel master = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2 };
            master.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60)); // History
            master.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40)); // Compose
            master.RowStyles.Add(new RowStyle(SizeType.Absolute, 70)); // Header
            master.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            this.Controls.Add(master);

            master.BackColor = Color.White;

            TableLayoutPanel tlpForm = new TableLayoutPanel() { 
                Width = 890,
                ColumnCount = 2, 
                RowCount = 9, 
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(0)
            };
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180f));
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            pnlForm.Controls.Add(tlpForm);

            int row = 0;

            // 1. Type
            tlpForm.Controls.Add(CreateFieldLabel("Type:*", 0, 0), 0, row);
            cmbType = new ComboBox() { 
                Width = 250, 
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = blueColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 20)
            };
            cmbType.Items.AddRange(new string[] { "-- Select Type --", "MEETING", "ADMIN", "NOTICE", "ANNOUNCEMENT", "URGENT" });
            cmbType.SelectedIndex = 0;
            tlpForm.Controls.Add(cmbType, 1, row++);

            // 2. Broadcast Date
            tlpForm.Controls.Add(CreateFieldLabel("Broadcast Date:*", 0, 0), 0, row);
            dtpDate = new DateTimePicker() { Width = 250, Font = new Font("Segoe UI", 11), Margin = new Padding(0, 0, 0, 20) };
            tlpForm.Controls.Add(dtpDate, 1, row++);

            // 3. Broadcast To (Audience)
            tlpForm.Controls.Add(CreateFieldLabel("Broadcast To:*", 0, 0), 0, row);
            cmbTo = new ComboBox() { 
                Width = 300, 
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.White,
                ForeColor = darkColor,
                Font = new Font("Segoe UI", 11),
                Margin = new Padding(0, 0, 0, 20)
            };
            cmbTo.Items.AddRange(new string[] { "-- Select Target Audience --", "All Faculty Members", "Departmental HODs", "BSc IT Department", "BSc CS Department", "BSc DS Department", "BMS/BBI Department", "Principal's Office", "Examination Cell" });
            cmbTo.SelectedIndex = 0;
            tlpForm.Controls.Add(cmbTo, 1, row++);

            // 4. Subject
            tlpForm.Controls.Add(CreateFieldLabel("Subject:*", 0, 0), 0, row);
            txtSubject = new TextBox() { 
                Width = 650,
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 50, 20)
            };
            ToolTip ttSubject = new ToolTip();
            ttSubject.SetToolTip(txtSubject, "Enter subject (e.g., Urgent Faculty Meeting: Exam Schedule)");
            tlpForm.Controls.Add(txtSubject, 1, row++);

            // 5. From (Sender)
            tlpForm.Controls.Add(CreateFieldLabel("From (Sender):*", 0, 0), 0, row);
            cmbFrom = new ComboBox() { 
                Width = 300, 
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = primaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 20)
            };
            cmbFrom.Items.AddRange(new string[] { "-- Select Department --", "Principal Office", "Exam Dept", "HOD - BSc IT", "Sys Admin", "Admin Office" });
            cmbFrom.SelectedIndex = 0;
            tlpForm.Controls.Add(cmbFrom, 1, row++);

            // 6. Message Content
            tlpForm.Controls.Add(CreateFieldLabel("Message Content:*", 0, 0), 0, row);
            txtMessage = new TextBox() { 
                Width = 650,
                Height = 150, 
                Multiline = true, 
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = ScrollBars.Vertical,
                Margin = new Padding(0, 0, 50, 20)
            };
            tlpForm.Controls.Add(txtMessage, 1, row++);

            // 6. Priority
            tlpForm.Controls.Add(CreateFieldLabel("Priority Level:*", 0, 0), 0, row);
            FlowLayoutPanel flpPriority = new FlowLayoutPanel() { Width = 500, Height = 40, Margin = new Padding(0, 0, 0, 20) };
            rbNormal = new RadioButton() { Text = "Normal", Checked = true, AutoSize = true, Font = new Font("Segoe UI", 10) };
            rbUrgent = new RadioButton() { Text = "Urgent", AutoSize = true, Font = new Font("Segoe UI", 10), ForeColor = primaryColor };
            flpPriority.Controls.AddRange(new Control[] { rbNormal, rbUrgent });
            tlpForm.Controls.Add(flpPriority, 1, row++);

            // 7. Action Buttons
            FlowLayoutPanel flpButtons = new FlowLayoutPanel() { Width = 600, Height = 60, Margin = new Padding(0, 10, 0, 0) };
            Button btnSend = new Button() { 
                Text = "📤 SEND BROADCAST", 
                Width = 250, 
                Height = 50, 
                BackColor = primaryColor, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Margin = new Padding(0, 0, 20, 0)
            };
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.Click += BtnSend_Click;

            Button btnClear = new Button() { 
                Text = "🗑️ CLEAR FORM", 
                Width = 180, 
                Height = 50, 
                BackColor = Color.Gray, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Font = new Font("Segoe UI", 10, FontStyle.Bold) 
            };
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.Click += (s, e) => ClearForm();

            flpButtons.Controls.AddRange(new Control[] { btnSend, btnClear });
            tlpForm.Controls.Add(flpButtons, 1, row++);

            // RECENT BROADCASTS SECTION
            flpMain.Controls.Add(new Panel() { Height = 40 }); // Spacer
            Label lblRecentTitle = new Label() { 
                Text = "RECENT BROADCASTS - Last 5 Sent", 
                Font = new Font("Segoe UI", 16, FontStyle.Bold), 
                ForeColor = primaryColor, 
                AutoSize = true, 
                Margin = new Padding(0, 0, 0, 15) 
            };
            flpMain.Controls.Add(lblRecentTitle);

            dgvRecent = new DataGridView() { 
                Width = 1000, 
                Height = 350, 
                BackgroundColor = Color.White, 
                BorderStyle = BorderStyle.None,
                DataSource = dtRecent,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowTemplate = { Height = 45 },
                ColumnHeadersHeight = 50,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            };
            dgvRecent.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            dgvRecent.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRecent.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgvRecent.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRecent.DefaultCellStyle.BackColor = Color.White;
            dgvRecent.DefaultCellStyle.ForeColor = darkColor;
            dgvRecent.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvRecent.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRecent.GridColor = Color.FromArgb(230, 230, 230);
            
            // Formatting for Priority/Type
            dgvRecent.CellFormatting += (s, e) => {
                if (dgvRecent.Columns[e.ColumnIndex].Name == "Type") {
                    e.CellStyle.ForeColor = blueColor;
                    e.CellStyle.Font = new Font(dgvRecent.Font, FontStyle.Bold);
                }
                if (e.RowIndex >= 0) {
                    string subject = dgvRecent.Rows[e.RowIndex].Cells["Subject"].Value?.ToString();
                    if (subject != null && subject.Contains("Maintenance")) {
                        dgvRecent.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                        dgvRecent.Rows[e.RowIndex].DefaultCellStyle.ForeColor = primaryColor;
                    }
                }
            };

            flpMain.Controls.Add(dgvRecent);

            // Resize Support - FULL WIDTH
            pnlScroll.Resize += (s, e) => {
                int targetWidth = pnlScroll.ClientSize.Width - 60; // 30px padding on each side
                if (targetWidth < 800) targetWidth = 800;
                
                flpMain.Width = pnlScroll.ClientSize.Width;
                pnlFormContainer.Width = targetWidth;
                pnlForm.Width = targetWidth;
                tlpForm.Width = pnlForm.Width - 60;
                dgvRecent.Width = targetWidth;

                // Sync child control widths for full horizontal stretch
                txtSubject.Width = tlpForm.Width - 200;
                txtMessage.Width = tlpForm.Width - 200;
            };
        }

        private Label CreateFieldLabel(string text, int x, int y)
        {
            return new Label() { 
                Text = text, 
                Location = new Point(x, y + 4), 
                AutoSize = true, 
                Font = new Font("Segoe UI", 10, FontStyle.Bold), 
                ForeColor = primaryColor 
            };
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (cmbType.SelectedIndex == 0 || cmbFrom.SelectedIndex == 0 || cmbTo.SelectedIndex == 0 || string.IsNullOrWhiteSpace(txtSubject.Text) || string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                MessageBox.Show("Please fill all required fields (*) before sending.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Success Logic
            DataRow row = dtRecent.NewRow();
            row["Date"] = dtpDate.Value.ToString("MMM dd");
            row["Type"] = cmbType.SelectedItem.ToString();
            row["Subject"] = txtSubject.Text;
            row["From"] = cmbFrom.SelectedItem.ToString();
            row["Status"] = "Sent (" + DateTime.Now.ToString("dd MMM, hh:mm tt") + ")";
            dtRecent.Rows.InsertAt(row, 0);

            MessageBox.Show("✓ Broadcast sent successfully to all teachers!", "Broadcast Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            ClearForm();
        }

        private void ClearForm()
        {
            dtpDate.Value = DateTime.Now;
            cmbType.SelectedIndex = 0;
            cmbFrom.SelectedIndex = 0;
            cmbTo.SelectedIndex = 0;
            txtSubject.Clear();
            txtMessage.Clear();
            rbNormal.Checked = true;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "AdminBroadcastControl";
            this.Size = new Size(1100, 800);
            this.ResumeLayout(false);
        }
    }
}
