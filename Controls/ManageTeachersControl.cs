using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public partial class ManageTeachersControl : UserControl
    {
        // DATA
        private DataTable dtTeachers;
        private DataView dvTeachers;

        // UI CONTAINERS
        private Panel pnlMainContainer;

        // FORM GLOBALS
        private TextBox txtName, txtEmpID, txtEmail, txtPhone;
        private ComboBox cmbDept, cmbDesg, cmbStatus;
        private Dictionary<string, CheckBox> roleCheckboxes;
        private DataRow currentRow = null; // null = Add Mode, non-null = Edit Mode

        // FILTERS
        private TextBox txtSearch;
        private ComboBox cmbFilterDept, cmbFilterDesg, cmbFilterStatus;

        public ManageTeachersControl()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(15, 15, 15);
            InitializeMockData();
            SetupBaseLayout();
            ShowDashboard();
        }

        // ---------------------------------------------------------
        // 1. DATA LAYER
        // ---------------------------------------------------------
        private void InitializeMockData()
        {
            dtTeachers = new DataTable();
            dtTeachers.Columns.Add("Name");
            dtTeachers.Columns.Add("EmployeeID");
            dtTeachers.Columns.Add("Department");
            dtTeachers.Columns.Add("Designation");
            dtTeachers.Columns.Add("Roles");
            dtTeachers.Columns.Add("Status");
            dtTeachers.Columns.Add("Email"); // Hidden
            dtTeachers.Columns.Add("Phone"); // Hidden

            // Mock Data
            dtTeachers.Rows.Add("Dr. Rajesh Kumar", "EMP-001", "CSE", "Professor", "HOD", "Active", "rajesh.k@univ.edu", "9876543210");
            dtTeachers.Rows.Add("Prof. Anita Sharma", "EMP-002", "ECE", "Associate Prof", "TPO", "Active", "anita.s@univ.edu", "9876543211");
            dtTeachers.Rows.Add("Mr. Amit Verma", "EMP-003", "Mechanical", "Assistant Prof", "Course Coordinator", "Inactive", "amit.v@univ.edu", "9876543212");
            dtTeachers.Rows.Add("Dr. Sneha Gupta", "EMP-004", "CSE", "Professor", "Dean", "Active", "sneha.g@univ.edu", "9876543213");
            dtTeachers.Rows.Add("Ms. Priya Singh", "EMP-005", "BSH", "Lecturer", "Warden", "Active", "priya.s@univ.edu", "9876543214");
            dtTeachers.Rows.Add("Dr. Vikram Singh", "EMP-006", "CSE", "Associate Prof", "Exam Cell", "Active", "vikram.s@univ.edu", "9876543215");
            dtTeachers.Rows.Add("Prof. Meena Iyer", "EMP-007", "ECE", "Professor", "HOD", "Active", "meena.i@univ.edu", "9876543216");
            dtTeachers.Rows.Add("Mr. Rohan Kapur", "EMP-008", "Civil", "Assistant Prof", "Sports Officer", "Active", "rohan.k@univ.edu", "9876543217");
            dtTeachers.Rows.Add("Dr. Aditi Rao", "EMP-009", "BSH", "Assistant Prof", "Cultural Head", "Active", "aditi.r@univ.edu", "9876543218");
            dtTeachers.Rows.Add("Prof. Suresh Raina", "EMP-010", "CSE", "Assistant Prof", "Placement Coordinator", "Inactive", "suresh.r@univ.edu", "9876543219");
            dtTeachers.Rows.Add("Dr. Neha Kakkar", "EMP-011", "ECE", "Lecturer", "NSS Officer", "Active", "neha.k@univ.edu", "9876543220");
            dtTeachers.Rows.Add("Mr. Sunil Gavaskar", "EMP-012", "Mechanical", "Professor", "Proctor", "Active", "sunil.g@univ.edu", "9876543221");

            dvTeachers = new DataView(dtTeachers);
        }

        // ---------------------------------------------------------
        // 2. BASE LAYOUT
        // ---------------------------------------------------------
        private void SetupBaseLayout()
        {
            this.Controls.Clear();
            
            TableLayoutPanel master = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, BackColor = Color.FromArgb(28, 40, 51) }; // #1C2833
            master.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
            master.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            this.Controls.Add(master);

            // HEADER
            Panel pnlHeader = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(44, 62, 80) }; // #2C3E50
            Label lblTitle = new Label() { Text = "TEACHER LIST & ROLE MANAGEMENT", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(20, 0, 0, 0) };
            pnlHeader.Controls.Add(lblTitle);
            master.Controls.Add(pnlHeader, 0, 0);

            // CONTENT
            pnlMainContainer = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(20) };
            master.Controls.Add(pnlMainContainer, 0, 1);
        }

        // ---------------------------------------------------------
        // 3. FEATURE: DASHBOARD & LIST
        // ---------------------------------------------------------
        private void ShowDashboard()
        {
            pnlMainContainer.Controls.Clear();

            TableLayoutPanel tlp = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 110)); // Stats
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));  // Toolbar
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100));  // Grid
            pnlMainContainer.Controls.Add(tlp);

            // --- FEATURE 2: QUICK STATS CARDS ---
            FlowLayoutPanel flpStats = new FlowLayoutPanel() { Dock = DockStyle.Fill, WrapContents = false };
            UpdateStats(flpStats);
            tlp.Controls.Add(flpStats, 0, 0);

            // --- FEATURE 3 & 8 & 9: TOOLBAR (Search, Filter, Export, Add) ---
            TableLayoutPanel tlpToolbar = new TableLayoutPanel() { 
                Dock = DockStyle.Fill, 
                ColumnCount = 2, 
                RowCount = 1,
                Padding = new Padding(0, 10, 0, 10),
                BackColor = Color.FromArgb(15, 15, 15) // Solid match for dashboard
            };
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f)); // Left: Filters
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f)); // Right: Actions
            
            // Left: Search & Filters
            FlowLayoutPanel flpFilters = new FlowLayoutPanel() { Dock = DockStyle.Fill, WrapContents = false, AutoSize = true };
            
            // Search
            txtSearch = new TextBox() { Width = 180, Height = 30, Font = new Font("Segoe UI", 10), Tag = "Search teachers...", BackColor = Color.FromArgb(45, 45, 48), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            SetPlaceholder(txtSearch);
            txtSearch.TextChanged += FilterData;
            
            // Filters
            cmbFilterDept = CreateFilterCombo("Dept", new string[] { "CSE", "ECE", "Mechanical", "BSH" });
            cmbFilterDesg = CreateFilterCombo("Desg", new string[] { "Professor", "Associate Prof", "Assistant Prof", "Lecturer" });
            cmbFilterStatus = CreateFilterCombo("Status", new string[] { "Active", "Inactive" });

            Button btnClear = new Button() { Text = "↺", Width = 35, Height = 28, FlatStyle = FlatStyle.Flat, ForeColor = Color.LightGray, BackColor = Color.FromArgb(45,45,48), Cursor = Cursors.Hand };
            btnClear.Click += (s, e) => { txtSearch.Text = ""; cmbFilterDept.SelectedIndex = 0; cmbFilterDesg.SelectedIndex = 0; cmbFilterStatus.SelectedIndex = 0; SetPlaceholder(txtSearch); };

            flpFilters.Controls.AddRange(new Control[] { txtSearch, cmbFilterDept, cmbFilterDesg, cmbFilterStatus, btnClear });

            // Right: Actions
            FlowLayoutPanel flpActions = new FlowLayoutPanel() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, WrapContents = false };
            
            Button btnAdd = new Button() { Text = "+ Add Teacher", Width = 130, Height = 32, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand, Margin = new Padding(5, 0, 0, 0) };
            btnAdd.Click += (s, e) => ShowAddEditForm(null);

            Button btnExport = new Button() { Text = "📥 Export", Width = 100, Height = 32, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand, Margin = new Padding(5, 0, 0, 0) };
            btnExport.Click += (s, e) => MessageBox.Show("Exporting data to Excel...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            flpActions.Controls.AddRange(new Control[] { btnAdd, btnExport });

            tlpToolbar.Controls.Add(flpFilters, 0, 0);
            tlpToolbar.Controls.Add(flpActions, 1, 0);
            tlp.Controls.Add(tlpToolbar, 0, 1);

            // --- FEATURE 1: TABLE ---
            DataGridView dgv = new DataGridView() { 
                Dock = DockStyle.Fill, DataSource = dvTeachers,
                BackgroundColor = Color.FromArgb(44, 62, 80), BorderStyle = BorderStyle.None, // #2C3E50
                EnableHeadersVisualStyles = false, ColumnHeadersHeight = 45, AllowUserToAddRows = false,
                RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, RowTemplate = { Height = 40 }
            };
            
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94); // #34495E
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 40, 51); // #1C2833
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80); // #2C3E50

            // Action Buttons Columns
            AddGridButton(dgv, "Edit", "✏️", Color.FromArgb(52, 152, 219));
            AddGridButton(dgv, "Delete", "🗑️", Color.FromArgb(231, 76, 60));

            // Set Specific Column Widths
            dgv.DataBindingComplete += (s, e) => {
                if (dgv.Columns.Count > 0)
                {
                    // Hide long/internal columns
                    if (dgv.Columns.Contains("Email")) dgv.Columns["Email"].Visible = false;
                    if (dgv.Columns.Contains("Phone")) dgv.Columns["Phone"].Visible = false;

                    // Adjust Weights for visible columns
                    if (dgv.Columns.Contains("Name")) { dgv.Columns["Name"].FillWeight = 150; dgv.Columns["Name"].MinimumWidth = 150; }
                    if (dgv.Columns.Contains("EmployeeID")) { dgv.Columns["EmployeeID"].FillWeight = 85; dgv.Columns["EmployeeID"].MinimumWidth = 85; }
                    if (dgv.Columns.Contains("Department")) { dgv.Columns["Department"].FillWeight = 85; dgv.Columns["Department"].MinimumWidth = 85; }
                    if (dgv.Columns.Contains("Designation")) { dgv.Columns["Designation"].FillWeight = 110; dgv.Columns["Designation"].MinimumWidth = 110; }
                    if (dgv.Columns.Contains("Roles")) { dgv.Columns["Roles"].FillWeight = 160; dgv.Columns["Roles"].MinimumWidth = 120; }
                    if (dgv.Columns.Contains("Status")) { dgv.Columns["Status"].FillWeight = 80; dgv.Columns["Status"].MinimumWidth = 80; }
                    
                    // Fixed width for button columns
                    if (dgv.Columns.Contains("Edit")) dgv.Columns["Edit"].Width = 45;
                    if (dgv.Columns.Contains("Delete")) dgv.Columns["Delete"].Width = 45;
                }
            };

            dgv.CellClick += (s, e) => {
                if(e.RowIndex < 0) return;
                string colName = dgv.Columns[e.ColumnIndex].Name;
                DataRowView drv = (DataRowView)dgv.Rows[e.RowIndex].DataBoundItem;
                DataRow row = drv.Row;

                if (colName == "Edit") ShowAddEditForm(row);
                if (colName == "Delete") ConfirmDelete(row);
            };

            tlp.Controls.Add(dgv, 0, 2);
        }

        // ---------------------------------------------------------
        // 4. FEATURE: ADD / EDIT / ASSIGN ROLES
        // ---------------------------------------------------------
        private void ShowAddEditForm(DataRow row)
        {
            currentRow = row;
            bool isEdit = (row != null);
            pnlMainContainer.Controls.Clear();

            // Split Layout
            TableLayoutPanel tlpForm = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            pnlMainContainer.Controls.Add(tlpForm);

            // LEFT: BASIC INFO
            Panel pnlL = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(30, 30, 32), Padding = new Padding(20) };
            Label lblL = new Label() { Text = isEdit ? "EDIT TEACHER DETAILS" : "NEW TEACHER PROFILE", Dock = DockStyle.Top, Height = 40, Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.White };
            Panel pnlFields = new Panel() { Dock = DockStyle.Fill, AutoScroll = true };
            
            int y = 0;
            txtName = CreateInput(pnlFields, "Full Name *", isEdit ? row["Name"].ToString() : "", ref y);
            txtEmpID = CreateInput(pnlFields, "Employee ID *", isEdit ? row["EmployeeID"].ToString() : "", ref y);
            txtEmail = CreateInput(pnlFields, "Email Address *", isEdit ? row["Email"].ToString() : "", ref y);
            txtPhone = CreateInput(pnlFields, "Phone", isEdit ? row["Phone"].ToString() : "", ref y);
            cmbDept = CreateDropdown(pnlFields, "Department *", new string[] { "CSE", "ECE", "Mechanical", "Civil", "BSH" }, isEdit ? row["Department"].ToString() : "CSE", ref y);
            cmbDesg = CreateDropdown(pnlFields, "Designation *", new string[] { "Professor", "Associate Prof", "Assistant Prof", "Lecturer" }, isEdit ? row["Designation"].ToString() : "Assistant Prof", ref y);
            cmbStatus = CreateDropdown(pnlFields, "Status", new string[] { "Active", "Inactive" }, isEdit ? row["Status"].ToString() : "Active", ref y);

            Button btnUpload = new Button() { Text = "📷 Upload Photo", Location = new Point(0, y), Width = 150, FlatStyle = FlatStyle.Flat, ForeColor = Color.LightBlue, AutoSize = true };
            pnlFields.Controls.Add(btnUpload);

            pnlL.Controls.Add(pnlFields);
            pnlL.Controls.Add(lblL);
            tlpForm.Controls.Add(pnlL, 0, 0);

            // RIGHT: ROLE ASSIGNMENT
            Panel pnlR = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(25, 25, 28), Padding = new Padding(20) };
            Label lblR = new Label() { Text = "ASSIGN ROLES", Dock = DockStyle.Top, Height = 40, Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.FromArgb(241, 196, 15) };
            FlowLayoutPanel flpRoles = new FlowLayoutPanel() { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false };
            
            roleCheckboxes = new Dictionary<string, CheckBox>();
            string currentRoles = isEdit ? row["Roles"].ToString() : "";

            AddRoleCategory(flpRoles, "ACADEMIC", new string[] { "Head of Department (HOD)", "Dean", "Course Coordinator" }, currentRoles);
            AddRoleCategory(flpRoles, "EXAMINATION", new string[] { "Controller of Examinations (COE)", "Examination Cell Member" }, currentRoles);
            AddRoleCategory(flpRoles, "PLACEMENT", new string[] { "Training & Placement Officer (TPO)", "Placement Coordinator" }, currentRoles);
            AddRoleCategory(flpRoles, "COMMITTEES", new string[] { "Sports Coordinator", "Cultural Coordinator" }, currentRoles);
            AddRoleCategory(flpRoles, "LIBRARY", new string[] { "Chief Librarian" }, currentRoles);

            // BUTTONS
            Panel pnlBtns = new Panel() { Dock = DockStyle.Bottom, Height = 60 };
            Button btnSave = new Button() { Text = isEdit ? "UPDATE & NOTIFY" : "SAVE & NOTIFY", Dock = DockStyle.Right, Width = 180, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnSave.Click += (s, e) => SaveData();
            Button btnCancel = new Button() { Text = "CANCEL", Dock = DockStyle.Left, Width = 100, ForeColor = Color.Gray, FlatStyle = FlatStyle.Flat };
            btnCancel.Click += (s, e) => ShowDashboard();

            pnlBtns.Controls.Add(btnSave);
            pnlBtns.Controls.Add(btnCancel);

            pnlR.Controls.Add(flpRoles);
            pnlR.Controls.Add(pnlBtns);
            pnlR.Controls.Add(lblR);
            tlpForm.Controls.Add(pnlR, 1, 0);
        }

        // ---------------------------------------------------------
        // LOGIC: SAVE / DELETE / FILTER
        // ---------------------------------------------------------
        private void SaveData()
        {
            List<string> roles = new List<string>();
            foreach (var kv in roleCheckboxes) if (kv.Value.Checked) roles.Add(kv.Key);
            string roleStr = string.Join(", ", roles);

            if (currentRow == null)
            {
                dtTeachers.Rows.Add(txtName.Text, txtEmpID.Text, cmbDept.Text, cmbDesg.Text, roleStr, cmbStatus.Text, txtEmail.Text, txtPhone.Text);
                MessageBox.Show("Teacher Added Successfully!", "Success");
            }
            else
            {
                currentRow["Name"] = txtName.Text;
                currentRow["EmployeeID"] = txtEmpID.Text;
                currentRow["Department"] = cmbDept.Text;
                currentRow["Designation"] = cmbDesg.Text;
                currentRow["Status"] = cmbStatus.Text;
                currentRow["Email"] = txtEmail.Text;
                currentRow["Phone"] = txtPhone.Text;
                currentRow["Roles"] = roleStr;
                MessageBox.Show("Teacher Updated Successfully!", "Success");
            }
            ShowDashboard();
        }

        private void ConfirmDelete(DataRow row)
        {
            var res = MessageBox.Show($"⚠️ Delete Teacher?\n\nAre you sure you want to delete {row["Name"]}?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                dtTeachers.Rows.Remove(row);
                ShowDashboard(); // Refresh UI/Stats
            }
        }

        private void FilterData(object sender, EventArgs e)
        {
            string q = txtSearch.Text;
            if (q == "Search teachers...") q = "";
            string filter = $"(Name LIKE '%{q}%' OR EmployeeID LIKE '%{q}%')";
            
            if (cmbFilterDept.SelectedIndex > 0) filter += $" AND Department = '{cmbFilterDept.Text}'";
            if (cmbFilterDesg.SelectedIndex > 0) filter += $" AND Designation = '{cmbFilterDesg.Text}'";
            if (cmbFilterStatus.SelectedIndex > 0) filter += $" AND Status = '{cmbFilterStatus.Text}'";

            dvTeachers.RowFilter = filter;
        }

        // ---------------------------------------------------------
        // HELPERS
        // ---------------------------------------------------------
        private void UpdateStats(FlowLayoutPanel flp)
        {
             flp.Controls.Clear();
             
             // Calculate dynamic stats from actual data
             int totalTeachers = dtTeachers.Rows.Count;
             int activeTeachers = 0, inactiveTeachers = 0;
             int hod = 0, tpo = 0, noRoles = 0;
             
             foreach(DataRow r in dtTeachers.Rows) {
                 string roles = r["Roles"].ToString();
                 string status = r["Status"].ToString();
                 
                 // Count status
                 if(status == "Active") activeTeachers++;
                 else inactiveTeachers++;
                 
                 // Count roles
                 if(roles.Contains("HOD")) hod++;
                 if(roles.Contains("TPO")) tpo++;
                 if(string.IsNullOrEmpty(roles)) noRoles++;
             }

             // Display KPI Cards
             flp.Controls.Add(CreateKpi("TOTAL TEACHERS", totalTeachers.ToString(), Color.FromArgb(52, 152, 219)));
             flp.Controls.Add(CreateKpi("ACTIVE", activeTeachers.ToString(), Color.FromArgb(46, 204, 113)));
             flp.Controls.Add(CreateKpi("INACTIVE", inactiveTeachers.ToString(), Color.FromArgb(231, 76, 60)));
             flp.Controls.Add(CreateKpi("HODs", hod.ToString(), Color.FromArgb(155, 89, 182)));
             flp.Controls.Add(CreateKpi("UNASSIGNED", noRoles.ToString(), Color.FromArgb(241, 196, 15)));
        }

        private Panel CreateKpi(string t, string v, Color c)
        {
            Panel p = new Panel() { Width = 240, Height = 95, BackColor = Color.FromArgb(52, 73, 94), Margin = new Padding(0, 0, 20, 0) }; // #34495E
            p.Controls.Add(new Panel() { Dock = DockStyle.Left, Width = 6, BackColor = c });
            
            Label lblVal = new Label() { Text = v, ForeColor = Color.White, Location = new Point(15, 35), AutoSize = true, Font = new Font("Segoe UI", 24, FontStyle.Bold) };
            Label lblTitle = new Label() { Text = t, ForeColor = Color.DarkGray, Location = new Point(15, 12), AutoSize = true, Font = new Font("Segoe UI", 8, FontStyle.Bold) };
            
            p.Controls.Add(lblVal);
            p.Controls.Add(lblTitle);
            return p;
        }

        private TextBox CreateInput(Panel p, string l, string v, ref int y)
        {
            p.Controls.Add(new Label() { Text = l, ForeColor = Color.Silver, Location = new Point(0, y), AutoSize = true });
            TextBox t = new TextBox() { Text = v, Location = new Point(0, y + 25), Width = 300, BackColor = Color.FromArgb(45, 45, 48), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10) };
            p.Controls.Add(t);
            y += 65;
            return t;
        }

        private ComboBox CreateDropdown(Panel p, string l, string[] i, string v, ref int y)
        {
            p.Controls.Add(new Label() { Text = l, ForeColor = Color.Silver, Location = new Point(0, y), AutoSize = true });
            ComboBox c = new ComboBox() { Text = v, Location = new Point(0, y + 25), Width = 300, BackColor = Color.FromArgb(45, 45, 48), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10) };
            c.Items.AddRange(i);
            p.Controls.Add(c);
            y += 65;
            return c;
        }

        private ComboBox CreateFilterCombo(string label, string[] items)
        {
            ComboBox c = new ComboBox() { Width = 140, Height = 30, BackColor = Color.FromArgb(40, 40, 40), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9), Margin = new Padding(5, 0, 5, 0) };
            c.Items.Add("Filter " + label);
            c.Items.AddRange(items);
            c.SelectedIndex = 0;
            c.SelectedIndexChanged += FilterData;
            return c;
        }

        private void SetPlaceholder(TextBox t) { 
            t.Text = t.Tag.ToString(); t.ForeColor = Color.Gray; 
            t.GotFocus += (s, e) => { if(t.Text == t.Tag.ToString()) { t.Text = ""; t.ForeColor = Color.White; } };
            t.LostFocus += (s, e) => { if(string.IsNullOrWhiteSpace(t.Text)) { t.Text = t.Tag.ToString(); t.ForeColor = Color.Gray; } };
        }

        private void AddGridButton(DataGridView d, string name, string text, Color color) {
            var btn = new DataGridViewButtonColumn() { Name = name, HeaderText = "", Text = text, UseColumnTextForButtonValue = true, FlatStyle = FlatStyle.Flat, Width = 50 };
            btn.DefaultCellStyle.BackColor = color;
            btn.DefaultCellStyle.ForeColor = Color.White;
            d.Columns.Add(btn);
        }
        
        private void AddRoleCategory(FlowLayoutPanel p, string cat, string[] roles, string existing) {
            p.Controls.Add(new Label() { Text = cat, ForeColor = Color.FromArgb(241, 196, 15), Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 15, 0, 5) });
            foreach(string r in roles) {
                CheckBox c = new CheckBox() { Text = r, ForeColor = Color.White, AutoSize = true, Margin = new Padding(10, 2, 0, 2) };
                if (existing.Contains(r)) c.Checked = true;
                roleCheckboxes[r] = c;
                p.Controls.Add(c);
            }
        }
    }
}
