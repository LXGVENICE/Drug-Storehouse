using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
using System.Drawing.Printing;
using System.Data.SqlClient;
using DLLFullPrint;

namespace xaut_hospital
{
    public partial class Out : Form
    {
        private DB db = new DB();
        public Out()
        {
            InitializeComponent();
        }
        private string durgnum;
        private string drugnum2;
        private double sumJ = 0;//金额统计
        //private double sumS = 0;
        private double Pdan=0;
        private double Sdan = 0;
        private double cursum = 0;//保存当前的药品的金额


        private string keshi = null;
        private string yaohao=null;
        private Double shuliang ;
        private string shijian = null;
        private string keshijingshou = null;
        //private string yaofangjingshou = null;
        private Double shoujia = 0;
        private Double jinjia = 0;
        private string yaokujingshou = null;
        private int yaofangshangzhang = 0;

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

   

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if ( textBox3.Text != "" || textBox4.Text != "" || textBox5.Text != "" || textBox6.Text != "" || textBox7.Text != "")
            {
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox7.Clear();
            }
            dataGridView2.Rows.Clear();
           if (textBox2.Text == "")
                dataGridView2.Visible = false;
            else
            {
                dataGridView2.Visible = true;
            //    DB db = new DB();
                db.close();
                if (db.connect() == false)
                {
                    db.close();
                    return;
                }
                string sql = "select * from v_药库库存 where 拼音码 like '" + textBox2.Text + "%'";
              //  MessageBox.Show(textBox2.Text);
                SqlDataReader thisReader = db.query(sql);
                if (thisReader == null)
                {
                    db.close();
                    return;
                }
                while(thisReader.Read())
                {
                    dataGridView2.Rows.Add(thisReader[2], thisReader[0], thisReader[1], thisReader[16], thisReader[3], thisReader[17], thisReader[6],thisReader[17]); 
                }
                //db.close();
                return;
             }
        /*    dataGridView2.Rows.Clear();
            if (textBox2.Text == "")
                dataGridView2.Visible = false;
            else
            {
                dataGridView2.Visible = true;

                string sql = "select * from v_药库库存 where 拼音码 like '" + textBox2.Text + "%'";
                db.close();
                db.connect();
                SqlDataReader thisReader = db.query(sql);
                while (thisReader.Read())
                {
                    dataGridView2.Rows.Add(thisReader[2], thisReader[0], thisReader[1], thisReader[16], thisReader[3], thisReader[17], thisReader[6], thisReader[17]); 
                }
            }*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //点击确定之后首先判断信息是否完整
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("请将信息填写完整");
                return;
            }

          this.dataGridView1.Rows.Add(textBox7.Text, textBox2.Text, textBox3.Text, textBox6.Text, textBox5.Text, textBox4.Text, comboBox1.Text, textBox1.Text, Pdan, Sdan, Convert.ToString(cursum));
      
          button2_Click(sender, e);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            double numout;
            double num;
            double.TryParse(textBox5.Text, out num);
            double.TryParse(textBox6.Text, out numout);
           
            if(num<numout)
            {
                MessageBox.Show("出库量不足，重新输入");
            }
            cursum = numout * Pdan;
        }

      

        private void button2_Click(object sender, EventArgs e)
        {
          //  comboBox1.SelectedIndex = -1;
          //  textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            comboBox1.Focus();
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox4.ReadOnly = false;
            textBox5.ReadOnly = false;
            textBox7.ReadOnly = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int count = dataGridView1.CurrentRow.Index;
            comboBox1.Text = (string)dataGridView1.Rows[count].Cells["Column8"].Value;
            textBox1.Text = (string)dataGridView1.Rows[count].Cells["Column9"].Value;
            textBox2.Text = (string)dataGridView1.Rows[count].Cells["Column2"].Value;
            textBox3.Text = (string)dataGridView1.Rows[count].Cells["Column3"].Value;
            textBox4.Text = (string)dataGridView1.Rows[count].Cells["Column7"].Value;
            textBox5.Text = (string)dataGridView1.Rows[count].Cells["Column6"].Value;
            textBox6.Text = (string)dataGridView1.Rows[count].Cells["Column5"].Value;
            textBox7.Text = (string)dataGridView1.Rows[count].Cells["Column1"].Value;
            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
            dataGridView2.Visible = false;
        }

        private void textBox2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
            {
                dataGridView2.Focus();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int dadan = 0;
            bool flag = false;
            int count = dataGridView1.Rows.Count;
            double sum = 0;//每一行的总金
            DialogResult MsgBoxResult;
            MsgBoxResult = MessageBox.Show("你确定出库吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if(MsgBoxResult==DialogResult.Yes)  //确认出库;
            {
                string nowtime = dateTimePicker1.Value.ToString();
                MsgBoxResult = MessageBox.Show("系统当前的时间是"+nowtime, "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                if(MsgBoxResult == DialogResult.No)
                {
                    nowtime = Interaction.InputBox("请输入时间格式为" + nowtime);
                }
                MsgBoxResult = MessageBox.Show("是否打印出库单" ,"确认", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                if(MsgBoxResult == DialogResult.Yes)
                {
                    dadan = 1;
                    flag = true;
                }
                if(flag)
                {
                    //需要打印出库单，开始打印出库单。
                    //出库单内容，药名，规格，数量，平均单价，金额合计
                    for(int i=0;i<count-1;++i)
                    {
                        sum = Convert.ToDouble(dataGridView1.Rows[i].Cells["Column12"].Value);
                        sumJ += sum;
                        dataGridView3.Rows.Add(dataGridView1.Rows[i].Cells["Column2"].Value, dataGridView1.Rows[i].Cells["Column3"].Value,dataGridView1.Rows[i].Cells["Column5"].Value, dataGridView1.Rows[i].Cells["Column10"].Value,dataGridView1.Rows[i].Cells["Column12"].Value);
                    }
                    dataGridView3.Rows.Add(null, null, null, null, sumJ);
                    //打印
                    System.Data.DataTable dt = new System.Data.DataTable();
                    DataRow dr;
                    foreach (DataGridViewColumn headerCell in dataGridView3.Columns)
                    {
                        dt.Columns.Add(headerCell.HeaderText);
                    }
                    foreach (DataGridViewRow item in dataGridView3.Rows)
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
         //       DB db = new DB();
                db.close();
                if(db.connect())
                {
                    shijian = nowtime;
                    for(int i=0;i<count-1;++i)
                    {
                   //     MessageBox.Show((string)dataGridView1.Rows[i].Cells["Column5"].Value);
                        string shu = (string)dataGridView1.Rows[i].Cells["Column5"].Value;
                        shuliang = System.Convert.ToDouble(shu);
                        keshi = (String)dataGridView1.Rows[i].Cells["Column8"].Value;
                        yaohao = (String)dataGridView1.Rows[i].Cells["Column1"].Value;
                       
                        jinjia = (Double)dataGridView1.Rows[i].Cells["Column10"].Value;
                        //   yaokujingshou= (String)dataGridView1.Rows[i].Cells["Column4"].Value;
                        keshijingshou = (String)dataGridView1.Rows[i].Cells["Column9"].Value;
                        //yaofangjingshou = null;
                 //       string shou = (string)dataGridView1.Rows[i].Cells["Column11"].Value;
                        shoujia = (Double)dataGridView1.Rows[i].Cells["Column11"].Value;
                        yaofangshangzhang = 1;
                        yaokujingshou = "zhang";
                        string str = "insert into 出库记录(科室,药号,数量,进价,日期,药库经手,科室经手,药房上账,售价,是否打单) values('" + keshi + "','" + yaohao + "'," + shuliang + "," + jinjia + ",'" + shijian + "','" + yaokujingshou + "','" + keshijingshou + "'," + yaofangshangzhang + "," + shoujia + "," + dadan + ")";
                        string sql="update  药库库存 set  本期出库 =本期出库 +" +shuliang +" where 药号='" +yaohao+"'";
                     /*   string sql2 = null;
                        if(keshi.Equals("门诊"))
                        {
                            sql2="update 药房库存 set  门诊领药 ="+shuliang;
                        }
                        if(keshi.Equals("病房"))
                        {
                            sql2 = "update 药房库存 set  药房领药 =" + shuliang;
                        }
                        if(keshi.Equals("门诊"))
                        {
                            sql2 = "update 药房库存 set  科室领药 =" + shuliang;
                        }*/
                       if(db.change(sql))
                       {
                           MessageBox.Show("update success" +sql);
                       }
                   //    MessageBox.Show(str);
                        if(db.change(str)==false)
                        {
                            return;
                        }
                    }
                }
                dataGridView3.Rows.Clear();
                dataGridView1.Rows.Clear();
                sumJ = 0;
                db.close();
                return;
            }
        }


        private void dataGridView2_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                textBox2.Focus();
                return;
            }
            if (e.KeyCode == Keys.Enter)  //选中了药名。
            {
                int i = dataGridView2.CurrentRow.Index;
                //    cursum = Convert.ToDouble(textBox4.Text) * (Double)dataGridView2.Rows[i].Cells["dataGridViewTextBoxColumn6"].Value;  //计算当前药品的金额合计
                String name = (String)dataGridView2.Rows[i].Cells["dataGridViewTextBoxColumn3"].Value;  //药名
                Double count = (Double)dataGridView2.Rows[i].Cells["dataGridViewTextBoxColumn4"].Value;  //库存合计
             //   String count = (String)dataGridView2.Rows[i].Cells["dataGridViewTextBoxColumn4"].Value;  //库存合计
                String size = (String)dataGridView2.Rows[i].Cells["dataGridViewTextBoxColumn5"].Value;  //规格
                String num = (String)dataGridView2.Rows[i].Cells["dataGridViewTextBoxColumn2"].Value; //药号
                drugnum2 = num;
                String _name = name.TrimEnd();
            //    String _count = count.TrimEnd();
                String _size = size.TrimEnd();
                String _count = count.ToString();

                Pdan = (double)dataGridView2.Rows[i].Cells["Column14"].Value;
                Sdan = (double)dataGridView2.Rows[i].Cells["dataGridViewTextBoxColumn6"].Value;
          //      cursum = Pdan * count;
               textBox2.Text = _name;  //药名
                textBox5.Text = _count;  //库存合计
                textBox3.Text = _size;  //规格
                textBox7.Text = num;  //药号
            //    textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                 textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox7.ReadOnly = true;

                durgnum = num;  //得到药号

            //    DB db = new DB();
                db.close();

                if (db.connect() == false)
                {
                    db.close();
                    return;
                }
                string strLB = durgnum.Substring(1, 2);
                string sql = "select 类别 from 药品类别 where 编号 = '" + strLB + "'";
                SqlDataReader reader = db.query(sql);
                while (reader.Read())
                {
                    textBox4.Text = (string)reader[0];
                    textBox4.ReadOnly = true;
                }
                if (textBox4.Text == "")
                {
                    MessageBox.Show("查询不到此类别，请检查后输入");
                }
                dataGridView2.Visible = false;
                textBox1.Focus();
                db.close();
                return;

            }
            if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right)
            {
                String s = textBox2.Text + e.KeyCode;
                textBox2.Text = s;
                return;
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(((e.KeyChar >= '0') && (e.KeyChar <= '9')) || e.KeyChar <= 31))
            {
                if (e.KeyChar == '.')
                {
                    if (((TextBox)sender).Text.Trim().IndexOf('.') > -1)
                        e.Handled = true;
                }
                else
                    e.Handled = true;
            }
            else
            {
                if (e.KeyChar <= 31)
                {
                    e.Handled = false;
                }
                else if (((TextBox)sender).Text.Trim().IndexOf('.') > -1)
                {
                    if (((TextBox)sender).Text.Trim().Substring(((TextBox)sender).Text.Trim().IndexOf('.') + 1).Length >= 4)
                        e.Handled = true;
                }
            }
        }

    }
}
