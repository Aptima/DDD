using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Aptima.Visualization
{
    /// <summary>
    /// Abstract visualization class.
    /// Adds required generation method for setting up visualization display
    /// and helper method to return unique name for visualization (not sure if that will be useful).
    /// </summary>
    public abstract class Visualization : Canvas
    {
        private static double maximumHeight = 400.0;
        private static double maximumWidth = 400.0;

        abstract public string getName();

        public abstract void generate();

        public Visualization()
        {
        }
    }
}
