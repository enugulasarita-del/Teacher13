using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data;

namespace TeacherDashboard.Controls
{
    public partial class AnalyticsControl : UserControl
    {
        private Panel pnlContent;
        private Button currentBtn;
        
        // Colors
        private Color clrBackground = Color.FromArgb(28, 40, 51); // #1C2833
        private Color clrSidebar = Color.FromArgb(44, 62, 80);    // #2C3E50
        private Color clrActive = Color.FromArgb(52, 152, 219);   // Blue
        private Color clrCard = Color.FromArgb(52, 73, 94);       // #34495E
        private Color clrText = Color.White;

        public AnalyticsControl()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = clrBackground;
            SetupLayout();
            LoadTeacherPerformance(); // Default View
        }

        private void SetupLayout()
        {
            this.Controls.Clear();
            TableLayoutPanel master = new TableLayoutPanel() { 
                Dock = DockStyle.Fill, 
                ColumnCount = 2, 
                RowCount = 1,
                BackColor = clrBackground 
            };
            master.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250)); // Left Nav
            master.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));  // Content

            // 1. LEFT NAVIGATION PANEL
            Panel pnlNav = new Panel() { Dock = DockStyle.Fill, BackColor = clrSidebar, Padding = new Padding(0, 20, 0, 0) };
            Label lblNavTitle = new Label() { Text = "REPORTS & ANALYTICS", ForeColor = Color.LightGray, Font = new Font("Segoe UI", 12, FontStyle.Bold), Dock = DockStyle.Top, Height = 50, TextAlign = ContentAlignment.MiddleCenter };
            pnlNav.Controls.Add(lblNavTitle);

            AddNavButton(pnlNav, "Teacher Performance", LoadTeacherPerformance);
            AddNavButton(pnlNav, "Leave Analysis", LoadLeaveAnalysis);
            AddNavButton(pnlNav, "Syllabus Tracking", LoadSyllabusReport);
            AddNavButton(pnlNav, "Exam Results", LoadExamResultsReport);

            master.Controls.Add(pnlNav, 0, 0);

            // 2. RIGHT CONTENT PANEL
            pnlContent = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(20) };
            master.Controls.Add(pnlContent, 1, 0);

            this.Controls.Add(master);
        }

        private void AddNavButton(Panel p, string text, Action action)
        {
            Button btn = new Button() { 
                Text = "  " + text, 
                Dock = DockStyle.Top, 
                Height = 45, 
                FlatStyle = FlatStyle.Flat, 
                FlatAppearance = { BorderSize = 0 },
                BackColor = clrSidebar, 
                ForeColor = Color.LightGray, 
                TextAlign = ContentAlignment.MiddleLeft, 
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand,
                Padding = new Padding(15, 0, 0, 0)
            };
            btn.Click += (s, e) => {
                if (currentBtn != null) { currentBtn.BackColor = clrSidebar; currentBtn.ForeColor = Color.LightGray; }
                currentBtn = btn;
                currentBtn.BackColor = clrActive;
                currentBtn.ForeColor = Color.White;
                action.Invoke();
            };
            p.Controls.Add(btn);
            p.Controls.SetChildIndex(btn, 0); // Add to top effectively (reversed via Dock.Top sequence)
        }

        // -------------------------------------------------------------------------
        // REPORT 1: TEACHER PERFORMANCE
        // -------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // REPORT 1: TEACHER PERFORMANCE
        // -------------------------------------------------------------------------
        private void LoadTeacherPerformance()
        {
            DataGridView dgv = CreateGrid();
            dgv.Columns.Add("Name", "FACULTY NAME");
            dgv.Columns.Add("Dept", "DEPT");
            dgv.Columns.Add("Feedback", "STUDENT FEEDBACK");
            dgv.Columns.Add("Result", "RESULT PASS %");
            dgv.Columns.Add("Compliance", "COMPLIANCE");
            
            dgv.Rows.Add("Dr. Rajesh Kumar", "CSE", "4.8/5.0", "98%", "100%");
            dgv.Rows.Add("Prof. Anita Sharma", "ECE", "4.6/5.0", "92%", "95%");
            dgv.Rows.Add("Mr. Amit Verma", "Mech", "4.2/5.0", "88%", "90%");
            dgv.Rows.Add("Ms. Priya Singh", "BSH", "4.9/5.0", "99%", "100%");
            dgv.Rows.Add("Dr. Sneha Gupta", "CSE", "4.7/5.0", "95%", "98%");

            BuildReportPage("Teacher Performance Report", "Detailed analysis of faculty efficiency, student feedback, and compliance.", dgv, 
                ("🏆 TOP RATED", "Dr. Rajesh Kumar", "4.8 / 5.0 Rating"),
                ("📉 NEEDS ATTENTION", "Mr. Amit Verma", "Compliance: 90%"), 
                ("🏫 DEPT AVG", "Computer Science", "High Performance")
            );
        }

        // -------------------------------------------------------------------------
        // REPORT 2: FACULTY LEAVE ANALYSIS
        // -------------------------------------------------------------------------
        private void LoadLeaveAnalysis()
        {
            DataGridView dgv = CreateGrid();
            dgv.Columns.Add("Name", "FACULTY NAME");
            dgv.Columns.Add("Casual", "CASUAL LEAVE (CL)");
            dgv.Columns.Add("Sick", "SICK LEAVE (SL)");
            dgv.Columns.Add("Earned", "EARNED LEAVE (EL)");
            dgv.Columns.Add("LOP", "LOSS OF PAY");
            dgv.Columns.Add("Balance", "REMAINING QUOTA");

            dgv.Rows.Add("Dr. Rajesh Kumar", "2", "1", "0", "0", "9 Days");
            dgv.Rows.Add("Prof. Anita Sharma", "5", "2", "1", "1", "3 Days");
            dgv.Rows.Add("Mr. Amit Verma", "1", "0", "0", "0", "11 Days");
            dgv.Rows.Add("Ms. Priya Singh", "3", "3", "0", "0", "6 Days");
            dgv.Rows.Add("Dr. Sneha Gupta", "0", "0", "0", "0", "12 Days");

            BuildReportPage("Faculty Leave Analysis", "Overview of leaves taken vs. allocated quota.", dgv,
                ("⚠️ HIGH ABSENTEEISM", "Prof. Anita Sharma", "Only 3 Days Left"),
                ("✅ PERFECT RECORD", "Dr. Sneha Gupta", "0 Leaves Taken"),
                ("📊 AVG QUOTA LEFT", "8.2 Days", "Institutional Avg")
            );
        }

        // -------------------------------------------------------------------------
        // REPORT 3: SYLLABUS COVERAGE TRACKING
        // -------------------------------------------------------------------------
        private void LoadSyllabusReport()
        {
            DataGridView dgv = CreateGrid();
            dgv.Columns.Add("Dept", "DEPARTMENT");
            dgv.Columns.Add("Sem", "SEMESTER");
            dgv.Columns.Add("Subject", "SUBJECT");
            dgv.Columns.Add("Completion", "COMPLETION %");
            dgv.Columns.Add("Status", "STATUS");

            dgv.Rows.Add("CSE", "Sem 4", "Data Structures", "85%", "On Track");
            dgv.Rows.Add("CSE", "Sem 6", "AI & ML", "60%", "Delayed");
            dgv.Rows.Add("ECE", "Sem 4", "Digital Circuits", "90%", "Ahead");
            dgv.Rows.Add("Mech", "Sem 2", "Thermodynamics", "75%", "On Track");
            dgv.Rows.Add("BSH", "Sem 1", "App. Mathematics", "95%", "Almost Done");

            BuildReportPage("Syllabus Coverage Tracking", "Syllabus completion status by department & semester.", dgv,
                ("🚨 CRITICAL DELAY", "Artificial Intelligence", "Only 60% Done (CSE)"),
                ("🚀 AHEAD OF TIME", "App. Mathematics", "95% Covered"),
                ("📅 EST. COMPLETION", "April 15th", "Based on current pace")
            );
        }

        // -------------------------------------------------------------------------
        // REPORT 4: EXAM RESULTS ANALYSIS
        // -------------------------------------------------------------------------
        private void LoadExamResultsReport()
        {
            DataGridView dgv = CreateGrid();
            dgv.Columns.Add("Exam", "EXAM NAME");
            dgv.Columns.Add("Dept", "DEPARTMENT");
            dgv.Columns.Add("Pass", "PASS %");
            dgv.Columns.Add("TopScore", "TOP SCORE");
            dgv.Columns.Add("Failures", "FAIL COUNT");

            dgv.Rows.Add("Mid-Term 2024", "CSE", "92%", "98/100 (Rohan K.)", "4");
            dgv.Rows.Add("Mid-Term 2024", "ECE", "88%", "95/100 (Sanya M.)", "6");
            dgv.Rows.Add("Mid-Term 2024", "Mech", "85%", "92/100 (Vikram S.)", "8");
            dgv.Rows.Add("Finals 2023", "BSH", "95%", "100/100 (Aditi R.)", "2");
            dgv.Rows.Add("Unit Test 4", "Data Sci", "89%", "48/50 (Arjun P.)", "3");

            BuildReportPage("Exam Results Analysis", "Performance overview of recent semester exams.", dgv,
                ("🎓 BATCH TOPPER", "Aditi R. (BSH)", "100/100 Score"),
                ("📉 UNDERPERFORMING", "Mechanical Dept", "8 Failures (High)"),
                ("📈 OVERALL PASS %", "89.8%", "Institutional Average")
            );
        }

        // -------------------------------------------------------------------------
        // HELPERS
        // -------------------------------------------------------------------------
        private void BuildReportPage(string title, string subtitle, Control mainContent, params (string t, string v, string s)[] stats)
        {
            pnlContent.Controls.Clear();

            // Root Layout: TableLayoutPanel to PREVENT OVERLAPPING
            TableLayoutPanel tlp = new TableLayoutPanel() { 
                Dock = DockStyle.Fill, 
                ColumnCount = 1, 
                RowCount = 3,
                BackColor = clrBackground 
            };
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 120)); // Increased Header Height
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100));  // Grid (Fills remaining)
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 160)); // Increased Footer Height (Stats)

            // 1. Header
            Panel pnlHead = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(20, 15, 20, 15) };
            
            // Text Container (Left)
            Panel pnlText = new Panel() { Dock = DockStyle.Fill };
            Label lblT = new Label() { Text = title.ToUpper(), Font = new Font("Segoe UI", 24, FontStyle.Bold), ForeColor = clrText, Dock = DockStyle.Top, Height = 50, AutoSize = false };
            Label lblS = new Label() { Text = subtitle, Font = new Font("Segoe UI", 11), ForeColor = Color.DarkGray, Dock = DockStyle.Top, Height = 30, AutoSize = false };
            pnlText.Controls.Add(lblS);
            pnlText.Controls.Add(lblT);
            
            // Buttons Container (Right)
            Panel pnlBtns = new Panel() { Dock = DockStyle.Right, Width = 240, Padding = new Padding(10, 25, 0, 0) };
            Button btnPdf = new Button() { Text = "📥 DOWNLOAD PDF", Width = 110, Dock = DockStyle.Right, BackColor = Color.FromArgb(231, 76, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 7, FontStyle.Bold), Cursor = Cursors.Hand };
            Button btnXls = new Button() { Text = "📊 EXPORT EXCEL", Width = 110, Dock = DockStyle.Right, BackColor = Color.FromArgb(39, 174, 96), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 7, FontStyle.Bold), Cursor = Cursors.Hand };
            btnPdf.FlatAppearance.BorderSize = 0;
            btnXls.FlatAppearance.BorderSize = 0;

            pnlBtns.Controls.Add(btnXls);
            pnlBtns.Controls.Add(new Panel() { Dock = DockStyle.Right, Width = 10 }); 
            pnlBtns.Controls.Add(btnPdf);

            pnlHead.Controls.Add(pnlText);
            pnlHead.Controls.Add(pnlBtns);

            // 2. Main Content (Grid)
            mainContent.Dock = DockStyle.Fill;
            Panel pnlGridWrapper = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(20, 0, 20, 0) };
            pnlGridWrapper.Controls.Add(mainContent);

            // 3. Footer (Stats)
            FlowLayoutPanel flpStats = new FlowLayoutPanel() { Dock = DockStyle.Fill, WrapContents = false, Padding = new Padding(20, 15, 20, 15) };
            foreach (var stat in stats)
            {
                Panel p = new Panel() { Width = 280, Height = 110, BackColor = clrCard, Margin = new Padding(0, 0, 20, 0) };
                
                // Add a border effect
                p.Paint += (s, ev) => {
                    ControlPaint.DrawBorder(ev.Graphics, p.ClientRectangle, Color.FromArgb(50, 255, 255, 255), ButtonBorderStyle.Solid);
                };

                Panel bar = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = clrActive };
                Label lT = new Label() { Text = stat.t, ForeColor = Color.LightGray, Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(15, 15), AutoSize = true };
                Label lV = new Label() { Text = stat.v, ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(15, 40), AutoSize = true };
                Label lS = new Label() { Text = stat.s, ForeColor = clrActive, Font = new Font("Segoe UI", 10, FontStyle.Italic), Location = new Point(15, 75), AutoSize = true };
                
                p.Controls.AddRange(new Control[] { bar, lT, lV, lS });
                flpStats.Controls.Add(p);
            }

            tlp.Controls.Add(pnlHead, 0, 0);
            tlp.Controls.Add(pnlGridWrapper, 0, 1);
            tlp.Controls.Add(flpStats, 0, 2);

            pnlContent.Controls.Add(tlp);
        }

        private DataGridView CreateGrid()
        {
            DataGridView dgv = new DataGridView() { 
                Dock = DockStyle.Fill, 
                BackgroundColor = clrBackground, 
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 50,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 45 },
                EnableHeadersVisualStyles = false,
                GridColor = clrCard,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            
            dgv.DefaultCellStyle.BackColor = clrCard;
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            
            dgv.ColumnHeadersDefaultCellStyle.BackColor = clrSidebar;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 55, 70); 
            
            return dgv;
        }

    }
}
