using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RecViewer
{
    public partial class RecordOpenDialog : Form
    {
        protected String filename=null;

        public String FileName { get { return filename; } }


        public String SelectFile()
        {
            if (this.ShowDialog() == DialogResult.OK)
            {
                return filename;
            }
            else
                return null;
        }

        public RecordOpenDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.filename = this.openFileDialog1.FileName;
                this.textBox1.Text = this.filename;
            }
        }
    }
}
