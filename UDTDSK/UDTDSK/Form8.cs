using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDTDSK
{
    public partial class Form8 : Form
    {
        Color originalBackColor;
        Color originalForeColor;
        public Form8()
        {
            InitializeComponent();
            


            conn = new SqlConnection(strConn);
        }
        string strConn = @"Data Source=.;Initial Catalog=QLSK;Integrated Security=True";
        SqlConnection conn;


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
        
            
        private void LoadMucTieu()
        {
            try
            {
                conn.Open();

                string sql =
                @"SELECT *
                  FROM Muc_tieu";

                SqlDataAdapter da =
                    new SqlDataAdapter(sql, conn);

                DataTable dt =
                    new DataTable();

                da.Fill(dt);

                dgvMucTieu.DataSource = dt;

                dgvMucTieu.Columns[0].HeaderText =
                    "Mã mục tiêu";

                dgvMucTieu.Columns[1].HeaderText =
                    "Mô tả";

                dgvMucTieu.Columns[2].HeaderText =
                    "Giá trị mục tiêu";

                dgvMucTieu.Columns[3].HeaderText =
                    "Giá trị hiện tại";

                dgvMucTieu.Columns[4].HeaderText =
                    "Trạng thái";

                dgvMucTieu.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            pictureBox1.Left = (splitContainer1.Panel1.Width - pictureBox1.Width) / 2;
            btnDangXuat.Left = (splitContainer1.Panel1.Width - btnDangXuat.Width) / 2;

            //Xử lý nút Button
            AddHoverEffect(this);
            CenterButtons(splitContainer1.Panel1);

            // Xử lý button đăng xuất
            btnDangXuat.Click += btnDangXuat_Click;
            btnDangXuat.MouseEnter += btnDangXuat_MouseEnter;
            btnDangXuat.MouseLeave += btnDangXuat_MouseLeave;

            //Xử lý Ảnh
            pictureBox1.MouseEnter += pictureBox1_MouseEnter;
            pictureBox1.MouseLeave += pictureBox1_MouseLeave;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                string sql =
                @"INSERT INTO Muc_tieu
                VALUES
                (
                    @Ma,
                    @MoTa,
                    @GiaTri,
                    @GiaTriHT,
                    @TrangThai
                )";

                SqlCommand cmd =
                    new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue(
                    "@Ma",
                    txtMaMT.Text
                );

                cmd.Parameters.AddWithValue(
                    "@MoTa",
                    txtMoTa.Text
                );

                cmd.Parameters.AddWithValue(
                    "@GiaTri",
                    txtGiaTri.Text
                );

                cmd.Parameters.AddWithValue(
                    "@GiaTriHT",
                    txtGiaTriHT.Text
                );

                cmd.Parameters.AddWithValue(
                    "@TrangThai",
                    txtTrangThai.Text
                );

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show(
                    "Thêm mục tiêu thành công!"
                );

                LoadMucTieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                string sql =
                @"UPDATE Muc_tieu
                  SET
                    mo_ta=@MoTa,
                    gia_tri=@GiaTri,
                    Gia_tri_hien_tai=@GiaTriHT,
                    Trang_thai=@TrangThai
                  WHERE Ma_muc_tieu=@Ma";

                SqlCommand cmd =
                    new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue(
                    "@Ma",
                    txtMaMT.Text
                );

                cmd.Parameters.AddWithValue(
                    "@MoTa",
                    txtMoTa.Text
                );

                cmd.Parameters.AddWithValue(
                    "@GiaTri",
                    txtGiaTri.Text
                );

                cmd.Parameters.AddWithValue(
                    "@GiaTriHT",
                    txtGiaTriHT.Text
                );

                cmd.Parameters.AddWithValue(
                    "@TrangThai",
                    txtTrangThai.Text
                );

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show(
                    "Cập nhật thành công!"
                );

                LoadMucTieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                string sql =
                @"DELETE FROM Muc_tieu
                  WHERE Ma_muc_tieu=@Ma";

                SqlCommand cmd =
                    new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue(
                    "@Ma",
                    txtMaMT.Text
                );

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show(
                    "Xóa thành công!"
                );

                LoadMucTieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                int mucTieu =
                    Convert.ToInt32(txtGiaTri.Text);

                int hienTai =
                    Convert.ToInt32(txtGiaTriHT.Text);

                if (hienTai >= mucTieu)
                {
                    txtTrangThai.Text =
                        "Hoàn thành";
                }
                else
                {
                    txtTrangThai.Text =
                        "Chưa hoàn thành";
                }
            }
            catch
            {
                MessageBox.Show(
                    "Giá trị phải là số!"
                );
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            txtMaMT.Clear();
            txtMoTa.Clear();
            txtGiaTri.Clear();
            txtGiaTriHT.Clear();
            txtTrangThai.Clear();

            LoadMucTieu();
        }

        private void dgvMucTieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMaMT.Text =
                    dgvMucTieu.Rows[e.RowIndex]
                    .Cells[0].Value.ToString();

                txtMoTa.Text =
                    dgvMucTieu.Rows[e.RowIndex]
                    .Cells[1].Value.ToString();

                txtGiaTri.Text =
                    dgvMucTieu.Rows[e.RowIndex]
                    .Cells[2].Value.ToString();

                txtGiaTriHT.Text =
                    dgvMucTieu.Rows[e.RowIndex]
                    .Cells[3].Value.ToString();

                txtTrangThai.Text =
                    dgvMucTieu.Rows[e.RowIndex]
                    .Cells[4].Value.ToString();
            }
        }
    }
}
