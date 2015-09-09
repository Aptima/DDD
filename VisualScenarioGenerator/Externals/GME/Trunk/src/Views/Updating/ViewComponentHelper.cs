using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Web.UI.WebControls;

namespace AME.Views.View_Components
{
    public enum UpdateType { Component, Parameter, ComponentAndParameter }

    public class ViewComponentHelper : IViewComponentHelper
    {
        private IViewComponent myIView;
        private UpdateType myUpdateType = UpdateType.ComponentAndParameter;
        private Boolean myNeedsUpdate = true;
        private List<String> parameterCategories = new List<string>();
        private UpdateType latestEventFromController;

        public ViewComponentHelper(IViewComponent iv)
        {
            myIView = iv;
        }

        public ViewComponentHelper(IViewComponent iv, UpdateType updateType)
            : this(iv)
        {
            myUpdateType = updateType;
        }

        public UpdateType IViewHelperUpdateType
        {
            get { return myUpdateType; }
            set { myUpdateType = value; }
        }

        public virtual bool IViewHelperVisible
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
                else if (myIView is WebControl)
                {
                    WebControl cast = (WebControl)myIView;
                    return cast.Visible && cast.Parent != null;
                }
                else if (myIView is CustomComboToolStripItem)
                {
                    CustomCombo cast = ((CustomComboToolStripItem)myIView).CustomComboControl;
                    return cast.Visible && cast.Parent != null;
                }
                else if (myIView is ToolStripItem)
                {
                    ToolStripItem cast = (ToolStripItem)myIView;
                    return cast.Visible;
                }
                else
                {
                    throw new Exception("Not a control...");
                }
            }
        }

        public bool NeedsUpdate
        {
            get { return myNeedsUpdate; }
            set { myNeedsUpdate = value; }
        }

        public List<String> ParameterCategories
        {
            get { return parameterCategories; }
            set { parameterCategories = value; }
        }

        public void IViewHelperUpdateViewComponent(UpdateType typeOfUpdate)
        {
            latestEventFromController = typeOfUpdate;
            myIView.UpdateViewComponent();
        }

        public UpdateType LatestEventFromController
        {
            get { return latestEventFromController; }
            set { latestEventFromController = value; }
        }
        
        public void InvokeUpdate(UpdateType typeOfUpdate)
        {
            ((Control)myIView).BeginInvoke(
                new UpdateDelegate(IViewHelperUpdateViewComponent), new object[] { typeOfUpdate });
        }

        public delegate void UpdateDelegate(UpdateType typeOfUpdate);

        public virtual Boolean InvokeRequired
        {
            get
            {
                if (myIView is Control)
                {
                    return ((Control)myIView).InvokeRequired;
                }
                else if (myIView is WebControl)
                {
                    return false; // this might cause issues with WebControls created on different threads
                                  // than whoever made this call
                }
                else if (myIView is CustomComboToolStripItem)
                {
                    return ((CustomComboToolStripItem)myIView).CustomComboControl.InvokeRequired;
                }
                else if (myIView is ToolStripItem)
                {
                    return false;
                }
                else
                {
                    throw new Exception("Not a control...");
                }
            }
        }
    }
}
