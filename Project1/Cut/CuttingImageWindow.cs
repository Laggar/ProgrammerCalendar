using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Reflection;

using System.Runtime.InteropServices;
using System.Collections;

using System.Runtime.CompilerServices;


namespace Cut
{
    public partial class CuttingImageWindow : Form
    {


        private Point startPoint = new Point(10,10);
        public CuttingImageWindow(CutMark mark)
        {   

            InitializeComponent();
            this.DoubleBuffered = true;
            this.AutoScroll = true;
            
            PaintControl p = new PaintControl(mark);
            p.Location = startPoint;
            this.Controls.Add(p);
            ShowStatusText(mark);
           
        }


        private void ShowStatusText(CutMark mark) { 
            if(mark!=null){
                Board b =  mark.getFromBoard();

                string str = "[长度:" + b.length + "] ";
                str += "[宽度:" + b.width + "] ";
                str += "[可裁切块数:" + mark.getPieceNum() + "] ";

                toolStripStatusLabel.Text = str;
            }
            
        }




        private void CuttingImageWindowPaint(object obj,PaintEventArgs e) {
            
        }


        private void CuttingImageWindow_Load(object sender, EventArgs e)
        {
           // this.Paint += new PaintEventHandler(CuttingImageWindowPaint);
          

        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }


    }
}
