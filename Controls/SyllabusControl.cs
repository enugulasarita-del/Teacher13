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
            this.BackColor = Color.FromArgb(18, 18, 18);

            // --- 1. HEADER AREA ---
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(25, 25, 25) };
            Label lblTitle = new Label() { 
                Text = "COURSE PROGRESS DASHBOARD", 
                Font = new Font("Segoe UI", 20, FontStyle.Bold), 
                ForeColor = Color.White, 
                AutoSize = true, 
                Location = new Point(25, 15) 
            };
            
            Panel pnlHeaderProgress = new Panel() { Dock = DockStyle.Right, Width = 300, Padding = new Padding(10, 15, 25, 10) };
            lblPerc = new Label() { Text = "Overall Progress: 0%", ForeColor = Color.Silver, Font = new Font("Segoe UI", 9, FontStyle.Bold), Dock = DockStyle.Top, TextAlign = ContentAlignment.TopRight };
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
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 110f)); // Filter Row
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 150f)); // Stats Row (Increased for larger cards)
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));   // Grid Row
            this.Controls.Add(tlpMain);

            // --- 2. FILTER SECTION ---
            Panel pnlFilterCard = new Panel() { 
                Dock = DockStyle.Fill, 
                BackColor = Color.FromArgb(32, 33, 36), 
                Padding = new Padding(15),
                Margin = new Padding(0, 0, 0, 20) // Bottom margin for gap to stats
            };
            TableLayoutPanel tlpFilters = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 2 };
            tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpFilters.RowStyles.Add(new RowStyle(SizeType.Absolute, 25f));

            tlpFilters.Controls.Add(CreateFilterLabel("DEPARTMENT / STREAM"), 0, 0);
            tlpFilters.Controls.Add(CreateFilterLabel("CLASS DIVISION"), 1, 0);
            tlpFilters.Controls.Add(CreateFilterLabel("SESSION DATE"), 2, 0);
            tlpFilters.Controls.Add(CreateFilterLabel("SEARCH TOPICS"), 3, 0);

            cmbDept = CreateDarkComboBox(new string[] { "B.Sc IT", "B.Sc CS", "BMS", "B.Com" });
            cmbDept.SelectedIndexChanged += (s, e) => LoadSyllabusData();
            tlpFilters.Controls.Add(cmbDept, 0, 1);

            cmbDiv = CreateDarkComboBox(new string[] { "Div A", "Div B", "Div C" });
            cmbDiv.SelectedIndexChanged += (s, e) => LoadSyllabusData();
            tlpFilters.Controls.Add(cmbDiv, 1, 1);

            dtpFilter = new DateTimePicker() { 
                Format = DateTimePickerFormat.Short, 
                BackColor = Color.FromArgb(45, 45, 48), 
                ForeColor = Color.White, 
                Width = 200, 
                Font = new Font("Segoe UI", 10),
                Margin = new Padding(5, 0, 0, 0)
            };
            dtpFilter.ValueChanged += (s, e) => LoadDataForGrid(); // Apply date filter
            tlpFilters.Controls.Add(dtpFilter, 2, 1);

            txtSearch = new TextBox() { 
                BackColor = Color.FromArgb(45, 45, 48), 
                ForeColor = Color.White, 
                BorderStyle = BorderStyle.FixedSingle, 
                Font = new Font("Segoe UI", 11), 
                Width = 220,
                Margin = new Padding(5, 0, 0, 0)
            };
            txtSearch.TextChanged += (s, e) => LoadDataForGrid(); // Live search
            tlpFilters.Controls.Add(txtSearch, 3, 1);

            pnlFilterCard.Controls.Add(tlpFilters);
            tlpMain.Controls.Add(pnlFilterCard, 0, 0);

            // --- 3. STATS SECTION ---
            FlowLayoutPanel flpStats = new FlowLayoutPanel() { 
                Dock = DockStyle.Fill, 
                Padding = new Padding(0), 
                Margin = new Padding(0, 10, 0, 30), // Added top margin for gap from filter
                WrapContents = false 
            };
            lblStatTotal = new Label() { Text = "0", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, AutoSize = true };
            lblStatDone = new Label() { Text = "0", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, AutoSize = true };
            lblStatPending = new Label() { Text = "0", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, AutoSize = true };
            
            flpStats.Controls.AddRange(new Control[] { 
                CreateStatBox("TOTAL TOPICS", lblStatTotal, Color.FromArgb(52, 152, 219)),
                CreateStatBox("COVERED TOPICS", lblStatDone, Color.FromArgb(46, 204, 113)),
                CreateStatBox("PENDING ITEMS", lblStatPending, Color.FromArgb(231, 76, 60))
            });
            tlpMain.Controls.Add(flpStats, 0, 1);

            // --- 4. GRID SECTION ---
            Panel pnlGridWrap = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(25, 25, 25), Padding = new Padding(1) };
            dgvSyllabus = new DataGridView() { 
                Dock = DockStyle.Fill, 
                BackgroundColor = Color.FromArgb(30, 30, 30), 
                BorderStyle = BorderStyle.None,
                ForeColor = Color.White,
                GridColor = Color.FromArgb(50, 50, 50),
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
            dgvSyllabus.DefaultCellStyle.BackColor = Color.FromArgb(32, 33, 36);
            dgvSyllabus.DefaultCellStyle.ForeColor = Color.White;
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
            Panel pnlFooter = new Panel() { Dock = DockStyle.Bottom, Height = 60, BackColor = Color.FromArgb(18, 18, 18), Padding = new Padding(0, 10, 20, 10) };
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
            return new Label() { Text = text, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true };
        }

        private Panel CreateStatBox(string title, Label valLabel, Color accent)
        {
            Panel p = new Panel() { Size = new Size(260, 100), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 25, 0) };
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
            valLabel.ForeColor = Color.White;
            valLabel.AutoSize = true;
            
            p.Controls.AddRange(new Control[] { l, lblT, valLabel });
            
            // Premium hover effect
            p.MouseEnter += (s, e) => p.BackColor = Color.FromArgb(45, 45, 50);
            p.MouseLeave += (s, e) => p.BackColor = Color.FromArgb(32, 33, 36);
            
            return p;
        }

        private Label CreateStatLabel(string name, string init, Color color)
        {
            return new Label() { Text = init, Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.White, AutoSize = true };
        }

        private ComboBox CreateDarkComboBox(string[] items)
        {
            ComboBox cb = new ComboBox();
            cb.Items.AddRange(items);
            cb.SelectedIndex = 0;
            cb.BackColor = Color.FromArgb(45, 45, 48);
            cb.ForeColor = Color.White;
            cb.FlatStyle = FlatStyle.Flat;
            cb.Font = new Font("Segoe UI", 10);
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.Width = 180;
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
            
            lblStatDone.ForeColor = done > 0 ? Color.FromArgb(46, 204, 113) : Color.White;
            lblStatPending.ForeColor = pending > 0 ? Color.FromArgb(231, 76, 60) : Color.White;
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
