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
        string strCon = @"Data Source=DESKTOP-NT4S0AQ;Initial Catalog=QLSK;Integrated Security=True";
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

            //Truy cập ID người dùng đã đăng nhập
            if (!string.IsNullOrEmpty(UserSession.CurrentUserID))
            {
                LoadUserData(UserSession.CurrentUserID);
            }
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
                    string query = "SELECT Ten, Tuoi, Email, Chieu_cao_, Can_nang, GioiTinh FROM Nguoidung WHERE maID = @maID";
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

                        string gt = reader["GioiTinh"].ToString();
                        if (gt == "Nam") radNam.Checked = true;
                        else if (gt == "Nữ") radNu.Checked = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
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
            if (string.IsNullOrEmpty(UserSession.CurrentUserID))
            {
                MessageBox.Show("Không xác định được người dùng hiện tại!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(strCon))
            {
                try
                {
                    conn.Open();
                    string gioiTinh = radNam.Checked ? "Nam" : (radNu.Checked ? "Nữ" : "");

                    // Câu lệnh UPDATE dựa trên maID của người đang đăng nhập
                    string query = @"UPDATE Nguoidung 
                             SET Ten = @ten, 
                                 Tuoi = @tuoi, 
                                 Email = @email, 
                                 Chieu_cao_ = @chieucao, 
                                 Can_nang = @cannang,
                                 GioiTinh = @gioitinh 
                             WHERE maID = @maID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ten", txtHoTen.Text);
                    cmd.Parameters.AddWithValue("@tuoi", txtTuoi.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@chieucao", txtChieuCao.Text);
                    cmd.Parameters.AddWithValue("@cannang", txtCanNang.Text);
                    cmd.Parameters.AddWithValue("@gioitinh", gioiTinh);
                    cmd.Parameters.AddWithValue("@maID", UserSession.CurrentUserID); // Quan trọng nhất ở đây

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                        MessageBox.Show("Cập nhật thông tin cá nhân thành công!", "Thông báo");
                    else
                        MessageBox.Show("Cập nhật thất bại. Vui lòng kiểm tra lại.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
                }
            }
        }
    }
}
