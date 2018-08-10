using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CRUD_Mysql
{
    public class DBConnection
    {
        private string serverIP = string.Empty;
        private string username = string.Empty;
        private string password = string.Empty;
        private string databaseName = string.Empty;
        private MySqlConnection connection = null;

        public string ServerIP { get => serverIP; }
        public string Username { get => username; }
        public string Password { get => password; }
        public string DatabaseName { get => databaseName; }
        public MySqlConnection Connection { get => connection; }

        public DBConnection(string serverIP, string databaseName, string username, string password)
        {
            this.serverIP = serverIP;
            this.databaseName = databaseName;
            this.username = username;
            this.password = password;
        }

        public bool Connect(ref string errorMsg)
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(databaseName))
                    return false;
                string connstring = string.Format("Server={0}; Database={1}; Uid={2}; Pwd={3};SslMode=none;",
                   serverIP, databaseName, username, password);
                connection = new MySqlConnection(connstring);

                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    return false;
                }
            }

            return true;
        }

        public DataTable SelectQuery(string query)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            MySqlDataReader reader = cmd.ExecuteReader();
            DataTable result = new DataTable();
            result.Load(reader);
            reader.Close();
            return result;
        }

        public DataTable SelectQuery(string query, params KeyValuePair<string, object>[] args)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            foreach (var item in args)
            {
                cmd.Parameters.AddWithValue(item.Key, item.Value);
            }
            MySqlDataReader reader = cmd.ExecuteReader();
            DataTable result = new DataTable();
            result.Load(reader);
            reader.Close();
            return result;
        }

        public bool ExecuteQuery(string query)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            return reader.RecordsAffected > 0;
        }

        public bool ExecuteQuery(string query, params KeyValuePair<string,object>[] args)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            foreach (var item in args)
            {
                cmd.Parameters.AddWithValue(item.Key, item.Value);
            }
            cmd.Prepare();
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            return reader.RecordsAffected > 0;
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
