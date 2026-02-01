using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public partial class DashboardControl : UserControl
    {
        public DashboardControl(string role = "Teacher", string userName = "User")
        {
            InitializeComponent();
            ApplyDarkTheme();
            lblWelcome.Text = $"Welcome Back, {userName}!";
            pnlChartMock.Paint += PnlChartMock_Paint;
            LoadRecentActivities(role);
        }

        private void ApplyDarkTheme()
        {
            this.BackColor = Color.FromArgb(18, 18, 18);
            lblWelcome.ForeColor = Color.White;
            pnlCards.BackColor = Color.Transparent;
            pnlChartMock.BackColor = Color.FromArgb(32, 33, 36);
            
            foreach (Control ctrl in pnlCards.Controls)
            {
                if (ctrl is StatCard card)
                {
                    card.BackColor = Color.FromArgb(32, 33, 36);
                    card.ForeColor = Color.White;
                }
            }
        }

        // Removed DashboardControl_Resize as we will use Dock/Anchor better.

        private void LoadRecentActivities(string role)
        {
            bool isAdmin = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);

            // 1. Clear existing for fresh layout
            this.Controls.Clear();

            // 2. RIGHT SIDEBAR
            Panel pnlRightSide = new Panel();
            pnlRightSide.Dock = DockStyle.Right;
            pnlRightSide.Width = 280; 
            pnlRightSide.BackColor = Color.FromArgb(24, 25, 26);
            pnlRightSide.Padding = new Padding(15, 25, 15, 25);
            this.Controls.Add(pnlRightSide);

            // 3. MAIN CONTENT
            Panel pnlMainContent = new Panel();
            pnlMainContent.Dock = DockStyle.Fill;
            pnlMainContent.BackColor = Color.Transparent;
            pnlMainContent.Padding = new Padding(35);
            this.Controls.Add(pnlMainContent);

            // Welcome Message with Role Context
            lblWelcome.Text = $"Welcome, {role.ToUpper()}";
            lblWelcome.Dock = DockStyle.Top;
            lblWelcome.Height = 70;
            lblWelcome.Padding = new Padding(0, 10, 0, 0); 
            lblWelcome.Font = new Font("Segoe UI", 26, FontStyle.Bold); 
            lblWelcome.ForeColor = Color.White;
            lblWelcome.BackColor = Color.Transparent;
            pnlMainContent.Controls.Add(lblWelcome);

            pnlCards.Dock = DockStyle.Top;
            pnlCards.Height = 120;
            pnlCards.BackColor = Color.Transparent;
            pnlMainContent.Controls.Add(pnlCards);

            // Role-Specific Stats
            if (isAdmin)
            {
                pnlCards.Controls.Add(new StatCard() { Title = "Total Faculty", Value = "128", AccentColor = Color.FromArgb(155, 89, 182), Size = new Size(180, 100) });
                pnlCards.Controls.Add(new StatCard() { Title = "System Load", Value = "12%", AccentColor = Color.FromArgb(46, 204, 113), Size = new Size(180, 100) });
                pnlCards.Controls.Add(new StatCard() { Title = "Active Users", Value = "1,240", AccentColor = Color.FromArgb(52, 152, 219), Size = new Size(180, 100) });
            }
            else
            {
                pnlCards.Controls.Add(new StatCard() { Title = "Classes Today", Value = "04", AccentColor = Color.FromArgb(173, 22, 37), Size = new Size(180, 100) });
                pnlCards.Controls.Add(new StatCard() { Title = "Pending Tasks", Value = "12", AccentColor = Color.FromArgb(241, 196, 15), Size = new Size(180, 100) });
                pnlCards.Controls.Add(new StatCard() { Title = "Student Reach", Value = "450+", AccentColor = Color.FromArgb(46, 204, 113), Size = new Size(180, 100) });
            }

            pnlChartMock.Dock = DockStyle.Fill;
            pnlChartMock.BackColor = Color.FromArgb(32, 33, 36);
            pnlMainContent.Controls.Add(pnlChartMock);

            // Force Strict Docking Priority (Main Area)
            pnlMainContent.Controls.SetChildIndex(lblWelcome, 2);   // Docks Top First
            pnlMainContent.Controls.SetChildIndex(pnlCards, 1);     // Docks Top Second
            pnlMainContent.Controls.SetChildIndex(pnlChartMock, 0); // Docks Last (Fill)

            Label lblTodoTitle = new Label() { Text = "PERSONAL TO-DO LIST", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.Gray, Dock = DockStyle.Top, Height = 35 };
            pnlRightSide.Controls.Add(lblTodoTitle);

            CheckedListBox clbTodo = new CheckedListBox() { 
                Dock = DockStyle.Top, Height = 130, 
                BorderStyle = BorderStyle.None, BackColor = Color.FromArgb(24, 25, 26), 
                ForeColor = Color.White, Font = new Font("Segoe UI", 9) 
            };
            
            if (isAdmin)
                clbTodo.Items.AddRange(new[] { "Approve Faculty Leaves", "Database Audit", "Update Fee Portal", "Check Server Logs" });
            else
                clbTodo.Items.AddRange(new[] { "Grade Assignments", "Upload Syllabus Unit 4", "Mark P1 Attendance", "Draft Quiz 2" });

            pnlRightSide.Controls.Add(clbTodo);

            Label lblLogTitle = new Label() { Text = isAdmin ? "ADMIN SYSTEM LOGS" : "ACADEMIC ACTIVITY", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(173, 22, 37), Dock = DockStyle.Top, Height = 35, Margin = new Padding(0, 15, 0, 0) };
            pnlRightSide.Controls.Add(lblLogTitle);

            ListBox lstLogs = new ListBox() { Dock = DockStyle.Top, Height = 140, BorderStyle = BorderStyle.None, BackColor = Color.FromArgb(24, 25, 26), ForeColor = Color.DimGray, Font = new Font("Segoe UI", 8.5f) };
            if (isAdmin)
                lstLogs.Items.AddRange(new[] { "• User 'Anil' permissions updated", "• Backup generated (v1.4.2)", "• Security patch applied", "• 5 new registrations" });
            else
                lstLogs.Items.AddRange(new[] { "• Attendance synced", "• Marks shared with Students", "• Resource 'Lec_2.pdf' uploaded", "• Feedback received" });
            
            pnlRightSide.Controls.Add(lstLogs);

            Label lblTools = new Label() { Text = "INSTANT ACTIONS", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.Gray, Dock = DockStyle.Top, Height = 35, Margin = new Padding(0, 15, 0, 0) };
            pnlRightSide.Controls.Add(lblTools);

            FlowLayoutPanel flpQuick = new FlowLayoutPanel() { Dock = DockStyle.Fill, Padding = new Padding(0, 5, 0, 0) };
            string[] toolList = isAdmin ? new[] { "DB Config", "Access", "Logs", "Security" } : new[] { "Attendance", "Syllabus", "Grades", "Exams" };
            foreach(var tool in toolList)
            {
                Button btn = new Button() { Text = tool, Size = new Size(110, 35), FlatStyle = FlatStyle.Flat, ForeColor = Color.White, Font = new Font("Segoe UI", 8, FontStyle.Bold), Margin = new Padding(0, 0, 5, 5) };
                btn.FlatAppearance.BorderColor = Color.FromArgb(50, 50, 50);
                flpQuick.Controls.Add(btn);
            }
            pnlRightSide.Controls.Add(flpQuick);

            // Force Strict Docking Priority (Right Sidebar)
            pnlRightSide.Controls.SetChildIndex(lblTodoTitle, 5);
            pnlRightSide.Controls.SetChildIndex(clbTodo, 4);
            pnlRightSide.Controls.SetChildIndex(lblLogTitle, 3);
            pnlRightSide.Controls.SetChildIndex(lstLogs, 2);
            pnlRightSide.Controls.SetChildIndex(lblTools, 1);
            pnlRightSide.Controls.SetChildIndex(flpQuick, 0);
        }

        private void PnlChartMock_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.FromArgb(32, 33, 36));
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            // "Digital" Chart Styling
            int[] values = { 85, 78, 92, 65, 88, 70, 95 };
            string[] labels = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            
            int startX = 70;
            int startY = 400; 
            int barWidth = 45;
            int gap = 50;

            // Draw Background Grid with Labels
            for (int i = 0; i < 6; i++)
            {
                int y = startY - (i * 70);
                g.DrawLine(new Pen(Color.FromArgb(45, 45, 45), 1), 60, y, 750, y);
                g.DrawString((i * 20).ToString() + "%", new Font("Segoe UI", 8), Brushes.DimGray, 25, y - 7);
            }

            // Draw Area Gradient
            Point[] points = new Point[values.Length + 2];
            points[0] = new Point(startX + (barWidth/2), startY);
            for (int i = 0; i < values.Length; i++)
            {
                points[i+1] = new Point(startX + (i * (barWidth + gap)) + (barWidth/2), startY - (int)(values[i] * 3.0));
            }
            points[points.Length - 1] = new Point(startX + ((values.Length-1) * (barWidth + gap)) + (barWidth/2), startY);

            using (LinearGradientBrush lgb = new LinearGradientBrush(new Rectangle(0,0,10,startY), Color.FromArgb(40, 173, 22, 37), Color.Transparent, 90f))
            {
                g.FillPolygon(lgb, points);
            }

            // Draw Bars
            for (int i = 0; i < values.Length; i++)
            {
                int barHeight = (int)(values[i] * 3.0);
                Rectangle barRect = new Rectangle(startX + (i * (barWidth + gap)), startY - barHeight, barWidth, barHeight);
                
                using (LinearGradientBrush b = new LinearGradientBrush(barRect, Color.FromArgb(173, 22, 37), Color.FromArgb(60, 173, 22, 37), 90f))
                {
                    g.FillRectangle(b, barRect);
                }

                // Bar Top Highlight
                using (Pen p = new Pen(Color.FromArgb(173, 22, 37), 2))
                {
                    g.DrawLine(p, barRect.Left, barRect.Top, barRect.Right, barRect.Top);
                }

                g.DrawString(labels[i], new Font("Segoe UI", 9, FontStyle.Bold), Brushes.LightGray, startX + (i * (barWidth + gap)) + 5, startY + 15);
                g.DrawString(values[i] + "%", new Font("Segoe UI", 8, FontStyle.Bold), Brushes.White, startX + (i * (barWidth + gap)) + 8, startY - barHeight - 25);
            }

            g.DrawString("STUDENT ENGAGEMENT SCORE (WEEKLY)", new Font("Segoe UI", 12, FontStyle.Bold), Brushes.White, 25, 25);
        }
    }

    public class StatCard : Panel
    {
        public string Title { get; set; } = "Title";
        public string Value { get; set; } = "0";
        public Color AccentColor { get; set; } = Color.FromArgb(173, 22, 37);

        public StatCard()
        {
            this.Size = new Size(200, 100);
            this.BackColor = Color.FromArgb(32, 33, 36);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            // Draw border
            using (Pen p = new Pen(Color.FromArgb(45, 45, 45), 1))
            {
                g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
            }

            // Draw accent bar
            using (SolidBrush b = new SolidBrush(AccentColor))
            {
                g.FillRectangle(b, 0, 0, 5, Height);
            }

            // Draw Title
            using (Font f = new Font("Segoe UI", 9, FontStyle.Bold))
            {
                g.DrawString(Title.ToUpper(), f, Brushes.DimGray, 15, 20);
            }

            // Draw Value
            using (Font f = new Font("Segoe UI", 20, FontStyle.Bold))
            {
                g.DrawString(Value, f, Brushes.White, 15, 45);
            }
        }
    }
}
