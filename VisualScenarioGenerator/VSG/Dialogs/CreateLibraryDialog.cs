using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

using System.Reflection;

using VSG.Libraries;

namespace VSG.Dialogs
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

        private Dictionary<string, Image> _images;
        public Dictionary<string, Image> Images
        {
            get
            {
                return _images;
            }
        }

        public CreateLibraryDialog()
        {
            InitializeComponent();
            _images = new Dictionary<string, Image>();
        }

        public CreateLibraryDialog(ImageList list)
        {
            InitializeComponent();
            _images = new Dictionary<string, Image>();
            listView1.LargeImageList = list;
            listView1.SmallImageList = list;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenImagesDlg.Title = "Include these image(s):";
            OpenImagesDlg.Multiselect = true;
            OpenImagesDlg.Filter = "Portable Network Graphics(*.png)|*.png";
            OpenImagesDlg.FileName = string.Empty;

            if (OpenImagesDlg.ShowDialog(this) == DialogResult.OK)
            {
                foreach (string file in OpenImagesDlg.FileNames)
                {
                    string filename = Path.GetFileName(file);
                    if (!_images.ContainsKey(filename))
                    {
                        _images.Add(Path.GetFileName(file), new Bitmap(file));
                        listView1.SmallImageList.Images.Add(filename, _images[filename]);
                        listView1.LargeImageList.Images.Add(filename, _images[filename]);
                        listView1.Items.Add(filename, filename, listView1.LargeImageList.Images.IndexOfKey(filename));
                    }
                    else
                    {
                        MessageBox.Show(string.Format("\'{0}\' already exists in the list of images, please remove or rename the image source file.", filename));
                    }
                }

            }
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\vsg";
            Directory.CreateDirectory(directory);
            Dictionary<string, bool> image_list = new Dictionary<string, bool>();

            using (Image i = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("VSG.Images.Unknown.png")))
            {
                i.Save(directory + "\\ImageLib.Unknown.png");
            }
            image_list.Add("ImageLib.Unknown.png", false);

            foreach (string name in _images.Keys)
            {
                if (name != "Unknown.png")
                {
                    if (listView1.Items.ContainsKey(name))
                    {

                        image_list.Add(string.Format("ImageLib.{0}", name), listView1.Items[name].Checked);
                        _images[name].Save(string.Format("{0}\\ImageLib.{1}", directory, name));
                    }
                }
            }
            ImageLibrary.CreateManifestFile(directory, image_list);

            SaveLibraryDlg.Title = "Save Image Library As:";
            SaveLibraryDlg.RestoreDirectory = true;
            SaveLibraryDlg.Filter = "Library(*.dll)|*.dll";
            SaveLibraryDlg.FileName = string.Empty;
            if (SaveLibraryDlg.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(SaveLibraryDlg.FileName))
                {
                    if (SaveLibraryDlg.OverwritePrompt)
                    {
                        try
                        {
                            File.Delete(SaveLibraryDlg.FileName);
                        }
                        catch (ArgumentNullException ane)
                        {
                            MessageBox.Show(ane.Message, "Save Library Error");
                        }
                        catch (DirectoryNotFoundException dne)
                        {
                            MessageBox.Show(dne.Message, "Save Library Error");
                        }
                        catch (NotSupportedException nse)
                        {
                            MessageBox.Show(nse.Message, "Save Library Error");
                        }
                        catch (UnauthorizedAccessException uae)
                        {
                            MessageBox.Show(uae.Message + "\nMake sure this file isn't already open.", "Save Library Error");
                        }
                        catch (PathTooLongException ptle)
                        {
                            MessageBox.Show(ptle.Message, "Save Library Error");
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                if (SaveLibraryDlg.FileName != null)
                {
                    string result = ImageLibrary.CreateLibrary(directory, Path.GetDirectoryName(SaveLibraryDlg.FileName), Path.GetFileName(SaveLibraryDlg.FileName));
                    if (result != string.Empty)
                    {
                        MessageBox.Show(result, "Create Library Failed");
                    }
                    _library_name = SaveLibraryDlg.FileName;
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
                listView1.Items.Remove(item);
            }
        }
    }
}
