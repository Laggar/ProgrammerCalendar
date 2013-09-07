using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
namespace Cut
{

    public class Logger
    {

        /// 日志文件大小
        private int fileSize;

        /// 日志文件的路径

        private string fileLogPath;


        /// 日志文件的名称
        private string logFileName;

        /// 构造函数,初始化日志文件大小[2M]

        private static Logger logger;

        private Logger()
        {
            //初始化大于2M日志文件将自动删除;

            this.fileSize = 2048 * 1024;//2M
            //默认路径
            this.fileLogPath = Directory.GetCurrentDirectory() + "\\config\\";
            this.logFileName = "log.txt";
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Logger getInstance()
        {
            if (logger == null)
            {
                logger = new Logger();
            }
            return logger;
        }
        /// <summary>
        /// 获取或设置定义日志文件大小
        /// </summary>
        public int FileSize
        {
            set
            {
                fileSize = value;
            }
            get
            {
                return fileSize;
            }
        }


        /// <summary>
        /// 获取或设置日志文件的路径
        /// </summary>
        public string FileLogPath
        {
            set
            {
                this.fileLogPath = value;
            }
            get
            {
                return this.fileLogPath;
            }
        }


        /// <summary>
        /// 获取或设置日志文件的名称
        /// </summary>
        public string LogFileName
        {
            set
            {
                this.logFileName = value;
            }
            get
            {
                return this.logFileName;
            }

        }

        public string FullLogFileName
        {
           
            get
            {
                return this.fileLogPath + LogFileName;
            }

        }


        /// <summary>
        /// 向指定目录中的指定的文件中追加日志文件
        /// </summary>
        /// <param name="Message">要写入的内容</param>
        public void WriteLog(string Message)
        {
            this.WriteLog(this.logFileName, Message);
        }


        /// <summary>
        /// 向指定目录中的文件中追加日志文件,日志文件的名称将由传递的参数决定.
        /// </summary>
        /// <param name="LogFileName">日志文件的名称,如:mylog.txt ,如果没有自动创建,如果存在将追加写入日志</param>
        /// <param name="Message">要写入的内容</param>
        public void WriteLog(string LogFileName, string Message)
        {
            lock (this)
            {
                //DirectoryInfo path=new DirectoryInfo(LogFileName);
                //如果日志文件目录不存在,则创建
                if (!Directory.Exists(this.fileLogPath))
                {
                    Directory.CreateDirectory(this.fileLogPath);
                }

                FileInfo finfo = new FileInfo(this.fileLogPath + LogFileName);
                if (finfo.Exists && finfo.Length > fileSize)
                {
                    finfo.Delete();
                }
                try
                {
                    FileStream fs = new FileStream(this.fileLogPath + LogFileName, FileMode.Append);
                    StreamWriter strwriter = new StreamWriter(fs);

                    try
                    {

                        DateTime d = DateTime.Now;
                        strwriter.WriteLine("[" + d.ToString() + "]:" + Message);
                       
                        strwriter.Flush();

                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine("日志文件写入失败信息:" + ee.ToString());
                    }
                    finally
                    {
                        strwriter.Close();
                        strwriter = null;
                        fs.Close();
                        fs = null;
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show("写入日志错误,详细信息如下:" + ee.Message);
                }

            }
        }
    }
}