using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AME.Controllers;
using AME;
using System.IO;

namespace AME.Views.View_Components
{
    public partial class EnvironmentPropertiesForm : Form, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private IController controller;
        private Int32 rootId;

        public Int32 RootId
        {
            get 
            { 
                return rootId;
            }

            set 
            { 
                rootId = value;
                setParameterIds(rootId);
            }
        }

        public String ImageLocation
        {
            get
            {
                return textBoxImageLocation.Text;
            }

            set
            {
                textBoxImageLocation.Text = value;
                textBoxImageLocation.SetParameterValue();
            }
        }

        public String XPixel
        {
            get
            {
                return textBoxXPixel.Text;
            }

            set
            {
                textBoxXPixel.Text = value;
                textBoxXPixel.SetParameterValue();
            }
        }

        public String XRotation
        {
            get
            {
                return textBoxXRotation.Text;
            }

            set
            {
                textBoxXRotation.Text = value;
                textBoxXRotation.SetParameterValue();
            }
        }

        public String YRotation
        {
            get
            {
                return textBoxYRotation.Text;
            }

            set
            {
                textBoxYRotation.Text = value;
                textBoxYRotation.SetParameterValue();
            }
        }

        public String YPixel
        {
            get
            {
                return textBoxYPixel.Text;
            }

            set
            {
                textBoxYPixel.Text = value;
                textBoxYPixel.SetParameterValue();
            }
        }

        public String Easting
        {
            get
            {
                return textBoxEasting.Text;
            }

            set
            {
                textBoxEasting.Text = value;
                textBoxEasting.SetParameterValue();
            }
        }

        public String Northing
        {
            get
            {
                return textBoxNorthing.Text;
            }

            set
            {
                textBoxNorthing.Text = value;
                textBoxNorthing.SetParameterValue();
            }
        }

        public Boolean UseTransformations
        {
            get
            {
                return checkBoxUseTransformations.Checked;
            }

            set
            {
                checkBoxUseTransformations.Checked = value;
            }
        }

        public EnvironmentPropertiesForm()
        {
            myHelper = new ViewComponentHelper(this,UpdateType.Parameter);

            InitializeComponent();
        }

        private void setParameterIds(Int32 id)
        {
            textBoxImageLocation.ComponentId = id;
            textBoxXPixel.ComponentId = id;
            textBoxXRotation.ComponentId = id;
            textBoxYRotation.ComponentId = id;
            textBoxYPixel.ComponentId = id;
            textBoxEasting.ComponentId = id;
            textBoxNorthing.ComponentId = id;
        }

        private void setParameterControllers(IController c)
        {
            textBoxImageLocation.Controller = c;
            textBoxXPixel.Controller = c;
            textBoxXRotation.Controller = c;
            textBoxYRotation.Controller = c;
            textBoxYPixel.Controller = c;
            textBoxEasting.Controller = c;
            textBoxNorthing.Controller = c;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = this.controller.DataPath;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result.Equals(DialogResult.OK))
            {
                // do update
                textBoxImageLocation.Text = openFileDialog1.FileName;
                textBoxImageLocation.SetParameterValue();

                // Check for world file in same dir as selected file.
                FileInfo imageFile = new FileInfo(openFileDialog1.FileName);
                String worldFilename;
                switch (imageFile.Extension)
                {
                    case ".tif":
                    case ".tiff":
                        try
                        {
                            worldFilename = Path.ChangeExtension(imageFile.FullName, ".tfw");
                            if (File.Exists(worldFilename))
                            {
                                using (StreamReader worldFileReader = new StreamReader(worldFilename))
                                {
                                    textBoxXPixel.Text = worldFileReader.ReadLine();
                                    textBoxXPixel.SetParameterValue();

                                    textBoxXRotation.Text = worldFileReader.ReadLine();
                                    textBoxXRotation.SetParameterValue();

                                    textBoxYRotation.Text = worldFileReader.ReadLine();
                                    textBoxYRotation.SetParameterValue();

                                    textBoxYPixel.Text = worldFileReader.ReadLine();
                                    textBoxYPixel.SetParameterValue();

                                    textBoxEasting.Text = worldFileReader.ReadLine();
                                    textBoxEasting.SetParameterValue();

                                    textBoxNorthing.Text = worldFileReader.ReadLine();
                                    textBoxNorthing.SetParameterValue();

                                    checkBoxUseTransformations.Checked = true;
                                    doChecked(checkBoxUseTransformations.Checked);
                                }
                            }
                            else
                            {
                                textBoxXPixel.Text = "0.0";
                                textBoxXPixel.SetParameterValue();

                                textBoxXRotation.Text = "0.0";
                                textBoxXRotation.SetParameterValue();

                                textBoxYRotation.Text = "0.0";
                                textBoxYRotation.SetParameterValue();

                                textBoxYPixel.Text = "0.0";
                                textBoxYPixel.SetParameterValue();

                                textBoxEasting.Text = "0.0";
                                textBoxEasting.SetParameterValue();

                                textBoxNorthing.Text = "0.0";
                                textBoxNorthing.SetParameterValue();

                                checkBoxUseTransformations.Checked = false;
                                doChecked(checkBoxUseTransformations.Checked);
                            }

                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message, "Error");
                        }
                        break;

                    case ".jpg":
                    case ".jpeg":
                        try
                        {
                            worldFilename = Path.GetFullPath(imageFile.DirectoryName + imageFile.Name + ".jgw");
                            if (File.Exists(worldFilename))
                            {
                                using (StreamReader worldFileReader = new StreamReader(worldFilename))
                                {
                                    textBoxXPixel.Text = worldFileReader.ReadLine();
                                    textBoxXPixel.SetParameterValue();

                                    textBoxXRotation.Text = worldFileReader.ReadLine();
                                    textBoxXRotation.SetParameterValue();

                                    textBoxYRotation.Text = worldFileReader.ReadLine();
                                    textBoxYRotation.SetParameterValue();

                                    textBoxYPixel.Text = worldFileReader.ReadLine();
                                    textBoxYPixel.SetParameterValue();

                                    textBoxEasting.Text = worldFileReader.ReadLine();
                                    textBoxEasting.SetParameterValue();

                                    textBoxNorthing.Text = worldFileReader.ReadLine();
                                    textBoxNorthing.SetParameterValue();

                                    checkBoxUseTransformations.Checked = true;
                                    doChecked(checkBoxUseTransformations.Checked);
                                }
                            }
                            else
                            {
                                textBoxXPixel.Text = "0.0";
                                textBoxXPixel.SetParameterValue();

                                textBoxXRotation.Text = "0.0";
                                textBoxXRotation.SetParameterValue();

                                textBoxYRotation.Text = "0.0";
                                textBoxYRotation.SetParameterValue();

                                textBoxYPixel.Text = "0.0";
                                textBoxYPixel.SetParameterValue();

                                textBoxEasting.Text = "0.0";
                                textBoxEasting.SetParameterValue();

                                textBoxNorthing.Text = "0.0";
                                textBoxNorthing.SetParameterValue();

                                checkBoxUseTransformations.Checked = false;
                                doChecked(checkBoxUseTransformations.Checked);
                            }

                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message, "Error");
                        }
                        break;
                }
            }
        }

        #region IViewComponent Members

        public AME.Controllers.IController Controller
        {
            get
            {
                return controller;
            }
            set
            {
                controller = value;
                setParameterControllers(controller);
            }
        }

        public void UpdateViewComponent()
        {
            textBoxImageLocation.UpdateViewComponent();
            textBoxXPixel.UpdateViewComponent();
            textBoxXRotation.UpdateViewComponent();
            textBoxYRotation.UpdateViewComponent();
            textBoxYPixel.UpdateViewComponent();
            textBoxEasting.UpdateViewComponent();
            textBoxNorthing.UpdateViewComponent();
        }

        #endregion

        private void checkBoxUseTransformations_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            doChecked(cb.Checked);
        }

        private void doChecked(Boolean checkedVlaue)
        {
            textBoxImageLocation.Enabled = checkedVlaue;
            textBoxXPixel.Enabled = checkedVlaue;
            textBoxXRotation.Enabled = checkedVlaue;
            textBoxYRotation.Enabled = checkedVlaue;
            textBoxYPixel.Enabled = checkedVlaue;
            textBoxEasting.Enabled = checkedVlaue;
            textBoxNorthing.Enabled = checkedVlaue;
        }
    }
}