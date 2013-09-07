using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace Cut
{
    public partial class ErrorLogForm : Form
    {
        public ErrorLogForm()
        {
            InitializeComponent();
            Logger log = Logger.getInstance();
            StreamReader sr =null;
            try
            {

               string strline = null;
               StringBuilder sb = new StringBuilder("[日志文件]"+log.FullLogFileName+"\n");
               using (sr = new StreamReader(log.FullLogFileName))
               {
                   while ((strline=sr.ReadLine()) != null)
                   {
                      sb = sb.Append(strline + "\n");
  
                   }
                }
              
               richTextBoxLog.Text = sb.ToString();

            }
            catch (Exception e) {
                log.WriteLog("[ErrorLogForm.ErrorLogForm()]"+e.Message);
            }finally{
                if(sr!=null){
                    sr.Close();
                    sr=null;
                }
            }

        }

        private void ErrorLogForm_Load(object sender, EventArgs e)
        {

        }
    }
}
