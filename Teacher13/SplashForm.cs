using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard
{
    public partial class SplashForm : Form
    {
        public SplashForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pnlLoading.Width += 5;
            if (pnlLoading.Width >= 400)
            {
                timer1.Stop();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
