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
            this.btnReports = new System.Windows.Forms.Button();
            this.btnResources = new System.Windows.Forms.Button();
            this.btnExams = new System.Windows.Forms.Button();
            this.btnAssignments = new System.Windows.Forms.Button();
            this.btnLeave = new System.Windows.Forms.Button();

            this.lblAdminSection = new System.Windows.Forms.Label();
            this.btnManageTeachers = new System.Windows.Forms.Button();
            this.btnAnalytics = new System.Windows.Forms.Button();
            this.btnFees = new System.Windows.Forms.Button();
            this.btnInventory = new System.Windows.Forms.Button();
            this.btnPlacement = new System.Windows.Forms.Button();
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
            this.profileCard.Size = new System.Drawing.Size(220, 80); // Increased height slightly
            
            System.Windows.Forms.TableLayoutPanel tlpProfile = new System.Windows.Forms.TableLayoutPanel() {
                Dock = System.Windows.Forms.DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = System.Drawing.Color.FromArgb(40, 45, 50) // Solid dark match
            };
            tlpProfile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55f));
            tlpProfile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45f));
            this.profileCard.Controls.Add(tlpProfile);

            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblUserName.ForeColor = System.Drawing.Color.White;
            this.lblUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUserName.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblUserName.Text = "User Name";

            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblRole.ForeColor = System.Drawing.Color.Silver;
            this.lblRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRole.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblRole.Text = "Academic Faculty";
            
            tlpProfile.Controls.Add(this.lblUserName, 0, 0);
            tlpProfile.Controls.Add(this.lblRole, 0, 1);

            // flpMenu
            this.flpMenu.BackColor = System.Drawing.Color.FromArgb(25, 30, 35);
            this.flpMenu.Location = new System.Drawing.Point(0, 160);
            this.flpMenu.Size = new System.Drawing.Size(250, 740);
            this.flpMenu.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpMenu.AutoScroll = true;
            this.flpMenu.WrapContents = false;
            this.flpMenu.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);

            // Setup Labels & Buttons
            SetupMenuLabel(this.lblAcademic, "ACADEMICS");
            SetupMenuButton(this.btnDashboard, "📊  Dashboard", "DASHBOARD");
            SetupMenuButton(this.btnClasses, "🎓  Teaching Portfolio", "CLASSES");
            SetupMenuButton(this.btnStudents, "👨‍🎓  Student Hub", "STUDENTS");
            SetupMenuButton(this.btnSyllabus, "📖  Curriculum Tracker", "SYLLABUS");
            SetupMenuButton(this.btnAttendance, "📅  Attendance Tracker", "ATTENDANCE");

            SetupMenuLabel(this.lblManagement, "MANAGEMENT");
            SetupMenuButton(this.btnNotices, "📌  Post Notices", "NOTICES");
            SetupMenuButton(this.btnComm, "📢  Official Hub", "MESSAGES");
            SetupMenuButton(this.btnQuiz, "⚡  Quiz Creator", "QUIZ");
            SetupMenuButton(this.btnReports, "📂  Reports", "REPORTS");
            SetupMenuButton(this.btnResources, "📚  Resources", "RESOURCES");
            SetupMenuButton(this.btnExams, "📜  Exam Portfolio", "TESTS");
            SetupMenuButton(this.btnAssignments, "📁  Assignments", "ASSIGNMENTS");
            SetupMenuButton(this.btnLeave, "🏖️  Faculty Leave", "LEAVE");

            this.lblOperations = new System.Windows.Forms.Label(); // Init New Label
            SetupMenuLabel(this.lblOperations, "OPERATIONS");      // Setup New Label

            SetupMenuLabel(this.lblAdminSection, "ADMINISTRATION");
            SetupMenuButton(this.btnManageTeachers, "👥  Faculty Management", "MANAGE TEACHERS");
            this.btnAdminTimetable = new System.Windows.Forms.Button(); // Instantiate
            SetupMenuButton(this.btnAdminTimetable, "🗓️  Academic Scheduling", "ADMIN_TIMETABLE");
            SetupMenuButton(this.btnAnalytics, "📈  Analytics", "ANALYTICS");
            SetupMenuButton(this.btnFees, "💰  Fee Management", "FEES");
            SetupMenuButton(this.btnInventory, "📦  Campus Management", "CAMPUS");
            SetupMenuButton(this.btnPlacement, "📍  Placement Cell", "PLACEMENT");
            SetupMenuButton(this.btnAlumni, "🎓  Alumni Network", "ALUMNI");

            SetupMenuButton(this.btnSettings, "⚙️  Settings", "SETTINGS");
            SetupMenuButton(this.btnLogout, "🚪  Logout", "LOGOUT");
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(255, 100, 100);

            // Add to Menu
            this.flpMenu.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblAcademic, this.btnDashboard, this.btnClasses, this.btnStudents, this.btnSyllabus, this.btnAttendance,
                this.lblManagement, this.btnNotices, this.btnComm, this.btnQuiz, this.btnExams,
                this.lblOperations, this.btnAssignments, this.btnResources, this.btnReports, this.btnLeave,
                this.lblAdminSection, this.btnManageTeachers, this.btnAdminTimetable, this.btnAnalytics, this.btnFees, this.btnInventory, this.btnPlacement, this.btnAlumni,
                this.btnSettings, this.btnLogout
            });

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(32, 33, 36);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            
            System.Windows.Forms.TableLayoutPanel tlpHeader = new System.Windows.Forms.TableLayoutPanel() {
                Dock = System.Windows.Forms.DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = System.Drawing.Color.FromArgb(32, 33, 36),
                Padding = new System.Windows.Forms.Padding(0, 0, 15, 0)
            };
            tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50f));  // Toggle
            tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35f)); // Title
            tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f)); // Search Area
            tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45f)); // Utility Area
            this.pnlHeader.Controls.Add(tlpHeader);

            this.btnToggleSidebar = new System.Windows.Forms.Button() {
                Text = "☰",
                Size = new System.Drawing.Size(50, 75),
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                Margin = new System.Windows.Forms.Padding(0)
            };
            this.btnToggleSidebar.FlatAppearance.BorderSize = 0;
            this.btnToggleSidebar.Click += new System.EventHandler(this.btnToggleSidebar_Click);
            this.lblCurrentView.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblCurrentView.ForeColor = System.Drawing.Color.White;
            this.lblCurrentView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrentView.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCurrentView.Text = "Dashboard";

            // Middle: Search Panel (Centered and constrained)
            System.Windows.Forms.Panel pnlSearchGroup = new System.Windows.Forms.Panel() { 
                Dock = System.Windows.Forms.DockStyle.Fill, 
                Padding = new System.Windows.Forms.Padding(20, 22, 20, 22) 
            };
            System.Windows.Forms.TextBox txtSearchHeader = new System.Windows.Forms.TextBox() { 
                Dock = System.Windows.Forms.DockStyle.Fill, 
                BackColor = System.Drawing.Color.FromArgb(45, 45, 48), 
                ForeColor = System.Drawing.Color.Gray, 
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Font = new System.Drawing.Font("Segoe UI", 10),
                Text = " 🔍 Global Search..."
            };
            pnlSearchGroup.Controls.Add(txtSearchHeader);

            // Right: Utility Flow Area (Prevents clumping/overlapping)
            System.Windows.Forms.FlowLayoutPanel flpRight = new System.Windows.Forms.FlowLayoutPanel() {
                Dock = System.Windows.Forms.DockStyle.Fill,
                FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft,
                WrapContents = false,
                BackColor = System.Drawing.Color.FromArgb(32, 33, 36)
            };

            // Exit Button
            this.btnExit.Size = new System.Drawing.Size(45, 75);
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnExit.Text = "✕";
            this.btnExit.Margin = new System.Windows.Forms.Padding(0);
            this.btnExit.Click += (s, e) => System.Windows.Forms.Application.Exit();

            // Minimize Button
            this.btnMin = new System.Windows.Forms.Button();
            this.btnMin.Size = new System.Drawing.Size(45, 75);
            this.btnMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMin.FlatAppearance.BorderSize = 0;
            this.btnMin.ForeColor = System.Drawing.Color.White;
            this.btnMin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnMin.Text = "—";
            this.btnMin.Margin = new System.Windows.Forms.Padding(0);
            this.btnMin.Click += (s, e) => this.WindowState = System.Windows.Forms.FormWindowState.Minimized;

            // Maximize/Restore Button
            this.btnMaximize = new System.Windows.Forms.Button();
            this.btnMaximize.Size = new System.Drawing.Size(45, 75);
            this.btnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximize.FlatAppearance.BorderSize = 0;
            this.btnMaximize.ForeColor = System.Drawing.Color.White;
            this.btnMaximize.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnMaximize.Text = "□";
            this.btnMaximize.Margin = new System.Windows.Forms.Padding(0);
            this.btnMaximize.Click += (s, e) => {
                if (this.WindowState == System.Windows.Forms.FormWindowState.Maximized)
                {
                    this.WindowState = System.Windows.Forms.FormWindowState.Normal;
                    this.btnMaximize.Text = "□";
                }
                else
                {
                    this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                    this.btnMaximize.Text = "❐";
                }
            };

            // Notification Button
            this.btnNotify = new System.Windows.Forms.Button() { 
                Text = "🔔", Size = new System.Drawing.Size(45, 75), 
                FlatStyle = System.Windows.Forms.FlatStyle.Flat, 
                ForeColor = System.Drawing.Color.White, 
                Font = new System.Drawing.Font("Segoe UI", 12),
                Margin = new System.Windows.Forms.Padding(0)
            };
            this.btnNotify.FlatAppearance.BorderSize = 0;

            // Refresh Button
            this.btnRefresh = new System.Windows.Forms.Button() { 
                Text = "🔄", Size = new System.Drawing.Size(45, 75), 
                FlatStyle = System.Windows.Forms.FlatStyle.Flat, 
                ForeColor = System.Drawing.Color.White, 
                Font = new System.Drawing.Font("Segoe UI", 12),
                Margin = new System.Windows.Forms.Padding(0)
            };
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.Click += (s, e) => this.SwitchPanel("DASHBOARD");

            // Clock
            this.lblHeaderClock = new System.Windows.Forms.Label() { 
                AutoSize = true,
                Height = 75,
                ForeColor = System.Drawing.Color.Silver, 
                Font = new System.Drawing.Font("Segoe UI Semibold", 10), 
                TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                Text = System.DateTime.Now.ToString("ddd, MMM dd | HH:mm"),
                Padding = new System.Windows.Forms.Padding(10, 25, 10, 0)
            };

            // Add icons in order from RIGHT to LEFT
            flpRight.Controls.Add(this.btnExit);
            flpRight.Controls.Add(this.btnMaximize);
            flpRight.Controls.Add(this.btnMin);
            flpRight.Controls.Add(this.btnNotify);
            flpRight.Controls.Add(this.btnRefresh);
            flpRight.Controls.Add(this.lblHeaderClock);

            tlpHeader.Controls.Add(this.btnToggleSidebar, 0, 0);
            tlpHeader.Controls.Add(this.lblCurrentView, 1, 0);
            tlpHeader.Controls.Add(pnlSearchGroup, 2, 0);
            tlpHeader.Controls.Add(flpRight, 3, 0);

            // pnlContent
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;

            // MainForm Reset
            this.ClientSize = new System.Drawing.Size(1280, 900);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            // MASTER COORDINATOR (TableLayoutPanel)
            this.masterCoordinator = new System.Windows.Forms.TableLayoutPanel() { 
                Dock = System.Windows.Forms.DockStyle.Fill, 
                ColumnCount = 2, 
                RowCount = 2,
                BackColor = System.Drawing.Color.FromArgb(18, 18, 18)
            };
            masterCoordinator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250)); // Col 0: Sidebar
            masterCoordinator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100));  // Col 1: Main Area
            masterCoordinator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75));    // Row 0: Header (Increased to 75 to prevent overlap)
            masterCoordinator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100));   // Row 1: Content Area
            this.Controls.Add(masterCoordinator);

            // pnlSidebar Coordinator (Stacking elements without overlap)
            System.Windows.Forms.TableLayoutPanel sidebarStack = new System.Windows.Forms.TableLayoutPanel() {
                Dock = System.Windows.Forms.DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = System.Drawing.Color.FromArgb(28, 40, 51)
            };
            sidebarStack.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80f));  // Logo Row
            sidebarStack.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100f)); // Profile Row
            sidebarStack.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));   // Menu Row
            this.pnlSidebar.Controls.Add(sidebarStack);

            this.imgLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.profileCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.profileCard.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.flpMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMenu.Margin = new System.Windows.Forms.Padding(0);

            sidebarStack.Controls.Add(this.imgLogo, 0, 0);
            sidebarStack.Controls.Add(this.profileCard, 0, 1);
            sidebarStack.Controls.Add(this.flpMenu, 0, 2);

            this.masterCoordinator.Controls.Add(pnlSidebar, 0, 0);
            this.masterCoordinator.SetRowSpan(pnlSidebar, 2); // Sidebar takes full height

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(32, 33, 36);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.Height = 60;
            this.pnlHeader.Controls.Add(this.lblCurrentView);
            this.pnlHeader.Controls.Add(this.lblUserBadge);
            this.pnlHeader.Controls.Add(this.btnExit);
            this.masterCoordinator.Controls.Add(pnlHeader, 1, 0);

            // pnlContent
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoScroll = true; // Added Navigation Support (Scrollbars)
            this.masterCoordinator.Controls.Add(pnlContent, 1, 1);

            // MainForm Reset
            this.ClientSize = new System.Drawing.Size(1280, 900);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

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
        private System.Windows.Forms.Label lblOperations; // New Control Declaration
        private System.Windows.Forms.Button btnNotices;
        private System.Windows.Forms.Button btnComm;
        private System.Windows.Forms.Button btnQuiz;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnResources;
        private System.Windows.Forms.Button btnExams;
        private System.Windows.Forms.Button btnAssignments;
        private System.Windows.Forms.Button btnLeave;
        private System.Windows.Forms.Label lblAdminSection;
        private System.Windows.Forms.Button btnManageTeachers;
        private System.Windows.Forms.Button btnAdminTimetable;
        private System.Windows.Forms.Button btnAnalytics;
        private System.Windows.Forms.Button btnFees;
        private System.Windows.Forms.Button btnInventory;
        private System.Windows.Forms.Button btnPlacement;
        private System.Windows.Forms.Button btnAlumni;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblCurrentView;
        private System.Windows.Forms.Label lblUserBadge;
        internal System.Windows.Forms.Label lblHeaderClock;
        private System.Windows.Forms.Button btnNotify;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnMin;
        private System.Windows.Forms.Button btnMaximize;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnToggleSidebar;
        private System.Windows.Forms.TableLayoutPanel masterCoordinator;
        private System.Windows.Forms.Panel pnlContent;
    }
}
