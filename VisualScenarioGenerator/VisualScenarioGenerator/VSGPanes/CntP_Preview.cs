using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VisualScenarioGenerator.Dialogs;
using AGT.Forms;
using AGT.Scenes;


namespace VisualScenarioGenerator.VSGPanes
{
    public partial class CntP_Preview : Ctl_ContentPane
    {
        private DDD_Playfield playfield;
        private bool FrameworkStarted = false;

        public CntP_Preview()
        {
            InitializeComponent();
        }

        private void CntP_Preview_Load(object sender, EventArgs e)
        {
            playfield = new DDD_Playfield();
            playfield.UnitTestMode = true;
            playfield.ImageLibraryPath = string.Format("{0}\\Resources\\{1}", System.Environment.CurrentDirectory, "ImageLib.dll");
            playfield.MapFile = "Resources\\Ramadi.jpg";

            agT_CanvasControl1.AddScene(playfield);
        }


        public void ShowPlayfield(bool Visible)
        {
            if (Visible)
            {
                if (FrameworkStarted)
                {
                    agT_CanvasControl1.ResumeFramework();
                }
                else
                {

                    agT_CanvasControl1.StartFramework();
                    FrameworkStarted = true;
                }
            }
            else
            {
                if (FrameworkStarted)
                {
                    if (!agT_CanvasControl1.Suspended)
                    {
                        agT_CanvasControl1.SuspendFramework();
                    }
                }
            }

        }
    }
}
