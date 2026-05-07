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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double canNang, chieuCao;
            if (!double.TryParse(txtCanNang.Text, out canNang) ||
                !double.TryParse(txtChieuCao.Text, out chieuCao))
            {
                MessageBox.Show("Vui lòng nhập số hợp lệ");
                return;
            }

            chieuCao /= 100;

            double bmi = canNang / (chieuCao * chieuCao);

            txtBMI.Text = bmi.ToString("0.00");

            if (bmi < 18.5)
            {
                txtPhanLoai.Text = "Gầy";
                txtDanhGia.Text = "Thiếu cân";
            }
            else if (bmi < 25)
            {
                txtPhanLoai.Text = "Bình thường";
                txtDanhGia.Text = "Sức khỏe tốt";
            }
            else if (bmi < 30)
            {
                txtPhanLoai.Text = "Thừa cân";
                txtDanhGia.Text = "Cần kiểm soát cân nặng";
            }
            else
            {
                txtPhanLoai.Text = "Béo phì";
                txtDanhGia.Text = "Nguy cơ sức khỏe cao";
            }
        }
    }
}
