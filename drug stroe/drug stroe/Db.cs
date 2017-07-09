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
        private static StringBuilder _passwb;

        //数据库
        private static SqlConnection _conn;

        DB()
        {
            string section = "Database";
            _ip = Ini.ReadIniData(section, "ip", "_passwb.ini");
            _port = Ini.ReadIniData(section, "port", "_passwb.ini");
            _basename = Ini.ReadIniData(section, "basename", "_passwb.ini");
            _username = Ini.ReadIniData(section, "username", "_passwb.ini");
            _passwb = Ini.ReadIniData(section, "passwb", "_passwb.ini");
        }

        ~DB()
        {
            _conn.Close();
        }

        public static bool connect()
        {
            //SQL server .net 4.5
            string con = "Data Source=" + _ip.ToString() + "," + _port.ToString() + ";"
                                  + "Network Library=DBMSSOCN;"
                                  + "Initial Catalog=" + _basename.ToString() + ";"
                                  + "User ID=" + _username.ToString() + ";"
                                  + "Password=" + _passwb.ToString() + ";";
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
        private static void ReadSingleRow(IDataRecord record, ref List<List<string>> vec, int n)
        {
            List<string> tmp = new List<string>();
            for (int i = 0; i < n; ++i)
            {
                tmp.Add(record[i].ToString());
            }
            vec.Add(tmp);
        }

        public static void query(string str, List<string> parse, int n)
        {
            List<List<string>> vec = new List<List<string>>();
            SqlCommand cmd = new SqlCommand(str, _conn);
            cmd.ExecuteNonQuery();
            SqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                if(parse.Count() == 0)
                {
                    ReadSingleRow((IDataRecord)reader,ref vec,n);
                }
                else
                {
                    List<string> tmp = new List<string>();
                    foreach (string s in parse)
                    {
                        tmp.Add(reader[s].ToString());
                    }
                    vec.Add(tmp);
                }
            }
        }
    }
}
