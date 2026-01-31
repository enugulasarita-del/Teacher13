namespace TeacherDashboard.Controls
{
    partial class DashboardControl
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
            this.lblWelcome = new System.Windows.Forms.Label();
            this.pnlCards = new System.Windows.Forms.FlowLayoutPanel();
            this.cardStudents = new TeacherDashboard.Controls.StatCard();
            this.cardClasses = new TeacherDashboard.Controls.StatCard();
            this.cardAttendance = new TeacherDashboard.Controls.StatCard();
            this.cardAssignments = new TeacherDashboard.Controls.StatCard();
            this.pnlChartMock = new System.Windows.Forms.Panel();
            this.lblChartTitle = new System.Windows.Forms.Label();
            this.pnlCards.SuspendLayout();
            this.pnlChartMock.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblWelcome.Location = new System.Drawing.Point(20, 20);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(262, 32);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome Back, User!";
            // 
            // pnlCards
            // 
            this.pnlCards.Controls.Add(this.cardStudents);
            this.pnlCards.Controls.Add(this.cardClasses);
            this.pnlCards.Controls.Add(this.cardAttendance);
            this.pnlCards.Controls.Add(this.cardAssignments);
            this.pnlCards.Location = new System.Drawing.Point(20, 60);
            this.pnlCards.Name = "pnlCards";
            this.pnlCards.Size = new System.Drawing.Size(700, 110);
            this.pnlCards.TabIndex = 1;
            // 
            // cardStudents
            // 
            this.cardStudents.AccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(22)))), ((int)(((byte)(37)))));
            this.cardStudents.BackColor = System.Drawing.Color.White;
            this.cardStudents.Location = new System.Drawing.Point(3, 3);
            this.cardStudents.Name = "cardStudents";
            this.cardStudents.Size = new System.Drawing.Size(165, 90);
            this.cardStudents.TabIndex = 0;
            this.cardStudents.Title = "Total Students";
            this.cardStudents.Value = "1,250";
            // 
            // cardClasses
            // 
            this.cardClasses.AccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.cardClasses.BackColor = System.Drawing.Color.White;
            this.cardClasses.Location = new System.Drawing.Point(174, 3);
            this.cardClasses.Name = "cardClasses";
            this.cardClasses.Size = new System.Drawing.Size(165, 90);
            this.cardClasses.TabIndex = 1;
            this.cardClasses.Title = "Active Classes";
            this.cardClasses.Value = "42";
            // 
            // cardAttendance
            // 
            this.cardAttendance.AccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.cardAttendance.BackColor = System.Drawing.Color.White;
            this.cardAttendance.Location = new System.Drawing.Point(345, 3);
            this.cardAttendance.Name = "cardAttendance";
            this.cardAttendance.Size = new System.Drawing.Size(165, 90);
            this.cardAttendance.TabIndex = 2;
            this.cardAttendance.Title = "Attendance";
            this.cardAttendance.Value = "94%";
            // 
            // cardAssignments
            // 
            this.cardAssignments.AccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(196)))), ((int)(((byte)(15)))));
            this.cardAssignments.BackColor = System.Drawing.Color.White;
            this.cardAssignments.Location = new System.Drawing.Point(516, 3);
            this.cardAssignments.Name = "cardAssignments";
            this.cardAssignments.Size = new System.Drawing.Size(165, 90);
            this.cardAssignments.TabIndex = 3;
            this.cardAssignments.Title = "Pending";
            this.cardAssignments.Value = "12";
            // 
            // pnlChartMock
            // 
            this.pnlChartMock.BackColor = System.Drawing.Color.White;
            this.pnlChartMock.Location = new System.Drawing.Point(20, 180);
            this.pnlChartMock.Name = "pnlChartMock";
            this.pnlChartMock.Size = new System.Drawing.Size(700, 460);
            this.pnlChartMock.TabIndex = 2;
            // 
            // DashboardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.pnlChartMock);
            this.Controls.Add(this.pnlCards);
            this.Controls.Add(this.lblWelcome);
            this.Name = "DashboardControl";
            this.Size = new System.Drawing.Size(1030, 660);
            this.pnlCards.ResumeLayout(false);
            this.pnlChartMock.ResumeLayout(false);
            this.pnlChartMock.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.FlowLayoutPanel pnlCards;
        private StatCard cardStudents;
        private StatCard cardClasses;
        private StatCard cardAttendance;
        private StatCard cardAssignments;
        private System.Windows.Forms.Panel pnlChartMock;
        private System.Windows.Forms.Label lblChartTitle;
    }
}
