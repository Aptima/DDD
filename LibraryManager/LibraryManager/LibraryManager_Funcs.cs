using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace DDD_ILC
{
    partial class LibraryManager
    {
        public string LocalMapDir = Environment.CurrentDirectory;
        private System.Diagnostics.Process Compiler = null;
        private string CurrentImage = null;
        public string ImageListBaseDir = Environment.CurrentDirectory + @"\Config\images";

        private void PopulateFileList()
        {
            FileList = Directory.GetFiles(".", "*.png", SearchOption.AllDirectories);
            BrowserDialog.Description = "Please navigate to the folder that contains your images.";
            BrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            CurrentDir.Text = string.Empty;
            if (BrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.toolStripStatusLabel1.Text = "Populating image choices ...";
                this.toolStripProgressBar1.Style = ProgressBarStyle.Marquee;

                ImageListBaseDir = BrowserDialog.SelectedPath;
                CurrentDir.Text = ImageListBaseDir;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void PopulateFileList(string path)
        {
            if (!backgroundWorker1.IsBusy)
            {
                this.toolStripStatusLabel1.Text = "Populating image choices ...";
                this.toolStripProgressBar1.Style = ProgressBarStyle.Marquee;

                ImageListBaseDir = path;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        public void FindFiles()
        {
            UI_StateInfo si = new UI_StateInfo();
            si.UpdateType = UPDATE_TYPE.CLEAR_IMAGE_TREE;
            backgroundWorker1.ReportProgress(0, (object)si);
            try
            {
                FileList = Directory.GetFiles(ImageListBaseDir, "*.png", SearchOption.AllDirectories);
                if (FileList.Length == 0)
                {
                    MessageBox.Show("Current directory doesn't contain Images in PNG format.", "Invalid Image Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    si.UpdateType = UPDATE_TYPE.PROGRESS;
                    si.Message = "Ready.";
                    si.StyleInfo = ProgressBarStyle.Blocks;
                    backgroundWorker1.ReportProgress(0, (object)si);
                    return;
                }

                if (FileList != null)
                {
                    foreach (string file in FileList)
                    {
                        display = file.Replace(ImageListBaseDir, ImageListBaseDir.Substring(ImageListBaseDir.LastIndexOf(@"\")+1));
                        display = display.Replace(@"\", ".");

                        si.UpdateType = UPDATE_TYPE.DATA;
                        si.CheckboxData = new Resources(file, display);

                        backgroundWorker1.ReportProgress(0, (object)si);
                    }
                }
            }
            catch (Exception exc)
            {

                Environment.CurrentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                si.UpdateType = UPDATE_TYPE.PROGRESS;
                si.Message = exc.Message;
                si.StyleInfo = ProgressBarStyle.Blocks;
                backgroundWorker1.ReportProgress(0, (object)si);
                return;
            }
            si.UpdateType = UPDATE_TYPE.PROGRESS;
            si.Message = "Ready.";
            si.StyleInfo = ProgressBarStyle.Blocks;
            backgroundWorker1.ReportProgress(0, (object)si);

        }
        
        private void BrowseImages()
        {
            PopulateFileList();
        }

        public void BrowseMaps()
        {
            OpenMapDialog.Filter = "Tiff files (*.tiff)|*.tif|All files (*.*)|*.*";
            OpenMapDialog.RestoreDirectory = true;
            OpenMapDialog.InitialDirectory = LocalMapDir;
            if (OpenMapDialog.ShowDialog() == DialogResult.OK)
            {
                this.MapBox.Image = this.OpenImageFile(OpenMapDialog.FileName, MapBox.Size);
                CurrentImage = OpenMapDialog.FileName;
            }
        }

        private void CreateLibrary()
        {
            string unknown = string.Format("{0}\\Unknown.png", ImageListBaseDir);

            if (Array.IndexOf(FileList, unknown) == -1)
            {
                MessageBox.Show("DDD Image Libraries must contain a default image file named Unknown.png.\nPlease include an Unknown.png image at the library's top level.",
                    "Image Library Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UI_StateInfo si = new UI_StateInfo();
            si.UpdateType = UPDATE_TYPE.PROGRESS;
            si.Message = "Compiling Library ...";
            si.StyleInfo = ProgressBarStyle.Marquee;
            backgroundWorker2.ReportProgress(0, (object)si);

            List<string> temp_files = new List<string>();
            if (OutputDir.Text != string.Empty)
            {
                if (LibraryName.Text != string.Empty)
                {
                    try
                    {
                        StringBuilder command = new StringBuilder();
                        
                        command.Append(string.Format(base_arguments_str, LibraryName.Text));

                        StreamWriter manifest_stream = File.CreateText(OutputDir.Text + "\\" + "ImageLibrary.mf");
                        StreamWriter batch_file = File.CreateText(OutputDir.Text + "\\resources.bat");
                        command.Append(string.Format(base_resource_str, "ImageLibrary.mf @resources.bat"));

                        foreach (TreeNode node in ImageTree.Nodes)
                        {
                            if (node.Checked)
                            {
                                Resources file = (Resources)node.Tag;
                                temp_files.Add(file.ResourceName);
                                manifest_stream.WriteLine(string.Format("{0}:{1}", temp_files[temp_files.Count - 1], file.Rotatable));
                                try
                                {
                                    System.IO.File.Copy(file.Path, OutputDir.Text + "\\" + temp_files[temp_files.Count - 1]);
                                }
                                catch (Exception copy_exception)
                                {
                                    si.UpdateType = UPDATE_TYPE.TEXT_BOX;
                                    si.Message = copy_exception.Message;
                                    backgroundWorker2.ReportProgress(0, (object)si);
                                }
                                batch_file.WriteLine(string.Format(base_resource_str, file.ResourceName));
                            }
                        }
                        manifest_stream.Close();

                        batch_file.Close();

                        si.UpdateType = UPDATE_TYPE.TEXT_BOX;
                        si.Message = command.ToString() + "\n";
                        backgroundWorker2.ReportProgress(0, (object)si);

                        Environment.CurrentDirectory = OutputDir.Text;

                        System.Diagnostics.Process Compiler = new System.Diagnostics.Process();
                        Compiler.StartInfo.FileName = base_command_str;
                        Compiler.StartInfo.Arguments = command.ToString();
                        Compiler.StartInfo.UseShellExecute = false; // because I'm redirecting output.
                        Compiler.StartInfo.CreateNoWindow = true;
                        Compiler.StartInfo.RedirectStandardError = true;
                        Compiler.StartInfo.RedirectStandardOutput = true;
                        Compiler.StartInfo.EnvironmentVariables["Path"] = System.Environment.ExpandEnvironmentVariables("%PATH%");

                        Compiler.Start();
                        StreamReader reader = Compiler.StandardOutput;
                        StreamReader errors = Compiler.StandardError;

                        Compiler.WaitForExit();
                        si.UpdateType = UPDATE_TYPE.TEXT_BOX;
                        si.Message = reader.ReadToEnd() + "\n";
                        backgroundWorker2.ReportProgress(0, (object)si);

                        si.UpdateType = UPDATE_TYPE.TEXT_BOX;
                        si.Message = errors.ReadToEnd() + "\n";
                        backgroundWorker2.ReportProgress(0, (object)si);

                        reader.Close();
                        errors.Close();

                        if (System.IO.File.Exists(LibraryName.Text + ".dll"))
                        {
                            si.UpdateType = UPDATE_TYPE.TEXT_BOX;
                            si.Message = LibraryName.Text + ".dll Successfully Created.\n";
                            backgroundWorker2.ReportProgress(0, (object)si);

                            System.IO.File.Move(LibraryName.Text + ".dll", OutputDir.Text + "\\" + LibraryName.Text + ".dll");
                        }

                    }
                    catch (Exception library_exception)
                    {
                        si.UpdateType = UPDATE_TYPE.TEXT_BOX;
                        si.Message = library_exception.Message;
                        backgroundWorker2.ReportProgress(0, (object)si);
                    }
                }
                else
                {
                    si.UpdateType = UPDATE_TYPE.TEXT_BOX;
                    si.Message = "Error, must specify a library name.";
                    backgroundWorker2.ReportProgress(0, (object)si);
                }
                si.UpdateType = UPDATE_TYPE.TEXT_BOX;
                si.Message = "Cleaning up ...\n";
                backgroundWorker2.ReportProgress(0, (object)si);

                System.IO.File.Delete("resources.bat");
                foreach (string file in temp_files)
                {
                    System.IO.File.Delete(file);
                }
            }

            si.UpdateType = UPDATE_TYPE.PROGRESS;
            si.Message = "Ready.";
            si.StyleInfo = ProgressBarStyle.Blocks;
            backgroundWorker2.ReportProgress(0, (object)si);

        }

        private Image OpenImageFile(string path, Size size)
        {
            Image b = Bitmap.FromFile(path);
            if ((b.Size.Width > size.Width) && (b.Width > size.Width))
            {
                return Bitmap.FromFile(path).GetThumbnailImage(size.Width, size.Height, null, IntPtr.Zero);
            }
            return b;
        }
        private Image OpenImageFile(string path)
        {
            return Bitmap.FromFile(path);
        }

        private void SaveMapFile(string inpath, string outpath)
        {
            // Can't call orginal.Save to convert file, because .NET puts a lock original.
            // As a result, we need to create a separate bitmap area (a copy of the original) and save
            // that image.
            Image original = Image.FromFile(inpath);
         
            Bitmap b = new Bitmap(original);
            b.Save(outpath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void OpenLibrary()
        {
            OpenMapDialog.RestoreDirectory = true;
            OpenMapDialog.Filter = "Library(*.dll)|*.dll";
            if (OpenMapDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = Path.GetFileNameWithoutExtension(OpenMapDialog.FileName);
                CurrentLibLbl.Text = OpenMapDialog.FileName;
                CurrentLibrary = Assembly.LoadFile(OpenMapDialog.FileName);
                try
                {
                    StreamReader s = new StreamReader(CurrentLibrary.GetManifestResourceStream("ImageLibrary.mf"));

                    ImageList l = new ImageList();
                    listView1.SmallImageList = l;
                    listView1.LargeImageList = l;
                    while (!s.EndOfStream)
                    {
                        //LibViewTree.Nodes.Add(s.ReadLine());
                        string[] attributes = (s.ReadLine()).Split(':');
                        string name = attributes[0];

                        l.Images.Add(Image.FromStream(CurrentLibrary.GetManifestResourceStream(name)));
                        listView1.Items.Add(new ListViewItem(name, l.Images.Count - 1));

                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("This library is incompatible or corrupted.");
                    listView1.Items.Clear();
                }
            }
        }


    }
}
