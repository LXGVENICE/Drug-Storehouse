using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace xaut_hospital
{
    class DB
    {
        //配置文件路径 ip 端口 数据库名 数据库用户名 用户名密码
        private static string _path = ".//passwd.ini";
        private static string section = "Database";
        private static string _ip = Ini.ReadIniData(section, "ip", _path).ToString();
        private static string _port = Ini.ReadIniData(section, "port", _path).ToString();
        private static string _basename = Ini.ReadIniData(section, "basename", _path).ToString();
        private static string _username = Ini.ReadIniData(section, "username", _path).ToString();
        private static string _passwd = Ini.ReadIniData(section, "passwd", _path).ToString();

        //数据库成员变量
        private static SqlConnection _conn;

        ~DB()
        {
            _conn.Close();
        }

        //主动关闭
        public void close()
        {
            _conn.Close();
        }

        //连接数据库
        public bool connect()
        {
            //SQL server .net 4.5
            string con = "Data Source=" + _ip.ToString() + ";"
                                  + "Network Library=DBMSSOCN;"
                                  + "Initial Catalog=" + _basename.ToString() + ";"
                                  + "User ID=" + _username.ToString() + ";"
                                  + "Password=" + _passwd.ToString() + ";";
            try
            {
                _conn = new SqlConnection(con);
                _conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库连接失败 "+ex.Message);
                return false;
            }
        }

        //普通查询
        public SqlDataReader query(string str)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(str, _conn);
                SqlDataReader SDR = cmd.ExecuteReader();
                return SDR;
            }
            catch (Exception e)
            {
                MessageBox.Show("查询失败 "+e.Message);
                return null;
            }
        }

        //便于返回到表格数据的查询
        public DataSet tableQuery(string str, string ta)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(str, _conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds, ta);
                return ds;
            }
            catch (Exception e)
            {
                MessageBox.Show("查询失败 "+e.Message);
                return null;
            }
        }

        //对表增删改
        public bool change(string str)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(str, _conn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("操作失败 "+e.Message);
                return false;
            }
        }
    }
}
