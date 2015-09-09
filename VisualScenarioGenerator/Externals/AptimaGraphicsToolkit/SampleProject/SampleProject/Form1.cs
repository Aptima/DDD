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
using SampleProject.Scenes;

using AGT.GameFramework;

namespace SampleProject
{
    public partial class Form1 : Form
    {
        private int index = 0;
        public Form1()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mdX_CanvasControl1.OnGameOver = new GameOverHandler(this.GameOver);

            mdX_CanvasControl1.AddScene(new SampleScene3D());
            mdX_CanvasControl1.AddScene(new SampleScene2D());
            mdX_CanvasControl1.StartFramework();
        }

        public void GameOver()
        {
            MessageBox.Show("Game Over");
            Application.Exit();
        }




        private void NextSceneClick(object sender, EventArgs e)
        {
            /* SceneState.END, Gracefully quits the current scene and loads the next one in the queue.
             * */
            //mdX_CanvasControl1.CurrentScene.State = SceneState.END;
            switch (index)
            {
                case 0:
                    index = 1;
                    break;
                case 1:
                    index = 0;
                    break;
            }
            mdX_CanvasControl1.GetScene(index);
        }



    }
}