using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Note
{
    [Serializable]
    public class TabFile
    {
        public bool isNewFile = true;
        public String fileName;
        public RichTextBox richTextBox;
    }
}
