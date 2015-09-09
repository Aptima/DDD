using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

using AGT.Sprites;
using AGT.Forms;
using AGT.GameFramework;



namespace SampleProject.Scenes
{
    class SampleScene2D: AGT_Scene, IAGT_SplashDialog
    {
        public AGT_SpriteManager sprites = null;
        private AGT_SpriteId active_zone;
        
        public SampleScene2D()
        {
            ShowSplashScreen = true;
        }




        public override void OnInitialize(Microsoft.DirectX.Direct3D.Device d)
        {
            RenderLayers.Add(new RenderableLayer(this.Layer1));
            State = SceneState.RENDER;
        }

        public override void OnReInitialize(Device d)
        {
            State = SceneState.RENDER;
        }

        public void ChangeScale()
        {
            if (sprites != null)
            {
                sprites.SetTextureScale(active_zone, 2f, .5f, 0f);
            }
        }


        private void Layer1(Microsoft.DirectX.Direct3D.Device d, float fps)
        {
            sprites.BatchDraw(SpriteFlags.AlphaBlend);
        }

        #region IAGT_SplashDialog Members
        public void DialogInitialize(AGT_SplashDialog dialog_instance)
        {
            //dialog_instance.BackgroundImageLayout = ImageLayout.Stretch;
            //dialog_instance.BackgroundImage = new Bitmap(@"C:\Documents and Settings\ebonomolo\My Documents\My Pictures\28413-1.jpg");
        }

        public void LoadResources(AGT_SplashDialog dialog_instance, Device device)
        {

            dialog_instance.UpdateStatusBar("Initializing Sprite Manager", 0, 3);

            sprites = new AGT_SpriteManager(device);

            dialog_instance.UpdateStatusBar("Loading Ramadi Map", 1, 3);
                sprites.AddResource("Ramadi", "Resources\\Map2.jpg", 0, 0, 0, false);

            dialog_instance.UpdateStatusBar("Loading Active Zones", 2, 3);
            using (Bitmap b = AGT_GDIBridge.CreateCircle(device, 200, new HatchBrush(HatchStyle.DashedDownwardDiagonal, Color.FromArgb(180, Color.Black), Color.FromArgb(180, Color.White))))
            {
                active_zone = sprites.AddResource("ActiveZone", b, 100, 100, 0);
            }

            dialog_instance.UpdateStatusBar("Finished", 3, 3);
        }

        #endregion

    }
}
