using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using AME.Controllers;

namespace AME.Views.View_Components {

    public partial class DiagramGridPanel : UserControl, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private String linkName = String.Empty;
        
        public DiagramGridPanel() 
        {
            myHelper = new ViewComponentHelper(this);

            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the text for the label.  If the text is an empty string or
        /// null, the label becomes infisible.
        /// </summary>
        /// <value>The text for the label.</value>
        public String Label {
            get {
                return this.label.Text;
            }
            set {
                if (value == null || value.Length == 0) {
                    this.label.Text = String.Empty;
                    this.label.Visible = false;
                }
                else {
                    this.label.Text = value;
                    this.label.Visible = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name attribute of the Link.
        /// </summary>
        /// <value>The name of the link.</value>
        public String LinkName {
            get {
                return this.linkName;
            }
            set {
                this.linkName = value;
            }
        }

        #region IViewComponent Members


        public void UpdateViewComponent() {

            this.dataGridView.UpdateViewComponent();
            //this.label.Dock = DockStyle.Top;
            //this.dataGridView.Dock = DockStyle.Fill;
            
            //if (this.SelectedID < 1)
            //    this.Label = null;
            //else {

            //    if (this.dataGridView.Rows.Count > 0)
            //        this.Label = this.LinkName + " Matrix View";
            //    else
            //        this.Label = null;

            //}
        }

        public IController Controller {
            get {
                return this.dataGridView.Controller;
            }
            set {
                this.dataGridView.Controller = value;
            }
        }        
        #endregion

        #region ParameterTable Members

        public int SelectedID {
            get {
                return this.dataGridView.SelectedID;
            }
            set {
                this.dataGridView.SelectedID = value;
            }
        }

        #endregion

        #region DiagramGrid Members

        public String ParameterName {
            get {
                return this.dataGridView.ParameterName;
            }
            set {
                this.dataGridView.ParameterName = value;
            }
        }

        public string LinkType {
            get {
                return this.dataGridView.LinkType;
            }
            set {
                this.dataGridView.LinkType = value;
            }
        }

        public int DisplayID
        {
            get
            {
                return this.dataGridView.DisplayID;
            }
            set
            {
                this.dataGridView.DisplayID = value;
            }
        }

        #endregion



    }
}
