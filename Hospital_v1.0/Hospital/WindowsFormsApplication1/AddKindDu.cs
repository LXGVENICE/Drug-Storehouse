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
    public partial class AddKindDu : Form
    {
        public AddKindDu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s1 = textBox1.Text;
            string s2 = textBox2.Text;
            if(s2.Length>3)
            {
                MessageBox.Show("编码长度最大为2位！");
                return;
            }
            DB dd = new DB();
            dd.connect();
            string sql = "insert into 药品类别 values('" + s1+"','"+s2+"')";
           
            dd.change(sql);
            MessageBox.Show("添加成功！");
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
