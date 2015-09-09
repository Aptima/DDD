using System;
using System.Collections.Generic;
using System.Text;

namespace View_Components
{
    // A custom combo item that we use to populate custom combo boxes.  This
    // object stores a string and an int which refer to a project/mission/org name
    // and the component id that name maps to.

    public class ComboItem
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

        public ComboItem(String comboName, int comboID)
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
