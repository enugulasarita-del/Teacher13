using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TeacherDashboard.Controls
{
    public class SimpleChart : Panel
    {
        public Dictionary<string, int> Data = new Dictionary<string, int>();
        public Color ChartColor { get; set; } = Color.FromArgb(173, 22, 37);
        public string Title { get; set; } = "Chart";

        public SimpleChart()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (Data.Count == 0) return;

            int margin = 60;
            int chartWidth = this.Width - (margin * 2);
            int chartHeight = this.Height - (margin * 2);
            int maxVal = 100;

            // Draw Axes
            using (Pen axisPen = new Pen(Color.LightGray, 1))
            {
                g.DrawLine(axisPen, margin, margin, margin, margin + chartHeight);
                g.DrawLine(axisPen, margin, margin + chartHeight, margin + chartWidth, margin + chartHeight);
            }

            // Draw Bars
            int barWidth = (chartWidth / Data.Count) - 20;
            int x = margin + 10;

            foreach (var entry in Data)
            {
                int barHeight = (int)((float)entry.Value / maxVal * chartHeight);
                Rectangle rect = new Rectangle(x, margin + chartHeight - barHeight, barWidth, barHeight);
                
                using (LinearGradientBrush lgb = new LinearGradientBrush(rect, ChartColor, Color.FromArgb(200, ChartColor), 90f))
                {
                    g.FillRectangle(lgb, rect);
                }

                g.DrawString(entry.Key, new Font("Segoe UI", 8), Brushes.Gray, x, margin + chartHeight + 10);
                g.DrawString(entry.Value.ToString() + "%", new Font("Segoe UI", 8, FontStyle.Bold), new SolidBrush(ChartColor), x + (barWidth / 4), margin + chartHeight - barHeight - 20);

                x += barWidth + 20;
            }
        }
    }
}
