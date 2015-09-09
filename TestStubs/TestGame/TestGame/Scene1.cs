using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing;
using GameLib;
using GameLib.GameObjects;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace TestGame
{
    class Scene1: Scene
    {
        Obj_Plane plane;

        public Scene1()
        {
        }


        public override void InitializeSceneProjection(ICanvas canvas)
        {
            CreatePerspectiveProjection((float)(Math.PI / 4), canvas.GetAspectRatio(), 0, 100);
        }
        
        public override void InitializeCameraView()
        {
            this.ViewCamera.Position = new Vector3(0, 0, -3);
            
        }


        public override void InitializeScene(ICanvas canvas)
        {

            Assembly ass = Assembly.LoadFrom("./GameLib.dll");
            Stream s = ass.GetManifestResourceStream("GameLib.images.AptimaLogo.jpg");
            
            plane = new Obj_Plane(canvas.CreateTexture(s), Color.LightGray);
            plane.Initialize(canvas);

            plane.Scaling = new Vector3(.8f, 1, 1);
            plane.Position = new Vector3(plane.Width * -.5f, plane.Height * -.5f, 0);

            plane.Texture(canvas);
            s.Close();
        }

 

        public override void RenderScene(ICanvas canvas)
        {
            plane.Rotation = new Vector3(0, System.Environment.TickCount * .001f, 0);
            canvas.SetWorldMatrix(plane.WorldMtx);
            plane.Draw(canvas);
        }

        public override void Pan(Vector3 offset)
        {
            
        }
        public override void Zoom(bool zoom_in)
        {
           
        }
    }
}
