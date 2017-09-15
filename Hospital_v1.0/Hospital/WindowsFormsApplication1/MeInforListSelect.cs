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
    public partial class MeInforListSelect : Form
    {
        public MeInforListSelect()
        {
            InitializeComponent();
        }

        public Action afterConfirm { get; set; }
        private void MeInforListSelect_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < SelectListSet.left.Count; ++i)
            {
                this.listBox1.Items.Add(SelectListSet.left[i]);
            }
            for (int i = 0; i < SelectListSet.right.Count; ++i)
            {
                this.listBox2.Items.Add(SelectListSet.right[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.listBox2.Items.AddRange(listBox1.Items);
            this.listBox1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.listBox2.SelectedItems.Count > 0)
            {
                if(this.listBox2.SelectedItem.ToString() != "药号")
                {
                    string removeitem = this.listBox2.SelectedItem.ToString();
                    this.listBox1.Items.Add(removeitem);
                    this.listBox2.Items.Remove(removeitem);
                }
                else
                {
                    MessageBox.Show("不能移除“药号”！");
                }
            }
            else
            {
                MessageBox.Show("没有选中任何一项！");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.AddRange(listBox2.Items);
            this.listBox2.Items.Clear();
            this.listBox2.Items.Add("药号");
            this.listBox1.Items.Remove("药号");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.listBox2.Items.Count == 0)
            {
                MessageBox.Show("至少选择一列！");
                return;
            }
            SelectListSet.left.Clear();
            SelectListSet.right.Clear();

            for (int i = 0; i < this.listBox1.Items.Count; ++i)
            {
                this.listBox1.SetSelected(i, true);
                SelectListSet.left.Add(this.listBox1.SelectedItem.ToString());
            }
            for (int i = 0; i < this.listBox2.Items.Count; ++i)
            {
                this.listBox2.SetSelected(i, true);
                SelectListSet.right.Add(this.listBox2.SelectedItem.ToString());
            }
            if (afterConfirm != null)
            {
                afterConfirm();
            }
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
