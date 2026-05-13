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
        //KNCSDL//
        private static SqlConnection cnn = new SqlConnection();
        public static void MoKetNoi()
        {
            try
            {
                string sqlcon = @"Data Source=DESKTOP-NT4S0AQ;Initial Catalog=QLSK;Integrated Security=True";
                cnn.ConnectionString = sqlcon;
                if (cnn.State == ConnectionState.Closed)
                    cnn.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Ket noi khong thanh cong");
            }
        }
        public static void DongKetNoi()
        {
            if (cnn.State == ConnectionState.Open)
                cnn.Close();
        }
        public static DataTable DocDuLieu(string sql)
        {
            MoKetNoi();
            SqlCommand cd = new SqlCommand(sql, cnn);
            SqlDataReader dr = cd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            DongKetNoi();
            return dt;
        }
        public static void ThucThiTruyVan(string sql)
        {
            MoKetNoi();
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.ExecuteNonQuery();
            DongKetNoi();
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
                MoKetNoi();

                string sql =
                @"SELECT *
          FROM Thong_bao";

                SqlDataAdapter da =
                    new SqlDataAdapter(sql, cnn);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dgvCanhBao.DataSource = dt;

                dgvCanhBao.Columns[0].HeaderText = "Mã TB";
                dgvCanhBao.Columns[1].HeaderText = "Nội dung";
                dgvCanhBao.Columns[2].HeaderText = "Loại thông báo";
                dgvCanhBao.Columns[3].HeaderText = "Thời gian";

                dgvCanhBao.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                dgvCanhBao.DefaultCellStyle.Font =
                    new Font("Arial", 11);

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

                DongKetNoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        class CanhBao
        {
            // LẤY DANH SÁCH THÔNG BÁO
            public static DataTable ThongTinThongBao()
            {
                string sql =
                @"SELECT *
              FROM Thong_bao";

                DataTable dt =
                    new DataTable();

                dt = Form9.DocDuLieu(sql);

                return dt;
            }

            // THÊM THÔNG BÁO
            public static void ThemThongBao(
                string maTB,
                string noiDung,
                string loai
            )
            {
                string sql =
                @"INSERT INTO Thong_bao
            VALUES
            (
                N'" + maTB + @"',
                N'" + noiDung + @"',
                N'" + loai + @"',
                GETDATE()
            )";

                Form9.ThucThiTruyVan(sql);
            }

            // XÓA THÔNG BÁO
            public static void XoaThongBao(string maTB)
            {
                string sql =
                @"DELETE FROM Thong_bao
              WHERE Ma_thong_bao = N'" + maTB + "'";

                Form9.ThucThiTruyVan(sql);
            }

            // TÌM KIẾM
            public static DataTable TimThongBao(
                string noiDung
            )
            {
                string sql =
                @"SELECT *
              FROM Thong_bao
              WHERE Noi_dung_
              LIKE N'%" + noiDung + "%'";

                DataTable dt =
                    new DataTable();

                dt = Form9.DocDuLieu(sql);

                return dt;
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
            try
            {
                if (dgvCanhBao.SelectedRows.Count > 0)
                {
                    string maTB =
                        dgvCanhBao.SelectedRows[0]
                        .Cells[0].Value.ToString();

                    CanhBao.XoaThongBao(maTB);

                    MessageBox.Show(
                        "Xóa thành công!"
                    );

                    LoadThongBao();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Không thể xóa thông báo!"
                );
            }
        }

        private void bt_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt =
                    new DataTable();

                dt = DocDuLieu(
                    @"SELECT *
              FROM Chi_so_suc_khoe"
                );

                foreach (DataRow rd in dt.Rows)
                {
                    string maTB =
                        "TB" + DateTime.Now.Ticks.ToString();

                    string noiDung = "";

                    string loai = "";

                    int nhipTim =
                        Convert.ToInt32(
                            rd["Nhip_tim"]
                        );

                    int nuoc =
                        Convert.ToInt32(
                            rd["Luong_nuoc"]
                        );

                    int ngu =
                        Convert.ToInt32(
                            rd["Thoi_gian_ngu"]
                        );

                    // NHỊP TIM
                    if (nhipTim > 120)
                    {
                        noiDung =
                            "Nhịp tim vượt ngưỡng an toàn";

                        loai = "Nguy hiểm";

                        CanhBao.ThemThongBao(
                            maTB,
                            noiDung,
                            loai
                        );
                    }

                    // NƯỚC
                    if (nuoc < 1500)
                    {
                        noiDung =
                            "Bạn uống chưa đủ nước";

                        loai = "Nhắc nhở";

                        CanhBao.ThemThongBao(
                            maTB + "1",
                            noiDung,
                            loai
                        );
                    }

                    // GIẤC NGỦ
                    if (ngu < 6)
                    {
                        noiDung =
                            "Bạn ngủ chưa đủ giấc";

                        loai = "Cảnh báo";

                        CanhBao.ThemThongBao(
                            maTB + "2",
                            noiDung,
                            loai
                        );
                    }
                }

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
                new SqlCommand(sql, cnn);

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
            DataTable dt =new DataTable();

            dt = CanhBao.TimThongBao(txtTim.Text );

            dgvCanhBao.DataSource = dt;
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

        private void button4_Click(object sender, EventArgs e)
        {
            Form8 fr8 = new Form8();
            fr8.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
   }

