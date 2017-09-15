using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xaut_hospital
{
    public partial class QuerySelect : Form
    {
        public QuerySelect()
        {
            InitializeComponent();
        }

        public Action afterConfirm { get; set; }
        private void button2_Click(object sender, EventArgs e)
        {
            this.listBox2.Items.AddRange(listBox1.Items);
            this.listBox1.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItems.Count > 0)
            {
                string removeitem = this.listBox1.SelectedItem.ToString();
                this.listBox2.Items.Add(removeitem);
                this.listBox1.Items.Remove(removeitem);
            }
            else
            {
                MessageBox.Show("没有选中任何一项！");
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (this.listBox2.SelectedItems.Count > 0)
            {
                string removeitem = this.listBox2.SelectedItem.ToString();
                this.listBox1.Items.Add(removeitem);
                this.listBox2.Items.Remove(removeitem);
            }
            else
            {
                MessageBox.Show("没有选中任何一项！");
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.listBox1.Items.AddRange(listBox2.Items);
            this.listBox2.Items.Clear();
        }

        private void Select_Load(object sender, EventArgs e)
        {
            for(int i =0;i < SelectList.ibl.Count;++i)
            {
                this.listBox1.Items.Add(SelectList.ibl[i]);
            }
            for (int i = 0; i < SelectList.ibr.Count; ++i)
            {
                this.listBox2.Items.Add(SelectList.ibr[i]);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(this.listBox2.Items.Count==0)
            {
                MessageBox.Show("至少选择一列！");
                return;
            }
            
            SelectList.ibl.Clear();
            SelectList.ibr.Clear();

            for(int i =0;i<this.listBox1.Items.Count;++i)
            {
                this.listBox1.SetSelected(i, true);
                SelectList.ibl.Add(this.listBox1.SelectedItem.ToString());
            }
            for (int i = 0; i < this.listBox2.Items.Count; ++i)
            {
                this.listBox2.SetSelected(i, true);
                SelectList.ibr.Add(this.listBox2.SelectedItem.ToString());
            }
            if (afterConfirm != null)
            {
                afterConfirm();
            }
            this.Hide();
        }
    }
}
