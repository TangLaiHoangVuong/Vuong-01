using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDTDSK
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            SetupUI();
        }
        private void SetupUI()
        {
            // ===== FORM =====
            this.Text = "Health Care Dashboard";
            this.Size = new Size(1000, 600);
            this.BackColor = Color.FromArgb(240, 245, 249);
            this.StartPosition = FormStartPosition.CenterScreen;

            // ===== SIDEBAR =====
            Panel sidebar = new Panel();
            sidebar.Size = new Size(200, this.Height);
            sidebar.BackColor = Color.FromArgb(52, 152, 219);
            sidebar.Dock = DockStyle.Left;

            this.Controls.Add(sidebar);

            // ===== TITLE =====
            Label title = new Label();
            title.Text = "HEALTH CARE";
            title.ForeColor = Color.White;
            title.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            title.Location = new Point(20, 30);
            title.AutoSize = true;

            sidebar.Controls.Add(title);

            // ===== MENU BUTTONS =====
            string[] menus =
            {
                "Dashboard",
                "Profile",
                "Medicine",
                "Schedule",
                "Reports"
            };

            int top = 100;

            foreach (string menu in menus)
            {
                Button btn = new Button();

                btn.Text = menu;
                btn.Size = new Size(160, 45);
                btn.Location = new Point(20, top);

                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;

                btn.BackColor = Color.FromArgb(41, 128, 185);
                btn.ForeColor = Color.White;

                btn.Font = new Font("Segoe UI", 11, FontStyle.Regular);

                sidebar.Controls.Add(btn);

                top += 60;
            }

            // ===== DASHBOARD TITLE =====
            Label dashboardTitle = new Label();

            dashboardTitle.Text = "Health Dashboard";
            dashboardTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            dashboardTitle.ForeColor = Color.FromArgb(44, 62, 80);

            dashboardTitle.Location = new Point(250, 30);
            dashboardTitle.AutoSize = true;

            this.Controls.Add(dashboardTitle);

            // ===== HEALTH CARDS =====
            CreateCard("BMI", "22.1", Color.SeaGreen, 250, 100);

            CreateCard("Heart Rate", "78 BPM", Color.IndianRed, 500, 100);

            CreateCard("Blood Pressure", "120/80", Color.SteelBlue, 750, 100);
        }

        private void CreateCard(
            string title,
            string value,
            Color valueColor,
            int x,
            int y)
        {
            Panel card = new Panel();

            card.Size = new Size(200, 120);
            card.BackColor = Color.White;
            card.Location = new Point(x, y);

            card.BorderStyle = BorderStyle.FixedSingle;

            // ===== TITLE =====
            Label lblTitle = new Label();

            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTitle.ForeColor = Color.Gray;

            lblTitle.Location = new Point(20, 20);
            lblTitle.AutoSize = true;

            // ===== VALUE =====
            Label lblValue = new Label();

            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblValue.ForeColor = valueColor;

            lblValue.Location = new Point(20, 50);
            lblValue.AutoSize = true;

            // ===== ADD =====
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            this.Controls.Add(card);
        }
    }
}
