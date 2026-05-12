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
        private void LoadThongBao()
        {
            try
            {
                conn.Open();

                string sql =
                @"SELECT *
                  FROM Thong_bao";

                SqlDataAdapter da =
                    new SqlDataAdapter(sql, conn);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dgvCanhBao.DataSource = dt;

                dgvCanhBao.Columns[0].HeaderText =
                    "Mã TB";

                dgvCanhBao.Columns[1].HeaderText =
                    "Nội dung";

                dgvCanhBao.Columns[2].HeaderText =
                    "Loại thông báo";

                dgvCanhBao.Columns[3].HeaderText =
                    "Thời gian";

                dgvCanhBao.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                dgvCanhBao.DefaultCellStyle.Font =
                    new Font("Arial", 11);

                // Tô màu
                foreach (DataGridViewRow row in dgvCanhBao.Rows)
                {
                    if (row.Cells[2].Value != null)
                    {
                        string loai =
                            row.Cells[2].Value.ToString();

                        if (loai == "Nguy hiểm")
                        {
                            row.Cells[2].Style.ForeColor =
                                Color.Red;
                        }
                        else if (loai == "Cảnh báo")
                        {
                            row.Cells[2].Style.ForeColor =
                                Color.Orange;
                        }
                        else if (loai == "Nhắc nhở")
                        {
                            row.Cells[2].Style.ForeColor =
                                Color.Blue;
                        }
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            LoadThongBao();
            pictureBox1.Left = (splitContainer1.Panel1.Width - pictureBox1.Width) / 2;
            btnDangXuat.Left = (splitContainer1.Panel1.Width - btnDangXuat.Width) / 2;

            //Xử lý nút Button
            AddHoverEffect(this);
            CenterButtons(splitContainer1.Panel1);

            //Màu viền nút Button Thông tin cá nhân
            button5.FlatStyle = FlatStyle.Flat;
            button5.FlatAppearance.BorderSize = 2;
            button5.FlatAppearance.BorderColor = Color.Violet;

            // Xử lý button đăng xuất
            btnDangXuat.Click += btnDangXuat_Click;
            btnDangXuat.MouseEnter += btnDangXuat_MouseEnter;
            btnDangXuat.MouseLeave += btnDangXuat_MouseLeave;

            //Xử lý Ảnh
            pictureBox1.MouseEnter += pictureBox1_MouseEnter;
            pictureBox1.MouseLeave += pictureBox1_MouseLeave;
        }
        string strConn =
        @"Data Source=.;Initial Catalog=QLSK;Integrated Security=True";

        SqlConnection conn;
        
        
       

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
                string noiDung =
                    dgvCanhBao.SelectedRows[0]
                    .Cells[1].Value.ToString();

                MessageBox.Show(
                    "Đã gửi:\n" + noiDung
                );
            }
        }

        // XÓA THÔNG BÁO


        private void button7_Click(object sender, EventArgs e)
        {
            if (dgvCanhBao.SelectedRows.Count > 0)
            {
                string maTB =
                    dgvCanhBao.SelectedRows[0]
                    .Cells[0].Value.ToString();

                conn.Open();

                string sql =
                @"DELETE FROM Thong_bao
                  WHERE Ma_thong_bao=@Ma";

                SqlCommand cmd =
                    new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue(
                    "@Ma",
                    maTB
                );

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show(
                    "Xóa thành công!"
                );

                LoadThongBao();
            }
        }

        private void bt_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                string sql =
                @"SELECT *
                  FROM Chi_so_suc_khoe";

                SqlCommand cmd =
                    new SqlCommand(sql, conn);

                SqlDataReader rd =
                    cmd.ExecuteReader();

                while (rd.Read())
                {
                    string maTB =
                        "TB" + DateTime.Now.Ticks.ToString();

                    string noiDung = "";

                    string loai = "";

                    // Nhịp tim
                    int nhipTim =
                        Convert.ToInt32(rd["Nhip_tim"]);

                    // Nước
                    int nuoc =
                        Convert.ToInt32(rd["Luong_nuoc"]);

                    // Giấc ngủ
                    int ngu =
                        Convert.ToInt32(rd["Thoi_gian_ngu"]);

                    // KIỂM TRA NHỊP TIM
                    if (nhipTim > 120)
                    {
                        noiDung =
                        "Nhịp tim vượt ngưỡng an toàn";

                        loai = "Nguy hiểm";

                        ThemThongBao(
                            maTB,
                            noiDung,
                            loai
                        );
                    }

                    // KIỂM TRA NƯỚC
                    if (nuoc < 1500)
                    {
                        noiDung =
                        "Bạn uống chưa đủ nước";

                        loai = "Nhắc nhở";

                        ThemThongBao(
                            maTB + "1",
                            noiDung,
                            loai
                        );
                    }

                    // KIỂM TRA GIẤC NGỦ
                    if (ngu < 6)
                    {
                        noiDung =
                        "Bạn ngủ chưa đủ giấc";

                        loai = "Cảnh báo";

                        ThemThongBao(
                            maTB + "2",
                            noiDung,
                            loai
                        );
                    }
                }

                rd.Close();

                conn.Close();

                MessageBox.Show(
                    "Kiểm tra hoàn tất!"
                );

                LoadThongBao();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        // THÊM THÔNG BÁO
        private void ThemThongBao(
            string maTB,
            string noiDung,
            string loai
        )
        {
            string sql =
            @"INSERT INTO Thong_bao
            VALUES
            (
                @MaTB,
                @NoiDung,
                @Loai,
                GETDATE()
            )";

            SqlCommand cmd =
                new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue(
                "@MaTB",
                maTB
            );

            cmd.Parameters.AddWithValue(
                "@NoiDung",
                noiDung
            );

            cmd.Parameters.AddWithValue(
                "@Loai",
                loai
            );

            cmd.ExecuteNonQuery();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                string sql =
                @"SELECT *
                  FROM Thong_bao
                  WHERE Noi_dung_
                  LIKE '%' + @NoiDung + '%'";

                SqlDataAdapter da =
                    new SqlDataAdapter(sql, conn);

                da.SelectCommand.Parameters.AddWithValue(
                    "@NoiDung",
                    txtTim.Text
                );

                DataTable dt =
                    new DataTable();

                da.Fill(dt);

                dgvCanhBao.DataSource = dt;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {

             txtTim.Clear();
            LoadThongBao();
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

        private void button5_Click(object sender, EventArgs e)
        {


        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form8 fr8 = new Form8();
            fr8.Show();
            this.Hide();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form6 fr6 = new Form6();
            fr6.Show();
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Form7 fr7 = new Form7();
            fr7.Show();
            this.Hide();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Form5 fr5 = new Form5();
            fr5.Show();
            this.Hide();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Form9 fr9 = new Form9();
            fr9.Show();
            this.Hide();
        }
    }
   }

