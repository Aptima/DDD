using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using AME.Views.View_Component_Packages;

namespace AME.Views.View_Components
{
    public class ViewPanelHelper : IViewComponentHelper
    {
        private IViewComponentPanel myIView;
        private UpdateType myUpdateType = UpdateType.ComponentAndParameter;
        private Boolean myNeedsUpdate = true;
        private List<String> parameterCategories = new List<String>();
        private UpdateType latestEventFromController;

        public ViewPanelHelper(IViewComponentPanel iv)
        {
            myIView = iv;
        }

        public ViewPanelHelper(IViewComponentPanel iv, UpdateType updateType)
            : this(iv)
        {
            myUpdateType = updateType;
        }

        public UpdateType IViewHelperUpdateType
        {
            get { return myUpdateType; }
            set { myUpdateType = value; }
        }

        public bool IViewHelperVisible
        {
            get
            {
                if (myIView is Form)
                {
                    Control cast = (Control)myIView;
                    return cast.Visible;
                }
                else if (myIView is Control)
                {
                    Control cast = (Control)myIView;
                    return cast.Visible && cast.Parent != null;
                }
                else if (myIView is CustomComboToolStripItem)
                {
                    CustomCombo cast = ((CustomComboToolStripItem)myIView).CustomComboControl;
                    return cast.Visible && cast.Parent != null;
                }
                else
                {
                    throw new Exception("Not a control...");
                }
            }
        }

        public List<String> ParameterCategories
        {
            get { return parameterCategories; }
            set { parameterCategories = value; }
        }

        public bool NeedsUpdate
        {
            get { return myNeedsUpdate; }
            set { myNeedsUpdate = value; }
        }

        public UpdateType LatestEventFromController
        {
            get { return latestEventFromController; }
            set { latestEventFromController = value; }
        }

        public void IViewHelperUpdateViewComponent(UpdateType typeOfUpdate)
        {
            latestEventFromController = typeOfUpdate;
            myIView.UpdateViewComponent();
        }

        public void InvokeUpdate(UpdateType typeOfUpdate)
        {
            ((Control)myIView).BeginInvoke(
                new UpdateDelegate(IViewHelperUpdateViewComponent), new object[] { typeOfUpdate });
        }

        public delegate void UpdateDelegate(UpdateType typeOfUpdate);

        public Boolean InvokeRequired
        {
            get
            {
                if (myIView is Control)
                {
                    return ((Control)myIView).InvokeRequired;
                }
                else
                {
                    throw new Exception("Not a control...");
                }
            }
        } 
    }
}
