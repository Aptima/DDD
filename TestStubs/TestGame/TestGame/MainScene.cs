using System;
using System.Collections.Generic;
using System.Text;
using GameLib;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace TestGame
{
    class MainScene: Scene
    {
        Mesh teapot;
        Material material;
        float zpos = .000001f;

        public MainScene()
        {
            teapot = null;
            material = new Material();
            material.Diffuse = Color.Yellow;
            
        }

        #region IScene Members

        public override void InitializeSceneProjection(ICanvas canvas)
        {
            CreatePerspectiveProjection((float)(Math.PI/4), canvas.GetAspectRatio(), 0, 100);
        }
        public override void InitializeCameraView()
        {
            this.ViewCamera.Position = new Vector3(0, 0, -10);
        }
        public override void InitializeScene(ICanvas canvas)
        {
            canvas.AddLight(LightType.Point, new Vector3(0, 10, 0), Vector3.Empty, 100, 10, Color.White);
            teapot = canvas.CreateTeapot();
        }


        public override void RenderScene(ICanvas canvas)
        {
            canvas.SetWorldMatrix(Matrix.Identity);
            canvas.SetMaterial(material);
            teapot.DrawSubset(0);

            this.ViewCamera.ChangePosition(0, 0, zpos);
            zpos += .000001f;
        }

        public override void Pan(Vector3 offset)
        {
            return;
        }

        public override void Zoom(bool zoom_in)
        {
            return;
        }

        #endregion
    }
}
