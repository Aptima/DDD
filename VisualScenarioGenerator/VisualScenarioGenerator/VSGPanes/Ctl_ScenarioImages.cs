using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VisualScenarioGenerator.Libraries;
using VisualScenarioGenerator.Dialogs;


namespace VisualScenarioGenerator.VSGPanes
{
    public partial class CntP_Playfield : Ctl_ContentPane
    {
        private static string _icon_library;
        private static string _map_file;

        public CntP_Playfield()
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
                        textBox7.Text = System.IO.Path.GetFileName(dlg.LibraryName);
                        _icon_library = dlg.LibraryName;
                        ImageList l = ImageLibrary.OpenLibrary(System.Reflection.Assembly.LoadFile(_icon_library));
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
            else
            {
                using (CreateLibraryDialog dlg = new CreateLibraryDialog(listView1.LargeImageList))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        textBox7.Text = System.IO.Path.GetFileName(dlg.LibraryName);
                        _icon_library = dlg.LibraryName;
                        ImageList l = ImageLibrary.OpenLibrary(System.Reflection.Assembly.LoadFile(_icon_library));
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

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenImageDlg = new OpenFileDialog())
            {
                OpenImageDlg.RestoreDirectory = true;
                OpenImageDlg.Filter = "Library(*.dll)|*.dll";

                if (OpenImageDlg.ShowDialog(this) == DialogResult.OK)
                {
                    _icon_library = OpenImageDlg.FileName;
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


        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenImageDlg = new OpenFileDialog())
            {
                OpenImageDlg.RestoreDirectory = true;
                OpenImageDlg.Filter = "GeoTiff(*.tiff)|*.tiff|Jpeg(*.jpg)|*.jpg";

                if (OpenImageDlg.ShowDialog(this) == DialogResult.OK)
                {
                    _map_file = OpenImageDlg.FileName;
                    map_file.Text = System.IO.Path.GetFileName(OpenImageDlg.FileName);
                    map_picturebox.Image = ImageLibrary.OpenImageFile(OpenImageDlg.FileName, map_picturebox.Size);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Console.WriteLine("{0}: {1}", _icon_library, _map_file);
        }
    }
}
