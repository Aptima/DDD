using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Aptima.Asim.DDD.Client.Controller;


namespace Aptima.Asim.DDD.Client.Dialogs
{
    public partial class DM_Dialog : Form
    {
        private ICommand _commands = null;
        private delegate void HandshakeAvailablePlayersDelegate(string[] players);

        public string SelectedItem = string.Empty;
        private string desired = string.Empty;
        List<string> playerlist = new List<string>();

        public DM_Dialog(ICommand command)
        {
            InitializeComponent();
            _commands = command;
            

        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                SelectedItem = (string)listBox1.Items[listBox1.SelectedIndex];
                desired = SelectedItem;
                DialogResult = DialogResult.OK;
            }
        }

        private void DM_Dialog_Load(object sender, EventArgs e)
        {
            //Console.WriteLine("HandshakeGUIRegister {0}", DDD_Global.Instance.TerminalID);
            //_commands.HandshakeGUIRegister(DDD_Global.Instance.TerminalID);
        }
        public void HandshakeAvailablePlayers(string[] players)
        {
            playerlist.Clear();
            playerlist.AddRange(players);
            if (desired != string.Empty && !playerlist.Contains(desired))
            {
                DialogResult = DialogResult.OK;
                return;
            }

            if (!InvokeRequired)
            {
                    listBox1.Items.Clear();
                    foreach (string s in players)
                    {
                        listBox1.Items.Add(s);
                    }
            }
            else
            {
                BeginInvoke(new HandshakeAvailablePlayersDelegate(HandshakeAvailablePlayers), players);
            }
        }

        private void DM_Dialog_Paint(object sender, PaintEventArgs e)
        {
        }

    }
}