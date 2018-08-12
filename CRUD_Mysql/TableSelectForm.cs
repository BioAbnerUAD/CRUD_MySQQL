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
    public partial class TableSelectForm : Form
    {
        DBConnection connection;

        public TableSelectForm()
        {
            InitializeComponent();
        }

        private void TableSelectForm_Load(object sender, EventArgs e)
        {
            string errorMsg = string.Empty;
            connection = new DBConnection("138.68.20.16", "aramirez_pfinal", "aramirez", "1234567890");
            if (connection.Connect(ref errorMsg))
            {
                UpdateTables();
            }
            else
            {
                MessageBox.Show("Conection Failure: " + errorMsg);
                Close();
            }
        }

        private void UpdateTables()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            DataTable tabla = connection.SelectQuery("SHOW TABLES;");
            dataGridView1.DataSource = tabla;
            dataGridView1.AutoResizeColumns();
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string tableName = dataGridView1.Rows[e.RowIndex].Cells[0].Value as string;
            CRUDForm form = new CRUDForm(connection, tableName);
            this.Hide();
            form.ShowDialog();
            if (form.PressedBack)
                this.Show();
            else this.Close();
        }
    }
}
