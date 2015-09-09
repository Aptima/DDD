namespace VisualScenarioGenerator.Dialogs
{
    partial class Ctl_EventCommands
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
            this.AddEvent = new System.Windows.Forms.Button();
            this.DeleteEvent = new System.Windows.Forms.Button();
            this.MoveUp = new System.Windows.Forms.Button();
            this.MoveDown = new System.Windows.Forms.Button();
            this.Properties = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddEvent
            // 
            this.AddEvent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AddEvent.Location = new System.Drawing.Point(3, 0);
            this.AddEvent.Name = "AddEvent";
            this.AddEvent.Size = new System.Drawing.Size(157, 23);
            this.AddEvent.TabIndex = 0;
            this.AddEvent.Text = "Create";
            this.AddEvent.UseVisualStyleBackColor = true;
            this.AddEvent.Click += new System.EventHandler(this.AddEvent_Click);
            // 
            // DeleteEvent
            // 
            this.DeleteEvent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteEvent.Location = new System.Drawing.Point(3, 29);
            this.DeleteEvent.Name = "DeleteEvent";
            this.DeleteEvent.Size = new System.Drawing.Size(157, 23);
            this.DeleteEvent.TabIndex = 1;
            this.DeleteEvent.Text = "Delete";
            this.DeleteEvent.UseVisualStyleBackColor = true;
            this.DeleteEvent.Click += new System.EventHandler(this.DeleteEvent_Click);
            // 
            // MoveUp
            // 
            this.MoveUp.Location = new System.Drawing.Point(4, 59);
            this.MoveUp.Name = "MoveUp";
            this.MoveUp.Size = new System.Drawing.Size(75, 23);
            this.MoveUp.TabIndex = 2;
            this.MoveUp.Text = "Move Up";
            this.MoveUp.UseVisualStyleBackColor = true;
            this.MoveUp.Click += new System.EventHandler(this.MoveUp_Click);
            // 
            // MoveDown
            // 
            this.MoveDown.Location = new System.Drawing.Point(85, 59);
            this.MoveDown.Name = "MoveDown";
            this.MoveDown.Size = new System.Drawing.Size(75, 23);
            this.MoveDown.TabIndex = 3;
            this.MoveDown.Text = "Move Down";
            this.MoveDown.UseVisualStyleBackColor = true;
            this.MoveDown.Click += new System.EventHandler(this.MoveDown_Click);
            // 
            // Properties
            // 
            this.Properties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Properties.Location = new System.Drawing.Point(4, 88);
            this.Properties.Name = "Properties";
            this.Properties.Size = new System.Drawing.Size(157, 23);
            this.Properties.TabIndex = 4;
            this.Properties.Text = "Properties";
            this.Properties.UseVisualStyleBackColor = true;
            this.Properties.Click += new System.EventHandler(this.Properties_Click);
            // 
            // Ctl_EventCommands
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Properties);
            this.Controls.Add(this.MoveDown);
            this.Controls.Add(this.MoveUp);
            this.Controls.Add(this.DeleteEvent);
            this.Controls.Add(this.AddEvent);
            this.Name = "Ctl_EventCommands";
            this.Size = new System.Drawing.Size(163, 150);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddEvent;
        private System.Windows.Forms.Button DeleteEvent;
        private System.Windows.Forms.Button MoveUp;
        private System.Windows.Forms.Button MoveDown;
        private System.Windows.Forms.Button Properties;
    }
}
