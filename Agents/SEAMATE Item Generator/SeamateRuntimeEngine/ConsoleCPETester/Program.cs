using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PME_DDD_CPE_Publisher;

namespace ConsoleCPETester
{
    class Program
    {
        static void Main(string[] args)
        {
            String dmId = "BAMS DM";
            double ffCpe = 0.25;
            double ttCpe = -0.25;

            Console.WriteLine("Press enter to connect");
            Console.ReadLine();
            bool res = CPEPublisher.Connect("adziki", 8888);
            if (res == false)
            {
                Console.WriteLine("Failed to connect");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Press enter to send test CPE event");
            Console.ReadLine();
            CPEPublisher.PublishCPE(dmId, ffCpe, ttCpe);
            Console.WriteLine("Event sent with FF_CPE="+ffCpe+", TT_CPE="+ttCpe+".");
            Console.WriteLine("Press 'R' to send another event, any other key to disconnect");
            ConsoleKeyInfo ki = Console.ReadKey();
            while (ki.Key == ConsoleKey.R)
            {
                ffCpe = GenerateRandomDifficulty();
                ttCpe = GenerateRandomDifficulty();
                CPEPublisher.PublishCPE(dmId, ffCpe, ttCpe);
                Console.WriteLine("Event sent with FF_CPE=" + ffCpe + ", TT_CPE=" + ttCpe + ".");
                Console.WriteLine("Press 'R' to send another event, any other key to disconnect");
                ki = Console.ReadKey();
            }
            CPEPublisher.Disconnect();
        }
        private static double GenerateRandomDifficulty()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            return r.NextDouble() * 4 - 2; //from 2.0 to -2.0
        }
    }
}
