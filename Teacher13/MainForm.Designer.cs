namespace TeacherDashboard
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlSidebar = new TeacherDashboard.GradientPanel();
            this.flpMenu = new System.Windows.Forms.FlowLayoutPanel();
            this.imgLogo = new System.Windows.Forms.PictureBox();
            this.profileCard = new TeacherDashboard.GlassPanel();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblRole = new System.Windows.Forms.Label();
            
            this.lblAcademic = new System.Windows.Forms.Label();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnClasses = new System.Windows.Forms.Button();
            this.btnStudents = new System.Windows.Forms.Button();
            this.btnSyllabus = new System.Windows.Forms.Button();
            this.btnAttendance = new System.Windows.Forms.Button();

            this.lblManagement = new System.Windows.Forms.Label();
            this.btnNotices = new System.Windows.Forms.Button();
            this.btnComm = new System.Windows.Forms.Button();
            this.btnQuiz = new System.Windows.Forms.Button();
            this.btnGrades = new System.Windows.Forms.Button();
            this.btnEvents = new System.Windows.Forms.Button();
            this.btnProject = new System.Windows.Forms.Button();
            this.btnCalendar = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnResources = new System.Windows.Forms.Button();
            this.btnExams = new System.Windows.Forms.Button();
            this.btnAssignments = new System.Windows.Forms.Button();
            this.btnTimeTable = new System.Windows.Forms.Button();
            this.btnLeave = new System.Windows.Forms.Button();

            this.lblAdminSection = new System.Windows.Forms.Label();
            this.btnManageTeachers = new System.Windows.Forms.Button();
            this.btnAnalytics = new System.Windows.Forms.Button();
            this.btnFees = new System.Windows.Forms.Button();
            this.btnInventory = new System.Windows.Forms.Button();
            this.btnPlacement = new System.Windows.Forms.Button();
            this.btnLabs = new System.Windows.Forms.Button();
            this.btnAlumni = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();

            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblCurrentView = new System.Windows.Forms.Label();
            this.lblUserBadge = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();

            this.pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).BeginInit();
            this.profileCard.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();

            // pnlSidebar
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(250, 900);
            this.pnlSidebar.TabIndex = 0;
            this.pnlSidebar.Controls.Add(this.flpMenu);
            this.pnlSidebar.Controls.Add(this.profileCard);
            this.pnlSidebar.Controls.Add(this.imgLogo);

            // imgLogo
            this.imgLogo.BackColor = System.Drawing.Color.Transparent;
            this.imgLogo.Location = new System.Drawing.Point(25, 15);
            this.imgLogo.Size = new System.Drawing.Size(200, 50);
            this.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgLogo.TabIndex = 0;

            // profileCard
            this.profileCard.BackColor = System.Drawing.Color.FromArgb(40, 255, 255, 255);
            this.profileCard.Location = new System.Drawing.Point(15, 80);
            this.profileCard.Size = new System.Drawing.Size(220, 70);
            this.profileCard.Controls.Add(this.lblUserName);
            this.profileCard.Controls.Add(this.lblRole);

            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblUserName.ForeColor = System.Drawing.Color.White;
            this.lblUserName.Location = new System.Drawing.Point(15, 10);
            this.lblUserName.Size = new System.Drawing.Size(190, 20);
            this.lblUserName.Text = "User Name";

            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblRole.ForeColor = System.Drawing.Color.Silver;
            this.lblRole.Location = new System.Drawing.Point(15, 45); // Increased spacing to 35px gap
            this.lblRole.Size = new System.Drawing.Size(190, 15);
            this.lblRole.Text = "Academic Faculty";

            // flpMenu
            this.flpMenu.BackColor = System.Drawing.Color.Transparent;
            this.flpMenu.Location = new System.Drawing.Point(0, 160);
            this.flpMenu.Size = new System.Drawing.Size(250, 740);
            this.flpMenu.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpMenu.AutoScroll = true;
            this.flpMenu.WrapContents = false;
            this.flpMenu.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);

            // Setup Labels & Buttons
            SetupMenuLabel(this.lblAcademic, "ACADEMICS");
            SetupMenuButton(this.btnDashboard, "📊  Dashboard", "DASHBOARD");
            SetupMenuButton(this.btnClasses, "🏫  Classes", "CLASSES");
            SetupMenuButton(this.btnStudents, "👨‍🎓  Students", "STUDENTS");
            SetupMenuButton(this.btnSyllabus, "📖  Syllabus", "SYLLABUS");
            SetupMenuButton(this.btnAttendance, "📅  Attendance", "ATTENDANCE");

            SetupMenuLabel(this.lblManagement, "MANAGEMENT");
            SetupMenuButton(this.btnNotices, "🔔  Notices", "NOTICES");
            SetupMenuButton(this.btnComm, "✉️  Messages", "MESSAGES");
            SetupMenuButton(this.btnQuiz, "⚡  Quiz Creator", "QUIZ");
            SetupMenuButton(this.btnLabs, "🧪  Lab Management", "LABS");
            SetupMenuButton(this.btnGrades, "⭐  Grades", "GRADES");
            SetupMenuButton(this.btnEvents, "🎈  Events", "EVENTS");
            SetupMenuButton(this.btnProject, "🛠️  Mentorship", "PROJECT");
            SetupMenuButton(this.btnCalendar, "🗓️  Calendar", "CALENDAR");
            SetupMenuButton(this.btnReports, "📂  Reports", "REPORTS");
            SetupMenuButton(this.btnResources, "📚  Resources", "RESOURCES");
            SetupMenuButton(this.btnExams, "📝  Exams", "EXAMS");
            SetupMenuButton(this.btnAssignments, "📁  Assignments", "ASSIGNMENTS");
            SetupMenuButton(this.btnTimeTable, "🕒  Timetable", "TIMETABLE");
            SetupMenuButton(this.btnLeave, "🏖️  Faculty Leave", "LEAVE");

            SetupMenuLabel(this.lblAdminSection, "ADMINISTRATION");
            SetupMenuButton(this.btnManageTeachers, "👥  Manage Users", "MANAGE TEACHERS");
            SetupMenuButton(this.btnAnalytics, "📈  Analytics", "ANALYTICS");
            SetupMenuButton(this.btnFees, "💰  Fee Management", "FEES");
            SetupMenuButton(this.btnInventory, "📦  Campus Inventory", "INVENTORY");
            SetupMenuButton(this.btnPlacement, "📍  Placement Cell", "PLACEMENT");
            SetupMenuButton(this.btnAlumni, "🎓  Alumni Network", "ALUMNI");

            SetupMenuButton(this.btnSettings, "⚙️  Settings", "SETTINGS");
            SetupMenuButton(this.btnLogout, "🚪  Logout", "LOGOUT");
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(255, 100, 100);

            // Add to Menu
            this.flpMenu.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblAcademic, this.btnDashboard, this.btnClasses, this.btnStudents, this.btnSyllabus, this.btnAttendance,
                this.lblManagement, this.btnNotices, this.btnComm, this.btnQuiz, this.btnLabs, this.btnGrades, this.btnEvents, this.btnProject, this.btnCalendar, this.btnReports, this.btnResources, this.btnExams, this.btnAssignments, this.btnTimeTable, this.btnLeave,
                this.lblAdminSection, this.btnManageTeachers, this.btnAnalytics, this.btnFees, this.btnInventory, this.btnPlacement, this.btnAlumni,
                this.btnSettings, this.btnLogout
            });

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(32, 33, 36);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 60;
            this.pnlHeader.Controls.Add(this.lblCurrentView);
            this.pnlHeader.Controls.Add(this.lblUserBadge);
            // Redundant btnExit addition removed to prevent overlap in features
            // this.pnlHeader.Controls.Add(this.btnExit);

            this.lblCurrentView.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblCurrentView.ForeColor = System.Drawing.Color.White;
            this.lblCurrentView.Location = new System.Drawing.Point(20, 18);
            this.lblCurrentView.Size = new System.Drawing.Size(300, 25);
            this.lblCurrentView.Text = "Dashboard";
 
            this.lblUserBadge.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblUserBadge.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblUserBadge.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.lblUserBadge.ForeColor = System.Drawing.Color.White;
            this.lblUserBadge.Location = new System.Drawing.Point(820, 15);
            this.lblUserBadge.Size = new System.Drawing.Size(150, 30);
            this.lblUserBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblUserBadge.Text = "ROLE";

            this.btnExit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(985, 12);
            this.btnExit.Size = new System.Drawing.Size(35, 35);
            this.btnExit.Text = "X";
            this.btnExit.Click += (s, e) => System.Windows.Forms.Application.Exit();

            // pnlContent
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;

            // MainForm Reset
            this.ClientSize = new System.Drawing.Size(1280, 900);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            this.Controls.Clear();
            this.Controls.Add(this.pnlSidebar); // Index 0: Docks Left (Full Height)
            this.Controls.Add(this.pnlHeader);  // Index 1: Docks Top (Remaining Width)
            this.Controls.Add(this.pnlContent); // Index 2: Docks Fill (Remaining Space)

            // Correct Docking Priority: Sidebar (Back/Outer) -> Header (Middle) -> Content (Front/Inner)
            this.Controls.SetChildIndex(this.pnlSidebar, 2); // HIGHEST INDEX = DOCKS FIRST (Full Height)
            this.Controls.SetChildIndex(this.pnlHeader, 1);  // SECOND INDEX = DOCKS SECOND
            this.Controls.SetChildIndex(this.pnlContent, 0); // LOWEST INDEX = DOCKS LAST (Fill Remaining)

            this.pnlSidebar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).EndInit();
            this.profileCard.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void SetupMenuButton(System.Windows.Forms.Button btn, string text, string tag)
        {
            btn.Size = new System.Drawing.Size(250, 42);
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            btn.ForeColor = System.Drawing.Color.White;
            btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btn.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            btn.Text = text;
            btn.Tag = tag;
            btn.Click += new System.EventHandler(this.SidebarButton_Click);
        }

        private void SetupMenuLabel(System.Windows.Forms.Label lbl, string text)
        {
            lbl.Size = new System.Drawing.Size(250, 35);
            lbl.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lbl.ForeColor = System.Drawing.Color.FromArgb(180, 255, 255, 255);
            lbl.Padding = new System.Windows.Forms.Padding(20, 15, 0, 0);
            lbl.Text = text;
        }

        private GradientPanel pnlSidebar;
        private System.Windows.Forms.FlowLayoutPanel flpMenu;
        private System.Windows.Forms.PictureBox imgLogo;
        private GlassPanel profileCard;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Label lblAcademic;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnClasses;
        private System.Windows.Forms.Button btnStudents;
        private System.Windows.Forms.Button btnSyllabus;
        private System.Windows.Forms.Button btnAttendance;
        private System.Windows.Forms.Label lblManagement;
        private System.Windows.Forms.Button btnNotices;
        private System.Windows.Forms.Button btnComm;
        private System.Windows.Forms.Button btnQuiz;
        private System.Windows.Forms.Button btnGrades;
        private System.Windows.Forms.Button btnEvents;
        private System.Windows.Forms.Button btnProject;
        private System.Windows.Forms.Button btnCalendar;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnResources;
        private System.Windows.Forms.Button btnExams;
        private System.Windows.Forms.Button btnAssignments;
        private System.Windows.Forms.Button btnTimeTable;
        private System.Windows.Forms.Button btnLeave;
        private System.Windows.Forms.Label lblAdminSection;
        private System.Windows.Forms.Button btnManageTeachers;
        private System.Windows.Forms.Button btnAnalytics;
        private System.Windows.Forms.Button btnFees;
        private System.Windows.Forms.Button btnInventory;
        private System.Windows.Forms.Button btnPlacement;
        private System.Windows.Forms.Button btnLabs;
        private System.Windows.Forms.Button btnAlumni;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblCurrentView;
        private System.Windows.Forms.Label lblUserBadge;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel pnlContent;
    }
}
