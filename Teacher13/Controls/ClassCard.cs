using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public class ClassCard : Panel
    {
        public string ClassName { get; set; } = "Class Name";
        public string Subject { get; set; } = "Subject";
        public string StudentsCount { get; set; } = "0 Students";
        public Color AccentColor { get; set; } = Color.FromArgb(173, 22, 37);

        public ClassCard()
        {
            this.Size = new Size(220, 150);
            this.BackColor = Color.White;
            this.Margin = new Padding(10);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Header Background
            using (SolidBrush b = new SolidBrush(AccentColor))
            {
                g.FillRectangle(b, 0, 0, Width, 40);
            }

            // Title
            using (Font f = new Font("Segoe UI", 11, FontStyle.Bold))
            {
                g.DrawString(ClassName, f, Brushes.White, 10, 10);
            }

            // Body Content
            using (Font f = new Font("Segoe UI", 10, FontStyle.Regular))
            {
                g.DrawString(Subject, f, Brushes.Gray, 10, 60);
                g.DrawString(StudentsCount, f, Brushes.DimGray, 10, 85);
            }

            // Bottom Border
            using (Pen p = new Pen(Color.FromArgb(230, 230, 230), 1))
            {
                g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
            }

            // Action Button Mock
            using (SolidBrush b = new SolidBrush(Color.FromArgb(245, 246, 250)))
            {
                g.FillRoundedRectangle(b, 10, 110, 100, 30, 5);
            }
            using (Font f = new Font("Segoe UI", 8, FontStyle.Bold))
            {
                g.DrawString("VIEW DETAILS", f, new SolidBrush(AccentColor), 20, 118);
            }
        }
    }

    public static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, int x, int y, int width, int height, int radius)
        {
            Rectangle rect = new Rectangle(x, y, width, height);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            g.FillPath(brush, path);
        }
    }
}
