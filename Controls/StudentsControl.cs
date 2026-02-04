using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TeacherDashboard.Controls
{
    public partial class StudentsControl : UserControl
    {
        private ComboBox cmbDept, cmbYear, cmbDiv;
        private DataGridView dgvMerit, dgvRemedial;
        private DataTable dtMeritHub, dtRemedialHub;
        private Panel pnlPieChartContainer;
        private float[] pieValues = { 40, 25, 20, 15 }; // Default IT, DS, CS, BMS
        
        public StudentsControl()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            SetupCrystalClearLayout();
            RefreshPerformanceData();
        }

        private void SetupCrystalClearLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(15, 15, 15);

            // 1. INSTITUTIONAL HEADER (FIXED)
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 80, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitle = new Label() { 
                Text = "STUDENT PERFORMANCE & MERIT HUB", 
                Font = new Font("Segoe UI", 22, FontStyle.Bold), 
                ForeColor = Color.White, 
                Dock = DockStyle.Fill, 
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(30, 0, 0, 0)
            };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            // 2. MAIN SCROLLABLE AREA
            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(30, 10, 30, 30) };
            this.Controls.Add(pnlScroll);

            // 3. MASTER STACK (FlowLayoutPanel)
            FlowLayoutPanel flpMaster = new FlowLayoutPanel() { 
                Dock = DockStyle.Top, 
                FlowDirection = FlowDirection.TopDown, 
                WrapContents = false, 
                AutoSize = true,
                Width = pnlScroll.Width - 80,
                Padding = new Padding(0)
            };
            pnlScroll.Controls.Add(flpMaster);
            
            // Adjust card widths on resize to prevent button clipping
            pnlScroll.Resize += (s, e) => { 
                flpMaster.Width = pnlScroll.Width - 80; 
                foreach(Control c in flpMaster.Controls) {
                    if(c is Panel) c.Width = flpMaster.Width - 20;
                }
            };

            // --- SECTION 1: FILTER CARD ---
            Panel pnlFilterCard = CreateStyledCard(105);
            FlowLayoutPanel flpFilters = new FlowLayoutPanel() { Dock = DockStyle.Fill, WrapContents = false, Padding = new Padding(10, 15, 0, 0) };
            flpFilters.Controls.Add(CreateFilter("DEPARTMENT", out cmbDept, new string[] { "BSc IT", "BSc DS", "BSc CS", "BMS" }));
            flpFilters.Controls.Add(CreateFilter("ACADEMIC YEAR", out cmbYear, new string[] { "2024-25", "2025-26", "2026-27" }));
            flpFilters.Controls.Add(CreateFilter("DIVISION", out cmbDiv, new string[] { "All", "Div A", "Div B", "Div C" }));
            
            // Add Apply Button directly to Filter Bar
            Button btnApply = new Button() { 
                Text = "🔍 APPLY FILTERS", 
                Width = 160, 
                Height = 35,
                Margin = new Padding(0, 25, 0, 0), // Align with ComboBoxes
                BackColor = Color.FromArgb(173, 22, 37), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnApply.FlatAppearance.BorderSize = 0;
            btnApply.Click += (s, e) => RefreshPerformanceData();
            flpFilters.Controls.Add(btnApply);

            pnlFilterCard.Controls.Add(flpFilters);
            flpMaster.Controls.Add(pnlFilterCard);

            // --- SECTION 2: ANALYTICS HUB (Centered Pie Chart) ---
            Panel pnlAnalyticsCard = CreateStyledCard(450);
            pnlAnalyticsCard.Margin = new Padding(0, 20, 0, 0);
            
            pnlPieChartContainer = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(24, 25, 26), Padding = new Padding(10) };
            Label lblC = new Label() { 
                Text = "DEPARTMENT PERFORMANCE INDEX", 
                Font = new Font("Segoe UI", 12, FontStyle.Bold), 
                ForeColor = Color.Red, 
                Dock = DockStyle.Top, 
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter
            };
            Panel piePaintArea = new Panel() { Dock = DockStyle.Fill };
            piePaintArea.Paint += PieArea_Paint;
            pnlPieChartContainer.Controls.Add(piePaintArea);
            pnlPieChartContainer.Controls.Add(lblC);
            
            pnlAnalyticsCard.Controls.Add(pnlPieChartContainer);
            flpMaster.Controls.Add(pnlAnalyticsCard);

            // --- SECTION 3: MANAGEMENT HUBS ---
            // MERIT HUB
            Panel pnlMeritHub = CreateStyledCard(380);
            pnlMeritHub.Margin = new Padding(0, 20, 0, 0);
            pnlMeritHub.Controls.Add(CreateDataGridHub("🏆 MERIT HUB", out dgvMerit, Color.FromArgb(46, 204, 113)));
            flpMaster.Controls.Add(pnlMeritHub);

            // REMEDIAL HUB
            Panel pnlRemedialHub = CreateStyledCard(380);
            pnlRemedialHub.Margin = new Padding(0, 20, 0, 30);
            pnlRemedialHub.Controls.Add(CreateDataGridHub("🆘 REMEDIAL HUB", out dgvRemedial, Color.FromArgb(231, 76, 60)));
            flpMaster.Controls.Add(pnlRemedialHub);

            // Ensure Correct Order
            this.Controls.SetChildIndex(pnlHeader, 1);
            this.Controls.SetChildIndex(pnlScroll, 0);
        }

        private Panel CreateStyledCard(int height)
        {
            return new Panel() { Height = height, Width = 900, BackColor = Color.FromArgb(30, 30, 32), Padding = new Padding(15), Margin = new Padding(0, 0, 0, 25) };
        }

        private Panel CreateFilter(string title, out ComboBox cb, string[] items)
        {
            Panel p = new Panel() { Width = 220, Height = 70, Margin = new Padding(0, 0, 30, 0) };
            Label l = new Label() { Text = title, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(173, 22, 37), Dock = DockStyle.Top, Height = 25 };
            cb = new ComboBox() { 
                Dock = DockStyle.Top, DropDownStyle = ComboBoxStyle.DropDownList, Height = 35,
                BackColor = Color.FromArgb(50, 50, 55), ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11)
            };
            cb.Items.AddRange(items);
            cb.SelectedIndex = 0;
            p.Controls.Add(cb);
            p.Controls.Add(l);
            return p;
        }

        private Panel CreateKPIBox(string title, string val, Color accent, out Label valLabel)
        {
            Panel p = new Panel() { Width = 160, Height = 100, BackColor = Color.FromArgb(40, 40, 45), Margin = new Padding(0, 0, 15, 15) };
            Panel bar = new Panel() { Dock = DockStyle.Left, Width = 6, BackColor = accent };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.DarkGray, Location = new Point(15, 15), AutoSize = true };
            valLabel = new Label() { Text = val, Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 40), AutoSize = true };
            p.Controls.AddRange(new Control[] { bar, lblT, valLabel });
            return p;
        }

        private Panel CreateDataGridHub(string title, out DataGridView dgv, Color accent)
        {
            Panel p = new Panel() { Dock = DockStyle.Fill };
            
            // Header Panel for Label + Button
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 50, Padding = new Padding(0, 0, 15, 0) }; // Added right padding
            Label lbl = new Label() { Text = title, Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Left, Width = 400, TextAlign = ContentAlignment.MiddleLeft };
            
            Button btnAdd = new Button() { 
                Text = "➕ ADD STUDENT", 
                Dock = DockStyle.Right, 
                Width = 140, 
                Height = 35, 
                Margin = new Padding(0, 5, 10, 5),
                BackColor = accent, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            
            pnlHeader.Controls.Add(lbl);
            pnlHeader.Controls.Add(btnAdd);

            dgv = new DataGridView() { 
                Dock = DockStyle.Fill, BackgroundColor = Color.FromArgb(28, 28, 28), BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false, ColumnHeadersHeight = 50, AllowUserToAddRows = false,
                GridColor = Color.FromArgb(45, 45, 45), RowHeadersVisible = false, RowTemplate = { Height = 45 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, Font = new Font("Segoe UI", 10),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing,
                ColumnHeadersVisible = true,
                AutoGenerateColumns = true
            };
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(173, 22, 37); // Distinct Red Header
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            
            // Correct order for WinForms docking: add header (Top) then grid (Fill)
            // or use BringToFront/SendToBack to resolve z-order overlap.
            p.Controls.Add(dgv);
            p.Controls.Add(pnlHeader);
            pnlHeader.BringToFront(); 
            dgv.SendToBack();

            // Logic to handle Manual Add
            bool isMerit = title.Contains("MERIT");
            btnAdd.Click += (s, e) => AddStudentEntry(isMerit);

            return p;
        }

        private void AddStudentEntry(bool isMerit)
        {
            using (Form f = new Form())
            {
                f.Text = isMerit ? "Add Merit Student" : "Add Remedial Student";
                f.Size = new Size(400, 320); // Reduced height
                f.StartPosition = FormStartPosition.CenterParent;
                f.BackColor = Color.FromArgb(30, 30, 30);
                f.ForeColor = Color.White;
                f.FormBorderStyle = FormBorderStyle.FixedDialog;

                Label l1 = new Label() { Text = "Student Name:", Location = new Point(20, 20), AutoSize = true };
                TextBox t1 = new TextBox() { Location = new Point(20, 45), Width = 340, BackColor = Color.FromArgb(60,60,60), ForeColor = Color.White };
                
                Label l2 = new Label() { Text = "Core Concern:", Location = new Point(20, 80), AutoSize = true };
                TextBox t2 = new TextBox() { Location = new Point(20, 105), Width = 340, BackColor = Color.FromArgb(60,60,60), ForeColor = Color.White };

                Label l4 = new Label() { Text = "Session Timing:", Location = new Point(20, 140), AutoSize = true }; // Moved Up
                TextBox t4 = new TextBox() { Location = new Point(20, 165), Width = 340, BackColor = Color.FromArgb(60,60,60), ForeColor = Color.White }; // Moved Up

                Button btnSave = new Button() { 
                    Text = "SAVE ENTRY", 
                    Location = new Point(20, 210), // Moved Up
                    Width = 340, 
                    Height = 40, 
                    BackColor = Color.FromArgb(173, 22, 37), 
                    ForeColor = Color.White, 
                    FlatStyle = FlatStyle.Flat 
                };
                btnSave.Click += (s, e) => {
                    DataTable target = isMerit ? dtMeritHub : dtRemedialHub;
                    target.Rows.InsertAt(target.NewRow(), 0);
                    DataRow dr = target.Rows[0];
                    dr[0] = t1.Text; dr[1] = t2.Text; dr[2] = t4.Text;
                    f.Close();
                };
                f.Controls.AddRange(new Control[] { l1, t1, l2, t2, l4, t4, btnSave });
                f.ShowDialog();
            }
        }

        private void PieArea_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            // Center the Pie Chart at the top
            int chartSize = 220; // Increased size
            int centerX = (e.ClipRectangle.Width - chartSize) / 2;
            Rectangle rect = new Rectangle(centerX, 10, chartSize, chartSize);
            
            Color[] colors = { 
                Color.FromArgb(46, 204, 113), // GREEN
                Color.FromArgb(52, 152, 219), // BLUE
                Color.FromArgb(173, 22, 37),  // RED
                Color.FromArgb(241, 196, 15)   // YELLOW
            };
            string[] names = { "BSc IT", "BSc DS", "BSc CS", "BMS" };
            string[] colorLabels = { "Green", "Blue", "Red", "Yellow" };
            
            float startAngle = 0;
            int legendStartY = chartSize + 35; 
            int legendWidth = 220; 
            int legendX = (e.ClipRectangle.Width - legendWidth) / 2; // Center the legend

            for (int i = 0; i < pieValues.Length; i++)
            {
                float sweep = (pieValues[i] / 100f) * 360f;
                using (SolidBrush b = new SolidBrush(colors[i])) { g.FillPie(b, rect, startAngle, sweep); }
                
                // Legend Block
                using (SolidBrush b = new SolidBrush(colors[i])) { g.FillRectangle(b, legendX, legendStartY + (i * 35), 15, 15); }
                
                // Formatted Label: "Green: BSc IT (XX%)"
                string displayName = $"{colorLabels[i]}: {names[i]} ({pieValues[i]}%)";
                g.DrawString(displayName, new Font("Segoe UI", 10, FontStyle.Bold), Brushes.White, legendX + 25, legendStartY - 2 + (i * 35));
                
                startAngle += sweep;
            }
        }

        private void RefreshPerformanceData()
        {
            if (cmbDept == null || cmbDiv == null) return;
            
            string dept = cmbDept.SelectedItem?.ToString() ?? "BSc IT";
            string div = cmbDiv.SelectedItem?.ToString() ?? "All";

            // 🟢 Update Strategic Insights based on Dept
            if (dept == "BSc IT") pieValues = new float[] { 45, 25, 20, 10 };
            else if (dept == "BSc DS") pieValues = new float[] { 20, 50, 15, 15 };
            else if (dept == "BSc CS") pieValues = new float[] { 15, 20, 55, 10 };
            else pieValues = new float[] { 10, 15, 20, 55 };

            // 1. Re-Create Data Tables (Schema)
            dtMeritHub = new DataTable();
            dtMeritHub.Columns.Add("Student Name");
            dtMeritHub.Columns.Add("Specialization");
            dtMeritHub.Columns.Add("MERIT SESSIONS");

            dtRemedialHub = new DataTable();
            dtRemedialHub.Columns.Add("Student Name");
            dtRemedialHub.Columns.Add("Critical Subject");
            dtRemedialHub.Columns.Add("REMEDIAL SESSIONS");

            // 2. Load Mock Data
            if (dept == "BSc IT")
            {
                dtMeritHub.Rows.Add("Rahul Sharma", "Cloud Architecture", "Mon, 4:30 PM");
                dtMeritHub.Rows.Add("Sneha Patil", "Network Security", "Wed, 4:30 PM");
                dtMeritHub.Rows.Add("Amit Mishra", "Advanced Java", "Fri, 4:30 PM");

                dtRemedialHub.Rows.Add("Vikram Singh (" + div + ")", "Digital Electronics", "Tue, 2:00 PM");
                dtRemedialHub.Rows.Add("Anjali Gupta (" + div + ")", "Logic Building", "Thu, 2:00 PM");
            }
            else if (dept == "BSc DS")
            {
                dtMeritHub.Rows.Add("Priya Nair", "Machine Learning", "Tue, 5:00 PM");
                dtMeritHub.Rows.Add("Rohan Joshi", "Stats Simulation", "Thu, 5:00 PM");

                dtRemedialHub.Rows.Add("Karan Malhotra (" + div + ")", "Probability Theory", "Wed, 3:00 PM");
            }
            else
            {
                dtMeritHub.Rows.Add("Deepak Verma", "Compiler Design", "Mon, 3:30 PM");
                dtRemedialHub.Rows.Add("Sonal Desai (" + div + ")", "Discrete Maths", "Fri, 1:00 PM");
            }

            // 3. Force Manual Column Configuration for Merit Hub
            dgvMerit.DataSource = null;
            dgvMerit.AutoGenerateColumns = false;
            dgvMerit.Columns.Clear();
            
            dgvMerit.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "STUDENT NAME", DataPropertyName = "Student Name" });
            dgvMerit.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "SPECIALIZATION", DataPropertyName = "Specialization" });
            dgvMerit.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "MERIT SESSIONS", DataPropertyName = "MERIT SESSIONS" });
            
            dgvMerit.DataSource = dtMeritHub;

            // 4. Force Manual Column Configuration for Remedial Hub
            dgvRemedial.DataSource = null;
            dgvRemedial.AutoGenerateColumns = false;
            dgvRemedial.Columns.Clear();

            dgvRemedial.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "STUDENT NAME", DataPropertyName = "Student Name" });
            dgvRemedial.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "CRITICAL SUBJECT", DataPropertyName = "Critical Subject" });
            dgvRemedial.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "REMEDIAL SESSIONS", DataPropertyName = "REMEDIAL SESSIONS" });

            dgvRemedial.DataSource = dtRemedialHub;
            
            if (pnlPieChartContainer != null) pnlPieChartContainer.Invalidate(true);
        }
    }
}
