using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecordFileUtil;

namespace RecViewer
{
    public partial class DataEditForm : Form
    {
        //static String[] headerName = { "A", "B", "C", "D", "E", "F", "G" };
        AbstractRecordInfo _info = null;
        public DataEditForm()
        {
            InitializeComponent();
        }

        /*public void EditData(DataTable dt)
        {
            BindingSource _bs=new BindingSource();
            _bs.DataSource=dt;
            this.dataGridView1.DataSource = _bs;
            if (this.dataGridView1.Columns.Count <= 7)
            {
                foreach (DataGridViewColumn dgvc in dataGridView1.Columns)
                {
                    dgvc.HeaderText = headerName[dgvc.Index];
                }
            }
            else
            {
            }
            this.ShowDialog();
        }*/

        public Boolean EditData(AbstractRecordInfo currentInfo)
        {
            _info = currentInfo;
            if (_info.getHeaderTable() != null)
            {
                BindingSource _bs = new BindingSource();
                DataTable dt = _info.getHeaderTable();
                if (dt.Columns.Count != 4) return false;
                dt.Columns[0].ColumnName = "名称";
                dt.Columns[1].ColumnName = "值";
                dt.Columns[2].ColumnName = "单位";
                _bs.DataSource = dt;
                this.dataGridView1.DataSource = _bs;
                this.dataGridView1.Columns[0].ReadOnly = true;
                this.dataGridView1.Columns[1].Width=160;
                this.dataGridView1.Columns[2].ReadOnly = true;
                this.dataGridView1.Columns[3].Visible = false;

                BindingSource _bsbody = new BindingSource();
                _bsbody.DataSource = _info.getBodyTable();
                this.dataGridView2.DataSource = _bsbody;
                /*for (int i = 0; i < this.dataGridView2.Rows.Count; i++)
                {
                    DataGridViewRow r = this.dataGridView2.Rows[i];
                    r.HeaderCell.Value = string.Format("{0}", i + 1);
                }
                dataGridView2.Refresh();*/
            }

            this.ShowDialog();
            return saveflag;
        }

        Boolean saveflag = false;

        private void toolStripBtnSave_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource != null && dataGridView2.DataSource != null)
            {
                DataTable dtheader = ((dataGridView1.DataSource as BindingSource).DataSource as DataTable);
                DataTable dtbody = ((dataGridView2.DataSource as BindingSource).DataSource as DataTable);

                _info.acceptHeaderChange(dtheader);
                _info.acceptBodyChange(dtbody);

                dtheader.AcceptChanges();
                dtbody.AcceptChanges();

                saveflag = true;
            }
            closeDialog();
        }

        private void closeDialog()
        {
            if (this.dataGridView1.DataSource != null && dataGridView2.DataSource != null)
            {
                DataTable dtheader = ((dataGridView1.DataSource as BindingSource).DataSource as DataTable);
                DataTable dtbody = ((dataGridView2.DataSource as BindingSource).DataSource as DataTable);
                if ((dtbody != null && dtbody.GetChanges()!=null && dtbody.GetChanges().Rows.Count > 0)||( dtheader != null && dtheader.GetChanges()!=null&&dtheader.GetChanges().Rows.Count > 0))
                {
                    if (MessageBox.Show("还没有保存，是否要退出？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
            }
            else
                this.Close();

        }

        private void toolStripBtnQuit_Click(object sender, EventArgs e)
        {
            closeDialog();
        }

        private void dataGridView2_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            //e.Row.HeaderCell.Value = e.Row.Index + 1;
            //sdataGridView2.Refresh();
        }

        private void tsmi_insert_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                DataGridViewRow dgvr = dataGridView2.SelectedRows[0];
                
                int rowIndex = dataGridView2.Rows.IndexOf(dgvr);
                DataTable table = ((dataGridView2.DataSource as BindingSource).DataSource as DataTable);
                if (table != null)
                {
                    DataRow dr=table.NewRow();
                    dr[0] = 0;
                    dr[1] = 0;
                    table.Rows.InsertAt(dr, rowIndex);
                }
            }
            else
            {
                appendNewRow();
            }
        }

        private void appendNewRow()
        {
            BindingSource bs = ((dataGridView2.DataSource) as BindingSource);
            if (bs != null)
            {
                DataRowView dr = bs.AddNew() as DataRowView;
                if (dr != null)
                {
                    dr.Row[0] = 0;
                    dr.Row[1] = 0;
                }
                //dataGridView2.Refresh();
                
            }
        }

        //private void NodeCntAdd(int n)
        //{
        //    //int val =int.Parse(dataGridView1.Rows[_info.NodeCntIdx].Cells[1].Value.ToString());
        //    //dataGridView1.Rows[_info.NodeCntIdx].Cells[1].Value = val + n;
        //}

        private void NodeCntUpdate()
        {
            //int val =int.Parse(dataGridView1.Rows[_info.NodeCntIdx].Cells[1].Value.ToString());
            dataGridView1.Rows[_info.NodeCntIdx].Cells[1].Value = dataGridView2.Rows.Count;
        }

        private void tsmi_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                DataGridViewRow dgvr = dataGridView2.SelectedRows[0];
                if (MessageBox.Show(String.Format("是否删除这一行[{0},{1}]?",dgvr.Cells[0].Value,dgvr.Cells[1].Value), "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    dataGridView2.Rows.Remove(dgvr);
 
                }
            }
        }

        private void tsmi_add_Click(object sender, EventArgs e)
        {
            appendNewRow();
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (Boolean.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString()) == true)
            {
                MessageBox.Show("此值不允许修改");
                e.Cancel = true;
            }
        }

        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            NodeCntUpdate();
        }

        private void dataGridView2_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            NodeCntUpdate();
        }

    }
}
