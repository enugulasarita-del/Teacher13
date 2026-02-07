using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TeacherDashboard.Controls;

namespace TeacherDashboard
{
    public partial class MainForm : Form
    {
        private string userRole;
        private string userName;
        private System.Windows.Forms.Timer clockTimer;
        private bool isSidebarCollapsed = false;
        private System.Collections.Generic.Dictionary<Button, string> menuOriginalText = new System.Collections.Generic.Dictionary<Button, string>();
        public string UserRole => userRole;
        public string UserName => userName;

        public MainForm(string role, string name)
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Improve Performance
            this.userRole = role;
            this.userName = name;
            
            SetupMenu();
            ApplyModernMenuStyles();
            SetupHeaderStyles(); // Initialize Header Styles
            UpdateProfile();
            StartClock();
            
            // Load Default View
            SwitchPanel("DASHBOARD");
        }
        
        private void SetupMenu()
        {
            // Admin features removed. Only Teacher features are visible.
            
            // --- Common Features ---
            btnDashboard.Visible = true;
            btnSettings.Visible = true;
            btnLogout.Visible = true;

            // --- ADMIN FEATURES REMOVED ---
            if (lblAdminSection != null) lblAdminSection.Visible = false;
            if (btnManageTeachers != null) btnManageTeachers.Visible = false;
            if (btnAdminTimetable != null) btnAdminTimetable.Visible = false;
            if (btnAdminBroadcast != null) btnAdminBroadcast.Visible = false;
            if (btnAdminLeave != null) btnAdminLeave.Visible = false;
            if (btnAdminFeedback != null) btnAdminFeedback.Visible = false;

            // --- TEACHER Features (Always Visible Now) ---
            lblAcademic.Visible = true;
            btnClasses.Visible = true;
            btnStudents.Visible = true;
            btnSyllabus.Visible = true;
            btnAttendance.Visible = true;
            
            lblManagement.Visible = true;
            lblOperations.Visible = true;
            btnComm.Visible = true;           
            btnNotices.Visible = true;        
            btnQuiz.Visible = true;
            btnResources.Visible = true;
            btnExams.Visible = true;
            btnAssignments.Visible = true;
            btnLeave.Visible = true;
            btnReports.Visible = true;
        }

        private void UpdateProfile()
        {
            lblUserName.Text = userName.Equals("System Administrator", StringComparison.OrdinalIgnoreCase) ? "Teacher" : userName;
            lblRole.Text = userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase) ? "Faculty" : userRole;
            lblUserBadge.Text = lblUserName.Text.Substring(0, 1).ToUpper();
        }

        private void StartClock()
        {
            clockTimer = new System.Windows.Forms.Timer();
            clockTimer.Interval = 1000;
            clockTimer.Tick += (s, e) => {
                lblHeaderClock.Text = DateTime.Now.ToString("ddd, MMM dd | hh:mm tt");
            };
            clockTimer.Start();
        }

        private void SidebarButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null || btn.Tag == null) return;
            
            string viewTag = btn.Tag.ToString();
            
            if (viewTag == "LOGOUT")
            {
                Application.Exit();
                return;
            }

            SwitchPanel(viewTag);
        }

        private void SwitchPanel(string view)
        {
            UserControl control = null;
            string title = "Home > " + view; // Breadcrumb Format

            switch (view)
            {
                case "DASHBOARD":
                    var dash = new DashboardControl(userRole, userName);
                    dash.RequestViewChange += (tag) => SwitchPanel(tag);
                    control = dash;
                    title = "Dashboard Overview";
                    break;
                case "CLASSES":
                    control = new ClassesControl();
                    title = "Teaching Portfolio & Classes";
                    break;
                case "STUDENTS":
                    control = new StudentsControl();
                    title = "Student Performance Hub";
                    break;
                case "ASSIGNMENTS":
                    control = new AssignmentsControl();
                    title = "Assignment Management";
                    break;
                case "ATTENDANCE":
                    control = new AttendanceControl();
                    title = "Attendance Management Tracker";
                    break;
                case "MESSAGES":
                    control = new CommunicationControl();
                    title = "Official Communication Hub";
                    break;
                case "NOTICES":
                    control = new NoticesControl();
                    title = "Post Faculty & Student Notices";
                    break;
                case "REPORTS":
                    control = new ReportsControl();
                    title = "Academic & Faculty Reports";
                    break;
                case "RESOURCES":
                    control = new ResourcesControl();
                    title = "Digital Library & Resources";
                    break;
                case "SYLLABUS":
                    control = new SyllabusControl();
                    title = "Curriculum Tracker & Progress";
                    break;

                case "LEAVE":
                    control = new FacultyLeaveControl();
                    title = "Faculty Leave Management";
                    break;
                case "TESTS":
                    control = new TestsControl();
                    title = "Exam Portfolio & Duties";
                    break;
                case "QUIZ":
                    control = new QuizControl();
                    title = "Interactive Quiz Creator";
                    break;
                case "SETTINGS":
                    control = new SettingsControl(userName, userRole);
                    title = "System Settings & Preferences";
                    break;
                case "ADMIN_TIMETABLE":
                case "ADMIN_BROADCAST":
                case "ADMIN_LEAVE":
                case "ADMIN_FEEDBACK":
                case "MANAGE TEACHERS":
                    // Admin features disabled
                    control = new UserControl() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(30, 30, 30) };
                    Label lblAdminErr = new Label() { Text = "Access Denied. Faculty Admin permissions only.", ForeColor = Color.Red, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
                    control.Controls.Add(lblAdminErr);
                    title = "Access Denied";
                    break;
                // Add more cases as needed for other buttons
                default:
                    // If not implemented yet, show a placeholder
                    control = new UserControl() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(30, 30, 30) };
                    Label lbl = new Label() { Text = view + " View - Under Development", ForeColor = Color.Gray, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 12) };
                    control.Controls.Add(lbl);
                    break;
            }

            if (control != null)
            {
                lblCurrentView.Text = title;
                pnlContent.Controls.Clear();
                pnlContent.Controls.Add(control);
                control.Dock = DockStyle.Fill;
                
                HighlightActiveButton(view);
            }
        }

        private void HighlightActiveButton(string viewTag)
        {
            foreach (Control c in flpMenu.Controls)
            {
                if (c is Button btn)
                {
                    // Reset to default
                    btn.BackColor = Color.FromArgb(173, 22, 37); // Default Red
                    btn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                    
                    if (btn.Tag != null && btn.Tag.ToString() == viewTag)
                    {
                        // Active Style
                        btn.BackColor = Color.White; // Active Background
                        btn.ForeColor = Color.FromArgb(173, 22, 37); // Active Text (Red)
                    }
                    else
                    {
                        btn.ForeColor = Color.White;
                    }
                }
            }
        }

        private void ApplyModernMenuStyles()
        {
            foreach (Control c in flpMenu.Controls)
            {
                if (c is Button btn)
                {
                    btn.Cursor = Cursors.Hand;
                    btn.MouseEnter += (s, e) => {
                        if (btn.BackColor != Color.White) // Don't hover active button
                            btn.BackColor = Color.FromArgb(195, 40, 55); // Lighter Red Hover
                    };
                    btn.MouseLeave += (s, e) => {
                        if (btn.BackColor != Color.White) // Don't reset active button
                            btn.BackColor = Color.FromArgb(173, 22, 37); // Default Red
                    };
                }
            }
        }

        private void SetupHeaderStyles()
        {
            // Header Bottom Border
            pnlHeader.Paint += (s, e) => {
                using (Pen p = new Pen(Color.FromArgb(230, 230, 230), 2))
                {
                    e.Graphics.DrawLine(p, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
                }
            };

            // Utility Button Hover Effects
            Button[] utilBtns = { btnExit, btnMaximize, btnMin, btnToggleSidebar };
            foreach (var btn in utilBtns)
            {
                btn.MouseEnter += (s, e) => btn.ForeColor = Color.Red;
                btn.MouseLeave += (s, e) => btn.ForeColor = Color.FromArgb(173, 22, 37);
            }

            // Search Box Styling
            if (pnlHeader.Controls.Count > 0 && pnlHeader.Controls[0] is TableLayoutPanel tlp)
            {
                Control pnlSearch = tlp.GetControlFromPosition(2, 0);
                if (pnlSearch != null)
                {
                    pnlSearch.Paint += (s, e) => {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        using (Pen p = new Pen(Color.FromArgb(220, 220, 220), 1))
                        using (GraphicsPath path = new GraphicsPath())
                        {
                            int r = 15;
                            Rectangle rect = new Rectangle(10, 10, pnlSearch.Width - 20, pnlSearch.Height - 20);
                            path.AddArc(rect.X, rect.Y, r, r, 180, 90);
                            path.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
                            path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
                            path.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
                            path.CloseFigure();
                            e.Graphics.DrawPath(p, path);
                        }
                    };
                }
            }
        }

        private void btnToggleSidebar_Click(object sender, EventArgs e)
        {
            isSidebarCollapsed = !isSidebarCollapsed;
            
            // Backup text if not already done
            if (menuOriginalText.Count == 0)
            {
                foreach (Control c in flpMenu.Controls)
                {
                    if (c is Button btn) menuOriginalText[btn] = btn.Text;
                }
            }

            if (isSidebarCollapsed)
            {
                masterCoordinator.ColumnStyles[0].Width = 65;
                profileCard.Visible = false;
                imgLogo.Visible = false;
                
                foreach (Control c in flpMenu.Controls)
                {
                    if (c is Button btn)
                    {
                        // Safely split text, if icon is present
                        string[] parts = btn.Text.Split(' ');
                        btn.Text = parts.Length > 0 ? parts[0] : btn.Text;
                        btn.TextAlign = ContentAlignment.MiddleCenter;
                        btn.Padding = new Padding(0);
                    }
                    else if (c is Label lbl)
                    {
                        lbl.Visible = false;
                    }
                }
            }
            else
            {
                masterCoordinator.ColumnStyles[0].Width = 250;
                profileCard.Visible = true;
                imgLogo.Visible = true;
                
                foreach (Control c in flpMenu.Controls)
                {
                    if (c is Button btn && menuOriginalText.ContainsKey(btn))
                    {
                        btn.Text = menuOriginalText[btn];
                        btn.TextAlign = ContentAlignment.MiddleLeft;
                        btn.Padding = new Padding(25, 0, 0, 0);
                    }
                    else if (c is Label lbl)
                    {
                        // Only show labels that should be visible based on role
                        bool isAdmin = userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);
                        if (lbl == lblAcademic || lbl == lblManagement || lbl == lblOperations)
                            lbl.Visible = !isAdmin;
                        else if (lbl == lblAdminSection)
                            lbl.Visible = isAdmin;
                    }
                }
            }
        }
    }

    // --- Helper UI Components Used in Designer ---

    public class GradientPanel : Panel
    {
        public Color ColorTop { get; set; } = Color.FromArgb(173, 22, 37); // VSIT Red
        public Color ColorBottom { get; set; } = Color.FromArgb(140, 20, 30);

        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush lgb = new LinearGradientBrush(this.ClientRectangle, this.ColorTop, this.ColorBottom, 90F);
            Graphics g = e.Graphics;
            g.FillRectangle(lgb, this.ClientRectangle);
            base.OnPaint(e);
        }
    }

    public class GlassPanel : Panel
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(20, 255, 255, 255)))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
            base.OnPaint(e);
        }
    }
}
