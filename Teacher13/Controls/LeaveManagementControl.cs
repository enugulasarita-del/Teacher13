using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public class LeaveManagementControl : UserControl
    {
        public LeaveManagementControl()
        {
            SetupStrictLayout();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitle = new Label() { Text = "FACULTY LEAVE MANAGEMENT", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            // Leave Summary
            Label lblSum = new Label() { Text = "LEAVE BALANCE (ANNUAL)", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlScroll.Controls.Add(lblSum);

            FlowLayoutPanel flpLeave = new FlowLayoutPanel() { Dock = DockStyle.Top, Height = 100, WrapContents = false };
            flpLeave.Controls.Add(CreateLeaveBox("Casual Leave", "08/12 Left", Color.FromArgb(52, 152, 219)));
            flpLeave.Controls.Add(CreateLeaveBox("Sick Leave", "10/10 Left", Color.FromArgb(46, 204, 113)));
            flpLeave.Controls.Add(CreateLeaveBox("Duty Leave", "02 Taken", Color.FromArgb(155, 89, 182)));
            pnlScroll.Controls.Add(flpLeave);

            // Apply Leave Form
            Panel pnlApply = new Panel() { Dock = DockStyle.Top, Height = 250, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(25), Margin = new Padding(0, 20, 0, 0) };
            Label lblApply = new Label() { Text = "Apply for New Leave", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlApply.Controls.Add(lblApply);
            
            pnlApply.Controls.Add(new Label() { Text = "Date Range:", ForeColor = Color.LightGray, Location = new Point(25, 85), AutoSize = true });
            pnlApply.Controls.Add(new DateTimePicker() { Location = new Point(120, 82), Width = 150 });
            pnlApply.Controls.Add(new Label() { Text = "to", ForeColor = Color.Gray, Location = new Point(280, 85), AutoSize = true });
            pnlApply.Controls.Add(new DateTimePicker() { Location = new Point(310, 82), Width = 150 });
            
            pnlApply.Controls.Add(new Label() { Text = "Reason:", ForeColor = Color.LightGray, Location = new Point(25, 125), AutoSize = true });
            pnlApply.Controls.Add(new TextBox() { Location = new Point(120, 122), Width = 340, BackColor = Color.FromArgb(45, 45, 45), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle });
            
            Button btnSubmit = new Button() { Text = "SUBMIT APPLICATION", Location = new Point(25, 180), Size = new Size(200, 40), BackColor = Color.FromArgb(173, 22, 37), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            pnlApply.Controls.Add(btnSubmit);
            pnlScroll.Controls.Add(pnlApply);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 1); // Docks First (Top)
            this.Controls.SetChildIndex(pnlScroll, 0); // Docks Last (Fill)
        }

        private Panel CreateLeaveBox(string title, string val, Color accent)
        {
            Panel p = new Panel() { Size = new Size(200, 80), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 0) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 8), ForeColor = Color.Gray, Location = new Point(15, 15), AutoSize = true };
            Label lblV = new Label() { Text = val, Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 35), AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lblT, lblV });
            return p;
        }
    }
}
