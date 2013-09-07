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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void SetContent(string text)
        {
            label1.Text = text;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
           
        }

    }
}
