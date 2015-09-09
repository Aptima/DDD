using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VisualScenarioGenerator.Libraries;
using VisualScenarioGenerator.Dialogs;


namespace VisualScenarioGenerator.Dialogs
{
    public partial class Ctl_ScenarioImages : Ctl_ContentPaneControl
    {
        private ScenarioResourcesDataStruct _datastore = ScenarioResourcesDataStruct.Empty;

        //private static string _icon_library;
        //private static string _map_file;

        public Ctl_ScenarioImages()
        {
            InitializeComponent();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.LargeImageList == null)
            {
                using (CreateLibraryDialog dlg = new CreateLibraryDialog())
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        if (dlg.LibraryName != null)
                        {
                            textBox7.Text = System.IO.Path.GetFileName(dlg.LibraryName);
                            _datastore.IconLibrary = dlg.LibraryName;
                            ImageList l = ImageLibrary.OpenLibrary(System.Reflection.Assembly.LoadFile(_datastore.IconLibrary));
                            listView1.SmallImageList = l;
                            listView1.LargeImageList = l;

                            listView1.Items.Clear();
                            foreach (string name in l.Images.Keys)
                            {
                                listView1.Items.Add(name, l.Images.IndexOfKey(name));
                            }
                        }
                    }
                }
            }
            else
            {
                using (CreateLibraryDialog dlg = new CreateLibraryDialog(listView1.LargeImageList))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        if (dlg.LibraryName != null)
                        {
                            textBox7.Text = System.IO.Path.GetFileName(dlg.LibraryName);
                            _datastore.IconLibrary = dlg.LibraryName;
                            ImageList l = ImageLibrary.OpenLibrary(System.Reflection.Assembly.LoadFile(_datastore.IconLibrary));
                            listView1.SmallImageList = l;
                            listView1.LargeImageList = l;

                            listView1.Items.Clear();
                            foreach (string name in l.Images.Keys)
                            {
                                listView1.Items.Add(name, l.Images.IndexOfKey(name));
                            }
                        }
                    }
                }

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenImageDlg = new OpenFileDialog())
            {
                OpenImageDlg.RestoreDirectory = true;
                OpenImageDlg.Filter = "Library(*.dll)|*.dll";

                if (OpenImageDlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (OpenImageDlg.FileName != null)
                    {
                        _datastore.IconLibrary = OpenImageDlg.FileName;
                        textBox7.Text = System.IO.Path.GetFileName(OpenImageDlg.FileName);

                        ImageList l = ImageLibrary.OpenLibrary(System.Reflection.Assembly.LoadFile(OpenImageDlg.FileName));
                        listView1.SmallImageList = l;
                        listView1.LargeImageList = l;


                        listView1.Items.Clear();
                        foreach (string name in l.Images.Keys)
                        {
                            listView1.Items.Add(name, l.Images.IndexOfKey(name));
                        }
                    }

                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenImageDlg = new OpenFileDialog())
            {
                OpenImageDlg.RestoreDirectory = true;
                OpenImageDlg.Filter = "GeoTiff(*.tiff)|*.tiff|Jpeg(*.jpg)|*.jpg";

                if (OpenImageDlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (OpenImageDlg.FileName != null)
                    {
                        _datastore.MapFile = OpenImageDlg.FileName;
                        map_file.Text = System.IO.Path.GetFileName(OpenImageDlg.FileName);
                        map_picturebox.Image = ImageLibrary.OpenImageFile(OpenImageDlg.FileName, map_picturebox.Size);
                        _datastore.Map = new Bitmap(OpenImageDlg.FileName);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            // Our Content Panel control is a few containers back.
            //((ICtl_ContentPane__OutboundUpdate)(Parent.Parent.Parent)).Update(this, (object)_datastore);
            Notify((object)_datastore);
        }
    }

    public struct ScenarioResourcesDataStruct : ICloneable
    {
        public string MapFile;
        public Bitmap Map;
        public string IconLibrary;
        public ImageList Images;

        public static ScenarioResourcesDataStruct Empty = new ScenarioResourcesDataStruct();


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is ScenarioResourcesDataStruct)
            {
                return ((((ScenarioResourcesDataStruct)obj).MapFile == MapFile) && 
                    (((ScenarioResourcesDataStruct)obj).IconLibrary == IconLibrary));
            }
            return false;
        }
        #region ICloneable Members

        public object Clone()
        {
            ScenarioResourcesDataStruct obj = new ScenarioResourcesDataStruct();
            obj.IconLibrary = IconLibrary;
            obj.MapFile = MapFile;
            obj.Map = Map;
            obj.Images = Images;
            return obj;
        }

        #endregion

    }

}
