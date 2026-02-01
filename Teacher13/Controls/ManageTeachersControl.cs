using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public partial class ManageTeachersControl : UserControl
    {
        public ManageTeachersControl()
        {
            InitializeComponent();
            SetupLayout();
            LoadMockTeachers();
        }

        private void SetupLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);
            
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitleText = new Label() { Text = "FACULTY MANAGEMENT", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitleText);
            this.Controls.Add(pnlHeader);

            Panel pnlActions = new Panel() { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(20, 10, 20, 10) };
            
            this.btnAddTeacher = new Button() { Text = "➕ ADD NEW TEACHER", Width = 180, Dock = DockStyle.Left, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(173, 22, 37), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            this.btnEditTeacher = new Button() { Text = "EDIT", Width = 100, Dock = DockStyle.Left, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(45, 45, 45), ForeColor = Color.White };
            this.btnDeleteTeacher = new Button() { Text = "DELETE", Width = 100, Dock = DockStyle.Left, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(45, 45, 45), ForeColor = Color.White };

            pnlActions.Controls.Add(this.btnDeleteTeacher);
            pnlActions.Controls.Add(this.btnEditTeacher);
            pnlActions.Controls.Add(this.btnAddTeacher);
            this.Controls.Add(pnlActions);

            this.dgvTeachers = new DataGridView() { Dock = DockStyle.Fill, BackgroundColor = Color.FromArgb(28, 28, 28), BorderStyle = BorderStyle.None };
            this.Controls.Add(this.dgvTeachers);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 2);   // Docks First (Top)
            this.Controls.SetChildIndex(pnlActions, 1);  // Docks Second (Top)
            this.Controls.SetChildIndex(dgvTeachers, 0); // Docks Last (Fill)
        }

        private void LoadMockTeachers()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            dt.Columns.Add("Department");
            dt.Columns.Add("Email");
            dt.Columns.Add("Role");

            dt.Rows.Add("T101", "Dr. Satish Kumar", "Computer Science", "satish.k@college.edu", "HOD");
            dt.Rows.Add("T102", "Prof. Megha Shah", "IT", "megha.s@college.edu", "Assistant Prof");
            dt.Rows.Add("T103", "Prof. Anil Kapoor", "Management", "anil.k@college.edu", "Senior Lecturer");

            dgvTeachers.DataSource = dt;
            dgvTeachers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTeachers.EnableHeadersVisualStyles = false;
            dgvTeachers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(173, 22, 37);
            dgvTeachers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTeachers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
    }
}
