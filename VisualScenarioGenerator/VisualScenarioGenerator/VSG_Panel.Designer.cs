namespace VisualScenarioGenerator
{
    partial class VSG_Panel
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Scenario Description",
            "Scenario",
            "Playfield",
            "Types",
            "Scoring",
            "Timeline",
            "Preview"}, 0, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Playfield"}, 1, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Object Types"}, 2, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Timeline"}, 4, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "Scoring"}, 3, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "Preview"}, 5, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSG_Panel));
            this.VSG_SplitContainer = new System.Windows.Forms.SplitContainer();
            this.Navigator_SplitContainer = new System.Windows.Forms.SplitContainer();
            this.TopLevel_Navigator = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.NavigatorImages = new System.Windows.Forms.ImageList(this.components);
            this.VSG_SplitContainer.Panel1.SuspendLayout();
            this.VSG_SplitContainer.SuspendLayout();
            this.Navigator_SplitContainer.Panel1.SuspendLayout();
            this.Navigator_SplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // VSG_SplitContainer
            // 
            this.VSG_SplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.VSG_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VSG_SplitContainer.Location = new System.Drawing.Point(0, 0);
            this.VSG_SplitContainer.Name = "VSG_SplitContainer";
            // 
            // VSG_SplitContainer.Panel1
            // 
            this.VSG_SplitContainer.Panel1.Controls.Add(this.Navigator_SplitContainer);
            // 
            // VSG_SplitContainer.Panel2
            // 
            this.VSG_SplitContainer.Panel2.AutoScroll = true;
            this.VSG_SplitContainer.Size = new System.Drawing.Size(587, 430);
            this.VSG_SplitContainer.SplitterDistance = 160;
            this.VSG_SplitContainer.TabIndex = 0;
            // 
            // Navigator_SplitContainer
            // 
            this.Navigator_SplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Navigator_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Navigator_SplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.Navigator_SplitContainer.Location = new System.Drawing.Point(0, 0);
            this.Navigator_SplitContainer.Name = "Navigator_SplitContainer";
            this.Navigator_SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // Navigator_SplitContainer.Panel1
            // 
            this.Navigator_SplitContainer.Panel1.Controls.Add(this.TopLevel_Navigator);
            // 
            // Navigator_SplitContainer.Panel2
            // 
            this.Navigator_SplitContainer.Panel2.AutoScroll = true;
            this.Navigator_SplitContainer.Size = new System.Drawing.Size(160, 430);
            this.Navigator_SplitContainer.SplitterDistance = 221;
            this.Navigator_SplitContainer.TabIndex = 0;
            // 
            // TopLevel_Navigator
            // 
            this.TopLevel_Navigator.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.TopLevel_Navigator.BackColor = System.Drawing.SystemColors.Window;
            this.TopLevel_Navigator.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TopLevel_Navigator.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.TopLevel_Navigator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TopLevel_Navigator.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.TopLevel_Navigator.HideSelection = false;
            this.TopLevel_Navigator.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6});
            this.TopLevel_Navigator.LargeImageList = this.NavigatorImages;
            this.TopLevel_Navigator.Location = new System.Drawing.Point(0, 0);
            this.TopLevel_Navigator.MultiSelect = false;
            this.TopLevel_Navigator.Name = "TopLevel_Navigator";
            this.TopLevel_Navigator.Size = new System.Drawing.Size(156, 217);
            this.TopLevel_Navigator.SmallImageList = this.NavigatorImages;
            this.TopLevel_Navigator.TabIndex = 0;
            this.TopLevel_Navigator.UseCompatibleStateImageBehavior = false;
            this.TopLevel_Navigator.View = System.Windows.Forms.View.Details;
            this.TopLevel_Navigator.SelectedIndexChanged += new System.EventHandler(this.TopLevel_Navigator_SelectedIndexChanged);
            this.TopLevel_Navigator.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.TopLevel_Navigator_ItemSelectionChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "VSG Navigator";
            this.columnHeader1.Width = 200;
            // 
            // NavigatorImages
            // 
            this.NavigatorImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("NavigatorImages.ImageStream")));
            this.NavigatorImages.TransparentColor = System.Drawing.Color.Transparent;
            this.NavigatorImages.Images.SetKeyName(0, "Scenario.png");
            this.NavigatorImages.Images.SetKeyName(1, "Playfield.png");
            this.NavigatorImages.Images.SetKeyName(2, "ObjectTypes.png");
            this.NavigatorImages.Images.SetKeyName(3, "Scoring.png");
            this.NavigatorImages.Images.SetKeyName(4, "Timeline.png");
            this.NavigatorImages.Images.SetKeyName(5, "Preview.png");
            // 
            // VSG_Panel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.VSG_SplitContainer);
            this.Name = "VSG_Panel";
            this.Size = new System.Drawing.Size(587, 430);
            this.Load += new System.EventHandler(this.VSG_Panel_Load);
            this.VSG_SplitContainer.Panel1.ResumeLayout(false);
            this.VSG_SplitContainer.ResumeLayout(false);
            this.Navigator_SplitContainer.Panel1.ResumeLayout(false);
            this.Navigator_SplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer VSG_SplitContainer;
        private System.Windows.Forms.SplitContainer Navigator_SplitContainer;
        private System.Windows.Forms.ListView TopLevel_Navigator;
        private System.Windows.Forms.ImageList NavigatorImages;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}
