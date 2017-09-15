using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DLLFullPrint;

namespace xaut_hospital
{
    //注释信息请查看outStoreQuery
    public partial class inStoreQuery : Form
    {
        private static string[] sStore = {"全部","拼音码","药名","类别","库存合计<最低库存",
                              "库存合计>最高库存","库存合计","管理级别"};
        private static string[] sinStore = { "全部", "拼音码", "药名", "类别","制造商","销售商",
                                        "日期","有效期","管理级别","采购员","经手人" };
        private static string[] liStoreL = {"药号","规格","平均进价","资金合计","最低库存","最高库存",
                                               "拼音码","管理级别"};
        private static string[] liStoreR = {"药名","期初库存","本期入库","本期出库","本期损耗","库存合计"};
        private static string[] lisinStoreL = {"药号","拼音码","进价","批号","制造商","销售商","采购员","经手人" };
        private static string[] lisinStoreR = {"药名","规格","数量","有效期","日期" };

        private static string[] sKind = new string[13];
        private string Two = null;    //与、或、非
        private string Table = null;  //库存or入库   分别对应两个视图进行查询

        private string sCol = null;   //where 对应第一行combobox1
        private string sSel = null;   //where 对应第二行combobox4

        private DB dd = new DB();
      
        public inStoreQuery()
        {
            InitializeComponent();
        }

        private void SetCol()
        {
            while (dataGridView1.RowCount > 1)
            {
                dataGridView1.Rows.RemoveAt(0);
            }
            dataGridView1.Columns.Clear();

            for (int i = 0; i < SelectList.ibr.Count; ++i)
            {
                this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn());
                this.dataGridView1.Columns[i].HeaderText = SelectList.ibr[i];
            }
        }

        private void button2_Click(object sender, EventArgs e)
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

        
        private void button3_Click(object sender, EventArgs e)
        {
            QuerySelect form = new QuerySelect();
            form.afterConfirm = new Action(this.SetCol);
            form.Show();
        }
        
        
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }    
       

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text=="日期" || comboBox1.Text=="有效期" ||comboBox1.Text=="管理级别" ||
                comboBox1.Text=="库存合计")
            {
                this.comboBox2.Enabled = true;
                checkBox1.Enabled = false;
            }
            else
            {
                this.comboBox2.Enabled = false;
            }
            if (comboBox1.Text == "库存合计<最低库存" || comboBox1.Text == "库存合计>最高库存" || comboBox1.Text=="全部")
            {                
                this.comboBox3.Enabled = false;
                this.checkBox1.Enabled = false;
            }
            else
            {
                this.comboBox3.Enabled = true;
            }

            if(comboBox1.Text=="类别")
            {
                comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox3.SelectedIndex = 0;
                this.checkBox1.Enabled = false;
            }
            else
            {                
                comboBox3.DropDownStyle = ComboBoxStyle.Simple;
                comboBox3.Text = "";
            }

            if(comboBox1.Text == "库存合计<最低库存" || comboBox1.Text == "库存合计>最高库存" 
                || comboBox1.Text=="类别" || comboBox1.Text=="全部")
            {
                checkBox1.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
            }

            if(comboBox1.Text=="全部")
            {
                comboBox4.Enabled = false;
            }
            else
            {
                comboBox4.Enabled = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            while(comboBox1.Items.Count>0)
            {
                this.comboBox1.Items.RemoveAt(0);
                this.comboBox4.Items.RemoveAt(0);
            }
            SelectList.ibl.Clear();
            SelectList.ibr.Clear();
            SelectList.ibl.AddRange(liStoreL);
            SelectList.ibr.AddRange(liStoreR);
            this.comboBox1.Items.AddRange(sStore);
            this.comboBox4.Items.AddRange(sStore);
            this.comboBox1.SelectedIndex = 0;
            this.comboBox4.SelectedIndex = 0;
            this.comboBox2.Enabled = false;
            this.comboBox5.Enabled = false;

            Table = "v_药库库存money";
            comboBox6.DropDownStyle = ComboBoxStyle.Simple;
            comboBox3.DropDownStyle = ComboBoxStyle.Simple;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            while (comboBox1.Items.Count > 0)
            {
                this.comboBox1.Items.RemoveAt(0);
                this.comboBox4.Items.RemoveAt(0);
            }
            SelectList.ibl.Clear();
            SelectList.ibr.Clear();
            SelectList.ibl.AddRange(lisinStoreL);
            SelectList.ibr.AddRange(lisinStoreR);
            this.comboBox1.Items.AddRange(sinStore);
            this.comboBox4.Items.AddRange(sinStore);
            this.comboBox1.SelectedIndex = 0;
            this.comboBox4.SelectedIndex = 0;
            this.comboBox2.Enabled = false;
            this.comboBox5.Enabled = false;
            Table = null;
            Table = "v_入库记录";
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.Text == "日期" || comboBox4.Text == "有效期" || comboBox4.Text == "管理级别" ||
                comboBox4.Text == "库存合计")
            {
                this.comboBox5.Enabled = true;
            }
            else
            {
                this.comboBox5.Enabled = false;
            }
            if (comboBox4.Text == "库存合计<最低库存" || comboBox4.Text == "库存合计>最高库存" 
                || comboBox4.Text=="全部")
            {
                this.comboBox6.Enabled = false;
                this.checkBox2.Enabled = false;
            }
            else
            {
                this.comboBox6.Enabled = true;
            }

            if (comboBox4.Text == "类别")
            {
                comboBox6.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox6.SelectedIndex = 0;
                this.checkBox2.Enabled = false;
            }
            else
            {                
                comboBox6.DropDownStyle = ComboBoxStyle.Simple;
                comboBox6.Text = "";
            }

            if (comboBox4.Text == "库存合计<最低库存" || comboBox4.Text == "库存合计>最高库存" 
                || comboBox4.Text == "类别" || comboBox4.Text=="全部")
            {
                checkBox2.Enabled = false;
            }
            else
            {
                checkBox2.Enabled = true;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Two = "and ";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            Two = "or ";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            Two = "and not ";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dd.close();
            dd.connect();

            string t = SelectList.getibr();

            string sql = "select " + t + " from " + Table;
            string ttt = "ttt";

            if(comboBox1.Text =="库存合计<最低库存" || comboBox1.Text=="库存合计>最高库存")
            {
                sCol = comboBox1.Text;
            }
            else if (comboBox1.Text == "类别")
            {
                sCol = " 药号 like '_" + sKind[comboBox3.SelectedIndex] + "%'";
            }
            else
            {
                if(checkBox1.CheckState == CheckState.Checked)
                {
                    sCol = comboBox1.Text + " like '" + comboBox3.Text + "%'";
                }
                else
                {
                    sCol = comboBox1.Text + comboBox2.Text + "'" + comboBox3.Text + "'";
                }
            }

            if (comboBox4.Text == "库存合计<最低库存" || comboBox4.Text == "库存合计>最高库存")
            {
                sSel = comboBox4.Text;
            }
            else if (comboBox4.Text == "类别")
            {
                sSel = " 药号 like '_" + sKind[comboBox6.SelectedIndex] + "%'";
            }
            else
            {
                if (checkBox2.CheckState == CheckState.Checked)
                {
                    sSel = comboBox4.Text + " like '" + comboBox6.Text + "%'";
                }
                else
                {
                    sSel = comboBox4.Text + comboBox5.Text + "'" + comboBox6.Text + "'";
                }
            }
            if(radioButton3.Checked == true)
            {
                Two = " and ";
            }
            else if(radioButton4.Checked == true)
            {
                Two = " or ";
            }
            else
            {
                Two = " and not ";
            }
            if(comboBox1.Text !="全部")
            {
                sql += " where ";
                sql += sCol;
                if(comboBox4.Text!="全部")
                {
                    sql += Two;
                    sql += sSel;
                }
            }

            DataSet dst = dd.tableQuery(sql, ttt);
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = dst;
            dataGridView1.DataMember = ttt;

            //dataGridView1.Rows.RemoveAt(0);
        }    

        private void inStoreQuery_Load(object sender, EventArgs e)
        {
            this.SetCol();
            this.radioButton1.Checked = true;
            this.radioButton3.Checked = true;

            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.SelectedIndex = 0;
            this.comboBox4.SelectedIndex = 0;
            this.comboBox2.Text = "=";
            this.comboBox5.Text = "=";

           
            dd.connect();
            string t = SelectList.getibr();
            string sql = "select * from 药品类别";
            SqlDataReader sd = dd.query(sql);
            int i = 0;
            while (sd.Read())
            {
                comboBox3.Items.Add(sd.GetString(0));
                comboBox6.Items.Add(sd.GetString(0));

                sKind[i] = sd.GetString(1).Substring(0, 2);
                i++;
            }
            dd.close();
        }
    }
    public class SelectList
    {
         static public List<string> ibl = new List<string>();
         static public List<string> ibr = new List<string>();
         
        static public string getibr()
         {
             string res = null;
             for (int i = 0; i < ibr.Count;++i )
             {
                 res += ibr[i];
                 if (i != ibr.Count - 1)
                     res += ", ";
             }
                 return res;
         }
    }
}
