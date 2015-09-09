using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Aptima.Asim.DDD.Client.Controller;
using Aptima.Asim.DDD.Client.Whiteboard;

namespace Aptima.Asim.DDD.Client.Dialogs
{
    public partial class WhiteboardDialog : UserControl
    {
        private Form _MainForm;
        private GUIController _Controller;
        private WhiteboardWindowProperties _WhiteboardWindowProperties = new WhiteboardWindowProperties();
        private WhiteboardRoom wbRoom;
        private Object otherWBRoomLock = new object();
        private Dictionary<string, WhiteboardRoom> otherWBRooms = new Dictionary<string, WhiteboardRoom>();

        public WhiteboardRoom WBRoom
        {
            set
            {
                wbRoom = value;
            }
            get
            {
                return wbRoom;
            }
        }

        public bool AllowPropertyChanges
        {
            set
            {
                _WhiteboardWindowProperties.AllowChanges = value;
            }
        }

        public string GroupId
        {
            get
            {
                return _WhiteboardWindowProperties.GroupId;
            }
            set
            {
                _WhiteboardWindowProperties.GroupId = value;
            }
        }
        public List<string> Members
        {
            get
            {
                lock (this)
                {
                    return _WhiteboardWindowProperties.Members;
                }
            }
            set
            {
                lock (this)
                {
                    _WhiteboardWindowProperties.Members = value;
                }
            }
        }
        public List<string> SelectedMembers
        {
            get
            {
                lock (this)
                {
                    return _WhiteboardWindowProperties.SelectedMembers;
                }
            }
        }
        public string Channel = string.Empty;

        public WhiteboardDialog(Form mainForm, GUIController controller, WhiteboardRoom wbRoom)
        {
            InitializeComponent();
            if (controller == null)
            {
                throw new ArgumentNullException("Cannot pass a null GUIController");
            }
            _MainForm = mainForm;
            _Controller = controller;
            WBRoom = wbRoom;
            this.Dock = DockStyle.Fill;

            if (WBRoom != null)
            {
                wbColorB.BackColor = Color.FromArgb(WBRoom.DrawColor);
                wbRoom.DrawPointSize = wbPointSize.Value;
            }

            // Turn off the gradient in toolstrip
            ProfessionalColorTable professionalColorTable = new ProfessionalColorTable();
            professionalColorTable.UseSystemColors = true;
            toolStrip1.Renderer = new ToolStripProfessionalRenderer(professionalColorTable);

            // Set the default image for tool button drop down
            if (WBRoom != null)
            {
                SetWhiteboardToolImage(WBRoom.DrawMode);
            }

            // Setup tooltips
            wbTooltip.SetToolTip (textDD, "Enter text for use in text or line drawing control");
            wbTooltip.SetToolTip(wbColorB, "Click to choose drawing color");
            wbTooltip.SetToolTip(wbPointSize, "Use to set size for drawing objects or text");
            wbTooltip.SetToolTip(undoB, "Click to undo your last drawing operation");
            wbTooltip.SetToolTip(clearB, "Click to clear all of your drawing in this Whiteboard room");
            wbTooltip.SetToolTip(clearAllB, "Click to clear entire Whiteboard room");
            wbTooltip.SetToolTip(playerListDD, "Select a player to synchronize your map view with");
            wbTooltip.SetToolTip(viewUndoB, "Revert to your previous view");
            wbTooltip.SetToolTip(otherWBRoomsLB, "Select additional Whiteboard rooms to overlay with this room");

        }
        public DialogResult ShowProperties()
        {
            if (_WhiteboardWindowProperties.ShowDialog() == DialogResult.OK)
            {
                if (Parent != null)
                {
                    if (this.Parent is TabPage)
                    {
                        ((TabPage)Parent).Text = _WhiteboardWindowProperties.GroupId;
                    }
                }
            }
            else
            {
                if (Parent != null)
                {
                    if (this.Parent is TabPage)
                    {
                        _WhiteboardWindowProperties.GroupId = ((TabPage)Parent).Text;
                    }
                }
            }
            return _WhiteboardWindowProperties.DialogResult;
        }

        private void wbColorB_Click(object sender, EventArgs e)
        {
            if (wbColorDialog.ShowDialog() != DialogResult.Cancel)
            {
                wbColorB.BackColor = wbColorDialog.Color;
            }
            if (wbRoom != null)
            {
                wbRoom.DrawColor = wbColorDialog.Color.ToArgb();
            }
        }

        private void wbPointSize_Scroll(object sender, EventArgs e)
        {
            if (wbRoom != null)
            {
                wbRoom.DrawPointSize = wbPointSize.Value;
            }
        }

        private void textDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wbRoom != null)
            {
                wbRoom.DrawText = textDD.Text;
            }
        }

        private void textDD_TextUpdate(object sender, EventArgs e)
        {
            if (wbRoom != null)
            {
                wbRoom.DrawText = textDD.Text;
            }
        }

        private void undoB_Click(object sender, EventArgs e)
        {
            string lastObjectID;
            if (wbRoom != null)
            {
                lastObjectID = wbRoom.GetMyLastObjectID();
                if (lastObjectID != null)
                {
                    _Controller.WhiteboardUndoRequest(lastObjectID, DDD_Global.Instance.PlayerID, WBRoom.Name);
                }
            }
        }

        private void clearB_Click(object sender, EventArgs e)
        {
            if (wbRoom != null)
            {
                _Controller.WhiteboardClearRequest(DDD_Global.Instance.PlayerID, WBRoom.Name);
            }
        }

        private void SetWhiteboardToolImage(DrawModes drawMode)
        {
            if (drawMode == DrawModes.Selection)
            {
                toolStripDropDownButton1.Image = global::Aptima.Asim.DDD.Client.Properties.Resources.PointerHS;
            }
            else if (drawMode == DrawModes.Arrow)
            {
                toolStripDropDownButton1.Image = global::Aptima.Asim.DDD.Client.Properties.Resources.arrow.ToBitmap();
            }
            else if (drawMode == DrawModes.Circle)
            {
                toolStripDropDownButton1.Image = global::Aptima.Asim.DDD.Client.Properties.Resources.circle.ToBitmap();
            }
            else if (drawMode == DrawModes.Line)
            {
                toolStripDropDownButton1.Image = global::Aptima.Asim.DDD.Client.Properties.Resources.line.ToBitmap();
            }
            else if (drawMode == DrawModes.Text)
            {
                toolStripDropDownButton1.Image = global::Aptima.Asim.DDD.Client.Properties.Resources.Text;
            }
        }

        private void toolStripDropDownButton1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (WBRoom == null)
            {
                return;
            }

            // Change the drawing tool to the one selected
            if (e.ClickedItem == selectToolStripMenuItem)
            {
                WBRoom.DrawMode = DrawModes.Selection;
            }
            else if (e.ClickedItem == arrowToolStripMenuItem)
            {
                WBRoom.DrawMode = DrawModes.Arrow;
            }
            else if (e.ClickedItem == circleToolStripMenuItem)
            {
                WBRoom.DrawMode = DrawModes.Circle;
            }
            else if (e.ClickedItem == lineToolStripMenuItem)
            {
                WBRoom.DrawMode = DrawModes.Line;
            }
            else if (e.ClickedItem == textToolStripMenuItem)
            {
                WBRoom.DrawMode = DrawModes.Text;
            }

            // Display the drawing tool in the drop down button
            SetWhiteboardToolImage(WBRoom.DrawMode);

            // Change the windows cursor
            if (e.ClickedItem == selectToolStripMenuItem)
            {
                ((DDD_MainWinFormInterface)_MainForm).SetDrawCursor(false);
            }
            else
            {
                ((DDD_MainWinFormInterface)_MainForm).SetDrawCursor(true);
            }

        }

        private void textDD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                if ((textDD.Text != null) && (textDD.Text.Length > 0) &&
                    (!textDD.Items.Contains(textDD.Text)))
                {
                    textDD.Items.Add(textDD.Text);
                }
            }
        }

        public void EnableRoomOwnerControls(bool isRoomOwner)
        {
            clearAllB.Enabled = isRoomOwner;
            clearAllB.Visible = isRoomOwner;
            wbRoom.IsRoomOwner = isRoomOwner;
        }

        private void clearAllB_Click(object sender, EventArgs e)
        {
            if (wbRoom != null)
            {
                _Controller.WhiteboardClearAllRequest(DDD_Global.Instance.PlayerID, WBRoom.Name);
            }
        }

        public void AddViewUser(string user_id)
        {
            if (!playerListDD.Items.Contains(user_id))
            {
                playerListDD.Items.Add(user_id);
            }
        }

        private void playerListDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Send a message to request this users view be syncronized with
            // another user's view
            string targetUser;

            if (playerListDD.SelectedItem == null)
            {
                return;
            }

            targetUser = playerListDD.SelectedItem.ToString();
            if (string.Compare(targetUser, DDD_Global.Instance.PlayerID) == 0)
            {
                return;
            }

            if (wbRoom != null)
            {
                _Controller.WhiteboardSyncScreenViewRequest(DDD_Global.Instance.PlayerID, targetUser, WBRoom.Name);
            }
        }

        private void viewUndoB_Click(object sender, EventArgs e)
        {
            if ((wbRoom != null) && (_MainForm != null) && (_MainForm is DDD_MainWinFormInterface))
            {
                if (!wbRoom.ScreenViewInfoEmpty())
                {
                    // Clear player list drop down - Bug 4538
                    playerListDD.SelectedIndex = -1;
                }
                ((DDD_MainWinFormInterface)_MainForm).WhiteboardPopScreenView(wbRoom.Name);
            }
        }

        public void AddOtherRoom(string room_name, WhiteboardRoom wbRoom)
        {
            // Add the room to the checkbox list
            otherWBRoomsLB.Items.Add(room_name);

            // Add a link to this whiteboard room
            lock (otherWBRoomLock)
            {
                if (!otherWBRooms.ContainsKey(room_name))
                {
                    otherWBRooms.Add(room_name, wbRoom);
                }
            }
        }

        private void otherWBRoomsLB_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string room_name = otherWBRoomsLB.Items[e.Index].ToString();

            if ((room_name == null) || (room_name.Length == 0) ||
                (WBRoom == null))
            {
                return;
            }

            if (e.NewValue == CheckState.Checked)
            {
                WhiteboardRoom otherWBRoom = null;
                lock (otherWBRoomLock)
                {
                    if (otherWBRooms.ContainsKey(room_name))
                    {
                        otherWBRoom = otherWBRooms[room_name];
                        WBRoom.AddOtherRoom(room_name, otherWBRoom);
                    }
                }
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                WBRoom.RemoveOtherRoom(room_name);
            }
        }

    }
}
