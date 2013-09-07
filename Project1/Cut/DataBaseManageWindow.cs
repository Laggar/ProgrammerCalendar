using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace Cut
{
    public partial class DataBaseManageWindow : Form
    {
        private string initSQL = Directory.GetCurrentDirectory() + "\\config\\init.sql";
     
        private BoardCutting boardCutting;
        private ConfigFile configFile;
        private ConnectionPool connectionPool;


        public DataBaseManageWindow(BoardCutting boardCutting)
        {   

            InitializeComponent();
            AddEventListeners();
            configFile = boardCutting.configFile;
            this.boardCutting = boardCutting;
            FillConfig();
        }

        private void DataBaseManageWindow_Load(object sender, EventArgs e) {

        }



        private void AddEventListeners(){
            saveConfig.Click +=new EventHandler(SaveConfigClick);
            rebuildData.Click += new EventHandler(RebuildClick);
        }

        public void SaveConfigClick(object sender, EventArgs e)
        {
            MySqlConnection conn = null;
          
            try
            {
                configFile.ip = ip.Text.Trim();
                configFile.port = port.Text.Trim();
                configFile.username = username.Text.Trim();
                configFile.password = password.Text.Trim();
                configFile.dbName = dbName.Text.Trim();
                configFile.WriteConfig();

                connectionPool = ConnectionPool.refresh(configFile);
                conn=connectionPool.getConnection();
                boardCutting.connectionPool = connectionPool;

                logRichTextBox.AppendText("[保存配置]操作成功.\n");

            }
            catch (Exception ex)
            {
                logRichTextBox.AppendText("[保存配置]操作失败,请检查数据库信息是否正确.\n");
                Logger.getInstance().WriteLog("[DataBaseManageWindow.SaveConfigClick()]" + ex.Message);
            }
            finally {
                if (conn != null)
                {
                    connectionPool.releaseConnection(conn);
                }
            }
        }

        public void RebuildClick(object sender, EventArgs e){

            DialogResult result = MessageBox.Show("重建数据将删除表数据，请确认", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);


            if (result == DialogResult.Yes)
            {              
              
                try {

                    logRichTextBox.AppendText("[数据信息]" + connectionPool.GetConnString() + "\n");

                    ExecuteSqlFile();

                    /*
                     * MySqlScript query = new MySqlScript(File.ReadAllText("d:\\query.sql"), con);
                        query.Execute();
                    */
                    logRichTextBox.AppendText("[重建数据]操作成功\n");
                }catch(Exception ex){
 
                    logRichTextBox.AppendText("[重建数据]操作失败." + ex.Message+"\n");
                    Logger.getInstance().WriteLog("[DataBaseManageWindow.RebuildClick()]" + ex.Message);

                }
            }

        }

        private void FillConfig() {

          

            ip.Text = configFile.ip;
            port.Text = configFile.port;

            username.Text = configFile.username;
            password.Text = configFile.password;
            dbName.Text = configFile.dbName;
        }


        private void ExecuteSqlFile()
        {

            using (StreamReader reader = new StreamReader(initSQL,Encoding.GetEncoding("utf-8")))
            {

                MySqlCommand command = null;
                MySqlConnection conn = null;
                string line = "";
                try
                {
                    conn = connectionPool.getConnection();
                    string sql =null;
                    logRichTextBox.AppendText("[重建数据SQL]\n");

                    while ((sql = reader.ReadLine()) != null)
                    {
                        // 如果line被使用，则设为空
        
                         // l = reader.ReadLine();

                        // 如果到了最后一行，则退出循环
                       // if (l == null) break;
                        // 去除空格
                        sql = sql.TrimEnd();
                        logRichTextBox.AppendText(sql + "\n");
                        // 如果是空行，则跳出循环
                        if (sql == "") continue;
                        // 如果是注释，则跳出循环
                        if (sql.StartsWith("--")) continue;

                        // 行数加1 
                        line += sql;

                    }

                    //执行当前行
                    command = new MySqlCommand(line, conn);
                    command.ExecuteNonQuery();

                }catch(Exception e){
                    
                    logRichTextBox.AppendText("[SQL执行错误]" + e.Message + "\n");
                    throw e;
                }
                finally
                {
                    if (conn != null)
                    {
                        connectionPool.releaseConnection(conn);
                    }
                
                }
            }

        }


        private void password_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void port_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
