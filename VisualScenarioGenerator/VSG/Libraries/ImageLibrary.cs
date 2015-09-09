using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace VSG.Libraries
{
    partial class ImageLibrary
    {
        private static string CSC = "csc";

        private static string csc_out_library = "/out:{0} /target:library ";
        private static string csc_lib_resources = "/resource:{0} ";

        private static string PATH = string.Empty;
        
        
       
        public static Image BrowseMaps(Size picturebox_size)
        {
            OpenFileDialog OpenMapDialog = new OpenFileDialog();

            OpenMapDialog.Filter = "Tiff files (*.tiff)|*.tif|All files (*.*)|*.*";
            OpenMapDialog.RestoreDirectory = true;
            OpenMapDialog.InitialDirectory = Environment.CurrentDirectory;
            if (OpenMapDialog.ShowDialog() == DialogResult.OK)
            {
                return OpenImageFile(OpenMapDialog.FileName, picturebox_size);
            }
            return null;
        }

        public static bool HasDotNet()
        {
            string windir = Path.GetDirectoryName(Environment.SystemDirectory);
            string path = string.Format("{0}\\Microsoft.NET\\Framework\\v{1}.{2}.{3}\\csc.exe", windir, Environment.Version.Major, Environment.Version.Minor, Environment.Version.Build);
            CSC = path;
            PATH = string.Format(";{0}\\Microsoft.NET\\Framework\\v{1}.{2}.{3}", windir, Environment.Version.Major, Environment.Version.Minor, Environment.Version.Build);
            return File.Exists(path);
        }

        public static void CreateManifestFile(string directory, Dictionary<string, bool> image_list)
        {
            StreamWriter manifest_stream = File.CreateText(directory + "\\ImageLibrary.mf");
            StreamWriter resource_file = File.CreateText(directory + "\\resources.bat");

            foreach (string name in image_list.Keys)
            {
                manifest_stream.WriteLine(string.Format("{0}:{1}", name, image_list[name].ToString()));
                resource_file.WriteLine(string.Format("/resource:{0} ", name));
            }
            manifest_stream.Close();
            resource_file.Close();            
        }

        public static string CreateLibrary(string temp_directory, string output_directory, string library_name)
        {
            if (!HasDotNet())
            {
                MessageBox.Show("Cannot generate libraries without the .Net Framework v2.0 (Software Development Kit) SDK. \nThe .NET Framework v2.0 SDK can be downloaded from www.microsoft.com.", "Unable to locate .NET SDK");
                return string.Empty;
            }
            string last_directory = Environment.CurrentDirectory;

            
            if (output_directory != string.Empty)
            {
                if (library_name != string.Empty)
                {
                    Environment.CurrentDirectory = temp_directory;
                    string output = string.Empty;

                    StringBuilder command = new StringBuilder();
                    command.Append(string.Format(csc_out_library, library_name));
                    command.Append(string.Format(csc_lib_resources, "ImageLibrary.mf @resources.bat"));

                    System.Diagnostics.Process Compiler = new System.Diagnostics.Process();
                    Compiler.StartInfo.FileName = CSC;
                    Compiler.StartInfo.Arguments = command.ToString();
                    Compiler.StartInfo.UseShellExecute = false; // because I'm redirecting output.
                    Compiler.StartInfo.CreateNoWindow = true;
                    Compiler.StartInfo.RedirectStandardError = true;
                    Compiler.StartInfo.RedirectStandardOutput = true;
                    Compiler.StartInfo.EnvironmentVariables["Path"] = System.Environment.ExpandEnvironmentVariables("%PATH%")+PATH;
                    try
                    {
                        Compiler.Start();
                        StreamReader reader = Compiler.StandardOutput;
                        StreamReader errors = Compiler.StandardError;

                        Compiler.WaitForExit();

                        output = errors.ReadToEnd();
                        reader.Close();
                        errors.Close();

                        if (File.Exists(library_name))
                        {
                            File.Move(library_name, output_directory + "//" + library_name);
                        }
                    }
                    catch (Exception )
                    {
                    }                    

                    foreach (string file in Directory.GetFiles(temp_directory, "*.png"))
                    {
                        System.IO.File.Delete(file);
                    }

                    Environment.CurrentDirectory = last_directory;
                    return output;
                }
            }
            return "Invalid Arguements";

        }

        public static Image OpenImageFile(string path, Size size)
        {
            Image b = Bitmap.FromFile(path);
            if ((b.Size.Width > size.Width) && (b.Width > size.Width))
            {
                return Bitmap.FromFile(path).GetThumbnailImage(size.Width, size.Height, null, IntPtr.Zero);
            }
            return b;
        }
        private static Image OpenImageFile(string path)
        {
            return Bitmap.FromFile(path);
        }

        public static void SaveMapFile(string inpath, string outpath)
        {
            // Can't call orginal.Save to convert file, because .NET puts a lock original.
            // As a result, we need to create a separate bitmap area (a copy of the original) and save
            // that image.
            Image original = Image.FromFile(inpath);
         
            Bitmap b = new Bitmap(original);
            b.Save(outpath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public static ImageList OpenLibrary(Assembly CurrentLibrary)
        {
            try
            {
                StreamReader s = new StreamReader(CurrentLibrary.GetManifestResourceStream("ImageLibrary.mf"));

                ImageList list = new ImageList();
                while (!s.EndOfStream)
                {
                    string[] attributes = (s.ReadLine()).Split(':');
                    string name = attributes[0];

                    list.Images.Add(name, Image.FromStream(CurrentLibrary.GetManifestResourceStream(name)));
                }

                s.Close();
                
                return list;
            }
            catch (Exception)
            {
                MessageBox.Show("This library is incompatible or corrupted.");
            }
            return null;
        }


    }
}
