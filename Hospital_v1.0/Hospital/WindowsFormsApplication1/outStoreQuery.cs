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
    public partial class outStoreQuery : Form
    {
        /*
         * 以下字符串存储的是“查询项目”的下拉列表信息以及查询的时候列信息（列选择）
         * outStore为单选按钮为出库时“查询项目”的下拉信息，lostS同理
         * lioutStroeR为SelectList类的初始化信息，为列选择功能服务，从而在查询时确定查询的项目
         * oelectList主要有两个List构成，分别是左右两个列选择框。
         * lilostS同理
         * sKind[]是在“查询条件”为种类的时候，存储当前种类的药号信息，以便查询
         * Two为判断两个条件是“与或非”
         * Table存储查询的表（单选框“选项”的信息）
         * sCol和sSel分别存储两个条件语句的组合
         */
        private static string[] outStore = {"全部","科室","拼音码","类别","日期",
                              "科室经手","药库经手"};
        private static string[] lostS = { "全部", "拼音码", "类别", "药名", "日期","经手人", "批准人","操作者"};
        private static string[] lioutStoreL = {"药号","小规格","领回额","进价","药库经手","科室经手",
                                               "拼音码"};
        private static string[] lioutStoreR = { "药名", "规格", "科室", "数量", "日期"};
        private static string[] lilostSL = { "药号", "拼音码", "小规格", "价格", "报损原因", "经手人", "批准人", "操作者" };
        private static string[] lilostSR = { "药名", "规格", "数量", "资金合计", "日期" };

        private static string[] sKind = new string[13];
        private string Two = null;    //与、或、非
        private string Table = null;  //出库or损耗   分别对应两个视图进行查询

        private string sCol = null;   //where 对应第一行combobox1语句组合
        private string sSel = null;   //where 对应第二行combobox4语句组合
        private DB dd = new DB();

        public outStoreQuery()
        {
            InitializeComponent();
        }

        //根据SelectList信息设置dataGridView的列信息，也是传入select窗体的委托事件
        private void SetCol()
        {
            while (dataGridView1.RowCount > 1)
            {
                dataGridView1.Rows.RemoveAt(0);
            }
            dataGridView1.Columns.Clear();

            for (int i = 0; i < oSelectList.ibr.Count; ++i)
            {
                this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn());
                this.dataGridView1.Columns[i].HeaderText = oSelectList.ibr[i];
            } 
        }

        //打印函数
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

        //查询函数
        private void button1_Click(object sender, EventArgs e)
        {           
            dd.close();
            dd.connect();

            string t = oSelectList.getibr();

            string sql = "select " + t + " from " + Table;
            string ttt = "ttt";

            //第一条语句的判断组合
            if (comboBox1.Text == "类别")
            {
                sCol = " 药号 like '_" + sKind[comboBox3.SelectedIndex] + "%'";
            }
            else if(comboBox1.Text == "日期")
            {
                sCol = " 日期 " + comboBox2.Text + comboBox3.Text;
            }
            else
            {
                if (checkBox1.CheckState == CheckState.Checked)
                {
                    sCol = comboBox1.Text + " like '" + comboBox3.Text + "%'";
                } 
                else
                {
                    sCol = comboBox1.Text + comboBox2.Text + "'" + comboBox3.Text + "'";
                }
            }
            //第二条语句的判断组合
            if (comboBox4.Text == "类别")
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
            //判断是与或非的哪一种
            if (radioButton3.Checked == true)
            {
                Two = " and ";
            }
            else if (radioButton4.Checked == true)
            {
                Two = " or ";
            }
            else
            {
                Two = " and not ";
            }
            if (comboBox1.Text != "全部")
            {
                sql += " where ";
                sql += sCol;
                if (comboBox4.Text != "全部")
                {
                    sql += Two;
                    sql += sSel;
                }
            }

            //查询并填充表格
            DataSet dst = dd.tableQuery(sql, ttt);
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = dst;
            dataGridView1.DataMember = ttt;

        }

        //选择出库单选按钮的时候执行的操作
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            while (comboBox1.Items.Count > 0)
            {
                this.comboBox1.Items.RemoveAt(0);
                this.comboBox4.Items.RemoveAt(0);
            }
            oSelectList.ibl.Clear();
            oSelectList.ibr.Clear();
            oSelectList.ibl.AddRange(lioutStoreL);
            oSelectList.ibr.AddRange(lioutStoreR);
            this.comboBox1.Items.AddRange(outStore);
            this.comboBox4.Items.AddRange(outStore);
            this.comboBox1.SelectedIndex = 0;
            this.comboBox4.SelectedIndex = 0;
            this.comboBox2.Enabled = false;
            this.comboBox5.Enabled = false;

            Table = "v_出库记录";
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
            oSelectList.ibl.Clear();
            oSelectList.ibr.Clear();
            oSelectList.ibl.AddRange(lilostSL);
            oSelectList.ibr.AddRange(lilostSR);
            this.comboBox1.Items.AddRange(lostS);
            this.comboBox4.Items.AddRange(lostS);
            this.comboBox1.SelectedIndex = 0;
            this.comboBox4.SelectedIndex = 0;
            this.comboBox2.Enabled = false;
            this.comboBox5.Enabled = false;
            Table = null;
            Table = "v_药库损耗money";
        }

        //选择查询条件的时候，根据不同的查询条件，对后续的选择框执行不同的操作
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "日期" )
            {
                this.comboBox2.Enabled = true;
            }
            else
            {
                this.comboBox2.Enabled = false;
            }
            if(comboBox1.Text=="全部")
            {
                this.comboBox3.Enabled = false;
            }
            else
            {
                this.comboBox3.Enabled = true;
            }
            if (comboBox1.Text == "类别")
            {
                comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox3.SelectedIndex = 0;
            }
            else
            {
                comboBox3.DropDownStyle = ComboBoxStyle.Simple;
                comboBox3.Text = "";
            }

            if (comboBox1.Text == "类别" || comboBox1.Text == "全部")
            {
                checkBox1.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
            }

            if (comboBox1.Text == "全部")
            {
                comboBox4.Enabled = false;
            }
            else
            {
                comboBox4.Enabled = true;
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.Text == "日期")
            {
                this.comboBox5.Enabled = true;
            }
            else
            {
                this.comboBox5.Enabled = false;
            }
            if (comboBox4.Text == "全部")
            {
                this.comboBox6.Enabled = false;
            }
            else
            {
                this.comboBox6.Enabled = true;
            }
            if (comboBox4.Text == "类别")
            {
                comboBox6.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox6.SelectedIndex = 0;
            }
            else
            {
                comboBox6.DropDownStyle = ComboBoxStyle.Simple;
                comboBox6.Text = "";
            }

            if (comboBox4.Text == "类别" || comboBox4.Text == "全部")
            {
                checkBox2.Enabled = false;
            }
            else
            {
                checkBox2.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            outStoreQuerySelect form = new outStoreQuerySelect();
            form.afterConfirm = new Action(this.SetCol);
            form.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /*
         * 窗体加载时候执行的动作
         * 将单选按钮设置为“出库”，“与”
         * 设置“查询项目”“查询条件”“查询值”三种六个下拉框的属性
         * 初始化sKind[]
         */
        private void outStoreQuery_Load(object sender, EventArgs e)
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

    //SelectList类，主要用于列选择功能
    public class oSelectList
    {
        static public List<string> ibl = new List<string>();
        static public List<string> ibr = new List<string>();

        static public string getibr()
        {
            string res = null;
            for (int i = 0; i < ibr.Count; ++i)
            {
                res += ibr[i];
                if (i != ibr.Count - 1)
                    res += ", ";
            }
            return res;
        }
    }
}
