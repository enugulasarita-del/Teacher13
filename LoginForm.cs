using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard
{
    public partial class LoginForm : Form
    {
        public string UserRole { get; private set; }
        public string UserName { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            this.cmbRole.SelectedIndex = 0; // Default to Teacher
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string selectedRole = cmbRole.SelectedItem?.ToString() ?? "Teacher";

            // Role-Based Login Validation
            if (selectedRole == "Admin" && txtUsername.Text == "admin" && txtPassword.Text == "admin")
            {
                UserRole = "Admin";
                UserName = "System Administrator";
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (selectedRole == "Teacher" && txtUsername.Text == "teacher" && txtPassword.Text == "teacher")
            {
                UserRole = "Teacher";
                UserName = "Prof. John Doe";
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                string msg = selectedRole == "Admin" ? "Invalid Admin credentials!" : "Invalid Teacher credentials!";
                MessageBox.Show(msg + "\n\nTry:\nAdmin: admin/admin\nTeacher: teacher/teacher", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblAdminLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtUsername.Text = "admin";
            txtPassword.Text = "admin";
            cmbRole.SelectedItem = "Admin";
        }
    }
}
