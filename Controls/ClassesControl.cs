using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TeacherDashboard.Controls
{
    public partial class ClassesControl : UserControl
    {
        private FlowLayoutPanel flpDailyReports;
        private ComboBox cmbDept;
        private ComboBox cmbDiv;
        private ComboBox cmbDay;
        private ComboBox cmbView;
        private Label lblLecCount;
        private Label lblPracCount;
        private Label lblBreakTime;

        public ClassesControl()
        {
            InitializeComponent();
            SetupStrictLayout();
            LoadMockData();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitle = new Label() { Text = "FACULTY CLASS ACTIVITIES", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            // 2. Filter Bar
            Panel pnlFilter = new Panel() { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(15) };
            FlowLayoutPanel flpFilters = new FlowLayoutPanel() { Dock = DockStyle.Fill, WrapContents = false };
            
            Label lblFilter = new Label() { Text = "FILTERS:", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.LightGray, AutoSize = true, Margin = new Padding(0, 5, 20, 0) };
            flpFilters.Controls.Add(lblFilter);

            cmbDept = CreateDarkComboBox(new string[] { "All Departments", "B.Sc IT", "B.Sc CS", "BMS", "B.Com" });
            cmbDept.Width = 160;
            flpFilters.Controls.Add(cmbDept);

            cmbDiv = CreateDarkComboBox(new string[] { "All Divisions", "Div A", "Div B", "Div C" });
            cmbDiv.Width = 120;
            flpFilters.Controls.Add(cmbDiv);

            cmbDay = CreateDarkComboBox(new string[] { "Today", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" });
            cmbDay.Width = 120;
            flpFilters.Controls.Add(cmbDay);

            cmbView = CreateDarkComboBox(new string[] { "Schedule View", "List View", "Calendar View" });
            cmbView.Width = 150;
            flpFilters.Controls.Add(cmbView);

            Button btnApply = new Button() { Text = "Apply Filter", FlatStyle = FlatStyle.Flat, ForeColor = Color.White, BackColor = Color.FromArgb(46, 204, 113), Size = new Size(120, 32), Margin = new Padding(20, 0, 0, 0), Cursor = Cursors.Hand };
            btnApply.FlatAppearance.BorderSize = 0;
            btnApply.Click += (s, e) => LoadMockData();
            flpFilters.Controls.Add(btnApply);

            pnlFilter.Controls.Add(flpFilters);
            pnlScroll.Controls.Add(pnlFilter);

            // ... (rest of SetupStrictLayout remains the same)
            // 3. Summary Stats
            FlowLayoutPanel flpStats = new FlowLayoutPanel() { Dock = DockStyle.Top, Height = 110, Padding = new Padding(0, 20, 0, 10) };
            
            lblLecCount = new Label() { Text = "0", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 35), AutoSize = true };
            lblPracCount = new Label() { Text = "0", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 35), AutoSize = true };
            lblBreakTime = new Label() { Text = "12:30 PM", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 35), AutoSize = true };

            flpStats.Controls.Add(CreateCompactStat("Lectures Today", lblLecCount, Color.FromArgb(52, 152, 219)));
            flpStats.Controls.Add(CreateCompactStat("Practicals", lblPracCount, Color.FromArgb(155, 89, 182)));
            flpStats.Controls.Add(CreateCompactStat("Next Break", lblBreakTime, Color.FromArgb(46, 204, 113)));
            pnlScroll.Controls.Add(flpStats);

            // 4. NEW: Today's Lectures (Time Table View)
            Label lblSchedule = new Label() { Text = "FACULTY DAILY TIMETABLE", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 45, Margin = new Padding(0, 25, 0, 0), TextAlign = ContentAlignment.BottomLeft };
            pnlScroll.Controls.Add(lblSchedule);

            FlowLayoutPanel flpSchedule = new FlowLayoutPanel() { Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Width = 1000, Padding = new Padding(0, 0, 0, 20) };
            flpSchedule.Name = "flpSchedule";
            pnlScroll.Controls.Add(flpSchedule);

            // 5. Faculty Class Engagement & Reminders (REPLACED Syllabus Tracker)
            Label lblRemindersTitle = new Label() { Text = "FACULTY ACTION ITEMS & CLASS NOTES", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 45, Margin = new Padding(0, 25, 0, 0), TextAlign = ContentAlignment.BottomLeft };
            pnlScroll.Controls.Add(lblRemindersTitle);

            flpDailyReports = new FlowLayoutPanel() { Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Width = 1000 };
            pnlScroll.Controls.Add(flpDailyReports);

            // Force Order
            this.Controls.SetChildIndex(pnlHeader, 1);
            this.Controls.SetChildIndex(pnlScroll, 0);
            
            pnlScroll.Controls.SetChildIndex(flpDailyReports, 0); // Bottom
            pnlScroll.Controls.SetChildIndex(lblRemindersTitle, 1);
            pnlScroll.Controls.SetChildIndex(flpSchedule, 2); // Middle
            pnlScroll.Controls.SetChildIndex(lblSchedule, 3);
            pnlScroll.Controls.SetChildIndex(flpStats, 4);    // Upper
            pnlScroll.Controls.SetChildIndex(pnlFilter, 5);   // Top
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
            return cb;
        }

        private Panel CreateCompactStat(string title, Label lblValue, Color accent)
        {
            Panel p = new Panel() { Size = new Size(220, 80), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 10) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 4, BackColor = accent };
            Label lblT = new Label() { Text = title.ToUpper(), Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.Gray, Location = new Point(15, 15), AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lblT, lblValue });
            return p;
        }
        
        // NEW: Schedule Card Helper
        private Panel CreateScheduleRow(string time, string type, string deptClass, string subject, Color accent)
        {
            Panel p = new Panel() { Size = new Size(950, 90), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 0, 15) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 6, BackColor = accent };
            string duration = (type == "PRACTICAL") ? "Duration: 2 Hours (Lab)" : "Duration: 1 Hour (Lec)";
            Color typeColor = (type == "PRACTICAL") ? Color.FromArgb(155, 89, 182) : Color.FromArgb(52, 152, 219);

            Label lblTime = new Label() { Text = time, Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Location = new Point(25, 20), AutoSize = true };
            Label lblTimeSub = new Label() { Text = duration, Font = new Font("Segoe UI", 9, FontStyle.Italic), ForeColor = Color.Gray, Location = new Point(25, 48), AutoSize = true };
            Label lblType = new Label() { Text = type, BackColor = Color.FromArgb(40, typeColor), ForeColor = typeColor, Font = new Font("Segoe UI", 8, FontStyle.Bold), Location = new Point(200, 30), Size = new Size(110, 28), TextAlign = ContentAlignment.MiddleCenter, FlatStyle = FlatStyle.Flat };
            Label lblClass = new Label() { Text = deptClass, Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Location = new Point(360, 15), AutoSize = true };
            Label lblSub = new Label() { Text = subject, Font = new Font("Segoe UI", 10), ForeColor = Color.LightGray, Location = new Point(360, 45), AutoSize = true };
            Label lblStatus = new Label() { Text = "Upcoming", Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(46, 204, 113), Location = new Point(830, 35), AutoSize = true };

            p.Controls.AddRange(new Control[] { l, lblTime, lblTimeSub, lblType, lblClass, lblSub, lblStatus });
            return p;
        }

        private Panel CreateActionItem(string type, string desc, string deadline, Color color)
        {
            Panel p = new Panel() { Size = new Size(950, 75), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 0, 12) };
            Label lblTag = new Label() { Text = type, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = color, Location = new Point(15, 12), AutoSize = true };
            Label lblDesc = new Label() { Text = desc, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 32), AutoSize = true };
            Label lblTime = new Label() { Text = "Due: " + deadline, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(780, 27), AutoSize = true };
            CheckBox cb = new CheckBox() { Location = new Point(910, 27), AutoSize = true, FlatStyle = FlatStyle.Flat };
            p.Controls.AddRange(new Control[] { lblTag, lblDesc, lblTime, cb });
            return p;
        }

        private void LoadMockData()
        {
            FlowLayoutPanel flpSchedule = (FlowLayoutPanel)FindControlRecursive(this, "flpSchedule");
            if(flpSchedule == null) return;
            flpSchedule.Controls.Clear();

            string deptFilt = cmbDept.Text;
            string divFilt = cmbDiv.Text;

            int lecCount = 0;
            int pracCount = 0;

            // Mock Data List
            var items = new[] {
                new { Time = "08:00 AM", Type = "LECTURE", Class = "FY-B.Sc IT | Div A", Sub = "Digital Electronics", Color = Color.FromArgb(52, 152, 219) },
                new { Time = "09:00 AM", Type = "LECTURE", Class = "SY-B.Sc IT | Div B", Sub = "Database Systems", Color = Color.FromArgb(52, 152, 219) },
                new { Time = "10:30 AM", Type = "PRACTICAL", Class = "TY-B.Sc IT | Div A", Sub = "Network Security Lab", Color = Color.FromArgb(155, 89, 182) },
                new { Time = "01:30 PM", Type = "LECTURE", Class = "FY-BMS | Div C", Sub = "Business Ethics", Color = Color.FromArgb(52, 152, 219) },
                new { Time = "02:30 PM", Type = "PRACTICAL", Class = "SY-B.Sc IT | Div B", Sub = "Java Programming Lab", Color = Color.FromArgb(155, 89, 182) }
            };

            foreach (var item in items)
            {
                bool deptMatch = deptFilt == "All Departments" || item.Class.Contains(deptFilt);
                bool divMatch = divFilt == "All Divisions" || item.Class.Contains(divFilt);

                if (deptMatch && divMatch)
                {
                    flpSchedule.Controls.Add(CreateScheduleRow(item.Time, item.Type, item.Class, item.Sub, item.Color));
                    if (item.Type == "LECTURE") lecCount++;
                    else if (item.Type == "PRACTICAL") pracCount++;
                }
            }

            // Update Statistics Cards Cards
            lblLecCount.Text = lecCount == 1 ? "1 Lecture" : $"{lecCount} Lectures";
            lblPracCount.Text = pracCount == 1 ? "1 Lab Session" : $"{pracCount} Lab Sessions";
            
            // Logic for Break Time (Simulated: Break after 10:30 labs or 12:00)
            lblBreakTime.Text = (deptFilt == "All Departments") ? "12:30 PM" : "01:00 PM";

            // Load Action Items (Static for now)
            flpDailyReports.Controls.Clear();
            flpDailyReports.Controls.Add(CreateActionItem("PENDING GRADE", "Grade SY-BSCIT Database Journals", "Today, 5:00 PM", Color.FromArgb(241, 196, 15)));
            flpDailyReports.Controls.Add(CreateActionItem("PREPARATION", "Update Slides for TY-Cloud Unit 3", "Tomorrow", Color.FromArgb(52, 152, 219)));
        }

        public Control FindControlRecursive(Control container, string name)
        {
            if (container.Name == name) return container;
            foreach (Control ctrl in container.Controls)
            {
                Control found = FindControlRecursive(ctrl, name);
                if (found != null) return found;
            }
            return null;
        }
    }
}

