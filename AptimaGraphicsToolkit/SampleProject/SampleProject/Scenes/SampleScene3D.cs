using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using AGT.GameFramework;

namespace SampleProject.Scenes
{
    public enum SceneMode : int { None = 0, Select = 1, Spin = 2 }

    class SampleScene3D: AGT_Scene
    {
        Mesh teapot;
        Material teapot_material = new Material();
        Material selected_material = new Material();

        Matrix teapot_matrix1 = Matrix.Identity;
        Matrix teapot_matrix2 = Matrix.Identity;
        
        Microsoft.DirectX.Direct3D.Font font;
        float camera_x = -1;
        float camera_y = -1;

        public SceneMode Mode = SceneMode.Spin;


        public  void Layer1(Microsoft.DirectX.Direct3D.Device d, float frame_rate)
        {
            
            teapot_matrix2 = Matrix.Identity * Matrix.Translation(5, 8, 30);

            d.Transform.World = teapot_matrix1;
            d.Material = teapot_material;
            
            teapot.DrawSubset(0);

            d.Transform.World = teapot_matrix2;
            teapot.DrawSubset(0);

        }

        public void Layer2(Microsoft.DirectX.Direct3D.Device d, float frame_rate)
        {

            _Line_.Width = 20f;
            _Line_.Antialias = true;


            _Line_.Begin();
            _Line_.Draw(new Vector2[] { new Vector2(0, d.Viewport.Height - (.5f * _Line_.Width)), new Vector2(d.Viewport.Width, d.Viewport.Height - (.5f * _Line_.Width)) }, Color.FromArgb(180, Color.Red));
            _Line_.End();

            font.DrawText(null, "Rotating Teapot: (Left click then drag)", 0, d.Viewport.Height - 20, Color.Lime);

        }




        public override void OnInitialize(Microsoft.DirectX.Direct3D.Device d)
        {
            ShowMouseCursor = true;
            d.RenderState.Lighting = true;

            d.RenderState.FillMode = FillMode.Solid;
            d.RenderState.ZBufferEnable = true;
            
            d.RenderState.ShadeMode = ShadeMode.Phong;
            d.RenderState.SpecularEnable = true;
            d.RenderState.AntiAliasedLineEnable = true;
            d.RenderState.MultiSampleAntiAlias = true;
            d.RenderState.Ambient = Color.White;
            

            d.Lights[0].Diffuse = Color.White;
            d.Lights[0].Type = LightType.Directional;
            d.Lights[0].Falloff = 132f;
            d.Lights[0].InnerConeAngle = 10;
            d.Lights[0].OuterConeAngle = 20;
            d.Lights[0].Specular = Color.White;
            d.Lights[0].Position = new Vector3(0, 0, -100);
            d.Lights[0].Direction = new Vector3(0, 0, 1);

            d.Lights[0].Update();
            d.Lights[0].Enabled = true;

            _Projection_ = Matrix.PerspectiveFovLH(.8f, (float)d.Viewport.Width / (float)d.Viewport.Height, 1.0f, 100.0f);
            _View_ = Matrix.LookAtLH(new Vector3(0, 0, -4f), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            
            selected_material.Diffuse = Color.Yellow;
            selected_material.Specular = Color.White;
            selected_material.SpecularSharpness = 32f;

            teapot_material.Diffuse = Color.DodgerBlue;
            teapot_material.Specular = Color.White;
            teapot_material.SpecularSharpness = 32f;

            font = new Microsoft.DirectX.Direct3D.Font(d, new System.Drawing.Font("Arial", 12));


            teapot = Mesh.Teapot(d);
            _Line_ = new Microsoft.DirectX.Direct3D.Line(d);

            RenderLayers.Add(new RenderableLayer(this.Layer1));
            RenderLayers.Add(new RenderableLayer(this.Layer2));


            /* Remember to set the appropriate State, or risk being stuck in INIT
             *  forever.  To begin rendering set the scene state to RENDER.
             * */
            State = SceneState.RENDER;
        }

        public override void OnReInitialize(Device d)
        {
            OnInitialize(d);
        }


        public override void OnMouseClick(object sender, MouseEventArgs e)
        {
            Vector3 near = new Vector3(e.X, e.Y, 0);
            Vector3 far = new Vector3(e.X, e.Y, 1);            
        }

        public override void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                camera_y = (float)e.Y ;
                camera_x = (float)e.X;
            }
        }
        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && TargetControlRect.Contains(e.X, e.Y))
            {
                float scale = AGT_Scene.ToRadians(.5f);
                teapot_matrix1 *= (Matrix.RotationY(scale * (camera_x - e.X)) * Matrix.RotationX(scale * (camera_y - e.Y)));
                camera_y = e.Y;
                camera_x = e.X;
            }
        }
    
    }
}
