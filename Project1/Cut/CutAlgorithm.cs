using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cut
{
    public class CutAlgorithm
    {

        private Board fromBoard;
        private Board toBoard;

        private Board convertFromBoard;
        private Board convertToBoard;

        public CutAlgorithm() {
          
        }


        public void setFromBoard(Board fromBoard)
        {
            this.fromBoard = fromBoard;
            convertFromBoard = this.fromBoard.Convert();
        }


        public void setToBoard(Board toBoard)
        {
            this.toBoard = toBoard;
            this.convertToBoard = this.toBoard.Convert();
        }


        public CutMark cut()
        {

            CutMark cutMarkHorizontal = cutByHorizontal();
            CutMark cutMarkyVertical = cutByVertical();

            if (cutMarkyVertical.getPieceNum() > cutMarkHorizontal.getPieceNum())
            {
                return cutMarkyVertical;
            }


            return cutMarkHorizontal;
        }

        public CutMark cutByHorizontal(){
		
		    CutMark cutMark = new CutMark();
            cutMark.fromBoard = convertFromBoard;
            cutMark.toBoard = convertToBoard;


            int fromBoardLength = cutMark.fromBoard.getLength();
            int fromBoardWidth = cutMark.fromBoard.getWidth();

            int toBoardLength = cutMark.toBoard.getLength();
            int toBoardWidth = cutMark.toBoard.getWidth();

            if (toBoardLength == 0 || toBoardWidth == 0) {
                return cutMark;
            }

            if (fromBoardLength >= toBoardLength)
            {

		        int horizontalNum = fromBoardLength/toBoardLength;
		        int verticalNum = fromBoardWidth/toBoardWidth;
		        int num1 = horizontalNum*verticalNum;
		
		        CutMethod cut1 = new CutMethod();
		        cut1.direction=CutMethod.HORIZONTAL;
		        cut1.times=num1;
		        cut1.hBlock=horizontalNum;
		        cut1.vBlock=verticalNum;
		        cutMark.firstCutMethod=cut1;
		
		
		        int horizontalRemainder = remainder(fromBoardLength,toBoardLength);
		        int remainderHorizontalNum = horizontalRemainder/toBoardWidth;

		        int remainderVerticalNum = fromBoardWidth/toBoardLength;
		        int num2 = remainderHorizontalNum*remainderVerticalNum;
		
		
		        CutMethod cut2 = new CutMethod();
		        cut2.direction=CutMethod.VERTICAL;
		        cut2.times=num2;
		        cut2.hBlock=remainderHorizontalNum;
		        cut2.vBlock=remainderVerticalNum;
		        cutMark.secondCutMethod=cut2;
		        cutMark.pieceNum=num1+num2;

            }
		
		    return cutMark;
	    }

        public CutMark cutByVertical(){
		
		    CutMark cutMark = new CutMark();

            cutMark.fromBoard = convertFromBoard;
            cutMark.toBoard = convertToBoard;

            int fromBoardLength = cutMark.fromBoard.getLength();
            int fromBoardWidth = cutMark.fromBoard.getWidth();

            int toBoardLength = cutMark.toBoard.getLength();
            int toBoardWidth = cutMark.toBoard.getWidth();

            if (toBoardLength == 0 || toBoardWidth == 0)
            {
                return cutMark;
            }

            if (fromBoardLength >= toBoardWidth)
            {
                int horizontalNum = fromBoardLength / toBoardWidth;
                int verticalNum = fromBoardWidth / toBoardLength;
                int num1 = horizontalNum * verticalNum;


                CutMethod cut1 = new CutMethod();
                cut1.direction = CutMethod.VERTICAL;
                cut1.times = num1;
                cut1.hBlock = horizontalNum;
                cut1.vBlock = verticalNum;
                cutMark.firstCutMethod = cut1;


                int horizontalRemainder = remainder(fromBoardWidth, toBoardLength);
                int remainderHorizontalNum = fromBoardLength / toBoardLength;
                //int remainderHorizontalNum = horizontalRemainder / toBoardWidth;
                int remainderVerticalNum = horizontalRemainder / toBoardWidth;

                int num2 = remainderHorizontalNum * remainderVerticalNum;


                CutMethod cut2 = new CutMethod();
                cut2.direction = CutMethod.HORIZONTAL;
                cut2.times = num2;
                cut2.hBlock = remainderHorizontalNum;
                cut2.vBlock = remainderVerticalNum;
                cutMark.secondCutMethod = cut2;

                cutMark.pieceNum = num1 + num2;


            }
		
		    return cutMark;
	    }


        public int remainder(int dividend, int divisor)
        {

            if (divisor > 0 && dividend > 0 && dividend > divisor)
            {

                return dividend % divisor;
            }

            return 0;
        }
 
    }


    public class CutMark
    {

        public Board fromBoard;
        public Board toBoard;
        public int pieceNum = 0;
        public float totalPrice = 0;


        public CutMethod firstCutMethod;
        public CutMethod secondCutMethod;


        public CutMethod getFirstCutMethod()
        {
            return firstCutMethod;
        }
        public void setFirstCutMethod(CutMethod firstCutMethod)
        {
            this.firstCutMethod = firstCutMethod;
        }
        public CutMethod getSecondCutMethod()
        {
            return secondCutMethod;
        }


        public void setSecondCutMethod(CutMethod secondCutMethod)
        {
            this.secondCutMethod = secondCutMethod;
        }
        public Board getFromBoard()
        {
            return fromBoard;
        }
        public void setFromBoard(Board fromBoard)
        {
            this.fromBoard = fromBoard;
        }
        public Board getToBoard()
        {
            return toBoard;
        }
        public void setToBoard(Board toBoard)
        {
            this.toBoard = toBoard;
        }
        public int getPieceNum()
        {
            return pieceNum;
        }
        public void setPieceNum(int pieceNum)
        {
            this.pieceNum = pieceNum;
        }


        public float getTotalPrice()
        {
            return totalPrice;
        }
        public void setTotalPrice(float totalPrice)
        {
            this.totalPrice = totalPrice;
        }
    }





    public class Board
    {

        public long id;
        public int length;
        public int width;
        public int thickness;
        public string path;

        public Board() { }

        public Board(int length, int width, int thickness, string path)
        {
            this.length = length;
            this.width = width;
            this.thickness = thickness;
            this.path = path;
        }



        public Board(long id, int length, int width, int thickness, string path)
            : this(length, width, thickness, path)
        {

            this.id = id;

        }

        public int getWidth()
        {
            return width;
        }


        public int getLength()
        {
            return length;
        }

        public Board Convert()
        {
            Board copy = new Board();
            CopyTo(copy);
            
            int w = copy.width;

            if (copy.width > copy.length)
            {
                copy.width = copy.length;
                copy.length = w;
            }

            return copy;
        }

        public object[] toObjectArray()
        {

            return new object[] { id, length, width, thickness, path, "编辑", "删除" };

        }

        public void CopyTo(Board copy)
        {

            copy.id = this.id;
            copy.length = this.length;
            copy.width = this.width;
            copy.thickness = this.thickness;
            copy.path = this.path;

        }
        public void CopyTo(DataGridViewRow row)
        {

            row.Cells[0].Value = this.id;
            row.Cells[1].Value = this.length;
            row.Cells[2].Value = this.width;
            row.Cells[3].Value = this.thickness;
            row.Cells[4].Value = this.path;
        }

        public static Board RowToBoard(DataGridViewRow row)
        {
            Board b = new Board(); ;
            b.id = long.Parse(row.Cells[0].Value.ToString());

            b.length = int.Parse(row.Cells[1].Value.ToString());

            b.width = int.Parse(row.Cells[2].Value.ToString());

            b.thickness = int.Parse(row.Cells[3].Value.ToString());

            b.path = row.Cells[4].Value.ToString();
            return b;
        }
    }

    public class CutMethod
    {

        public int direction = HORIZONTAL;
        public int times=0;
        public int hBlock = 0;
        public int vBlock = 0;

        public static int HORIZONTAL = 1;
        public static int VERTICAL = 2;

    }


}
