using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace TeacherDashboard.Controls
{
    public partial class SettingsControl : UserControl
    {
        private Color primaryColor = Color.FromArgb(173, 22, 37);
        private Color bgColor = Color.FromArgb(18, 18, 18);
        private Color cardBg = Color.FromArgb(30, 30, 33);
        private Color borderColor = Color.FromArgb(45, 45, 48);

        public SettingsControl()
        {
            InitializeComponent();
            SetupLayout();
        }

        private void SetupLayout()
        {
            this.Controls.Clear();
            this.BackColor = bgColor;
            this.Dock = DockStyle.Fill;

            // Root Layout
            TableLayoutPanel rootLayout = new TableLayoutPanel();
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F)); // Header
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Body
            this.Controls.Add(rootLayout);

            // Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(25, 25, 25) };
            Label lblTitle = new Label() { 
                Text = "⚙️  SYSTEM SETTINGS & PREFERENCES", 
                Font = new Font("Segoe UI", 18, FontStyle.Bold), 
                ForeColor = Color.White, 
                Location = new Point(30, 25), 
                AutoSize = true 
            };
            pnlHeader.Controls.Add(lblTitle);
            Panel accent = new Panel() { Dock = DockStyle.Bottom, Height = 3, BackColor = primaryColor };
            pnlHeader.Controls.Add(accent);
            rootLayout.Controls.Add(pnlHeader, 0, 0);

            // Scrollable Body
            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, BackColor = bgColor, Padding = new Padding(30) };
            rootLayout.Controls.Add(pnlScroll, 0, 1);

            FlowLayoutPanel flpMain = new FlowLayoutPanel() { 
                Dock = DockStyle.Top, 
                FlowDirection = FlowDirection.TopDown, 
                WrapContents = false, 
                AutoSize = true, 
                Width = 1000 
            };
            pnlScroll.Controls.Add(flpMain);

            // PROFILE SETTINGS
            flpMain.Controls.Add(CreateSectionTitle("PROFILE INFORMATION"));
            Panel pnlProfile = new Panel() { Width = 1000, Height = 280, BackColor = cardBg, Margin = new Padding(0, 0, 0, 30) };
            pnlProfile.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(borderColor, 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlProfile.Width - 1, pnlProfile.Height - 1);
            };
            
            TableLayoutPanel tlpProfile = new TableLayoutPanel() { 
                Dock = DockStyle.Fill, 
                ColumnCount = 2, 
                RowCount = 3, 
                Padding = new Padding(10) 
            };
            tlpProfile.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tlpProfile.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tlpProfile.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F)); // Row 0
            tlpProfile.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F)); // Row 1
            tlpProfile.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F)); // Row 2
            
            AddSettingRow(tlpProfile, "Full Name", "Prof. John Doe", 0, 0);
            AddSettingRow(tlpProfile, "Employee ID", "FAC-2024-1234", 1, 0);
            AddSettingRow(tlpProfile, "Department", "Computer Science", 0, 1);
            AddSettingRow(tlpProfile, "Email Address", "john.doe@vsit.edu.in", 1, 1);
            AddSettingRow(tlpProfile, "Contact Number", "+91 98765 43210", 0, 2);
            AddSettingRow(tlpProfile, "Office Location", "Block A, Room 305", 1, 2);
            
            pnlProfile.Controls.Add(tlpProfile);
            flpMain.Controls.Add(pnlProfile);

            // NOTIFICATION PREFERENCES
            flpMain.Controls.Add(CreateSectionTitle("NOTIFICATION PREFERENCES"));
            Panel pnlNotif = CreateSettingsCard();
            FlowLayoutPanel flpNotif = new FlowLayoutPanel() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(20) };
            
            flpNotif.Controls.Add(CreateToggleOption("Email Notifications", "Receive email alerts for important updates", true));
            flpNotif.Controls.Add(CreateToggleOption("Assignment Reminders", "Get notified when assignments are due", true));
            flpNotif.Controls.Add(CreateToggleOption("Student Queries", "Receive notifications for student messages", true));
            flpNotif.Controls.Add(CreateToggleOption("System Updates", "Alerts for system maintenance and updates", false));
            
            pnlNotif.Controls.Add(flpNotif);
            flpMain.Controls.Add(pnlNotif);

            // DISPLAY PREFERENCES
            flpMain.Controls.Add(CreateSectionTitle("DISPLAY & INTERFACE"));
            Panel pnlDisplay = CreateSettingsCard();
            FlowLayoutPanel flpDisplay = new FlowLayoutPanel() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(20) };
            
            Panel pnlTheme = new Panel() { Width = 920, Height = 60, Margin = new Padding(0, 0, 0, 10) };
            Label lblTheme = new Label() { Text = "Theme Mode", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.White, Location = new Point(0, 5), AutoSize = true };
            ComboBox cmbTheme = new ComboBox() { 
                Location = new Point(0, 30), 
                Width = 300, 
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            cmbTheme.Items.AddRange(new string[] { "Dark Mode (Current)", "Light Mode (Coming Soon)" });
            cmbTheme.SelectedIndex = 0;
            pnlTheme.Controls.AddRange(new Control[] { lblTheme, cmbTheme });
            flpDisplay.Controls.Add(pnlTheme);

            Panel pnlLang = new Panel() { Width = 920, Height = 60, Margin = new Padding(0, 0, 0, 10) };
            Label lblLang = new Label() { Text = "Language", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.White, Location = new Point(0, 5), AutoSize = true };
            ComboBox cmbLang = new ComboBox() { 
                Location = new Point(0, 30), 
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            cmbLang.Items.AddRange(new string[] { "English (US)", "हिन्दी (Hindi)", "मराठी (Marathi)" });
            cmbLang.SelectedIndex = 0;
            pnlLang.Controls.AddRange(new Control[] { lblLang, cmbLang });
            flpDisplay.Controls.Add(pnlLang);
            
            pnlDisplay.Controls.Add(flpDisplay);
            flpMain.Controls.Add(pnlDisplay);

            // SECURITY SETTINGS
            flpMain.Controls.Add(CreateSectionTitle("SECURITY & PRIVACY"));
            Panel pnlSecurity = CreateSettingsCard();
            FlowLayoutPanel flpSecurity = new FlowLayoutPanel() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(20) };
            
            Button btnChangePass = CreateActionButton("🔒 Change Password", "Update your account password");
            btnChangePass.Click += (s, e) => MessageBox.Show("Password change functionality will be implemented here.", "Security", MessageBoxButtons.OK, MessageBoxIcon.Information);
            flpSecurity.Controls.Add(btnChangePass);
            
            Button btnTwoFactor = CreateActionButton("🛡️ Enable Two-Factor Authentication", "Add an extra layer of security");
            flpSecurity.Controls.Add(btnTwoFactor);
            
            Button btnSessions = CreateActionButton("📱 Manage Active Sessions", "View and manage logged-in devices");
            flpSecurity.Controls.Add(btnSessions);
            
            pnlSecurity.Controls.Add(flpSecurity);
            flpMain.Controls.Add(pnlSecurity);

            // SYSTEM INFORMATION
            flpMain.Controls.Add(CreateSectionTitle("SYSTEM INFORMATION"));
            Panel pnlSystem = CreateSettingsCard();
            FlowLayoutPanel flpSystem = new FlowLayoutPanel() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(20) };
            
            flpSystem.Controls.Add(CreateInfoRow("Application Version", "v2.5.1 (Build 2026.02.03)"));
            flpSystem.Controls.Add(CreateInfoRow("Last Login", DateTime.Now.AddHours(-2).ToString("dd MMM yyyy, hh:mm tt")));
            flpSystem.Controls.Add(CreateInfoRow("Database Status", "✅ Connected"));
            flpSystem.Controls.Add(CreateInfoRow("Storage Used", "2.4 GB / 10 GB"));
            
            pnlSystem.Controls.Add(flpSystem);
            flpMain.Controls.Add(pnlSystem);

            // ACTIONS
            Panel pnlActions = new Panel() { Width = 1000, Height = 80, Margin = new Padding(0, 20, 0, 50) };
            Button btnSave = new Button() { 
                Text = "💾 SAVE CHANGES", 
                Size = new Size(200, 50), 
                BackColor = primaryColor, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(0, 15)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) => MessageBox.Show("Settings saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            Button btnReset = new Button() { 
                Text = "🔄 RESET TO DEFAULT", 
                Size = new Size(200, 50), 
                BackColor = Color.FromArgb(45, 45, 48), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(220, 15)
            };
            btnReset.FlatAppearance.BorderSize = 0;
            
            pnlActions.Controls.AddRange(new Control[] { btnSave, btnReset });
            flpMain.Controls.Add(pnlActions);

            pnlScroll.Resize += (s, e) => {
                int w = pnlScroll.Width - 80;
                if (w < 800) w = 800;
                flpMain.Width = w;
            };
        }

        private Panel CreateSettingsCard()
        {
            Panel p = new Panel() { Width = 1000, Height = 250, BackColor = cardBg, Margin = new Padding(0, 0, 0, 30) };
            p.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(borderColor, 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
            };
            return p;
        }

        private void AddSettingRow(TableLayoutPanel tlp, string label, string value, int col, int row)
        {
            Panel pnl = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(10) };
            Label lbl = new Label() { Text = label, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.Gray, Dock = DockStyle.Top, Height = 20 };
            TextBox txt = new TextBox() { 
                Text = value, 
                Dock = DockStyle.Top, 
                BackColor = Color.FromArgb(45, 45, 48), 
                ForeColor = Color.White, 
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10),
                Height = 30
            };
            pnl.Controls.AddRange(new Control[] { txt, lbl });
            tlp.Controls.Add(pnl, col, row);
        }

        private Panel CreateToggleOption(string title, string desc, bool enabled)
        {
            Panel p = new Panel() { Width = 920, Height = 60, Margin = new Padding(0, 0, 0, 10) };
            Label lblTitle = new Label() { Text = title, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.White, Location = new Point(0, 5), AutoSize = true };
            Label lblDesc = new Label() { Text = desc, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(0, 30), AutoSize = true };
            CheckBox chk = new CheckBox() { 
                Checked = enabled, 
                Location = new Point(880, 15), 
                Width = 30, 
                Height = 30,
                ForeColor = Color.White
            };
            p.Controls.AddRange(new Control[] { lblTitle, lblDesc, chk });
            return p;
        }

        private Button CreateActionButton(string text, string desc)
        {
            Button btn = new Button() { 
                Width = 920, 
                Height = 60, 
                BackColor = Color.FromArgb(38, 38, 42), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 10)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Text = text + "\n" + desc;
            return btn;
        }

        private Panel CreateInfoRow(string label, string value)
        {
            Panel p = new Panel() { Width = 920, Height = 40, Margin = new Padding(0, 0, 0, 5) };
            Label lbl = new Label() { Text = label, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Gray, Location = new Point(0, 10), Width = 300 };
            Label val = new Label() { Text = value, Font = new Font("Segoe UI", 10), ForeColor = Color.White, Location = new Point(320, 10), AutoSize = true };
            p.Controls.AddRange(new Control[] { lbl, val });
            return p;
        }

        private Label CreateSectionTitle(string text)
        {
            return new Label() { 
                Text = "──  " + text, 
                Font = new Font("Segoe UI", 10, FontStyle.Bold), 
                ForeColor = primaryColor, 
                AutoSize = true, 
                Margin = new Padding(0, 20, 0, 15) 
            };
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "SettingsControl";
            this.Size = new Size(1200, 800);
            this.ResumeLayout(false);
        }
    }
}
