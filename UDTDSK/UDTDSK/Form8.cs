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

            //ListView
            LoadDataToListView();
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;

            //txtTienDo
            TinhTienDo();
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
            Form8 fr8 = new Form8();
            fr8.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form9 fr9 = new Form9();
            fr9.Show();
            this.Hide();
        }
        // ====================== LẤY LOẠI MỤC TIÊU ======================
        private string GetLoaiMucTieu()
        {
            if (radCanNang.Checked)
                return "Cân nặng";

            if (radSoBuoc.Checked)
                return "Số bước chân";

            if (radLuongNuoc.Checked)
                return "Lượng nước uống";

            if (radCalorie.Checked)
                return "Calories";

            return "";
        }
        // ====================== TẠO MÃ MỤC TIÊU ======================
        private string TaoMaMucTieu()
        {
            string maMoi = "MT001";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query =
                    "SELECT TOP 1 Ma_muc_tieu FROM Muc_tieu ORDER BY Ma_muc_tieu DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string maCu = result.ToString();

                    // Lấy phần số phía sau MT
                    int so = int.Parse(maCu.Substring(2));

                    // Tăng lên 1
                    so++;

                    // Format thành MT001, MT002... => D2 thành MT01, MT02,...
                    maMoi = "MT" + so.ToString("D3");
                }
            }

            return maMoi;
        }
        // ====================== Button Lưu Tiến Độ ======================
        private void btnLuuTienDo_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem người dùng đã chọn mục tiêu nào chưa
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một mục tiêu trong danh sách để cập nhật!");
                return;
            }

            // 2. Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(txtTienDo.Text))
            {
                MessageBox.Show("Vui lòng nhập tiến độ thực tế!");
                return;
            }

            // Kiểm tra Session người dùng
            string currentUserId = UserSession.CurrentUserID;
            if (string.IsNullOrEmpty(currentUserId))
            {
                MessageBox.Show("Phiên đăng nhập hết hạn. Vui lòng đăng nhập lại!");
                return;
            }

            try
            {
                string moTa = listView1.SelectedItems[0].Text;
                double tienDoMoi;
                if (!double.TryParse(txtTienDo.Text, out tienDoMoi))
                {
                    MessageBox.Show("Tiến độ phải là một số hợp lệ!");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // --- BƯỚC A: Lấy giá trị mục tiêu của ĐÚNG người dùng này ---
                    string queryGet = "SELECT gia_tri FROM Muc_tieu WHERE mo_ta = @moTa AND maID = @maID";
                    SqlCommand cmdGet = new SqlCommand(queryGet, conn);
                    cmdGet.Parameters.AddWithValue("@moTa", moTa);
                    cmdGet.Parameters.AddWithValue("@maID", currentUserId);

                    object result = cmdGet.ExecuteScalar();
                    if (result == null)
                    {
                        MessageBox.Show("Không tìm thấy dữ liệu mục tiêu phù hợp!");
                        return;
                    }

                    string giaTriMTStr = result.ToString();
                    // Tách số từ chuỗi (VD: "50 kg" -> 50)
                    var match = System.Text.RegularExpressions.Regex.Match(giaTriMTStr, @"\d+");
                    if (!match.Success)
                    {
                        MessageBox.Show("Định dạng giá trị mục tiêu trong DB không hợp lệ!");
                        return;
                    }
                    double giaTriMT = double.Parse(match.Value);

                    // 3. Tính toán tỉ lệ %
                    double phanTram = (tienDoMoi / giaTriMT) * 100;
                    txtGiatriHT.Text = phanTram.ToString("F1") + "%";

                    // --- BƯỚC B: Cập nhật vào CSDL kèm theo điều kiện maID ---
                    string queryUpdate = "UPDATE Muc_tieu SET Gia_tri_hien_tai = @ht WHERE mo_ta = @moTa AND maID = @maID";
                    SqlCommand cmdUpdate = new SqlCommand(queryUpdate, conn);

                    cmdUpdate.Parameters.AddWithValue("@ht", txtGiatriHT.Text);
                    cmdUpdate.Parameters.AddWithValue("@moTa", moTa);
                    cmdUpdate.Parameters.AddWithValue("@maID", currentUserId); // Thêm tham số maID ở đây

                    int rowsAffected = cmdUpdate.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Lưu tiến độ thành công!");
                        LoadDataToListView(); // Làm mới danh sách
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu nào được cập nhật. Vui lòng kiểm tra lại!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }
        // ====================== ListView ======================
        private void LoadDataToListView()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Thêm điều kiện WHERE maID = @maID
                    string query = "SELECT mo_ta, Loai_MT, gia_tri, Thoi_han, Trang_thai, Gia_tri_hien_tai " +
                                   "FROM Muc_tieu WHERE maID = @maID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maID", UserSession.CurrentUserID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    listView1.Items.Clear(); // Xóa dữ liệu cũ trên giao diện

                    while (reader.Read())
                    {
                        // Tạo một dòng mới cho ListView
                        ListViewItem item = new ListViewItem(reader["mo_ta"].ToString());
                        item.SubItems.Add(reader["Loai_MT"].ToString());
                        item.SubItems.Add(reader["gia_tri"].ToString());

                        // Định dạng ngày tháng cho đẹp (dd/MM/yyyy)
                        DateTime thoiHan = Convert.ToDateTime(reader["Thoi_han"]);
                        item.SubItems.Add(thoiHan.ToString("dd/MM/yyyy"));

                        item.SubItems.Add(reader["Trang_thai"].ToString());

                        // Gán giá trị % vào SubItems thứ 5 (Cột thứ 6)
                        string phanTram = reader["Gia_tri_hien_tai"] != DBNull.Value ? reader["Gia_tri_hien_tai"].ToString() : "0%";
                        item.SubItems.Add(phanTram);

                        item.Tag = reader["mo_ta"].ToString();

                        listView1.Items.Add(item); // Thêm dòng vào ListView
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
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

            // Trạng thái mặc định
            string loaiMucTieu = GetLoaiMucTieu();
            string maMucTieu = TaoMaMucTieu();
            string trangThai = "Chưa hoàn thành";

            // Lấy ID người dùng hiện tại từ Session
            string currentUserId = UserSession.CurrentUserID;

            if (string.IsNullOrEmpty(currentUserId))
            {
                MessageBox.Show("Lỗi: Không tìm thấy thông tin đăng nhập. Vui lòng đăng nhập lại!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Cập nhật câu lệnh SQL: Thêm cột maID và tham số @maID
                    string query = @"INSERT INTO Muc_tieu 
                            (Ma_muc_tieu, mo_ta, Loai_MT, gia_tri, Gia_tri_hien_tai, Thoi_han, Trang_thai, maID) 
                            VALUES 
                            (@MaMT, @TenMT, @LoaiMT, @GiaTri, @GiaTriHT, @ThoiHan, @TrangThai, @maID)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaMT", maMucTieu);
                    cmd.Parameters.AddWithValue("@TenMT", txtTenMucTieu.Text);
                    cmd.Parameters.AddWithValue("@LoaiMT", loaiMucTieu);
                    cmd.Parameters.AddWithValue("@GiaTri", txtMTHoanThanh.Text + " " + cboDonVi.Text);
                    cmd.Parameters.AddWithValue("@GiaTriHT", txtGiatriHT.Text);
                    cmd.Parameters.AddWithValue("@ThoiHan", dtpThoiHan.Value);
                    cmd.Parameters.AddWithValue("@TrangThai", trangThai);

                    // Truyền ID người dùng vào đây
                    cmd.Parameters.AddWithValue("@maID", currentUserId);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thiết lập mục tiêu thành công!");
                }

                //ListView
                LoadDataToListView();

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

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem có dòng nào đang được chọn không
            if (listView1.SelectedItems.Count > 0)
            {
                // Lấy dòng đầu tiên trong danh sách các dòng được chọn
                ListViewItem item = listView1.SelectedItems[0];

                // 1. Hiển thị Tên mục tiêu (Cột 0)
                txtTenMucTieu.Text = item.Text;

                // 2. Xử lý Loại mục tiêu (Cột 1) để check vào RadioButton tương ứng
                string loaiMT = item.SubItems[1].Text;
                radCanNang.Checked = (loaiMT == "Cân nặng");
                radSoBuoc.Checked = (loaiMT == "Số bước chân");
                radLuongNuoc.Checked = (loaiMT == "Lượng nước uống");
                radCalorie.Checked = (loaiMT == "Calories");

                // 3. Xử lý Giá trị và Đơn vị (Cột 2 - Ví dụ: "50 kg")
                string giaTriFull = item.SubItems[2].Text;
                // Tách số và chữ (Đơn giản nhất là dùng Split)
                string[] parts = giaTriFull.Split(' ');
                if (parts.Length >= 1) txtMTHoanThanh.Text = parts[0];
                if (parts.Length >= 2) cboDonVi.Text = parts[1];

                // 4. Hiển thị Thời hạn (Cột 3)
                try
                {
                    string dateString = item.SubItems[3].Text.Trim();
                    // Thử dùng Parse thông thường để hệ thống tự nhận diện định dạng
                    dtpThoiHan.Value = DateTime.Parse(dateString);
                }
                catch
                {
                    // Nếu vẫn lỗi, gán ngày hiện tại để tránh crash chương trình
                    dtpThoiHan.Value = DateTime.Now;
                }

                // 5. Hiển thị tiến độ % (Lấy từ SubItems index 5)
                if (item.SubItems.Count > 5) // Kiểm tra để tránh lỗi nếu dòng không có đủ cột
                {
                    txtGiatriHT.Text = item.SubItems[5].Text;
                }
                else
                {
                    txtGiatriHT.Text = "0%";
                }
                TinhTienDo();
            }
        }
        // ====================== txt Tiến Độ hoàn thành ======================
        private void TinhTienDo()
        {
            double mucTieu;
            double giaTriHT;

            // 1. Loại bỏ ký tự '%' để có thể ép kiểu sang số
            string strGiaTriHT = txtGiatriHT.Text.Replace("%", "").Trim();

            // 2. QUAN TRỌNG: Dùng biến strGiaTriHT đã xử lý thay vì txtGiatriHT.Text
            if (double.TryParse(txtMTHoanThanh.Text, out mucTieu) &&
                double.TryParse(strGiaTriHT, out giaTriHT))
            {
                if (mucTieu > 0)
                {
                    // Công thức tính ngược từ phần trăm ra con số thực tế
                    double tienDo = (giaTriHT * mucTieu) / 100;
                    txtTienDo.Text = tienDo.ToString("0.#");
                }
                else
                {
                    txtTienDo.Text = "0";
                }
            }
            else
            {
                txtTienDo.Text = "0";
            }
        }
    }
}
