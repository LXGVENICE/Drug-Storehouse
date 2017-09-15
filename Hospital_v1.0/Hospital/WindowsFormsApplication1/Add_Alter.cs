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

namespace xaut_hospital
{
    public partial class Add_Alter : Form
    {
        bool flag = false; //判断是修改还是增加
        DB db = new DB();
        string init = ""; //存储修改信息时候的药号
        public Add_Alter()
        {
            InitializeComponent();
        }
        public Add_Alter(string res)
        {
            InitializeComponent();
            flag = true;
            textBox1.Text = res;
            string sql = "select  药名, 拼音码, 规格, 系数, 小规格, 病房系数, 病房规格, 售价, 管理级别, 最低库存, 最高库存 from 药品 where 药号='" + res + "'";
            init = res;
            db.connect();
            SqlDataReader sd = db.query(sql);
            while(sd.Read())
            {
                textBox2.Text = sd.GetString(0);
                textBox3.Text = sd.GetString(1);
                textBox4.Text = sd.GetString(2);
                textBox5.Text = sd.GetDouble(3).ToString();
                textBox6.Text = sd.GetString(4);
                textBox7.Text = sd.GetDouble(5).ToString();
                textBox8.Text = sd.GetString(6);
                textBox9.Text = sd.GetDouble(7).ToString();
                textBox10.Text = sd.GetValue(8).ToString();
                textBox11.Text = sd.GetValue(9).ToString();
                textBox12.Text = sd.GetValue(10).ToString();
            }
            db.close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(flag)
            {
                Double xishu = System.Convert.ToDouble(textBox5.Text);
                Double bingfangxishu = System.Convert.ToDouble(textBox7.Text);
                Double shoujia = System.Convert.ToDouble(textBox9.Text);
                string sql = "update 药品 set  药名='" + textBox2.Text + "', 拼音码='" +
                    textBox3.Text + "', 规格='" + textBox4.Text + "', 系数=" + xishu + ", 小规格='" + textBox6.Text +
                    "', 病房系数=" + bingfangxishu + ", 病房规格='" + textBox8.Text + "', 售价=" + shoujia;
                if(textBox10.Text!="")
                {
                    string guanlijibie = ", 管理级别='"+textBox10.Text+"' ";
                    sql += guanlijibie;
                }
                if(textBox11.Text!="")
                {
                    Double zuidikucun = System.Convert.ToDouble(textBox11.Text);
                    string zuidi = ", 最低库存=";
                    sql += zuidi;
                    sql += zuidikucun;
                }
                if(textBox12.Text!="")
                {
                    Double zuigaokucun = System.Convert.ToDouble(textBox12.Text);
                    string zuigao = ", 最高库存=";
                    sql += zuigao;
                    sql += zuigaokucun;
                }
                string whe = " where 药号='" + init + "'";
                sql += whe;

                //textBox12.Text = sql;
                db.close();
                db.connect();

                if (db.change(sql))
                {
                    MessageBox.Show("修改成功！");
                }
            }
            else
            {
                if(textBox1.Text=="" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text=="" || 
                    textBox5.Text == "" || textBox6.Text == "" || textBox7.Text=="" || textBox8.Text == "" || textBox9.Text == "" ||textBox10.Text=="" || textBox11.Text == "" || textBox12.Text == "")
                {
                    MessageBox.Show("请将信息填写完整");
                }
                else
                {
                    string sql = "insert into 药品(药号, 药名, 拼音码, 规格, 系数, 小规格, 病房系数, 病房规格, 售价, 管理级别, 最低库存, 最高库存) ";
                    Double xishu = System.Convert.ToDouble(textBox5.Text);
                    Double bingfangxishu = System.Convert.ToDouble(textBox7.Text);
                    Double shoujia = System.Convert.ToDouble(textBox9.Text);
                    Double zuidikucun = System.Convert.ToDouble(textBox11.Text);
                    Double zuigaokucun = System.Convert.ToDouble(textBox12.Text);
                    string val = " values('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "', '" + textBox4.Text +
                        "'," + xishu + ", '" + textBox6.Text + "', " + bingfangxishu + ", '" + textBox8.Text + "', " + shoujia +
                        ", '" + textBox10.Text + "', " + zuidikucun + ", " + zuigaokucun+");";

                    sql+=val;
            
                    db.close();
                    db.connect();
                   // textBox10.Text = sql;
                    if(db.change(sql))
                    {
                        MessageBox.Show("插入成功！");
                    }
                }
            }
           // this.Close();
        }

        private void Add_Alter_Load(object sender, EventArgs e)
        {
            if (flag)
                textBox1.Enabled = false;

        }
    }
}
