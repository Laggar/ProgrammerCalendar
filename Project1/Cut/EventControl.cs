using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Cut
{
    class EventControl
    {
        private BoardCutting boardCutting;
        private DataBaseManageWindow dataBaseForm;
        private BoardInfoWindow boardInfoForm;
        private List<CutMark> cutMarkList = new List<CutMark>();
        private CuttingImageWindow imageWindow;

        private ErrorLogForm logForm;

        public EventControl(BoardCutting boardCutting) {
            this.boardCutting = boardCutting;
         
        }
  

        //数据库管理
        public void DataBaseManage(object sender, EventArgs e){

            if (dataBaseForm==null)
            {
                 dataBaseForm = new DataBaseManageWindow(boardCutting);
            }
            dataBaseForm.ShowDialog(boardCutting);
           
        }

        public void ShowError(object sender, EventArgs e) {

            if (logForm == null)
            {
                logForm = new ErrorLogForm();
            }
            logForm.ShowDialog(boardCutting);
        }

        //关于
        public void About(object sender, EventArgs e) {

         
        }
        
        //查询
        public void Search(object sender, EventArgs e) {
            try {
        

                string pathText =boardCutting. path.Text;
                int l = Convert.ToInt32(boardCutting.length.Value);
                int w = Convert.ToInt32(boardCutting.width.Value);
                int t = Convert.ToInt32(boardCutting.thickness.Value);

                Board board = new Board(l, w, t, pathText);

                List<Board> list = boardCutting.boardService.Select(board);               
                DataGridViewRowCollection rgbc = boardCutting.GetBoardListDataGrid().Rows;
                rgbc.Clear();
                boardCutting.boardList.Clear();

                foreach (Board b in list)
                {
                    boardCutting.AddRow(b);

                }

            }catch(Exception ex){
                Logger.getInstance().WriteLog("[EventControl.Search()]" + ex.Message);
                boardCutting.SettStatusForError("查询错误,请查看日志");
            }
        }

        //新增
        public void Add(object sender, EventArgs e)
        {
            if (boardInfoForm == null)
            {
                boardInfoForm = new BoardInfoWindow(boardCutting);
            }
            boardInfoForm.SetFlag(0);
            boardInfoForm.Init();
            boardInfoForm.ShowDialog(boardCutting);
        }

        //匹配
        public void Match(object sender, EventArgs e){

            Board to = new Board();
            int l = Convert.ToInt32(boardCutting.toLength.Value);
            int w = Convert.ToInt32(boardCutting.toWidth.Value);
            int t = Convert.ToInt32(boardCutting.toThickness.Value);

            to.length = l;
            to.width = w;
            to.thickness = t;

            if (t <= 0) {
                boardCutting.SettStatusForError("请输入厚度");
                return;
            }
            try
            {
                CutAlgorithm cutAlgorithm = new CutAlgorithm();
                List<Board> list = boardCutting.boardService.Select(t);
                boardCutting.SettStatusForError("");
                cutMarkList.Clear();
                foreach (Board from in list)
                {

                    cutAlgorithm.setFromBoard(from);
                    cutAlgorithm.setToBoard(to);
                    CutMark mark = cutAlgorithm.cut();
                    if (mark.getPieceNum() > 0)
                    {
                        cutMarkList.Add(mark);
                    }
                }

                DataGridViewRowCollection rgbc = boardCutting.GetMatchListDataGrid().Rows;
                rgbc.Clear();

                foreach (CutMark mark in cutMarkList)
                {
                    Board from = mark.getFromBoard();
                    rgbc.Add(from.id, from.length, from.width, from.thickness, from.path, mark.getPieceNum(), "查看");
                }
            }catch(Exception ex){
                Logger.getInstance().WriteLog("[EventControl.Match()]" + ex.Message);
                boardCutting.SettStatusForError("匹配错误,请查看日志");
            }
        }

        //点击查看切割方法
        public void MatchListCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView view = boardCutting.GetMatchListDataGrid();
            if (IsANonHeaderLinkCell(view, e))
            {
                try
                {
                    DataGridViewRow row = view.Rows[e.RowIndex];
                    Board from = Board.RowToBoard(row);

                    CutMark mark = cutMarkList.Find(delegate(CutMark m)
                    {
                        return from.id == m.getFromBoard().id;
                    });



                    imageWindow = new CuttingImageWindow(mark);

                    //imageWindow.SetCutMark(mark);
                    // imageWindow.Init();
                    imageWindow.ShowDialog(boardCutting);
                }
                catch (Exception ex)
                {
                    Logger.getInstance().WriteLog("[EventControl.MatchListCellContentClick()]" + ex.Message);
                    boardCutting.SettStatusForError("切割错误,请查看日志");
                }

            }
        }

        public void BoardListCellContentClick(object sender, DataGridViewCellEventArgs e) {

            DataGridView view = boardCutting.GetBoardListDataGrid();

            if (IsANonHeaderLinkCell(view,e))
            {
                 
                    //编辑
                DataGridViewRow row = view.Rows[e.RowIndex];

                int index = e.ColumnIndex;
                try
                {
                    if (5 == index)
                    {

                        if (boardInfoForm == null)
                        {
                            boardInfoForm = new BoardInfoWindow(boardCutting, 1);
                        }
                        boardInfoForm.SetFlag(1);
                        boardInfoForm.SetRow(row);
                        boardInfoForm.Init();
                        boardInfoForm.ShowDialog(boardCutting);

                    }
                }
                catch (Exception ex)
                {
                    Logger.getInstance().WriteLog("[EventControl.BoardListCellContentClick()]" + ex.Message);
                    boardCutting.SettStatusForError("查看错误,请查看日志");
                }
                try
                {
                    //删除
                    if (6 == index)
                    {
                        DialogResult result = MessageBox.Show("是否删除数据，请确认", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            Board board = Board.RowToBoard(row);
                            boardCutting.boardService.Delete(board.id);
                            boardCutting.DeleteRow(board,row);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Logger.getInstance().WriteLog("[EventControl.BoardListCellContentClick()]" + ex.Message);
                    boardCutting.SettStatusForError("删除错误,请查看日志");
                }
            }
        }

        private bool IsANonHeaderLinkCell(DataGridView view ,DataGridViewCellEventArgs cellEvent)
        {
            if (view.Columns[cellEvent.ColumnIndex] is  DataGridViewLinkColumn && cellEvent.RowIndex != -1)
            { 
                return true; 
            }

            return false;
        }
    }
}
