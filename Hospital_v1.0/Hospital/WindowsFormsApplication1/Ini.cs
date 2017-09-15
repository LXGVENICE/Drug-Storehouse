using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace xaut_hospital
{
    class Ini
    {
        //引用API
        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key,
            string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);

        //读ini配置文件
        public static StringBuilder ReadIniData(string Section, string Key, string iniFilePath)
        {
            StringBuilder temp;
            if (File.Exists(iniFilePath))
            {
                temp = new StringBuilder(256);
                GetPrivateProfileString(Section, Key, null, temp, 1024, iniFilePath);
            }
            else
            {
                MessageBox.Show("配置文件不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                temp = null;
                System.Environment.Exit(0);
            }
            return temp;
        }
    }

}

