using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

using System.Resources;
using System.Reflection;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using AGT;
using AGT.Forms;


namespace AGT.Sprites
{
    public static class AGT_ImageLibrary
    {
        public static AGT_SpriteManager Load(string library_path,  System.Windows.Forms.Control dialog, Microsoft.DirectX.Direct3D.Device VideoDevice)
        {
            try
            {
                List<string> names = new List<string>();
                AGT_SpriteManager sprite_manager = new AGT_SpriteManager(VideoDevice);

                Assembly image_lib = Assembly.LoadFile(library_path);

                StreamReader s = new StreamReader(image_lib.GetManifestResourceStream("ImageLibrary.mf"));

                while (!s.EndOfStream)
                {
                    names.Add(s.ReadLine());
                }
                s.Close();

                for (int i = 0; i < names.Count; i++)
                {
                    string[] texture_name = names[i].Split(':');
                    if (dialog is AGT_SplashDialog)
                    {
                        ((AGT_SplashDialog)dialog).UpdateStatusBar(string.Format("Loading {0} ...", texture_name[0]), i + 1, names.Count);
                    }
                    if (dialog is AGT_SceneLoadDialog)
                    {
                        ((AGT_SceneLoadDialog)dialog).UpdateStatusBar(string.Format("Loading {0} ...", texture_name[0]), i + 1, names.Count);
                    }
                    using (Bitmap b = new Bitmap(image_lib.GetManifestResourceStream(texture_name[0])))
                    {
                        AGT_SpriteId id = sprite_manager.AddResource(texture_name[0], b, 0, 0, 0, bool.Parse(texture_name[1]));
                        sprite_manager.SetCenter(id, (float)(b.Width * .5f), (float)(b.Height * .5f), 0f);
                    }
                }

                return sprite_manager;

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.StackTrace, e.Message);
            }
            return null;

        }
        
        public static AGT_SpriteManager Load(string library_path, List<string> images, System.Windows.Forms.Control dialog, Microsoft.DirectX.Direct3D.Device VideoDevice)
        {
            try
            {
                List<string> names = new List<string>();
                AGT_SpriteManager sprite_manager = new AGT_SpriteManager(VideoDevice);

                Assembly image_lib = Assembly.LoadFile(library_path);

                StreamReader s = new StreamReader(image_lib.GetManifestResourceStream("ImageLibrary.mf"));

                while (!s.EndOfStream)
                {
                    names.Add(s.ReadLine());
                }
                s.Close();

                for (int i = 0; i < names.Count; i++)
                {
                    if (images.Contains(names[i]))
                    {
                        string[] texture_name = names[i].Split(':');
                        if (dialog is AGT_SceneLoadDialog)
                        {
                            ((AGT_SceneLoadDialog)dialog).UpdateStatusBar(string.Format("Loading {0} ...", texture_name[0]), i + 1, names.Count);
                        }
                        if (dialog is AGT_SplashDialog)
                        {
                            ((AGT_SplashDialog)dialog).UpdateStatusBar(string.Format("Loading {0} ...", texture_name[0]), i + 1, names.Count);
                        }
                        using (Bitmap b = new Bitmap(image_lib.GetManifestResourceStream(texture_name[0])))
                        {
                            AGT_SpriteId id = sprite_manager.AddResource(texture_name[0], b, 0, 0, 0, bool.Parse(texture_name[1]));
                            sprite_manager.SetCenter(id, (float)(b.Width * .5f), (float)(b.Height * .5f), 0f);
                        }
                    }
                }

                return sprite_manager;

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.StackTrace, e.Message);
            }
            return null;

        }



    }
}
