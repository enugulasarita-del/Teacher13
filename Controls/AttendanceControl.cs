using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TeacherDashboard.Controls
{
    public partial class AttendanceControl : UserControl
    {
        private Label lblCurrentClassDisplay;
        private ComboBox cmbDept;
        private ComboBox cmbDiv;
        private ListBox lstMonthlyDef;
        private ListBox lstMerit;
        private Label lblDefTitle;
        private Label lblMeritTitle;
        private Panel pnlPie;
        private Panel pnlLegend;
        private float regularPercent = 75;
        private float defaulterPercent = 25;

        public AttendanceControl()
        {
            InitializeComponent();
            SetupStrictLayout();
            UpdateClassMapping(); // Initialize classes
            LoadMockAttendance();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37), Padding = new Padding(20, 0, 20, 0) };
            Label lblMainTitle = new Label() { Text = "ATTENDANCE TRACKER", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 18) };
            
            this.lblCurrentClassDisplay = new Label() { 
                Name = "lblCurrentClassDisplay",
                Text = "MARKING: SELECT FILTERS", 
                Font = new Font("Segoe UI", 11, FontStyle.Bold), 
                ForeColor = Color.FromArgb(241, 196, 15), 
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false,
                Width = 450,
                Padding = new Padding(0, 0, 25, 0)
            };
            
            pnlHeader.Controls.AddRange(new Control[] { lblCurrentClassDisplay, lblMainTitle });
            this.Controls.Add(pnlHeader);

            // 2. Advanced Action Bar (Filters)
            Panel pnlActions = new Panel() { Dock = DockStyle.Top, Height = 100, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(20, 10, 20, 10) };
            
            FlowLayoutPanel flpFilters = new FlowLayoutPanel() { Dock = DockStyle.Fill, WrapContents = true };
            
            // Filter Groups
            flpFilters.Controls.Add(CreateFilterGroup("DATE", new DateTimePicker() { Width = 130 }));
            
            ComboBox cmbMonth = CreateStyledComboBox(new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" }, 110, Point.Empty);
            flpFilters.Controls.Add(CreateFilterGroup("MONTH", cmbMonth));

            ComboBox cmbYear = CreateStyledComboBox(new string[] { "2024", "2025", "2026" }, 70, Point.Empty);
            flpFilters.Controls.Add(CreateFilterGroup("YEAR", cmbYear));

            cmbDept = CreateStyledComboBox(new string[] { "B.Sc IT", "B.Sc CS", "BMS", "B.Com" }, 110, Point.Empty);
            flpFilters.Controls.Add(CreateFilterGroup("DEPT", cmbDept));
            
            cmbClass = CreateStyledComboBox(new string[] { "FY", "SY", "TY" }, 80, Point.Empty);
            flpFilters.Controls.Add(CreateFilterGroup("CLASS", cmbClass));
            
            cmbDiv = CreateStyledComboBox(new string[] { "Div A", "Div B", "Div C" }, 80, Point.Empty);
            flpFilters.Controls.Add(CreateFilterGroup("DIV", cmbDiv));

            Button btnFilter = new Button() { Text = "🔍 APPLY FILTER", Size = new Size(130, 32), BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 8, FontStyle.Bold), Margin = new Padding(10, 5, 0, 0) };
            btnFilter.FlatAppearance.BorderSize = 0;
            flpFilters.Controls.Add(btnFilter);

            Button btnSave = new Button() { Text = "✔ SAVE ATTENDANCE", Size = new Size(150, 32), BackColor = Color.FromArgb(173, 22, 37), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 8, FontStyle.Bold), Margin = new Padding(10, 5, 0, 0) };
            btnSave.FlatAppearance.BorderSize = 0;
            flpFilters.Controls.Add(btnSave);

            pnlActions.Controls.Add(flpFilters);
            this.Controls.Add(pnlActions);
            
            // Interaction Logic
            cmbDept.SelectedIndexChanged += (s, e) => { UpdateClassMapping(); UpdateHeaderLabel(); };
            cmbClass.SelectedIndexChanged += (s, e) => UpdateHeaderLabel();
            cmbDiv.SelectedIndexChanged += (s, e) => UpdateHeaderLabel();
            btnFilter.Click += (s, e) => LoadMockAttendance();

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(0, 0, 0, 20) };
            this.Controls.Add(pnlScroll);

            // 3. Grid (Primary)
            this.dgvAttendance = new DataGridView() { 
                Dock = DockStyle.Top, 
                Height = 350,
                BackgroundColor = Color.FromArgb(28, 28, 28), 
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 40,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(45, 45, 45),
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false
            };
            this.dgvAttendance.DefaultCellStyle.BackColor = Color.White;
            this.dgvAttendance.DefaultCellStyle.ForeColor = Color.Black;
            this.dgvAttendance.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            this.dgvAttendance.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            pnlScroll.Controls.Add(this.dgvAttendance);

            // 4. BOTTOM SECTION: Analytics Dashboard
            TableLayoutPanel tlpBottom = new TableLayoutPanel() { 
                Dock = DockStyle.Top, 
                Height = 350, 
                ColumnCount = 4, 
                RowCount = 2,
                BackColor = Color.FromArgb(18, 18, 18),
                Padding = new Padding(20)
            };
            tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 270)); // Pie
            tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180)); // Legend
            tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));   // Defaulters List
            tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));   // Merit List
            
            pnlScroll.Controls.Add(tlpBottom);

            Label lblPerfTitle = new Label() { Text = "CLASS ATTENDANCE PERFORMANCE", Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Margin = new Padding(0, 0, 0, 10) };
            tlpBottom.Controls.Add(lblPerfTitle, 0, 0);
            tlpBottom.SetColumnSpan(lblPerfTitle, 2);

            // Pie Chart Panel
            this.pnlPie = new Panel() { Size = new Size(240, 240), BackColor = Color.FromArgb(18, 18, 18), Margin = new Padding(0) };
            this.pnlPie.Paint += PnlPie_Paint;
            tlpBottom.Controls.Add(this.pnlPie, 0, 1);

            // Legend Panel
            this.pnlLegend = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(18, 18, 18), Margin = new Padding(10, 0, 0, 0) };
            UpdateLegend();
            tlpBottom.Controls.Add(this.pnlLegend, 1, 1);

            // Defaulters List Panel
            Panel pnlDefaultersContainer = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(15), Margin = new Padding(15, 0, 0, 0) };
            lblDefTitle = new Label() { Text = "MONTHLY DEFAULTERS", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(231, 76, 60), Dock = DockStyle.Top, Height = 35 };
            lstMonthlyDef = new ListBox() { 
                Dock = DockStyle.Fill, 
                BackColor = Color.FromArgb(32, 33, 36), 
                ForeColor = Color.FromArgb(200, 200, 200), 
                BorderStyle = BorderStyle.None, 
                Font = new Font("Segoe UI", 10),
                ItemHeight = 28
            };
            pnlDefaultersContainer.Controls.AddRange(new Control[] { lstMonthlyDef, lblDefTitle });
            tlpBottom.Controls.Add(pnlDefaultersContainer, 2, 0);
            tlpBottom.SetRowSpan(pnlDefaultersContainer, 2);

            // Merit List Panel
            Panel pnlMeritContainer = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(15), Margin = new Padding(15, 0, 0, 0) };
            lblMeritTitle = new Label() { Text = "ATTENDANCE MERIT LIST", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(46, 204, 113), Dock = DockStyle.Top, Height = 35 };
            lstMerit = new ListBox() { 
                Dock = DockStyle.Fill, 
                BackColor = Color.FromArgb(32, 33, 36), 
                ForeColor = Color.FromArgb(200, 200, 200), 
                BorderStyle = BorderStyle.None, 
                Font = new Font("Segoe UI", 10),
                ItemHeight = 28
            };
            pnlMeritContainer.Controls.AddRange(new Control[] { lstMerit, lblMeritTitle });
            tlpBottom.Controls.Add(pnlMeritContainer, 3, 0);
            tlpBottom.SetRowSpan(pnlMeritContainer, 2);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 2);    // Top
            this.Controls.SetChildIndex(pnlActions, 1);   // Second Top
            this.Controls.SetChildIndex(pnlScroll, 0);    // Fill
        }

        private void UpdateClassMapping()
        {
            string dept = cmbDept.SelectedItem?.ToString();
            cmbClass.Items.Clear();
            if (dept == "B.Sc IT" || dept == "B.Sc CS")
                cmbClass.Items.AddRange(new string[] { "FY", "SY", "TY" });
            else
                cmbClass.Items.AddRange(new string[] { "FY", "SY" }); // Diploma/Others usually 2 years in this mock
            cmbClass.SelectedIndex = 0;
        }

        private void UpdateHeaderLabel()
        {
            if (lblCurrentClassDisplay != null)
            {
                lblCurrentClassDisplay.Text = $"MARKING: {cmbDept.SelectedItem} | {cmbClass.SelectedItem} | {cmbDiv.SelectedItem}";
            }
        }

        private void UpdateLegend()
        {
            if (pnlLegend == null) return;
            pnlLegend.Controls.Clear();
            AddLegendItem(pnlLegend, "Regular (>= 75%)", Color.FromArgb(46, 204, 113), 0);
            AddLegendItem(pnlLegend, "Defaulters (< 75%)", Color.FromArgb(231, 76, 60), 1);
        }

        private void AddLegendItem(Panel parent, string text, Color color, int index)
        {
            Panel pAccent = new Panel() { Size = new Size(15, 15), Location = new Point(0, 10 + (index * 30)), BackColor = color };
            Label lbl = new Label() { Text = text, ForeColor = Color.White, Font = new Font("Segoe UI", 9), Location = new Point(25, 8 + (index * 30)), AutoSize = true };
            parent.Controls.AddRange(new Control[] { pAccent, lbl });
        }

        private void PnlPie_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            float[] values = { regularPercent, defaulterPercent }; 
            Color[] colors = { Color.FromArgb(46, 204, 113), Color.FromArgb(231, 76, 60) };
            
            float total = regularPercent + defaulterPercent;
            if (total == 0) return;

            float startAngle = 0;
            Rectangle rect = new Rectangle(10, 10, 220, 220);
            
            for (int i = 0; i < values.Length; i++)
            {
                float sweepAngle = (values[i] / total) * 360;
                using (SolidBrush b = new SolidBrush(colors[i]))
                {
                    g.FillPie(b, rect, startAngle, sweepAngle);
                }
                startAngle += sweepAngle;
            }

            // Draw Inner Circle for Donut Effect
            using (SolidBrush b = new SolidBrush(Color.FromArgb(18, 18, 18)))
            {
                g.FillEllipse(b, 60, 60, 120, 120);
            }

            string avgText = regularPercent.ToString("0") + "%";
            g.DrawString(avgText, new Font("Segoe UI", 18, FontStyle.Bold), Brushes.White, 85, 95);
            g.DrawString("AVG REG.", new Font("Segoe UI", 8, FontStyle.Bold), Brushes.Gray, 92, 135);
        }

        private Panel CreateFilterGroup(string label, Control input)
        {
            Panel p = new Panel() { Width = input.Width, Height = 55, Margin = new Padding(0, 0, 15, 0) };
            Label l = new Label() { Text = label, Font = new Font("Segoe UI", 7, FontStyle.Bold), ForeColor = Color.DarkGray, Dock = DockStyle.Top, Height = 20 };
            input.Dock = DockStyle.Top;
            input.Font = new Font("Segoe UI", 10);
            p.Controls.Add(input);
            p.Controls.Add(l);
            return p;
        }

        private ComboBox CreateStyledComboBox(string[] items, int width, Point location)
        {
            ComboBox cmb = new ComboBox() { 
                Width = width, 
                BackColor = Color.FromArgb(45, 45, 48), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmb.Items.AddRange(items);
            cmb.SelectedIndex = 0;
            return cmb;
        }

        private Panel CreateDefaulterList(string dept, string[] list, Color accent)
        {
            Panel p = new Panel() { Size = new Size(240, 180), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 15, 15) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 4, BackColor = accent };
            Label lblT = new Label() { Text = dept.ToUpper(), Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = accent, Location = new Point(15, 15), AutoSize = true };
            
            int y = 45;
            foreach (var name in list)
            {
                Label lblStudent = new Label() { Text = "• " + name, Font = new Font("Segoe UI", 9), ForeColor = Color.LightGray, Location = new Point(15, y), Size = new Size(210, 20) };
                p.Controls.Add(lblStudent);
                y += 25;
            }
            
            p.Controls.AddRange(new Control[] { l, lblT });
            return p;
        }

        private Panel CreateInsightBox(string title, string val, Color accent)
        {
            Panel p = new Panel() { Size = new Size(200, 100), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(15, 20), AutoSize = true };
            Label lblV = new Label() { Text = val, Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 45), AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lblT, lblV });
            return p;
        }

        private void LoadMockAttendance()
        {
            if (cmbDept.SelectedItem == null || cmbClass.SelectedItem == null || cmbDiv.SelectedItem == null) return;

            string dept = cmbDept.SelectedItem.ToString();
            string cls = cmbClass.SelectedItem.ToString();
            string div = cmbDiv.SelectedItem.ToString();
            string context = $"{dept} {cls} {div}";

            lblDefTitle.Text = $"MONTHLY DEFAULTERS ({context})";
            lblMeritTitle.Text = $"MERIT LIST (>= 90%) ({context})";
            
            // 1. Update Pie Chart Statistics based on Dept/Div
            if (dept == "B.Sc IT") { defaulterPercent = 12; regularPercent = 88; }
            else if (dept == "B.Sc CS") { defaulterPercent = 18; regularPercent = 82; }
            else { defaulterPercent = 25; regularPercent = 75; }
            
            if (div == "Div C") { defaulterPercent += 10; regularPercent -= 10; } // Div C usually more defaulters in mock data

            this.pnlPie?.Invalidate(); 

            // 2. Name Pool & Deterministic Mock Data
            string[] namesIT = { "Arjun Rao", "Neha Verma", "Vikrant Singh", "Pooja Hegde", "Kabir Khan", "Isha Deshmukh", "Sameer Naik" };
            string[] namesCS = { "Siddharth Malra", "Riya Sen", "Armaan Jain", "Tara Sutaria", "Varun Dhawan", "Kriti Sanon" };
            string[] activePool = (dept == "B.Sc IT") ? namesIT : namesCS;

            // 3. Populate Defaulter List
            lstMonthlyDef.Items.Clear();
            lstMonthlyDef.Items.Add($"• {activePool[0]} - 62%");
            lstMonthlyDef.Items.Add($"• {activePool[1]} - 68%");

            // 4. Populate Merit List
            lstMerit.Items.Clear();
            lstMerit.Items.Add($"⭐ {activePool[activePool.Length - 1]} - 98%");
            lstMerit.Items.Add($"⭐ {activePool[activePool.Length - 2]} - 94%");

            // 5. Populate Attendance Grid
            DataTable dt = new DataTable();
            dt.Columns.Add("Roll No");
            dt.Columns.Add("Student Name");
            dt.Columns.Add("Attendance Status", typeof(bool));

            for (int i = 0; i < activePool.Length; i++)
            {
                bool isPresent = (i < activePool.Length - 2); // Mostly present
                dt.Rows.Add((i + 1).ToString(), activePool[i], isPresent);
            }

            if (this.dgvAttendance != null)
            {
                this.dgvAttendance.DataSource = dt;
                this.dgvAttendance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                
                if(dgvAttendance.Columns.Count > 2 && dgvAttendance.Columns[2] is DataGridViewCheckBoxColumn)
                {
                    dgvAttendance.Columns[2].HeaderText = "MARK PRESENT";
                }
            }
        }
    }
}
