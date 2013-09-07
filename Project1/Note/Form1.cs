using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Note
{
    public partial class Form1 : Form
    {

        public Dictionary<string, TabFile> allOpenFiles = new Dictionary<string, TabFile>();
        private EventControl ec;
        private BinarySerialize serialize = new BinarySerialize();

        public Form1()
        {
            ec = new EventControl(this);
            InitializeComponent();
            AddEventListeners();

            InitLastOpenFile();
        }

      private void AddEventListeners()
        { 

           newToolStripMenuItem.Click +=new EventHandler(ec.NewFile);
           openToolStripMenuItem.Click +=new EventHandler(ec.OpenFile); 
           saveToToolStripMenuItem.Click +=new EventHandler(ec.SaveFileTo); 
           quitToolStripMenuItem.Click +=new EventHandler(ec.Quit);
           saveToolStripMenuItem.Click += new EventHandler(ec.SaveFile);
           copyToolStripMenuItem.Click += new EventHandler(ec.Copy);



           aboutToolStripMenuItem.Click += new EventHandler(ec.About); 
        }

        private void InitLastOpenFile(){

            Array dic = serialize.DeSerialize();

            if(dic!=null ){
                foreach (string fileName in  dic)
                {   

                    ec.CreateTab(fileName);
           
                }
            }
        }

        private void SaveLastOpenFile()
        {
            if (allOpenFiles.Count > 0)
            {
                string[]  arr = new string[allOpenFiles.Count];

                int count = 0;
                foreach(KeyValuePair<String,TabFile> kvp in allOpenFiles){

                    if (kvp.Value.isNewFile)
                    {
                        Show("有未保存的文件,请先保存");
                        return;
                    }

                    arr[count] = kvp.Value.fileName;

                }

                serialize.Serialize(arr);
            }
        }


        public void Show(string msg) {

            Form2 form2 = new Form2();
            form2.SetContent(msg);
            form2.ShowDialog();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
