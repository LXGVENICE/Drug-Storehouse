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

    public partial class Insert : Form
    {
        private DB db = new DB();
        public Insert()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) 
        { //确定按钮的事件处理
            if (med_name.Text != "" && med_mark.Text != "" && norms.Text != "" && number.Text != "" && price.Text != "" && batch.Text != "" && maker.Text != "" && seller.Text != "" && valid_time.Text != "" )
            {//如果信息完善，则将数据添加到待入库表中，并调用清除按钮的事件处理清除文本框内容
                dataGridView1.Rows.Add(med_name.Text, med_mark.Text, norms.Text, number.Text, price.Text, batch.Text, maker.Text, seller.Text, valid_time.Text);
                button2_Click(sender, e);
            }
            else 
            {
                MessageBox.Show("请完善药品信息");
            }
        }

        private void med_name_TextChanged(object sender, EventArgs e)
        {//提示框的实现，当文本框发生改变时显示提示框并从数据库中获取提示信息
            dataGridView2.Rows.Clear();
            if (med_name.Text == "")
                dataGridView2.Visible = false;
            else
            { 
                dataGridView2.Visible = true;
                
                string sql = "select 拼音码,药名,药号,规格 from 药品 where 拼音码 like '" + med_name.Text + "%'";
                db.connect();
                SqlDataReader thisReader = db.query(sql);
                while(thisReader.Read())
                {
                    dataGridView2.Rows.Add(thisReader[0], thisReader[1], thisReader[2], thisReader[3]);
                }
            }
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {//提示框的按键处理
            if (e.KeyCode == Keys.Back)  //按下退格键时将焦点转移到文本框中
            {
                med_name.Focus();
                return;
            }
            if(e.KeyCode == Keys.Enter) //回车时将提示框中选中内容添加到文本框
            {
                int i = dataGridView2.CurrentRow.Index;
                String mar = (String)dataGridView2.Rows[i].Cells["Column3"].Value;
                String nor = (String)dataGridView2.Rows[i].Cells["Column4"].Value;
                String name = (String)dataGridView2.Rows[i].Cells["Column2"].Value;
                String _mar = mar.TrimEnd();
                String _nor = nor.TrimEnd();
                String _name = name.TrimEnd();
                med_mark.Text = _mar;
                norms.Text = _nor;
                med_name.Text = _name;
                dataGridView2.Visible = false;
                number.Focus();
                return;
            }
            int keychar = e.KeyValue;
            if(keychar>=65 && keychar <= 90)  //如果按键为字母，则将按下的内容添加到文本框中
            {
                String s = med_name.Text + e.KeyCode;
                med_name.Text = s;
                return;
            }
        }

        private void med_name_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {//在药名文本框Down或Enter时将焦点给提示框
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
            {
                dataGridView2.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//清除按钮事件处理
            med_name.Text = "";
            med_mark.Text = "";
            norms.Text = "";
            maker.Text = "";
            number.Text = "";
            price.Text = "";
            maker.Text = "";
            seller.Text = "";
            batch.Text = "";
        }


        private void number_KeyPress(object sender, KeyPressEventArgs e)
        {//限制数量框只能输入数字和小数点
            if (Char.IsControl(e.KeyChar))
                return;
            if (Char.IsDigit(e.KeyChar) && ((e.KeyChar & 0xFF) == e.KeyChar))
                return;
            if (e.KeyChar == 46)
            {
                if (number.Text.Split('.').Length < 2)
                    return;
            }
            e.Handled = true;
        }

        private void price_KeyPress(object sender, KeyPressEventArgs e)
        {//限制进价框只能输入数字和小数点
            if (Char.IsControl(e.KeyChar))
                return;
            if (Char.IsDigit(e.KeyChar) && ((e.KeyChar & 0xFF) == e.KeyChar))
                return;
            if (e.KeyChar == 46)
            {
                if (price.Text.Split('.').Length < 2)
                    return;
            }
            e.Handled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {//删除待入库表中所有数据
            if (MessageBox.Show("确定要删除所有行吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                dataGridView1.Rows.Clear();
            }
            else
            {
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {//退出按钮
            this.Close();
        }

        private void Insert_FormClosing(object sender, FormClosingEventArgs e)
        {//关闭页面前如果待入库框中还有数据，进行确认
            if(dataGridView1.RowCount > 0)
            {
                if (MessageBox.Show("有未提交的数据，确定退出吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    db.close();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void datato3()
        {//将待入库表中数据填入打印表中，打印用
            int r = dataGridView1.Rows.Count;
            double allprice = 0;
            for(int i = 0; i<r; i++)
            {
                String num = (String)dataGridView1.Rows[i].Cells["入库量"].Value;
                String pri = (String)dataGridView1.Rows[i].Cells["进价"].Value;
                double price = Convert.ToDouble(num) * Convert.ToDouble(pri);
                allprice += price;
                String name = (String)dataGridView1.Rows[i].Cells["药名"].Value;
                String norms = (String)dataGridView1.Rows[i].Cells["规格"].Value;
                dataGridView3.Rows.Add(name,norms,num,pri,price);
            }
            dataGridView3.Rows.Add("", "", "", "金额合计：", allprice);
        }
        void insert_inlib()
        {//在入库记录表中添加数据
            db.close();
            db.connect();
            int r = dataGridView1.Rows.Count;
            for (int i = 0; i < r; i++)
            {
                String sql = "insert into 入库记录(药号,数量,进价,批号,有效期,制造商,销售商,日期,采购员,经手人,是否打单) values ('"+
                    dataGridView1.Rows[i].Cells["药号"].Value.ToString() +"','"+
                    Convert.ToDouble(dataGridView1.Rows[i].Cells["入库量"].Value.ToString()) + "','" +
                    Convert.ToDouble(dataGridView1.Rows[i].Cells["进价"].Value.ToString()) + "','" +
                    dataGridView1.Rows[i].Cells["批号"].Value.ToString() + " ','" +
                    Convert.ToDateTime(dataGridView1.Rows[i].Cells["有效期至"].Value.ToString()) + "','" +
                    dataGridView1.Rows[i].Cells["制造商"].Value.ToString() + "','" +
                    dataGridView1.Rows[i].Cells["销售商"].Value.ToString() + "','" +
                    Convert.ToDateTime(insert_time.Value.ToString()) +"','"+
                    textBox1.Text+"','"+textBox2.Text+"','1')";
                db.change(sql);
            }
        }
        void update_lib()
        {//更新药库库存表
            db.close();
            db.connect();
            int r = dataGridView1.Rows.Count;
            for (int i = 0; i < r; i++)
            {
                string mark = dataGridView1.Rows[i].Cells["药号"].Value.ToString();
                string get = "select * from 药库库存 where 药号= '" + mark + "'";
                SqlDataReader thisReader = db.query(get);
                if(thisReader.Read())
                {
                    double _first = (double)thisReader[1];
                    double _in = (double)thisReader[2];
                    double _out = (double)thisReader[3];
                    double _des = (double)thisReader[4];
                    double _pri = (double)thisReader[5];
                    double num = _first + _in - _out - _des;
                    _in = _in + Convert.ToDouble(dataGridView1.Rows[i].Cells["入库量"].Value.ToString());
                    double in_pri = Convert.ToDouble(dataGridView1.Rows[i].Cells["入库量"].Value.ToString())*Convert.ToDouble(dataGridView1.Rows[i].Cells["进价"].Value.ToString());
                    _pri = (num * _pri + in_pri) / (num + Convert.ToDouble(dataGridView1.Rows[i].Cells["入库量"].Value.ToString()));
                    Math.Round(_pri, 2);
                    string s1 = "update 药库库存 set 本期入库 = '"+_in+"',平均进价 = '"+ _pri +"' where 药号 = '"+mark+"'";
                    db.close();
                    db.connect();
                    db.change(s1);
                }
                else
                {
                    String s2 = "insert into 药库库存(药号,期初库存,本期入库,本期出库,本期损耗,平均进价) values ('" +
                    dataGridView1.Rows[i].Cells["药号"].Value.ToString() + "','0','" +
                    Convert.ToDouble(dataGridView1.Rows[i].Cells["入库量"].Value.ToString()) + "','0','0','" +
                    Convert.ToDouble(dataGridView1.Rows[i].Cells["进价"].Value.ToString()) + "'";
                    db.close();
                    db.connect();
                    db.change(s2);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {//入库按钮事件处理
            if(dataGridView1.RowCount > 0)
            {
                insert_inlib();   //添加入库记录
                update_lib();   // 修改库存

                if (MessageBox.Show("入库完成，是否需要打印？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    /////////////////////////////////////////////////////打印模块
                    datato3();
                    System.Data.DataTable dt = new System.Data.DataTable();
                    DataRow dr;
                    //设置列表头 
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
                dataGridView3.Rows.Clear();   //打印完成后清空表格中数据
                dataGridView1.Rows.Clear();
                //////////////////////////////////////////////////////
            }
            else
            {
                MessageBox.Show("待入库药品为空！");
            }
        }

        private void change_Click(object sender, EventArgs e)
        {//修改本行按钮事件处理
            dataGridView1.Focus();
            int j = dataGridView1.RowCount;
            if (j != 0) //先判断待入库信息是否为空
            {
                int i = dataGridView1.CurrentRow.Index;
                med_name.Text = (String)dataGridView1.Rows[i].Cells["药名"].Value;
                dataGridView2.Visible = false;
                med_mark.Text = (String)dataGridView1.Rows[i].Cells["药号"].Value;
                norms.Text = (String)dataGridView1.Rows[i].Cells["规格"].Value;
                number.Text = (String)dataGridView1.Rows[i].Cells["入库量"].Value;
                price.Text = (String)dataGridView1.Rows[i].Cells["进价"].Value;
                batch.Text = (String)dataGridView1.Rows[i].Cells["批号"].Value;
                maker.Text = (String)dataGridView1.Rows[i].Cells["制造商"].Value;
                seller.Text = (String)dataGridView1.Rows[i].Cells["销售商"].Value;
                delete_Click(sender, e);
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {//删除全部按钮处理
             if (MessageBox.Show("确定要删除所有行吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
             {
                 dataGridView1.Rows.Clear();
             }
        }

        private void Insert_FormClosed(object sender, FormClosedEventArgs e)
        {
            db = null;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
