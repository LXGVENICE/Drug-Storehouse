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
    public partial class TakeMed : Form
    {
        private DB db = new DB();
        public TakeMed()
        {
            InitializeComponent();
        }

        private void TakeMed_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "科室";
            db.connect();
            string sql = "select 科室名 from 科室";
            SqlDataReader thisReader = db.query(sql);
            while(thisReader.Read())
            {
                comboBox2.Items.Add(thisReader[0]);
            }
            db.close();
        }
        string getTJ()
        {
            if(comboBox1.Text.Equals("全部"))
            {
                return "1=1";
            }
            else if(comboBox1.Text.Equals("类别"))
            {
                string sql = "select 编号 from 药品类别 where 类别 like '%" + comboBox2.Text + "%'";
                db.close();
                db.connect();
                SqlDataReader tmp = db.query(sql);
                tmp.Read();
                string mark = tmp[0].ToString().Substring(0, 2);
                db.close();
                return "药号 like '%" + mark +"%'";
            }
            else if (comboBox1.Text.Equals("拼音码"))
            {
                if(checkBox1.Checked)
                {
                    return "拼音码 like '%" + comboBox2.Text+"%'";
                }
                else
                {
                    return "拼音码='" + comboBox2.Text+"'";
                }
            }
            else if (comboBox1.Text.Equals("药名"))
            {
                if (checkBox1.Checked)
                {
                    return "药名 like '%" + comboBox2.Text + "%'";
                }
                else
                {
                    return "药名='" + comboBox2.Text + "'";
                }
            }
            else if (comboBox1.Text.Equals("科室"))
            {
                return "科室='" + comboBox2.Text + "'";
            }
            return "1=1";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            string box1 = comboBox1.Text;
            string box2 = comboBox2.Text;
            string ltime = dateTimePicker1.Value.Year.ToString() + dateTimePicker1.Value.Month.ToString() + dateTimePicker1.Value.Day.ToString();
            string rtime = dateTimePicker2.Value.Year.ToString() + dateTimePicker2.Value.Month.ToString() + dateTimePicker2.Value.Day.ToString();
            string sql = "SELECT 药号, 药名, 规格, 科室, SUM(数量) AS 数量, 进价,SUM(数量 * 进价) AS 资金, LEFT(CONVERT(char, 日期, 112), 6) AS 月份 "
                        + " from dbo.v_出库记录 where " + getTJ() + " and 日期>='" + dateTimePicker1.Value + "' and 日期<='"
                        + dateTimePicker2.Value + "' GROUP BY 药号, 药名,进价,规格, 科室, LEFT(CONVERT(char, 日期, 112), 6)";
            db.close();
            db.connect();
            SqlDataReader thisReader = db.query(sql);
            while (thisReader.Read())
            {
                dataGridView1.Rows.Add(thisReader[0], thisReader[1], thisReader[2], thisReader[3], thisReader[4], thisReader[5], thisReader[6], thisReader[7]);
            }
            db.close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string box1 = comboBox1.Text;
            comboBox2.Items.Clear();
            string sql;
            if(box1.Equals("科室"))
            {
                comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
                sql = "select 科室名 from 科室";
            }
            else if(box1.Equals("类别"))
            {
                comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
                sql = "select 类别 from 药品类别";
            }
            else if(box1.Equals("拼音码"))
            {
                comboBox2.DropDownStyle = ComboBoxStyle.Simple;
                return;
            }
            else if(box1.Equals("药名"))
            {
                comboBox2.DropDownStyle = ComboBoxStyle.Simple;
                return;
            }
            else 
            {
                comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
                return; 
            }
            db.connect();
            SqlDataReader thisReader = db.query(sql);
            while (thisReader.Read())
            {
                comboBox2.Items.Add(thisReader[0]);
            }
            db.close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(dataGridView1.RowCount <= 0)
            {
                MessageBox.Show("没有要打印的内容");
                return;
            }
             /////////////////////////////////////////////////////打印模块
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
            //////////////////////////////////////////////////////
        }
    }
}
