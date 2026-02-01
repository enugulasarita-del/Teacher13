using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public class LabManagementControl : UserControl
    {
        public LabManagementControl()
        {
            SetupStrictLayout();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            pnlHeader.Controls.Add(new Label() { Text = "LAB RESERVATION & EQUIPMENTS", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) });
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            Label lblRes = new Label() { Text = "LAB BOOKING STATUS", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlScroll.Controls.Add(lblRes);

            FlowLayoutPanel flpLabs = new FlowLayoutPanel() { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(0, 5, 0, 20) };
            flpLabs.Controls.Add(CreateLabCard("Networking Lab (601)", "Occupied", Color.FromArgb(231, 76, 60)));
            flpLabs.Controls.Add(CreateLabCard("Software Lab (502)", "Available", Color.FromArgb(46, 204, 113)));
            flpLabs.Controls.Add(CreateLabCard("Hardware Lab (405)", "Maintenance", Color.FromArgb(241, 196, 15)));
            pnlScroll.Controls.Add(flpLabs);

            Label lblInv = new Label() { Text = "LAB EQUIPMENT FAULTS (MAPPING)", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblInv);

            DataGridView dgvFaults = new DataGridView() { 
                Dock = DockStyle.Top, Height = 250, 
                BackgroundColor = Color.FromArgb(28, 28, 28), 
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 40,
                ReadOnly = true,
                AllowUserToAddRows = false,
                GridColor = Color.FromArgb(45, 45, 45)
            };
            dgvFaults.DefaultCellStyle.BackColor = Color.White;
            dgvFaults.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvFaults.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFaults.Columns.Add("ID", "Asset ID");
            dgvFaults.Columns.Add("Type", "Equipment");
            dgvFaults.Columns.Add("Issue", "Reported Issue");
            dgvFaults.Columns.Add("Lab", "Lab No");

            dgvFaults.Rows.Add("PC-601-22", "Desktop Computer", "No Power", "Lab 601");
            dgvFaults.Rows.Add("PRJ-502-01", "Projector", "Blurry Image", "Lab 502");
            dgvFaults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            pnlScroll.Controls.Add(dgvFaults);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 1); // Docks First (Top)
            this.Controls.SetChildIndex(pnlScroll, 0); // Docks Last (Fill)
        }

        private Panel CreateLabCard(string name, string status, Color accent)
        {
            Panel p = new Panel() { Size = new Size(240, 90), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Panel l = new Panel() { Dock = DockStyle.Top, Height = 5, BackColor = accent };
            Label lblN = new Label() { Text = name, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 20), AutoSize = true };
            Label lblS = new Label() { Text = status, Font = new Font("Segoe UI", 9), ForeColor = accent, Location = new Point(15, 45), AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lblN, lblS });
            return p;
        }
    }
}
