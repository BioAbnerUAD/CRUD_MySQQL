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
    public partial class Form1 : Form
    {
        DBConnection connection;
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
            DataTable alumnos = connection.SelectQuery("Select * from Alumnos");
            dataGridView1.DataSource = alumnos;

            var editBtnColumn = new DataGridViewButtonColumn()
            {
                Text = "Edit",
                UseColumnTextForButtonValue = true,
                DataPropertyName = "Edit"
            };
            dataGridView1.Columns.Add(editBtnColumn);

            var deleteBtnColumn = new DataGridViewButtonColumn()
            {
                Text = "Delete",
                UseColumnTextForButtonValue = true,
                DataPropertyName = "Delete"
            };
            dataGridView1.Columns.Add(deleteBtnColumn);
            dataGridView1.AutoResizeColumns();
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                if(senderGrid.Columns[e.ColumnIndex].DataPropertyName == "Edit")
                {
                    DataTable alumnos = dataGridView1.DataSource as DataTable;
                    Form2 form = new Form2("Update",alumnos.Rows[e.RowIndex]);
                    form.ShowDialog();
                    if (!form.submited) return;
                    UpdateField((int)senderGrid.Rows[e.RowIndex].Cells[0].Value, form);
                }
                else if (senderGrid.Columns[e.ColumnIndex].DataPropertyName == "Delete")
                {
                    bool success = connection.ExecuteQuery("Delete from Alumnos where id =" +
                        senderGrid.Rows[e.RowIndex].Cells[0].Value.ToString());

                    if (!success) MessageBox.Show("Unknow failure where deleting index.\n" +
                         "The field might not exist anymore.");
                    UpdateTableData();
                }
            }
        }

        private void UpdateField(int id, Form2 form)
        {
            DataTable alumnos = dataGridView1.DataSource as DataTable;
            string query = "UPDATE Alumnos SET ";
            for (int i = 1; i < alumnos.Columns.Count; i++)
            {
                query += alumnos.Columns[i].ColumnName + "=";
                if (alumnos.Columns[i].DataType == typeof(DateTime))
                {
                    var value = (DateTime)form.dataGridView1.Rows[i - 1].Cells[1].Value;
                    query += "'" + value.ToString("yyyy-MM-dd HH:mm:ss") + "'" + ",";
                }
                else
                {
                    query += "'" + form.dataGridView1.Rows[i - 1].Cells[1].Value + "'" + ",";
                }
            }
            query = query.Remove(query.Length - 1);
            query += "WHERE id=" + id;
            if (connection.ExecuteQuery(query))
            {
                MessageBox.Show("Se logró actualizar con éxito");
                UpdateTableData();
            }
            else
            {
                MessageBox.Show("Ha ocurrido un error");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            DataTable alumnos = dataGridView1.DataSource as DataTable;

            Form2 form = new Form2("Insert",alumnos);
            form.ShowDialog();
            if (!form.submited) return;
            string query = "INSERT into Alumnos(";
            foreach (var col in (from DataColumn c in alumnos.Columns select c).Skip(1))
            {
                query += col.ColumnName + ",";
            }
            query = query.Remove(query.Length - 1);
            query += ") VALUES (";
            for (int i = 1; i < alumnos.Columns.Count; i++)
            {
                if(alumnos.Columns[i].DataType == typeof(DateTime))
                {
                    DateTime value;
                    try
                    {
                        value = (DateTime)form.dataGridView1.Rows[i - 1].Cells[1].Value;
                    }
                    catch(InvalidCastException)
                    {
                        value = DateTime.Parse((String)form.dataGridView1.Rows[i - 1].Cells[1].Value);
                    }
                    query += "'" + value.ToString("yyyy-MM-dd HH:mm:ss") + "'" + ",";
                }
                else
                {
                    query += "'" + form.dataGridView1.Rows[i - 1].Cells[1].Value + "'" + ",";
                }
            }
            query = query.Remove(query.Length - 1);
            query += ")";
            if(connection.ExecuteQuery(query))
            {
                MessageBox.Show("Se logró insertar con éxito");
                UpdateTableData();
            }
            else
            {
                MessageBox.Show("Ha ocurrido un error");
            }
        }
    }
}
