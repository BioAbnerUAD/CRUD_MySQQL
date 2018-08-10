using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Mysql
{
    public partial class Form2 : Form
    {
        private bool submited = false;
        public bool Submited
        {
            get => submited;
            private set => submited = value;
        }

        public Form2(string title, DataTable tabla)
        {
            InitializeComponent();
            this.Text = title;
            dataGridView1.DataSource = tabla;
            dataGridView1.Columns.RemoveAt(0);
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add();
            dataGridView1.AutoResizeColumns();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Submited = true;
            this.Close();
        }
    }
}
