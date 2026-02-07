using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Drawing.Drawing2D;

namespace TeacherDashboard.Controls
{
    public partial class AssignmentsControl : UserControl
    {
        // Theme Colors
        private Color primaryColor = Color.FromArgb(173, 22, 37); // VSIT Red
        private Color bgColor = Color.White;
        private Color cardBg = Color.White;
        private Color borderColor = Color.FromArgb(220, 220, 220);

        // UI Components
        private ComboBox cmbDept, cmbDiv, cmbGradingDept, cmbGradingDiv, cmbSelectAssignment;
        private TextBox txtTopic, txtMaxMarks;
        private DateTimePicker dtpDue;
        private DataGridView dgvAssignments, dgvStudentGrading;
        private DataTable dtAssignments, dtStudentGrading;
        private Label lblTotalAss, lblPendingGrading, lblAvgMarks;

        // Student Data Pool
        private List<StudentEntry> allStudents = new List<StudentEntry>();

        public AssignmentsControl()
        {
            InitializeComponent();
            InitializeDataPool();
            SetupDataStructures();
            SetupStrictLayout();
            SetupGridEvents();
            UpdateStats(); // Initial trigger
        }

        private void InitializeDataPool()
        {
            string[] deptList = { "B.Sc IT", "B.Sc CS", "BMS" };
            string[] divList = { "Div A", "Div B", "Div C" };
            string[] names = { "Rahul Enugula", "Amit Sharma", "Priya Kapur", "Sneha Rao", "Vikram Deshmukh", "Deepak Mishra", "Anjali Gupta", "Karan Verma", "Suresh Nair", "Megha Patil" };
            Random r = new Random();

            foreach (var dept in deptList)
            {
                foreach (var div in divList)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        var s = new StudentEntry { 
                            RollNo = (allStudents.Count + 101).ToString(),
                            Name = names[(allStudents.Count) % names.Length],
                            Dept = dept,
                            Div = div
                        };
                        
                        // Simulate Pre-filled Data (Dummy Data Logic)
                        // AS-101 (Introduction to OOP) - Mostly Graded (80% chance)
                        if (r.Next(100) < 80) s.Marks["AS-101"] = r.Next(12, 20).ToString();
                        
                        // AS-102 (SQL Joins) - Partially Graded (40% chance)
                        if (r.Next(100) < 40) s.Marks["AS-102"] = r.Next(30, 48).ToString();

                        allStudents.Add(s);
                    }
                }
            }
        }

        private void SetupDataStructures()
        {
            dtAssignments = new DataTable();
            dtAssignments.Columns.Add("ID");
            dtAssignments.Columns.Add("Topic");
            dtAssignments.Columns.Add("Dept/Div");
            dtAssignments.Columns.Add("Due Date");
            dtAssignments.Columns.Add("Max Marks");
            dtAssignments.Columns.Add("Status");

            // AS-101 and AS-102 match the keys used in InitializeDataPool
            dtAssignments.Rows.Add("AS-101", "Introduction to OOP", "B.Sc IT / Div A", "05/02/2026", "20", "Active");
            dtAssignments.Rows.Add("AS-102", "SQL Joins & Queries", "B.Sc CS / Div B", "08/02/2026", "50", "Active");

            dtStudentGrading = new DataTable();
            dtStudentGrading.Columns.Add("Roll No");
            dtStudentGrading.Columns.Add("Student Name");
            dtStudentGrading.Columns.Add("Assignment");
            dtStudentGrading.Columns.Add("Marks Obtained");
            dtStudentGrading.Columns.Add("Max Marks");
            dtStudentGrading.Columns.Add("Status");
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = bgColor;
            this.Dock = DockStyle.Fill;

            // --- ROOT LAYOUT (Prevents Overlap) ---
            TableLayoutPanel rootLayout = new TableLayoutPanel();
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F)); // Fixed Header Height
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Scrollable Content
            rootLayout.Padding = new Padding(0);
            rootLayout.Margin = new Padding(0);
            this.Controls.Add(rootLayout);

            // 1. FIXED HEADER
            Panel pnlHeader = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0) };
            Label lblTitle = new Label() { Text = "📝  ASSIGNMENT & MARKS MANAGEMENT", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = primaryColor, Location = new Point(30, 25), AutoSize = true };
            pnlHeader.Controls.Add(lblTitle);
            Panel pnlAccent = new Panel() { Dock = DockStyle.Bottom, Height = 3, BackColor = primaryColor };
            pnlHeader.Controls.Add(pnlAccent);
            
            rootLayout.Controls.Add(pnlHeader, 0, 0);

            // 2. SCROLLABLE CONTENT BODY
            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, BackColor = bgColor, Margin = new Padding(0) };
            pnlScroll.Padding = new Padding(30, 30, 30, 50); // Spacious padding
            rootLayout.Controls.Add(pnlScroll, 0, 1);

            FlowLayoutPanel flpMaster = new FlowLayoutPanel() { 
                Dock = DockStyle.Top, 
                FlowDirection = FlowDirection.TopDown, 
                WrapContents = false, 
                AutoSize = true,
                Width = 1000
            };
            pnlScroll.Controls.Add(flpMaster);

            // --- A. DYNAMIC STATS CARDS ---
            TableLayoutPanel tlpStats = new TableLayoutPanel() { Width = 1000, Height = 130, ColumnCount = 3, RowCount = 1, Margin = new Padding(0, 0, 0, 40) };
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));

            lblTotalAss = CreateValLabel("0");
            lblPendingGrading = CreateValLabel("0", Color.FromArgb(241, 196, 15));
            lblAvgMarks = CreateValLabel("N/A", Color.Gray);

            tlpStats.Controls.Add(CreateStatCard("TOTAL ASSIGNMENTS", lblTotalAss, primaryColor), 0, 0);
            tlpStats.Controls.Add(CreateStatCard("STUDENTS PENDING MARKS", lblPendingGrading, Color.FromArgb(241, 196, 15)), 1, 0);
            tlpStats.Controls.Add(CreateStatCard("AVG. CLASS SCORE %", lblAvgMarks, Color.FromArgb(46, 204, 113)), 2, 0);
            flpMaster.Controls.Add(tlpStats);

            // --- B. POST ASSIGNMENT ---
            flpMaster.Controls.Add(CreateSectionTitle("GENERATE NEW ASSIGNMENT"));
            Panel pnlPost = new Panel() { Width = 1000, Height = 220, BackColor = cardBg, Padding = new Padding(20), Margin = new Padding(0, 0, 0, 40) };
            pnlPost.Paint += (s, e) => DrawBorder(e.Graphics, pnlPost.ClientRectangle);
            
            TableLayoutPanel tlpForm = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 2 };
            txtTopic = AddInput(tlpForm, "Assignment Topic Name", 0, 0, 2);
            txtMaxMarks = AddInput(tlpForm, "Max Marks Possible", 2, 0, 1);
            dtpDue = AddDate(tlpForm, "Submission Deadline", 3, 0);
            
            cmbDept = AddDrop(tlpForm, "Stream/Dept", new string[] { "B.Sc IT", "B.Sc CS", "BMS" }, 0, 1);
            cmbDiv = AddDrop(tlpForm, "Target Division", new string[] { "Div A", "Div B", "Div C" }, 1, 1);
            
            Button btnCreate = new Button() { Text = "⚡ POST & PUSH", BackColor = primaryColor, ForeColor = Color.White, Dock = DockStyle.Top, Height = 45, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Margin = new Padding(10, 18, 0, 0) };
            btnCreate.Click += (s, e) => CreateAssignment();
            tlpForm.Controls.Add(btnCreate, 2, 1); tlpForm.SetColumnSpan(btnCreate, 2);
            pnlPost.Controls.Add(tlpForm);
            flpMaster.Controls.Add(pnlPost);

            // --- C. REPOSITORY ---
            flpMaster.Controls.Add(CreateSectionTitle("ASSIGNMENT REPOSITORY"));
            Panel pnlGrid1Wrap = new Panel() { Width = 1000, Height = 250, BackColor = cardBg, Padding = new Padding(1), Margin = new Padding(0, 0, 0, 45) };
            dgvAssignments = CreateStyledGrid(dtAssignments);
            pnlGrid1Wrap.Controls.Add(dgvAssignments);
            flpMaster.Controls.Add(pnlGrid1Wrap);

            // --- D. CLASS FILTERING & GRADING SECTION ---
            flpMaster.Controls.Add(CreateSectionTitle("CLASS WISE MARKS ENTRY & GRADING"));
            Panel pnlFilterCard = new Panel() { Width = 1000, Height = 100, BackColor = Color.FromArgb(245, 245, 245), Padding = new Padding(15), Margin = new Padding(0, 0, 0, 20) };
            TableLayoutPanel tlpFilter = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 1 };
            cmbGradingDept = AddDrop(tlpFilter, "SELECT DEPT", new string[] { "B.Sc IT", "B.Sc CS", "BMS" }, 0, 0);
            cmbGradingDiv = AddDrop(tlpFilter, "SELECT DIV", new string[] { "Div A", "Div B", "Div C" }, 1, 0);
            cmbSelectAssignment = AddDrop(tlpFilter, "LINK ASSIGNMENT", new string[] { "AS-101: OOP", "AS-102: SQL" }, 2, 0);
            
            Button btnFetch = new Button() { Text = "🔍 LOAD CLASS ROLL", BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold), Margin = new Padding(10, 18, 0, 0) };
            btnFetch.Click += (s, e) => FilterStudents();
            tlpFilter.Controls.Add(btnFetch, 3, 0);
            pnlFilterCard.Controls.Add(tlpFilter);
            flpMaster.Controls.Add(pnlFilterCard);

            // --- E. STUDENT GRADING TABLE ---
            Panel pnlGrid2Wrap = new Panel() { Width = 1000, Height = 450, BackColor = cardBg, Padding = new Padding(1), Margin = new Padding(0, 0, 0, 50) };
            dgvStudentGrading = CreateStyledGrid(dtStudentGrading);
            dgvStudentGrading.ReadOnly = false;
            foreach (DataGridViewColumn col in dgvStudentGrading.Columns) 
                if (col.Name != "Marks Obtained") col.ReadOnly = true; 
            pnlGrid2Wrap.Controls.Add(dgvStudentGrading);
            flpMaster.Controls.Add(pnlGrid2Wrap);

            pnlScroll.Resize += (s, e) => {
                int w = Math.Max(600, pnlScroll.Width - 75);
                flpMaster.Width = pnlScroll.Width; 
                tlpStats.Width = w;
                pnlPost.Width = w;
                pnlGrid1Wrap.Width = w;
                pnlFilterCard.Width = w;
                pnlGrid2Wrap.Width = w;
            };
            FilterStudents(); // Primary Load
        }

        private void FilterStudents()
        {
            dtStudentGrading.Rows.Clear();
            string dept = cmbGradingDept.Text;
            string div = cmbGradingDiv.Text;
            string selAss = cmbSelectAssignment.Text; 
            string assId = selAss.Split(':')[0].Trim(); 
            string assName = selAss.Contains(":") ? selAss.Split(':')[1].Trim() : selAss;

            string max = "20"; 
            foreach (DataRow r in dtAssignments.Rows) if (r["ID"].ToString() == assId) max = r["Max Marks"].ToString();

            // Filter students
            var list = allStudents.Where(s => s.Dept == dept && s.Div == div).ToList();
            
            foreach (var s in list) {
                // Pull from Persistent Data Store
                string currentMark = s.Marks.ContainsKey(assId) ? s.Marks[assId] : "";
                string status = string.IsNullOrEmpty(currentMark) ? "Not Entered" : "✅ Entered";
                dtStudentGrading.Rows.Add(s.RollNo, s.Name, assName, currentMark, max, status);
            }

            // Update stats (will use global data logic)
            UpdateStats(); 
            if (list.Count == 0 && this.Visible) MessageBox.Show($"No students found in {dept} - {div}.", "Data Filter", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateStats()
        {
            if (dtAssignments == null || allStudents.Count == 0) return;

            // 1. Total Assignments in Repo
            lblTotalAss.Text = dtAssignments.Rows.Count.ToString();
            
            // 2. Global Pending - Count of (Student * Assignment) pairs missing a mark
            // We assume simple linking: All students in the pool should have marks for all assignments.
            // (In a real app, assignments would be assigned to specific Depts, but for Dummy Data, we assume global application or just count gaps)
            int totalPending = 0;
            double totalScore = 0;
            double gradedCount = 0;

            foreach(DataRow row in dtAssignments.Rows)
            {
                string aid = row["ID"].ToString();
                if(double.TryParse(row["Max Marks"].ToString(), out double max))
                {
                    foreach(var s in allStudents)
                    {
                        // Check if student has mark for this assignment
                        if (s.Marks.ContainsKey(aid) && !string.IsNullOrEmpty(s.Marks[aid]))
                        {
                            if(double.TryParse(s.Marks[aid], out double score))
                            {
                                totalScore += (score/max); 
                                gradedCount++;
                            }
                        }
                        else
                        {
                            totalPending++;
                        }
                    }
                }
            }

            lblPendingGrading.Text = totalPending.ToString();
            lblPendingGrading.ForeColor = totalPending > 0 ? Color.FromArgb(241, 196, 15) : Color.White;

            // 3. Global Avg Performance
            if (gradedCount > 0)
            {
                double avg = (totalScore / gradedCount) * 100;
                lblAvgMarks.Text = avg.ToString("0.0") + "%";
                lblAvgMarks.ForeColor = avg > 70 ? Color.FromArgb(46, 204, 113) : Color.FromArgb(231, 76, 60);
            }
            else
            {
                lblAvgMarks.Text = "N/A";
                lblAvgMarks.ForeColor = Color.Gray;
            }
        }

        private void SetupGridEvents()
        {
            // Trigger stats update on ANY change in the grading table AND PERSIST IT
            dgvStudentGrading.CellValueChanged += (s, e) => {
                if (e.RowIndex >= 0 && dgvStudentGrading.Columns[e.ColumnIndex].Name == "Marks Obtained") {
                    
                    // Snapshot the new value
                    string roll = dgvStudentGrading.Rows[e.RowIndex].Cells["Roll No"].Value.ToString();
                    string val = dgvStudentGrading.Rows[e.RowIndex].Cells["Marks Obtained"].Value?.ToString();

                    // Find student in master list and update
                    var st = allStudents.FirstOrDefault(x => x.RollNo == roll);
                    if(st != null)
                    {
                        string assId = cmbSelectAssignment.Text.Split(':')[0].Trim();
                        if(!string.IsNullOrEmpty(val)) st.Marks[assId] = val;
                        else if(st.Marks.ContainsKey(assId)) st.Marks.Remove(assId);
                    }

                    dgvStudentGrading.Rows[e.RowIndex].Cells["Status"].Value = string.IsNullOrEmpty(val) ? "Not Entered" : "✅ Entered";
                    UpdateStats();
                }
            };
            dgvStudentGrading.CurrentCellDirtyStateChanged += (s, e) => { if (dgvStudentGrading.IsCurrentCellDirty) dgvStudentGrading.CommitEdit(DataGridViewDataErrorContexts.Commit); };
            
            // Trigger stats if Repo changes
            dgvAssignments.CellValueChanged += (s, e) => UpdateStats();
        }

        private void CreateAssignment()
        {
            if (string.IsNullOrWhiteSpace(txtTopic.Text)) return;
            string newId = "AS-" + new Random().Next(200, 999);
            dtAssignments.Rows.InsertAt(dtAssignments.NewRow(), 0);
            dtAssignments.Rows[0][0] = newId;
            dtAssignments.Rows[0][1] = txtTopic.Text;
            dtAssignments.Rows[0][2] = cmbDept.Text + " / " + cmbDiv.Text;
            dtAssignments.Rows[0][3] = dtpDue.Value.ToShortDateString();
            dtAssignments.Rows[0][4] = txtMaxMarks.Text;
            dtAssignments.Rows[0][5] = "Active";
            
            txtTopic.Clear();
            RefreshAssignmentSelector();
            UpdateStats(); 
            MessageBox.Show("New Assignment Successfully Pushed to Repository!", "Workflow", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RefreshAssignmentSelector()
        {
            cmbSelectAssignment.Items.Clear();
            foreach (DataRow row in dtAssignments.Rows) cmbSelectAssignment.Items.Add(row["ID"] + ": " + row["Topic"]);
            if (cmbSelectAssignment.Items.Count > 0) cmbSelectAssignment.SelectedIndex = 0;
        }

        private Panel CreateStatCard(string title, Label val, Color accent) {
            Panel p = new Panel() { Dock = DockStyle.Fill, BackColor = cardBg, Margin = new Padding(0, 0, 15, 0) };
            p.Paint += (s, e) => DrawBorder(e.Graphics, p.ClientRectangle);
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label head = new Label() { Text = title, ForeColor = Color.Gray, Font = new Font("Segoe UI", 7, FontStyle.Bold), Location = new Point(15, 12), AutoSize = true };
            val.Location = new Point(15, 38);
            p.Controls.AddRange(new Control[] { l, head, val });
            return p;
        }

        private TextBox AddInput(TableLayoutPanel p, string label, int col, int row, int span = 1) {
            Panel w = new Panel() { Dock = DockStyle.Top, Height = 65, Padding = new Padding(5) };
            Label l = new Label() { Text = label, ForeColor = Color.FromArgb(173, 22, 37), Font = new Font("Segoe UI", 8, FontStyle.Bold), Dock = DockStyle.Top };
            TextBox t = new TextBox() { Dock = DockStyle.Top, BackColor = Color.White, ForeColor = Color.FromArgb(40, 40, 40), BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 11) };
            w.Controls.AddRange(new Control[] { t, l });
            p.Controls.Add(w, col, row); if (span > 1) p.SetColumnSpan(w, span);
            return t;
        }

        private ComboBox AddDrop(TableLayoutPanel p, string label, string[] items, int col, int row) {
            Panel w = new Panel() { Dock = DockStyle.Top, Height = 65, Padding = new Padding(5) };
            Label l = new Label() { Text = label, ForeColor = Color.FromArgb(173, 22, 37), Font = new Font("Segoe UI", 8, FontStyle.Bold), Dock = DockStyle.Top };
            ComboBox c = new ComboBox() { Dock = DockStyle.Top, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.FromArgb(40, 40, 40), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            c.Items.AddRange(items); if (c.Items.Count > 0) c.SelectedIndex = 0;
            w.Controls.AddRange(new Control[] { c, l });
            p.Controls.Add(w, col, row);
            return c;
        }

        private DateTimePicker AddDate(TableLayoutPanel p, string label, int col, int row) {
            Panel w = new Panel() { Dock = DockStyle.Top, Height = 65, Padding = new Padding(5) };
            Label l = new Label() { Text = label, ForeColor = Color.FromArgb(173, 22, 37), Font = new Font("Segoe UI", 8, FontStyle.Bold), Dock = DockStyle.Top };
            DateTimePicker d = new DateTimePicker() { Dock = DockStyle.Top, Format = DateTimePickerFormat.Short, BackColor = Color.White, ForeColor = Color.FromArgb(40, 40, 40) };
            w.Controls.AddRange(new Control[] { d, l });
            p.Controls.Add(w, col, row);
            return d;
        }

        private DataGridView CreateStyledGrid(DataTable dt) {
            DataGridView dgv = new DataGridView() { Dock = DockStyle.Fill, DataSource = dt, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, ForeColor = Color.FromArgb(40, 40, 40), GridColor = Color.FromArgb(220, 220, 220), RowTemplate = { Height = 40 }, ColumnHeadersHeight = 45, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, EnableHeadersVisualStyles = false };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = primaryColor; dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; dgv.DefaultCellStyle.BackColor = Color.White; dgv.DefaultCellStyle.SelectionBackColor = primaryColor;
            return dgv;
        }

        private Label CreateSectionTitle(string text) => new Label() { Text = "──  " + text, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = primaryColor, AutoSize = true, Margin = new Padding(0, 0, 0, 15) };
        private Label CreateValLabel(string text, Color? c = null) => new Label() { Text = text, Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = c ?? Color.RoyalBlue, AutoSize = true };

        private void DrawBorder(Graphics g, Rectangle r) {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen pen = new Pen(borderColor, 1)) g.DrawRectangle(pen, r.X, r.Y, r.Width - 1, r.Height - 1);
        }

        private void InitializeComponent() {
            this.SuspendLayout();
            this.Name = "AssignmentsControl";
            this.Size = new Size(1100, 1600);
            this.ResumeLayout(false);
        }

        private class StudentEntry { 
            public string RollNo, Name, Dept, Div; 
            public Dictionary<string, string> Marks = new Dictionary<string, string>();
        }
    }
}
