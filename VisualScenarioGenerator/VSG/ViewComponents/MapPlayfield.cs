using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VSG.Controllers;
using VSG.Libraries;

using AME.Views.View_Components;
using AME.Controllers;
using VSG.ConfigFile;
using VSG.CoordinateTransform;
using AME.Views.View_Components.CoordinateTransform;

namespace VSG.ViewComponents
{
    public partial class MapPlayfield : UserControl, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 playfieldId = -1;
        private Guid guidMap; // Map image handle.
        private VSGController controller;
        private Boolean updatingView = false;

        public MapPlayfield()
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        public Int32 PlayfieldId
        {
            get
            {
                return playfieldId;
            }
            set
            {
                playfieldId = value;
                if (playfieldId >= 0)
                {
                    map_file.ComponentId = playfieldId;
                    utm_zone.ComponentId = playfieldId;
                    hor_mmp.ComponentId = playfieldId;
                    vert_mmp.ComponentId = playfieldId;
                    northing.ComponentId = playfieldId;
                    easting.ComponentId = playfieldId;

                    // Need to fix this when import is working. No need to open existing DB. I think we decided to only import scenarios... buut this could change.
                    UpdateViewComponent();
                    updateMap();
                }
            }
        }

        //private void reset()
        //{
        //    map_picturebox.Image = null;
        //}

        private void importMapDialog()
        {
            using (OpenImageDlg = new OpenFileDialog())
            {
                OpenImageDlg.Title = "Open Map";
                OpenImageDlg.RestoreDirectory = true;
                if (VSGConfig.MapDir != String.Empty)
                {
                    OpenImageDlg.InitialDirectory = VSGConfig.MapDir;
                }
                OpenImageDlg.Filter = "Jpeg(*.jpg)|*.jpg|GeoTiff(*.tiff)|*.tiff";
                OpenImageDlg.DefaultExt = ".jpg";

                if (OpenImageDlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (OpenImageDlg.FileName != null)
                    {
                        string fullPath = System.IO.Path.GetFullPath(OpenImageDlg.FileName);
                        string fileName = System.IO.Path.GetFileName(fullPath);
                        map_file.Text = fileName;
                        VSGConfig.MapDir = System.IO.Path.GetDirectoryName(fullPath);
                        VSGConfig.WriteFile();
                        map_file.SetParameterValue();
                        updateMap();
                    }
                    
                }
            }
            
        }

        private void buttonImportMap_Click(object sender, EventArgs e)
        {
            importMapDialog();
        }

        private void updateMap()
        {
            if (!map_file.Text.Equals(String.Empty))
            {
                try
                {
                    if (controller!= null)
                    {
                        if (!updatingView)
                        {
                            //map_picturebox.Image = ImageLibrary.OpenImageFile(map_file.Text, map_picturebox.Size);
                            controller.CurrentMapLocation = String.Format("{0}\\{1}", VSGConfig.MapDir, map_file.Text);
                            controller.CurrentMap = Image.FromFile(controller.CurrentMapLocation);
                            guidMap = controller.CurrentMap.GetType().GUID;
                            map_picturebox.Image = controller.CurrentMap;

                        }
                        else
                        {
                            if (controller.CurrentMap != null && !guidMap.Equals(controller.CurrentMap.GetType().GUID))
                            {
                                controller.CurrentMapLocation = String.Format("{0}\\{1}", VSGConfig.MapDir,map_file.Text);
                                controller.CurrentMap = Image.FromFile(controller.CurrentMapLocation);
                                guidMap = controller.CurrentMap.GetType().GUID;
                                map_picturebox.Image = controller.CurrentMap;
                            }
                        }

                        if (controller.CurrentMap != null)
                        {
                            buildTransform();
                        }
                    }
                }
                catch (System.IO.FileNotFoundException e)
                {
                    try
                    {
                        controller.CurrentMapLocation = String.Format("{0}\\{1}", VSGConfig.MapDir,map_file.Text);
                        controller.CurrentMap = Image.FromFile(controller.CurrentMapLocation);
                        guidMap = controller.CurrentMap.GetType().GUID;
                        map_picturebox.Image = controller.CurrentMap;
                        buildTransform();
                    }
                    catch (System.IO.FileNotFoundException e2)
                    {

                        string msg = String.Format("The map file: \"{0}\" could not be found.  Please navigate to it.", e.Message);
                        System.Windows.Forms.MessageBox.Show(this, msg, "Map file not found");
                        controller.CurrentMapLocation = string.Empty;

                        importMapDialog();
                    }
                }
            }
            else
            {
                // Empty... display no map.
                map_picturebox.Image = null;
            }
        }

        private void buildTransform()
        {
            // For georeferencing - set image height, width, vertical and horizontal mpp
            if (controller.CurrentMap != null && !hor_mmp.Text.Equals(String.Empty) && !vert_mmp.Text.Equals(String.Empty))
            {
                try
                {
                    float horz = float.Parse(hor_mmp.Text);
                    float vert = float.Parse(vert_mmp.Text);
                    PartialUTMTransform trans =
                        new PartialUTMTransform(controller.CurrentMap.Width, controller.CurrentMap.Height,
                        horz, vert);
                    controller.CoordinateTransform = trans;
                }
                catch (Exception e) // parse
                {
                    MessageBox.Show("Parse exception:  Meters per pixel", e.Message);
                }
            }
        }

        #region IViewComponent Members

        public AME.Controllers.IController Controller
        {
            get
            {
                return controller;
            }
            set
            {
                controller = value as VSGController;
                map_file.Controller = controller;
                utm_zone.Controller = controller;
                hor_mmp.Controller = controller;
                vert_mmp.Controller = controller;
                northing.Controller = controller;
                easting.Controller = controller;
            }
        }

        public void UpdateViewComponent()
        {
            updatingView = true;
            map_file.UpdateViewComponent();
            utm_zone.UpdateViewComponent();
            hor_mmp.UpdateViewComponent();
            vert_mmp.UpdateViewComponent();
            northing.UpdateViewComponent();
            easting.UpdateViewComponent();
            updateMap();
            updatingView = false;
        }

        #endregion
    }
}
