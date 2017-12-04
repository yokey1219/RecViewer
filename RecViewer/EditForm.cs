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
    public partial class EditForm : Form
    {
        public EditForm()
        {
            InitializeComponent();
        }

        public String EditValue(String str)
        {
            this.tbvalue.Text = str;
            if (this.ShowDialog() == DialogResult.OK)
            {
                return this.tbvalue.Text;
            }
            else
                return str;
            
        }
    }
}
