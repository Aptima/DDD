using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DDD_ILC
{
    class Resources
    {
        public string Path;
        public string ResourceName;
        public bool Rotatable = false;
        public Resources(string path, string display)
        {
            Path = path;
            ResourceName = display;
            Rotatable = false;
        }
        public override string ToString()
        {
            //return Path;
            return ResourceName;
        }
    }

    public enum UPDATE_TYPE : int { PROGRESS = 0, DATA = 1, CLEAR_IMAGE_TREE=2, CLEAR_MAP_TREE=3, TEXT_BOX=4 };
    struct UI_StateInfo
    {
        public UPDATE_TYPE UpdateType;
        public string Message;
        public ProgressBarStyle StyleInfo;
        public Resources CheckboxData;
    }
}
