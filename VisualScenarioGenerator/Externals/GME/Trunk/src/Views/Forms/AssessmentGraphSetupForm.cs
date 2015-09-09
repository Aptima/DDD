using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GME.Views.View_Components;
using System.Xml.XPath;
using Model;
using Controllers;
using System.Collections;

namespace GME.Views.Forms {

    public partial class AssessmentGraphSetupForm : Form {

        private int tabCount = 3; //<ml> temp, will use the config file instead

        public AssessmentGraphSetupForm(IXPathNavigable data) {
            //this.xmlData = data;
            InitializeComponent();
            this.setupTabs(data);
        }

        private void setupTabs(IXPathNavigable data) {

            GraphSetupTabPage graphSetupTabPage = null;
            TabPage tabPage = null;
            for (int i = 0; i < tabCount; i++) {

                // graphSetupTabPage            
                graphSetupTabPage = new GraphSetupTabPage(i, data);
                graphSetupTabPage.Location = new System.Drawing.Point(-4, 0);
                graphSetupTabPage.Name = "graphSetupTabPage" + i;
                graphSetupTabPage.Size = new System.Drawing.Size(332, 200);
                graphSetupTabPage.ActivateEvent += 
                    new GraphSetupTabPage.EventHandler(this.graphSetupTabPage_ActivateEvent);
                if (i > 0) //enable only the first tab
                    graphSetupTabPage.Enabled = false;

                // tabPage1
                tabPage = new TabPage("Graph " + (i + 1));
                tabPage.Controls.Add(graphSetupTabPage);
                tabPage.Location = new System.Drawing.Point(4, 22);
                tabPage.Name = "tabPage" + i;
                tabPage.Padding = new System.Windows.Forms.Padding(3);
                tabPage.Size = new System.Drawing.Size(324, 193);
                tabPage.TabIndex = i;
                tabPage.UseVisualStyleBackColor = true;

                this.graphPanel.Controls.Add(tabPage);
            }
        }

        public void graphSetupTabPage_ActivateEvent(object sender, EventArgs e) {

            int index = ((GraphSetupTabPage)sender).getIndex(); //get index of the controll
            bool activate = ((ActivatePanelEvent)e).isActive(); //get activate/diactivate flag
            try {
                TabPage page = (TabPage)this.graphPanel.Controls[index + 1]; //get the NEXT page
                page.Controls["graphSetupTabPage" + (index + 1)].Enabled = activate; //activate its control
                if (!activate) {
                    for (int i = index + 2; i < tabCount; i++) {
                        page = (TabPage)this.graphPanel.Controls[i];
                        page.Controls["graphSetupTabPage" + (i)].Enabled = activate; //activate = false
                    }
                }
            }
            catch (ArgumentOutOfRangeException) {
                //Last tab was activated, just continue...
            }
        }

        private int getNumberToDisplay() {
            int count = 0;
            GraphSetupTabPage page = null;
            for (int i = 0; i < this.tabCount; i++) {
                page = (GraphSetupTabPage)((TabPage)this.graphPanel.Controls[i]).Controls[0];
                count += page.isDisplayed() ? 1 : 0;
            }
            return count;
        }

        private void okButton_Click(object sender, EventArgs e) {

            ChartViewData[] data = new ChartViewData[this.getNumberToDisplay()];
            GraphSetupTabPage page = null;
            for (int i = 0; i < data.Length; i++) {
                page = (GraphSetupTabPage)((TabPage)this.graphPanel.Controls[i]).Controls[0];
                data[i] = page.getGraphingData();
            }

            ChartForm form = new ChartForm();
            form.loadGraphData(data[0]);
            form.Show();

            //for (int i = 0; i < this.tabCount; i++) {
            //    page = (GraphSetupTabPage)((TabPage)this.graphPanel.Controls[i]).Controls[0];
            //    data = page.getGraphingData();
            //    if (page.isDisplayed() && data2 != null)
            //        graphData.Add(data2);
            //    else
            //        continue;
            //}

            //if (graphData.Count == 0)
            //    return;
            //TO DO: do the actual graphing
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        public static void Main() {
            IModel myModel = new Model.Model("localhost", 3306, "root", "devpass", "most");
            myModel.ConfigurationPath = @"C:\dev\BACKUP A-Model\Projects\IO-AOC\Config";
            AssessmentController ass = new AssessmentController(myModel, "ProjectEditor");
            IXPathNavigable runs = ass.GetSimRunData(1059);

            Application.Run(new AssessmentGraphSetupForm(runs));
        }
    }
}