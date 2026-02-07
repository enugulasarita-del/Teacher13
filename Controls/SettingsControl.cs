using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public partial class SettingsControl : UserControl
    {
        private string userName;
        private string userRole;
        
        // Theme Colors
        private Color primaryRed = Color.FromArgb(173, 22, 37);
        private Color darkRed = Color.FromArgb(140, 20, 30);
        private Color lightRed = Color.FromArgb(195, 40, 55);
        private Color cardBg = Color.White;
        private Color textPrimary = Color.FromArgb(40, 40, 40);
        private Color textSecondary = Color.FromArgb(120, 120, 120);
        private Color borderColor = Color.FromArgb(230, 230, 230);

        public SettingsControl(string name, string role)
        {
            InitializeComponent();
            this.userName = name;
            this.userRole = role;
            this.Dock = DockStyle.Fill;
            SetupSettingsUI();
        }

        private void SetupSettingsUI()
        {
            this.Controls.Clear();

            // Main Layout
            TableLayoutPanel rootLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.FromArgb(245, 245, 245)
            };
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.Controls.Add(rootLayout);

            // Header
            Panel pnlHeader = CreateHeader();
            rootLayout.Controls.Add(pnlHeader, 0, 0);

            // Scrollable Content
            Panel pnlScroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(30)
            };
            
            // Background pattern
            pnlScroll.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    pnlScroll.ClientRectangle,
                    Color.FromArgb(245, 245, 245),
                    Color.FromArgb(250, 250, 250),
                    45f))
                {
                    e.Graphics.FillRectangle(brush, pnlScroll.ClientRectangle);
                }
            };

            FlowLayoutPanel flpMain = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                Width = 1000
            };

            // Add sections
            flpMain.Controls.Add(CreateProfileSection());
            flpMain.Controls.Add(CreatePasswordSection());
            flpMain.Controls.Add(CreateNotificationSection());
            flpMain.Controls.Add(CreateDetailsSection());

            pnlScroll.Controls.Add(flpMain);
            rootLayout.Controls.Add(pnlScroll, 0, 1);
        }

        private Panel CreateHeader()
        {
            Panel header = new Panel { Dock = DockStyle.Fill };
            
            header.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    header.ClientRectangle,
                    Color.White,
                    Color.FromArgb(255, 250, 250),
                    LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, header.ClientRectangle);
                }
                
                using (Pen borderPen = new Pen(Color.FromArgb(240, 240, 240), 2))
                {
                    e.Graphics.DrawLine(borderPen, 0, header.Height - 1, header.Width, header.Height - 1);
                }
            };

            Label lblTitle = new Label
            {
                Text = "⚙️ SETTINGS",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = primaryRed,
                Location = new Point(30, 20),
                AutoSize = true
            };

            Label lblSubtitle = new Label
            {
                Text = "Manage your profile and preferences",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = textSecondary,
                Location = new Point(34, 60),
                AutoSize = true
            };

            header.Controls.AddRange(new Control[] { lblTitle, lblSubtitle });
            return header;
        }

        private Panel CreateProfileSection()
        {
            Panel section = CreateStyledCard("👤 PROFILE INFORMATION", 450);

            // Profile Picture
            Panel picPanel = new Panel
            {
                Size = new Size(120, 120),
                Location = new Point(30, 70),
                BackColor = Color.FromArgb(250, 250, 250)
            };
            
            picPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, 120, 120);
                    picPanel.Region = new Region(path);
                    
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        picPanel.ClientRectangle,
                        primaryRed,
                        lightRed,
                        45f))
                    {
                        e.Graphics.FillEllipse(brush, 0, 0, 120, 120);
                    }
                    
                    string initial = userName.Substring(0, 1).ToUpper();
                    using (Font font = new Font("Segoe UI", 48, FontStyle.Bold))
                    {
                        SizeF size = e.Graphics.MeasureString(initial, font);
                        e.Graphics.DrawString(initial, font, Brushes.White,
                            (120 - size.Width) / 2, (120 - size.Height) / 2);
                    }
                }
            };

            Button btnChangePhoto = CreateModernButton("Change Photo", 120, 35);
            btnChangePhoto.Location = new Point(30, 200);
            btnChangePhoto.Click += (s, e) => MessageBox.Show("Photo upload feature coming soon!", "Info");

            // Input Fields
            Label lblName = CreateLabel("Full Name:", 180, 70);
            TextBox txtName = CreateTextBox(userName, 180, 95);

            Label lblEmail = CreateLabel("Email Address:", 180, 140);
            TextBox txtEmail = CreateTextBox("teacher@vsit.edu.in", 180, 165);

            Label lblPhone = CreateLabel("Phone Number:", 180, 210);
            TextBox txtPhone = CreateTextBox("+91 98765 43210", 180, 235);
            txtPhone.Width = 350; // Ensure consistent width

            Button btnUpdate = CreateModernButton("UPDATE PROFILE", 200, 45);
            btnUpdate.Location = new Point(180, 290);
            btnUpdate.Click += (s, e) => MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            section.Controls.AddRange(new Control[] { 
                picPanel, btnChangePhoto, 
                lblName, txtName, 
                lblEmail, txtEmail, 
                lblPhone, txtPhone, 
                btnUpdate 
            });

            return section;
        }

        private Panel CreatePasswordSection()
        {
            Panel section = CreateStyledCard("🔐 CHANGE PASSWORD", 350);

            Label lblCurrent = CreateLabel("Current Password:", 30, 70);
            TextBox txtCurrent = CreateTextBox("", 30, 95);
            txtCurrent.PasswordChar = '●';

            Label lblNew = CreateLabel("New Password:", 30, 140);
            TextBox txtNew = CreateTextBox("", 30, 165);
            txtNew.PasswordChar = '●';

            Label lblConfirm = CreateLabel("Confirm New Password:", 30, 210);
            TextBox txtConfirm = CreateTextBox("", 30, 235);
            txtConfirm.PasswordChar = '●';

            Button btnChange = CreateModernButton("CHANGE PASSWORD", 200, 45);
            btnChange.Location = new Point(30, 290);
            btnChange.Click += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtCurrent.Text) || string.IsNullOrEmpty(txtNew.Text))
                {
                    MessageBox.Show("Please fill all fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtNew.Text != txtConfirm.Text)
                {
                    MessageBox.Show("New passwords don't match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                MessageBox.Show("Password changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCurrent.Clear();
                txtNew.Clear();
                txtConfirm.Clear();
            };

            section.Controls.AddRange(new Control[] { 
                lblCurrent, txtCurrent, 
                lblNew, txtNew, 
                lblConfirm, txtConfirm, 
                btnChange 
            });

            return section;
        }

        private Panel CreateNotificationSection()
        {
            Panel section = CreateStyledCard("🔔 NOTIFICATION PREFERENCES", 250);

            Label lblEmail = CreateLabel("Email Notifications:", 30, 70);
            CheckBox chkEmail = CreateToggleSwitch(true);
            chkEmail.Location = new Point(250, 70);

            Label lblSMS = CreateLabel("SMS Notifications:", 30, 120);
            CheckBox chkSMS = CreateToggleSwitch(false);
            chkSMS.Location = new Point(250, 120);

            Button btnSave = CreateModernButton("SAVE PREFERENCES", 200, 45);
            btnSave.Location = new Point(30, 180);
            btnSave.Click += (s, e) => MessageBox.Show("Preferences saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            section.Controls.AddRange(new Control[] { lblEmail, chkEmail, lblSMS, chkSMS, btnSave });

            return section;
        }

        private Panel CreateDetailsSection()
        {
            Panel section = CreateStyledCard("ℹ️ EMPLOYMENT DETAILS (Read-Only)", 350);

            int y = 70;
            section.Controls.Add(CreateDetailRow("Employee ID:", "EMP2024001", ref y));
            section.Controls.Add(CreateDetailRow("Department:", "Computer Science", ref y));
            section.Controls.Add(CreateDetailRow("Designation:", userRole, ref y));
            section.Controls.Add(CreateDetailRow("Subjects Assigned:", "Data Structures, Algorithms, DBMS", ref y));
            section.Controls.Add(CreateDetailRow("Date of Joining:", "15-Aug-2020", ref y));

            return section;
        }

        private Panel CreateDetailRow(string label, string value, ref int yPos)
        {
            Panel row = new Panel
            {
                Size = new Size(900, 45),
                Location = new Point(30, yPos),
                BackColor = Color.FromArgb(252, 252, 252)
            };

            row.Paint += (s, e) =>
            {
                using (Pen borderPen = new Pen(Color.FromArgb(240, 240, 240), 1))
                {
                    e.Graphics.DrawRectangle(borderPen, 0, 0, row.Width - 1, row.Height - 1);
                }
            };

            Label lblKey = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = primaryRed,
                Location = new Point(15, 12),
                AutoSize = true
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 10),
                ForeColor = textPrimary,
                Location = new Point(250, 12),
                AutoSize = true
            };

            row.Controls.AddRange(new Control[] { lblKey, lblValue });
            yPos += 50;
            return row;
        }

        private Panel CreateStyledCard(string title, int height)
        {
            Panel card = new Panel
            {
                Width = 950,
                Height = height,
                BackColor = cardBg,
                Margin = new Padding(0, 0, 0, 25)
            };

            // Title panel with gradient
            Panel titlePanel = new Panel { Dock = DockStyle.Top, Height = 50 };
            titlePanel.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    titlePanel.ClientRectangle,
                    Color.FromArgb(255, 250, 250),
                    Color.White,
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, titlePanel.ClientRectangle);
                }
                
                using (Pen accentPen = new Pen(primaryRed, 2))
                {
                    e.Graphics.DrawLine(accentPen, 0, titlePanel.Height - 1, titlePanel.Width, titlePanel.Height - 1);
                }
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = primaryRed,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            titlePanel.Controls.Add(lblTitle);
            card.Controls.Add(titlePanel);

            // Shadow and border
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                
                // Multi-layer shadow
                for (int i = 5; i > 0; i--)
                {
                    using (GraphicsPath shadowPath = CreateRoundedRect(i, i, card.Width - (i * 2), card.Height - (i * 2), 10))
                    {
                        using (Pen shadowPen = new Pen(Color.FromArgb(6, 0, 0, 0), 2))
                        {
                            e.Graphics.DrawPath(shadowPen, shadowPath);
                        }
                    }
                }
                
                // Border
                using (GraphicsPath borderPath = CreateRoundedRect(0, 0, card.Width - 1, card.Height - 1, 10))
                {
                    using (Pen borderPen = new Pen(primaryRed, 2))
                    {
                        e.Graphics.DrawPath(borderPen, borderPath);
                    }
                }
            };

            return card;
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = textPrimary,
                Location = new Point(x, y),
                AutoSize = true
            };
        }

        private TextBox CreateTextBox(string text, int x, int y)
        {
            TextBox txt = new TextBox
            {
                Text = text,
                Font = new Font("Segoe UI", 10),
                Size = new Size(350, 30),
                Location = new Point(x, y),
                BorderStyle = BorderStyle.FixedSingle
            };
            return txt;
        }

        private Button CreateModernButton(string text, int width, int height)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(width, height),
                BackColor = primaryRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;

            btn.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = CreateRoundedRect(0, 0, btn.Width - 1, btn.Height - 1, 8))
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        btn.ClientRectangle,
                        btn.BackColor,
                        ControlPaint.Light(btn.BackColor, 0.2f),
                        LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
                
                // Explicitly draw text over the path
                TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, btn.ForeColor, 
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            btn.MouseEnter += (s, e) => btn.BackColor = lightRed;
            btn.MouseLeave += (s, e) => btn.BackColor = primaryRed;

            return btn;
        }

        private CheckBox CreateToggleSwitch(bool isChecked)
        {
            CheckBox chk = new CheckBox
            {
                Size = new Size(60, 30),
                Checked = isChecked,
                Appearance = Appearance.Button,
                FlatStyle = FlatStyle.Flat,
                BackColor = isChecked ? primaryRed : Color.Gray,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Text = isChecked ? "ON" : "OFF",
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };

            chk.FlatAppearance.BorderSize = 0;

            chk.CheckedChanged += (s, e) =>
            {
                chk.BackColor = chk.Checked ? primaryRed : Color.Gray;
                chk.Text = chk.Checked ? "ON" : "OFF";
            };

            chk.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = CreateRoundedRect(0, 0, chk.Width - 1, chk.Height - 1, 15))
                {
                    chk.Region = new Region(path);
                    using (SolidBrush brush = new SolidBrush(chk.BackColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
                // Explicitly draw toggle text
                TextRenderer.DrawText(e.Graphics, chk.Text, chk.Font, chk.ClientRectangle, chk.ForeColor, 
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            return chk;
        }

        private GraphicsPath CreateRoundedRect(int x, int y, int width, int height, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, radius, radius, 180, 90);
            path.AddArc(x + width - radius, y, radius, radius, 270, 90);
            path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
            path.AddArc(x, y + height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
