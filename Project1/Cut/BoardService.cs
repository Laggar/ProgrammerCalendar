using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Cut
{
    public class BoardService {

        private ConnectionPool connectionPool;

        public BoardService(ConnectionPool connectionPool)
        {

            this.connectionPool = connectionPool;
        }

        public Board Insert(Board board)
        {

            string insert = "insert into board(length,width,thickness,path) values(?length,?width,?thickness,?path)";
            //string insert = "insert into board(length,width,thickness,path) values(1,2,3,'qqss')";
            MySqlConnection conn=null;

            try{
                conn = connectionPool.getConnection();
                MySqlCommand command = new MySqlCommand();
                command.CommandText = insert;
                command.Connection = conn;

                command.Parameters.AddWithValue("?length", board.length);
                command.Parameters.AddWithValue("?width", board.width);
                command.Parameters.AddWithValue("?thickness", board.thickness);
                command.Parameters.AddWithValue("?path", board.path);

                command.Prepare();

                command.ExecuteNonQuery();


                board.id = getMaxId();

            }catch(Exception ex){
                throw ex;
            }finally{
                if(conn!=null){
                    connectionPool.releaseConnection(conn);
                }
            }

         
            return board;

        }

        private long getMaxId() {

            string sql = "select max(id) id from board ";
            int maxId = 0;

            MySqlConnection conn = null;

            try
            {
                conn = connectionPool.getConnection();
                MySqlCommand command = new MySqlCommand();
                command.CommandText = sql;
                command.Connection = conn;
              

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        maxId = reader.GetInt32("id");
                    }
                }
            

            }
            finally
            {
                if (conn != null)
                {
                    connectionPool.releaseConnection(conn);
                }
            }


            return maxId;

        }

        public List<Board> Select(int thickness)
        {
            return Select(new Board(0, 0, thickness, null));
        }
        public List<Board> Select(Board board)
        {

            string query = "select * from board where 1=1 ";
            MySqlConnection conn = null;
            List<Board> list = new List<Board>();

            try
            {
 
                conn = connectionPool.getConnection();
                MySqlCommand command = new MySqlCommand();

                if (board.length > 0)
                {
                    query += "and length=?length ";
                    command.Parameters.AddWithValue("?length", board.length);
                }

                if (board.width > 0)
                {
                    query += "and width=?width ";
                    command.Parameters.AddWithValue("?width", board.width);
                }

                if (board.thickness > 0)
                {
                    query += "and thickness=?thickness ";
                    command.Parameters.AddWithValue("?thickness", board.thickness);
                }
                if (board.path != null && !board.path.Trim().Equals(""))
                {
                    query += "and path like ?path ";
                    command.Parameters.AddWithValue("?path", "%" + board.path.Trim() + "%");
                }


                command.CommandText = query;
                command.Connection = conn;

                command.Prepare();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Board b = new Board();
                        b.id = reader.GetInt32("id");
                        b.length = reader.GetInt16("length");
                        b.width = reader.GetInt16("width");
                        b.thickness = reader.GetInt16("thickness");
                        b.path = reader.GetString("path");
                        list.Add(b);
                    }
                }

            }
            finally
            {
                if (conn != null)
                {
                    connectionPool.releaseConnection(conn);
                }
            }

            return list;
        }



        public void Delete(long id)
        {
            string delete = "delete from board where id=?id ";
            MySqlConnection conn = null;

            try
            {
                conn = connectionPool.getConnection();
                MySqlCommand command = new MySqlCommand();
                command.CommandText = delete;
                command.Connection = conn;
                command.Parameters.AddWithValue("?id",id);
                command.ExecuteNonQuery();
            }
            finally
            {
                if (conn != null)
                {
                    connectionPool.releaseConnection(conn);
                }
            }
        }

        public void execute() { 
        
      
        }

        public void Update(Board board)
        {
            string update = "update board set length=?length,width=?width,thickness=?thickness,path=?path where id=?id ";
            MySqlConnection conn = null;

            try
            {
                conn = connectionPool.getConnection();
                MySqlCommand command = new MySqlCommand();
                command.CommandText = update;
                command.Connection = conn;
                command.Parameters.AddWithValue("?length", board.length);
                command.Parameters.AddWithValue("?width", board.width);
                command.Parameters.AddWithValue("?thickness", board.thickness);
                command.Parameters.AddWithValue("?path", board.path);
                command.Parameters.AddWithValue("?id", board.id);
                command.Prepare();
                command.ExecuteNonQuery();

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

}
