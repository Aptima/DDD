using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;

using System.Text;
using System.Windows.Forms;

using AGT.Sprites;
using AGT.Forms;
using AGT.GameToolkit;
using AGT.Scenes;

using AGT.GameFramework;

namespace SampleProject
{
    public partial class Form1 : Form
    {
        private DDD_Playfield scene_2D = null;

        public Form1()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            InitializeComponent();

            scene_2D = new DDD_Playfield();
            scene_2D.UnitTestMode = true;
            scene_2D.ImageLibraryPath = string.Format("{0}\\Resources\\{1}", System.Environment.CurrentDirectory, "ImageLib.dll");
            scene_2D.MapFile = "Resources\\Ramadi.jpg";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mdX_CanvasControl1.OnGameOver = new GameOverHandler(this.GameOver);
            mdX_CanvasControl1.AddScene(scene_2D);
            mdX_CanvasControl1.StartFramework();
        }

        public void GameOver()
        {
            MessageBox.Show("Game Over");
            Application.Exit();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (mdX_CanvasControl1.CurrentScene is DDD_Playfield)
            {
                DDD_Playfield Scene = ((DDD_Playfield)mdX_CanvasControl1.CurrentScene);
                if (Scene.PlayfieldScale <= .75f)
                {
                    Scene.PlayfieldScale += .25f;
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DDD_Playfield Scene = ((DDD_Playfield)mdX_CanvasControl1.CurrentScene);
            if (Scene.PlayfieldScale > .25f)
            {
                Scene.PlayfieldScale -= .25f;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (mdX_CanvasControl1.CurrentScene is DDD_Playfield)
            {
                if (((DDD_Playfield)mdX_CanvasControl1.CurrentScene).Style == HeadingStyle.Aptima)
                {
                    ((DDD_Playfield)mdX_CanvasControl1.CurrentScene).Style = HeadingStyle.MilStd;
                }
                else
                {
                    ((DDD_Playfield)mdX_CanvasControl1.CurrentScene).Style = HeadingStyle.Aptima;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (mdX_CanvasControl1.CurrentScene is DDD_Playfield)
            {
                ((DDD_Playfield)mdX_CanvasControl1.CurrentScene).ChangeMode(PlayfieldModeType.Waypoint);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (mdX_CanvasControl1.CurrentScene is DDD_Playfield)
            {
                ((DDD_Playfield)mdX_CanvasControl1.CurrentScene).ChangeMode(PlayfieldModeType.Zone);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            /* SceneState.END, Gracefully quits the current scene and loads the next one in the queue.
             * */
            mdX_CanvasControl1.CurrentScene.State = SceneState.END;
        }



    }
}