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
    public class PaintControl:Control
    {

        private CutMark mark;
        private static int FIX_LENGTH = 640;
        private static int FIX_WIDTH = 440;
        private Point startPoint = new Point(1,1);
        private Random random = new Random();
        private Brush[] brushs;
        private static int MAX_BRUSH = 100;
        private int brushLen = 0;
        private Board covertBoard;

        public PaintControl()
        { 
        
        }

        public PaintControl(CutMark mark)
        {
            this.mark = mark;
            init();
            this.DoubleBuffered = true;
           
           
        }

        //设置裁切方式 初始化笔刷
        public void init()
        {
          
            if (mark != null)
            {

                initBrush(mark.getPieceNum());
                covertBoard = new Board();
                mark.getFromBoard().CopyTo(covertBoard);
                Convert(covertBoard);

                this.Width = covertBoard.length+3;
                this.Height = covertBoard.width+3;
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            if (mark == null) {

                return;
            }

         
            Graphics g = e.Graphics;
           

            //Bitmap image = new Bitmap(pictureBox.ClientSize.Width, pictureBox.ClientSize.Height);

            // 初始化图形面板
            //Graphics g = Graphics.FromImage(image);



            if (mark != null)
            {



                Pen penForBorder = new Pen(Color.Red);

                Rectangle border = new Rectangle(new Point(0 , 0), new Size(covertBoard.length+2, covertBoard.width+2));
                g.DrawRectangle(penForBorder, border);


                CutMethod cutMethod = mark.getFirstCutMethod();

                if (CutMethod.HORIZONTAL == cutMethod.direction)
                {

                    DrawFromHorizontal(g);
                }
                else
                {
                    DrawFromVertical(g);
                }

            }


            //pictureBox.Refresh();
            //pictureBox.CreateGraphics().DrawImage(image, 0, 0);

        }

        private void initBrush(int num)
        {
            if (num > MAX_BRUSH)
            {
                brushLen = MAX_BRUSH;
            }
            else
            {
                brushLen = num;
            }

            brushs = new Brush[brushLen];

            for (int i = 0; i < brushLen; i++)
            {
                brushs[i] = new SolidBrush(GetRandomColor());
            }

        }

        //横向裁切
        private void DrawFromHorizontal(Graphics g)
        {

            CutMethod method = mark.getFirstCutMethod();
            Board to = new Board();
            mark.getToBoard().CopyTo(to);
            Convert(covertBoard, mark.getFromBoard(), to);


            int hb = method.hBlock;
            int vb = method.vBlock;
            int index = 0;

            for (int i = 0; i < hb; i++)
            {
                for (int j = 0; j < vb; j++)
                {


                    Point p = new Point(startPoint.X + to.length * i, startPoint.Y + to.width * j);

                    Pen penForBorder = new Pen(Color.PowderBlue);

                    Rectangle border = new Rectangle(p, new Size(to.length, to.width));

                    Brush b = GetBrush(index);

                    g.DrawRectangle(penForBorder, border);
                    g.FillRectangle(b, border);
                  
                    drawString((index + 1) + "\n(" + to.length + "*" + to.width + ")", p.X, p.Y, g);

                    index++;
                }
            }


            CutMethod method2 = mark.getSecondCutMethod();
            int hb2 = method2.hBlock;
            int vb2 = method2.vBlock;

            int m2st = hb * to.length;

            for (int i = 0; i < hb2; i++)
            {
                for (int j = 0; j < vb2; j++)
                {

                    Point p = new Point(startPoint.X  + m2st + to.width * i, startPoint.Y  + to.length * j);

                    Pen penForBorder = new Pen(Color.PowderBlue);

                    Rectangle border = new Rectangle(p, new Size(to.width, to.length));

                    Brush b = GetBrush(index);
                    g.DrawRectangle(penForBorder, border);
                    g.FillRectangle(b, border);

                    drawString((index + 1) + "\n(" + to.width + "*" + to.length + ")", p.X, p.Y, g);

                    index++;

                }

            }
        }

        //纵向裁切
        private void DrawFromVertical(Graphics g)
        {

            CutMethod method = mark.getFirstCutMethod();
            Board to = new Board();
            mark.getToBoard().CopyTo(to);
            Convert(covertBoard, mark.getFromBoard(), to);


            int hb = method.hBlock;
            int vb = method.vBlock;
            int index = 0;

            for (int i = 0; i < hb; i++)
            {
                for (int j = 0; j < vb; j++)
                {


                    Point p = new Point(startPoint.X + to.width * i, startPoint.Y + to.length * j);

                    Pen penForBorder = new Pen(Color.PowderBlue);

                    Rectangle border = new Rectangle(p, new Size(to.width, to.length));

                    Brush b = GetBrush(index);

                    g.DrawRectangle(penForBorder, border);
                    g.FillRectangle(b, border);

                    drawString((index + 1) + "\n(" + to.width + "*" + to.length + ")", p.X, p.Y, g);

                    index++;
                }
            }


            CutMethod method2 = mark.getSecondCutMethod();
            int hb2 = method2.hBlock;
            int vb2 = method2.vBlock;

            int m2st = hb * to.length;

            for (int i = 0; i < hb2; i++)
            {
                for (int j = 0; j < vb2; j++)
                {

                    Point p = new Point(startPoint.X + to.length * i, m2st + startPoint.Y + to.width * j);

                    Pen penForBorder = new Pen(Color.PowderBlue);

                    Rectangle border = new Rectangle(p, new Size(to.length, to.width));

                    Brush b = GetBrush(index);
                    g.DrawRectangle(penForBorder, border);
                    g.FillRectangle(b, border);

                    drawString((index + 1) + "\n(" + to.length + "*" + to.width + ")", p.X, p.Y, g);

                    index++;

                }

            }
        }

        private void drawString(string str,float x,float y ,Graphics g) {

            Font f = new Font("宋体", 11, FontStyle.Bold);
            g.DrawString(" " + str, f, Brushes.White, x + 4, y + 4);
        }

        private Brush GetBrush(int index)
        {

            if (index >= brushLen)
            {
                return brushs[index % brushLen];
            }
            else
            {
                return brushs[index];
            }


        }


        private Color GetRandomColor()
        {

            Color color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));

            return color;
        }



        private Board Convert(Board covertBoard, Board from, Board board)
        {


            if (board.length >= FIX_LENGTH || board.width > FIX_WIDTH)
            {
                return board;

            }

            board.length = (int)((float)covertBoard.length * board.length / from.length);
            board.width = (int)((float)covertBoard.width * board.width / from.width);
            return board;
        }

        private Board Convert(Board board)
        {

            if (board.length >= FIX_LENGTH || board.width > FIX_WIDTH)
            {
                return board;

            }

            board.length = (int)(FIX_WIDTH * (float)board.length / board.width);

            board.width = FIX_WIDTH;

            return board; ;
        }

    }
}
