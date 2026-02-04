using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public class SimpleChart : Control
    {
        public enum Type { Pie, Bar, Line }
        public Type ChartType { get; set; } = Type.Bar;
        public List<DataPoint> DataPoints { get; set; } = new List<DataPoint>();

        public SimpleChart()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;
            this.BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (DataPoints.Count == 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (ChartType == Type.Pie)
            {
                DrawPieChart(g);
            }
            else
            {
                DrawBarChart(g);
            }
        }

        private void DrawPieChart(Graphics g)
        {
            int margin = 20;
            Rectangle rect = new Rectangle(margin, margin, Math.Min(Width, Height) - (margin * 2), Math.Min(Width, Height) - (margin * 2));
            
            float total = 0;
            foreach (var dp in DataPoints) total += dp.Value;

            float startAngle = 0;
            for (int i = 0; i < DataPoints.Count; i++)
            {
                float sweepAngle = (DataPoints[i].Value / total) * 360f;
                using (SolidBrush brush = new SolidBrush(DataPoints[i].DotColor))
                {
                    g.FillPie(brush, rect, startAngle, sweepAngle);
                }
                startAngle += sweepAngle;
            }

            // Legend
            int ly = margin;
            int lx = rect.Right + 20;
            Font font = new Font("Segoe UI", 9);
            foreach (var dp in DataPoints)
            {
                using (SolidBrush brush = new SolidBrush(dp.DotColor))
                {
                    g.FillRectangle(brush, lx, ly, 12, 12);
                    g.DrawString($"{dp.Label} ({dp.Value}%)", font, Brushes.White, lx + 20, ly - 2);
                }
                ly += 22;
            }
        }

        private void DrawBarChart(Graphics g)
        {
            int margin = 40;
            int barWidth = (Width - (margin * 2)) / DataPoints.Count - 10;
            float maxVal = 0;
            foreach (var dp in DataPoints) if (dp.Value > maxVal) maxVal = dp.Value;
            if (maxVal == 0) maxVal = 100;

            Font font = new Font("Segoe UI", 8);

            for (int i = 0; i < DataPoints.Count; i++)
            {
                int h = (int)((DataPoints[i].Value / maxVal) * (Height - (margin * 2)));
                int x = margin + (i * (barWidth + 10));
                int y = Height - margin - h;

                using (SolidBrush brush = new SolidBrush(DataPoints[i].DotColor))
                {
                    g.FillRectangle(brush, x, y, barWidth, h);
                }

                g.DrawString(DataPoints[i].Label, font, Brushes.Gray, x, Height - margin + 5);
                g.DrawString(DataPoints[i].Value.ToString(), font, Brushes.White, x, y - 15);
            }
        }

        public class DataPoint
        {
            public string Label { get; set; }
            public float Value { get; set; }
            public Color DotColor { get; set; }

            public DataPoint(string label, float value, Color color)
            {
                Label = label;
                Value = value;
                DotColor = color;
            }
        }
    }
}
