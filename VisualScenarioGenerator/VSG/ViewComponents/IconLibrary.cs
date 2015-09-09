using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VSG.Controllers;
using VSG.Libraries;
using VSG.Dialogs;
using VSG.ConfigFile;

using AME.Views.View_Components;
using AME.Controllers;

namespace VSG.ViewComponents
{
    public partial class IconLibrary : UserControl, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private Int32 playfieldId = -1;
        private VSGController controller;

        public IconLibrary()
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
                    icon_lib.ComponentId = playfieldId;

                    // Need to fix this when import is working. No need to open existing DB. I think we decided to only import scenarios... buut this could change.
                    UpdateViewComponent();
                    updateIcons();
                }
            }
        }

        private void updateIcons()
        {
            if (!icon_lib.Text.Equals(String.Empty))
            {
                try
                {
                    controller.CurrentIconLibraryLocation = String.Format("{0}\\{1}.dll", VSGConfig.IconDir, icon_lib.Text);                  
                    controller.CurrentIconLibrary = ImageLibrary.OpenLibrary(System.Reflection.Assembly.LoadFile(icon_lib.Text));

                    iconListView1.UpdateViewComponent();
                }
                catch (Exception)
                {
                    try
                    {
                        controller.CurrentIconLibraryLocation = String.Format("{0}\\{1}.dll", VSGConfig.IconDir, icon_lib.Text);
                        controller.CurrentIconLibrary = ImageLibrary.OpenLibrary(System.Reflection.Assembly.LoadFile(controller.CurrentIconLibraryLocation));

                        iconListView1.UpdateViewComponent();
                    }
                    catch (Exception)
                    {
                        String msg = String.Format("The icon library: \"{0}.dll\" could not be found.  Please navigate to it.", icon_lib.Text);
                        System.Windows.Forms.MessageBox.Show(this, msg, "Icon Library not found");
                        openIconLibDialog();
                    }

                    

                }
            }
        }



        private void buttonNewIconLib_Click(object sender, EventArgs e)
        {
            if (iconListView1.LargeImageList == null)
            {
                using (CreateLibraryDialog dlg = new CreateLibraryDialog())
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        if (dlg.LibraryName != null)
                        {
                            string fullPath = System.IO.Path.GetFullPath(dlg.LibraryName);
                            string fileName = System.IO.Path.GetFileName(fullPath);
                            fileName = fileName.Replace(".dll", "");
                            fileName = fileName.Replace(".DLL", "");
                            icon_lib.Text = fileName;
                            VSGConfig.IconDir = System.IO.Path.GetDirectoryName(fullPath);
                            VSGConfig.WriteFile();
                            icon_lib.SetParameterValue();
                            updateIcons();
                        }   
                        
                    }
                }
            }
            else
            {
                using (CreateLibraryDialog dlg = new CreateLibraryDialog(iconListView1.LargeImageList))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        if (dlg.LibraryName != null)
                        {
                            string fullPath = System.IO.Path.GetFullPath(dlg.LibraryName);
                            string fileName = System.IO.Path.GetFileName(fullPath);
                            fileName = fileName.Replace(".dll", "");
                            fileName = fileName.Replace(".DLL", "");
                            icon_lib.Text = fileName;
                            icon_lib.SetParameterValue();
                            VSGConfig.IconDir = System.IO.Path.GetDirectoryName(fullPath);
                            VSGConfig.WriteFile();
                            updateIcons();
                        }
                    }
                }
            }
        }

        private void openIconLibDialog()
        {
            using (OpenImageDlg = new OpenFileDialog())
            {
                OpenImageDlg.Title = "Open Icon Library";
                OpenImageDlg.RestoreDirectory = true;
                if (VSGConfig.IconDir != String.Empty)
                {
                    OpenImageDlg.InitialDirectory = VSGConfig.IconDir;
                }
                OpenImageDlg.Filter = "Library(*.dll)|*.dll";

                if (OpenImageDlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (OpenImageDlg.FileName != null)
                    {
                        string fullPath = System.IO.Path.GetFullPath(OpenImageDlg.FileName);
                        string fileName = System.IO.Path.GetFileName(fullPath);
                        fileName = fileName.Replace(".dll", "");
                        fileName = fileName.Replace(".DLL", "");
                        icon_lib.Text = fileName;
                        icon_lib.SetParameterValue();
                        VSGConfig.IconDir = System.IO.Path.GetDirectoryName(fullPath);
                        VSGConfig.WriteFile();
                        updateIcons();
                    }
                }
            }
            
        }

        private void buttonOpenIconLib_Click(object sender, EventArgs e)
        {
            openIconLibDialog();
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
                icon_lib.Controller = controller;
                iconListView1.Controller = controller;
            }
        }

        public void UpdateViewComponent()
        {
            icon_lib.UpdateViewComponent();
            iconListView1.UpdateViewComponent();
        }

        #endregion
    }
}
