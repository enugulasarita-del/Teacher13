using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public partial class AdminTimetableControl : UserControl
    {
        private DataTable dtSchedule;
        private DataTable dtTeachers;
        private Color primaryColor = Color.FromArgb(173, 22, 37);
        private Color bgColor = Color.White;
        private Color cardBg = Color.White;
        private FlowLayoutPanel flpStats;

        public AdminTimetableControl()
        {
            InitializeComponent();
            InitializeData();
            SetupLayout();
            CheckAllConflicts();
        }

        private void InitializeData()
        {
            // Teachers Data
            dtTeachers = new DataTable();
            dtTeachers.Columns.Add("Name");
            dtTeachers.Columns.Add("Department");
            dtTeachers.Columns.Add("Specialization");
            
            dtTeachers.Rows.Add("Dr. Rajesh Kumar", "BSc IT", "Networking, Cloud Computing");
            dtTeachers.Rows.Add("Prof. Anita Sharma", "BSc CS", "Discrete Mathematics, C++");
            dtTeachers.Rows.Add("Dr. Sneha Gupta", "BSc DS", "Probability, Machine Learning");
            dtTeachers.Rows.Add("Dr. Vikram Singh", "BSc CS", "Operating Systems, Java");
            dtTeachers.Rows.Add("Prof. Meena Iyer", "BSc IT", "Web Technology, PHP");
            dtTeachers.Rows.Add("Mr. Sunil Gavaskar", "BSc DS", "Big Data, Spark");
            dtTeachers.Rows.Add("Dr. Aditi Rao", "BSc IT", "Communication Skills");

            // Schedule Data (Class-Subject-Teacher Assignments)
            dtSchedule = new DataTable();
            dtSchedule.Columns.Add("Class");
            dtSchedule.Columns.Add("Subject");
            dtSchedule.Columns.Add("Teacher");
            dtSchedule.Columns.Add("Day");
            dtSchedule.Columns.Add("Time");
            dtSchedule.Columns.Add("Room");
            
            // Sample Assignments
            dtSchedule.Rows.Add("BSc IT-1", "Web Development", "Dr. Rajesh Kumar", "Monday", "09:00-10:00", "Lab-301");
            dtSchedule.Rows.Add("BSc IT-2", "Software Engg", "Prof. Meena Iyer", "Monday", "10:00-11:00", "Room-405");
            dtSchedule.Rows.Add("BSc CS-1", "C++ Programming", "Prof. Anita Sharma", "Tuesday", "09:00-10:00", "Lab-302");
            dtSchedule.Rows.Add("BSc DS-1", "Statistics", "Dr. Sneha Gupta", "Wednesday", "11:00-12:00", "Room-101");
            dtSchedule.Rows.Add("BSc CS-2", "Java Programming", "Dr. Vikram Singh", "Thursday", "10:00-11:00", "Room-201");
            dtSchedule.Rows.Add("BSc DS-2", "Data Science", "Mr. Sunil Gavaskar", "Friday", "09:00-10:00", "Lab-202");
            dtSchedule.Rows.Add("BSc IT-1", "Soft Skills", "Dr. Aditi Rao", "Monday", "11:00-12:00", "Room-306");
            
            // Infrastructure Data
            DataTable dtRooms = new DataTable();
            dtRooms.Columns.Add("RoomNo");
            dtRooms.Columns.Add("Type");
            dtRooms.Columns.Add("Capacity");
            dtRooms.Columns.Add("Status");
            dtRooms.Rows.Add("Lab-301", "Computer Lab", "45", "✅ AVAILABLE");
            dtRooms.Rows.Add("Lab-302", "Hardware Lab", "30", "✅ AVAILABLE");
            dtRooms.Rows.Add("Room-101", "Smart Class", "60", "✅ AVAILABLE");
            dtRooms.Rows.Add("Room-201", "Lecture Hall", "120", "✅ AVAILABLE");
            dtRooms.Rows.Add("Lab-202", "Data Science Lab", "40", "✅ AVAILABLE");
            dtRooms.Rows.Add("Room-405", "Classroom", "50", "✅ AVAILABLE");
            dtRooms.Rows.Add("Room-306", "Seminar Room", "80", "✅ AVAILABLE");
            this.Tag = dtRooms;
            
            // Add a sample conflict for demonstration
            dtSchedule.Rows.Add("BSc IT-3", "Cyber Security", "Dr. Rajesh Kumar", "Monday", "09:00-10:00", "Room-102");
        }

        private void SetupLayout()
        {
            this.Controls.Clear();
            this.BackColor = bgColor;
            this.Dock = DockStyle.Fill;

            // Root Layout
            TableLayoutPanel rootLayout = new TableLayoutPanel();
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 140F)); // Header
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Body
            this.Controls.Add(rootLayout);

            // Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 140, BackColor = Color.White, Padding = new Padding(30, 25, 30, 0) };
            
            FlowLayoutPanel tlpHead = new FlowLayoutPanel() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = true };
            
            Label lblTitle = new Label() { Text = "🗓️ MASTER TIMETABLE", Font = new Font("Segoe UI", 22, FontStyle.Bold), ForeColor = primaryColor, AutoSize = true, Margin = new Padding(0, 0, 0, 5) };
            Label lblSubtitle = new Label() { Text = "Optimize faculty assignments and monitor daily academic schedules", Font = new Font("Segoe UI", 11), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(4, 0, 0, 0) };
            
            tlpHead.Controls.Add(lblTitle);
            tlpHead.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(tlpHead);

            Panel accent = new Panel() { Dock = DockStyle.Bottom, Height = 5, BackColor = primaryColor };
            pnlHeader.Controls.Add(accent);
            rootLayout.Controls.Add(pnlHeader, 0, 0);

            // Body
            Panel pnlBody = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, BackColor = bgColor, Padding = new Padding(30) };
            rootLayout.Controls.Add(pnlBody, 0, 1);

            TableLayoutPanel tlpMain = new TableLayoutPanel() { 
                Dock = DockStyle.Top, 
                ColumnCount = 1, 
                RowCount = 4,
                AutoSize = true,
                Padding = new Padding(0, 0, 0, 80) // Increased bottom padding
            };
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F)); // 1. Stats (Increased)
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F)); // 2. Toolbar (Increased)
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 550F)); // 3. Schedule Grid
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 550F)); // 4. Room Monitoring 
            pnlBody.Controls.Add(tlpMain);

            // 1. STATISTICS CARDS
            flpStats = new FlowLayoutPanel() { 
                Dock = DockStyle.Fill, 
                WrapContents = true, 
                AutoScroll = true,
                Padding = new Padding(0, 5, 0, 20) // Increased bottom padding
            };
            UpdateStats();
            tlpMain.Controls.Add(flpStats, 0, 0);

            // 2. TOOLBAR
            Panel pnlToolbar = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(20, 10, 20, 10), BackColor = Color.FromArgb(245, 245, 245) };
            Button btnExport = new Button() { 
                Text = "📥 EXPORT", 
                Width = 120, 
                Height = 36, 
                BackColor = Color.FromArgb(52, 152, 219), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(720, 32)
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += (s, e) => MessageBox.Show("Exporting timetable to Excel...", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            Button btnAddAssignment = new Button() { 
                Text = "+ ASSIGN TEACHER", 
                Width = 180, 
                Height = 36, 
                BackColor = Color.FromArgb(46, 204, 113), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(0, 32)
            };
            btnAddAssignment.FlatAppearance.BorderSize = 0;
            btnAddAssignment.Click += (s, e) => ShowAssignmentForm(null);
            
            pnlToolbar.Controls.AddRange(new Control[] { btnAddAssignment, btnExport });
            
            Label lblFilter = new Label() { 
                Text = "SELECT TEACHER", 
                ForeColor = Color.FromArgb(173, 22, 37), 
                Location = new Point(450, 10), 
                AutoSize = true, 
                Font = new Font("Segoe UI", 9, FontStyle.Bold) 
            };
            ComboBox cmbFilterTeacher = new ComboBox() { 
                Location = new Point(450, 32), 
                Width = 220, 
                BackColor = Color.White, 
                ForeColor = Color.RoyalBlue, 
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10) 
            };
            // Add border
            cmbFilterTeacher.Paint += (s, e) => {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(180, 180, 180), 1), 0, 0, cmbFilterTeacher.Width - 1, cmbFilterTeacher.Height - 1);
            };
            cmbFilterTeacher.Items.Add("All Teachers");
            foreach(DataRow r in dtTeachers.Rows) cmbFilterTeacher.Items.Add(r["Name"].ToString());
            cmbFilterTeacher.SelectedIndex = 0;
            cmbFilterTeacher.SelectedIndexChanged += (s, e) => {
                DataView dv = dtSchedule.DefaultView;
                if (cmbFilterTeacher.SelectedIndex == 0) dv.RowFilter = "";
                else dv.RowFilter = $"Teacher = '{cmbFilterTeacher.Text.Replace("'", "''")}'";
                UpdateStats();
            };
            
            pnlToolbar.Controls.Add(lblFilter);
            pnlToolbar.Controls.Add(cmbFilterTeacher);
            
            tlpMain.Controls.Add(pnlToolbar, 0, 1);

            // 3. SCHEDULE GRID
            Panel pnlSchedule = new Panel() { Dock = DockStyle.Fill, BackColor = cardBg, Padding = new Padding(15) };
            Label lblScheduleTitle = new Label() { 
                Text = "CLASS SCHEDULE & ASSIGNMENTS", 
                Dock = DockStyle.Top, 
                Height = 40, 
                Font = new Font("Segoe UI", 12, FontStyle.Bold), 
                ForeColor = Color.FromArgb(173, 22, 37) 
            };
            
            DataGridView dgvSchedule = new DataGridView() { 
                Dock = DockStyle.Fill, 
                DataSource = dtSchedule,
                BackgroundColor = Color.White, 
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false, 
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 55, 
                AllowUserToAddRows = false,
                RowHeadersVisible = false, 
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, 
                RowTemplate = { Height = 45 }
            };
            
            dgvSchedule.DefaultCellStyle.BackColor = Color.White;
            dgvSchedule.DefaultCellStyle.ForeColor = Color.RoyalBlue;
            dgvSchedule.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvSchedule.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(173, 22, 37);
            dgvSchedule.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSchedule.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvSchedule.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            // Add action buttons
            DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn() { 
                Name = "Edit", 
                HeaderText = "", 
                Text = "✏️", 
                UseColumnTextForButtonValue = true, 
                Width = 45 
            };
            btnEdit.DefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            btnEdit.DefaultCellStyle.ForeColor = Color.White;
            dgvSchedule.Columns.Add(btnEdit);

            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn() { 
                Name = "Delete", 
                HeaderText = "", 
                Text = "🗑️", 
                UseColumnTextForButtonValue = true, 
                Width = 45 
            };
            btnDelete.DefaultCellStyle.BackColor = Color.FromArgb(231, 76, 60);
            btnDelete.DefaultCellStyle.ForeColor = Color.White;
            dgvSchedule.Columns.Add(btnDelete);

            dgvSchedule.CellFormatting += (s, e) => {
                if (e.RowIndex < 0) return;
                DataRowView drv = (DataRowView)dgvSchedule.Rows[e.RowIndex].DataBoundItem;
                if (drv.Row.Table.Columns.Contains("_Conflict") && drv.Row["_Conflict"] != DBNull.Value && (bool)drv.Row["_Conflict"])
                {
                    e.CellStyle.BackColor = Color.FromArgb(192, 57, 43); // Dark Red
                    e.CellStyle.SelectionBackColor = Color.FromArgb(231, 76, 60);
                }
            };

            dgvSchedule.CellClick += (s, e) => {
                if(e.RowIndex < 0) return;
                string colName = dgvSchedule.Columns[e.ColumnIndex].Name;
                DataRowView drv = (DataRowView)dgvSchedule.Rows[e.RowIndex].DataBoundItem;
                
                if (colName == "Edit") ShowAssignmentForm(drv.Row);
                if (colName == "Delete") DeleteAssignment(drv.Row);
            };

            pnlSchedule.Controls.Add(lblScheduleTitle);
            pnlSchedule.Controls.Add(dgvSchedule);
            dgvSchedule.BringToFront(); 
            pnlSchedule.Padding = new Padding(15, 30, 15, 15); // Top padding for gap
            tlpMain.Controls.Add(pnlSchedule, 0, 2);

            // 4. CAMPUS RESOURCE & ROOM MONITORING
            Panel pnlRoomSect = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(15), Margin = new Padding(0, 30, 0, 0) };
            pnlRoomSect.Paint += (s, e) => {
                using (Pen pen = new Pen(Color.FromArgb(230, 230, 230), 1)) {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlRoomSect.Width - 1, pnlRoomSect.Height - 1);
                }
            };

            Label lblRoomTitle = new Label() { 
                Text = "🏙️ INFRASTRUCTURE & ROOM OCCUPANCY TRACKER", 
                Dock = DockStyle.Top, 
                Height = 35, 
                Font = new Font("Segoe UI", 12, FontStyle.Bold), 
                ForeColor = primaryColor 
            };

            DataGridView dgvRooms = new DataGridView() { 
                Dock = DockStyle.Fill, 
                DataSource = (DataTable)this.Tag,
                BackgroundColor = Color.White, 
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 40 },
                ColumnHeadersHeight = 45
            };
            dgvRooms.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
            dgvRooms.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRooms.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            dgvRooms.DataBindingComplete += (s, e) => {
                if (!dgvRooms.Columns.Contains("Status")) {
                    dgvRooms.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Status", HeaderText = "Live Status", Width = 150 });
                    dgvRooms.Columns.Add(new DataGridViewTextBoxColumn() { Name = "ActiveSub", HeaderText = "Scheduled Subject", Width = 200 });
                }
            };

            dgvRooms.CellFormatting += (s, e) => {
                if (e.RowIndex < 0) return;
                string roomNo = dgvRooms.Rows[e.RowIndex].Cells["RoomNo"].Value?.ToString();
                if (string.IsNullOrEmpty(roomNo)) return;

                DataRow assigned = dtSchedule.AsEnumerable().FirstOrDefault(r => r["Room"].ToString() == roomNo);

                if (dgvRooms.Columns[e.ColumnIndex].Name == "Status") {
                    if (assigned != null) { e.Value = "🛑 BUSY"; e.CellStyle.ForeColor = Color.FromArgb(192, 57, 43); }
                    else { e.Value = "✅ AVAILABLE"; e.CellStyle.ForeColor = Color.FromArgb(39, 174, 96); }
                    e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }
                if (dgvRooms.Columns[e.ColumnIndex].Name == "ActiveSub") {
                    e.Value = assigned != null ? assigned["Subject"].ToString() + " (" + assigned["Class"].ToString() + ")" : "---";
                    e.CellStyle.ForeColor = Color.FromArgb(100, 100, 100);
                }
            };

            pnlRoomSect.Controls.Add(dgvRooms);
            pnlRoomSect.Controls.Add(lblRoomTitle);
            dgvRooms.BringToFront();
            pnlRoomSect.Padding = new Padding(15, 40, 15, 15); // Top padding for gap from grid above
            tlpMain.Controls.Add(pnlRoomSect, 0, 3);

            UpdateTeacherLoad();
        }

        private void UpdateStats()
        {
            if (flpStats == null) return;
            flpStats.Controls.Clear();
            
            DataView dv = dtSchedule.DefaultView;
            int totalAssignments = dv.Count;
            int uniqueClasses = dv.ToTable(true, "Class").Rows.Count;
            int conflictCount = dv.Cast<DataRowView>().Count(drv => drv.Row.Table.Columns.Contains("_Conflict") && drv.Row["_Conflict"] != DBNull.Value && (bool)drv.Row["_Conflict"]);
            
            // For these, we still want global context unless filtering
            int totalTeachers = dtTeachers.Rows.Count;
            int assignedTeachers = dtSchedule.AsEnumerable().Select(r => r["Teacher"].ToString()).Distinct().Count();
            int unassignedTeachers = totalTeachers - assignedTeachers;

            flpStats.Controls.Add(CreateStatCard(dv.RowFilter == "" ? "TOTAL ASSIGNMENTS" : "TEACHER SLOT COUNT", totalAssignments.ToString(), Color.FromArgb(52, 152, 219)));
            flpStats.Controls.Add(CreateStatCard("CONFLICTS", conflictCount.ToString(), conflictCount > 0 ? Color.FromArgb(231, 76, 60) : Color.FromArgb(46, 204, 113)));
            flpStats.Controls.Add(CreateStatCard("CLASSES COVERED", uniqueClasses.ToString(), Color.FromArgb(46, 204, 113)));
            flpStats.Controls.Add(CreateStatCard("UNASSIGNED TEACHERS", unassignedTeachers.ToString(), Color.FromArgb(241, 196, 15)));
        }

        private Panel CreateStatCard(string title, string value, Color color)
        {
            Panel p = new Panel() { Width = 260, Height = 120, BackColor = cardBg, Margin = new Padding(0, 0, 25, 10) };
            p.Paint += (s, e) => ControlPaint.DrawBorder(e.Graphics, p.ClientRectangle, Color.FromArgb(240, 240, 240), ButtonBorderStyle.Solid);
            
            Panel bar = new Panel() { Dock = DockStyle.Top, Height = 5, BackColor = color };
            
            TableLayoutPanel tlp = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = new Padding(15, 10, 15, 10) };
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.Gray, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            Label lblV = new Label() { Text = value, Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.RoyalBlue, Dock = DockStyle.Fill, TextAlign = ContentAlignment.BottomLeft };
            
            tlp.Controls.Add(lblT, 0, 0);
            tlp.Controls.Add(lblV, 0, 1);
            
            p.Controls.Add(tlp);
            p.Controls.Add(bar);
            return p;
        }

        private void ShowAssignmentForm(DataRow row)
        {
            bool isEdit = (row != null);
            
            Form formAssign = new Form() { 
                Text = isEdit ? "Edit Assignment" : "New Teacher Assignment", 
                Size = new Size(500, 450), 
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.White,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            int y = 20;
            
            Label lblClass = new Label() { Text = "Class/Division *", ForeColor = Color.FromArgb(173, 22, 37), Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            TextBox txtClass = new TextBox() { 
                Text = isEdit ? row["Class"].ToString() : "", 
                Location = new Point(20, y + 25), 
                Width = 440, 
                BackColor = Color.White, 
                ForeColor = Color.RoyalBlue, 
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10) 
            };
            y += 70;

            Label lblSubject = new Label() { Text = "Subject *", ForeColor = Color.FromArgb(173, 22, 37), Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            TextBox txtSubject = new TextBox() { 
                Text = isEdit ? row["Subject"].ToString() : "", 
                Location = new Point(20, y + 25), 
                Width = 440, 
                BackColor = Color.White, 
                ForeColor = Color.RoyalBlue, 
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10) 
            };
            y += 70;

            Label lblTeacher = new Label() { Text = "Teacher *", ForeColor = Color.FromArgb(173, 22, 37), Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            ComboBox cmbTeacher = new ComboBox() { 
                Location = new Point(20, y + 25), 
                Width = 440, 
                BackColor = Color.White, 
                ForeColor = Color.RoyalBlue, 
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10) 
            };
            foreach(DataRow r in dtTeachers.Rows) 
                cmbTeacher.Items.Add(r["Name"].ToString());
            if(isEdit) cmbTeacher.Text = row["Teacher"].ToString();
            y += 70;

            Label lblDay = new Label() { Text = "Day *", ForeColor = Color.FromArgb(173, 22, 37), Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            ComboBox cmbDay = new ComboBox() { 
                Location = new Point(20, y + 25), 
                Width = 200, 
                BackColor = Color.White, 
                ForeColor = Color.RoyalBlue, 
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10) 
            };
            cmbDay.Items.AddRange(new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" });
            if(isEdit) cmbDay.Text = row["Day"].ToString();
            
            Label lblTime = new Label() { Text = "Time *", ForeColor = Color.FromArgb(173, 22, 37), Location = new Point(240, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            TextBox txtTime = new TextBox() { 
                Text = isEdit ? row["Time"].ToString() : "09:00-10:00", 
                Location = new Point(240, y + 25), 
                Width = 220, 
                BackColor = Color.White, 
                ForeColor = Color.RoyalBlue, 
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10) 
            };
            y += 70;

            Label lblRoom = new Label() { Text = "Room/Lab", ForeColor = Color.FromArgb(173, 22, 37), Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            TextBox txtRoom = new TextBox() { 
                Text = isEdit ? row["Room"].ToString() : "", 
                Location = new Point(20, y + 25), 
                Width = 440, 
                BackColor = Color.White, 
                ForeColor = Color.RoyalBlue, 
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10) 
            };
            y += 70;

            Button btnSave = new Button() { 
                Text = isEdit ? "UPDATE" : "ASSIGN", 
                Location = new Point(360, y), 
                Width = 100, 
                Height = 35, 
                BackColor = Color.FromArgb(46, 204, 113), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) => {
                string teacher = cmbTeacher.Text;
                string day = cmbDay.Text;
                string time = txtTime.Text;
                string room = txtRoom.Text;

                // Simple check for conflicts before saving
                var conflicts = dtSchedule.AsEnumerable().Where(r => 
                    r != row && // Don't check against self if editing
                    r["Day"].ToString() == day && 
                    r["Time"].ToString() == time && 
                    (r["Teacher"].ToString() == teacher || r["Room"].ToString() == room)
                );

                if (conflicts.Any())
                {
                    var first = conflicts.First();
                    string reason = first["Teacher"].ToString() == teacher ? "Teacher busy" : "Room occupied";
                    var res = MessageBox.Show($"Conflict Detected: {reason}!\nExisting: {first["Class"]} with {first["Teacher"]} in {first["Room"]}\n\nDo you want to proceed anyway?", "Timing Conflict", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res == DialogResult.No) return;
                }

                if(isEdit) {
                    row["Class"] = txtClass.Text;
                    row["Subject"] = txtSubject.Text;
                    row["Teacher"] = cmbTeacher.Text;
                    row["Day"] = cmbDay.Text;
                    row["Time"] = txtTime.Text;
                    row["Room"] = txtRoom.Text;
                    MessageBox.Show("Assignment updated successfully!", "Success");
                } else {
                    dtSchedule.Rows.Add(txtClass.Text, txtSubject.Text, cmbTeacher.Text, cmbDay.Text, txtTime.Text, txtRoom.Text);
                    MessageBox.Show("Teacher assigned successfully!", "Success");
                }
                CheckAllConflicts();
                UpdateTeacherLoad();
                UpdateStats();
                formAssign.Close();
            };

            formAssign.Controls.AddRange(new Control[] { 
                lblClass, txtClass, lblSubject, txtSubject, lblTeacher, cmbTeacher, 
                lblDay, cmbDay, lblTime, txtTime, lblRoom, txtRoom, btnSave 
            });
            formAssign.ShowDialog();
        }

        private void DeleteAssignment(DataRow row)
        {
            var result = MessageBox.Show(
                $"Remove assignment?\n\nClass: {row["Class"]}\nSubject: {row["Subject"]}\nTeacher: {row["Teacher"]}", 
                "Confirm Delete", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Warning
            );
            
            if(result == DialogResult.Yes) {
                dtSchedule.Rows.Remove(row);
                CheckAllConflicts();
                UpdateTeacherLoad();
                UpdateStats();
                MessageBox.Show("Assignment removed successfully!", "Success");
            }
        }

        private void UpdateTeacherLoad()
        {
            if (!dtTeachers.Columns.Contains("LoadHours"))
                dtTeachers.Columns.Add("LoadHours", typeof(int));

            foreach (DataRow row in dtTeachers.Rows)
            {
                string name = row["Name"].ToString();
                int count = dtSchedule.AsEnumerable().Count(r => r["Teacher"].ToString() == name);
                row["LoadHours"] = count * 10; // Simple math: each slot = 10% load
            }
        }

        private void CheckAllConflicts()
        {
            if (!dtSchedule.Columns.Contains("_Conflict"))
                dtSchedule.Columns.Add("_Conflict", typeof(bool));

            foreach (DataRow row in dtSchedule.Rows)
            {
                string teacher = row["Teacher"].ToString();
                string day = row["Day"].ToString();
                string time = row["Time"].ToString();
                string room = row["Room"].ToString();

                bool hasConflict = dtSchedule.AsEnumerable().Any(r => 
                    r != row && 
                    r["Day"].ToString() == day && 
                    r["Time"].ToString() == time && 
                    (r["Teacher"].ToString() == teacher || r["Room"].ToString() == room)
                );
                
                row["_Conflict"] = hasConflict;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "AdminTimetableControl";
            this.Size = new Size(1200, 800);
            this.ResumeLayout(false);
        }
    }

    // Custom Column for DataGridView to show progress bars
    public class DataGridViewProgressColumn : DataGridViewImageColumn
    {
        public DataGridViewProgressColumn()
        {
            CellTemplate = new DataGridViewProgressCell();
        }
    }

    public class DataGridViewProgressCell : DataGridViewImageCell
    {
        static Image emptyImage;
        static DataGridViewProgressCell()
        {
            emptyImage = new Bitmap(1, 1);
            ((Bitmap)emptyImage).SetPixel(0, 0, Color.Transparent);
        }

        public override object DefaultNewRowValue => emptyImage;

        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, System.ComponentModel.TypeConverter valueTypeConverter, System.ComponentModel.TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            return emptyImage;
        }

        protected override void Paint(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            if (value == null || value == DBNull.Value) value = 0;
            int progressVal = (int)value;
            float percentage = (float)progressVal / 100f;
            if (percentage > 1) percentage = 1;

            // Draw Background
            g.FillRectangle(new SolidBrush(cellStyle.BackColor), cellBounds);

            // Calculate progress bar bounds
            int margin = 5;
            Rectangle progressRect = new Rectangle(cellBounds.X + margin, cellBounds.Y + margin, cellBounds.Width - (margin * 2), cellBounds.Height - (margin * 2));
            
            // Draw Progress BG
            g.FillRectangle(new SolidBrush(Color.FromArgb(230, 230, 230)), progressRect);

            // Draw Progress Fill
            Color barColor = percentage > 0.8 ? Color.FromArgb(231, 76, 60) : (percentage > 0.5 ? Color.FromArgb(241, 196, 15) : Color.FromArgb(46, 204, 113));
            g.FillRectangle(new SolidBrush(barColor), new Rectangle(progressRect.X, progressRect.Y, (int)(progressRect.Width * percentage), progressRect.Height));

            // Draw Text
            string text = $"{progressVal}% Load";
            TextRenderer.DrawText(g, text, cellStyle.Font, progressRect, Color.RoyalBlue, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
