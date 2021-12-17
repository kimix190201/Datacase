using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using MySql.Data.MySqlClient;

namespace CPUTemp2
{
    public class Database
    {
        public MySqlConnection connection;
        public Database()
        {
            string connStr = "server=109.204.232.139;user=datacase;database=datacase;port=3306;password=SQLvaliLite2021";
            connection = new MySqlConnection(connStr);
        }

        public void OpenConnection()
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }

        public void CloseConnection()
        {
            connection.Close();
        }
    }
}
