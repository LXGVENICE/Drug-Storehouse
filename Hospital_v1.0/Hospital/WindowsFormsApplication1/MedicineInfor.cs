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
using DLLFullPrint;

namespace xaut_hospital
{
    public partial class MedicineInfor : Form
    {
        private static string[] listRight = { "药号", "药名", "拼音码", "规格", "系数", "小规格", "病房系数", "病房规格", "售价", "费别", "管理级别", "最低库存", "最高库存", "制药否" };
        private static string[] listLeft = {"用法"};
        private static string[] sKind = new string[13];
        private DB dd = new DB();
        public MedicineInfor()
        {
            InitializeComponent();
        }

        private void SetCol()
        {
            while (dataGridView1.RowCount > 1)
                dataGridView1.Rows.RemoveAt(0);
            dataGridView1.Columns.Clear();

            for(int i=0;i<SelectListSet.right.Count;++i)
            {
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn());
                dataGridView1.Columns[i].HeaderText = SelectListSet.right[i];
            }
        } 
        private void MedicineInfor_Load(object sender, EventArgs e)
        {
            SelectListSet.right.AddRange(listRight);
            SelectListSet.left.AddRange(listLeft);
            SetCol();
            string[] Select = { "拼音码", "类别" };//查询的时候后选项
            this.radioButton1.Checked = true;
            this.comboBox1.Items.AddRange(Select);
            this.comboBox2.Items.AddRange(Select);
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;

            this.comboBox3.Text = "";
            this.comboBox4.Text = "";

            
            dd.connect();
            string sql = "select * from 药品类别";
            SqlDataReader sd = dd.query(sql);
            int i = 0;
            while(sd.Read())
            {
                comboBox3.Items.Add(sd.GetString(0));
                comboBox4.Items.Add(sd.GetString(0));
                sKind[i] = sd.GetString(1).Substring(0, 2);
                i++;
            }

            sql = "select * " + " from 药品";
            string ttt = "123";
            dd.close();
            dd.connect();
            DataSet dst = dd.tableQuery(sql, ttt);
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = dst;
            dataGridView1.DataMember = ttt;
            dd.close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "拼音码")
            {
                comboBox3.DropDownStyle = ComboBoxStyle.Simple;
                comboBox3.Text = "";
            }
            else
            {
                comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox3.SelectedIndex = 0;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "拼音码")
            {
                comboBox4.DropDownStyle = ComboBoxStyle.Simple;
                comboBox4.Text = "";
            }
            else
            {
                comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox4.SelectedIndex = 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MeInforListSelect me = new MeInforListSelect();
            me.afterConfirm = new Action(this.SetCol);
            me.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dd.close();
            dd.connect();

            string t = SelectListSet.getString();
            string sql = "select " + t + " from 药品";

            DataSet dst = dd.tableQuery(sql, "123");
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = dst;
            dataGridView1.DataMember = "123";
            dd.close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dd.close();
            string sql = "select " + SelectListSet.getString() + " from 药品 where ";
            string sqlOne = "";
            string sqlTwo = "";
            string IF = "";
            if (comboBox1.Text == "类别")
            {
                sqlOne = " 药号 like '_" + sKind[comboBox3.SelectedIndex] + "%'";
            }
            else
            {
                sqlOne = " 拼音码 like '" + comboBox3.Text + "%'";
            }

            if(comboBox2.Text == "类别")
            {
                sqlTwo = " 药号 like '_" + sKind[comboBox4.SelectedIndex] + "%'";
            }
            else
            {
                sqlTwo = " 拼音码 like '" + comboBox4.Text + "%'";
            }

            if (radioButton1.Checked == true)
            { IF = " and "; }
            else if (radioButton2.Checked)
            { IF = " or "; }
            else
                IF = " and not ";

            sql += sqlOne;
            sql += IF;
            sql += sqlTwo;
            DataSet dst = dd.tableQuery(sql, "123");
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = dst;
            dataGridView1.DataMember = "123";

            dd.close();
        }
        public string getSelectRow()
        {
            dataGridView1.Focus();
            int i = dataGridView1.CurrentRow.Index;
            string res = (String)dataGridView1.Rows[i].Cells["药号"].Value;
            return res;
        }
       

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.Focus();
            if(dataGridView1.RowCount > 1)
            {
                string res = getSelectRow();
                Add_Alter add = new Add_Alter(res);
                add.ShowDialog();
            }
            else
            {
                MessageBox.Show("没有选中的行！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Add_Alter addat = new Add_Alter();
            addat.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Focus();

            int a = dataGridView1.RowCount;
            if(a>1)
            {
                string se = getSelectRow();
                string sql = "delete from 药品 where 药号='" + se + "'";
                dd.close();
                dd.connect();
                if(dd.change(sql))
                {
                    MessageBox.Show("删除成功");
                    button1_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("请先选中一行！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            DataRow dr;
            //设置列表头 
            foreach (DataGridViewColumn headerCell in dataGridView1.Columns)
            {
                dt.Columns.Add(headerCell.HeaderText);
            }
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                dr = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dr[i] = item.Cells[i].Value;
                }
                dt.Rows.Add(dr);
            }
            DataSet dy = new DataSet();
            dy.Tables.Add(dt);
            MyDLL.TakeOver(dy);
        }

    }
    public class SelectListSet
    {
        static public List<string> right = new List<string>();
        static public List<string> left = new List<string>();
        static public string getString()
        {
            string res = null;
            for(int i=0;i<right.Count;++i)
            {
                res += right[i];
                if (i != right.Count - 1)
                    res += ", ";
            }
            return res;
        }
    }
}
