using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TeacherDashboard
{
    /// <summary>
    /// Modern UI Helper Components for Enhanced Visual Design
    /// Provides reusable card panels, buttons, and styling utilities
    /// </summary>
    public static class ModernUI
    {
        // Color Palette
        public static readonly Color PrimaryRed = Color.FromArgb(173, 22, 37);
        public static readonly Color DarkRed = Color.FromArgb(140, 20, 30);
        public static readonly Color LightRed = Color.FromArgb(195, 40, 55);
        public static readonly Color CardBackground = Color.White;
        public static readonly Color BorderColor = Color.FromArgb(230, 230, 230);
        public static readonly Color TextPrimary = Color.FromArgb(40, 40, 40);
        public static readonly Color TextSecondary = Color.FromArgb(120, 120, 120);

        /// <summary>
        /// Creates a modern card panel with shadow effect
        /// </summary>
        public static Panel CreateCard(int width = 300, int height = 200, int margin = 10)
        {
            Panel card = new Panel
            {
                Width = width,
                Height = height,
                BackColor = CardBackground,
                Margin = new Padding(margin),
                Padding = new Padding(20)
            };

            // Add shadow effect via Paint event
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                
                // Draw subtle shadow
                using (GraphicsPath path = CreateRoundedRectangle(0, 0, card.Width - 1, card.Height - 1, 8))
                {
                    // Shadow
                    using (Pen shadowPen = new Pen(Color.FromArgb(30, 0, 0, 0), 3))
                    {
                        e.Graphics.DrawPath(shadowPen, path);
                    }
                    
                    // Border
                    using (Pen borderPen = new Pen(BorderColor, 1))
                    {
                        e.Graphics.DrawPath(borderPen, path);
                    }
                }
            };

            return card;
        }

        /// <summary>
        /// Creates a modern button with hover effects
        /// </summary>
        public static Button CreateModernButton(string text, Color bgColor, Color fgColor)
        {
            Button btn = new Button
            {
                Text = text,
                BackColor = bgColor,
                ForeColor = fgColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Height = 40,
                Width = 150
            };

            btn.FlatAppearance.BorderSize = 0;

            // Hover effects
            Color originalBg = bgColor;
            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = ControlPaint.Light(originalBg, 0.1f);
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = originalBg;
            };

            return btn;
        }

        /// <summary>
        /// Creates a stat card with icon, value, and label
        /// </summary>
        public static Panel CreateStatCard(string icon, string value, string label, Color accentColor)
        {
            Panel card = CreateCard(250, 120, 10);

            // Accent bar
            Panel accent = new Panel
            {
                Dock = DockStyle.Left,
                Width = 5,
                BackColor = accentColor
            };

            // Icon label
            Label lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 24F),
                ForeColor = accentColor,
                Location = new Point(25, 20),
                AutoSize = true
            };

            // Value label
            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(25, 50),
                AutoSize = true
            };

            // Description label
            Label lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSecondary,
                Location = new Point(25, 85),
                AutoSize = true
            };

            card.Controls.AddRange(new Control[] { accent, lblIcon, lblValue, lblLabel });

            // Hover effect
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(248, 248, 248);
            card.MouseLeave += (s, e) => card.BackColor = CardBackground;

            return card;
        }

        /// <summary>
        /// Helper method to create rounded rectangle path
        /// </summary>
        private static GraphicsPath CreateRoundedRectangle(int x, int y, int width, int height, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, radius, radius, 180, 90);
            path.AddArc(x + width - radius, y, radius, radius, 270, 90);
            path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
            path.AddArc(x, y + height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Applies modern styling to a DataGridView
        /// </summary>
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = Color.White;
            dgv.GridColor = BorderColor;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Header styling
            dgv.ColumnHeadersDefaultCellStyle.BackColor = PrimaryRed;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10);
            dgv.ColumnHeadersHeight = 45;

            // Cell styling
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 230, 230);
            dgv.DefaultCellStyle.SelectionForeColor = PrimaryRed;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgv.DefaultCellStyle.Padding = new Padding(8);
            dgv.RowTemplate.Height = 45;

            // Alternating row colors
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
        }

        /// <summary>
        /// Creates a section header label
        /// </summary>
        public static Label CreateSectionHeader(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = PrimaryRed,
                AutoSize = true,
                Margin = new Padding(0, 20, 0, 10)
            };
        }

        /// <summary>
        /// Creates a divider line
        /// </summary>
        public static Panel CreateDivider(int height = 1)
        {
            return new Panel
            {
                Height = height,
                Dock = DockStyle.Top,
                BackColor = BorderColor,
                Margin = new Padding(0, 10, 0, 10)
            };
        }
    }

    /// <summary>
    /// Custom Panel with Rounded Corners
    /// </summary>
    public class RoundedPanel : Panel
    {
        public int BorderRadius { get; set; } = 8;
        public Color BorderColor { get; set; } = Color.FromArgb(230, 230, 230);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = CreateRoundedRectangle(0, 0, Width - 1, Height - 1, BorderRadius))
            {
                // Fill background
                using (SolidBrush brush = new SolidBrush(BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // Draw border
                using (Pen pen = new Pen(BorderColor, 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath CreateRoundedRectangle(int x, int y, int width, int height, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, radius, radius, 180, 90);
            path.AddArc(x + width - radius, y, radius, radius, 270, 90);
            path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
            path.AddArc(x, y + height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
