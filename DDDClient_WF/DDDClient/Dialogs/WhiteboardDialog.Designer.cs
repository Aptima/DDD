namespace Aptima.Asim.DDD.Client.Dialogs
{
    partial class WhiteboardDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WhiteboardDialog));
            this.wbColorDialog = new System.Windows.Forms.ColorDialog();
            this.wbColorB = new System.Windows.Forms.Button();
            this.wbPointSize = new System.Windows.Forms.TrackBar();
            this.textDD = new System.Windows.Forms.ComboBox();
            this.clearB = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.circleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.arrowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoB = new System.Windows.Forms.Button();
            this.clearAllB = new System.Windows.Forms.Button();
            this.playerListDD = new System.Windows.Forms.ComboBox();
            this.viewUndoB = new System.Windows.Forms.Button();
            this.otherWBRoomsLB = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.wbTooltip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.wbPointSize)).BeginInit();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // wbColorB
            // 
            this.wbColorB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.wbColorB.Location = new System.Drawing.Point(9, 58);
            this.wbColorB.Name = "wbColorB";
            this.wbColorB.Size = new System.Drawing.Size(28, 30);
            this.wbColorB.TabIndex = 7;
            this.wbColorB.UseVisualStyleBackColor = false;
            this.wbColorB.Click += new System.EventHandler(this.wbColorB_Click);
            // 
            // wbPointSize
            // 
            this.wbPointSize.Location = new System.Drawing.Point(147, 51);
            this.wbPointSize.Maximum = 20;
            this.wbPointSize.Minimum = 1;
            this.wbPointSize.Name = "wbPointSize";
            this.wbPointSize.Size = new System.Drawing.Size(104, 56);
            this.wbPointSize.TabIndex = 9;
            this.wbPointSize.Value = 1;
            this.wbPointSize.Scroll += new System.EventHandler(this.wbPointSize_Scroll);
            // 
            // textDD
            // 
            this.textDD.FormattingEnabled = true;
            this.textDD.Location = new System.Drawing.Point(147, 21);
            this.textDD.Name = "textDD";
            this.textDD.Size = new System.Drawing.Size(130, 24);
            this.textDD.TabIndex = 14;
            this.textDD.SelectedIndexChanged += new System.EventHandler(this.textDD_SelectedIndexChanged);
            this.textDD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textDD_KeyPress);
            this.textDD.TextUpdate += new System.EventHandler(this.textDD_TextUpdate);
            // 
            // clearB
            // 
            this.clearB.Location = new System.Drawing.Point(42, 94);
            this.clearB.Name = "clearB";
            this.clearB.Size = new System.Drawing.Size(57, 27);
            this.clearB.TabIndex = 16;
            this.clearB.Text = "Clear";
            this.clearB.UseVisualStyleBackColor = true;
            this.clearB.Click += new System.EventHandler(this.clearB_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.toolStrip1);
            this.panel2.Location = new System.Drawing.Point(9, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(58, 43);
            this.panel2.TabIndex = 17;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(58, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectToolStripMenuItem,
            this.lineToolStripMenuItem,
            this.circleToolStripMenuItem,
            this.arrowToolStripMenuItem,
            this.textToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::Aptima.Asim.DDD.Client.Properties.Resources.Text;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(33, 24);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.ToolTipText = "Expand this dropdown to select a drawing control";
            this.toolStripDropDownButton1.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripDropDownButton1_DropDownItemClicked);
            // 
            // selectToolStripMenuItem
            // 
            this.selectToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("selectToolStripMenuItem.Image")));
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            this.selectToolStripMenuItem.Size = new System.Drawing.Size(156, 26);
            this.selectToolStripMenuItem.Text = "Select";
            // 
            // lineToolStripMenuItem
            // 
            this.lineToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("lineToolStripMenuItem.Image")));
            this.lineToolStripMenuItem.Name = "lineToolStripMenuItem";
            this.lineToolStripMenuItem.Size = new System.Drawing.Size(156, 26);
            this.lineToolStripMenuItem.Text = "Line";
            // 
            // circleToolStripMenuItem
            // 
            this.circleToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("circleToolStripMenuItem.Image")));
            this.circleToolStripMenuItem.Name = "circleToolStripMenuItem";
            this.circleToolStripMenuItem.Size = new System.Drawing.Size(156, 26);
            this.circleToolStripMenuItem.Text = "Circle";
            // 
            // arrowToolStripMenuItem
            // 
            this.arrowToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("arrowToolStripMenuItem.Image")));
            this.arrowToolStripMenuItem.Name = "arrowToolStripMenuItem";
            this.arrowToolStripMenuItem.Size = new System.Drawing.Size(156, 26);
            this.arrowToolStripMenuItem.Text = "Arrow";
            // 
            // textToolStripMenuItem
            // 
            this.textToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("textToolStripMenuItem.Image")));
            this.textToolStripMenuItem.Name = "textToolStripMenuItem";
            this.textToolStripMenuItem.Size = new System.Drawing.Size(156, 26);
            this.textToolStripMenuItem.Text = "Text";
            // 
            // undoB
            // 
            this.undoB.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("undoB.BackgroundImage")));
            this.undoB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.undoB.Location = new System.Drawing.Point(9, 94);
            this.undoB.Name = "undoB";
            this.undoB.Size = new System.Drawing.Size(27, 27);
            this.undoB.TabIndex = 15;
            this.undoB.UseVisualStyleBackColor = true;
            this.undoB.Click += new System.EventHandler(this.undoB_Click);
            // 
            // clearAllB
            // 
            this.clearAllB.Location = new System.Drawing.Point(105, 94);
            this.clearAllB.Name = "clearAllB";
            this.clearAllB.Size = new System.Drawing.Size(80, 27);
            this.clearAllB.TabIndex = 18;
            this.clearAllB.Text = "Clear All";
            this.clearAllB.UseVisualStyleBackColor = true;
            this.clearAllB.Click += new System.EventHandler(this.clearAllB_Click);
            // 
            // playerListDD
            // 
            this.playerListDD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playerListDD.FormattingEnabled = true;
            this.playerListDD.Location = new System.Drawing.Point(6, 21);
            this.playerListDD.Name = "playerListDD";
            this.playerListDD.Size = new System.Drawing.Size(130, 24);
            this.playerListDD.Sorted = true;
            this.playerListDD.TabIndex = 19;
            this.playerListDD.SelectedIndexChanged += new System.EventHandler(this.playerListDD_SelectedIndexChanged);
            // 
            // viewUndoB
            // 
            this.viewUndoB.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("viewUndoB.BackgroundImage")));
            this.viewUndoB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.viewUndoB.Location = new System.Drawing.Point(142, 21);
            this.viewUndoB.Name = "viewUndoB";
            this.viewUndoB.Size = new System.Drawing.Size(27, 27);
            this.viewUndoB.TabIndex = 20;
            this.viewUndoB.UseVisualStyleBackColor = true;
            this.viewUndoB.Click += new System.EventHandler(this.viewUndoB_Click);
            // 
            // otherWBRoomsLB
            // 
            this.otherWBRoomsLB.FormattingEnabled = true;
            this.otherWBRoomsLB.Location = new System.Drawing.Point(5, 51);
            this.otherWBRoomsLB.Name = "otherWBRoomsLB";
            this.otherWBRoomsLB.Size = new System.Drawing.Size(168, 72);
            this.otherWBRoomsLB.Sorted = true;
            this.otherWBRoomsLB.TabIndex = 21;
            this.otherWBRoomsLB.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.otherWBRoomsLB_ItemCheck);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.wbColorB);
            this.groupBox1.Controls.Add(this.clearAllB);
            this.groupBox1.Controls.Add(this.clearB);
            this.groupBox1.Controls.Add(this.undoB);
            this.groupBox1.Controls.Add(this.textDD);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.wbPointSize);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(291, 130);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Drawing Controls";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 17);
            this.label2.TabIndex = 20;
            this.label2.Text = "Point Size:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(102, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 17);
            this.label1.TabIndex = 19;
            this.label1.Text = "Text:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.playerListDD);
            this.groupBox2.Controls.Add(this.viewUndoB);
            this.groupBox2.Controls.Add(this.otherWBRoomsLB);
            this.groupBox2.Location = new System.Drawing.Point(309, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(187, 130);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Viewing Controls";
            // 
            // WhiteboardDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "WhiteboardDialog";
            this.Size = new System.Drawing.Size(846, 294);
            ((System.ComponentModel.ISupportInitialize)(this.wbPointSize)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColorDialog wbColorDialog;
        private System.Windows.Forms.Button wbColorB;
        private System.Windows.Forms.TrackBar wbPointSize;
        private System.Windows.Forms.ComboBox textDD;
        private System.Windows.Forms.Button undoB;
        private System.Windows.Forms.Button clearB;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem circleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem arrowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textToolStripMenuItem;
        private System.Windows.Forms.Button clearAllB;
        private System.Windows.Forms.ComboBox playerListDD;
        private System.Windows.Forms.Button viewUndoB;
        private System.Windows.Forms.CheckedListBox otherWBRoomsLB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip wbTooltip;


    }
}
