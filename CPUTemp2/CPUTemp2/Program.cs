using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;
using System.Data.SQLite;
using MySql.Data.MySqlClient;

namespace CPUTemp2
{
    class Program
    {
        static float cpuTemp;

        // Instantiate the computer object to expose hardware information
        static Computer c = new Computer()
        {
            CPUEnabled = true
        };
        
        public class UpdateVisitor : IVisitor
        {
            public void VisitComputer(IComputer c)
            {
                c.Traverse(this);
            }
            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
            }
            public void VisitSensor(ISensor sensor) { }
            public void VisitParameter(IParameter parameter) { }
        }

        static void ReportSystemInfo()
        {
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Database databaseObject = new Database();
            databaseObject.OpenConnection();
            c.Accept(updateVisitor);

            for (float i = 0; i < c.Hardware.Length; i++)
            {
                if (c.Hardware[Convert.ToInt32(i)].HardwareType == HardwareType.CPU)
                {
                    for (float j = 0; j < c.Hardware[Convert.ToInt32(i)].Sensors.Length; j++)
                    {
                        if (c.Hardware[Convert.ToInt32(i)].Sensors[Convert.ToInt32(j)].SensorType == SensorType.Temperature)
                        {
                            cpuTemp = c.Hardware[Convert.ToInt32(i)].Sensors[Convert.ToInt32(j)].Value.GetValueOrDefault();
                            System.Diagnostics.Debug.WriteLine(cpuTemp);

                            // Timestamps
                            string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            Int32 unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                            // User
                            string u = System.Environment.UserName;
                            string m = System.Environment.MachineName;

                            MySqlCommand command = new MySqlCommand();
                            command.Connection = databaseObject.connection;
                            command.CommandText = "INSERT INTO CPU (temperature,time,user,unixtime) VALUES(@temperature,@time,@user,@unixtime)";
                            command.Parameters.AddWithValue("@temperature", cpuTemp);
                            command.Parameters.AddWithValue("@time", dt);
                            command.Parameters.AddWithValue("@unixtime", unixTimestamp);
                            command.Parameters.AddWithValue("@user", u + "_" + m);

                            var result = command.ExecuteNonQuery();
                            Console.WriteLine("Rows added : {0}", result);
                        }
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
