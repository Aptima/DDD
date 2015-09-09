using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace AGT.GameToolkit
{
    public abstract class AGT_RenderableElement
    {
        public float X_Rotation;
        public float Y_Rotation;
        public float Z_Rotation;

        public float X_Center;
        public float Y_Center;
        public float Z_Center;

        public float X_Position;
        public float Y_Position;
        public float Z_Position;

        public abstract void Initialize(Device d);
        public abstract void Draw(Device d);
    }
}
