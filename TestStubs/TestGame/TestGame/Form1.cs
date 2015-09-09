using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using GameLib;

namespace TestGame
{
    public partial class Form1 : Form, IGameControl
    {
        Scene[] levels;
        Vector3 mouse_offset = Vector3.Empty;
        Vector3 dist_vector = Vector3.Empty;
        


        public Form1()
        {
            InitializeComponent();
            MouseWheel += new MouseEventHandler(Form1_MouseWheel);
        }

        void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            Scene s = GameFramework.GetCurrentScene();
            if (e.Delta < 0)
            {
                s.Zoom(false);
            }
            else
            {
                s.Zoom(true);
            }

        }




       #region IGameControl Members

        public Control GetTargetControl()
        {
            return this;
        }

       public void GameOver(ICanvas canvas)
       {
           MessageBox.Show("Game Over.");
       }
        public void InitializeCanvasOptions(CanvasOptions options)
        {
            options.Windowed = true;
            options.Shader = ShadeMode.Gouraud;
            options.Device = DeviceType.Hardware;
            options.BackgroundColor = Color.Black;
            options.AmbientColor = Color.White;
            options.BackfaceCulling = Cull.None;
        }

 
       public Scene[] InitializeScenes(ICanvas canvas)
       {
           //levels = new Scene[] { new MainScene(), new Scene1() };
           levels = new Scene[] { new GIS_Scene("test.jpg", 8.49990671075954650f, 8.56718696599567760f), new Scene1() };
           return levels;
           //return new IScene[] { new MainScene() };

       }


       #endregion

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape)
            //{
            //    GameFramework.NextScene();
            //}
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //if ((e.Button == MouseButtons.Middle) || (e.Button == MouseButtons.Left))
            //{
            //    mouse_offset = new Vector3(e.X, e.Y, 0);
            //    if (e.Button == MouseButtons.Left)
            //    {
            //        GIS_Scene s = (GIS_Scene)GameFramework.GetCurrentScene();
            //        s.drawline = true;
            //        s.lineEnd = s.lineStart = new Vector2(e.X, e.Y);

            //    }
            //}
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            //switch (e.Button)
            //{
            //    case MouseButtons.Middle:
            //        mouse_offset = Vector3.Empty;
            //        break;
            //    case MouseButtons.Left:
            //        Scene s = GameFramework.GetCurrentScene();
            //        ((GIS_Scene)s).drawline = false;
            //        mouse_offset = Vector3.Empty;
            //        this.toolTip1.Hide((IWin32Window)this);

            //        break;
            //}
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //switch (e.Button)
            //{
            //    case MouseButtons.Middle:
            //        mouse_offset.Subtract(new Vector3(e.X, e.Y, 0));
            //        mouse_offset.Multiply(-1);
            //        Scene sm = GameFramework.GetCurrentScene();
            //        sm.Pan(mouse_offset);
            //        mouse_offset = new Vector3(e.X, e.Y, 0);
            //        break;
            //    case MouseButtons.Left:
            //        dist_vector = mouse_offset;
            //        dist_vector.Subtract(new Vector3(e.X, e.Y, 0));
            //        GIS_Scene sl = (GIS_Scene)GameFramework.GetCurrentScene();
            //        sl.lineEnd = new Vector2(e.X, e.Y);
            //        this.toolTip1.Show(sl.MouseDistance(dist_vector) + "m", (IWin32Window)this, 6 + e.X, 6 + e.Y);
            //        break;
            //}
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            //GIS_Scene s = (GIS_Scene)GameFramework.GetCurrentScene();
            //Vector2 screen_coordinates = new Vector2(e.X, e.Y);
            //if (!s.Select(screen_coordinates))
            //{
            //    s.MoveSelected(screen_coordinates);
            //}
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Scene s = GameFramework.GetCurrentScene();
            ((GIS_Scene)s).Exiting();
        }



        
     }
}