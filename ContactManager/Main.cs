using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContactManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            using (var form = new CreateEdit())
            {
                form.ShowDialog(this);
            } 
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            using (var form = new ContactList())
            {
                form.ShowDialog(this);
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (var form = new ContactList(true))
            {
                form.ShowDialog(this);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var form = new GitCommit())
            {
                form.ShowDialog(this);
            }
        }
    }
}
