using System;
using System.Collections.Generic;
using System.Text;

namespace VSG.PreviewSimulator
{
    public abstract class AbstractPreviewEvent
    {
        public int Time = 0;
        public string ID = string.Empty;
        public string Name = string.Empty;
        public float X = 0;
        public float Y = 0;
        public float Z = 0;
    }
}
