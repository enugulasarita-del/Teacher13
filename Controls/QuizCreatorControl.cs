using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public class QuizCreatorControl : UserControl
    {
        public QuizCreatorControl()
        {
            SetupStrictLayout();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitle = new Label() { Text = "QUIZ & ASSESSMENT CREATOR", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            // Quiz Info
            Panel pnlInfo = new Panel() { Dock = DockStyle.Top, Height = 120, BackColor = Color.FromArgb(32, 33, 36), Padding = new Padding(20) };
            pnlInfo.Controls.Add(new Label() { Text = "Quiz Title:", ForeColor = Color.LightGray, Location = new Point(20, 25), AutoSize = true });
            pnlInfo.Controls.Add(new TextBox() { Location = new Point(120, 22), Width = 300, BackColor = Color.FromArgb(45, 45, 45), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle });
            
            pnlInfo.Controls.Add(new Label() { Text = "Total Marks:", ForeColor = Color.LightGray, Location = new Point(20, 65), AutoSize = true });
            pnlInfo.Controls.Add(new NumericUpDown() { Location = new Point(120, 62), Width = 60 });
            
            pnlScroll.Controls.Add(pnlInfo);

            // Questions Section
            Label lblQ = new Label() { Text = "ADD QUESTIONS", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblQ);

            FlowLayoutPanel flpQuestions = new FlowLayoutPanel() { Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false };
            pnlScroll.Controls.Add(flpQuestions);

            flpQuestions.Controls.Add(CreateQuestionTemplate(1));
            flpQuestions.Controls.Add(CreateQuestionTemplate(2));
            flpQuestions.Controls.Add(CreateQuestionTemplate(3));

            Button btnAdd = new Button() { Text = "+ ADD NEW QUESTION", Dock = DockStyle.Top, Height = 40, BackColor = Color.FromArgb(45, 45, 45), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            pnlScroll.Controls.Add(btnAdd);

            Button btnSave = new Button() { Text = "🚀 PUBLISH QUIZ", Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(173, 22, 37), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(btnSave);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 1); // Docks First (Top)
            this.Controls.SetChildIndex(pnlScroll, 0); // Docks Last (Fill)
        }

        private Panel CreateQuestionTemplate(int num)
        {
            Panel p = new Panel() { Size = new Size(700, 150), BackColor = Color.FromArgb(28, 28, 28), Margin = new Padding(0, 0, 0, 15), Padding = new Padding(15) };
            p.Controls.Add(new Label() { Text = $"Q{num}. Enter Question Text:", ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Dock = DockStyle.Top, Height = 30 });
            p.Controls.Add(new TextBox() { Multiline = true, Dock = DockStyle.Fill, BackColor = Color.FromArgb(45, 45, 45), ForeColor = Color.White, BorderStyle = BorderStyle.None });
            
            Panel pnlOptions = new Panel() { Dock = DockStyle.Bottom, Height = 40 };
            pnlOptions.Controls.Add(new RadioButton() { Text = "Opt A", ForeColor = Color.Gray, Location = new Point(0, 10), Width = 80 });
            pnlOptions.Controls.Add(new RadioButton() { Text = "Opt B", ForeColor = Color.Gray, Location = new Point(100, 10), Width = 80 });
            pnlOptions.Controls.Add(new RadioButton() { Text = "Opt C", ForeColor = Color.Gray, Location = new Point(200, 10), Width = 80 });
            pnlOptions.Controls.Add(new RadioButton() { Text = "Opt D", ForeColor = Color.Gray, Location = new Point(300, 10), Width = 80 });
            p.Controls.Add(pnlOptions);
            
            return p;
        }
    }
}
