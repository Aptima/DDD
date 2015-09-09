using System.Windows.Forms;
using AME.Views.View_Components;
using AME.Controllers;
using Forms;
using System.Drawing;

namespace AME.Views.View_Components
{
    public class CustomDeleteButtonToolStripItem : ToolStripButton
    {
        private IController myController;

        public CustomDeleteButtonToolStripItem()
            : base()
        {
            this.AutoSize = true;
        }

        private int m_DeleteID;

        public int DeleteID
        {
            get { return m_DeleteID; }
            set { m_DeleteID = value; }
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
            //
        }

        #endregion
    }
}
