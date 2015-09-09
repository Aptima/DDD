using System;
using System.Collections.Generic;
using System.Text;

namespace AGT.Forms
{
    public interface IAGT_SceneLoadDialog
    {
        void DialogInitialize(AGT_SceneLoadDialog dialog_instance);
        void LoadResources(AGT_SceneLoadDialog dialog_instance, Microsoft.DirectX.Direct3D.Device device);
    }
}
