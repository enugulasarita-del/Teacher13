using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace TeacherDashboard.Controls
{
    public class FeeManagementControl : UserControl
    {
        public FeeManagementControl()
        {
            SetupStrictLayout();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitle = new Label() { Text = "INSTITUTIONAL FEE MANAGEMENT", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            // Summary Cards
            FlowLayoutPanel flpStats = new FlowLayoutPanel() { Dock = DockStyle.Top, Height = 120, WrapContents = false };
            flpStats.Controls.Add(CreateFeeCard("Total Collected", "₹ 45.2L", Color.FromArgb(46, 204, 113)));
            flpStats.Controls.Add(CreateFeeCard("Pending Dues", "₹ 8.4L", Color.FromArgb(231, 76, 60)));
            flpStats.Controls.Add(CreateFeeCard("Scholarships", "₹ 5.0L", Color.FromArgb(52, 152, 219)));
            pnlScroll.Controls.Add(flpStats);

            // Fee Table
            Label lblTable = new Label() { Text = "PENDING FEE DEFUALTERS", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblTable);

            DataGridView dgvFees = new DataGridView() { 
                Dock = DockStyle.Top, 
                Height = 300, 
                BackgroundColor = Color.FromArgb(28, 28, 28), 
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 40,
                ReadOnly = true,
                AllowUserToAddRows = false,
                GridColor = Color.FromArgb(45, 45, 45)
            };
            dgvFees.DefaultCellStyle.BackColor = Color.White;
            dgvFees.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(173, 22, 37);
            dgvFees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFees.Columns.Add("ID", "Student ID");
            dgvFees.Columns.Add("Name", "Name");
            dgvFees.Columns.Add("Class", "Class");
            dgvFees.Columns.Add("Pending", "Pending Amount");
            dgvFees.Columns.Add("Status", "Notice Status");

            dgvFees.Rows.Add("S101", "Amaya Rao", "SY-BSCIT", "₹ 15,000", "Final Warning sent");
            dgvFees.Rows.Add("S105", "Kabir Singh", "TY-BMM", "₹ 8,500", "Email sent");
            dgvFees.Rows.Add("S112", "Esha Gupta", "FY-BMS", "₹ 22,000", "Follow-up required");
            dgvFees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            pnlScroll.Controls.Add(dgvFees);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 1); // Docks First (Top)
            this.Controls.SetChildIndex(pnlScroll, 0); // Docks Last (Fill)
        }

        private Panel CreateFeeCard(string title, string val, Color accent)
        {
            Panel p = new Panel() { Size = new Size(220, 100), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(15, 15), AutoSize = true };
            Label lblV = new Label() { Text = val, Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 40), AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lblT, lblV });
            return p;
        }
    }
}
