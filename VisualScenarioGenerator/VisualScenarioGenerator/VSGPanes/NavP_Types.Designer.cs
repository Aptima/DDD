namespace VisualScenarioGenerator.VSGPanes
{
    partial class NavP_Types
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Teams");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Decision Makers");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Networks");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Engrams");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Emitters");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Sensors");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Capabilities");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Vulnerabilities");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Species");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("States");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NavP_Types));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnNewNode = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteNode = new System.Windows.Forms.ToolStripButton();
            this.label4 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.HideSelection = false;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 51);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Team_Node";
            treeNode1.Text = "Teams";
            treeNode2.Name = "DM_Node";
            treeNode2.Text = "Decision Makers";
            treeNode3.Name = "Network_Node";
            treeNode3.Text = "Networks";
            treeNode4.Name = "Define_Engram_Node";
            treeNode4.Text = "Engrams";
            treeNode5.Name = "Emitter_Node";
            treeNode5.Text = "Emitters";
            treeNode6.Name = "Sensor_Node";
            treeNode6.Text = "Sensors";
            treeNode7.Name = "Capability_Node";
            treeNode7.Text = "Capabilities";
            treeNode8.Name = "Vulnerabilities_Node";
            treeNode8.Text = "Vulnerabilities";
            treeNode9.Name = "Species_Node";
            treeNode9.Text = "Species";
            treeNode10.Name = "State_Node";
            treeNode10.Text = "States";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(182, 188);
            this.treeView1.TabIndex = 0;
            this.treeView1.Enter += new System.EventHandler(this.treeView1_Enter);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Folder.ico");
            this.imageList1.Images.SetKeyName(1, "folderopen.ico");
            this.imageList1.Images.SetKeyName(2, "user.ico");
            this.imageList1.Images.SetKeyName(3, "otheroptions.ico");
            this.imageList1.Images.SetKeyName(4, "OrgChartHS.png");
            this.imageList1.Images.SetKeyName(5, "FindHS.png");
            this.imageList1.Images.SetKeyName(6, "RadialChartHS.png");
            this.imageList1.Images.SetKeyName(7, "DeleteHS.png");
            this.imageList1.Images.SetKeyName(8, "NewDocumentHS.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNewNode,
            this.btnDeleteNode});
            this.toolStrip1.Location = new System.Drawing.Point(0, 26);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(182, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnNewNode
            // 
            this.btnNewNode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNewNode.Image = global::VisualScenarioGenerator.Properties.Resources.NewDocumentHS;
            this.btnNewNode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNewNode.Name = "btnNewNode";
            this.btnNewNode.Size = new System.Drawing.Size(23, 22);
            this.btnNewNode.Text = "toolStripButton1";
            this.btnNewNode.Click += new System.EventHandler(this.btnNewNode_Click);
            // 
            // btnDeleteNode
            // 
            this.btnDeleteNode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteNode.Image = global::VisualScenarioGenerator.Properties.Resources.DeleteHS;
            this.btnDeleteNode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteNode.Name = "btnDeleteNode";
            this.btnDeleteNode.Size = new System.Drawing.Size(23, 22);
            this.btnDeleteNode.Text = "toolStripButton2";
            this.btnDeleteNode.Click += new System.EventHandler(this.btnDeleteNode_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(182, 26);
            this.label4.TabIndex = 27;
            this.label4.Text = "Object Types";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NavP_Types
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label4);
            this.Name = "NavP_Types";
            this.Size = new System.Drawing.Size(182, 239);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnNewNode;
        private System.Windows.Forms.ToolStripButton btnDeleteNode;
        private System.Windows.Forms.Label label4;

    }
}
