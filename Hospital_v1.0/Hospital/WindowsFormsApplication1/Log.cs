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
    
    public partial class Log : Form
    {
        public Log()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, EventArgs e)
        {
            string u = user.Text;
            string p = password.Text;
            if (u == string.Empty || p == string.Empty)
            {
                MessageBox.Show("账号或密码不能为空！","提示");
            }
            else
            {
                DB dd = new DB();
                dd.connect();
                string sql = "select 用户名,口令 from 用户 where 用户名='" + u + "'and 口令='" + p + "'";
                SqlDataReader thisReader = dd.query(sql);
                if(thisReader.Read())
                {
                    success frm = new success();
                    frm.Show();
                    this.Dispose(false);
                }
                else
                {
                    MessageBox.Show("账号或密码错误！");
                }
            }
        }
    }
}