using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TeacherDashboard.Controls;

namespace TeacherDashboard
{
    public partial class MainForm : Form
    {
        public string Role { get; private set; }
        public string UserName { get; private set; }

        public MainForm(string role, string userName)
        {
            InitializeComponent();
            this.Role = role;
            this.UserName = userName;
            this.lblUserName.Text = userName;
            this.lblRole.Text = role;
            
            SetupMenu();
            SwitchPanel("DASHBOARD");
        }

        private void SetupMenu()
        {
            bool isAdmin = Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            
            // --- ADMINISTRATION SECTION (Admin Only) ---
            lblAdminSection.Visible = isAdmin;
            btnManageTeachers.Visible = isAdmin;
            btnAnalytics.Visible = isAdmin;
            btnFees.Visible = isAdmin;
            btnInventory.Visible = isAdmin;
            btnPlacement.Visible = isAdmin;
            btnAlumni.Visible = isAdmin;

            // --- ACADEMIC SECTION (Teacher/Faculty Only) ---
            lblAcademic.Visible = !isAdmin;
            btnClasses.Visible = !isAdmin;
            btnStudents.Visible = !isAdmin;
            btnSyllabus.Visible = !isAdmin;
            btnAttendance.Visible = !isAdmin;
            
            // --- MANAGEMENT (Mixed/Role-Dependent) ---
            lblManagement.Visible = true; 
            btnNotices.Visible = true;
            btnComm.Visible = true;
            btnEvents.Visible = true;
            btnCalendar.Visible = true;
            btnReports.Visible = true;

            // Teacher Specific Management Items (Hide from Admin)
            btnQuiz.Visible = !isAdmin;
            btnLabs.Visible = !isAdmin;
            btnGrades.Visible = !isAdmin;
            btnProject.Visible = !isAdmin;
            btnResources.Visible = !isAdmin;
            btnExams.Visible = !isAdmin;
            btnAssignments.Visible = !isAdmin;
            btnTimeTable.Visible = !isAdmin;
            btnLeave.Visible = !isAdmin;

            // Header Dark Theme Apply
            pnlHeader.BackColor = Color.FromArgb(32, 33, 36);
            lblCurrentView.ForeColor = Color.White;
            btnExit.ForeColor = Color.White;

            // Profile Card Role Badge
            if (isAdmin)
            {
                lblRole.Text = "👑 System Administrator";
                lblRole.ForeColor = Color.FromArgb(241, 196, 15); // Golden
                lblUserBadge.Text = "ADMIN PRIVILEGES";
                lblUserBadge.BackColor = Color.FromArgb(173, 22, 37);
            }
            else
            {
                lblRole.Text = "👨‍🏫 Academic Faculty";
                lblRole.ForeColor = Color.FromArgb(180, 180, 180);
                lblUserBadge.Text = "FACULTY MEMBER";
                lblUserBadge.BackColor = Color.FromArgb(46, 204, 113);
            }

            // Force layout recalculation for the FlowLayoutPanel
            flpMenu.ResumeLayout(true);
            flpMenu.PerformLayout();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            // Assuming LoginForm will be shown by Program.cs or handled elsewhere
        }

        private void SidebarButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string tag = btn.Tag?.ToString();

            if (tag == "LOGOUT")
            {
                // Restart the application to show Login Form again
                Application.Restart(); 
                return;
            }

            lblCurrentView.Text = btn.Text;
            SwitchPanel(tag);
        }

        private void SwitchPanel(string tag)
        {
            pnlContent.Controls.Clear();
            UserControl control = null;

            switch (tag)
            {
                case "DASHBOARD":
                    control = new DashboardControl(this.Role, this.UserName);
                    break;
                case "MANAGE TEACHERS":
                    control = new ManageTeachersControl();
                    break;
                case "CLASSES":
                    control = new ClassesControl();
                    break;
                case "STUDENTS":
                    control = new StudentsControl();
                    break;
                case "ATTENDANCE":
                    control = new AttendanceControl();
                    break;
                case "MESSAGES":
                    control = new CommunicationControl();
                    break;
                case "SETTINGS":
                    control = new DashboardControl(this.Role, this.UserName);
                    break;
                case "RESOURCES":
                    control = new ResourcesControl();
                    break;
                case "EXAMS":
                    control = new ExamsControl();
                    break;
                case "ASSIGNMENTS":
                    control = new AssignmentsControl();
                    break;
                case "TIMETABLE":
                    control = new TimeTableControl();
                    break;
                case "ANALYTICS":
                    control = new AnalyticsControl();
                    break;
                case "NOTICES":
                    control = new NoticesControl();
                    break;
                case "GRADES":
                    control = new GradesControl();
                    break;
                case "EVENTS":
                    control = new EventsControl();
                    break;
                case "REPORTS":
                    control = new ReportsControl();
                    break;
                case "CALENDAR":
                    control = new CalendarControl();
                    break;
                case "SYLLABUS":
                    control = new SyllabusControl();
                    break;
                case "QUIZ":
                    control = new QuizCreatorControl();
                    break;
                case "LEAVE":
                    control = new LeaveManagementControl();
                    break;
                case "PROJECT":
                    control = new ProjectMentorshipControl();
                    break;
                case "FEES":
                    control = new FeeManagementControl();
                    break;
                case "INVENTORY":
                    control = new InventoryControl();
                    break;
                case "PLACEMENT":
                    control = new PlacementControl();
                    break;
                case "LABS":
                    control = new LabManagementControl();
                    break;
                case "ALUMNI":
                    control = new AlumniControl();
                    break;
                default:
                    control = new DashboardControl(this.Role, this.UserName);
                    break;
            }

            if (control != null)
            {
                control.Dock = DockStyle.Fill;
                pnlContent.Controls.Add(control);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // Custom painting if needed
        }
    }

    // Custom Panel for Glassmorphism
    public class GlassPanel : Panel
    {
        public GlassPanel()
        {
            this.BackColor = Color.FromArgb(40, 255, 255, 255);
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            base.OnPaint(e);
            using (Pen pen = new Pen(Color.FromArgb(80, 255, 255, 255), 1))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            }
        }
    }

    public class GradientPanel : Panel
    {
        public Color ColorTop { get; set; } = Color.FromArgb(173, 22, 37);
        public Color ColorBottom { get; set; } = Color.FromArgb(60, 10, 20);

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.ClientRectangle.Width <= 0 || this.ClientRectangle.Height <= 0) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (LinearGradientBrush lgb = new LinearGradientBrush(this.ClientRectangle, ColorTop, ColorBottom, 90F))
            {
                e.Graphics.FillRectangle(lgb, this.ClientRectangle);
            }
            base.OnPaint(e);
        }
    }
}
