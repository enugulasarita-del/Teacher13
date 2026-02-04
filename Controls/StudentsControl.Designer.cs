using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    partial class StudentsControl
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // StudentsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.Name = "StudentsControl";
            this.Size = new System.Drawing.Size(1000, 750);
            this.ResumeLayout(false);
        }
    }
}
