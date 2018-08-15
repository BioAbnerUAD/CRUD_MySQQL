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
    public partial class InsertForm : Form
    {
        private bool submited = false;
        public bool Submited
        {
            get => submited;
            private set => submited = value;
        }

        public InsertForm(string title, DataTable tabla)
        {
            InitializeComponent();
            this.Text = title;
            tabla.Rows.Clear();
            List<object> values = new List<object>();
            foreach (DataColumn col in tabla.Columns)
            {
                if (col.DataType == typeof(string)) values.Add(String.Empty);
                else if (col.DataType == typeof(int)) values.Add(0);
                else if (col.DataType == typeof(uint)) values.Add(0u);
                else if (col.DataType == typeof(bool)) values.Add(false);
                else values.Add(null);
            }
            tabla.Rows.Add(values.ToArray());
            dataGridView1.DataSource = tabla;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.AutoResizeColumns();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Submited = true;
            this.Close();
        }
    }
}
