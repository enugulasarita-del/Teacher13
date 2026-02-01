using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public class AlumniControl : UserControl
    {
        public AlumniControl()
        {
            SetupStrictLayout();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            pnlHeader.Controls.Add(new Label() { Text = "ALUMNI NETWORK & RELATIONS", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) });
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            Label lblEvents = new Label() { Text = "ALUMNI MEET & REUNION CALENDAR", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlScroll.Controls.Add(lblEvents);

            FlowLayoutPanel flpEvents = new FlowLayoutPanel() { Dock = DockStyle.Top, Height = 130, WrapContents = false };
            flpEvents.Controls.Add(CreateEventBox("Grand Reunion 2026", "Dec 15", Color.FromArgb(155, 89, 182)));
            flpEvents.Controls.Add(CreateEventBox("Tech-Talk Webinar", "Mar 10", Color.FromArgb(52, 152, 219)));
            flpEvents.Controls.Add(CreateEventBox("Donation Drive", "Ongoing", Color.FromArgb(46, 204, 113)));
            pnlScroll.Controls.Add(flpEvents);

            Label lblList = new Label() { Text = "DISTINGUISHED ALUMNI DIRECTORY", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblList);

            DataGridView dgvAlumni = new DataGridView() { 
                Dock = DockStyle.Top, Height = 300, 
                BackgroundColor = Color.FromArgb(28, 28, 28), 
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 40,
                ReadOnly = true,
                AllowUserToAddRows = false,
                GridColor = Color.FromArgb(45, 45, 45)
            };
            dgvAlumni.DefaultCellStyle.BackColor = Color.White;
            dgvAlumni.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvAlumni.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAlumni.Columns.Add("Name", "Name");
            dgvAlumni.Columns.Add("Batch", "Batch");
            dgvAlumni.Columns.Add("Company", "Current Organization");
            dgvAlumni.Columns.Add("Contact", "Email Address");

            dgvAlumni.Rows.Add("Rahul Malhotra", "2018-21", "Google India", "rahul.m@gmail.com");
            dgvAlumni.Rows.Add("Sneha Kulkarni", "2015-18", "Amazon AWS", "sneha.k@outlook.com");
            dgvAlumni.Rows.Add("Vikram Shah", "2019-22", "Tata Motors", "v.shah@tata.com");
            dgvAlumni.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            pnlScroll.Controls.Add(dgvAlumni);

            // Force Strict Docking Priority for Outer Layout
            this.Controls.SetChildIndex(pnlHeader, 1); 
            this.Controls.SetChildIndex(pnlScroll, 0); 

            // Force Section Ordering inside Scroll Panel (Top to Bottom)
            pnlScroll.Controls.SetChildIndex(lblList, 3);    // 1. Directory Title
            pnlScroll.Controls.SetChildIndex(dgvAlumni, 2); // 2. Directory Grid
            pnlScroll.Controls.SetChildIndex(lblEvents, 1);  // 3. Events Title
            pnlScroll.Controls.SetChildIndex(flpEvents, 0);  // 4. Event Cards
        }

        private Panel CreateEventBox(string title, string date, Color accent)
        {
            Panel p = new Panel() { Size = new Size(220, 100), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Panel l = new Panel() { Dock = DockStyle.Left, Width = 5, BackColor = accent };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 15), Size = new Size(180, 40) };
            Label lblD = new Label() { Text = date, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(15, 60), AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lblT, lblD });
            return p;
        }
    }
}
