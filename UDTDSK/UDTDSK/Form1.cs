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
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=DELL\SQLEXPRESS;Initial Catalog=QLSK;Integrated Security=True";
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
            // 1.Kiểm tra dữ liệu đầu vào
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

            // 2. Chuỗi kết nối (Thay "TEN_MAY_TINH" bằng tên Server của bạn)
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Lấy thêm cột maID thay vì chỉ COUNT
                    string sql = "SELECT maID FROM Nguoidung WHERE (maID = @input OR Email = @input) AND MatKhau = @pass";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@input", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@pass", txtPass.Text.Trim());

                    object result = cmd.ExecuteScalar(); // Lấy giá trị đầu tiên (maID)

                    if (result != null)
                    {
                        // GÁN ID VÀO SESSION ĐỂ CÁC FORM KHÁC SỬ DỤNG
                        UserSession.CurrentUserID = result.ToString();

                        MessageBox.Show("Đăng nhập thành công!");
                        Form4 fr4 = new Form4();
                        fr4.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
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
        private void Form1_Load(object sender, EventArgs e)
        {
            // Ẩn mật khẩu
            txtPass.UseSystemPasswordChar = true;
        }
    }
}
