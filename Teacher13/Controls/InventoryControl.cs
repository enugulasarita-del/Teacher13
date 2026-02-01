using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherDashboard.Controls
{
    public class InventoryControl : UserControl
    {
        public InventoryControl()
        {
            SetupStrictLayout();
        }

        private void SetupStrictLayout()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(18, 18, 18);

            // Header
            Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(173, 22, 37) };
            Label lblTitle = new Label() { Text = "INSTITUTIONAL ASSET & INVENTORY", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(25, 15) };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            Panel pnlScroll = new Panel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25) };
            this.Controls.Add(pnlScroll);

            // Categories
            Label lblCats = new Label() { Text = "ASSET CATEGORIES", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40 };
            pnlScroll.Controls.Add(lblCats);

            FlowLayoutPanel flpCats = new FlowLayoutPanel() { Dock = DockStyle.Top, Height = 130, WrapContents = false };
            flpCats.Controls.Add(CreateAssetBox("Lab Equipment", "420 Units", Color.FromArgb(52, 152, 219)));
            flpCats.Controls.Add(CreateAssetBox("IT Assets", "150 PCs", Color.FromArgb(155, 89, 182)));
            flpCats.Controls.Add(CreateAssetBox("Furniture", "1.2k Units", Color.FromArgb(230, 126, 34)));
            pnlScroll.Controls.Add(flpCats);

            // Inventory List
            Label lblList = new Label() { Text = "RECENT PROCUREMENT & LOW STOCK", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 40, Margin = new Padding(0, 20, 0, 0) };
            pnlScroll.Controls.Add(lblList);

            DataGridView dgvInv = new DataGridView() { 
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
            dgvInv.DefaultCellStyle.BackColor = Color.White;
            dgvInv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvInv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvInv.Columns.Add("Item", "Item Name");
            dgvInv.Columns.Add("Stock", "Available Stock");
            dgvInv.Columns.Add("Location", "Location");
            dgvInv.Columns.Add("Status", "Stock Status");

            dgvInv.Rows.Add("Projector Bulbs", "02 Units", "IT Storage", "CRITICAL LOW");
            dgvInv.Rows.Add("A4 Paper Rims", "45 Units", "Admin Cell", "GOOD");
            dgvInv.Rows.Add("RJ45 Connectors", "500 Units", "Networking Lab", "EXCESS");
            dgvInv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            pnlScroll.Controls.Add(dgvInv);

            // Force Strict Docking Priority
            this.Controls.SetChildIndex(pnlHeader, 1); // Docks First (Top)
            this.Controls.SetChildIndex(pnlScroll, 0); // Docks Last (Fill)
        }

        private Panel CreateAssetBox(string title, string count, Color accent)
        {
            Panel p = new Panel() { Size = new Size(220, 100), BackColor = Color.FromArgb(32, 33, 36), Margin = new Padding(0, 0, 20, 20) };
            Panel l = new Panel() { Dock = DockStyle.Bottom, Height = 5, BackColor = accent };
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.Gray, Location = new Point(15, 15), AutoSize = true };
            Label lblC = new Label() { Text = count, Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 40), AutoSize = true };
            p.Controls.AddRange(new Control[] { l, lblT, lblC });
            return p;
        }
    }
}
