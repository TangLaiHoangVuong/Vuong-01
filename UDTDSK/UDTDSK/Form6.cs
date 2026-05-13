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
    public partial class Form6 : Form
    {
        string strCon = @"Data Source=DELL\SQLEXPRESS;Initial Catalog=QLSK;Integrated Security=True";
        SqlConnection sqlCon = null;

        Color originalBackColor;
        Color originalForeColor;
        public Form6()
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
        private void Form6_Load(object sender, EventArgs e)
        {
            pictureBox1.Left = (splitContainer1.Panel1.Width - pictureBox1.Width) / 2;
            btnDangXuat.Left = (splitContainer1.Panel1.Width - btnDangXuat.Width) / 2;

            //Xử lý nút Button
            AddHoverEffect(this);
            CenterButtons(splitContainer1.Panel1);

            //Màu viền nút Button Thông tin cá nhân
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 2;
            button1.FlatAppearance.BorderColor = Color.Violet;

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
        private void button3_Click(object sender, EventArgs e)
        {
            Form5 fr5 = new Form5();
            fr5.Show();
            this.Hide();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form7 fr7 = new Form7();
            fr7.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form9 fr9 = new Form9();
            fr9.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form8 fr8 = new Form8();
            fr8.Show();
            this.Hide();
        }
        // 2. Hàm để lấy dữ liệu từ DB lên TextBox khi load Form
        private void LoadUserData(string maID)
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Ten, Tuoi, Email, Chieu_cao_, Can_nang FROM Nguoidung WHERE maID = @maID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maID", maID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtHoTen.Text = reader["Ten"].ToString();
                        txtTuoi.Text = reader["Tuoi"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                        txtChieuCao.Text = reader["Chieu_cao_"].ToString();
                        txtCanNang.Text = reader["Can_nang"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
                }
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra đầu vào
            if (txtHoTen.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tên của bạn!");
                txtHoTen.Focus();
                return;
            }

            if (txtTuoi.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tuổi!");
                txtTuoi.Focus();
                return;
            }

            // Kiểm tra RadioButton
            if (!radNam.Checked &&
                !radNu.Checked)
            {
                MessageBox.Show("Vui lòng chọn giới tính!");
                return;
            }

            if (txtChieuCao.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tên của bạn!");
                txtChieuCao.Focus();
                return;
            }

            if (txtCanNang.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tuổi!");
                txtCanNang.Focus();
                return;
            }

            if (txtEmail.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tuổi!");
                txtEmail.Focus();
                return;
            }

            //SQL
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                try
                {
                    conn.Open();

                    // 1. Lấy giá trị giới tính
                    string gioiTinh = radNam.Checked ? "Nam" : (radNu.Checked ? "Nữ" : "");

                    // 2. Câu lệnh INSERT (Thêm mới dữ liệu)
                    // Lưu ý: maID là khóa chính nên không được trùng
                    string query = @"INSERT INTO Nguoidung (maID, Ten, Tuoi, Chieu_cao_, Can_nang, Email, GioiTinh) 
                             VALUES (@maID, @ten, @tuoi, @chieucao, @cannang, @email, @gioitinh)";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Bạn cần có 1 ô nhập mã ID hoặc lấy mã tự động, ở đây mình ví dụ lấy từ txtHoTen (hoặc 1 biến tạm)
                    // Tốt nhất bạn nên có 1 TextBox txtMaID để người dùng nhập mã số sinh viên/mã user
                    cmd.Parameters.AddWithValue("@maID", "USER_" + DateTime.Now.Ticks.ToString().Substring(10)); // Tạo ID tạm thời
                    cmd.Parameters.AddWithValue("@ten", txtHoTen.Text);
                    cmd.Parameters.AddWithValue("@tuoi", txtTuoi.Text);
                    cmd.Parameters.AddWithValue("@chieucao", txtChieuCao.Text);
                    cmd.Parameters.AddWithValue("@cannang", txtCanNang.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@gioitinh", gioiTinh);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Đã lưu thông tin người dùng mới thành công!", "Thông báo");
                        // Sau khi lưu xong có thể xóa trắng ô nhập để nhập người tiếp theo
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu: " + ex.Message);
                }
            }
        }
        // Hàm phụ để xóa trắng các ô nhập liệu
        private void ClearFields()
        {
            txtHoTen.Clear();
            txtTuoi.Clear();
            txtEmail.Clear();
            txtChieuCao.Clear();
            txtCanNang.Clear();
            radNam.Checked = false;
            radNu.Checked = false;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                try
                {
                    conn.Open();
                    // 1. Xác định giới tính từ RadioButton
                    string gioiTinh = "";
                    if (radNam.Checked) // Thay 'radioNam' bằng Name đúng của RadioButton Nam
                    {
                        gioiTinh = "Nam";
                    }
                    else if (radNu.Checked) // Thay 'radioNu' bằng Name đúng của RadioButton Nữ
                    {
                        gioiTinh = "Nữ";
                    }
                    // Câu lệnh SQL update đầy đủ các trường dựa trên giao diện của bạn
                    string query = @"UPDATE Nguoidung 
                             SET Ten = @ten, 
                                 Tuoi = @tuoi, 
                                 Email = @email, 
                                 Chieu_cao_ = @chieucao, 
                                 Can_nang = @cannang,
                                 GioiTinh = @gioitinh 
                             WHERE Ten = @ten";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ten", txtHoTen.Text);
                    cmd.Parameters.AddWithValue("@tuoi", txtTuoi.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@chieucao", txtChieuCao.Text);
                    cmd.Parameters.AddWithValue("@cannang", txtCanNang.Text);
                    cmd.Parameters.AddWithValue("@gioitinh", gioiTinh);

                    // Chỗ này bạn cần truyền ID thực tế (ví dụ: "User01")
                    cmd.Parameters.AddWithValue("@maID", "User01");

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                        MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo");
                    else
                        MessageBox.Show("Không tìm thấy người dùng để cập nhật.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
                }
            }
        }
    }
}
