using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace drug_stroe
{
    class DB
    {
        //配置文件
        private static StringBuilder _ip;
        private static StringBuilder _port;
        private static StringBuilder _basename;
        private static StringBuilder _username;
        private static StringBuilder _passwd;

        //数据库
        private static SqlConnection _conn;

        public DB(string path)
        {
            string section = "Database";
            _ip = Ini.ReadIniData(section, "ip", path);
            _port = Ini.ReadIniData(section, "port", path);
            _basename = Ini.ReadIniData(section, "basename", path);
            _username = Ini.ReadIniData(section, "username", path);
            _passwd = Ini.ReadIniData(section, "passwd", path);
        }

        ~DB()
        {
            _conn.Close();
        }

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
                return false;
            }
        }

        private void InitList(ref List<List<string>> ans, int n)
        {
            for (int i = 0; i < n; ++i)
            {
                List<string> tmp = new List<string>();
                ans.Add(tmp);
            }
        }

        private void ReadSingleRow(IDataRecord record, ref List<List<string>> ans)
        {
            int n = record.FieldCount;
            for (int i = 0; i < n; ++i)
            {
                ans[i].Add(record[i].ToString());
            }
        }

/*
        参数功能废弃
        private void ReadSingleRow(IDataRecord record, ref List<string> parse, ref List<List<string>> ans)
        {
            int n = parse.Count();
            for (int i = 0; i < n; ++i)
            {
                ans[i].Add(record[parse[i]].ToString());
            }
        }
 */

        public void query(string str, ref List<List<string>> ans)
        {
            SqlCommand cmd = new SqlCommand(str, _conn);
            cmd.ExecuteNonQuery();
            SqlDataReader reader = cmd.ExecuteReader();

            int n = ((IDataRecord)reader).FieldCount;
            InitList(ref ans, n);

            while (reader.Read())
            {
                    ReadSingleRow((IDataRecord)reader, ref ans);
            }
        }
    }
}
