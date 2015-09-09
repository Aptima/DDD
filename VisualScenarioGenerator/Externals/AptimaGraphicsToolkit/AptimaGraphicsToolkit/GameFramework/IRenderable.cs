using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace AGT.GameFramework
{
    interface IRenderable
    {
        void OnInitialize(Device d);
        void OnReInitialize(Device d);
        void Render(Device d);
    }
}
