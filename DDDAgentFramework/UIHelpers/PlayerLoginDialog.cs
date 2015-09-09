using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Aptima.Asim.DDD.DDDAgentFramework;

namespace Aptima.Asim.DDD.DDDAgentFramework.UIHelpers
{
    public partial class PlayerLoginDialog : Form
    {
        String m_selectedPlayer;
        DDDServerConnection m_serverConnection = null;
        public PlayerLoginDialog(ref DDDServerConnection serverConnection)
        {
            InitializeComponent();
            m_serverConnection = serverConnection;
            m_selectedPlayer = String.Empty;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            m_selectedPlayer = (String)playerListBox.SelectedItem;
            if (m_selectedPlayer == null)
            {
                MessageBox.Show("Must select a player.");
                return;
            }
            m_serverConnection.SetLocalPlayer(m_selectedPlayer);
        }

        private void ServerConnectDialog_Load(object sender, EventArgs e)
        {
            m_serverConnection.RequestPlayers();
            playerListTimer.Enabled = true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void playerListTimer_Tick(object sender, EventArgs e)
        {
            m_serverConnection.ProcessEvents();
            switch (m_serverConnection.State)
            {
                case DDDServerConnection.SessionStateType.WAITING_FOR_PLAYERS:
                    m_selectedPlayer = (String)playerListBox.SelectedItem;
                    playerListBox.Items.Clear();
                    List<String> availablePlayers = m_serverConnection.Players;
                    foreach (String dm in availablePlayers)
                    {
                        playerListBox.Items.Add(dm);
                    }
                    playerListBox.SelectedItem = m_selectedPlayer;
                    break;
                case DDDServerConnection.SessionStateType.LOGGED_IN:
                    playerListTimer.Enabled = false;
                    DialogResult = DialogResult.OK;
                    break;
            }
        }

        private void playerListBox_DoubleClick(object sender, EventArgs e)
        {
            m_selectedPlayer = (String)playerListBox.SelectedItem;
            if (m_selectedPlayer == null)
            {
                MessageBox.Show("Must select a player.");
                return;
            }
            m_serverConnection.SetLocalPlayer(m_selectedPlayer);
        }
    }
}