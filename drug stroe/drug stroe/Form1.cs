using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace drug_stroe
{
    public partial class Form1 : Form
    {
        private DB dd;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(dd.connect())
            {
                textBox1.Text = "成功";
            }
            else
            {
                textBox1.Text = "失败";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string que = "select 药号,拼音码 from 药品 where 药号='yck0440'";
            List<List<string>> ans = new List<List<string>>();
            dd.query(que, ref ans);
            int n = ans[0].Count();
            textBox1.Text = ans[0][0];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dd = new DB("E:\\passwd.ini");
        }
    }
}
