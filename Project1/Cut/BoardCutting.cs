using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cut
{
    public partial class BoardCutting : Form
    {
        private EventControl eventControl;
        public ConfigFile configFile;
        public ConnectionPool connectionPool;
        public BoardService boardService;
        public List<Board> boardList =new List<Board>();


        public BoardCutting()
        {
            InitializeComponent();
            eventControl = new EventControl(this);
            configFile = ConfigFile.getInstance();
            connectionPool = ConnectionPool.getInstance(configFile);
            boardService = new BoardService(connectionPool);
            AddEventListeners();

        }

       private void AddEventListeners()
        {

            //boardToolStripMenuItem.Click += new EventHandler(eventControl.BoardManage);
            dbToolStripMenuItem.Click += new EventHandler(eventControl.DataBaseManage);
            aboutToolStripMenuItem.Click += new EventHandler(eventControl.About);

            search.Click += new EventHandler(eventControl.Search);
            add.Click += new EventHandler(eventControl.Add);
            match.Click += new EventHandler(eventControl.Match);
            showError.Click += new EventHandler(eventControl.ShowError);

            boardListView.CellContentClick += eventControl.BoardListCellContentClick;
            matchListView.CellContentClick += eventControl.MatchListCellContentClick;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void search_Click(object sender, EventArgs e)
        {

        }

        private void add_Click(object sender, EventArgs e)
        {

        }

        public DataGridView GetBoardListDataGrid()
        {

            return boardListView;
        }

        public DataGridView GetMatchListDataGrid()
        {

            return matchListView;
        }


        public void SetStatus(string msg,Color color) {

            //status.Text = msg;
            //status.ForeColor = color;
        }

        public void SettStatusForError(string msg)
        {
            status.Text = msg;
            status.ForeColor = Color.Red;
            
        }

        private void boardListView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //加入一条数据
        public void AddRow(Board board) {
             boardListView.Rows.Add(board.toObjectArray());
             boardList.Add( board);
        }

        //插入一条数据
        public void InsertRow(Board board)
        {
            try
            {
                boardListView.Rows.Insert(1, board.toObjectArray());
                boardList.Insert(0, board);
            }
            catch (Exception e) {
                AddRow(board);
            }
        }

        //更新数据
        public void UpdateRow(Board board, DataGridViewRow row)
        {
            Board find= boardList.Find(delegate(Board b) {
                return b.id == board.id;
            });

            board.CopyTo(find);
            board.CopyTo(row);
        }



        //删除数据
        public void DeleteRow(Board board,DataGridViewRow row)
        {

            boardList.RemoveAll(delegate(Board b)
            {
                return b.id == board.id;
            });

            boardListView.Rows.Remove(row);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
