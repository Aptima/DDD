using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Aptima.CommonComponent
{
    public class ConsoleKeyGenerator
    {
        public static void Main(string[] args)
        {
            string key = CommandLineKeyGen.GenerateKey(args);
            if (key.Contains("ERROR:"))
            {
                Console.Out.WriteLine(key);
            }
            else
            {
                Console.Out.Write(key.Substring(0,8));
                Console.Out.Write("  ");
                Console.Out.Write(key.Substring(8,8));
                Console.Out.Write("  ");
                Console.Out.Write(key.Substring(16,8));
                Console.Out.Write("  ");
                Console.Out.Write(key.Substring(24,8));
            }

        }
    }
}
