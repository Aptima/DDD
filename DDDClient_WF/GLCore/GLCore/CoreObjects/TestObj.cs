using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects
{
    public class TestObj:BaseGameObject
    {
        private CustomVertex.PositionColored[] vertices;
        
        public TestObj()
        {
        }


        public override void Initialize(Canvas canvas)
        {
            vertices = new CustomVertex.PositionColored[3];

            //vertices[0].Position = new Vector4(150f, 100f, 0f, 1f);
            vertices[0].Position = new Vector3(0, 0, 0f);
            vertices[0].Color = Color.Red.ToArgb();

            //vertices[1].Position = new Vector4(this.Width / 2 + 100f, 100f, 0f, 1f);
            vertices[1].Position = new Vector3(30, 0, 0f);
            vertices[1].Color = Color.Green.ToArgb();

            //vertices[2].Position = new Vector4(250f, 300f, 0f, 1f);
            vertices[2].Position = new Vector3(15, 30, 0f);
            vertices[2].Color = Color.Yellow.ToArgb();
        }


        public override void Texture(Canvas canvas)
        {
            //canvas.SetTexture(0, null);               
        }
        public override void Draw(Canvas canvas)
        {
            canvas.DrawUserPrimitives(PrimitiveType.TriangleList, 1, CustomVertex.PositionColored.Format, this.vertices);
        }

    }
}
