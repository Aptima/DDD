using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using User_Controls;
using AME.Views.View_Components;
using AME.EngineModels;

namespace AME.Views.User_Controls
{

    public partial class SimulationProcessPanel : ProcessPanel
    {

        public SimulationProcessPanel()
            : base(ProcessType.SIMULATION)
        {
        }

        protected override void runButtonClick()
        {

            List<Int32> selectedIDs = new List<Int32>();

            foreach (CustomCombo customCombo in this.InputsComboList)
            {
                if (customCombo.SelectedItem != null)
                {
                    ComboItem selectedCast = (ComboItem)customCombo.SelectedItem;
                    selectedIDs.Add(selectedCast.MyID);
                }
                else
                {
                    MessageBox.Show("Please select Simulation Run inputs!");
                    return;
                }
            }
            if (this.ProcessCustomCombo.SelectedItem != null)
            {
                ComboItem selectedComboItem = (ComboItem)this.ProcessCustomCombo.SelectedItem;

                this.Cursor = Cursors.WaitCursor;

                int[] runIds = modelingController.Run(selectedComboItem.MyID, selectedIDs);//, textBox1.Text);

                // Ruuning the measures should be optional.
                if (measuresController != null)
                {
                    if (calculateMeasures)
                    {
                        foreach (int run in runIds)
                            measuresController.Run(selectedComboItem.MyID, run);
                    }
                }

                if (runIds.Length > 0)
                    this.OnNewRunCreated(new EventArgs());

                this.createRunInputs();

                this.Cursor = Cursors.Default;
            }
        }
    }
}
