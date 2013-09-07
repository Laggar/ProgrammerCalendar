using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Runtime.CompilerServices;

namespace Cut
{
    public class ConnectionPool
    {
        private Stack<MySqlConnection> pool;
        private const int POOL_MAX_SIZE = 8;
        private int current_Size = 0;
        private string connString = "";//连接字符串 
        private ConfigFile configFile;


        private static ConnectionPool connPool;

        private ConnectionPool(ConfigFile configFile)
        {
            if (pool == null)
            {
                this.configFile = configFile;
                pool = new Stack<MySqlConnection>();
            }
        }

        public static ConnectionPool refresh(ConfigFile cf)
        {
            return new ConnectionPool(cf);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ConnectionPool getInstance(ConfigFile configFile)
        {
            if (connPool == null)
            {
                connPool = new ConnectionPool(configFile);
            }
            return connPool;
        }

        public MySqlConnection getConnection(){

            MySqlConnection conn;
            lock (this)
            {
                if (pool.Count == 0)
                {
                    if (current_Size < POOL_MAX_SIZE)
                    {
                        conn = createConnection();
                        current_Size++;
                        //把conn加入到pool 中

                        pool.Push(conn);
                    
                    } else {
                        try
                        {
                            Monitor.Wait(this);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                conn = (MySqlConnection)pool.Pop();
            }
            return conn;
        }

        public string GetConnString()
        {
            if (connString == "")
            {

                string ip = configFile.ip;
                string dbName = configFile.dbName;
                string userID = configFile.username;
                string userPwd = configFile.password;
                string port = configFile.port;

                //
                connString = "Database=" + dbName +
                    ";Data Source=" + ip +
                    ";User Id=" + userID +
                    ";Password=" + userPwd + ";" +
                    "pooling=true;CharSet=utf8;" +
                    "port=" + port + ";";
            }
            return connString;
        }

        public void releaseConnection(MySqlConnection conn){

            lock (this)
            {
                pool.Push(conn);
                Monitor.Pulse(this);
            }
        }

        private MySqlConnection createConnection()
        {
            lock (this)
            {
                MySqlConnection newConn = new MySqlConnection(GetConnString());
                newConn.Open();
                return newConn;
            }
        }
    }
}