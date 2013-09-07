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
    public partial class BoardInfoWindow : Form
    {   
        private BoardCutting boardCutting;
        public int flag = 0;//0表示新增 1修改
        private Board editBoard;
        private DataGridViewRow row;

        public BoardInfoWindow(BoardCutting boardCutting)
        {
            InitializeComponent();
            this.boardCutting = boardCutting;
            Init();
            AddEventListeners();
        }

        public void SetFlag(int flag)
        {
            this.flag = flag;
           
        }

        public void SetRow(DataGridViewRow row)
        {
            this.row = row;
            this.editBoard = Board.RowToBoard(row);
        }

        public void Init() {

            
            if (flag == 0)
            {
                this.Text = "新增板材";
                length.Value = 0;
                width.Value = 0;
                thickness.Value =0;
                path.Text = "";
                editBoard = null;
                row = null;
            }
            else
            {
                this.Text = "编辑板材";
                length.Value = editBoard.length;
                width.Value=editBoard.width;
                thickness.Value = editBoard.thickness;
                path.Text = editBoard.path;
            }
            SetInfo("");
        }


        public BoardInfoWindow(BoardCutting boardCutting,int flag): this(boardCutting)
        {
            this.flag = flag;
         
        }

        private void AddEventListeners()
        {
            confirm.Click += new EventHandler(ConfirmClick);
            cancel.Click += new EventHandler(CancelClick);
        }


        private void ConfirmClick(object sender, EventArgs e)
        {
            string pathText = path.Text;
            if (pathText == null || pathText.Trim().Equals(""))
            {
                SetInfo("请填写路径");
                return;
            }
            if (flag == 0)
            {
                Save();

            }
            else {
                Update();
            }
        }

        private void Save() {

            string pathText = path.Text;
            int l = Convert.ToInt32(length.Value);
            int w = Convert.ToInt32(width.Value);
            int t = Convert.ToInt32(thickness.Value);

            try
            {
                Board board = new Board(l, w, t, pathText);
                boardCutting.boardService.Insert(board);
                boardCutting.InsertRow(board);
                SetInfo("新增成功");


            } catch (Exception ex) {
                Logger.getInstance().WriteLog("[BoardInfoWindow.Save()]" + ex.Message);
                SetInfo("新增失败,请查看日志");
            }
        }

        private void Update() {  

            string pathText = path.Text;
            int l = Convert.ToInt32(length.Value);
            int w = Convert.ToInt32(width.Value);
            int t = Convert.ToInt32(thickness.Value);

            try
            {
                editBoard.length = l;
                editBoard.width = w;
                editBoard.thickness = t;
                editBoard.path = pathText;

                boardCutting.boardService.Update(editBoard);
                boardCutting.UpdateRow(editBoard,row);

                SetInfo("编辑成功");
            }
            catch (Exception ex)
            {
                Logger.getInstance().WriteLog("[BoardInfoWindow.Update()]" + ex.Message);
                SetInfo("编辑失败,请查看日志");
            }
        
        }

        private void CancelClick(object sender, EventArgs e)
        {
            Hide();
        }

        private void SetInfo(string msg) {
            info.Text = msg;
        }
        private void thickness_ValueChanged(object sender, EventArgs e)
        {

        }

        private void BoardInfoWindow_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
