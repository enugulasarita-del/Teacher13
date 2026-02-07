using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace TeacherDashboard.Controls
{
    public partial class NoticesControl : UserControl
    {
        // Theme Colors
        private Color primaryColor = Color.FromArgb(173, 22, 37); // VSIT Red
        private Color bgColor = Color.White;
        private Color cardBg = Color.White;
        private Color textColor = Color.FromArgb(40, 40, 40);
        private Color placeholderColor = Color.Gray;
        private Color borderColor = Color.FromArgb(220, 220, 220);

        public NoticesControl()
        {
            InitializeComponent();
            SetupLayout();
        }

        private void SetupLayout()
        {
            this.Controls.Clear();
            this.BackColor = bgColor;
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 11);

            // 1. Fixed Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.White };
            Label lblHeaderTitle = new Label() { 
                Text = "📢  POST NOTICES", 
                Font = new Font("Segoe UI", 18, FontStyle.Bold), 
                ForeColor = primaryColor, 
                AutoSize = true, 
                Location = new Point(30, 18) 
            };
            pnlHeader.Controls.Add(lblHeaderTitle);
            Panel pnlAccent = new Panel() { Dock = DockStyle.Bottom, Height = 3, BackColor = primaryColor };
            pnlHeader.Controls.Add(pnlAccent);
            this.Controls.Add(pnlHeader);

            // 2. Scrollable Container
            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(0) };
            this.Controls.Add(pnlScroll);

            // Flow Layout for vertical stacking with proper spacing
            FlowLayoutPanel flpForm = new FlowLayoutPanel() { 
                Dock = DockStyle.Top, 
                FlowDirection = FlowDirection.TopDown, 
                WrapContents = false, 
                AutoSize = true, 
                Padding = new Padding(50, 20, 50, 50),
                BackColor = Color.Transparent
            };
            pnlScroll.Controls.Add(flpForm);

            // --- FORM ROWS (One by One from Top to Bottom) ---

            // A. Title
            flpForm.Controls.Add(CreateLabel("NOTICE TITLE"));
            flpForm.Controls.Add(CreateInputGroup("T", "Enter title of notice here...", 55));

            // B. Category & Priority (Side by Side)
            flpForm.Controls.Add(new Panel() { Height = 20 }); // Spacer
            TableLayoutPanel tlpRow1 = new TableLayoutPanel() { Width = 900, Height = 85, ColumnCount = 2, Margin = new Padding(0) };
            tlpRow1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tlpRow1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            AddDropdown(tlpRow1, "Category", new string[] { "General", "Academic", "Exam", "Event" }, 0);
            AddDropdown(tlpRow1, "Priority", new string[] { "Medium", "High", "Low" }, 1);
            flpForm.Controls.Add(tlpRow1);

            // C. Target Audience Section
            flpForm.Controls.Add(new Panel() { Height = 20 }); // Spacer
            flpForm.Controls.Add(CreateLabel("TARGET AUDIENCE (OPTIONAL)"));
            
            TableLayoutPanel tlpRow2 = new TableLayoutPanel() { Width = 900, Height = 85, ColumnCount = 2, Margin = new Padding(0) };
            tlpRow2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tlpRow2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            AddDropdown(tlpRow2, "Department", new string[] { "All Departments", "B.Sc IT", "B.Sc CS", "BMS", "B.Com" }, 0);
            AddDropdown(tlpRow2, "Year / Semester", new string[] { "All Years", "FY", "SY", "TY" }, 1);
            flpForm.Controls.Add(tlpRow2);

            // D. Notice Content
            flpForm.Controls.Add(new Panel() { Height = 20 }); // Spacer
            flpForm.Controls.Add(CreateLabel("NOTICE CONTENT / MESSAGE"));
            Panel pnlContentWrap = new Panel() { Size = new Size(900, 200), BackColor = cardBg, Padding = new Padding(15) };
            pnlContentWrap.Paint += (s, e) => DrawBorder(e.Graphics, pnlContentWrap.ClientRectangle, borderColor);
            TextBox txtContent = new TextBox() { 
                Text = "", 
                Multiline = true, 
                Dock = DockStyle.Fill, 
                BorderStyle = BorderStyle.None, 
                BackColor = cardBg, 
                ForeColor = textColor, 
                Font = new Font("Segoe UI", 11) 
            };
            pnlContentWrap.Controls.Add(txtContent);
            flpForm.Controls.Add(pnlContentWrap);

            // E. Link (Optional)
            flpForm.Controls.Add(new Panel() { Height = 20 }); // Spacer
            flpForm.Controls.Add(CreateLabel("REDIRECT LINK (OPTIONAL)"));
            flpForm.Controls.Add(CreateInputGroup("🔗", "https://example.com/more-info", 55));

            // F. Attachment Section
            flpForm.Controls.Add(new Panel() { Height = 20 }); // Spacer
            flpForm.Controls.Add(CreateLabel("ATTACHMENT"));
            Panel pnlAttach = new Panel() { Size = new Size(900, 100), BackColor = cardBg, Padding = new Padding(20) };
            pnlAttach.Paint += (s, e) => DrawBorder(e.Graphics, pnlAttach.ClientRectangle, borderColor);
            Button btnSelect = new Button() { 
                Text = "📎 Select File (PDF, Image)", 
                Dock = DockStyle.Fill, 
                FlatStyle = FlatStyle.Flat, 
                ForeColor = primaryColor, 
                Font = new Font("Segoe UI Semibold", 10),
                Cursor = Cursors.Hand
            };
            btnSelect.FlatAppearance.BorderColor = primaryColor;
            pnlAttach.Controls.Add(btnSelect);
            flpForm.Controls.Add(pnlAttach);

            // G. Final Post Button
            flpForm.Controls.Add(new Panel() { Height = 40 }); // Large Spacer
            Button btnPost = new Button() { 
                Text = "➤ POST NOTICE TO STUDENTS", 
                Size = new Size(900, 60), 
                BackColor = primaryColor, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnPost.FlatAppearance.BorderSize = 0;
            flpForm.Controls.Add(btnPost);

            // Ensure the form fills width on resize
            pnlScroll.Resize += (s, e) => {
                int targetWidth = pnlScroll.Width - 100;
                flpForm.Width = pnlScroll.Width;
                foreach (Control c in flpForm.Controls) {
                    if (c is Panel || c is TableLayoutPanel || c is Button) {
                        c.Width = targetWidth;
                    }
                }
            };
        }

        private Label CreateLabel(string text)
        {
            return new Label() { 
                Text = text, 
                Font = new Font("Segoe UI", 9, FontStyle.Bold), 
                ForeColor = primaryColor, 
                AutoSize = true, 
                Margin = new Padding(0, 10, 0, 5) 
            };
        }

        private Panel CreateInputGroup(string icon, string placeholder, int height)
        {
            Panel p = new Panel() { Size = new Size(900, height), BackColor = cardBg };
            p.Paint += (s, e) => DrawBorder(e.Graphics, p.ClientRectangle, borderColor);
            
            Label lblIcon = new Label() { 
                Text = icon, 
                Font = new Font("Segoe UI", 16), 
                ForeColor = primaryColor, 
                Location = new Point(15, (height-30)/2), 
                AutoSize = true,
                BackColor = Color.Transparent
            };
            
            TextBox txt = new TextBox() { 
                Text = "", 
                BorderStyle = BorderStyle.None, 
                Font = new Font("Segoe UI", 11), 
                BackColor = cardBg,
                ForeColor = textColor, 
                Location = new Point(60, (height-25)/2), 
                Width = 800,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            
            p.Controls.AddRange(new Control[] { lblIcon, txt });
            return p;
        }

        private void AddDropdown(TableLayoutPanel parent, string labelText, string[] items, int col)
        {
            Panel container = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(0, 0, 20, 0) };
            Label lbl = new Label() { Text = labelText, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = primaryColor, Location = new Point(5, 0), AutoSize = true };
            
            Panel box = new Panel() { Location = new Point(0, 22), Height = 50, BackColor = cardBg };
            box.Paint += (s, e) => DrawBorder(e.Graphics, box.ClientRectangle, borderColor);
            
            ComboBox cb = new ComboBox() { 
                Location = new Point(10, 12), 
                FlatStyle = FlatStyle.Flat, 
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11),
                BackColor = cardBg,
                ForeColor = textColor,
                Width = 350 // Will be resized
            };
            cb.Items.AddRange(items);
            cb.SelectedIndex = 0;

            box.Controls.Add(cb);
            container.Controls.AddRange(new Control[] { lbl, box });
            parent.Controls.Add(container, col, 0);

            parent.Resize += (s, e) => {
                box.Width = container.Width - 25;
                cb.Width = box.Width - 25;
            };
        }

        private void DrawBorder(Graphics g, Rectangle r, Color color)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen pen = new Pen(color, 1))
            {
                g.DrawRectangle(pen, r.X, r.Y, r.Width - 1, r.Height - 1);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "NoticesControl";
            this.Size = new Size(1100, 900);
            this.ResumeLayout(false);
        }
    }
}
