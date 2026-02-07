using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Linq;

namespace TeacherDashboard.Controls
{
    public partial class AdminFeedbackControl : UserControl
    {
        private Color primaryColor = Color.FromArgb(173, 22, 37); // VSIT Red
        private Color darkColor = Color.RoyalBlue;
        private Color lightGray = Color.FromArgb(245, 245, 245);
        private Color blueColor = Color.FromArgb(41, 128, 185);
        private Color greenColor = Color.FromArgb(46, 204, 113);
        private Color orangeColor = Color.Orange;
        private Color redColor = Color.FromArgb(231, 76, 60);

        private DataTable dtFeedback;
        private DataTable dtComplaints;
        private DataGridView dgvFeedback;
        private DataGridView dgvComplaints;

        // Feedback Creation Fields
        private ComboBox cmbTeacher;
        private ComboBox cmbRating;
        private TextBox txtComment;

        // Stats Labels
        private Label lblAvgRating;
        private Label lblTotalReviews;

        public AdminFeedbackControl()
        {
            InitializeComponent();
            SetupData();
            SetupLayout();
            UpdateStats();
        }

        private void SetupData()
        {
            dtFeedback = new DataTable();
            dtFeedback.Columns.Add("Teacher");
            dtFeedback.Columns.Add("Dept");
            dtFeedback.Columns.Add("Type"); 
            dtFeedback.Columns.Add("RatingValue", typeof(double));
            dtFeedback.Columns.Add("RatingDisplay");
            dtFeedback.Columns.Add("Comment");
            dtFeedback.Columns.Add("Date");

            // Seed Data
            AddFeedbackRow("Dr. Rajesh Kumar", "CSE", "Student", 4.8, "Excellent explanation of algorithms. Very clear logic.");
            AddFeedbackRow("Prof. Anita Sharma", "IT", "Student", 3.5, "Lectures are a bit fast, hard to follow sometimes.");
            AddFeedbackRow("Mr. Amit Verma", "Mech", "Peer", 4.0, "Good class control but needs more diagrams on board.");
            AddFeedbackRow("Ms. Priya Singh", "BMS", "Student", 5.0, "Best teacher ever! Very supportive.");
            AddFeedbackRow("Prof. Anita Sharma", "IT", "Admin", 3.0, "Needs to improve punctuality.");

            dtComplaints = new DataTable();
            dtComplaints.Columns.Add("ID");
            dtComplaints.Columns.Add("Category");
            dtComplaints.Columns.Add("Description");
            dtComplaints.Columns.Add("RaisedBy");
            dtComplaints.Columns.Add("Status");
            dtComplaints.Columns.Add("Priority");

            dtComplaints.Rows.Add("CMP-101", "Infrastructure", "Projector in Lab 3 not working", "Mr. Rohan Das", "Pending", "High");
            dtComplaints.Rows.Add("CMP-102", "Academic", "Syllabus for Applied Math is outdated", "Dr. Rajesh Kumar", "In Progress", "Medium");
            dtComplaints.Rows.Add("CMP-103", "Hygiene", "Water cooler on 2nd floor leaking", "Student Council", "Resolved", "Low");
        }

        private void AddFeedbackRow(string teacher, string dept, string type, double rating, string comment)
        {
            string stars = "";
            int r = (int)Math.Max(0, Math.Min(5, Math.Round(rating)));
            for (int i = 0; i < r; i++) stars += "★";
            for (int i = r; i < 5; i++) stars += "☆";

            dtFeedback.Rows.Add(teacher, dept, type, rating, $"{stars} ({rating:F1}/5.0)", comment, DateTime.Now.ToString("yyyy-MM-dd"));
        }

        private void SetupLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            // 1. Fixed Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 130, BackColor = Color.White, Padding = new Padding(30, 25, 30, 0) };
            
            FlowLayoutPanel tlpHead = new FlowLayoutPanel() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = true };
            
            Label lblTitle = new Label() { Text = "💬 QUALITY & GRIEVANCE HUB", Font = new Font("Segoe UI", 22, FontStyle.Bold), ForeColor = primaryColor, AutoSize = true, Margin = new Padding(0, 0, 0, 5) };
            Label lblSubtitle = new Label() { Text = "Monitor faculty performance reviews and manage institutional complaints", Font = new Font("Segoe UI", 11), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(4, 0, 0, 0) };
            
            tlpHead.Controls.Add(lblTitle);
            tlpHead.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(tlpHead);

            Panel pnlAccent = new Panel() { Dock = DockStyle.Bottom, Height = 5, BackColor = primaryColor };
            pnlHeader.Controls.Add(pnlAccent);
            this.Controls.Add(pnlHeader);

            // 2. Main Content Area
            Panel pnlMain = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(15), BackColor = Color.FromArgb(240, 240, 240) };
            
            TabControl tabControl = new TabControl() { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10) };
            
            TabPage tabFeedback = new TabPage("Teacher Feedback & Reviews");
            tabFeedback.BackColor = lightGray;
            SetupFeedbackTab(tabFeedback);
            tabControl.TabPages.Add(tabFeedback);

            TabPage tabComplaints = new TabPage("Complaint Management");
            tabComplaints.BackColor = lightGray;
            SetupComplaintsTab(tabComplaints);
            tabControl.TabPages.Add(tabComplaints);

            pnlMain.Controls.Add(tabControl);
            this.Controls.Add(pnlMain);

            // Fix Docking Order
            pnlMain.BringToFront();
            pnlHeader.SendToBack();
        }

        private void SetupFeedbackTab(TabPage tab)
        {
            // Root container with spacing
            TableLayoutPanel tlpRoot = new TableLayoutPanel() { 
                Dock = DockStyle.Fill, 
                ColumnCount = 1, 
                RowCount = 5, 
                Padding = new Padding(20),
                BackColor = lightGray
            };
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 120)); // Stats
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));  // Gap
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 180)); // Create Form
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));  // Gap
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100));  // Table

            // 1. STATS PANEL
            Panel pnlStatsWrap = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White };
            pnlStatsWrap.Paint += (s, e) => ControlPaint.DrawBorder(e.Graphics, pnlStatsWrap.ClientRectangle, Color.LightGray, ButtonBorderStyle.Solid);
            
            TableLayoutPanel tlpStats = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 3 };
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));

            lblAvgRating = new Label() { Text = "0.0", Font = new Font("Segoe UI", 26, FontStyle.Bold), ForeColor = blueColor, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            lblTotalReviews = new Label() { Text = "0", Font = new Font("Segoe UI", 26, FontStyle.Bold), ForeColor = darkColor, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            Label lblHighest = new Label() { Text = "Dr. Rajesh Kumar", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = greenColor, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };

            tlpStats.Controls.Add(CreateStatItem("AVERAGE RATING", lblAvgRating), 0, 0);
            tlpStats.Controls.Add(CreateStatItem("TOTAL FEEDBACKS", lblTotalReviews), 1, 0);
            tlpStats.Controls.Add(CreateStatItem("TOP RATED FACULTY", lblHighest), 2, 0);
            pnlStatsWrap.Controls.Add(tlpStats);
            tlpRoot.Controls.Add(pnlStatsWrap, 0, 0);

            // 2. CREATE FORM PANEL
            Panel pnlCreateWrap = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(25, 20, 25, 20) };
            pnlCreateWrap.Paint += (s, e) => ControlPaint.DrawBorder(e.Graphics, pnlCreateWrap.ClientRectangle, Color.FromArgb(220, 220, 220), ButtonBorderStyle.Solid);
            
            Label lblCreateTitle = new Label() { Text = "➕ CREATE NEW FACULTY REVIEW", Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = primaryColor, Dock = DockStyle.Top, Height = 35 };
            pnlCreateWrap.Controls.Add(lblCreateTitle);

            TableLayoutPanel tlpForm = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 4, Padding = new Padding(0, 10, 0, 0) };
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15f));
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45f));
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15f));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F)); // Increased height for label + input visibility

            // Teacher
            cmbTeacher = new ComboBox() { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cmbTeacher.Items.AddRange(new string[] { "Dr. Rajesh Kumar", "Prof. Anita Sharma", "Mr. Amit Verma", "Ms. Priya Singh" });
            cmbTeacher.SelectedIndex = 0;
            tlpForm.Controls.Add(CreateFieldGroup("Faculty Member:", cmbTeacher), 0, 0);

            // Rating
            cmbRating = new ComboBox() { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cmbRating.Items.AddRange(new string[] { "5", "4", "3", "2", "1" });
            cmbRating.SelectedIndex = 0;
            tlpForm.Controls.Add(CreateFieldGroup("Rating (1-5):", cmbRating), 1, 0);

            // Comment
            txtComment = new TextBox() { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10) };
            tlpForm.Controls.Add(CreateFieldGroup("Detailed Reason / Comment:", txtComment), 2, 0);

            // Button
            Button btnSubmit = new Button() { 
                Text = "Submit Review", BackColor = primaryColor, ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, Height = 40, Margin = new Padding(10, 22, 0, 0),
                Font = new Font("Segoe UI", 9, FontStyle.Bold), Dock = DockStyle.Top
            };
            btnSubmit.Click += BtnSubmit_Click;
            tlpForm.Controls.Add(btnSubmit, 3, 0);

            pnlCreateWrap.Controls.Add(tlpForm);
            tlpForm.BringToFront(); // Ensure it respects the Title area
            tlpRoot.Controls.Add(pnlCreateWrap, 0, 2);

            // 3. DATA GRID
            Panel pnlGridWrap = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White };
            dgvFeedback = CreateStyledGrid(dtFeedback);
            
            dgvFeedback.DataBindingComplete += (s, e) => {
                if (dgvFeedback.Columns.Contains("RatingValue")) dgvFeedback.Columns["RatingValue"].Visible = false;
                if (dgvFeedback.Columns.Contains("RatingDisplay")) dgvFeedback.Columns["RatingDisplay"].HeaderText = "Rating";
            };

            dgvFeedback.CellFormatting += (s, e) => {
                if (e.RowIndex < 0) return;
                var grid = (DataGridView)s;
                if (grid.Columns.Contains("RatingDisplay") && grid.Columns[e.ColumnIndex].Name == "RatingDisplay")
                {
                    if (grid.Columns.Contains("RatingValue") && grid.Rows[e.RowIndex].Cells["RatingValue"].Value != null)
                    {
                        double val = Convert.ToDouble(grid.Rows[e.RowIndex].Cells["RatingValue"].Value);
                        if (val >= 4.0) e.CellStyle.ForeColor = greenColor;
                        else if (val <= 2.5) e.CellStyle.ForeColor = redColor;
                        else e.CellStyle.ForeColor = orangeColor;
                        e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    }
                }
            };

            pnlGridWrap.Controls.Add(dgvFeedback);
            tlpRoot.Controls.Add(pnlGridWrap, 0, 4);

            tab.Controls.Add(tlpRoot);
        }

        private Panel CreateStatItem(string title, Label valLabel)
        {
            Panel p = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(10) };
            valLabel.Dock = DockStyle.Fill;
            Label lbl = new Label() { 
                Text = title, ForeColor = Color.Gray, Font = new Font("Segoe UI", 8, FontStyle.Bold), 
                Dock = DockStyle.Bottom, Height = 25, TextAlign = ContentAlignment.MiddleCenter 
            };
            p.Controls.Add(valLabel);
            p.Controls.Add(lbl);
            return p;
        }

        private Panel CreateFieldGroup(string label, Control c)
        {
            Panel p = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(0, 0, 10, 0) };
            Label l = new Label() { Text = label, ForeColor = Color.DimGray, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), Dock = DockStyle.Top, Height = 25 };
            p.Controls.Add(c);
            p.Controls.Add(l);
            return p;
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtComment.Text)) {
                MessageBox.Show("Please provide a comment.", "Validation");
                return;
            }
            double r = double.Parse(cmbRating.Text);
            AddFeedbackRow(cmbTeacher.Text, "General", "Admin", r, txtComment.Text);
            txtComment.Clear();
            MessageBox.Show("Review Submitted Successfully!");
            UpdateStats();
        }

        private void UpdateStats()
        {
            int count = dtFeedback.Rows.Count;
            double sum = 0;
            foreach(DataRow row in dtFeedback.Rows) sum += Convert.ToDouble(row["RatingValue"]);
            
            if(count > 0) lblAvgRating.Text = (sum / count).ToString("F1") + " / 5.0";
            else lblAvgRating.Text = "0.0";
            
            lblTotalReviews.Text = count.ToString();
        }

        private void SetupComplaintsTab(TabPage tab)
        {
            Panel pnlContainer = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(25) };
            dgvComplaints = CreateStyledGrid(dtComplaints);
            
            DataGridViewButtonColumn btnAction = new DataGridViewButtonColumn() { 
                Name = "StatusAction", HeaderText = "Action", Text = "Resolve", UseColumnTextForButtonValue = true, Width = 100, FlatStyle = FlatStyle.Flat 
            };
            btnAction.DefaultCellStyle.BackColor = blueColor;
            btnAction.DefaultCellStyle.ForeColor = Color.White;
            dgvComplaints.Columns.Add(btnAction);
            
            dgvComplaints.CellClick += HandleComplaintAction;
            pnlContainer.Controls.Add(dgvComplaints);
            tab.Controls.Add(pnlContainer);
        }

        private void HandleComplaintAction(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvComplaints.Columns[e.ColumnIndex].Name == "StatusAction") {
                dtComplaints.Rows[e.RowIndex]["Status"] = "Resolved";
                MessageBox.Show("Complaint has been marked as RESOLVED.");
            }
        }

        private DataGridView CreateStyledGrid(DataTable dt)
        {
            DataGridView d = new DataGridView() { 
                Dock = DockStyle.Fill, DataSource = dt, 
                BackgroundColor = Color.White, BorderStyle = BorderStyle.None, 
                RowHeadersVisible = false, AllowUserToAddRows = false, 
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, 
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, 
                ColumnHeadersHeight = 45, RowTemplate = { Height = 45 }, 
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(240, 240, 240)
            };
            d.ColumnHeadersDefaultCellStyle.BackColor = primaryColor; 
            d.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; 
            d.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold); 
            d.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            return d;
        }

        private void InitializeComponent() {
            this.SuspendLayout();
            this.Name = "AdminFeedbackControl";
            this.Size = new Size(1100, 800);
            this.ResumeLayout(false);
        }
    }
}
