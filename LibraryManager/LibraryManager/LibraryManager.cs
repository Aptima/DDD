using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;


namespace DDD_ILC
{

    public partial class LibraryManager : Form
    {
        private string[] FileList = null;
        private string display = string.Empty;
        private string exists_command_str = @"C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\csc.exe";

        private string base_command_str = "csc";

        private string base_arguments_str = "/out:{0}.dll /target:library ";
        private string base_resource_str = "/resource:{0} ";


        private Assembly CurrentLibrary = null;

        private SDK_Missing MissingNET;

        public LibraryManager()
        {
            MissingNET = new SDK_Missing();
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(exists_command_str))
            {
                if (MissingNET.ShowDialog() == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
            CurrentDir.Text = ImageListBaseDir;
            OutputDir.Text = Environment.CurrentDirectory;
            PopulateFileList(CurrentDir.Text);
            CancelBtn.Enabled = false;
        }




        private void CurrentDir_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(CurrentDir, CurrentDir.Text);
        }
        
        private void ImageTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.ImageBox.Image = this.OpenImageFile(((Resources)e.Node.Tag).Path, ImageBox.Size);
            this.Rotate_Checkbox.Checked = ((Resources)e.Node.Tag).Rotatable;
        }

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            this.BrowseImages();
        }

        private void CurrentDirLbl_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Return) || (e.KeyCode == Keys.Enter))
            {
                PopulateFileList(CurrentDir.Text);
            }
        }

        private void OutputDirBtn_Click(object sender, EventArgs e)
        {
            if (BrowserDialog.ShowDialog() == DialogResult.OK)
            {
                OutputDir.Text = BrowserDialog.SelectedPath;
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            if (Compiler != null)
            {
                if (!Compiler.HasExited)
                {
                    Compiler.Kill();
                }
                while (!Compiler.HasExited)
                {
                }
                toolStripStatusLabel1.Text = "Canceled.";
                toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
                CancelBtn.Enabled = false;
            }
        }

        private void CompileBtn_Click(object sender, EventArgs e)
        {
            CancelBtn.Enabled = true;
            backgroundWorker2.RunWorkerAsync();
        }

        private void MapImage_Click(object sender, EventArgs e)
        {
            BrowseMaps();
        }

        private void ExportMapBtn_Click(object sender, EventArgs e)
        {
            if (CurrentImage != null)
            {
                SaveMapDialog.RestoreDirectory = true;
                SaveMapDialog.Filter = "Jpeg(*.jpg)|*.jpg";
                if (SaveMapDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveMapFile(CurrentImage, SaveMapDialog.FileName);
                }
            }
        }
        
        private void LibViewBrowseBtn_Click(object sender, EventArgs e)
        {
            this.OpenLibrary();
        }



        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            FindFiles();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UI_StateInfo si = (UI_StateInfo)e.UserState;
            switch (si.UpdateType)
            {
                case UPDATE_TYPE.PROGRESS:
                    this.toolStripStatusLabel1.Text = si.Message;
                    this.toolStripProgressBar1.Style = si.StyleInfo;
                    break;
                case UPDATE_TYPE.DATA:
                    TreeNode t = new TreeNode();
                    t.Checked = true;
                    t.Text = si.CheckboxData.ToString();
                    t.Tag = si.CheckboxData;

                    this.ImageTree.Nodes.Add(t);
                    this.toolStripStatusLabel1.Text = si.CheckboxData.Path;

                    if (!CompileBtn.Enabled)
                    {
                        CompileBtn.Enabled = true;
                        LibraryName.Enabled = true;
                        CancelBtn.Enabled = true;
                        OutputDir.Enabled = true;
                        OutputDirBtn.Enabled = true;
                        Rotate_Checkbox.Enabled = true;
                    }
                    //this.checkedListBox1.Items.Add(si.CheckboxData, true);
                    break;
                case UPDATE_TYPE.CLEAR_IMAGE_TREE:
                    ImageTree.Nodes.Clear();
                    if (CompileBtn.Enabled)
                    {
                        CompileBtn.Enabled = false;
                        ImageBox.Image = null;
                        CancelBtn.Enabled = false;
                        LibraryName.Enabled = false;
                        OutputDir.Enabled = false;
                        OutputDirBtn.Enabled = false;
                        Rotate_Checkbox.Enabled = false;
                    }
                    break;
                case UPDATE_TYPE.CLEAR_MAP_TREE:
                    break;
            }


        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            CreateLibrary();
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UI_StateInfo si = (UI_StateInfo)e.UserState;
            switch (si.UpdateType)
            {
                case UPDATE_TYPE.PROGRESS:
                    this.toolStripStatusLabel1.Text = si.Message;
                    this.toolStripProgressBar1.Style = si.StyleInfo;
                    break;
                case UPDATE_TYPE.TEXT_BOX:
                    richTextBox1.AppendText(si.Message);
                    richTextBox1.ScrollToCaret();
                    break;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in ImageTree.Nodes)
            {
                node.Checked = !node.Checked;
            }
        }

        private void Rotate_Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            ((Resources)ImageTree.SelectedNode.Tag).Rotatable = Rotate_Checkbox.Checked;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }




    }
}