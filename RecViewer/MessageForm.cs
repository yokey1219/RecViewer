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
    public partial class MessageForm : Form
    {
        public MessageForm()
        {
            InitializeComponent();
            groupBox1.Visible = false;
            this.Height = 120;
        }

        private const Int32 infoheight = 150;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (groupBox1.Visible == true)
            {
                groupBox1.Visible = false;
                this.Height = this.Height - MessageForm.infoheight;//groupBox1.Height;
                linkLabel1.Text = "查看详情";
            }
            else
            {
                groupBox1.Visible = true;
                this.Height = this.Height + MessageForm.infoheight;// groupBox1.Height;
                linkLabel1.Text = "收起详情";
            }
        }
        static MessageForm _instance = new MessageForm();

        public static void Show(String message,Exception ex)
        {
            _instance.label1.Text = message;
            _instance.label2.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            _instance.tbinfo.Clear();
            _instance.tbinfo.AppendText(ex.Message + Environment.NewLine);
            _instance.tbinfo.AppendText(ex.StackTrace);
            _instance.ShowDialog();
        }
    }
}
