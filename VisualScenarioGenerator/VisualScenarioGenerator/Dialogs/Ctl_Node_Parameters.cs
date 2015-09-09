using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace VisualScenarioGenerator.Dialogs
{
    public partial class Ctl_Node_Parameters : UserControl, ICtl_ContentPane__OutboundUpdate
    {

        //These have to be declared publicaly because they are automatically private to the control, and 
        // the control will be used by a higher level control
        public NumericUpDown NudLaunchDuration
        {
            get { return nudLaunchDuration; }
            set { nudLaunchDuration = value; }
        }
        public NumericUpDown NudDockingDuration
        {
            get { return nudDockingDuration; }
            set { nudDockingDuration = value; }
        }
        public NumericUpDown NudTimeToAttack
        {
            get { return nudTimeToAttack; }
            set { nudTimeToAttack = value; }
        }
        public CheckedListBox ClbxSenses
        {
            get { return clbxSenses; }
            set { clbxSenses = value; }
        }

        public CheckedListBox ClbxCapabilities
        {
            get { return clbxCapabilities; }
            set { clbxCapabilities = value; }
        }

        public CheckedListBox ClbxEmitters
        {
            get { return clbxEmitters; }
            set { clbxEmitters = value; }
        }
        public Button CtnCreateEmitter
        {
            get { return ctnCreateEmitter; }
            set { ctnCreateEmitter = value; }
        }
        public NonNegDecimal NndMaximumSpeed
        {
            get { return nndMaximumSpeed; }
            set { nndMaximumSpeed = value; }
        }
        public NonNegDecimal NndFuelCapacity
        {
            get { return nndFuelCapacity; }
            set { nndFuelCapacity = value; }
        }
        public NonNegDecimal NndInitialFuelLoad
        {
            get { return nndInitialFuelLoad; }
            set { nndInitialFuelLoad = value; }
        }
        public NonNegDecimal NndFuelUseRate
        {
            get { return nndFuelUseRate; }
            set { nndFuelUseRate = value; }
        }
        public ListBox LbxDepletionState
        {
            get { return lbxDepletionState; }
            set { lbxDepletionState = value; }
        }
        public CheckBox CkStealable
        {
            get { return ckStealable; }
            set { ckStealable = value; }
        }
        public Ctl_Singletons SngSingleton_1
        {
            get { return sngSingleton_1; }
            set { sngSingleton_1 = value; }
        }
        public Ctl_Singletons SngSingleton_3
        {
            get { return sngSingleton_3; }
            set { sngSingleton_3 = value; }
        }
        public Ctl_Singletons SngSingleton_2
        {
            get { return sngSingleton_2; }
            set { sngSingleton_2 = value; }
        }
  
        public Ctl_Combos CmbCombo_1
        {
            get { return cmbCombo_1; }
            set { cmbCombo_1 = value; }
        }
        public Ctl_Combos CmbCombo_2
        {
            get { return cmbCombo_2; }
            set { cmbCombo_2 = value; }
        }





        private Ctl_Singletons []singletons=new Ctl_Singletons[3];
        private Ctl_Combos []combos=new Ctl_Combos[2];
        public Ctl_Node_Parameters()
        {
            InitializeComponent();
            singletons[0] = sngSingleton_1;
            singletons[1]=sngSingleton_2;
            singletons[2] = sngSingleton_3;
            combos[0] = cmbCombo_1;
            combos[1] = cmbCombo_2;
        }



        #region IVSG_ControlStateOutboundUpdate Members

        void ICtl_ContentPane__OutboundUpdate.Update(Control control, object object_data)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
