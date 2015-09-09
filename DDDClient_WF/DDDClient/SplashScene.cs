using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.Client.Common.GLCore;
using Aptima.Asim.DDD.Client.Common.GLCore.Controls;
using Aptima.Asim.DDD.Client.Common.GLCore.CoreObjects;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Threading;

namespace Aptima.Asim.DDD.Client
{
    class SplashScene: WindowManager
    {
        private Microsoft.DirectX.Direct3D.Font font_large;
        private Microsoft.DirectX.Direct3D.Font font_small;
        private PanelMenu menu;
        private string Item = "Nothing";
        
        public SplashScene(IGameControl control):base(control)
        {
        }

        #region IScene Members

        public override void OnInitializeScene(GameFramework g)
        {
            BindGameController();

            font_small = g.CANVAS.CreateFont(new System.Drawing.Font("Arial", 12));
            font_large = g.CANVAS.CreateFont(new System.Drawing.Font("Arial", 28));
            menu = new PanelMenu(font_small, new PanelMenuSelectHandler(MenuHandler));
            menu.ForegroundColor = Color.White;
            menu.BackgroundColor = Color.Black;
            menu.BorderColor = Color.White;

            //menu.ScrollButtonWidth = 20;
            menu.SetHeight(100);
            menu.SetPosition(200, 200);
            menu.SetWidth(100);
            //menu.DrawFormat = DrawTextFormat.None;

            int height = menu.LayoutMenuOptions(new string[] {
                "item1",
                "item2",
                "item3",
                "item4",
                "item5",
                 "item6",
                "item7",
                "item8 ",
                "item9 ",
                "item10"
            }, PanelLayout.HorizontalFree);
 


        }

        public void MenuHandler(int selection, string item_str)
        {
            Item = item_str;

        }

        public override void OnSceneLoading(GameFramework g)
        {
            SceneMode = MODE.SCENE_RENDER;
        }




        public override void OnRender(Canvas canvas)
        {
            font_small.DrawText(null, "Selected Item: " + Item, 300, 10, Color.Yellow);
            menu.OnRender(canvas);
            
            //base.OnRender(canvas);
        }

        //public override void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        //{
        //    Console.WriteLine("Got KeyDown");
        //    switch (e.KeyCode)
        //    {
        //        case System.Windows.Forms.Keys.Down:
        //            menu.ScrollDown(5);
        //            break;
        //        case System.Windows.Forms.Keys.Up:
        //            menu.ScrollUp(5);
        //            break;
        //    }
        //}
        public override void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (menu.ClientArea.Contains(e.Location))
            {
                menu.OnMouseDown(sender, e);
                return;
            }
            base.OnMouseDown(sender, e);
        }
        public override void OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (menu.ClientArea.Contains(e.Location))
            {
                menu.OnMouseUp(sender, e);
            }
            else
            {
                base.OnMouseUp(sender, e);
            }
        }

        #endregion
    }
}
