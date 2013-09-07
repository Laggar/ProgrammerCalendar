using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Runtime.CompilerServices;

namespace Cut
{
    public class ConfigFile
    {

       private string fileName = Directory.GetCurrentDirectory() + "\\config\\db.ini";
       private string section = "DataBaseConfig";
       public string ip;
       public string port;
       public string username;
       public string password;
       public string dbName;
       private static ConfigFile configFile;

       [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, 
                                                             string val, string filePath);
        /*参数说明：section：INI文件中的段落；key：INI文件中的关键字；
          val：INI文件中关键字的数值；filePath：INI文件的完整的路径和名称。*/
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, 
                                                          string def, StringBuilder retVal, 
                                                          int size, string filePath);
        /*参数说明：section：INI文件中的段落名称；key：INI文件中的关键字；
          def：无法读取时候时候的缺省数值；retVal：读取数值；size：数值的大小；
          filePath：INI文件的完整路径和名称。*/

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ConfigFile getInstance()
        {
            if (configFile == null)
            {
                configFile = new ConfigFile();
            }
            return configFile;
        }

        private ConfigFile() {

            ReadConfig();

        }

        private void ReadConfig() {

            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(section, "dbUserName", "root", temp, 255, fileName);
            this.username = temp.ToString().Trim();


            GetPrivateProfileString(section, "dbPassword", "", temp, 255, fileName);
            this.password = Decode(temp.ToString().Trim());

            GetPrivateProfileString(section, "dbIp", "127.0.0.1", temp, 255, fileName);
            this.ip = temp.ToString().Trim();

            GetPrivateProfileString(section, "dbPort", "3306", temp, 255, fileName);
            this.port = temp.ToString().Trim();

            GetPrivateProfileString(section, "dbName", "", temp, 255, fileName);
            this.dbName = temp.ToString().Trim();

        }

        public void WriteConfig(){

            WritePrivateProfileString(section,"dbUserName",username.Trim(), fileName);
            WritePrivateProfileString(section,"dbPassword",Encode(password.Trim()), fileName);
            WritePrivateProfileString(section,"dbIp",ip.Trim(), fileName);
            WritePrivateProfileString(section,"dbPort",port.Trim(), fileName);
            WritePrivateProfileString(section, "dbName", dbName.Trim(), fileName);
     }


        public string Encode(string str)
        {
            string htext = "";

            for (int i = 0; i < str.Length; i++)
            {
                htext = htext + (char)(str[i] + 10 - 1 * 2);
            }
            return htext;
        }

        public string Decode(string str)
        {
            string dtext = "";

            for (int i = 0; i < str.Length; i++)
            {
                dtext = dtext + (char)(str[i] - 10 + 1 * 2);
            }
            return dtext;
        }

    }


    
}
