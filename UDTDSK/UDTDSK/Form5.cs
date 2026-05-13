using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDTDSK
{
    public partial class Form5 : Form
    {
        Color originalBackColor;
        Color originalForeColor;
        string strCon = @"Data Source=DESKTOP-NT4S0AQ;Initial Catalog=QLSK;Integrated Security=True";
        SqlConnection sqlCon = null;
        public Form5()
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
        private void Form5_Load(object sender, EventArgs e)
        {
            pictureBox1.Left = (splitContainer1.Panel1.Width - pictureBox1.Width) / 2;
            btnDangXuat.Left = (splitContainer1.Panel1.Width - btnDangXuat.Width) / 2;

            //Xử lý nút Button
            AddHoverEffect(this);
            CenterButtons(splitContainer1.Panel1);

            //Màu viền nút Button QUẢN LÝ PHÂN TÍCH TÍNH TOÁN
            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 2;
            button3.FlatAppearance.BorderColor = Color.Violet;

            // Xử lý button đăng xuất
            btnDangXuat.Click += btnDangXuat_Click;
            btnDangXuat.MouseEnter += btnDangXuat_MouseEnter;
            btnDangXuat.MouseLeave += btnDangXuat_MouseLeave;

            //Xử lý Ảnh
            pictureBox1.MouseEnter += pictureBox1_MouseEnter;
            pictureBox1.MouseLeave += pictureBox1_MouseLeave;

            LoadDataToGrid();//1
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

        private void button6_Click(object sender, EventArgs e)
        {
            if (txtCanNang.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập cân nặng!");
                txtCanNang.Focus();
                return;
            }

            if (txtChieuCao.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập chiều cao!");
                txtChieuCao.Focus();
                return;
            }

            try
            {
                
                double canNang = double.Parse(txtCanNang.Text.Trim());
                double chieuCao = double.Parse(txtChieuCao.Text.Trim());
                double chieuCaoMet = chieuCao / 100;
                double bmi = canNang / (chieuCaoMet * chieuCaoMet);

                string phanLoai = "";
                string danhGia = "";

                if (bmi < 18.5) { phanLoai = "Gầy"; danhGia = "Thiếu cân"; }
                else if (bmi < 25) { phanLoai = "Bình thường"; danhGia = "Sức khỏe tốt"; }
                else if (bmi < 30) { phanLoai = "Thừa cân"; danhGia = "Cần kiểm soát cân nặng"; }
                else { phanLoai = "Béo phì"; danhGia = "Nguy cơ sức khỏe cao"; }

                txtBMI.Text = bmi.ToString("0.00");
                txtPhanLoai.Text = phanLoai;
                txtDanhGia.Text = danhGia;

                
                if (sqlCon == null) sqlCon = new SqlConnection(strCon);
                if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();

                
                string maPT = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                string sql = "INSERT INTO Phan_tich (maPT, BMI, Can_nang, Chieu_cao, Phan_tich_xu_huong, Xu_li_du_lieu) " +
                     "VALUES (@maPT, @bmi, @cn, @cc, @loai, @dg)";

                SqlCommand sqlCmd = new SqlCommand(sql, sqlCon);
                sqlCmd.Parameters.AddWithValue("@maPT", maPT);
                sqlCmd.Parameters.AddWithValue("@bmi", Math.Round(bmi, 2));
                sqlCmd.Parameters.AddWithValue("@cn", double.Parse(txtCanNang.Text)); // Lưu cân nặng
                sqlCmd.Parameters.AddWithValue("@cc", double.Parse(txtChieuCao.Text)); // Lưu chiều cao
                sqlCmd.Parameters.AddWithValue("@loai", phanLoai);
                sqlCmd.Parameters.AddWithValue("@dg", danhGia);

                int kq = sqlCmd.ExecuteNonQuery();

                if (kq > 0)
                {
                    
                    LoadDataToGrid();
                    MessageBox.Show("Tính toán và lưu dữ liệu thành công!", "Thông báo");
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
        private void LoadDataToGrid()
        {
            try
            {
                if (sqlCon == null) sqlCon = new SqlConnection(strCon);
                if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();

                string query = "SELECT maPT, Can_nang, Chieu_cao, BMI, Phan_tich_xu_huong, Xu_li_du_lieu FROM Phan_tich";
                SqlDataAdapter da = new SqlDataAdapter(query, sqlCon);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvPhanTich.DataSource = null;
                dgvPhanTich.Columns.Clear();
                dgvPhanTich.DataSource = dt;

                
                dgvPhanTich.Columns["maPT"].HeaderText = "Ngày đo";
                dgvPhanTich.Columns["Can_nang"].HeaderText = "Cân nặng (kg)";
                dgvPhanTich.Columns["Chieu_cao"].HeaderText = "Chiều cao (cm)";
                dgvPhanTich.Columns["BMI"].HeaderText = "BMI";
                dgvPhanTich.Columns["Phan_tich_xu_huong"].HeaderText = "Phân loại";
                dgvPhanTich.Columns["Xu_li_du_lieu"].HeaderText = "Đánh giá";

                dgvPhanTich.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị bảng: " + ex.Message);
            }
            finally
            {
                if (sqlCon != null && sqlCon.State == ConnectionState.Open)
                    sqlCon.Close();
            }
        }
        
    
        private void Form5_Load_1(object sender, EventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem người dùng đã chọn dòng nào chưa
            if (dgvPhanTich.SelectedRows.Count > 0)
            {
                // Lấy giá trị maPT (Ngày đo) của dòng đang chọn
                string maPT_CanXoa = dgvPhanTich.CurrentRow.Cells["maPT"].Value.ToString();

                // Hỏi xác nhận trước khi xóa (giống Form đăng ký của bạn)
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa bản ghi ngày " + maPT_CanXoa + " không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // 2. Kết nối CSDL (Dùng đúng biến strCon và sqlCon của bạn)
                        if (sqlCon == null) sqlCon = new SqlConnection(strCon);
                        if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();

                        // Câu lệnh xóa dựa trên maPT
                        string sqlXoa = "DELETE FROM Phan_tich WHERE maPT = @maPT";
                        SqlCommand cmd = new SqlCommand(sqlXoa, sqlCon);
                        cmd.Parameters.AddWithValue("@maPT", maPT_CanXoa);

                        int kq = cmd.ExecuteNonQuery();

                        if (kq > 0)
                        {
                            MessageBox.Show("Đã xóa dữ liệu thành công!");
                            // 3. Cập nhật lại bảng sau khi xóa
                            LoadDataToGrid();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa: " + ex.Message);
                    }
                    finally
                    {
                        if (sqlCon != null && sqlCon.State == ConnectionState.Open)
                            sqlCon.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng trong bảng để xóa!");
            }
        }
    }
}
