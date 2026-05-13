using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace UDTDSK
{
    public partial class Form2 : Form
    {
        string strCon = @"Data Source=DESKTOP-NT4S0AQ;Initial Catalog=QLSK;Integrated Security=True";
        SqlConnection sqlCon = null;
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
            // 1. Kiểm tra đầu vào
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

            if (txtPass.Text != txtPass2.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!");
                txtPass2.Focus();
                txtPass2.SelectAll();
                return;
            }

            // 2. Kết nối CSDL và lưu dữ liệu
            try
            {
                if (sqlCon == null) sqlCon = new SqlConnection(strCon);
                if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();

                // Kiểm tra email đã tồn tại chưa
                string checkSql = "SELECT COUNT(*) FROM Nguoidung WHERE Email = @email";
                SqlCommand cmdCheck = new SqlCommand(checkSql, sqlCon);
                cmdCheck.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                int count = (int)cmdCheck.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Email này đã được đăng ký. Vui lòng dùng email khác!");
                    return;
                }

                // Lệnh chèn dữ liệu (Vì maID không tự tăng nên mình lấy Email làm ID luôn)
                string insertSql = "INSERT INTO Nguoidung(maID, Email, MatKhau) VALUES(@id, @email, @pass)";
                SqlCommand sqlCmd = new SqlCommand(insertSql, sqlCon);

                sqlCmd.Parameters.AddWithValue("@id", txtEmail.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@pass", txtPass.Text.Trim());

                int kq = sqlCmd.ExecuteNonQuery();

                if (kq > 0)
                {
                    DialogResult result = MessageBox.Show(
                        "Đăng ký tài khoản thành công!\nBạn có muốn quay lại trang đăng nhập không?",
                        "Thông báo",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        Form1 fr1 = new Form1();
                        fr1.Show();
                        this.Hide();
                    }
                    else
                    {
                        txtEmail.Text = "";
                        txtPass.Text = "";
                        txtPass2.Text = "";
                        txtEmail.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
            finally
            {
                if (sqlCon != null && sqlCon.State == ConnectionState.Open)
                    sqlCon.Close();
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
