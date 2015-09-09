using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aptima.Asim.DDD.Client.Dialogs
{
    public partial class ColorPalette : Form
    {
        private Color selectedColor = Color.Black;

        public Color GetColor()
        {
            return selectedColor;
        }

        public void SetColor(String namedColor)
        {
            selectedColor = Color.FromName(namedColor);
        }

        public ColorPalette(String currentlySelected)
        {
            InitializeComponent();
            
            if (currentlySelected != null)
            {
                selectedColor = Color.FromName(currentlySelected);
                HighlightPanel(currentlySelected);
            }
        }

        public void HighlightPanel(string colorName)
        {
            string panelName = "panel" + colorName;
            foreach (Control c in this.Controls)
            {
                if (c.Name == panelName)
                { 
                    panel_Click(c, new EventArgs());
                    return;
                }
            }
        }

        private void panel_MouseEnter(object sender, EventArgs e)
        {
            Color c = ((Panel)sender).BackColor;
            textBoxNameDisplay.Text = c.Name;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void panel_Click(object sender, EventArgs e)
        {
            Color c = ((Panel)sender).BackColor;
            selectedColor = c;

            //highlight panel
            int x = ((Panel)sender).Location.X;
            int y = ((Panel)sender).Location.Y;

            x -= 3;
            y -= 3;
            panelHighlight.Location = new Point(x, y);
        }

        private void panel_MouseLeave(object sender, EventArgs e)
        {
            textBoxNameDisplay.Text = "";
        }

        private void panel_DoubleClick(object sender, EventArgs e)
        {
            panel_Click(sender, e);

            //this.DialogResult = DialogResult.OK;
            //this.Close();
            buttonOK_Click(this, new EventArgs());
        }
    }

    
}