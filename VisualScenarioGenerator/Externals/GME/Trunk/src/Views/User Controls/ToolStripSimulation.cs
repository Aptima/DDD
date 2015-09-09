using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GME.Controllers;
using System.Windows.Forms.Design;

namespace User_Controls
{
    //[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripSimulation : ToolStripControlHost
    {
        //public ToolStripSimulation(GME cm) : base(new Simulation(cm)) { }
        public ToolStripSimulation() : base(new Simulation()) { }

        public Simulation SimulationControl
        {
            get
            {
                return Control as Simulation;
            }
        }
    }
}
