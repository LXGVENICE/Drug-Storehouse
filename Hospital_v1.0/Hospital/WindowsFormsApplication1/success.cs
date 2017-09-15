using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xaut_hospital
{
    public partial class success : Form
    {
        public success()
        {
            InitializeComponent();
        }

        private void success_Load(object sender, EventArgs e)
        {
 
        }

        private void success_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定要退出程序吗？", "退出程序", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void success_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Insert frm = new Insert();
            frm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            inStoreQuery f = new inStoreQuery();
            f.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            outStoreQuery oo = new outStoreQuery();
            oo.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Out ou = new Out();
            ou.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MedicineInfor me = new MedicineInfor();
            me.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TakeMed tt = new TakeMed();
            tt.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            drugCategory dc = new drugCategory();
            dc.ShowDialog();
        }
    }
}
