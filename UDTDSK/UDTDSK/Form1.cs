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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form2 fr2 = new Form2();
            fr2.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tài khoản Email của bạn!");
                txtEmail.Focus();
                return;
            }

            if (txtPass.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!");
                txtPass.Focus();
                return;
            }

            Form4 fr4 = new Form4();
            fr4.Show();
            this.Hide();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Hiện mật khẩu
            txtPass.UseSystemPasswordChar = false;

            pictureBox1.Visible = false;
            pictureBox2.Visible = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Ẩn mật khẩu
            txtPass.UseSystemPasswordChar = true;

            pictureBox1.Visible = true;
            pictureBox2.Visible = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Ẩn mật khẩu
            txtPass.UseSystemPasswordChar = true;
        }
    }
}
