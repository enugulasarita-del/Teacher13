using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TeacherDashboard.Controls
{
    public partial class DashboardControl : UserControl
    {
        private string role;
        private string name;
        
        // Theme Constants
        private Color clrBackground = Color.FromArgb(18, 18, 18);
        private Color clrCard = Color.FromArgb(30, 30, 33);
        private Color clrText = Color.White;
        private Color clrGray = Color.Gray;
        
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

            // --- ROOT LAYOUT (Prevents Overlap) ---
            TableLayoutPanel rootLayout = new TableLayoutPanel();
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F)); // Header
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Body
            rootLayout.Padding = new Padding(0);
            rootLayout.Margin = new Padding(0);
            this.Controls.Add(rootLayout);

            // 1. HEADER
            Panel pnlHeader = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(25, 25, 25) };
            
            // Fix: Remove Date/Time as requested and fix "Prof. Prof." duplication
            Label lblGreeting = new Label() { 
                Text = $"Good Day, {name}", 
                Font = new Font("Segoe UI", 24, FontStyle.Bold), 
                ForeColor = Color.White, 
                Location = new Point(30, 20), 
                AutoSize = true 
            };
            pnlHeader.Controls.Add(lblGreeting);
            
            rootLayout.Controls.Add(pnlHeader, 0, 0);

            // 2. SCROLLABLE BODY
            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, BackColor = clrBackground };
            pnlScroll.Padding = new Padding(30);
            rootLayout.Controls.Add(pnlScroll, 0, 1);

            FlowLayoutPanel flpMain = new FlowLayoutPanel() { 
                Dock = DockStyle.Top, 
                FlowDirection = FlowDirection.TopDown, 
                WrapContents = false, 
                AutoSize = true, 
                Width = 1100 
            };
            pnlScroll.Controls.Add(flpMain);

            // --- A. KPI CARDS ROW (High Level Stats) ---
            FlowLayoutPanel flpStats = new FlowLayoutPanel() { Width = 1100, Height = 130, Margin = new Padding(0, 0, 0, 30) };
            flpStats.Controls.Add(CreateStatCard("PENDING GRADING", "54 Papers", Color.FromArgb(241, 196, 15))); // Linked to AssignmentControl
            flpStats.Controls.Add(CreateStatCard("CRITICAL ATTENDANCE", "8 Students", Color.FromArgb(231, 76, 60))); // Linked to Attendance
            flpStats.Controls.Add(CreateStatCard("ACTIVE QUIZZES", "2 Live", Color.FromArgb(52, 152, 219))); // Linked to QuizControl
            flpStats.Controls.Add(CreateStatCard("MERIT ACHIEVERS", "12 High", Color.FromArgb(46, 204, 113))); // Linked to StudentsControl
            flpMain.Controls.Add(flpStats);

            // --- B. SPLIT SECTION (Quick Actions & Insights) ---
            TableLayoutPanel tlpSplit = new TableLayoutPanel() { Width = 1100, Height = 400, ColumnCount = 2, RowCount = 1 };
            tlpSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            tlpSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

            // LEFT: QUICK ACTIONS GRID
            Panel pnlActions = new Panel() { Dock = DockStyle.Fill, BackColor = clrCard, Padding = new Padding(20), Margin = new Padding(0, 0, 20, 0) };
            Label lblAct = new Label() { Text = "⚡ QUICK ACTIONS", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            FlowLayoutPanel flpActions = new FlowLayoutPanel() { Dock = DockStyle.Fill };
            
            flpActions.Controls.Add(CreateActionButton("New Assignment", "📦", Color.FromArgb(41, 128, 185)));
            flpActions.Controls.Add(CreateActionButton("Create Quiz", "📝", Color.FromArgb(142, 68, 173)));
            flpActions.Controls.Add(CreateActionButton("Mark Attendance", "📅", Color.FromArgb(39, 174, 96)));
            flpActions.Controls.Add(CreateActionButton("Syllabus Log", "📚", Color.FromArgb(211, 84, 0)));
            flpActions.Controls.Add(CreateActionButton("Send Notice", "📢", Color.FromArgb(192, 57, 43)));
            flpActions.Controls.Add(CreateActionButton("View Reports", "📊", Color.FromArgb(127, 140, 141)));

            pnlActions.Controls.Add(flpActions);
            pnlActions.Controls.Add(lblAct);
            tlpSplit.Controls.Add(pnlActions, 0, 0);

            // RIGHT: INSIGHTS LIST (Mocking data from other modules)
            Panel pnlInsights = new Panel() { Dock = DockStyle.Fill, BackColor = clrCard, Padding = new Padding(20) };
            Label lblIns = new Label() { Text = "🔔 PRIORITY ALERTS", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            FlowLayoutPanel flpInsights = new FlowLayoutPanel() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, AutoScroll = true };

            flpInsights.Controls.Add(CreateInsightItem("Needs Remedial: Vikram Singh", "Scored 5/20 in Digital Electronics", Color.FromArgb(231, 76, 60))); // From StudentControl
            flpInsights.Controls.Add(CreateInsightItem("Assignment Due Today", "AS-101: Intro to OOP (Div A)", Color.FromArgb(241, 196, 15))); // From AssignmentControl
            flpInsights.Controls.Add(CreateInsightItem("System Update", "New Quiz Features Added", Color.FromArgb(52, 152, 219)));
            
            pnlInsights.Controls.Add(flpInsights);
            pnlInsights.Controls.Add(lblIns);
            tlpSplit.Controls.Add(pnlInsights, 1, 0);

            flpMain.Controls.Add(tlpSplit);

            pnlScroll.Resize += (s, e) => {
                // Fix: Increased margin to prevent horizontal scrolling/overlap issues
                int safeWidth = pnlScroll.Width - 80; 
                if (safeWidth < 800) safeWidth = 800; // Minimum safe width
                
                flpMain.Width = safeWidth;
                flpStats.Width = flpMain.Width;
                tlpSplit.Width = flpMain.Width;
            };
        }

        private Panel CreateStatCard(string title, string val, Color theme)
        {
            Panel p = new Panel() { Width = 250, Height = 110, BackColor = clrCard, Margin = new Padding(0, 0, 20, 0) };
            Panel bar = new Panel() { Dock = DockStyle.Top, Height = 4, BackColor = theme };
            
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.Gray, Location = new Point(15, 20), AutoSize = true };
            Label lblV = new Label() { Text = val, Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 45), AutoSize = true };
            
            p.Controls.AddRange(new Control[] { bar, lblT, lblV });
            return p;
        }

        private Button CreateActionButton(string text, string icon, Color color)
        {
            Button b = new Button();
            b.Size = new Size(200, 100);
            b.BackColor = Color.FromArgb(38, 38, 42); // Slightly lighter than card
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Margin = new Padding(0, 0, 15, 15);
            b.Cursor = Cursors.Hand;
            b.Text = icon + "\n" + text;
            b.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            b.TextAlign = ContentAlignment.MiddleCenter;
            
            // Side strip
            Panel strip = new Panel() { Width = 5, Dock = DockStyle.Left, BackColor = color, Height = 100 };
            b.Controls.Add(strip);

            return b;
        }

        private Panel CreateInsightItem(string title, string desc, Color dotColor)
        {
            Panel p = new Panel() { Width = 380, Height = 60, Margin = new Padding(0, 0, 0, 10), BackColor = Color.Transparent };
            Panel dot = new Panel() { Width = 10, Height = 10, BackColor = dotColor, Location = new Point(5, 15) }; // Circle
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, 10, 10);
            dot.Region = new Region(gp);

            Label l1 = new Label() { Text = title, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.White, Location = new Point(25, 10), AutoSize = true };
            Label l2 = new Label() { Text = desc, Font = new Font("Segoe UI", 9, FontStyle.Regular), ForeColor = Color.LightGray, Location = new Point(25, 30), AutoSize = true };

            p.Controls.AddRange(new Control[] { dot, l1, l2 });
            return p;
        }


    }
}
