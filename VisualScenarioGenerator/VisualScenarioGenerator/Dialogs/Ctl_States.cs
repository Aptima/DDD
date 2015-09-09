using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using VisualScenarioGenerator.VSGPanes;


namespace VisualScenarioGenerator.Dialogs
{
    public partial class Ctl_States : Ctl_ContentPaneControl
    {
      
        private StateDataStruct _datastore = StateDataStruct.Empty();
        public static ObjectTypeLists<StateDataStruct> States = new ObjectTypeLists<StateDataStruct>();

        public Ctl_States()
        {
            InitializeComponent();
        }
        public override void Update(object object_data)
        {
            if (object_data != null)
            {
                StateDataStruct data = (StateDataStruct)object_data;

                string[] splitName = data.ID.Split('+');

                txtName.Text = splitName[0];
                List<string> speciesNames = Ctl_Species.Species.GetNames();
                lbxSpecies.Items.Clear();
                for (int i = 0; i < speciesNames.Count; i++)
                    lbxIconSelectIcon.Items.Add(speciesNames[i]);
                lbxSpecies.SelectedValue = splitName[1];
            // Handle Icon ...
            // 
                ctlParameters.NndFuelCapacity.Value = data.Parameters.FuelCapacity;
                ctlParameters.NudLaunchDuration.Value = data.Parameters.LaunchDuration;
                ctlParameters.NudDockingDuration.Value = data.Parameters.DockingDuration;
                ctlParameters.NndMaximumSpeed.Value = data.Parameters.MaximumSpeed;
                ctlParameters.NndFuelUseRate.Value = data.Parameters.FuelUseRate;
                 ctlParameters.CkStealable.Checked = data.Parameters.Stealable;

                 #region singletons
                 ctlParameters.SngSingleton_1.LbxCapability.Items.Clear();
    List<string> capabilitiesNames = Ctl_Capabilities.Capabilities.GetNames();
    for (int i = 0; i < capabilitiesNames.Count; i++)
    {
        ctlParameters.SngSingleton_1.LbxCapability.Items.Add(capabilitiesNames[i]);
        ctlParameters.SngSingleton_2.LbxCapability.Items.Add(capabilitiesNames[i]);
        ctlParameters.SngSingleton_3.LbxCapability.Items.Add(capabilitiesNames[i]);

    }
    ctlParameters.SngSingleton_1.LbxCapability.SelectedValue = data.Parameters.Singletons[0].Capability;
    ctlParameters.SngSingleton_2.LbxCapability.SelectedValue = data.Parameters.Singletons[1].Capability;
    ctlParameters.SngSingleton_3.LbxCapability.SelectedValue = data.Parameters.Singletons[2].Capability;
    ctlParameters.SngSingleton_1.NndRange_1.Value = data.Parameters.Singletons[0].Transition_1.Range;
    ctlParameters.SngSingleton_2.NndRange_1.Value = data.Parameters.Singletons[1].Transition_1.Range;
    ctlParameters.SngSingleton_3.NndRange_1.Value = data.Parameters.Singletons[2].Transition_1.Range;
    ctlParameters.SngSingleton_1.NndRange_2.Value = data.Parameters.Singletons[0].Transition_1.Range;
    ctlParameters.SngSingleton_2.NndRange_2.Value = data.Parameters.Singletons[1].Transition_1.Range;
    ctlParameters.SngSingleton_3.NndRange_2.Value = data.Parameters.Singletons[2].Transition_1.Range;
    ctlParameters.SngSingleton_1.NudIntensity_1.Value = data.Parameters.Singletons[0].Transition_1.Intensity;
    ctlParameters.SngSingleton_2.NudIntensity_1.Value = data.Parameters.Singletons[1].Transition_1.Intensity;
    ctlParameters.SngSingleton_3.NudIntensity_1.Value = data.Parameters.Singletons[2].Transition_1.Intensity;
    ctlParameters.SngSingleton_1.NudIntensity_2.Value = data.Parameters.Singletons[0].Transition_1.Intensity;
    ctlParameters.SngSingleton_2.NudIntensity_2.Value = data.Parameters.Singletons[1].Transition_1.Intensity;
    ctlParameters.SngSingleton_3.NudIntensity_2.Value = data.Parameters.Singletons[2].Transition_1.Intensity;
    ctlParameters.SngSingleton_1.NudProbability_1.Value = data.Parameters.Singletons[0].Transition_1.Probability;
    ctlParameters.SngSingleton_2.NudProbability_1.Value = data.Parameters.Singletons[1].Transition_1.Probability;
    ctlParameters.SngSingleton_3.NudProbability_1.Value = data.Parameters.Singletons[2].Transition_1.Probability;
    ctlParameters.SngSingleton_1.NudProbability_2.Value = data.Parameters.Singletons[0].Transition_1.Probability;
    ctlParameters.SngSingleton_2.NudProbability_2.Value = data.Parameters.Singletons[1].Transition_1.Probability;
    ctlParameters.SngSingleton_3.NudProbability_2.Value = data.Parameters.Singletons[2].Transition_1.Probability;

    List<string> stateNames = Ctl_States.States.GetNames(); //but restrict to current species
    for (int i = stateNames.Count-1;i>=0; i++)
    {
        splitName = stateNames[i].Split('+');
        if (splitName[1] != (string)(lbxSpecies.SelectedValue))
            stateNames.RemoveAt(i);

    }
    ctlParameters.SngSingleton_1.LbxState_1.Items.Clear();
    ctlParameters.SngSingleton_1.LbxState_2.Items.Clear();
    ctlParameters.SngSingleton_2.LbxState_1.Items.Clear();
    ctlParameters.SngSingleton_2.LbxState_2.Items.Clear();
    ctlParameters.SngSingleton_3.LbxState_1.Items.Clear();
    ctlParameters.SngSingleton_3.LbxState_2.Items.Clear();
    ctlParameters.LbxDepletionState.Items.Clear();
    for (int i = 0; i < stateNames.Count; i++)
    {
        ctlParameters.SngSingleton_1.LbxState_1.Items.Add(stateNames[i]);
        ctlParameters.SngSingleton_1.LbxState_2.Items.Add(stateNames[i]);
        ctlParameters.SngSingleton_2.LbxState_1.Items.Add(stateNames[i]);
        ctlParameters.SngSingleton_2.LbxState_2.Items.Add(stateNames[i]);
        ctlParameters.SngSingleton_3.LbxState_1.Items.Add(stateNames[i]);
        ctlParameters.SngSingleton_3.LbxState_2.Items.Add(stateNames[i]);
        ctlParameters.LbxDepletionState.Items.Add(stateNames[i]);
    }
    ctlParameters.SngSingleton_1.LbxState_1.SelectedValue = data.Parameters.Singletons[0].Transition_1.State;
    ctlParameters.SngSingleton_1.LbxState_2.SelectedValue = data.Parameters.Singletons[0].Transition_2.State;
    ctlParameters.SngSingleton_2.LbxState_1.SelectedValue = data.Parameters.Singletons[1].Transition_1.State;
    ctlParameters.SngSingleton_2.LbxState_2.SelectedValue = data.Parameters.Singletons[1].Transition_2.State;
    ctlParameters.SngSingleton_3.LbxState_1.SelectedValue = data.Parameters.Singletons[2].Transition_1.State;
    ctlParameters.SngSingleton_3.LbxState_2.SelectedValue = data.Parameters.Singletons[2].Transition_2.State;
    ctlParameters.LbxDepletionState.SelectedValue = data.Parameters.FuelDepletionState;
                 #endregion


    #region Combos

    ctlParameters.CmbCombo_1.LbxCapability_1.Items.Clear();
    ctlParameters.CmbCombo_2.LbxCapability_1.Items.Clear();
    for (int i = 0; i < capabilitiesNames.Count; i++)
    {
        ctlParameters.CmbCombo_1.LbxCapability_1.Items.Add(capabilitiesNames[i]);
        ctlParameters.CmbCombo_1.LbxCapability_2.Items.Add(capabilitiesNames[i]);
    }

    ctlParameters.CmbCombo_1.LbxCapability_1.SelectedValue = data.Parameters.Combos[0].Contribution_1.Capability;
      ctlParameters.CmbCombo_1.LbxCapability_2.SelectedValue = data.Parameters.Combos[0].Contribution_2.Capability;
      ctlParameters.CmbCombo_2.LbxCapability_1.SelectedValue = data.Parameters.Combos[1].Contribution_1.Capability;
      ctlParameters.CmbCombo_2.LbxCapability_2.SelectedValue = data.Parameters.Combos[1].Contribution_2.Capability;

      ctlParameters.CmbCombo_1.NndRange_1.Value = data.Parameters.Combos[0].Contribution_1.Range;
      ctlParameters.CmbCombo_1.NndRange_2.Value = data.Parameters.Combos[0].Contribution_2.Range;
      ctlParameters.CmbCombo_2.NndRange_1.Value = data.Parameters.Combos[1].Contribution_1.Range;
      ctlParameters.CmbCombo_2.NndRange_2.Value = data.Parameters.Combos[1].Contribution_2.Range;

      ctlParameters.CmbCombo_1.NudEffect_1.Value = data.Parameters.Combos[0].Contribution_1.Effect;
      ctlParameters.CmbCombo_1.NudEffect_2.Value = data.Parameters.Combos[0].Contribution_2.Effect;
      ctlParameters.CmbCombo_2.NudEffect_1.Value = data.Parameters.Combos[1].Contribution_1.Effect;
      ctlParameters.CmbCombo_2.NudEffect_2.Value = data.Parameters.Combos[1].Contribution_2.Effect;

      ctlParameters.CmbCombo_1.NudProbability_1.Value = data.Parameters.Combos[0].Contribution_1.Probability;
      ctlParameters.CmbCombo_1.NudProbability_2.Value = data.Parameters.Combos[0].Contribution_2.Probability;
      ctlParameters.CmbCombo_2.NudProbability_1.Value = data.Parameters.Combos[1].Contribution_1.Probability;
       ctlParameters.CmbCombo_2.NudProbability_2.Value = data.Parameters.Combos[1].Contribution_2.Probability;

                ctlParameters.CmbCombo_1.LbxNewState.Items.Clear();
                ctlParameters.CmbCombo_2.LbxNewState.Items.Clear();
                for(int i=0;i<stateNames.Count;i++)
                {
                    ctlParameters.CmbCombo_1.LbxNewState.Items.Add(stateNames[i]);
                     ctlParameters.CmbCombo_2.LbxNewState.Items.Add(stateNames[i]);

                }
      if( stateNames.Contains(data.Parameters.Combos[0].NewState))
      ctlParameters.CmbCombo_1.LbxNewState.SelectedValue = data.Parameters.Combos[0].NewState;
         if( stateNames.Contains(data.Parameters.Combos[1].NewState))
                ctlParameters.CmbCombo_2.LbxNewState.SelectedValue = data.Parameters.Combos[1].NewState;
    #endregion



       ctlParameters.ClbxSenses.Items.Clear();
       List<string> sensorNames = Ctl_Sensors.Sensors.GetNames();
       for (int i = 0; i < sensorNames.Count; i++)
       {

           if (data.Parameters.Sensors.Contains(sensorNames[i]))
           {
               ctlParameters.ClbxSenses.Items.Add(sensorNames[i], true);
           }
           else
           {
               ctlParameters.ClbxSenses.Items.Add(sensorNames[i], false);
           }
              
       }

       ctlParameters.ClbxEmitters.Items.Clear();
       List<string> emitterNames = Ctl_Emitters.Emitters.GetNames();
       for (int i = 0; i < emitterNames.Count; i++)
       {

           if (data.Parameters.Emitters.Contains(emitterNames[i]))
           {
               ctlParameters.ClbxEmitters.Items.Add(emitterNames[i], true);
           }
           else
           {
               ctlParameters.ClbxEmitters.Items.Add(emitterNames[i], false);
           }

       }


       ctlParameters.ClbxCapabilities.Items.Clear();
       List<string> capabilityNames = Ctl_Capabilities.Capabilities.GetNames();
       for (int i = 0; i < capabilityNames.Count; i++)
       {

           if (data.Parameters.Capabilities.Contains(capabilityNames[i]))
           {
               ctlParameters.ClbxCapabilities.Items.Add(capabilityNames[i], true);
           }
           else
           {
               ctlParameters.ClbxEmitters.Items.Add(capabilityNames[i], false);
           }

       }  


            }
 


        }

        public void ResetForNewEntry()
        {
            txtName.Text = "";
            List<string> speciesNames = Ctl_Species.Species.GetNames();
            lbxSpecies.Items.Clear();
            for (int i = 0; i < speciesNames.Count; i++)
                lbxIconSelectIcon.Items.Add(speciesNames[i]);
            lbxSpecies.SelectedValue = "" ;
            // Handle Icon ...

            ctlParameters.NndFuelCapacity.Value = 0;
            ctlParameters.NudLaunchDuration.Value = 0;
            ctlParameters.NudDockingDuration.Value = 0;
            ctlParameters.NndMaximumSpeed.Value = 0;
            ctlParameters.NndFuelUseRate.Value = 0;
            ctlParameters.CkStealable.Checked = false;

            #region singletons
            ctlParameters.SngSingleton_1.LbxCapability.Items.Clear();
            List<string> capabilitiesNames = Ctl_Capabilities.Capabilities.GetNames();
            for (int i = 0; i < capabilitiesNames.Count; i++)
            {
                ctlParameters.SngSingleton_1.LbxCapability.Items.Add(capabilitiesNames[i]);
                ctlParameters.SngSingleton_2.LbxCapability.Items.Add(capabilitiesNames[i]);
                ctlParameters.SngSingleton_3.LbxCapability.Items.Add(capabilitiesNames[i]);

            }
            ctlParameters.SngSingleton_1.LbxCapability.SelectedValue = "";
            ctlParameters.SngSingleton_2.LbxCapability.SelectedValue = "";
            ctlParameters.SngSingleton_3.LbxCapability.SelectedValue = "";
            ctlParameters.SngSingleton_1.NndRange_1.Value = 0;
            ctlParameters.SngSingleton_2.NndRange_1.Value = 0;
            ctlParameters.SngSingleton_3.NndRange_1.Value = 0;
            ctlParameters.SngSingleton_1.NndRange_2.Value = 0;
            ctlParameters.SngSingleton_2.NndRange_2.Value = 0;
            ctlParameters.SngSingleton_3.NndRange_2.Value = 0;
            ctlParameters.SngSingleton_1.NudIntensity_1.Value = 0;
            ctlParameters.SngSingleton_2.NudIntensity_1.Value = 0;
            ctlParameters.SngSingleton_3.NudIntensity_1.Value = 0;
            ctlParameters.SngSingleton_1.NudIntensity_2.Value = 0;
            ctlParameters.SngSingleton_2.NudIntensity_2.Value = 0;
            ctlParameters.SngSingleton_3.NudIntensity_2.Value = 0;
            ctlParameters.SngSingleton_1.NudProbability_1.Value = 100;
            ctlParameters.SngSingleton_2.NudProbability_1.Value = 100;
            ctlParameters.SngSingleton_3.NudProbability_1.Value = 100;
            ctlParameters.SngSingleton_1.NudProbability_2.Value = 100;
            ctlParameters.SngSingleton_2.NudProbability_2.Value = 100;
            ctlParameters.SngSingleton_3.NudProbability_2.Value = 100;


            ctlParameters.SngSingleton_1.LbxState_1.Items.Clear();
            ctlParameters.SngSingleton_1.LbxState_2.Items.Clear();
            ctlParameters.SngSingleton_2.LbxState_1.Items.Clear();
            ctlParameters.SngSingleton_2.LbxState_2.Items.Clear();
            ctlParameters.SngSingleton_3.LbxState_1.Items.Clear();
            ctlParameters.SngSingleton_3.LbxState_2.Items.Clear();
            ctlParameters.LbxDepletionState.Items.Clear();
            List<string> stateNames = Ctl_States.States.GetNames();
            for (int i = 0; i < stateNames.Count; i++)
                ctlParameters.LbxDepletionState.Items.Add(stateNames[i]);

         #endregion

#region combos
                ctlParameters.CmbCombo_1.LbxCapability_1.Items.Clear();
    ctlParameters.CmbCombo_2.LbxCapability_1.Items.Clear();
    for (int i = 0; i < capabilitiesNames.Count; i++)
    {
        ctlParameters.CmbCombo_1.LbxCapability_1.Items.Add(capabilitiesNames[i]);
        ctlParameters.CmbCombo_1.LbxCapability_2.Items.Add(capabilitiesNames[i]);
    }


            ctlParameters.CmbCombo_1.LbxCapability_1.SelectedValue = "";
            ctlParameters.CmbCombo_1.LbxCapability_2.SelectedValue = "";
            ctlParameters.CmbCombo_2.LbxCapability_1.SelectedValue = "";
            ctlParameters.CmbCombo_2.LbxCapability_2.SelectedValue = "";

            ctlParameters.CmbCombo_1.NndRange_1.Value = 0;
            ctlParameters.CmbCombo_1.NndRange_2.Value = 0;
            ctlParameters.CmbCombo_2.NndRange_1.Value = 0;
            ctlParameters.CmbCombo_2.NndRange_2.Value = 0;

            ctlParameters.CmbCombo_1.NudEffect_1.Value = 0;
            ctlParameters.CmbCombo_1.NudEffect_2.Value = 0;
            ctlParameters.CmbCombo_2.NudEffect_1.Value = 0;
            ctlParameters.CmbCombo_2.NudEffect_2.Value = 0;

            ctlParameters.CmbCombo_1.NudProbability_1.Value = 100;
            ctlParameters.CmbCombo_1.NudProbability_2.Value = 100;
            ctlParameters.CmbCombo_2.NudProbability_1.Value = 100;
            ctlParameters.CmbCombo_2.NudProbability_2.Value = 100;

                  ctlParameters.CmbCombo_1.LbxNewState.Items.Clear();
                ctlParameters.CmbCombo_2.LbxNewState.Items.Clear();
                for(int i=0;i<stateNames.Count;i++)
                {
                    ctlParameters.CmbCombo_1.LbxNewState.Items.Add(stateNames[i]);
                     ctlParameters.CmbCombo_2.LbxNewState.Items.Add(stateNames[i]);

                }         
#endregion


              ctlParameters.ClbxSenses.Items.Clear();
       List<string> sensorNames = Ctl_Sensors.Sensors.GetNames();
       for (int i = 0; i < sensorNames.Count; i++)
       {

        
     
              
       }

       ctlParameters.ClbxEmitters.Items.Clear();
       List<string> emitterNames = Ctl_Emitters.Emitters.GetNames();
       for (int i = 0; i < emitterNames.Count; i++)
       {

         
               ctlParameters.ClbxEmitters.Items.Add(emitterNames[i], false);
           }

     


       ctlParameters.ClbxCapabilities.Items.Clear();
       List<string> capabilityNames = Ctl_Capabilities.Capabilities.GetNames();
       for (int i = 0; i < capabilityNames.Count; i++)
       {

          
         

       }  


            }

        
        private void btnAccept_Click(object sender, EventArgs e)
        {

            if (""==lbxSpecies.SelectedValue)
            {
                MessageBox.Show("Please indicate the species for which this is a state.");
            }
            else if ("" == txtName.Text)
            {
                MessageBox.Show("Please provide a name for the state you are defining.");
            }
            else if (txtName.Text.Contains("+"))
            {
                MessageBox.Show("A state name may not contain the symbol '+'");
            }
            else
            {
                _datastore.ID = txtName.Text + "+" + lbxSpecies.SelectedValue;
                // Handle Icon
                StateParameters _parms = _datastore.Parameters;

                _parms.LaunchDuration = (int)(ctlParameters.NudLaunchDuration.Value);
                _parms.DockingDuration = (int)(ctlParameters.NudDockingDuration.Value);
                _parms.TimeToAttack = (int)(ctlParameters.NudTimeToAttack.Value);
                _parms.MaximumSpeed = ctlParameters.NndMaximumSpeed.Value;
                _parms.FuelCapacity = ctlParameters.NndFuelCapacity.Value;
                _parms.InitialFuelLoad = ctlParameters.NndInitialFuelLoad.Value;
                _parms.FuelUseRate = ctlParameters.NndFuelUseRate.Value;
                _parms.FuelDepletionState = ctlParameters.LbxDepletionState.SelectedValue.ToString();
                _parms.Stealable = ctlParameters.CkStealable.Checked;
        _parms.Sensors = new List<string>();  
                for(int i=0;i<ctlParameters.ClbxSenses.CheckedItems.Count;i++)
                    _parms.Sensors.Add(ctlParameters.ClbxSenses.CheckedItems[i].ToString() );
                
        _parms.Emitters = new List<string>();
        for (int i = 0; i < ctlParameters.ClbxEmitters.CheckedItems.Count; i++)
            _parms.Emitters.Add((string)(ctlParameters.ClbxEmitters.CheckedItems[i]));
      
        _parms.Capabilities = new List<string>();
        for (int i = 0; i < ctlParameters.ClbxCapabilities.CheckedItems.Count; i++)
            _parms.Capabilities.Add(ctlParameters.ClbxCapabilities.CheckedItems[i].ToString());
      
        _parms.Singletons = new Singleton[3];
        _parms.Singletons[0].Transition_1.State = ctlParameters.SngSingleton_1.LbxState_1.SelectedValue.ToString();
        _parms.Singletons[0].Transition_2.State = ctlParameters.SngSingleton_1.LbxState_2.SelectedValue.ToString();
        _parms.Singletons[1].Transition_1.State = ctlParameters.SngSingleton_2.LbxState_1.SelectedValue.ToString();
        _parms.Singletons[1].Transition_2.State = ctlParameters.SngSingleton_2.LbxState_2.SelectedValue.ToString();
        _parms.Singletons[2].Transition_1.State = ctlParameters.SngSingleton_3.LbxState_1.SelectedValue.ToString();
        _parms.Singletons[2].Transition_2.State = ctlParameters.SngSingleton_3.LbxState_2.SelectedValue.ToString();

        _parms.Singletons[0].Transition_1.Intensity = (int)ctlParameters.SngSingleton_1.NudIntensity_1.Value;
        _parms.Singletons[0].Transition_2.Intensity = (int)ctlParameters.SngSingleton_1.NudIntensity_2.Value;
        _parms.Singletons[1].Transition_1.Intensity = (int)ctlParameters.SngSingleton_2.NudIntensity_1.Value;
        _parms.Singletons[1].Transition_2.Intensity = (int)ctlParameters.SngSingleton_2.NudIntensity_2.Value;
        _parms.Singletons[2].Transition_1.Intensity = (int)ctlParameters.SngSingleton_3.NudIntensity_1.Value;
        _parms.Singletons[2].Transition_2.Intensity = (int)ctlParameters.SngSingleton_3.NudIntensity_2.Value;

        _parms.Singletons[0].Transition_1.Range = ctlParameters.SngSingleton_1.NndRange_1.Value;
        _parms.Singletons[0].Transition_2.Range = ctlParameters.SngSingleton_1.NndRange_2.Value;
        _parms.Singletons[1].Transition_1.Range = ctlParameters.SngSingleton_2.NndRange_1.Value;
        _parms.Singletons[1].Transition_2.Range = ctlParameters.SngSingleton_2.NndRange_2.Value;
        _parms.Singletons[2].Transition_1.Range = ctlParameters.SngSingleton_3.NndRange_1.Value;
        _parms.Singletons[2].Transition_2.Range = ctlParameters.SngSingleton_3.NndRange_2.Value;

        _parms.Singletons[0].Transition_1.Probability = (int)ctlParameters.SngSingleton_1.NudProbability_1.Value;
        _parms.Singletons[0].Transition_2.Probability = (int)ctlParameters.SngSingleton_1.NudProbability_2.Value;
        _parms.Singletons[1].Transition_1.Probability = (int)ctlParameters.SngSingleton_2.NudProbability_1.Value;
        _parms.Singletons[1].Transition_2.Probability = (int)ctlParameters.SngSingleton_2.NudProbability_2.Value;
        _parms.Singletons[2].Transition_1.Probability = (int)ctlParameters.SngSingleton_3.NudProbability_1.Value;
        _parms.Singletons[2].Transition_2.Probability = (int)ctlParameters.SngSingleton_3.NudProbability_2.Value;
  
                _parms.Combos = new Combo[2];
        _parms.Combos[0].NewState = ctlParameters.CmbCombo_1.LbxNewState.SelectedValue.ToString();
        _parms.Combos[1].NewState = ctlParameters.CmbCombo_2.LbxNewState.SelectedValue.ToString();

        _parms.Combos[0].Contribution_1.Capability = ctlParameters.CmbCombo_1.LbxCapability_1.SelectedValue.ToString();
        _parms.Combos[0].Contribution_2.Capability = ctlParameters.CmbCombo_1.LbxCapability_2.SelectedValue.ToString();
        _parms.Combos[1].Contribution_1.Capability = ctlParameters.CmbCombo_1.LbxCapability_1.SelectedValue.ToString();
        _parms.Combos[1].Contribution_2.Capability = ctlParameters.CmbCombo_1.LbxCapability_2.SelectedValue.ToString();

        _parms.Combos[0].Contribution_1.Effect = (int)ctlParameters.CmbCombo_1.NudEffect_1.Value;
        _parms.Combos[0].Contribution_2.Effect = (int)ctlParameters.CmbCombo_1.NudEffect_2.Value;
        _parms.Combos[1].Contribution_1.Effect = (int)ctlParameters.CmbCombo_1.NudEffect_1.Value;
        _parms.Combos[1].Contribution_2.Effect = (int)ctlParameters.CmbCombo_1.NudEffect_2.Value;


        _parms.Combos[0].Contribution_1.Range = ctlParameters.CmbCombo_1.NndRange_1.Value;
        _parms.Combos[0].Contribution_2.Range = ctlParameters.CmbCombo_1.NndRange_2.Value;
        _parms.Combos[1].Contribution_1.Range = ctlParameters.CmbCombo_1.NndRange_1.Value;
        _parms.Combos[1].Contribution_2.Range = ctlParameters.CmbCombo_1.NndRange_2.Value;

        _parms.Combos[0].Contribution_1.Probability = (int)ctlParameters.CmbCombo_1.NudProbability_1.Value;
        _parms.Combos[0].Contribution_2.Probability = (int)ctlParameters.CmbCombo_1.NudProbability_2.Value;
        _parms.Combos[1].Contribution_1.Probability = (int)ctlParameters.CmbCombo_1.NudProbability_1.Value;
        _parms.Combos[1].Contribution_2.Probability = (int)ctlParameters.CmbCombo_1.NudProbability_2.Value;
 
                

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }


    
    }
}
