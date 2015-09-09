namespace DDD_ILC
{
    partial class LibraryManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LibraryManager));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.BrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.ImageTab = new System.Windows.Forms.TabPage();
            this.ImageTabContent = new System.Windows.Forms.SplitContainer();
            this.button1 = new System.Windows.Forms.Button();
            this.ImageTree = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CurrentDir = new System.Windows.Forms.TextBox();
            this.BrowseBtn = new System.Windows.Forms.Button();
            this.ImageDialog = new System.Windows.Forms.SplitContainer();
            this.Rotate_Checkbox = new System.Windows.Forms.CheckBox();
            this.OutputDirBtn = new System.Windows.Forms.Button();
            this.OutputDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.CompileBtn = new System.Windows.Forms.Button();
            this.LibraryName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ImageBox = new System.Windows.Forms.PictureBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.LibViewerTab = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.CurrentLibLbl = new System.Windows.Forms.Label();
            this.LibViewBrowseBtn = new System.Windows.Forms.Button();
            this.MapTab = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.ExportMapBtn = new System.Windows.Forms.Button();
            this.MapBox = new System.Windows.Forms.PictureBox();
            this.OpenMapDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveMapDialog = new System.Windows.Forms.SaveFileDialog();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.ImageTab.SuspendLayout();
            this.ImageTabContent.Panel1.SuspendLayout();
            this.ImageTabContent.Panel2.SuspendLayout();
            this.ImageTabContent.SuspendLayout();
            this.panel1.SuspendLayout();
            this.ImageDialog.Panel1.SuspendLayout();
            this.ImageDialog.Panel2.SuspendLayout();
            this.ImageDialog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).BeginInit();
            this.LibViewerTab.SuspendLayout();
            this.panel3.SuspendLayout();
            this.MapTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MapBox)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // BrowserDialog
            // 
            this.BrowserDialog.Description = "Locate images.";
            this.BrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.BrowserDialog.SelectedPath = "C:\\Documents and Settings\\ebonomolo\\My Documents";
            this.BrowserDialog.ShowNewFolderButton = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(533, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "Ready.";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 466);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(681, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.ImageTab);
            this.tabControl1.Controls.Add(this.LibViewerTab);
            this.tabControl1.Controls.Add(this.MapTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(681, 466);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // ImageTab
            // 
            this.ImageTab.BackColor = System.Drawing.SystemColors.Control;
            this.ImageTab.Controls.Add(this.ImageTabContent);
            this.ImageTab.Location = new System.Drawing.Point(4, 22);
            this.ImageTab.Name = "ImageTab";
            this.ImageTab.Padding = new System.Windows.Forms.Padding(3);
            this.ImageTab.Size = new System.Drawing.Size(673, 440);
            this.ImageTab.TabIndex = 0;
            this.ImageTab.Text = "Library Creator";
            // 
            // ImageTabContent
            // 
            this.ImageTabContent.BackColor = System.Drawing.SystemColors.Control;
            this.ImageTabContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageTabContent.Location = new System.Drawing.Point(3, 3);
            this.ImageTabContent.Name = "ImageTabContent";
            // 
            // ImageTabContent.Panel1
            // 
            this.ImageTabContent.Panel1.Controls.Add(this.ImageTree);
            this.ImageTabContent.Panel1.Controls.Add(this.button1);
            this.ImageTabContent.Panel1.Controls.Add(this.panel1);
            // 
            // ImageTabContent.Panel2
            // 
            this.ImageTabContent.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.ImageTabContent.Panel2.Controls.Add(this.ImageDialog);
            this.ImageTabContent.Size = new System.Drawing.Size(667, 434);
            this.ImageTabContent.SplitterDistance = 222;
            this.ImageTabContent.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 411);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(222, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Toggle Select";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ImageTree
            // 
            this.ImageTree.CheckBoxes = true;
            this.ImageTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageTree.HideSelection = false;
            this.ImageTree.Location = new System.Drawing.Point(0, 20);
            this.ImageTree.Name = "ImageTree";
            this.ImageTree.ShowLines = false;
            this.ImageTree.ShowRootLines = false;
            this.ImageTree.Size = new System.Drawing.Size(222, 391);
            this.ImageTree.TabIndex = 1;
            this.ImageTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ImageTree_AfterSelect);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CurrentDir);
            this.panel1.Controls.Add(this.BrowseBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(222, 20);
            this.panel1.TabIndex = 3;
            // 
            // CurrentDir
            // 
            this.CurrentDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CurrentDir.Location = new System.Drawing.Point(0, 0);
            this.CurrentDir.Name = "CurrentDir";
            this.CurrentDir.Size = new System.Drawing.Size(168, 20);
            this.CurrentDir.TabIndex = 2;
            this.CurrentDir.MouseHover += new System.EventHandler(this.CurrentDir_MouseHover);
            this.CurrentDir.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CurrentDirLbl_KeyDown);
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.BrowseBtn.Location = new System.Drawing.Point(168, 0);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.Size = new System.Drawing.Size(54, 20);
            this.BrowseBtn.TabIndex = 3;
            this.BrowseBtn.Text = "Browse";
            this.BrowseBtn.UseVisualStyleBackColor = true;
            this.BrowseBtn.Click += new System.EventHandler(this.BrowseBtn_Click);
            // 
            // ImageDialog
            // 
            this.ImageDialog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageDialog.Location = new System.Drawing.Point(0, 0);
            this.ImageDialog.Name = "ImageDialog";
            this.ImageDialog.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // ImageDialog.Panel1
            // 
            this.ImageDialog.Panel1.Controls.Add(this.Rotate_Checkbox);
            this.ImageDialog.Panel1.Controls.Add(this.OutputDirBtn);
            this.ImageDialog.Panel1.Controls.Add(this.OutputDir);
            this.ImageDialog.Panel1.Controls.Add(this.label2);
            this.ImageDialog.Panel1.Controls.Add(this.CancelBtn);
            this.ImageDialog.Panel1.Controls.Add(this.CompileBtn);
            this.ImageDialog.Panel1.Controls.Add(this.LibraryName);
            this.ImageDialog.Panel1.Controls.Add(this.label1);
            this.ImageDialog.Panel1.Controls.Add(this.ImageBox);
            // 
            // ImageDialog.Panel2
            // 
            this.ImageDialog.Panel2.Controls.Add(this.richTextBox1);
            this.ImageDialog.Size = new System.Drawing.Size(441, 434);
            this.ImageDialog.SplitterDistance = 161;
            this.ImageDialog.TabIndex = 0;
            // 
            // Rotate_Checkbox
            // 
            this.Rotate_Checkbox.AutoSize = true;
            this.Rotate_Checkbox.Location = new System.Drawing.Point(290, 121);
            this.Rotate_Checkbox.Name = "Rotate_Checkbox";
            this.Rotate_Checkbox.Size = new System.Drawing.Size(112, 17);
            this.Rotate_Checkbox.TabIndex = 8;
            this.Rotate_Checkbox.Text = "Rotatable Graphic";
            this.Rotate_Checkbox.UseVisualStyleBackColor = true;
            this.Rotate_Checkbox.CheckedChanged += new System.EventHandler(this.Rotate_Checkbox_CheckedChanged);
            // 
            // OutputDirBtn
            // 
            this.OutputDirBtn.Location = new System.Drawing.Point(190, 74);
            this.OutputDirBtn.Name = "OutputDirBtn";
            this.OutputDirBtn.Size = new System.Drawing.Size(59, 23);
            this.OutputDirBtn.TabIndex = 7;
            this.OutputDirBtn.Text = "Browse";
            this.OutputDirBtn.UseVisualStyleBackColor = true;
            this.OutputDirBtn.Click += new System.EventHandler(this.OutputDirBtn_Click);
            // 
            // OutputDir
            // 
            this.OutputDir.Location = new System.Drawing.Point(15, 74);
            this.OutputDir.Name = "OutputDir";
            this.OutputDir.Size = new System.Drawing.Size(169, 20);
            this.OutputDir.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Export To:";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(135, 117);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(114, 23);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // CompileBtn
            // 
            this.CompileBtn.Location = new System.Drawing.Point(15, 117);
            this.CompileBtn.Name = "CompileBtn";
            this.CompileBtn.Size = new System.Drawing.Size(114, 23);
            this.CompileBtn.TabIndex = 3;
            this.CompileBtn.Text = "Create Library";
            this.CompileBtn.UseVisualStyleBackColor = true;
            this.CompileBtn.Click += new System.EventHandler(this.CompileBtn_Click);
            // 
            // LibraryName
            // 
            this.LibraryName.Location = new System.Drawing.Point(15, 34);
            this.LibraryName.Name = "LibraryName";
            this.LibraryName.Size = new System.Drawing.Size(169, 20);
            this.LibraryName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Library Name";
            // 
            // ImageBox
            // 
            this.ImageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ImageBox.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ImageBox.Location = new System.Drawing.Point(290, 17);
            this.ImageBox.Name = "ImageBox";
            this.ImageBox.Size = new System.Drawing.Size(126, 94);
            this.ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ImageBox.TabIndex = 0;
            this.ImageBox.TabStop = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(441, 269);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // LibViewerTab
            // 
            this.LibViewerTab.BackColor = System.Drawing.Color.Silver;
            this.LibViewerTab.Controls.Add(this.listView1);
            this.LibViewerTab.Controls.Add(this.panel3);
            this.LibViewerTab.Location = new System.Drawing.Point(4, 22);
            this.LibViewerTab.Name = "LibViewerTab";
            this.LibViewerTab.Size = new System.Drawing.Size(673, 440);
            this.LibViewerTab.TabIndex = 2;
            this.LibViewerTab.Text = "Library Viewer";
            this.LibViewerTab.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(0, 21);
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(673, 419);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel3.Controls.Add(this.CurrentLibLbl);
            this.panel3.Controls.Add(this.LibViewBrowseBtn);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(673, 21);
            this.panel3.TabIndex = 6;
            // 
            // CurrentLibLbl
            // 
            this.CurrentLibLbl.AutoEllipsis = true;
            this.CurrentLibLbl.BackColor = System.Drawing.SystemColors.Control;
            this.CurrentLibLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CurrentLibLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CurrentLibLbl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CurrentLibLbl.Location = new System.Drawing.Point(0, 0);
            this.CurrentLibLbl.Name = "CurrentLibLbl";
            this.CurrentLibLbl.Size = new System.Drawing.Size(598, 21);
            this.CurrentLibLbl.TabIndex = 6;
            this.CurrentLibLbl.Text = "No Library Chosen.";
            this.CurrentLibLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LibViewBrowseBtn
            // 
            this.LibViewBrowseBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.LibViewBrowseBtn.Location = new System.Drawing.Point(598, 0);
            this.LibViewBrowseBtn.Name = "LibViewBrowseBtn";
            this.LibViewBrowseBtn.Size = new System.Drawing.Size(75, 21);
            this.LibViewBrowseBtn.TabIndex = 5;
            this.LibViewBrowseBtn.Text = "Browse";
            this.LibViewBrowseBtn.UseVisualStyleBackColor = true;
            this.LibViewBrowseBtn.Click += new System.EventHandler(this.LibViewBrowseBtn_Click);
            // 
            // MapTab
            // 
            this.MapTab.BackColor = System.Drawing.SystemColors.Control;
            this.MapTab.Controls.Add(this.label8);
            this.MapTab.Controls.Add(this.ExportMapBtn);
            this.MapTab.Controls.Add(this.MapBox);
            this.MapTab.Location = new System.Drawing.Point(4, 22);
            this.MapTab.Name = "MapTab";
            this.MapTab.Padding = new System.Windows.Forms.Padding(3);
            this.MapTab.Size = new System.Drawing.Size(673, 440);
            this.MapTab.TabIndex = 1;
            this.MapTab.Text = "Map Converter";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(222, 16);
            this.label8.TabIndex = 12;
            this.label8.Text = "Click Picture Box to select an image.";
            // 
            // ExportMapBtn
            // 
            this.ExportMapBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportMapBtn.Location = new System.Drawing.Point(569, 289);
            this.ExportMapBtn.Name = "ExportMapBtn";
            this.ExportMapBtn.Size = new System.Drawing.Size(96, 23);
            this.ExportMapBtn.TabIndex = 2;
            this.ExportMapBtn.Text = "Export Mapfile";
            this.ExportMapBtn.UseVisualStyleBackColor = true;
            this.ExportMapBtn.Click += new System.EventHandler(this.ExportMapBtn_Click);
            // 
            // MapBox
            // 
            this.MapBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MapBox.BackColor = System.Drawing.Color.Black;
            this.MapBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MapBox.BackgroundImage")));
            this.MapBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.MapBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MapBox.ErrorImage = null;
            this.MapBox.InitialImage = null;
            this.MapBox.Location = new System.Drawing.Point(6, 42);
            this.MapBox.Name = "MapBox";
            this.MapBox.Size = new System.Drawing.Size(659, 229);
            this.MapBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.MapBox.TabIndex = 0;
            this.MapBox.TabStop = false;
            this.MapBox.Click += new System.EventHandler(this.MapImage_Click);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.WorkerReportsProgress = true;
            this.backgroundWorker2.WorkerSupportsCancellation = true;
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 250;
            this.toolTip1.ReshowDelay = 100;
            // 
            // LibraryManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 488);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(689, 522);
            this.Name = "LibraryManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DDD Library Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ImageTab.ResumeLayout(false);
            this.ImageTabContent.Panel1.ResumeLayout(false);
            this.ImageTabContent.Panel2.ResumeLayout(false);
            this.ImageTabContent.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ImageDialog.Panel1.ResumeLayout(false);
            this.ImageDialog.Panel1.PerformLayout();
            this.ImageDialog.Panel2.ResumeLayout(false);
            this.ImageDialog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).EndInit();
            this.LibViewerTab.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.MapTab.ResumeLayout(false);
            this.MapTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MapBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.FolderBrowserDialog BrowserDialog;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage ImageTab;
        private System.Windows.Forms.TabPage MapTab;
        private System.Windows.Forms.PictureBox MapBox;
        private System.Windows.Forms.Button ExportMapBtn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.OpenFileDialog OpenMapDialog;
        private System.Windows.Forms.SaveFileDialog SaveMapDialog;
        private System.Windows.Forms.TabPage LibViewerTab;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button LibViewBrowseBtn;
        private System.Windows.Forms.Label CurrentLibLbl;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.SplitContainer ImageTabContent;
        private System.Windows.Forms.TreeView ImageTree;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox CurrentDir;
        private System.Windows.Forms.Button BrowseBtn;
        private System.Windows.Forms.SplitContainer ImageDialog;
        private System.Windows.Forms.Button OutputDirBtn;
        private System.Windows.Forms.TextBox OutputDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button CompileBtn;
        private System.Windows.Forms.TextBox LibraryName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox ImageBox;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox Rotate_Checkbox;

    }
}

