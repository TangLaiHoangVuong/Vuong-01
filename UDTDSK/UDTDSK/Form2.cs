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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form1 fr1 = new Form1();
            fr1.Show();
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

            if (txtPass2.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng xác nhận lại mật khẩu!");
                txtPass2.Focus();
                return;
            }

            // Kiểm tra mật khẩu nhập lại có giống không
            if (txtPass.Text != txtPass2.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!");
                txtPass2.Focus();
                txtPass2.SelectAll();
                return;
            }

            // Thông báo đăng ký thành công và hỏi quay lại đăng nhập
            DialogResult result = MessageBox.Show(
                "Đăng ký tài khoản thành công!\nBạn có muốn quay lại trang đăng nhập không?",
                "Thông báo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Nếu chọn Yes
            if (result == DialogResult.Yes)
            {
                Form1 fr1 = new Form1();
                fr1.Show();
                this.Hide();
            }
            // Nếu chọn No
            else if (result == DialogResult.No)
            {
                txtEmail.Text = "";
                txtPass.Text = "";
                txtPass2.Text = "";

                txtEmail.Focus();
            }
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

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            // Hiện mật khẩu
            txtPass2.UseSystemPasswordChar = false;

            pictureBox4.Visible = false;
            pictureBox3.Visible = true;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // Ẩn mật khẩu
            txtPass2.UseSystemPasswordChar = true;

            pictureBox4.Visible = true;
            pictureBox3.Visible = false;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            // Ẩn mật khẩu
            txtPass.UseSystemPasswordChar = true;
            txtPass2.UseSystemPasswordChar = true;
        }
    }
}
