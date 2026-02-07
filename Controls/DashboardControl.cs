using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TeacherDashboard.Controls
{
    public partial class DashboardControl : UserControl
    {
        private string role;
        private string name;
        public event Action<string> RequestViewChange;
        
        // Theme Constants
        private Color clrBackground = Color.FromArgb(245, 245, 245);
        private Color clrCard = Color.White;
        private Color clrText = Color.FromArgb(40, 40, 40);
        private Color clrGray = Color.FromArgb(100, 100, 100);
        private Color clrBorder = Color.FromArgb(220, 220, 220);
        private Color primaryRed = Color.FromArgb(173, 22, 37);
        private Color greenColor = Color.FromArgb(46, 204, 113);
        private Color blueColor = Color.FromArgb(52, 152, 219);
        private Color orangeColor = Color.FromArgb(230, 126, 34);

        public DashboardControl(string userRole, string userName)
        {
            InitializeComponent();
            this.role = userRole;
            this.name = userName;
            this.Dock = DockStyle.Fill;
            this.BackColor = clrBackground;
            SetupModernDashboard();
        }

        private void SetupModernDashboard()
        {
            this.Controls.Clear();
            // Force Teacher portal labels (Admin features removed)
            bool isAdmin = false; 
            string displayName = name;
            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase)) {
                displayName = "Faculty Member"; 
            }

            // --- ROOT LAYOUT ---
            TableLayoutPanel rootLayout = new TableLayoutPanel();
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F)); // Header (slightly taller for tag)
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Body
            this.Controls.Add(rootLayout);

            // 1. HEADER with Gradient Background
            Panel pnlHeader = new Panel() { Dock = DockStyle.Fill };
            
            // Gradient background
            pnlHeader.Paint += (s, e) =>
            {
                using (System.Drawing.Drawing2D.LinearGradientBrush brush = 
                    new System.Drawing.Drawing2D.LinearGradientBrush(
                        pnlHeader.ClientRectangle,
                        Color.White,
                        Color.FromArgb(255, 250, 250), // Very subtle red tint
                        System.Drawing.Drawing2D.LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, pnlHeader.ClientRectangle);
                }
                
                // Bottom border
                using (Pen borderPen = new Pen(Color.FromArgb(240, 240, 240), 2))
                {
                    e.Graphics.DrawLine(borderPen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
                }
            };
            
            Label lblGreeting = new Label() { 
                Text = $"Good Day, {displayName}", 
                Font = new Font("Segoe UI", 26, FontStyle.Bold), 
                ForeColor = primaryRed, 
                Location = new Point(30, 20), 
                AutoSize = true 
            };
            
            Label lblTag = new Label() {
                Text = isAdmin ? "Administrator Control Center | Academic Year 2025-26" : "Faculty Teaching Portal | Digital Campus",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.FromArgb(120, 120, 120),
                Location = new Point(34, 65),
                AutoSize = true
            };
            
            pnlHeader.Controls.AddRange(new Control[] { lblGreeting, lblTag });

            // 1.1 Enhanced Status Widget with gradient
            Panel pnlStatus = new Panel() { Size = new Size(250, 70), Location = new Point(820, 20) };
            pnlStatus.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                
                // Gradient background
                using (System.Drawing.Drawing2D.LinearGradientBrush brush = 
                    new System.Drawing.Drawing2D.LinearGradientBrush(
                        pnlStatus.ClientRectangle,
                        Color.FromArgb(240, 255, 240), // Light green tint
                        Color.White,
                        45f))
                {
                    e.Graphics.FillRectangle(brush, pnlStatus.ClientRectangle);
                }
                
                // Rounded border
                using (System.Drawing.Drawing2D.GraphicsPath path = CreateRoundedRect(0, 0, pnlStatus.Width - 1, pnlStatus.Height - 1, 8))
                {
                    using (Pen borderPen = new Pen(Color.FromArgb(46, 204, 113), 2))
                    {
                        e.Graphics.DrawPath(borderPen, path);
                    }
                }
            };
            
            Label lblStatTitle = new Label() { 
                Text = "✓ SYSTEM HEALTH: 100%", 
                Font = new Font("Segoe UI", 9, FontStyle.Bold), 
                ForeColor = Color.FromArgb(39, 174, 96), 
                Location = new Point(10, 15), 
                AutoSize = true 
            };
            Label lblStatBody = new Label() { 
                Text = "Campus Mode: ACTIVE | Secured", 
                Font = new Font("Segoe UI", 8.5F), 
                ForeColor = Color.Gray, 
                Location = new Point(10, 38), 
                AutoSize = true 
            };
            pnlStatus.Controls.AddRange(new Control[] { lblStatTitle, lblStatBody });
            pnlHeader.Controls.Add(pnlStatus);

            rootLayout.Controls.Add(pnlHeader, 0, 0);

            // 2. SCROLLABLE BODY with subtle pattern
            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true };
            pnlScroll.Padding = new Padding(30);
            
            // Add subtle dotted pattern background
            pnlScroll.Paint += (s, e) =>
            {
                // Base gradient
                using (System.Drawing.Drawing2D.LinearGradientBrush brush = 
                    new System.Drawing.Drawing2D.LinearGradientBrush(
                        pnlScroll.ClientRectangle,
                        Color.FromArgb(245, 245, 245),
                        Color.FromArgb(250, 250, 250),
                        45f))
                {
                    e.Graphics.FillRectangle(brush, pnlScroll.ClientRectangle);
                }
                
                // Subtle dot pattern
                using (SolidBrush dotBrush = new SolidBrush(Color.FromArgb(8, 173, 22, 37)))
                {
                    for (int x = 0; x < pnlScroll.Width; x += 30)
                    {
                        for (int y = 0; y < pnlScroll.Height; y += 30)
                        {
                            e.Graphics.FillEllipse(dotBrush, x, y, 2, 2);
                        }
                    }
                }
            };
            
            rootLayout.Controls.Add(pnlScroll, 0, 1);

            FlowLayoutPanel flpMain = new FlowLayoutPanel() { 
                Dock = DockStyle.Top, 
                FlowDirection = FlowDirection.TopDown, 
                WrapContents = false, 
                AutoSize = true, 
                Width = 1100 
            };
            pnlScroll.Controls.Add(flpMain);

            if (isAdmin)
                SetupAdminContent(flpMain);
            else
                SetupTeacherContent(flpMain);

            pnlScroll.Resize += (s, e) => {
                int safeWidth = pnlScroll.Width - 80; 
                if (safeWidth < 900) safeWidth = 900;
                flpMain.Width = safeWidth;
            };
        }

        private void SetupAdminContent(FlowLayoutPanel main)
        {
            // --- A. ADMIN KPI CARDS ---
            FlowLayoutPanel flpStats = new FlowLayoutPanel() { Width = 1100, Height = 160, Margin = new Padding(0, 0, 0, 40), Padding = new Padding(0, 5, 0, 5) };
            
            int pendingLeaves = 0;
            if (AppData.LeaveRequests != null) {
                foreach (DataRow row in AppData.LeaveRequests.Rows) {
                    if (row["Status"].ToString() == "Pending") pendingLeaves++;
                }
            }

            flpStats.Controls.Add(CreateStatCard("TOTAL FACULTY", "128 Active", Color.FromArgb(41, 128, 185), "MANAGE TEACHERS"));
            flpStats.Controls.Add(CreateStatCard("PENDING LEAVES", $"{pendingLeaves} Requests", Color.FromArgb(231, 76, 60), "ADMIN_LEAVE"));
            flpStats.Controls.Add(CreateStatCard("CAMPUS FEEDBACK", "12 Active", Color.FromArgb(241, 196, 15), "ADMIN_FEEDBACK"));
            flpStats.Controls.Add(CreateStatCard("LIVE BROADCASTS", "3 Sent", Color.FromArgb(46, 204, 113), "ADMIN_BROADCAST"));
            main.Controls.Add(flpStats);

            // --- B. COMPARATIVE ANALYTICS (Management & Compliance) ---
            Panel pnlCompare = CreateStyledContainer("📊 INSTITUTIONAL OVERSIGHT & COMPLIANCE");
            pnlCompare.Dock = DockStyle.None; // Reset dock for FlowLayout
            pnlCompare.Height = 280;
            pnlCompare.Width = 1100;
            pnlCompare.Margin = new Padding(0, 0, 0, 40);
            
            
            TableLayoutPanel tlpCompare = new TableLayoutPanel() { 
                Dock = DockStyle.Fill, 
                ColumnCount = 4, 
                Padding = new Padding(15),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                AutoSize = true
            };
            tlpCompare.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpCompare.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpCompare.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpCompare.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            tlpCompare.Controls.Add(CreateComparisonItem("TEACHER QUALITY", new string[] { "Avg Rating: 4.5★", "Top Dept: MSc IT", "Peer Reviews: 85+" }, greenColor), 0, 0);
            tlpCompare.Controls.Add(CreateComparisonItem("CURRICULUM STATUS", new string[] { "Portion Done: 78%", "Syllabus Logs: 240", "Missing Logs: 12" }, blueColor), 1, 0);
            tlpCompare.Controls.Add(CreateComparisonItem("LEAVE EFFICIENCY", new string[] { "Presence: 98.2%", "Pending: 3", "Substitutes: 100%" }, orangeColor), 2, 0);
            tlpCompare.Controls.Add(CreateComparisonItem("LIBRARY & OFFICIAL HUB", new string[] { "Books issued: 45", "Digital Resources: 12", "Office Notices: 8" }, Color.Teal), 3, 0);
            
            pnlCompare.Controls.Add(tlpCompare);
            tlpCompare.BringToFront();
            main.Controls.Add(pnlCompare);

            // --- C. SPLIT SECTION ---
            TableLayoutPanel tlpSplit = new TableLayoutPanel() { Width = 1100, Height = 550, ColumnCount = 2, RowCount = 1, Margin = new Padding(0, 0, 0, 40) };
            tlpSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 63));
            tlpSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37));

            // LEFT: MANAGEMENT TOOLS
            Panel pnlActions = CreateStyledContainer("🛡️ ADMINISTRATIVE MANAGEMENT TOOLS");
            FlowLayoutPanel flpA = new FlowLayoutPanel() { Dock = DockStyle.Fill, Padding = new Padding(10, 15, 10, 0) };
            flpA.Controls.Add(CreateActionButton("Faculty Directory", "👨‍🏫", Color.FromArgb(41, 128, 185), "MANAGE TEACHERS"));
            flpA.Controls.Add(CreateActionButton("Request Approvals", "📝", Color.FromArgb(192, 57, 43), "ADMIN_LEAVE"));
            flpA.Controls.Add(CreateActionButton("Global Announcer", "📢", Color.Maroon, "ADMIN_BROADCAST"));
            flpA.Controls.Add(CreateActionButton("Quality Grievance", "💬", Color.Orange, "ADMIN_FEEDBACK"));
            flpA.Controls.Add(CreateActionButton("Master Timetable", "📅", Color.SeaGreen, "ADMIN_TIMETABLE"));
            flpA.Controls.Add(CreateActionButton("System Audit", "⚙️", Color.Gray, "SETTINGS"));
            pnlActions.Controls.Add(flpA);
            flpA.BringToFront();
            tlpSplit.Controls.Add(pnlActions, 0, 0);

            // RIGHT: RECENT ALERTS
            Panel pnlAlerts = CreateStyledContainer("🔔 CRITICAL SYSTEM ALERTS");
            FlowLayoutPanel flpAlertList = new FlowLayoutPanel() { 
                Dock = DockStyle.Fill, 
                AutoScroll = true, 
                FlowDirection = FlowDirection.TopDown, 
                WrapContents = false, 
                Padding = new Padding(15), 
                BackColor = Color.FromArgb(252, 252, 252),
                MinimumSize = new Size(320, 450)
            };
            
            flpAlertList.Controls.Clear();
            flpAlertList.Controls.Add(CreateAlertItem("SYSTEM MONITOR", "Real-time security active", Color.LimeGreen));
            flpAlertList.Controls.Add(CreateAlertItem("Urgent Leave Request", "Prof. Anita Sharma - Personal Work", Color.Red, "ADMIN_LEAVE"));
            flpAlertList.Controls.Add(CreateAlertItem("Campus Infrastructure", "New complaint raised for Lab 3 Wi-Fi", Color.Orange, "ADMIN_FEEDBACK"));
            flpAlertList.Controls.Add(CreateAlertItem("Syllabus Update", "Mechanical Dept completed 80% coverage", Color.Green, "MANAGE TEACHERS"));
            flpAlertList.Controls.Add(CreateAlertItem("Faculty Feedback", "New peer evaluation submitted by HOD", Color.Blue, "ADMIN_FEEDBACK"));

            pnlAlerts.Controls.Add(flpAlertList);
            flpAlertList.BringToFront();
            tlpSplit.Controls.Add(pnlAlerts, 1, 0);

            main.Controls.Add(tlpSplit);
        }

        private Panel CreateComparisonItem(string title, string[] metrics, Color themeColor)
        {
            Panel p = new Panel() { 
                Width = 260,  // Fixed width instead of Dock.Fill
                Height = 200, // Increased height for more breathing room
                Margin = new Padding(12),  // Increased margins for gaps
                BackColor = Color.White 
            };
            
            // Add complete box border with rounded corners
            p.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                
                // Fill white background
                e.Graphics.Clear(Color.White);
                
                // Draw complete border (all 4 sides)
                Rectangle rect = new Rectangle(0, 0, p.Width - 1, p.Height - 1);
                using (Pen borderPen = new Pen(Color.FromArgb(180, 180, 180), 3))
                {
                    e.Graphics.DrawRectangle(borderPen, rect);
                }
                
                // Top accent bar (thick colored line at top)
                using (SolidBrush accentBrush = new SolidBrush(themeColor))
                {
                    e.Graphics.FillRectangle(accentBrush, 0, 0, p.Width, 6);
                }
            };
            
            // Title with icon
            Label lTitle = new Label() { 
                Text = title, 
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), 
                ForeColor = themeColor, 
                Location = new Point(12, 18),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            p.Controls.Add(lTitle);

            // Metrics
            int yPos = 50;
            foreach (string m in metrics)
            {
                Label lm = new Label() { 
                    Text = "  • " + m, 
                    Font = new Font("Segoe UI", 9F), 
                    ForeColor = clrText, 
                    Location = new Point(12, yPos),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                p.Controls.Add(lm);
                yPos += 28;
            }
            
            return p;
        }

        private void SetupTeacherContent(FlowLayoutPanel main)
        {
            // --- A. TEACHER KPI CARDS ---
            FlowLayoutPanel flpStats = new FlowLayoutPanel() { Width = 1100, Height = 160, Margin = new Padding(0, 0, 0, 40), Padding = new Padding(0, 5, 0, 5) };
            flpStats.Controls.Add(CreateStatCard("PENDING GRADING", "54 Papers", Color.FromArgb(241, 196, 15), "REPORTS"));
            flpStats.Controls.Add(CreateStatCard("LOW ATTENDANCE", "8 Students", Color.FromArgb(231, 76, 60), "ATTENDANCE"));
            flpStats.Controls.Add(CreateStatCard("QUIZZES LIVE", "2 Active", Color.FromArgb(52, 152, 219), "QUIZ"));
            flpStats.Controls.Add(CreateStatCard("TOP PERFORMERS", "12 High", Color.FromArgb(46, 204, 113), "STUDENTS"));
            main.Controls.Add(flpStats);

            // --- B. SPLIT SECTION ---
            TableLayoutPanel tlpSplit = new TableLayoutPanel() { Width = 1100, Height = 550, ColumnCount = 2, RowCount = 1, Margin = new Padding(0, 0, 0, 40) };
            tlpSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
            tlpSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));

            Panel pnlActions = CreateStyledContainer("⚡ QUICK TEACHING ACTIONS");
            FlowLayoutPanel flpA = new FlowLayoutPanel() { Dock = DockStyle.Fill, Padding = new Padding(10, 15, 10, 0) };
            flpA.Controls.Add(CreateActionButton("Teaching Hub", "📊", Color.FromArgb(41, 128, 185), "CLASSES"));
            flpA.Controls.Add(CreateActionButton("Smart Exam", "📝", Color.FromArgb(142, 68, 173), "TESTS"));
            flpA.Controls.Add(CreateActionButton("Daily Attend.", "📅", Color.FromArgb(39, 174, 96), "ATTENDANCE"));
            flpA.Controls.Add(CreateActionButton("Syllabus Log", "📚", Color.FromArgb(211, 84, 0), "SYLLABUS"));
            flpA.Controls.Add(CreateActionButton("Post Alert", "📢", Color.FromArgb(192, 57, 43), "NOTICES"));
            flpA.Controls.Add(CreateActionButton("Assignments", "📉", Color.FromArgb(127, 140, 141), "ASSIGNMENTS"));
            pnlActions.Controls.Add(flpA);
            flpA.BringToFront();
            tlpSplit.Controls.Add(pnlActions, 0, 0);

            Panel pnlInsights = CreateStyledContainer("🔔 UPCOMING EVENTS");
            FlowLayoutPanel flpInsightList = new FlowLayoutPanel() { 
                Dock = DockStyle.Fill, 
                AutoScroll = true, 
                FlowDirection = FlowDirection.TopDown, 
                WrapContents = false, 
                Padding = new Padding(15), 
                BackColor = Color.FromArgb(252, 252, 252), // Off-white to distinguish
                MinimumSize = new Size(320, 450)
            };
            
            // Critical: Clear before adding to avoid ghost controls
            flpInsightList.Controls.Clear();
            
            flpInsightList.Controls.Add(CreateAlertItem("SYSTEM ONLINE", "All modules loaded successfully", Color.LimeGreen));
            flpInsightList.Controls.Add(CreateAlertItem("Remedial Classes", "Special session for CSE Div B", Color.Red, "CLASSES"));
            flpInsightList.Controls.Add(CreateAlertItem("Internal Exam", "Unit Test 1 starting Monday", Color.Purple, "TESTS"));
            flpInsightList.Controls.Add(CreateAlertItem("Attendance Alert", "5 Students in TY Div A are defaulters", Color.Teal, "ATTENDANCE"));
            flpInsightList.Controls.Add(CreateAlertItem("Syllabus Milestone", "Log Unit 3 completion for BSc IT", Color.Brown, "SYLLABUS"));
            flpInsightList.Controls.Add(CreateAlertItem("Student Performance", "New grade report ready for review", Color.Gold, "STUDENTS"));
            flpInsightList.Controls.Add(CreateAlertItem("Digital Resources", "New PDF uploaded for Java Unit 4", Color.DeepPink, "RESOURCES"));
            flpInsightList.Controls.Add(CreateAlertItem("Quiz Live", "Active quiz: 'Data Structures MCQ'", Color.LimeGreen, "QUIZ"));
            flpInsightList.Controls.Add(CreateAlertItem("Leave Request", "Your leave for Friday is Approved", Color.DodgerBlue, "LEAVE"));
            flpInsightList.Controls.Add(CreateAlertItem("Assignment Task", "OOP Unit-2 due tomorrow", Color.Orange, "ASSIGNMENTS"));
            flpInsightList.Controls.Add(CreateAlertItem("Academic Report", "Weekly attendance summary generated", Color.SlateGray, "REPORTS"));
            flpInsightList.Controls.Add(CreateAlertItem("Announcement", "Broadcast library book return date", Color.DarkCyan, "NOTICES"));
            flpInsightList.Controls.Add(CreateAlertItem("Faculty Meeting", "Scheduled at 4:30 PM today", Color.MediumVioletRed, "MESSAGES"));

            pnlInsights.Controls.Add(flpInsightList);
            flpInsightList.BringToFront(); // Force to top layer
            tlpSplit.Controls.Add(pnlInsights, 1, 0);

            main.Controls.Add(tlpSplit);

            // --- D. EXTRA FEATURE: RECENT ACTIVITY LOGS ---
            Panel pnlActivityLog = CreateStyledContainer("🕒 RECENT SYSTEM ACTIVITY & LOGS");
            pnlActivityLog.Dock = DockStyle.None; // Reset dock for FlowLayout
            pnlActivityLog.Width = 1100;
            pnlActivityLog.Height = 350;
            pnlActivityLog.Margin = new Padding(0, 30, 0, 40);

            FlowLayoutPanel flpLogs = new FlowLayoutPanel() { 
                Dock = DockStyle.Fill, 
                AutoScroll = true, 
                FlowDirection = FlowDirection.TopDown, 
                WrapContents = false, 
                Padding = new Padding(15) 
            };
            
            flpLogs.Controls.Add(CreateLogEntry("Attendance Submitted", "You marked TY-IT Div B attendance at 10:15 AM", "ATTENDANCE", "ATTENDANCE"));
            flpLogs.Controls.Add(CreateLogEntry("New Assignment Posted", "Java Unit 4 practicals assigned to SY-IT", "ASSIGNMENTS", "ASSIGNMENTS"));
            flpLogs.Controls.Add(CreateLogEntry("Syllabus Log Updated", "Chapter 5 'Recursion' completed in FY-IT", "SYLLABUS", "SYLLABUS"));
            flpLogs.Controls.Add(CreateLogEntry("PDF Resource Added", "DBMS Normalization notes uploaded to Library", "RESOURCES", "RESOURCES"));
            flpLogs.Controls.Add(CreateLogEntry("Leave request Updated", "Your medical leave for next Monday was Approved", "LEAVE", "LEAVE"));
            flpLogs.Controls.Add(CreateLogEntry("Quiz Published", "Unit Test Mock Quiz is now LIVE for students", "QUIZ", "QUIZ"));

            pnlActivityLog.Controls.Add(flpLogs);
            flpLogs.BringToFront();
            main.Controls.Add(pnlActivityLog);
        }

        private Panel CreateLogEntry(string action, string detail, string tag, string viewTag = null)
        {
            Panel p = new Panel() { 
                Width = 1000, 
                Height = 52, 
                Margin = new Padding(0, 0, 0, 10), 
                Cursor = (viewTag != null ? Cursors.Hand : Cursors.Default) 
            };
            
            // Gradient background with rounded corners
            p.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                
                // Gradient
                using (System.Drawing.Drawing2D.LinearGradientBrush brush = 
                    new System.Drawing.Drawing2D.LinearGradientBrush(
                        p.ClientRectangle,
                        Color.White,
                        Color.FromArgb(252, 252, 252),
                        System.Drawing.Drawing2D.LinearGradientMode.Horizontal))
                {
                    using (System.Drawing.Drawing2D.GraphicsPath path = CreateRoundedRect(0, 0, p.Width - 1, p.Height - 1, 6))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
                
                // Subtle shadow
                using (System.Drawing.Drawing2D.GraphicsPath shadowPath = CreateRoundedRect(1, 1, p.Width - 2, p.Height - 2, 6))
                {
                    using (Pen shadowPen = new Pen(Color.FromArgb(8, 0, 0, 0), 1))
                    {
                        e.Graphics.DrawPath(shadowPen, shadowPath);
                    }
                }
                
                // Border
                using (System.Drawing.Drawing2D.GraphicsPath borderPath = CreateRoundedRect(0, 0, p.Width - 1, p.Height - 1, 6))
                {
                    using (Pen borderPen = new Pen(Color.FromArgb(235, 235, 235), 1))
                    {
                        e.Graphics.DrawPath(borderPen, borderPath);
                    }
                }
            };
            
            TableLayoutPanel tlp = new TableLayoutPanel() { 
                Dock = DockStyle.Fill, 
                ColumnCount = 3, 
                RowCount = 1, 
                BackColor = Color.Transparent,
                Padding = new Padding(15, 0, 15, 0)
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));

            Label l1 = new Label() { 
                Text = "● " + action, 
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), 
                ForeColor = primaryRed, 
                AutoSize = true, 
                Dock = DockStyle.Fill, 
                TextAlign = ContentAlignment.MiddleLeft 
            };
            Label l2 = new Label() { 
                Text = detail, 
                Font = new Font("Segoe UI", 9F), 
                ForeColor = Color.FromArgb(60, 60, 60), 
                AutoSize = true, 
                Dock = DockStyle.Fill, 
                TextAlign = ContentAlignment.MiddleLeft, 
                Padding = new Padding(5, 0, 0, 0) 
            };
            Label l3 = new Label() { 
                Text = "[" + tag + "]", 
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic), 
                ForeColor = blueColor, 
                AutoSize = true, 
                Dock = DockStyle.Fill, 
                TextAlign = ContentAlignment.MiddleRight 
            };

            tlp.Controls.Add(l1, 0, 0);
            tlp.Controls.Add(l2, 1, 0);
            tlp.Controls.Add(l3, 2, 0);

            // Hover effect
            p.MouseEnter += (s, e) => 
            { 
                p.BackColor = Color.FromArgb(248, 248, 248);
                p.Invalidate();
            };
            p.MouseLeave += (s, e) => 
            { 
                p.BackColor = Color.Transparent;
                p.Invalidate();
            };

            // Click navigation
            Action navigate = () => { if (viewTag != null) RequestViewChange?.Invoke(viewTag); };
            tlp.Click += (s, e) => navigate();
            l1.Click += (s, e) => navigate();
            l2.Click += (s, e) => navigate();
            p.Click += (s, e) => navigate();
            
            p.Controls.Add(tlp);
            return p;
        }

        private Panel CreateStyledContainer(string title)
        {
            Panel p = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(20), Margin = new Padding(0, 0, 15, 0) };
            
            // Title panel with gradient background
            Panel titlePanel = new Panel() { Dock = DockStyle.Top, Height = 50 };
            titlePanel.Paint += (s, e) =>
            {
                // Gradient background for title
                using (System.Drawing.Drawing2D.LinearGradientBrush brush = 
                    new System.Drawing.Drawing2D.LinearGradientBrush(
                        titlePanel.ClientRectangle,
                        Color.FromArgb(255, 250, 250),
                        Color.White,
                        System.Drawing.Drawing2D.LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, titlePanel.ClientRectangle);
                }
                
                // Bottom accent line
                using (Pen accentPen = new Pen(primaryRed, 2))
                {
                    e.Graphics.DrawLine(accentPen, 0, titlePanel.Height - 1, titlePanel.Width, titlePanel.Height - 1);
                }
            };
            
            Label l = new Label() { 
                Text = title, 
                Font = new Font("Segoe UI", 11, FontStyle.Bold), 
                ForeColor = primaryRed, 
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5, 0, 0, 0)
            };
            titlePanel.Controls.Add(l);
            p.Controls.Add(titlePanel);
            
            // Enhanced shadow and rounded border
            p.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                
                // Multiple shadow layers for depth
                for (int i = 5; i > 0; i--)
                {
                    using (System.Drawing.Drawing2D.GraphicsPath shadowPath = CreateRoundedRect(i, i, p.Width - (i * 2), p.Height - (i * 2), 10))
                    {
                        using (Pen shadowPen = new Pen(Color.FromArgb(6, 0, 0, 0), 2))
                        {
                            e.Graphics.DrawPath(shadowPen, shadowPath);
                        }
                    }
                }
                
                // Border
                using (System.Drawing.Drawing2D.GraphicsPath borderPath = CreateRoundedRect(0, 0, p.Width - 1, p.Height - 1, 10))
                {
                    using (Pen borderPen = new Pen(Color.FromArgb(220, 220, 220), 1))
                    {
                        e.Graphics.DrawPath(borderPen, borderPath);
                    }
                }
            };
            
            return p;
        }

        private Panel CreateStatCard(string title, string val, Color theme, string viewTag = null)
        {
            Panel p = new Panel() { Width = 260, Height = 140, BackColor = clrCard, Margin = new Padding(0, 0, 25, 20), Cursor = Cursors.Hand };
            
            // Start invisible for fade-in animation
            p.Tag = new { OriginalOpacity = 1.0f, CurrentOpacity = 0.0f };
            
            // Add shadow and rounded corner effect with glow
            p.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                
                // Outer glow effect (multiple shadow layers)
                for (int i = 8; i > 0; i--)
                {
                    using (System.Drawing.Drawing2D.GraphicsPath glowPath = CreateRoundedRect(i, i, p.Width - (i * 2), p.Height - (i * 2), 12))
                    {
                        using (Pen glowPen = new Pen(Color.FromArgb(5, theme.R, theme.G, theme.B), 2))
                        {
                            e.Graphics.DrawPath(glowPen, glowPath);
                        }
                    }
                }
                
                // Main shadow
                using (System.Drawing.Drawing2D.GraphicsPath shadowPath = CreateRoundedRect(3, 3, p.Width - 4, p.Height - 4, 12))
                {
                    using (Pen shadowPen = new Pen(Color.FromArgb(30, 0, 0, 0), 6))
                    {
                        e.Graphics.DrawPath(shadowPen, shadowPath);
                    }
                }
                
                // Border with subtle gradient
                using (System.Drawing.Drawing2D.GraphicsPath borderPath = CreateRoundedRect(0, 0, p.Width - 1, p.Height - 1, 12))
                {
                    using (Pen borderPen = new Pen(Color.FromArgb(230, 230, 230), 2))
                    {
                        e.Graphics.DrawPath(borderPen, borderPath);
                    }
                }
            };
            
            p.Click += (s, e) => { if (viewTag != null) RequestViewChange?.Invoke(viewTag); };

            // Top accent bar with gradient
            Panel bar = new Panel() { Dock = DockStyle.Top, Height = 5 };
            bar.Paint += (s, e) =>
            {
                using (System.Drawing.Drawing2D.LinearGradientBrush brush = 
                    new System.Drawing.Drawing2D.LinearGradientBrush(
                        bar.ClientRectangle,
                        theme,
                        ControlPaint.Light(theme, 0.3f),
                        System.Drawing.Drawing2D.LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, bar.ClientRectangle);
                }
            };
            bar.Click += (s, e) => { if (viewTag != null) RequestViewChange?.Invoke(viewTag); };
            
            Label lblT = new Label() { 
                Text = title, 
                Font = new Font("Segoe UI", 8, FontStyle.Bold), 
                ForeColor = clrGray, 
                Location = new Point(15, 25), 
                AutoSize = true 
            };
            Label lblV = new Label() { 
                Text = val, 
                Font = new Font("Segoe UI", 24, FontStyle.Bold), 
                ForeColor = theme, 
                Location = new Point(12, 55), 
                AutoSize = true 
            };
            
            lblT.Click += (s, e) => { if (viewTag != null) RequestViewChange?.Invoke(viewTag); };
            lblV.Click += (s, e) => { if (viewTag != null) RequestViewChange?.Invoke(viewTag); };

            // Enhanced hover effect with scale illusion
            p.MouseEnter += (s, e) => 
            { 
                p.BackColor = Color.FromArgb(250, 250, 250);
                lblV.ForeColor = ControlPaint.Light(theme, 0.2f);
                p.Invalidate();
            };
            p.MouseLeave += (s, e) => 
            { 
                p.BackColor = clrCard;
                lblV.ForeColor = theme;
                p.Invalidate();
            };

            p.Controls.AddRange(new Control[] { bar, lblT, lblV });
            return p;
        }

        // Helper method for creating rounded rectangles
        private System.Drawing.Drawing2D.GraphicsPath CreateRoundedRect(int x, int y, int width, int height, int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(x, y, radius, radius, 180, 90);
            path.AddArc(x + width - radius, y, radius, radius, 270, 90);
            path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
            path.AddArc(x, y + height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        private Button CreateActionButton(string text, string icon, Color color, string viewTag = null)
        {
            Button b = new Button();
            b.Size = new Size(185, 125);
            b.BackColor = Color.White;
            b.ForeColor = clrText;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Margin = new Padding(15);
            b.Cursor = Cursors.Hand;
            b.Text = icon + "\n\n" + text;
            b.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            b.TextAlign = ContentAlignment.MiddleCenter;
            
            // Enhanced shadow and rounded corners with glow
            b.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                
                // Colored glow effect on hover
                if (b.BackColor != Color.White)
                {
                    for (int i = 6; i > 0; i--)
                    {
                        using (System.Drawing.Drawing2D.GraphicsPath glowPath = CreateRoundedRect(i, i, b.Width - (i * 2), b.Height - (i * 2), 8))
                        {
                            using (Pen glowPen = new Pen(Color.FromArgb(8, color.R, color.G, color.B), 2))
                            {
                                e.Graphics.DrawPath(glowPen, glowPath);
                            }
                        }
                    }
                }
                
                // Main shadow
                using (System.Drawing.Drawing2D.GraphicsPath shadowPath = CreateRoundedRect(2, 2, b.Width - 3, b.Height - 3, 8))
                {
                    using (Pen shadowPen = new Pen(Color.FromArgb(20, 0, 0, 0), 4))
                    {
                        e.Graphics.DrawPath(shadowPen, shadowPath);
                    }
                }
                
                // Border
                using (System.Drawing.Drawing2D.GraphicsPath borderPath = CreateRoundedRect(0, 0, b.Width - 1, b.Height - 1, 8))
                {
                    Color borderColor = b.BackColor == Color.White ? Color.FromArgb(235, 235, 235) : color;
                    using (Pen borderPen = new Pen(borderColor, 2))
                    {
                        e.Graphics.DrawPath(borderPen, borderPath);
                    }
                }
            };
            
            if (viewTag != null)
                b.Click += (s, e) => RequestViewChange?.Invoke(viewTag);

            // Premium hover effect with gradient background
            b.MouseEnter += (s, e) => 
            { 
                b.BackColor = Color.FromArgb(252, 252, 252);
                b.ForeColor = color;
                b.Invalidate();
            };
            b.MouseLeave += (s, e) => 
            { 
                b.BackColor = Color.White;
                b.ForeColor = clrText;
                b.Invalidate();
            };

            // Colored accent strip with gradient
            Panel strip = new Panel() { Width = 5, Dock = DockStyle.Left };
            strip.Paint += (s, e) =>
            {
                using (System.Drawing.Drawing2D.LinearGradientBrush brush = 
                    new System.Drawing.Drawing2D.LinearGradientBrush(
                        strip.ClientRectangle,
                        color,
                        ControlPaint.Light(color, 0.3f),
                        System.Drawing.Drawing2D.LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, strip.ClientRectangle);
                }
            };
            b.Controls.Add(strip);

            return b;
        }

        private TableLayoutPanel CreateAlertItem(string title, string desc, Color dotColor, string viewTag = null)
        {
            TableLayoutPanel tlp = new TableLayoutPanel() { 
                Width = 330, 
                MinimumSize = new Size(300, 75),
                AutoSize = true,
                Padding = new Padding(12),
                Margin = new Padding(0, 0, 0, 12),
                Cursor = (viewTag != null ? Cursors.Hand : Cursors.Default)
            };
            
            // Gradient background with shadow
            tlp.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                
                // Gradient background
                using (System.Drawing.Drawing2D.LinearGradientBrush brush = 
                    new System.Drawing.Drawing2D.LinearGradientBrush(
                        tlp.ClientRectangle,
                        Color.White,
                        Color.FromArgb(254, 254, 254),
                        45f))
                {
                    using (System.Drawing.Drawing2D.GraphicsPath path = CreateRoundedRect(0, 0, tlp.Width - 1, tlp.Height - 1, 8))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
                
                // Shadow effect
                using (System.Drawing.Drawing2D.GraphicsPath shadowPath = CreateRoundedRect(1, 1, tlp.Width - 2, tlp.Height - 2, 8))
                {
                    using (Pen shadowPen = new Pen(Color.FromArgb(10, 0, 0, 0), 2))
                    {
                        e.Graphics.DrawPath(shadowPen, shadowPath);
                    }
                }
                
                // Border
                using (System.Drawing.Drawing2D.GraphicsPath borderPath = CreateRoundedRect(0, 0, tlp.Width - 1, tlp.Height - 1, 8))
                {
                    using (Pen borderPen = new Pen(Color.FromArgb(240, 240, 240), 1))
                    {
                        e.Graphics.DrawPath(borderPen, borderPath);
                    }
                }
            };
            
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 25F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Colored dot indicator
            Panel dot = new Panel() { Width = 10, Height = 10, BackColor = dotColor, Margin = new Padding(5, 7, 0, 0) };
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, 10, 10);
            dot.Region = new Region(gp);

            Label l1 = new Label() { 
                Text = title, 
                Font = new Font("Segoe UI", 10, FontStyle.Bold), 
                ForeColor = Color.FromArgb(40, 40, 40), 
                AutoSize = true, 
                Dock = DockStyle.Top 
            };
            Label l2 = new Label() { 
                Text = desc, 
                Font = new Font("Segoe UI", 9), 
                ForeColor = Color.DarkGray, 
                Dock = DockStyle.Top, 
                AutoSize = true 
            };

            tlp.Controls.Add(dot, 0, 0);
            tlp.Controls.Add(l1, 1, 0);
            tlp.Controls.Add(l2, 1, 1);

            // Hover effect
            Color originalBg = tlp.BackColor;
            tlp.MouseEnter += (s, e) => 
            { 
                tlp.BackColor = Color.FromArgb(248, 248, 248);
                tlp.Invalidate();
            };
            tlp.MouseLeave += (s, e) => 
            { 
                tlp.BackColor = originalBg;
                tlp.Invalidate();
            };

            // Click handler
            Action onClick = () => { if (viewTag != null) RequestViewChange?.Invoke(viewTag); };
            tlp.Click += (s, e) => onClick();
            l1.Click += (s, e) => onClick();
            l2.Click += (s, e) => onClick();
            
            return tlp;
        }

    }
}
