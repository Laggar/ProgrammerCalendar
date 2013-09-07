using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Note
{
    class EventControl
    {
        
        private Form1 form;
   
        public EventControl(Form form)
        {
            this.form = (Form1)form;
        }


        public void NewFile(object sender, EventArgs e){
            //MessageBox.Show("NewFile");
            CreateTab(null);
        }


        public void CreateTab(String fileName) {

           TabControl tc= form.tabControl;
           TabPage tp= new TabPage();

           TabFile tabFile = new TabFile();
           if (fileName == null)
           {
                fileName = "New File";

           } else {
               tabFile.isNewFile = false;
           }
           tp.Location = new System.Drawing.Point(4, 22);

           tp.UseVisualStyleBackColor = true;
           tp.Text =fileName;
 
           tc.Controls.Add(tp);
           int index =  tc.TabCount - 1;
           tc.SelectedIndex = index;
           tp.Name = "tabPage_"+index;

           RichTextBox richTextBox = new RichTextBox();
           tp.Controls.Add(richTextBox);

           richTextBox.Location = new System.Drawing.Point(120, 8);
           richTextBox.Name = "richTextBox1";
           richTextBox.Dock = DockStyle.Fill;

           tabFile.fileName = fileName;
           tabFile.richTextBox = richTextBox;

           if (!tabFile.isNewFile){
              richTextBox.AppendText(File.ReadAllText(fileName));

            }

            form.allOpenFiles.Add(tp.Name, tabFile);
        }


        public void OpenFile(object sender, EventArgs e)
        {
            //MessageBox.Show("NewFile");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory= Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Filter="文本文件|*.txt|C#文件|*.cs";
         

            if (openFileDialog.ShowDialog()==DialogResult.OK)
            {
                String fName=openFileDialog.FileName;
                CreateTab(fName);

            }
          
        }

        public void SaveFile(object sender, EventArgs e)
        {

            TabFile tabFile = GetCurrentTabFile();

            if (tabFile != null)
            {
                if (tabFile.isNewFile)
                {
                    SaveFileTo(sender, e);

                } else {
                    tabFile.richTextBox.SaveFile(tabFile.fileName, RichTextBoxStreamType.PlainText);
                }
            }

        }

        public void SaveFileTo(object sender, EventArgs e)
        {
        
            TabFile tabFile = GetCurrentTabFile();
            if (tabFile != null)
            {
                SaveFileDialog sfileD = new SaveFileDialog();
                sfileD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (sfileD.ShowDialog() == DialogResult.OK)
                {

                    String fName = sfileD.FileName;

                    tabFile.richTextBox.SaveFile(fName, RichTextBoxStreamType.PlainText);
                    tabFile.isNewFile = false;
                    tabFile.fileName = fName;

                }
            }

        }

        private TabFile GetCurrentTabFile() {
            if (form.allOpenFiles.Count == 0)
            {
                return null;
            }
            TabControl tc = form.tabControl;
            TabPage tabPage = tc.SelectedTab;
            TabFile tabFile = form.allOpenFiles[tabPage.Name];
            return tabFile;
        }


        public void Quit(object sender, EventArgs e){

            form.Close();
        }


        public void Copy(object sender, EventArgs e) { 
             TabFile tabFile = GetCurrentTabFile();
             if (tabFile != null)
             {
                 tabFile.richTextBox.Copy();
             }
        }

        public void About(object sender, EventArgs e) {

            form.Show("测试。。。。。。");
        }
    }



}
