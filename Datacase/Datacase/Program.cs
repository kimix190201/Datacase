using System;

namespace Datacase
{
    class Program
    {
        
        
        static void Main(string[] args)
        {
            // CPU check
            string CPU = System.Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER");
            Console.WriteLine(CPU);

            // 32bit/64bit



            Console.WriteLine("-- Operating System Detaiils --");

            OperatingSystem os = Environment.OSVersion;

            Console.WriteLine("OS Version: " + os.Version.ToString());

            Console.WriteLine("OS Platoform: " + os.Platform.ToString());

            Console.WriteLine("OS SP: " + os.ServicePack.ToString());

            Console.WriteLine("OS Version String: " + os.VersionString.ToString());

            Console.WriteLine();

            Version ver = os.Version;

            Console.WriteLine("Major version: " + ver.Major);

            Console.WriteLine("Major Revision: " + ver.MajorRevision);

            Console.WriteLine("Minor version: " + ver.Minor);

            Console.WriteLine("Minor Revision: " + ver.MinorRevision);

            Console.WriteLine("Build: " + ver.Build);

            Console.WriteLine("----");

            bool is64 = System.Environment.Is64BitOperatingSystem;
            if (is64 == true) {
                Console.WriteLine("64bit");
            } else
            {
                Console.WriteLine("32bit");
            }
            
        }


    }
}
