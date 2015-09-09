using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Aptima.Asim.DDD.Client.Controller;

namespace Aptima.Asim.DDD.Client.Dialogs
{
    public partial class ChatDialog : UserControl
    {
        private GUIController _Controller;
        private ChatWindowProperties _ChatWindowProperties = new ChatWindowProperties();

        private bool m_currentlyTyping;

        public bool AllowPropertyChanges
        {
            set
            {
                _ChatWindowProperties.AllowChanges = value;
            }
        }

        public bool EnableProperties
        {
            get
            {
                return button1.Enabled;
            }
            set
            {
                button1.Enabled = value;
            }
        }
        public int MessageCount = 0;
        public string GroupId
        {
            get
            {
                return _ChatWindowProperties.GroupId;
            }
            set
            {
                _ChatWindowProperties.GroupId = value;
            }
        }
        public List<string> Members
        {
            get
            {
                lock (this)
                {
                    return _ChatWindowProperties.Members;
                }
            }
            set
            {
                lock (this)
                {
                    _ChatWindowProperties.Members = value;
                }
            }
        }
        public List<string> SelectedMembers
        {
            get
            {
                lock (this)
                {
                    return _ChatWindowProperties.SelectedMembers;
                }
            }
        }
        public string Channel = string.Empty;

        public ChatDialog(GUIController controller)
        {
            InitializeComponent();
            if (controller == null)
            {
                throw new ArgumentNullException("Cannot pass a null GUIController");
            }
            _Controller = controller;
            this.Dock = DockStyle.Fill;
            m_currentlyTyping = false;
        }
        public void WriteLine(string message)
        {
            TextChatWindow.SelectedText += message + "\n";
            TextChatWindow.ScrollToCaret();
            if (Parent != null)
            {
                if (Parent is TabPage)
                {
                    if (((TabControl)((TabPage)Parent).Parent).SelectedTab != ((TabPage)Parent))
                    {
                        MessageCount++;
                        ((TabPage)Parent).Text = string.Format("{0}: {1}", GroupId, MessageCount);
                    }
                }
            }
        }
        public void WriteLine(string message, Color color)
        {
            TextChatWindow.SelectedRtf += string.Format("{\\colortbl ;\\red{0}\\green{1}\\blue{2};}{3}\n", color.R, color.G, color.B, message);
            TextChatWindow.ScrollToCaret();
            if (Parent != null)
            {
                if (Parent is TabPage)
                {
                    if (((TabControl)((TabPage)Parent).Parent).SelectedTab != ((TabPage)Parent))
                    {
                        MessageCount++;
                        ((TabPage)Parent).Text = string.Format("{0}: {1}", GroupId, MessageCount);
                    }
                }
            }
        }
        public DialogResult ShowProperties()
        {
            if (_ChatWindowProperties.ShowDialog() == DialogResult.OK)
            {
                if (Parent != null)
                {
                    if (this.Parent is TabPage)
                    {
                        ((TabPage)Parent).Text = _ChatWindowProperties.GroupId;
                    }
                } 
            }
            else
            {
                if (Parent != null)
                {
                    if (this.Parent is TabPage)
                    {
                        _ChatWindowProperties.GroupId = ((TabPage)Parent).Text;
                    }
                }
            }
            return _ChatWindowProperties.DialogResult;
        }
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Return) && (textBox1.Text.Length > 0))
            {
                _Controller.TextChatRequest(DDD_Global.Instance.PlayerID, textBox1.Text, "TEAM", this.Channel);
                textBox1.Text = string.Empty;
                m_currentlyTyping = false;
            }
            else if (m_currentlyTyping == false)
            {
                m_currentlyTyping = true;
                //Console.WriteLine(String.Format("Starting chat request by {0} for {1}", DDD_Global.Instance.PlayerID, this.Channel));
                _Controller.BeginTextChatRequest(DDD_Global.Instance.PlayerID, this.Channel);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                _Controller.TextChatRequest(DDD_Global.Instance.PlayerID, textBox1.Text, "TEAM", this.Channel);
                textBox1.Text = string.Empty;
                m_currentlyTyping = false;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ShowProperties();
        }

    }
}
