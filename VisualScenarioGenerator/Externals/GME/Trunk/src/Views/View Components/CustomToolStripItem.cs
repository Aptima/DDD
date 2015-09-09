using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace AME.Views.View_Components
{
    // A custom toolstrip item that we use to populate toolstrips.  This
    // object stores a string and an int which refer the component name
    // and its id

    public class CustomToolStripItem : ToolStripButton
    {
        private String myName;
        public String MyName
        {
            get { return myName; }
            set { myName = value; }
        }

        private int myID;
        public int MyID
        {
            get { return myID; }
            set { myID = value; }
        }

        public CustomToolStripItem(String comboName, int comboID)
            : base(comboName)

        {
            myName = comboName;
            myID = comboID;
        }

        public override String ToString()
        {
            return myName;
        }
    }
}
