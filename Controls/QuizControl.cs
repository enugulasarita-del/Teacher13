using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TeacherDashboard.Controls
{
    public partial class QuizControl : UserControl
    {
        // Controls
        private TextBox txtQuestion;
        private TextBox[] txtOptions = new TextBox[4];
        private ComboBox cmbCorrect;
        private Panel pnlQuizList;
        
        // Data
        private List<QuizItem> quizItems = new List<QuizItem>();

        public QuizControl()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(18, 18, 18); // Dark Theme Background

            SetupRobustLayout();
            LoadDummyData();
        }

        private void InitializeComponent()
        {
            this.Name = "QuizControl";
            this.Size = new Size(1100, 750);
        }

        private void SetupRobustLayout()
        {
            this.Controls.Clear();
            
            // --- MAIN GRID CONTAINER (Prevents Docking Overlaps) ---
            TableLayoutPanel mainGrid = new TableLayoutPanel();
            mainGrid.Dock = DockStyle.Fill;
            mainGrid.ColumnCount = 2;
            mainGrid.RowCount = 2;
            
            // Columns: 40% Left (Form), 60% Right (List)
            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            
            // Rows: Header (80px), Content (Remaining)
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            
            this.Controls.Add(mainGrid);

            // --- 1. HEADER (Spans Both Columns) ---
            Panel pnlHeader = new Panel() { 
                Dock = DockStyle.Fill, 
                BackColor = Color.FromArgb(173, 22, 37), 
                Padding = new Padding(25, 0, 0, 0),
                Margin = new Padding(0) 
            };
            Label lblTitle = new Label() { 
                Text = "⚡ SMART QUIZ CREATOR", 
                Font = new Font("Segoe UI", 18, FontStyle.Bold), 
                ForeColor = Color.White, 
                Dock = DockStyle.Fill, 
                TextAlign = ContentAlignment.MiddleLeft 
            };
            pnlHeader.Controls.Add(lblTitle);
            
            mainGrid.Controls.Add(pnlHeader, 0, 0);
            mainGrid.SetColumnSpan(pnlHeader, 2);

            // --- 2. LEFT PANEL (Input Form) ---
            Panel pnlLeft = new Panel() { 
                Dock = DockStyle.Fill, 
                BackColor = Color.FromArgb(28, 28, 30), 
                Padding = new Padding(20),
                Margin = new Padding(0)
            };
            mainGrid.Controls.Add(pnlLeft, 0, 1);

            FlowLayoutPanel flpForm = new FlowLayoutPanel() { 
                Dock = DockStyle.Fill, 
                FlowDirection = FlowDirection.TopDown, 
                WrapContents = false, 
                AutoScroll = true 
            };
            pnlLeft.Controls.Add(flpForm);

            Label lblFormTitle = new Label() { Text = "CREATE NEW QUESTION", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.Red, AutoSize = true, Margin = new Padding(0, 0, 0, 20) };
            flpForm.Controls.Add(lblFormTitle);

            // Inputs
            AddInputGroup(flpForm, "Enter Question:", out txtQuestion, 60);
            string[] opLabels = { "Option A", "Option B", "Option C", "Option D" };
            for(int i=0; i<4; i++) AddInputGroup(flpForm, opLabels[i] + ":", out txtOptions[i], 30);

            // Correct Option
            flpForm.Controls.Add(CreateLabel("Select Correct Option:"));
            cmbCorrect = new ComboBox() { 
                Width = 300, 
                DropDownStyle = ComboBoxStyle.DropDownList, 
                BackColor = Color.FromArgb(45, 45, 48), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Font = new Font("Segoe UI", 10), 
                Margin = new Padding(10, 5, 0, 20) 
            };
            cmbCorrect.Items.AddRange(new string[] { "Option A", "Option B", "Option C", "Option D" });
            cmbCorrect.SelectedIndex = 0;
            flpForm.Controls.Add(cmbCorrect);

            // Responsive Resizing for Form
            flpForm.Resize += (s, e) => {
                int w = flpForm.ClientSize.Width - 30;
                if(w > 50) {
                    txtQuestion.Width = w;
                    foreach(var t in txtOptions) if(t!=null) t.Width = w;
                    cmbCorrect.Width = w;
                }
            };

            // Buttons
            FlowLayoutPanel flpBtns = new FlowLayoutPanel() { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, Margin = new Padding(10, 10, 0, 20) };
            Button btnAdd = new Button() { Text = "➕ ADD", Width = 120, Height = 40, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand, Margin = new Padding(0, 0, 10, 0) };
            btnAdd.FlatAppearance.BorderSize = 0; btnAdd.Click += BtnAdd_Click;
            
            Button btnClear = new Button() { Text = "🧹 CLEAR", Width = 120, Height = 40, BackColor = Color.FromArgb(50, 50, 55), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand };
            btnClear.FlatAppearance.BorderSize = 0; btnClear.Click += (s, e) => ClearForm();
            
            flpBtns.Controls.Add(btnAdd); 
            flpBtns.Controls.Add(btnClear);
            flpForm.Controls.Add(flpBtns);

            // --- 3. RIGHT PANEL (Preview List) ---
            Panel pnlRight = new Panel() { 
                Dock = DockStyle.Fill, 
                BackColor = Color.FromArgb(24, 25, 26), 
                Padding = new Padding(20),
                Margin = new Padding(0)
            };
            mainGrid.Controls.Add(pnlRight, 1, 1);

            Label lblPreview = new Label() { Text = "QUIZ PREVIEW & LIVE LIST", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlRight.Controls.Add(lblPreview);

            Button btnPub = new Button() { 
                Text = "🚀 PUBLISH QUIZ TO STUDENTS", 
                Dock = DockStyle.Bottom, 
                Height = 50, 
                BackColor = Color.FromArgb(173, 22, 37), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Font = new Font("Segoe UI", 11, FontStyle.Bold), 
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 20, 0, 0)
            };
            btnPub.FlatAppearance.BorderSize = 0;
            btnPub.Click += (s,e) => MessageBox.Show($"Successfully published {quizItems.Count} questions!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            pnlRight.Controls.Add(btnPub);

            pnlQuizList = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.Transparent, Padding = new Padding(0, 10, 0, 10) };
            pnlRight.Controls.Add(pnlQuizList);
            pnlQuizList.BringToFront(); // Ensure it fills space between Title and Button
        }

        private void AddInputGroup(FlowLayoutPanel p, string label, out TextBox txt, int height)
        {
            Label l = new Label() { Text = label, ForeColor = Color.LightGray, AutoSize = true, Font = new Font("Segoe UI", 9), Margin = new Padding(10, 10, 0, 5) };
            txt = new TextBox() { 
                Width = 350, Height = height, Multiline = height > 30, BackColor = Color.FromArgb(45, 45, 48), 
                ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10), Margin = new Padding(10, 0, 0, 15)
            };
            p.Controls.Add(l);
            p.Controls.Add(txt);
        }

        private Label CreateLabel(string text)
        {
            return new Label() { Text = text, ForeColor = Color.LightGray, AutoSize = true, Font = new Font("Segoe UI", 9), Margin = new Padding(10, 10, 0, 5) };
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQuestion.Text)) {
                MessageBox.Show("Please enter a question.");
                return;
            }

            var item = new QuizItem {
                Question = txtQuestion.Text,
                Options = new string[] { txtOptions[0].Text, txtOptions[1].Text, txtOptions[2].Text, txtOptions[3].Text },
                CorrectIndex = cmbCorrect.SelectedIndex
            };

            quizItems.Add(item);
            AddPreviewCard(item);
            ClearForm();
        }

        private void AddPreviewCard(QuizItem item)
        {
            // Container for spacing
            Panel container = new Panel() {
                Dock = DockStyle.Top,
                Height = 130, 
                Padding = new Padding(0, 0, 0, 15),
                BackColor = Color.Transparent
            };

            // Card Background
            Panel card = new Panel() { 
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(35, 35, 40), 
                Padding = new Padding(15)
            };
            container.Controls.Add(card);

            // 1. Delete Button (Top Right)
            Button btnDel = new Button() {
                Text = "❌", Size = new Size(30, 30),
                FlatStyle = FlatStyle.Flat, ForeColor = Color.Red,
                Dock = DockStyle.Right, Cursor = Cursors.Hand
            };
            btnDel.FlatAppearance.BorderSize = 0;
            btnDel.Click += (s, e) => {
                quizItems.Remove(item);
                pnlQuizList.Controls.Remove(container);
                container.Dispose();
            };
            card.Controls.Add(btnDel);

            // 2. Question Text
            Label lQ = new Label() { 
                Text = $"{quizItems.IndexOf(item) + 1}. {item.Question}", 
                Font = new Font("Segoe UI", 10, FontStyle.Bold), 
                ForeColor = Color.White, 
                Dock = DockStyle.Top, 
                AutoSize = true,
                MaximumSize = new Size(0, 45), // Limit height
                AutoEllipsis = true
            };
            card.Controls.Add(lQ);

            // 3. Correct Answer
            Label lA = new Label() { 
                Text = $"Correct: {item.Options[item.CorrectIndex]}", 
                Font = new Font("Segoe UI", 9, FontStyle.Italic), 
                ForeColor = Color.FromArgb(46, 204, 113), 
                Dock = DockStyle.Bottom, 
                Height = 25
            };
            card.Controls.Add(lA);

            // Add to list
            pnlQuizList.Controls.Add(container);
            container.BringToFront(); // Moves to bottom of stack (WinForms Dock=Top logic)
        }

        private void LoadDummyData()
        {
            quizItems.Clear();
            var dummy1 = new QuizItem { Question = "What is the time complexity of Binary Search?", Options = new string[] { "O(n)", "O(log n)", "O(n^2)", "O(1)" }, CorrectIndex = 1 };
            var dummy2 = new QuizItem { Question = "Which one of these is not a Java keyword?", Options = new string[] { "class", "interface", "null", "boolean" }, CorrectIndex = 2 };
            var dummy3 = new QuizItem { Question = "OSI Model has how many layers?", Options = new string[] { "4 Layers", "5 Layers", "6 Layers", "7 Layers" }, CorrectIndex = 3 };

            quizItems.Add(dummy1);
            quizItems.Add(dummy2);
            quizItems.Add(dummy3);

            foreach (var item in quizItems) { AddPreviewCard(item); }
        }

        private void ClearForm()
        {
            txtQuestion.Clear();
            foreach (var t in txtOptions) t.Clear();
            cmbCorrect.SelectedIndex = 0;
            txtQuestion.Focus();
        }
    }

    public class QuizItem
    {
        public string Question { get; set; }
        public string[] Options { get; set; }
        public int CorrectIndex { get; set; }
    }
}
