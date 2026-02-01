namespace TeacherDashboard.Controls
{
    partial class ExamsControl
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvExams = new System.Windows.Forms.DataGridView();
            this.btnCreateExam = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExams)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(207, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Exams & Grading";
            // 
            // dgvExams
            // 
            this.dgvExams.BackgroundColor = System.Drawing.Color.White;
            this.dgvExams.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvExams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExams.Location = new System.Drawing.Point(26, 80);
            this.dgvExams.Name = "dgvExams";
            this.dgvExams.Size = new System.Drawing.Size(950, 480);
            this.dgvExams.TabIndex = 1;
            // 
            // btnCreateExam
            // 
            this.btnCreateExam.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(22)))), ((int)(((byte)(37)))));
            this.btnCreateExam.FlatAppearance.BorderSize = 0;
            this.btnCreateExam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateExam.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCreateExam.ForeColor = System.Drawing.Color.White;
            this.btnCreateExam.Location = new System.Drawing.Point(26, 580);
            this.btnCreateExam.Name = "btnCreateExam";
            this.btnCreateExam.Size = new System.Drawing.Size(180, 40);
            this.btnCreateExam.TabIndex = 2;
            this.btnCreateExam.Text = "CREATE NEW EXAM";
            this.btnCreateExam.UseVisualStyleBackColor = false;
            // 
            // ExamsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.btnCreateExam);
            this.Controls.Add(this.dgvExams);
            this.Controls.Add(this.lblTitle);
            this.Name = "ExamsControl";
            this.Size = new System.Drawing.Size(1030, 660);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExams)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvExams;
        private System.Windows.Forms.Button btnCreateExam;
    }
}
