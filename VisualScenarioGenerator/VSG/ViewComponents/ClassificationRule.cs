using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AME.Views.View_Components;
using AME.Controllers;
using System.Xml.XPath;
using VSG.Helpers;
using VSG.Controllers;
using VSG.Dialogs;
using System.Xml;

namespace VSG.ViewComponents
{
    public partial class ClassificationRule : UserControl, IViewComponent
    {
        private ClassificationDisplayRules currentRules;
        private VSGController controller;
        private IViewComponentHelper viewComponentHelper;

        private Int32 rootId = -1;
        private Int32 componentId = -1; //species id
        private String sourceLink = String.Empty;
        private String link = String.Empty;
        private String parameterCategory;
        private String parameterName;

        public String ParameterCategory
        {
            get
            {
                return parameterCategory;
            }

            set
            {
                parameterCategory = value;
            }
        }
        public String ParameterName
        {

            get
            {
                return parameterName;
            }

            set
            {
                parameterName = value;
            }
        }
        public String SourceLink
        {
            get
            {
                return sourceLink;
            }
            set
            {
                sourceLink = value;
            }
        }
        public String Link
        {
            get
            {
                return link;
            }
            set
            {
                link = value;
            }
        }
        public Int32 RootId
        {
            get
            {
                return rootId;
            }
            set
            {
                rootId = value;
            }
        }
        public Int32 ComponentId
        {
            get
            {
                return componentId;
            }
            set
            {
                componentId = value;
            }
        }
        public ClassificationRule()
        {
            currentRules = new ClassificationDisplayRules();
            viewComponentHelper = new ViewComponentHelper(this);
            InitializeComponent();
            dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
        }
        public ClassificationRule(IContainer container)
        {
            container.Add(this);
            currentRules = new ClassificationDisplayRules();
            viewComponentHelper = new ViewComponentHelper(this);
            InitializeComponent();
            dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
        }

        void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            { //edit
                EditRule(e.RowIndex);
            }
            else if (e.ColumnIndex == 5)
            { //delete
                DeleteRule(e.RowIndex);
            }
            //throw new NotImplementedException();
        }

        private void EditRule(int rowIndex)
        {
            if (dataGridView1.Rows.Count - 1 < rowIndex || rowIndex < 0)
                return;
            String state = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
            String classification = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
            String icon = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();

            EditClassificationDialog dlg = new EditClassificationDialog();
            dlg.controller = this.controller;
            dlg.StateEnum.AddRange(GetStatesListBySpecies(controller, rootId, link, componentId));
            List<String> s = GetClassificationList(controller, rootId);
            dlg.ClassificationEnum.AddRange(s);
            dlg.IconEnum.AddRange(controller.CurrentIconLibraryIconNames());
            dlg.StateName = state;
            dlg.ClassificationName = classification;
            dlg.IconName = icon;
            dlg.Initialize();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                //state = dlg.StateName;
                //classification = dlg.ClassificationName;
                //icon = dlg.IconName;
                UpdateClassificationRule(classification, state, icon, dlg.ClassificationName, dlg.StateName, dlg.IconName);
                UpdateViewComponent();
                //dataGridView1.Rows[rowIndex].Cells[0].Value = state;
                //dataGridView1.Rows[rowIndex].Cells[1].Value = classification;
                //dataGridView1.Rows[rowIndex].Cells[2].Value = icon;
                //dataGridView1.Rows[rowIndex].Cells[3].Value = GetImage(icon);
            }
        }
        private void UpdateClassificationRule(String oldClassification, String oldState, String oldIconName, String classification, String state, String iconName)
        {
            if (currentRules == null)
                currentRules = new ClassificationDisplayRules();
            foreach (ClassificationDisplayRule rule in currentRules.Rules)
            {
                if (rule.Classification == oldClassification && rule.IconName == oldIconName && rule.StateName == oldState)
                {
                    rule.Classification = classification;
                    rule.IconName = iconName;
                    rule.StateName = state;
                    break;
                }
            }
            UpdateRuleInAME(currentRules);
        }
        private void UpdateRuleInAME(ClassificationDisplayRules rules)
        {
            if (controller == null || componentId < 0)
                return;
            try
            {
                controller.UpdateParameters(componentId, parameterCategory + "." + parameterName, rules.ToXML(), eParamParentType.Component);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error updating parameter. Check the format of the parameter and any other constraints");
            }
        }
        private void DeleteRule(int rowIndex)
        {
            if (dataGridView1.Rows.Count - 1 < rowIndex || rowIndex < 0)
                return;
            if (MessageBox.Show("Are you sure you want to delete this rule?", "Confirm Rule Delete", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            String oldState = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
            String oldClassification = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
            String oldIconName = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
            int count = 0;
            int removeAt = -1;
            while(count < currentRules.Rules.Count && removeAt == -1)
            {
                if (currentRules.Rules[count].Classification == oldClassification && currentRules.Rules[count].IconName == oldIconName && currentRules.Rules[count].StateName == oldState)
                {
                    removeAt = count;
                }
                count++;
            }
            if (removeAt > -1)
            {
                currentRules.Rules.RemoveAt(removeAt);
                UpdateRuleInAME(currentRules);
                UpdateViewComponent();
            }
        }

        #region IViewComponent Members

        public IController Controller
        {
            get
            {
                return controller;
            }
            set
            {
                controller = (VSGController)value;
            }
        }

        public void UpdateViewComponent()
        {
            if (controller == null)
                return;
            if (componentId < 0)
            {
                this.SuspendLayout();
                dataGridView1.Rows.Clear();
                this.ResumeLayout();
                return;
            }

            this.SuspendLayout();
            controller.TurnViewUpdateOff();

            dataGridView1.Rows.Clear();
            String serializedString = String.Empty;
            if (!parameterCategory.Equals(String.Empty) && !parameterName.Equals(String.Empty))
            {
                IXPathNavigable inav = controller.GetParametersForComponent(componentId);
                XPathNavigator navigator = inav.CreateNavigator();

                XPathNavigator node = navigator.SelectSingleNode(String.Format("ComponentParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", parameterCategory, parameterName));
                if (node != null)
                {
                    serializedString = node.GetAttribute("value", String.Empty);
                }
                ClassificationDisplayRules cdr = new ClassificationDisplayRules();
                currentRules.Rules = ClassificationDisplayRules.FromXML(serializedString);
                foreach (ClassificationDisplayRule r in currentRules.Rules)
                {
                    dataGridView1.Rows.Add(CreateRow(r));
                }
            }

            controller.TurnViewUpdateOn(false, false);
            this.ResumeLayout();
        }

        public IViewComponentHelper IViewHelper
        {
            get { return viewComponentHelper; }
        }

        #endregion

        private DataGridViewRow CreateRow(ClassificationDisplayRule r)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dataGridView1, r.StateName, r.Classification, r.IconName, GetImage(r.IconName), "Edit...", "Delete");
            return row;
        }

        private Image GetImage(String imgName)
        {
            Graphics unkGraphics = this.CreateGraphics();
            List<Point> p = new List<Point>();
            p.Add(new Point(5, 5));
            p.Add(new Point(5, 43));
            p.Add(new Point(43, 43));
            p.Add(new Point(43, 5));
            p.Add(new Point(5, 5));

            unkGraphics.FillPolygon(new SolidBrush(Color.Red), p.ToArray());
            Image img = new Bitmap(48, 48, unkGraphics);
            try
            {
                img = controller.CurrentIconLibrary.Images[imgName];
            }
            catch (Exception ex)
            {
            }
            return img;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (controller == null)
                return;
            String state = ""; 
            String classification = "";
            String icon = "";
            EditClassificationDialog dlg = new EditClassificationDialog();
            dlg.controller = this.controller;
            dlg.StateEnum.AddRange(GetStatesListBySpecies(controller, rootId, link, componentId));
            dlg.ClassificationEnum.AddRange(GetClassificationList(controller, rootId));
            dlg.IconEnum.AddRange(controller.CurrentIconLibraryIconNames());
            if (dlg.StateEnum.Count == 0)
            {
                MessageBox.Show(this, "In order to create Classification Display Rules, you'll need to define States for this Species first.");
                return;
            }
            if (dlg.ClassificationEnum.Count == 0)
            {
                MessageBox.Show(this, "In order to create Classification Display Rules, you'll need to define Classifications first.  You can do this on the Scenario Elements window, under the Players tab.");
                return;
            }
            if (dlg.IconEnum.Count == 0)
            {
                MessageBox.Show(this, "In order to create Classification Display Rules, you'll need to choose an Icon library first");
                return;
            }
            //dlg.StateName = state;
            //dlg.ClassificationName = classification;
            //dlg.IconName = icon;
            dlg.Initialize();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                state = dlg.StateName;
                classification = dlg.ClassificationName;
                icon = dlg.IconName;
                if (currentRules == null)
                    currentRules = new ClassificationDisplayRules();
                currentRules.Rules.Add(new ClassificationDisplayRule(state, classification, icon));
                UpdateRuleInAME(currentRules);
                UpdateViewComponent();
                //DataGridViewRow row = new DataGridViewRow();
                //row.CreateCells(dataGridView1, state, classification, icon, GetImage(icon), "Edit...", "Delete");
                //dataGridView1.Rows.Add(row);
            }
            //ClassificationDisplayRule r = new ClassificationDisplayRule(state, classification, icon);
            //dataGridView1.Rows.Add(CreateRow(r));
        }

        public static List<String> GetStatesListBySpecies(VSGController control, int rootID, string linkType, int speciesId)
        {
            List<String> states = new List<string>();
            if (speciesId < 0)
            {
                return states;
            }
            ComponentOptions compOptions = new ComponentOptions();
            IXPathNavigable iNavSpeciesType = control.GetComponentAndChildren(control.ScenarioId, "SpeciesType", compOptions);
            XPathNavigator navSpeciesType = iNavSpeciesType.CreateNavigator();
            XPathNavigator navSpecies = navSpeciesType.SelectSingleNode(String.Format("/Components/Component/Component[@ID='{0}']", speciesId));
            XPathNodeIterator itSpecies = navSpecies.Select("self::*");
            while (itSpecies.MoveNext())
            {
                String id = itSpecies.Current.GetAttribute("ID", itSpecies.Current.NamespaceURI);
                IXPathNavigable iNavScenario = control.GetComponentAndChildren(Int32.Parse(id), "Scenario", compOptions);
                XPathNavigator navScenario = iNavScenario.CreateNavigator();
                XPathNodeIterator itStates = navScenario.Select("/Components/Component[@Type='Species']/Component[@Type='State']");
                while (itStates.MoveNext())
                {
                    String name = itStates.Current.GetAttribute("Name", itStates.Current.NamespaceURI);
                    if (!states.Contains(name))
                        states.Add(name);
                }
            }
            return states;
        }
        public static List<String> GetClassificationList(VSGController control, int rootID)
        {
            List<String> classifications = new List<string>();

            IXPathNavigable iNavClassType = control.GetComponentAndChildren(control.ScenarioId, "Scenario", new ComponentOptions());//.GetComponent("Scenario");
            XPathNavigator navigator = iNavClassType.CreateNavigator();
            XPathNodeIterator itClassifications = navigator.Select("/Components/Component[@Type='Scenario']/Component[@Type='Classification']");
            if (itClassifications.Count > 0)
            {
                while (itClassifications.MoveNext())
                {
                    classifications.Add(itClassifications.Current.GetAttribute("Name", itClassifications.Current.NamespaceURI));
                }
            }

            return classifications;
        }
    }
}
