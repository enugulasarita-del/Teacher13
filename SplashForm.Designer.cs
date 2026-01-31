namespace TeacherDashboard
{
    partial class SplashForm
    {
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.imgVidya = new System.Windows.Forms.PictureBox();
            this.lblSystemName = new System.Windows.Forms.Label();
            this.pnlProgressBarBase = new System.Windows.Forms.Panel();
            this.pnlLoading = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.imgVidya)).BeginInit();
            this.pnlProgressBarBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgVidya
            // 
            this.imgVidya.ImageLocation = "Resources/vidyalankar_logo.png";
            this.imgVidya.Location = new System.Drawing.Point(200, 100);
            this.imgVidya.Name = "imgVidya";
            this.imgVidya.Size = new System.Drawing.Size(200, 150);
            this.imgVidya.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgVidya.TabIndex = 0;
            this.imgVidya.TabStop = false;
            // 
            // lblSystemName
            // 
            this.lblSystemName.AutoSize = true;
            this.lblSystemName.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSystemName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(22)))), ((int)(((byte)(37)))));
            this.lblSystemName.Location = new System.Drawing.Point(142, 280);
            this.lblSystemName.Name = "lblSystemName";
            this.lblSystemName.Size = new System.Drawing.Size(315, 25);
            this.lblSystemName.TabIndex = 1;
            this.lblSystemName.Text = "TEACHER MANAGEMENT SYSTEM";
            this.lblSystemName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlProgressBarBase
            // 
            this.pnlProgressBarBase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlProgressBarBase.Controls.Add(this.pnlLoading);
            this.pnlProgressBarBase.Location = new System.Drawing.Point(100, 350);
            this.pnlProgressBarBase.Name = "pnlProgressBarBase";
            this.pnlProgressBarBase.Size = new System.Drawing.Size(400, 10);
            this.pnlProgressBarBase.TabIndex = 2;
            // 
            // pnlLoading
            // 
            this.pnlLoading.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(22)))), ((int)(((byte)(37)))));
            this.pnlLoading.Location = new System.Drawing.Point(0, 0);
            this.pnlLoading.Name = "pnlLoading";
            this.pnlLoading.Size = new System.Drawing.Size(0, 10);
            this.pnlLoading.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // SplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(600, 450);
            this.Controls.Add(this.pnlProgressBarBase);
            this.Controls.Add(this.lblSystemName);
            this.Controls.Add(this.imgVidya);
            this.Name = "SplashForm";
            this.Text = "SplashForm";
            ((System.ComponentModel.ISupportInitialize)(this.imgVidya)).EndInit();
            this.pnlProgressBarBase.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.PictureBox imgVidya;
        private System.Windows.Forms.Label lblSystemName;
        private System.Windows.Forms.Panel pnlProgressBarBase;
        private System.Windows.Forms.Panel pnlLoading;
        private System.Windows.Forms.Timer timer1;
    }
}
