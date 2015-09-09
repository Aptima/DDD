using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

using System.Reflection;

using VisualScenarioGenerator.Libraries;

namespace VisualScenarioGenerator.Dialogs
{
    public partial class CreateLibraryDialog : Form
    {
        private string _library_name;
        public string LibraryName
        {
            get
            {
                return _library_name;
            }
        }

        private ImageList _images;
        public ImageList Images
        {
            get
            {
                return _images;
            }
        }

        public CreateLibraryDialog()
        {
            InitializeComponent();
            _images = new ImageList();
            listView1.LargeImageList = _images;
            listView1.SmallImageList = _images;
        }

        public CreateLibraryDialog(ImageList list)
        {
            InitializeComponent();
            _images = list;
            listView1.LargeImageList = _images;
            listView1.SmallImageList = _images;

            foreach (string name in _images.Images.Keys)
            {
                listView1.Items.Add(name, name, _images.Images.IndexOfKey(name));
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenImagesDlg.Title = "Include these image(s):";
            OpenImagesDlg.Multiselect = true;
            OpenImagesDlg.Filter = "Portable Network Graphics(*.png)|*.png";
            if (OpenImagesDlg.ShowDialog(this) == DialogResult.OK)
            {
                foreach (string file in OpenImagesDlg.FileNames)
                {
                    _images.Images.Add(file, new Bitmap(file));
                }

                foreach (string file in OpenImagesDlg.FileNames)
                {
                    listView1.Items.Add(file, Path.GetFileName(file), _images.Images.IndexOfKey(file));
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\vsg";
            Directory.CreateDirectory(directory);
            StreamWriter manifest_stream = File.CreateText(directory + "\\ImageLibrary.mf");
            StreamWriter resource_file = File.CreateText(directory + "\\resources.bat");

            manifest_stream.WriteLine("Unknown.png:False");
            using (Image i = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualScenarioGenerator.images.Unknown.png")))
            {
                i.Save(directory + "\\Unknown.png");
            }
            resource_file.WriteLine("/resource:Unknown.png ");

            foreach (ListViewItem item in listView1.Items)
            {
                System.Console.WriteLine("{0}, {1}", item.Name, item.Text);
            }
            foreach (string file in _images.Images.Keys)
            {
                string name = Path.GetFileName(file);
                if (name != "Unknown.png")
                {
                    System.Console.WriteLine("{0}", listView1.Items[file]);
                    manifest_stream.WriteLine(string.Format("{0}:{1}", name, listView1.Items[file].Checked.ToString()));
                    _images.Images[file].Save(directory + "\\" + name);
                    resource_file.WriteLine(string.Format("/resource:{0} ", name));
                }
            }

            manifest_stream.Close();

            resource_file.Close();

            SaveLibraryDlg.Title = "Save Image Library As:";
            SaveLibraryDlg.RestoreDirectory = true;
            SaveLibraryDlg.Filter = "Library(*.dll)|*.dll";
            if (SaveLibraryDlg.ShowDialog() == DialogResult.OK)
            {
                if (SaveLibraryDlg.FileName != null)
                {
                    string result = ImageLibrary.CreateLibrary(directory, Path.GetFileName(SaveLibraryDlg.FileName));
                    if (result != string.Empty)
                    {
                        MessageBox.Show(result);
                    }
                    if (File.Exists(SaveLibraryDlg.FileName))
                    {
                        File.Delete(SaveLibraryDlg.FileName);
                    }
                    System.IO.File.Move(directory + "\\" + Path.GetFileName(SaveLibraryDlg.FileName), SaveLibraryDlg.FileName);
                }
            }
            else
            {
                System.Console.WriteLine("Nothing saved.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                Console.WriteLine("Item name {0}", item.Name);
                listView1.Items.Remove(item);
            }
        }
    }
}