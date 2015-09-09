namespace VisualScenarioGenerator.Dialogs
{
    partial class Ctl_Node_CloseChatRoomEvent
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
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.ctl_Node1 = new VisualScenarioGenerator.Dialogs.Ctl_Node();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(0, 85);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(208, 139);
            this.checkedListBox1.TabIndex = 1;
            // 
            // ctl_Node1
            // 
            this.ctl_Node1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctl_Node1.ID = "";
            this.ctl_Node1.Location = new System.Drawing.Point(0, 0);
            this.ctl_Node1.Name = "ctl_Node1";
            this.ctl_Node1.Size = new System.Drawing.Size(208, 85);
            this.ctl_Node1.TabIndex = 0;
            this.ctl_Node1.Tick = 0;
            // 
            // Ctl_Node_CloseChatRoomEvent
            // 
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.ctl_Node1);
            this.Name = "Ctl_Node_CloseChatRoomEvent";
            this.Size = new System.Drawing.Size(208, 227);
            this.ResumeLayout(false);

        }

        #endregion

        private Ctl_Node ctl_Node1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;

    }
}
