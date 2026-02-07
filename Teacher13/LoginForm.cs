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
            this.cmbRole.Visible = false;
            this.lblRoleSelect.Visible = false;
            
            // --- PREMIUM REDESIGN ---
            this.pnlRight.BackColor = Color.White;
            
            // 1. Center Title
            lblTitle.Text = "FACULTY LOGIN";
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(173, 22, 37);
            lblTitle.Size = new Size(500, 60);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Location = new Point(0, 50);

            // 2. Styled Inputs (Compact Layout)
            int startY = 145; // Exactly 35px from Title bottom
            int inputWidth = 420; 
            int centerX = (pnlRight.Width - inputWidth) / 2;

            // Username Section
            lblUsername.Text = "USERNAME";
            lblUsername.ForeColor = Color.FromArgb(120, 120, 120);
            lblUsername.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblUsername.Location = new Point(centerX, startY);
            
            txtUsername.Size = new Size(inputWidth, 42); 
            txtUsername.Location = new Point(centerX, startY + 25); // 7px gap from label
            txtUsername.Font = new Point(centerX, startY + 32).Y == 0 ? new Font("Segoe UI", 12) : new Font("Segoe UI", 13); // Larger font
            txtUsername.BackColor = Color.FromArgb(250, 250, 250);
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.ForeColor = Color.FromArgb(40, 40, 40);

            // Password Section
            lblPassword.Text = "PASSWORD";
            lblPassword.ForeColor = Color.FromArgb(120, 120, 120);
            lblPassword.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblPassword.Location = new Point(centerX, startY + 87); // 20px gap from username field
            
            txtPassword.Size = new Size(inputWidth, 42); 
            txtPassword.Location = new Point(centerX, startY + 112); // 7px gap from label
            txtPassword.Font = new Font("Segoe UI", 13); // Larger font
            txtPassword.BackColor = Color.FromArgb(250, 250, 250);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.ForeColor = Color.FromArgb(40, 40, 40);

            // 3. Login Button
            btnLogin.Size = new Size(inputWidth, 55); 
            btnLogin.Location = new Point(centerX, startY + 179); // 25px gap from password field
            btnLogin.Text = "SUBMIT";
            btnLogin.BackColor = Color.FromArgb(173, 22, 37);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            // 4. Default Credentials Link
            lblAdminLogin.Text = "Quick Login (Teacher)";
            lblAdminLogin.Font = new Font("Segoe UI", 10, FontStyle.Bold); 
            lblAdminLogin.LinkColor = Color.FromArgb(173, 22, 37);
            lblAdminLogin.AutoSize = true;
            lblAdminLogin.Location = new Point((pnlRight.Width - 160) / 2, startY + 249); // 15px gap from button top
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Always use Teacher role implicitly

            // Teacher Login Validation
            if (txtUsername.Text == "teacher" && txtPassword.Text == "teacher")
            {
                UserRole = "Teacher";
                UserName = "Prof. John Doe";
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Teacher credentials!\n\nTry: teacher/teacher", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblAdminLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtUsername.Text = "teacher";
            txtPassword.Text = "teacher";
        }
    }
}
