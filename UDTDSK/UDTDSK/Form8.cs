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
    public partial class Form8 : Form
    {
        string connectionString = @"Data Source=DESKTOP-NT4S0AQ;Initial Catalog=QLSK;Integrated Security=True";

        Color originalBackColor;
        Color originalForeColor;
        public Form8()
        {
            InitializeComponent();
        }
        private void HideLogoutIfNeeded()
        {
            if (!pictureBox1.ClientRectangle.Contains(pictureBox1.PointToClient(Cursor.Position)) &&
                !btnDangXuat.ClientRectangle.Contains(btnDangXuat.PointToClient(Cursor.Position)))
            {
                btnDangXuat.Visible = false;
            }
        }
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            btnDangXuat.Visible = true;
            btnDangXuat.BringToFront();
        }
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            HideLogoutIfNeeded();
        }
        private void btnDangXuat_MouseEnter(object sender, EventArgs e)
        {
            btnDangXuat.Visible = true;
        }
        private void btnDangXuat_MouseLeave(object sender, EventArgs e)
        {
            HideLogoutIfNeeded();
        }
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có muốn đăng xuất không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Form1 fr1 = new Form1();
                fr1.Show();
                this.Hide();
            }
        }
        private void Button_MouseEnter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            originalBackColor = btn.BackColor;
            originalForeColor = btn.ForeColor;
            btn.BackColor = Color.Blue;
            btn.ForeColor = Color.White;
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = originalBackColor;
            btn.ForeColor = originalForeColor;
        }
        private void CenterButtons(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Button)
                {
                    Button btn = (Button)ctrl;
                    btn.Left = (parent.Width - btn.Width) / 2;
                }

                if (ctrl.HasChildren)
                {
                    CenterButtons(ctrl);
                }
            }
        }
        private void AddHoverEffect(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Button)
                {
                    Button btn = (Button)ctrl;

                    btn.MouseEnter += Button_MouseEnter;
                    btn.MouseLeave += Button_MouseLeave;

                    btn.FlatStyle = FlatStyle.Flat;
                    btn.UseVisualStyleBackColor = false;
                }

                if (ctrl.HasChildren)
                {
                    AddHoverEffect(ctrl);
                }
            }
        }
        private void Form8_Load(object sender, EventArgs e)
        {
            pictureBox1.Left = (splitContainer1.Panel1.Width - pictureBox1.Width) / 2;
            btnDangXuat.Left = (splitContainer1.Panel1.Width - btnDangXuat.Width) / 2;

            //Xử lý nút Button
            AddHoverEffect(this);
            CenterButtons(splitContainer1.Panel1);

            //Màu viền nút Button Thông tin cá nhân
            button4.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 2;
            button4.FlatAppearance.BorderColor = Color.Violet;

            // Xử lý button đăng xuất
            btnDangXuat.Click += btnDangXuat_Click;
            btnDangXuat.MouseEnter += btnDangXuat_MouseEnter;
            btnDangXuat.MouseLeave += btnDangXuat_MouseLeave;

            //Xử lý Ảnh
            pictureBox1.MouseEnter += pictureBox1_MouseEnter;
            pictureBox1.MouseLeave += pictureBox1_MouseLeave;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form6 fr6 = new Form6();
            fr6.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form7 fr7 = new Form7();
            fr7.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form5 fr5 = new Form5();
            fr5.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form9 fr9 = new Form9();
            fr9.Show();
            this.Hide();
        }
        private string GetLoaiMucTieu()
        {
            if (radCanNang.Checked) return "Cân nặng";
            if (radSoBuoc.Checked) return "Số bước chân";
            if (radLuongNuoc.Checked) return "Lượng nước uống";
            if (radCalorie.Checked) return "Calorie";
            return "";
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (txtTenMucTieu.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng đặt tên mục tiêu!");
                txtTenMucTieu.Focus();
                return;
            }

            // Kiểm tra RadioButton
            if (!radCanNang.Checked &&
                !radSoBuoc.Checked &&
                !radLuongNuoc.Checked &&
                !radCalorie.Checked)
            {
                MessageBox.Show("Vui lòng chọn loại mục tiêu!");
                return;
            }

            if (txtMTHoanThanh.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập số lượng mục tiêu cần hoàn thành!");
                txtMTHoanThanh.Focus();
                return;
            }

            // Xác định loại mục tiêu
            string loaiMucTieu = "";

            if (radCanNang.Checked)
                loaiMucTieu = "Cân nặng";

            else if (radSoBuoc.Checked)
                loaiMucTieu = "Số bước";

            else if (radLuongNuoc.Checked)
                loaiMucTieu = "Lượng nước";

            else if (radCalorie.Checked)
                loaiMucTieu = "Calories";

            // Mã mục tiêu tự động
            string maMucTieu = "MT" + DateTime.Now.Ticks.ToString();

            // Trạng thái mặc định
            string trangThai = "Chưa hoàn thành";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"INSERT INTO Muc_tieu
                            (Ma_muc_tieu, mo_ta, gia_tri, Gia_tri_hien_tai, Trang_thai)
                            VALUES
                            (@MaMT, @MoTa, @GiaTri, @GiaTriHT, @TrangThai)";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@MaMT", maMucTieu);

                    // Tên mục tiêu
                    cmd.Parameters.AddWithValue("@MoTa",
                        txtTenMucTieu.Text + " - " + loaiMucTieu);

                    // Giá trị mục tiêu
                    cmd.Parameters.AddWithValue("@GiaTri",
                        txtMTHoanThanh.Text + " " + cboDonVi.Text);

                    // Giá trị hiện tại mặc định
                    cmd.Parameters.AddWithValue("@GiaTriHT", "0");

                    cmd.Parameters.AddWithValue("@TrangThai", trangThai);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Thêm mục tiêu thành công!");
                }

                // Reset dữ liệu
                txtTenMucTieu.Clear();
                txtMTHoanThanh.Clear();

                radCanNang.Checked = false;
                radSoBuoc.Checked = false;
                radLuongNuoc.Checked = false;
                radCalorie.Checked = false;

                cboDonVi.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message);
            }
        }

        private void radCanNang_CheckedChanged(object sender, EventArgs e)
        {
            if (radCanNang.Checked)
            {
                cboDonVi.Items.Clear();
                cboDonVi.Items.Add("kg");
                cboDonVi.SelectedIndex = 0; // Tự động chọn mục đầu tiên
            }
        }

        private void radSoBuoc_CheckedChanged(object sender, EventArgs e)
        {
            if (radSoBuoc.Checked)
            {
                cboDonVi.Items.Clear();
                cboDonVi.Items.Add("bước");
                cboDonVi.SelectedIndex = 0;
            }
        }

        private void radLuongNuoc_CheckedChanged(object sender, EventArgs e)
        {
            if (radLuongNuoc.Checked)
            {
                cboDonVi.Items.Clear();
                cboDonVi.Items.Add("ml");
                cboDonVi.Items.Add("lít");
                cboDonVi.SelectedIndex = 0;
            }
        }

        private void radCalorie_CheckedChanged(object sender, EventArgs e)
        {
            if (radCalorie.Checked)
            {
                cboDonVi.Items.Clear();
                cboDonVi.Items.Add("kcal");
                cboDonVi.SelectedIndex = 0;
            }
        }
    }
}
