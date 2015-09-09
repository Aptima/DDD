using System;
using System.Collections.Generic;
using System.Text;

namespace AGT.Forms
{
    public interface IAGT_SplashDialog
    {
        void DialogInitialize(AGT_SplashDialog dialog_instance);
        void LoadResources(AGT_SplashDialog dialog_instance, Microsoft.DirectX.Direct3D.Device device);
    }
}
