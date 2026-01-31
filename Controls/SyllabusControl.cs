using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TeacherDashboard.Controls
{
    public class SyllabusControl : UserControl
    {
        public SyllabusControl()
        {
            SetupStrictLayout();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // 1. Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitle = new Label() { Text = "SYLLABUS & COURSE TRACKER", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            // 2. Active Courses Progress
            Label lblCourses = new Label() { Text = "ACTIVE COURSE PROGRESS", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlScroll.Controls.Add(lblCourses);

            FlowLayoutPanel flpCourses = new FlowLayoutPanel() { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(0, 5, 0, 20) };
            flpCourses.Controls.Add(CreateCourseProgress("Data Structures", 85, Color.FromArgb(46, 204, 113)));
            flpCourses.Controls.Add(CreateCourseProgress("Operating Systems", 60, Color.FromArgb(52, 152, 219)));
            flpCourses.Controls.Add(CreateCourseProgress("Digital Electronics", 45, Color.FromArgb(241, 196, 15)));
            pnlScroll.Controls.Add(flpCourses);

            // 3. Syllabus Chapters List
            Label lblChapters = new Label() { Text = "CHAPTER-WISE STATUS (DBMS)", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblChapters);

            DataGridView dgvSyllabus = new DataGridView() { 
                Dock = DockStyle.Top, 
                Height = 250, 
                BackgroundColor = Color.FromArgb(28, 28, 28), 
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 40,
                ReadOnly = true,
                AllowUserToAddRows = false,
                GridColor = Color.FromArgb(45, 45, 45)
            };
            dgvSyllabus.DefaultCellStyle.BackColor = Color.White;
            dgvSyllabus.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvSyllabus.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSyllabus.Columns.Add("Unit", "Unit #");
            dgvSyllabus.Columns.Add("Title", "Chapter Title");
            dgvSyllabus.Columns.Add("Status", "Completion Status");
            dgvSyllabus.Columns.Add("Resources", "Resources Uploaded");
            
            dgvSyllabus.Rows.Add("Unit 1", "ER Modeling", "Completed", "05 PDFs");
            dgvSyllabus.Rows.Add("Unit 2", "SQL Queries", "In Progress", "03 PDFs, 02 Videos");
            dgvSyllabus.Rows.Add("Unit 3", "Normalization", "Pending", "Not Started");
            dgvSyllabus.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            pnlScroll.Controls.Add(dgvSyllabus);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 1); // Docks First (Top)
            this.Controls.SetChildIndex(pnlScroll, 0); // Docks Last (Fill)
        }

        private Panel CreateCourseProgress(string name, int progress, Color accent)
        {
            Panel p = new Panel() { Size = new Size(240, 100), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Label lblN = new Label() { Text = name, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 15), AutoSize = true };
            Label lblP = new Label() { Text = progress + "%", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.Gray, Location = new Point(190, 15), AutoSize = true };
            
            Panel pnlBar = new Panel() { Location = new Point(15, 50), Size = new Size(210, 8), BackColor = Color.FromArgb(45, 45, 45) };
            Panel pnlFill = new Panel() { Dock = DockStyle.Left, Width = (int)(210 * (progress / 100.0)), BackColor = accent };
            pnlBar.Controls.Add(pnlFill);
            
            p.Controls.AddRange(new Control[] { lblN, lblP, pnlBar });
            return p;
        }
    }
}
