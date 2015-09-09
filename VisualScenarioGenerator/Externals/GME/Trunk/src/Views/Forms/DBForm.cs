using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AME.Views.View_Components;
using AME.Controllers;

namespace Forms
{
    // Form that shows some DataTables - useful for
    // debugging the state of the DB
    public partial class DBForm : Form, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private IController myController;

        public DBForm(IController c)
        {
            myHelper = new ViewComponentHelper(this, UpdateType.Component);

            InitializeComponent();

            myController = c;

            myController.RegisterForUpdate(this);

            this.FormClosing += new FormClosingEventHandler(DBForm_FormClosing);
        }

        // unregister for the event
        void DBForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            myController.UnregisterForUpdate(this);
        }

        void DBForm_ComponentUpdate()
        {
            this.UpdateViewComponent();
        }

        #region ViewComponentUpdate Members

        public IController Controller
        {
            get
            {
                return myController;
            }
            set
            {
                myController = value;
            }
        }

        public void UpdateViewComponent()
        {
            componentView.DataSource = ((Controller)myController).GetComponentTable();
            linkView.DataSource = ((Controller)myController).GetLinkTable();
            paramView.DataSource = ((Controller)myController).GetParameterTable();
        }

        #endregion
    }
}
