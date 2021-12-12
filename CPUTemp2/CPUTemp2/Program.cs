using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;
using System.Data.SQLite;

namespace CPUTemp2
{
    class Program
    {
        static float cpuTemp;


        // Instantiate the computer object to expose hardware (CPU) information
        static Computer c = new Computer()
        {
            CPUEnabled = true
        };

        
        static void ReportSystemInfo()
        {
            Database databaseObject = new Database();
            string query = "INSERT INTO CPU ('temperature') VALUES (@temperature)";
            SQLiteCommand command = new SQLiteCommand(query, databaseObject.connection);
            databaseObject.OpenConnection();

            foreach (var hardware in c.Hardware)
            {
                if (hardware.HardwareType == HardwareType.CPU)
                {
                    //
                    hardware.Update();
                    foreach (var sensor in hardware.Sensors)
                    {
                        // store
                        cpuTemp = sensor.Value.GetValueOrDefault();
                        System.Diagnostics.Debug.WriteLine(cpuTemp);
                        command.Parameters.AddWithValue("@temperature", cpuTemp);
                        var result = command.ExecuteNonQuery();
                        
                        Console.WriteLine("Rows added : {0}", result);
                    }
                }
                else
                {
                    databaseObject.CloseConnection();
                }
            }
        }

        static void Main(string[] args)
        {
            c.Open();

            //loop to report exposed information
            while (true)
            {
                ReportSystemInfo();
            }
        }
    }
}
