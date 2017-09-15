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

namespace xaut_hospital
{
    public partial class drugCategory : Form
    {
        private DB dd = new DB();
        public drugCategory()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult MsgBoxResult = MessageBox.Show("你确定退出吗？", "是", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            if (MsgBoxResult == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void 药品类别_Load(object sender, EventArgs e)
        {
            dd.connect();
            string sql = "select * from  药品类别";
            SqlDataReader thisReader = dd.query(sql);
            while (thisReader.Read())
            {
                dataGridView1.Rows.Add(thisReader[0], thisReader[1]);
            }
            dd.close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form f1 = new AddKindDu();
            f1.ShowDialog();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.Focus();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Focus();
            int a = dataGridView1.RowCount;
            if (a > 1)
            {
                int i = dataGridView1.CurrentRow.Index;
                string se = (String)dataGridView1.Rows[i].Cells[1].Value;
                
                string sql = "delete from 药品类别 where 编号='" + se + "'";
                dd.close();
                dd.connect();
                if (dd.change(sql))
                {
                    MessageBox.Show("删除成功");
                    刷新_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("请先选中一行！");
            }
        }

        private void 刷新_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dd.close();
            dd.connect();
            string sql = "select * from  药品类别";
            SqlDataReader thisReader = dd.query(sql);
            while (thisReader.Read())
            {
                dataGridView1.Rows.Add(thisReader[0], thisReader[1]);
            }
            dd.close();
        }
    }
}
