using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace CPUTemp2
{
    public class Database
    {
        public SQLiteConnection connection;
        public Database()
        {
            connection = new SQLiteConnection("Data Source=Datacase.db");

            if (!File.Exists("./Datacase.db"))
            {
                SQLiteConnection.CreateFile("Datacase.db");
                Console.WriteLine("Database file created");
            }
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
