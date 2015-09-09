using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GameLib;

namespace TestGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GameFramework.Initialize();
            Form1 form = new Form1();
            Console.WriteLine("Before everything");
            GameFramework.RegisterGameControl(form, form);
            Console.WriteLine("Got here");
            Application.Run(form);
        }
    }
}