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
            UpdateProfile();
            StartClock();
            
            // Load Default View
            SwitchPanel("DASHBOARD");
        }

        private void SetupMenu()
        {
            // Initializing Visibility based on Role
            bool isAdmin = userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            
            // --- Common Features (Always Visible) ---
            btnDashboard.Visible = true;
            btnSettings.Visible = true;
            btnLogout.Visible = true;

            // --- ADMIN ONLY Features ---
            lblAdminSection.Visible = isAdmin;
            btnManageTeachers.Visible = isAdmin;
            btnAnalytics.Visible = isAdmin;
            btnAdminTimetable.Visible = isAdmin;
            btnFees.Visible = isAdmin;
            btnInventory.Visible = isAdmin;
            btnPlacement.Visible = isAdmin;
            btnAlumni.Visible = isAdmin;

            // --- TEACHER ONLY Features ---
            lblAcademic.Visible = !isAdmin;
            btnClasses.Visible = !isAdmin;
            btnStudents.Visible = !isAdmin;
            btnSyllabus.Visible = !isAdmin;
            btnAttendance.Visible = !isAdmin;
            
            lblManagement.Visible = !isAdmin;
            lblOperations.Visible = !isAdmin;
            btnComm.Visible = !isAdmin;           // Official Hub - Teacher Only
            btnNotices.Visible = !isAdmin;        // Post Notices - Teacher Only
            btnQuiz.Visible = !isAdmin;
            btnResources.Visible = !isAdmin;
            btnExams.Visible = !isAdmin;
            btnAssignments.Visible = !isAdmin;
            btnLeave.Visible = !isAdmin;
            btnReports.Visible = !isAdmin;

            if (isAdmin)
            {
                btnAnalytics.Text = "📊  Project Analytics";
            }
        }

        private void UpdateProfile()
        {
            lblUserName.Text = userName;
            lblRole.Text = userRole;
            lblUserBadge.Text = userName.Substring(0, 1).ToUpper();
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
                this.DialogResult = DialogResult.None;
                Application.Restart();
                return;
            }

            SwitchPanel(viewTag);
        }

        private void SwitchPanel(string view)
        {
            UserControl control = null;
            string title = view;

            switch (view)
            {
                case "DASHBOARD":
                    control = new DashboardControl(userRole, userName);
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
                case "MANAGE TEACHERS":
                    control = new ManageTeachersControl();
                    title = "Faculty Management";
                    break;
                case "ANALYTICS":
                    control = new AnalyticsControl();
                    title = "Reports & Analytics";
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
                    control = new SettingsControl();
                    title = "System Settings & Preferences";
                    break;
                case "ADMIN_TIMETABLE":
                    control = new AdminTimetableControl();
                    title = "Timetable & Teacher Assignment";
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
        public Color ColorTop { get; set; } = Color.FromArgb(28, 40, 51); // Dark Blueish
        public Color ColorBottom { get; set; } = Color.FromArgb(20, 26, 31);

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
