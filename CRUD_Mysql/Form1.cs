using System;
using System.Collections;
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
    public partial class Form1 : Form
    {
        DBConnection connection;

        public string TableName { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string errorMsg = string.Empty;
            connection = new DBConnection("138.68.20.16", "aramirez", "aramirez", "1234567890");
            if (connection.Connect(ref errorMsg))
            {
                UpdateTableData();
            }
            else
            {
                MessageBox.Show("Conection Failure: " + errorMsg);
                Close();
            }
        }

        private void UpdateTableData()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            DataTable tabla = connection.SelectQuery("Select * from '" + TableName + "'");
            dataGridView1.DataSource = tabla;
            dataGridView1.AutoResizeColumns();
            dataGridView1.Columns[0].ReadOnly = true;
        }
        
        private void Button1_Click(object sender, EventArgs e)
        {
            DataTable tabla = dataGridView1.DataSource as DataTable;

            Form2 form = new Form2("Insert", tabla);
            form.ShowDialog();
            if (!form.Submited) return;
            string query = "INSERT into " + TableName + "(";
            List<KeyValuePair<string,object>> args = new List<KeyValuePair<string, object>>();
            foreach (var col in (from DataColumn c in tabla.Columns select c).Skip(1))
            {
                query += col.ColumnName + ",";
            }
            query = query.Remove(query.Length - 1);
            query += ") VALUES (";
            for (int i = 1; i < tabla.Columns.Count; i++)
            {
                query += "@val" + i + ",";
                if (tabla.Columns[i].DataType == typeof(DateTime))
                {
                    var value = (DateTime)form.dataGridView1.Rows[0].Cells[i - 1].Value;
                    args.Add(new KeyValuePair<string, object>
                        ("@val" + i, value.ToString("yyyy-MM-dd HH:mm:ss")));
                }
                else
                {
                    args.Add(new KeyValuePair<string, object>
                        ("@val" + i, form.dataGridView1.Rows[0].Cells[i - 1].Value));
                }
            }
            query = query.Remove(query.Length - 1);
            query += ")";
            if(connection.ExecuteQuery(query, args.ToArray()))
            {
                MessageBox.Show("Se logró insertar con éxito");
                UpdateTableData();
            }
            else
            {
                MessageBox.Show("Ha ocurrido un error");
            }
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataTable tabla = dataGridView1.DataSource as DataTable;
            string query = "UPDATE " + TableName + " SET "
                + tabla.Columns[e.ColumnIndex].ColumnName + "= @val "
                + "WHERE id = " + tabla.Rows[e.RowIndex][0];

            if (connection.ExecuteQuery(query, 
                new KeyValuePair<string, object>("@val", tabla.Rows[e.RowIndex][e.ColumnIndex])))
            {
                MessageBox.Show("Se logró actualizar con éxito");
                UpdateTableData();
            }
            else
            {
                MessageBox.Show("Ha ocurrido un error");
            }
        }

        private void DataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var senderGrid = sender as DataGridView;
            bool success = connection.ExecuteQuery("Delete from Alumnos where id =" +
                        e.Row.Cells[0].Value.ToString());

            if (!success) MessageBox.Show("Unknow failure where deleting index.\n" +
                 "The field might not exist anymore.");
            UpdateTableData();
            e.Cancel = true;
        }
    }
}
