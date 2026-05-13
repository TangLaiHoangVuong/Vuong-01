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
    public partial class Form7 : Form
    {
        Color originalBackColor;
        Color originalForeColor;
        //DataTable
        DataTable dt = new DataTable();
        public Form7()
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
        private void DLSK()
        {
            dt.Columns.Add("Mã BN");
            dt.Columns.Add("Họ tên");
            dt.Columns.Add("Nhịp tim");
            dt.Columns.Add("Huyết áp");
            dt.Columns.Add("Nhiệt độ");
            dt.Columns.Add("Cân nặng");
            dt.Columns.Add("Chiều cao");

            dgvSucKhoe.DataSource = dt;
        }
        private void Form7_Load(object sender, EventArgs e)
        {
            DLSK();
            pictureBox1.Left = (splitContainer1.Panel1.Width - pictureBox1.Width) / 2;
            btnDangXuat.Left = (splitContainer1.Panel1.Width - btnDangXuat.Width) / 2;

            //Xử lý nút Button
            AddHoverEffect(this);
            CenterButtons(splitContainer1.Panel1);

            //Màu viền nút Button QUẢN LÝ PHÂN TÍCH TÍNH TOÁN
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 2;
            button2.FlatAppearance.BorderColor = Color.Violet;

            // Xử lý button đăng xuất
            btnDangXuat.Click += btnDangXuat_Click;
            btnDangXuat.MouseEnter += btnDangXuat_MouseEnter;
            btnDangXuat.MouseLeave += btnDangXuat_MouseLeave;

            //Xử lý Ảnh
            pictureBox1.MouseEnter += pictureBox1_MouseEnter;
            pictureBox1.MouseLeave += pictureBox1_MouseLeave;
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnNhap_Click(object sender, EventArgs e)
        {
            if (txtMaBN.Text == "" || txtHoTen.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            dt.Rows.Add(
                txtMaBN.Text,
                txtHoTen.Text,
                txtNhipTim.Text,
                txtHuyetAp.Text,
                txtNhietDo.Text,
                txtCanNang.Text,
                txtChieuCao.Text
            );

            MessageBox.Show("Thêm thành công!");
            XoaTrang();
        }

        private void dgvSucKhoe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMaBN.Text = dgvSucKhoe.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtHoTen.Text = dgvSucKhoe.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtNhipTim.Text = dgvSucKhoe.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtHuyetAp.Text = dgvSucKhoe.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtNhietDo.Text = dgvSucKhoe.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtCanNang.Text = dgvSucKhoe.Rows[e.RowIndex].Cells[5].Value.ToString();
                txtChieuCao.Text = dgvSucKhoe.Rows[e.RowIndex].Cells[6].Value.ToString();
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (dgvSucKhoe.CurrentRow == null)
            {
                MessageBox.Show("Chọn dòng cần sửa!");
                return;
            }

            int i = dgvSucKhoe.CurrentRow.Index;

            dt.Rows[i][0] = txtMaBN.Text;
            dt.Rows[i][1] = txtHoTen.Text;
            dt.Rows[i][2] = txtNhipTim.Text;
            dt.Rows[i][3] = txtHuyetAp.Text;
            dt.Rows[i][4] = txtNhietDo.Text;
            dt.Rows[i][5] = txtCanNang.Text;
            dt.Rows[i][6] = txtChieuCao.Text;

            MessageBox.Show("Cập nhật thành công!");
            XoaTrang();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSucKhoe.CurrentRow == null)
            {
                MessageBox.Show("Chọn dòng cần xóa!");
                return;
            }

            DialogResult rs = MessageBox.Show(
                "Bạn có chắc muốn xóa?",
                "Xác nhận",
                MessageBoxButtons.YesNo
            );

            if (rs == DialogResult.Yes)
            {
                dt.Rows.RemoveAt(dgvSucKhoe.CurrentRow.Index);
                MessageBox.Show("Xóa thành công!");
                XoaTrang();
            }
        }
        // XÓA TRẮNG
        void XoaTrang()
        {
            txtMaBN.Clear();
            txtHoTen.Clear();
            txtNhipTim.Clear();
            txtHuyetAp.Clear();
            txtNhietDo.Clear();
            txtCanNang.Clear();
            txtChieuCao.Clear();

            txtMaBN.Focus();
        }

        private void labelQuanLy_Click(object sender, EventArgs e)
        {

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

        private void button2_Click(object sender, EventArgs e)
        {
            Form7 fr7 = new Form7();
            fr7.Show();
            this.Hide();
        }
    }
}
