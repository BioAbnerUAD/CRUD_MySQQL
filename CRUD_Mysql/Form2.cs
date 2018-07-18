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
        public bool submited = false;
        public Form2(string title, DataTable alumnos)
        {
            InitializeComponent();
            this.Text = title;
            dataGridView1.Columns.Add("FieldNames", "");
            dataGridView1.Columns.Add("Values", "");
            foreach (var col in (from DataColumn c in alumnos.Columns select c).Skip(1))
            {
                if(col.DataType == typeof(String))
                {
                    dataGridView1.Rows.Add(col.ColumnName, "");
                }
                else if (col.DataType == typeof(DateTime))
                {
                    dataGridView1.Rows.Add(col.ColumnName, new DateTimePicker());
                }
                else
                {
                    dataGridView1.Rows.Add(col.ColumnName, Activator.CreateInstance(col.DataType));
                }

            }
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.AutoResizeColumns();
        }

        public Form2(string title, DataRow row)
        {
            InitializeComponent();
            this.Text = title;
            dataGridView1.Columns.Add("FieldNames", "");
            dataGridView1.Columns.Add("Values", "");
            for (int i = 1; i < row.Table.Columns.Count; i++)
            {
                if(row.Table.Columns[i].DataType == typeof(DateTime))
                {
                    var dtp = new DateTimePicker();
                    dataGridView1.Rows.Add(row.Table.Columns[i].ColumnName, dtp);
                    dtp.Value = (DateTime)row[i];
                }
                else
                {
                    dataGridView1.Rows.Add(row.Table.Columns[i].ColumnName, row[i]);
                }
            }
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.AutoResizeColumns();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            submited = true;
            this.Close();
        }
    }
}
