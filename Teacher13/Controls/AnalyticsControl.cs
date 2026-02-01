using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TeacherDashboard.Controls
{
    public partial class AnalyticsControl : UserControl
    {
        public AnalyticsControl()
        {
            InitializeComponent();
            SetupStrictLayout();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitle = new Label() { Text = "DEPARTMENTAL ANALYTICS", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            // 2. Statistics Grid (The circular charts)
            Panel pnlCircles = new Panel() { Dock = DockStyle.Top, Height = 250, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(20) };
            pnlScroll.Controls.Add(pnlCircles);

            Panel pnlDraw = new Panel() { Dock = DockStyle.Fill };
            pnlDraw.Paint += (s, e) => {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                DrawStatCircle(g, "Faculty Strength", 85, new Point(50, 20), Color.FromArgb(173, 22, 37));
                DrawStatCircle(g, "Resource Usage", 65, new Point(250, 20), Color.FromArgb(52, 152, 219));
                DrawStatCircle(g, "Budget Utilization", 45, new Point(450, 20), Color.FromArgb(46, 204, 113));
            };
            pnlCircles.Controls.Add(pnlDraw);

            // 3. Goals Section (Replacing the redundant SimpleChart)
            Label lblGoalTitle = new Label() { 
                Text = "STRATEGIC DEPARTMENTAL GOALS", 
                Font = new Font("Segoe UI", 12, FontStyle.Bold), 
                ForeColor = Color.White, 
                Dock = DockStyle.Top, 
                Height = 50, 
                Padding = new Padding(0, 20, 0, 0) 
            };
            pnlScroll.Controls.Add(lblGoalTitle);

            FlowLayoutPanel flpGoals = new FlowLayoutPanel() { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(0, 10, 0, 20) };
            flpGoals.Controls.Add(CreateGoalItem("Research Publications", 75, Color.FromArgb(173, 22, 37)));
            flpGoals.Controls.Add(CreateGoalItem("Placement Ratio", 92, Color.FromArgb(46, 204, 113)));
            flpGoals.Controls.Add(CreateGoalItem("Student Satisfaction", 88, Color.FromArgb(52, 152, 219)));
            flpGoals.Controls.Add(CreateGoalItem("Infra Development", 40, Color.FromArgb(241, 196, 15)));
            
            pnlScroll.Controls.Add(flpGoals);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 1); // Docks First (Top)
            this.Controls.SetChildIndex(pnlScroll, 0); // Docks Last (Fill)
        }

        private Panel CreateGoalItem(string title, int progress, Color accent)
        {
            Panel p = new Panel() { Size = new Size(450, 80), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 0, 15) };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 15), AutoSize = true };
            Label lblP = new Label() { Text = progress + "%", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = accent, Location = new Point(400, 15), AutoSize = true };
            
            Panel pnlBarBg = new Panel() { Location = new Point(15, 45), Size = new Size(410, 12), BackColor = Color.FromArgb(45, 45, 45) };
            Panel pnlBarFg = new Panel() { Location = new Point(0, 0), Size = new Size((410 * progress) / 100, 12), BackColor = accent };
            pnlBarBg.Controls.Add(pnlBarFg);
            
            p.Controls.AddRange(new Control[] { lblT, lblP, pnlBarBg });
            return p;
        }

        private void DrawStatCircle(Graphics g, string title, int percent, Point p, Color c)
        {
            g.DrawArc(new Pen(Color.FromArgb(240, 240, 240), 10), p.X, p.Y, 130, 130, 0, 360);
            g.DrawArc(new Pen(c, 10), p.X, p.Y, 130, 130, -90, (int)(percent * 3.6));
            
            string txt = percent + "%";
            Font f = new Font("Segoe UI", 18, FontStyle.Bold);
            SizeF sz = g.MeasureString(txt, f);
            g.DrawString(txt, f, new SolidBrush(c), p.X + (130 - sz.Width) / 2, p.Y + (130 - sz.Height) / 2);
            
            Font tf = new Font("Segoe UI", 9, FontStyle.Bold);
            SizeF tsz = g.MeasureString(title, tf);
            g.DrawString(title, tf, Brushes.DimGray, p.X + (130 - tsz.Width) / 2, p.Y + 145);
        }
    }
}
