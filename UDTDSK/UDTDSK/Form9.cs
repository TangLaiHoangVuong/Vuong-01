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
    public partial class Form9 : Form
    {
        Color originalBackColor;
        Color originalForeColor;
        public Form9()
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
        private void Form9_Load(object sender, EventArgs e)
        {
            pictureBox1.Left = (splitContainer1.Panel1.Width - pictureBox1.Width) / 2;
            btnDangXuat.Left = (splitContainer1.Panel1.Width - btnDangXuat.Width) / 2;

            //Xử lý nút Button
            AddHoverEffect(this);
            CenterButtons(splitContainer1.Panel1);

            //Xử lý Ảnh
            pictureBox1.MouseEnter += pictureBox1_MouseEnter;
            pictureBox1.MouseLeave += pictureBox1_MouseLeave;
        }
        private void HienThiCanhBao()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("STT");
            dt.Columns.Add("Chỉ số");
            dt.Columns.Add("Giá trị");
            dt.Columns.Add("Trạng thái");
            dt.Columns.Add("Thời gian");
            dgvCanhBao.DataSource = dt;
            foreach (DataGridViewRow row in dgvCanhBao.Rows)
            {
                if (row.Cells[3].Value != null)
                {
                    string trangThai = row.Cells[3].Value.ToString();

                    if (trangThai == "Nguy hiểm")
                    {
                        row.Cells[3].Style.ForeColor = Color.Red;
                    }
                    else if (trangThai == "Cao")
                    {
                        row.Cells[3].Style.ForeColor = Color.Orange;
                    }
                    else
                    {
                        row.Cells[3].Style.ForeColor = Color.Blue;
                    }
                }
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvCanhBao_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dgvCanhBao.SelectedRows.Count > 0)
            {
                string chiSo = dgvCanhBao.SelectedRows[0].Cells[1].Value.ToString();

                MessageBox.Show(
                    "Đã gửi thông báo cảnh báo cho chỉ số: " + chiSo,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            else
            {
                MessageBox.Show(
                    "Vui lòng chọn cảnh báo!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dgvCanhBao.SelectedRows.Count > 0)
            {
                dgvCanhBao.Rows.RemoveAt(
                    dgvCanhBao.SelectedRows[0].Index
                );

                MessageBox.Show(
                    "Xóa cảnh báo thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    "Vui lòng chọn cảnh báo cần xóa!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void bt_Click(object sender, EventArgs e)
        {
            if (dgvCanhBao.SelectedRows.Count > 0)
            {
                string chiSo =
                    dgvCanhBao.SelectedRows[0].Cells[1].Value.ToString();

                string giaTri =
                    dgvCanhBao.SelectedRows[0].Cells[2].Value.ToString();

                string trangThai =
                    dgvCanhBao.SelectedRows[0].Cells[3].Value.ToString();

                string thoiGian =
                    dgvCanhBao.SelectedRows[0].Cells[4].Value.ToString();

                MessageBox.Show(
                    "Chỉ số: " + chiSo +
                    "\nGiá trị: " + giaTri +
                    "\nTrạng thái: " + trangThai +
                    "\nThời gian: " + thoiGian,
                    "Chi tiết cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    "Vui lòng chọn cảnh báo!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }
    }
   }

