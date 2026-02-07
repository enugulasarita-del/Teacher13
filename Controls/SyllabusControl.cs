using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace TeacherDashboard.Controls
{
    public partial class SyllabusControl : UserControl
    {
        private ComboBox cmbDept;
        private ComboBox cmbDiv;
        private DateTimePicker dtpFilter;
        private DataGridView dgvSyllabus;
        private TextBox txtSearch;
        private ProgressBar pbOverall;
        private Label lblPerc;
        private Label lblStatTotal, lblStatDone, lblStatPending;

        public SyllabusControl()
        {
            InitializeComponent();
            SetupLayout();
            // Initial load
            LoadSyllabusData();
        }

        private void SetupLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.White;

            // --- 1. HEADER AREA ---
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.White };
            Label lblTitle = new Label() { 
                Text = "COURSE PROGRESS DASHBOARD", 
                Font = new Font("Segoe UI", 20, FontStyle.Bold), 
                ForeColor = Color.FromArgb(173, 22, 37), 
                AutoSize = true, 
                Location = new Point(25, 15) 
            };
            
            Panel pnlHeaderProgress = new Panel() { Dock = DockStyle.Right, Width = 300, Padding = new Padding(10, 15, 25, 10) };
            lblPerc = new Label() { Text = "Overall Progress: 0%", ForeColor = Color.FromArgb(100, 100, 100), Font = new Font("Segoe UI", 9, FontStyle.Bold), Dock = DockStyle.Top, TextAlign = ContentAlignment.TopRight };
            pbOverall = new ProgressBar() { Height = 12, Dock = DockStyle.Top, Maximum = 100, Value = 0, Style = ProgressBarStyle.Continuous };
            pnlHeaderProgress.Controls.Add(pbOverall);
            pnlHeaderProgress.Controls.Add(lblPerc);
            
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(pnlHeaderProgress);
            this.Controls.Add(pnlHeader);

            Panel pnlSep = new Panel() { Dock = DockStyle.Top, Height = 2, BackColor = Color.FromArgb(173, 22, 37) };
            this.Controls.Add(pnlSep);

            // --- MAIN CONTAINER ---
            TableLayoutPanel tlpMain = new TableLayoutPanel() { 
                Dock = DockStyle.Fill, 
                ColumnCount = 1, 
                RowCount = 3, 
                Padding = new Padding(20) 
            };
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 140f)); // Filter Row (Increased for breathing room)
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 180f)); // Stats Row (Increased for larger cards)
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));   // Grid Row
            this.Controls.Add(tlpMain);

            // --- 2. FILTER SECTION ---
            Panel pnlFilterBg = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 245, 245), Padding = new Padding(20, 15, 20, 15) };
            tlpMain.Controls.Add(pnlFilterBg, 0, 0);

            TableLayoutPanel tlpFilters = new TableLayoutPanel() { 
                Dock = DockStyle.Fill, 
                ColumnCount = 4, 
                RowCount = 2,
                BackColor = Color.Transparent
            };
            tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260f));
            tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260f));
            tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260f));
            tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320f));
            tlpFilters.RowStyles.Add(new RowStyle(SizeType.Absolute, 35f)); // Increased height for labels

            tlpFilters.Controls.Add(CreateFilterLabel("DEPARTMENT / STREAM"), 0, 0);
            tlpFilters.Controls.Add(CreateFilterLabel("CLASS DIVISION"), 1, 0);
            tlpFilters.Controls.Add(CreateFilterLabel("SESSION DATE"), 2, 0);
            tlpFilters.Controls.Add(CreateFilterLabel("SEARCH TOPICS"), 3, 0);

            cmbDept = CreateDarkComboBox(new string[] { "B.Sc IT", "B.Sc CS", "BMS", "B.Com" });
            cmbDept.Margin = new Padding(0, 0, 20, 0);
            cmbDept.SelectedIndexChanged += (s, e) => LoadSyllabusData();
            tlpFilters.Controls.Add(cmbDept, 0, 1);

            cmbDiv = CreateDarkComboBox(new string[] { "Div A", "Div B", "Div C" });
            cmbDiv.Margin = new Padding(0, 0, 20, 0);
            cmbDiv.SelectedIndexChanged += (s, e) => LoadSyllabusData();
            tlpFilters.Controls.Add(cmbDiv, 1, 1);

            dtpFilter = new DateTimePicker() { 
                Format = DateTimePickerFormat.Short, 
                BackColor = Color.White, 
                ForeColor = Color.FromArgb(40, 40, 40), 
                Width = 200, 
                Font = new Font("Segoe UI", 10),
                Margin = new Padding(5, 0, 0, 0)
            };
            dtpFilter.ValueChanged += (s, e) => LoadDataForGrid(); // Apply date filter
            tlpFilters.Controls.Add(dtpFilter, 2, 1);

            txtSearch = new TextBox() { 
                Width = 250, 
                Height = 32, 
                Font = new Font("Segoe UI", 11), 
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(40, 40, 40),
                Margin = new Padding(0, 0, 0, 0)
            };
            txtSearch.TextChanged += (s, e) => LoadDataForGrid(); // Live search
            tlpFilters.Controls.Add(txtSearch, 3, 1);

            pnlFilterBg.Controls.Add(tlpFilters);

            // --- 3. STATS SECTION ---
            FlowLayoutPanel flpStats = new FlowLayoutPanel() { 
                Dock = DockStyle.Fill, 
                Padding = new Padding(0, 5, 0, 5), 
                Margin = new Padding(0, 20, 0, 40), // Increased margins for gap
                WrapContents = false 
            };
            lblStatTotal = new Label() { Text = "0", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.FromArgb(173, 22, 37), AutoSize = true };
            lblStatDone = new Label() { Text = "0", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.FromArgb(173, 22, 37), AutoSize = true };
            lblStatPending = new Label() { Text = "0", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.FromArgb(173, 22, 37), AutoSize = true };
            
            flpStats.Controls.AddRange(new Control[] { 
                CreateStatBox("TOTAL TOPICS", lblStatTotal, Color.FromArgb(173, 22, 37)),
                CreateStatBox("COVERED TOPICS", lblStatDone, Color.FromArgb(173, 22, 37)),
                CreateStatBox("PENDING ITEMS", lblStatPending, Color.FromArgb(173, 22, 37))
            });
            tlpMain.Controls.Add(flpStats, 0, 1);

            // --- 4. GRID SECTION ---
            Panel pnlGridWrap = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(1, 40, 1, 1) }; // Added top padding for gap from stats
            dgvSyllabus = new DataGridView() { 
                Dock = DockStyle.Fill, 
                BackgroundColor = Color.White, 
                BorderStyle = BorderStyle.None,
                ForeColor = Color.FromArgb(40, 40, 40),
                GridColor = Color.FromArgb(220, 220, 220),
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = true, // Allow manual entries
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeight = 45,
                RowTemplate = { Height = 40 },
                EnableHeadersVisualStyles = false
            };
            
            dgvSyllabus.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(173, 22, 37);
            dgvSyllabus.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSyllabus.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvSyllabus.DefaultCellStyle.BackColor = Color.White;
            dgvSyllabus.DefaultCellStyle.ForeColor = Color.FromArgb(40, 40, 40);
            dgvSyllabus.DefaultCellStyle.SelectionBackColor = Color.FromArgb(173, 22, 37);
            
            // --- EDITABLE & REACTIVE LOGIC ---
            dgvSyllabus.CellValueChanged += (s, e) => {
                if (e.RowIndex >= 0) {
                    // Sync with master list for accurate stats
                    string topicName = dgvSyllabus.Rows[e.RowIndex].Cells["Topic"].Value?.ToString();
                    var masterItem = _masterSyllabus.FirstOrDefault(t => t.Topic == topicName);
                    
                    if (e.ColumnIndex == dgvSyllabus.Columns["Status"].Index) {
                        bool val = (bool)(dgvSyllabus.Rows[e.RowIndex].Cells["Status"].Value ?? false);
                        if (masterItem != null) masterItem.Done = val;
                        
                        // Auto-fill date when 'Done' is checked
                        if (val && string.IsNullOrEmpty(dgvSyllabus.Rows[e.RowIndex].Cells["Date"].Value?.ToString())) {
                            dgvSyllabus.Rows[e.RowIndex].Cells["Date"].Value = DateTime.Now.ToShortDateString();
                            if (masterItem != null) masterItem.Date = DateTime.Now.ToShortDateString();
                        }
                    }
                    
                    UpdateStatsFromMaster();
                }
            };

            // Fix for immediate checkbox response
            dgvSyllabus.CurrentCellDirtyStateChanged += (s, e) => {
                if (dgvSyllabus.IsCurrentCellDirty) {
                    dgvSyllabus.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            };
            
            SetupGridColumns();
            pnlGridWrap.Controls.Add(dgvSyllabus);
            tlpMain.Controls.Add(pnlGridWrap, 0, 2);

            // --- 5. FOOTER ACTIONS ---
            Panel pnlFooter = new Panel() { Dock = DockStyle.Bottom, Height = 60, BackColor = Color.White, Padding = new Padding(0, 10, 20, 10) };
            Button btnSave = new Button() { 
                Text = "CONFIRM UPDATES", 
                FlatStyle = FlatStyle.Flat, 
                ForeColor = Color.White, 
                BackColor = Color.FromArgb(173, 22, 37), 
                Size = new Size(180, 40), 
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Dock = DockStyle.Right 
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) => MessageBox.Show("Syllabus progress has been manually synced with the database.", "Sync Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            pnlFooter.Controls.Add(btnSave);
            this.Controls.Add(pnlFooter);
            
            pnlFooter.BringToFront();
            tlpMain.BringToFront();
        }

        private void SetupGridColumns()
        {
            dgvSyllabus.Columns.Clear();
            dgvSyllabus.Columns.Add("Subject", "SUBJECT");
            dgvSyllabus.Columns.Add("Unit", "UNIT");
            dgvSyllabus.Columns.Add("Topic", "TOPIC DESCRIPTION");
            
            DataGridViewCheckBoxColumn colDone = new DataGridViewCheckBoxColumn() { 
                Name = "Status", 
                HeaderText = "DONE", 
                Width = 70, 
                FlatStyle = FlatStyle.Flat 
            };
            dgvSyllabus.Columns.Add(colDone);
            
            dgvSyllabus.Columns.Add("Date", "DATE");
            dgvSyllabus.Columns.Add("Remarks", "TEACHER NOTES");
            
            dgvSyllabus.Columns["Subject"].FillWeight = 80;
            dgvSyllabus.Columns["Unit"].FillWeight = 50;
            dgvSyllabus.Columns["Topic"].FillWeight = 150;
            dgvSyllabus.Columns["Status"].FillWeight = 40;
            
            // Allow manual editing
            foreach (DataGridViewColumn col in dgvSyllabus.Columns) col.ReadOnly = false;
        }

        private Label CreateFilterLabel(string text)
        {
            return new Label() { 
                Text = text, 
                Font = new Font("Segoe UI", 9, FontStyle.Bold), 
                ForeColor = Color.FromArgb(173, 22, 37), 
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 8)
            };
        }

        private Panel CreateStatBox(string title, Label valLabel, Color accent)
        {
            Panel p = new Panel() { Size = new Size(260, 110), BackColor = Color.White, Margin = new Padding(0, 0, 35, 0) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 6, BackColor = accent };
            
            Label lblT = new Label() { 
                Text = title.ToUpper(), 
                Font = new Font("Segoe UI", 9, FontStyle.Bold), 
                ForeColor = Color.DarkGray, 
                Location = new Point(20, 20), 
                AutoSize = true 
            };
            
            valLabel.Location = new Point(20, 45);
            valLabel.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            valLabel.ForeColor = Color.RoyalBlue;
            valLabel.AutoSize = true;
            
            p.Controls.AddRange(new Control[] { l, lblT, valLabel });
            
            // Premium hover effect
            p.MouseEnter += (s, e) => p.BackColor = Color.FromArgb(245, 245, 245);
            p.MouseLeave += (s, e) => p.BackColor = Color.White;
            
            return p;
        }

        private Label CreateStatLabel(string name, string init, Color color)
        {
            return new Label() { Text = init, Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.RoyalBlue, AutoSize = true };
        }

        private ComboBox CreateDarkComboBox(string[] items)
        {
            ComboBox cb = new ComboBox();
            cb.Items.AddRange(items);
            cb.SelectedIndex = 0;
            cb.BackColor = Color.White;
            cb.ForeColor = Color.FromArgb(40, 40, 40);
            cb.FlatStyle = FlatStyle.Flat;
            cb.Font = new Font("Segoe UI", 10);
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.Width = 180;
            
            // Add visible border
            cb.Paint += (s, e) => {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(180, 180, 180), 1), 0, 0, cb.Width - 1, cb.Height - 1);
            };
            return cb;
        }

        // --- MANUALLY CONTROLLED MOCK DATA STORE ---
        private List<TopicData> _masterSyllabus = new List<TopicData>();

        private void LoadSyllabusData()
        {
            _masterSyllabus.Clear();
            string dept = cmbDept.SelectedItem?.ToString() ?? "B.Sc IT";
            
            if (dept == "B.Sc IT") {
                _masterSyllabus.Add(new TopicData("Core Java", "Unit 1", "Introduction to JVM", true, "01/02/2026"));
                _masterSyllabus.Add(new TopicData("Core Java", "Unit 1", "Data Types & Loops", true, "02/02/2026"));
                _masterSyllabus.Add(new TopicData("Core Java", "Unit 1", "String Handling", true, "04/02/2026"));
                _masterSyllabus.Add(new TopicData("Core Java", "Unit 2", "OOPS Concepts", false, ""));
                _masterSyllabus.Add(new TopicData("Core Java", "Unit 2", "Inheritance & Polymorphism", false, ""));
                _masterSyllabus.Add(new TopicData("SQL Server", "Unit 1", "Select Queries", true, "03/02/2026"));
                _masterSyllabus.Add(new TopicData("SQL Server", "Unit 2", "Joins & Unions", false, ""));
                _masterSyllabus.Add(new TopicData("Web Tech", "Unit 1", "HTML5 & CSS3 Basics", true, "25/01/2026"));
                _masterSyllabus.Add(new TopicData("Web Tech", "Unit 1", "Javascript Fundamentals", true, "28/01/2026"));
                _masterSyllabus.Add(new TopicData("Web Tech", "Unit 2", "Responsive Design", false, ""));
            } else if (dept == "B.Sc CS") {
                _masterSyllabus.Add(new TopicData("Algorithms", "Unit 1", "Time Complexity O(n)", true, "30/01/2026"));
                _masterSyllabus.Add(new TopicData("Algorithms", "Unit 1", "Space Complexity", true, "31/01/2026"));
                _masterSyllabus.Add(new TopicData("Algorithms", "Unit 2", "Binary Search Trees", false, ""));
                _masterSyllabus.Add(new TopicData("Algorithms", "Unit 2", "Graph Traversals", false, ""));
                _masterSyllabus.Add(new TopicData("OS", "Unit 1", "Memory Management", true, "02/02/2026"));
                _masterSyllabus.Add(new TopicData("OS", "Unit 1", "Process Scheduling", false, ""));
            } else if (dept == "BMS") {
                _masterSyllabus.Add(new TopicData("Marketing", "Unit 1", "4Ps of Marketing", true, "01/02/2026"));
                _masterSyllabus.Add(new TopicData("Marketing", "Unit 1", "Consumer Behavior", true, "03/02/2026"));
                _masterSyllabus.Add(new TopicData("HR Management", "Unit 1", "Recruitment Cycle", false, ""));
                _masterSyllabus.Add(new TopicData("HR Management", "Unit 2", "Training & Development", false, ""));
            } else if (dept == "B.Com") {
                _masterSyllabus.Add(new TopicData("Accountancy", "Unit 1", "Double Entry System", true, "28/01/2026"));
                _masterSyllabus.Add(new TopicData("Accountancy", "Unit 1", "Bank Reconciliation", true, "29/01/2026"));
                _masterSyllabus.Add(new TopicData("Auditing", "Unit 1", "Verification of Assets", false, ""));
                _masterSyllabus.Add(new TopicData("Auditing", "Unit 2", "Tax Audit Procedures", false, ""));
            }
            
            LoadDataForGrid();
        }

        private void LoadDataForGrid()
        {
            dgvSyllabus.Rows.Clear();
            string search = txtSearch.Text.ToLower();

            var filtered = _masterSyllabus.Where(t => 
                (string.IsNullOrEmpty(search) || t.Topic.ToLower().Contains(search) || t.Subject.ToLower().Contains(search))
            ).ToList();

            foreach (var t in filtered) {
                dgvSyllabus.Rows.Add(t.Subject, t.Unit, t.Topic, t.Done, t.Date, "");
            }

            // Sync stats with MASTER data for the selected department
            UpdateStatsFromMaster();
        }

        private void UpdateStatsFromMaster()
        {
            int total = _masterSyllabus.Count;
            int done = _masterSyllabus.Count(t => t.Done);
            int pending = total - done;

            lblStatTotal.Text = total.ToString();
            lblStatDone.Text = done.ToString();
            lblStatPending.Text = pending.ToString();

            int perc = total > 0 ? (done * 100 / total) : 0;
            pbOverall.Value = perc;
            lblPerc.Text = $"Course Progress: {perc}%";
            
            lblStatDone.ForeColor = done > 0 ? Color.FromArgb(173, 22, 37) : Color.FromArgb(40, 40, 40);
            lblStatPending.ForeColor = pending > 0 ? Color.FromArgb(173, 22, 37) : Color.FromArgb(40, 40, 40);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "SyllabusControl";
            this.Size = new Size(1100, 800);
            this.ResumeLayout(false);
        }

        private class TopicData
        {
            public string Subject, Unit, Topic, Date;
            public bool Done;
            public TopicData(string s, string u, string t, bool d, string dt) { Subject = s; Unit = u; Topic = t; Done = d; Date = dt; }
        }
    }
}
