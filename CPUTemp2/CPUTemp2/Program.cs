﻿using System;
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
            string query = "INSERT INTO CPU ('temperature') VALUES (@temperature)";
            SQLiteCommand command = new SQLiteCommand(query, databaseObject.connection);
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
                            command.Parameters.AddWithValue("@temperature", cpuTemp);
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

            //
           
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
