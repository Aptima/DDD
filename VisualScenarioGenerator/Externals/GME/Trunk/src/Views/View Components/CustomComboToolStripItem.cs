using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using AME.Views.View_Components;
using AME.Controllers;
using System.Windows.Forms.Design;

namespace AME.Views.View_Components {

    [ToolStripItemDesignerAvailability
    (ToolStripItemDesignerAvailability.ToolStrip |
    ToolStripItemDesignerAvailability.StatusStrip)]
    public class CustomComboToolStripItem : ToolStripControlHost, IViewComponent
    {
        private ViewComponentHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }


        public CustomComboToolStripItem() : base(new CustomCombo()) {
            myHelper = new ViewComponentHelper(this, UpdateType.Component);
        }

        public CustomCombo CustomComboControl {
            get {
                return Control as CustomCombo;
            }
        }

        #region Exposed Properties
        public int DisplayID {
            get {
                return CustomComboControl.DisplayID;
            }
            set {
                CustomComboControl.DisplayID = value;
            }
        }

        public String Type {
            get {
                return CustomComboControl.Type;
            }
            set {
                CustomComboControl.Type = value;
            }
        }

        public int SelectedID {
            get {
                return CustomComboControl.SelectedID;
            }
            set {
                CustomComboControl.SelectedID = value;
            }
        }

        public void SetSelectedId(Int32 id)
        {
            foreach (ComboItem item in CustomComboControl.Items)
            {
                if (item.MyID.Equals(id))
                {
                    CustomComboControl.SelectedItem = item;
                    CustomComboControl.SelectedID = id;
                    CustomComboControl_SelectedIDChangedEvent(CustomComboControl, id, item.MyName);
                    break;
                }
            }
        }

        public IController Controller {
            get {
                return CustomComboControl.Controller;
            }
            set {
                CustomComboControl.Controller = value;
            }
        }

        public string LinkType {
            get {
                return CustomComboControl.LinkType;
            }
            set {
                CustomComboControl.LinkType = value;
            }
        }

        public Int32 SelectedIndex
        {
            get
            {
                return CustomComboControl.SelectedIndex;
            }

            set
            {
                CustomComboControl.SelectedIndex = value;
            }
        }

        #endregion

        #region Exposed Methods
        public void UpdateViewComponent() {
            CustomComboControl.UpdateViewComponent();
        }
        #endregion

        #region Exposing Events
        protected override void OnSubscribeControlEvents(Control c) {
            // Call the base so the base events are connected.
            base.OnSubscribeControlEvents(c);

            // Cast the control to a CustomCombo control.
            CustomCombo customComboControl = (CustomCombo)c;

            // Add the event.
            customComboControl.SelectedIDChangedEvent += new CustomCombo.SelectedIDChanged(CustomComboControl_SelectedIDChangedEvent);
        }

        protected override void OnUnsubscribeControlEvents(Control c) {
            // Call the base method so the basic events are unsubscribed.
            base.OnUnsubscribeControlEvents(c);

            // Cast the control to a CustomCombo control.
            CustomCombo customComboControl = (CustomCombo)c;

            // Remove the event.
            customComboControl.SelectedIDChangedEvent -= new CustomCombo.SelectedIDChanged(CustomComboControl_SelectedIDChangedEvent);
        }

        public delegate void SelectedIDChanged(CustomCombo source, int ID, String Name);
        public event SelectedIDChanged SelectedIDChangedEvent;

        private void CustomComboControl_SelectedIDChangedEvent(CustomCombo source, int ID, string Name)
        {
            if (SelectedIDChangedEvent != null)
            {
                SelectedIDChangedEvent(this.CustomComboControl, ID, Name);
            }
        }
        #endregion
    }
}
