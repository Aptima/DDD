using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using AME.Views.View_Components;
using AME.Controllers;
using System.Xml.XPath;

namespace VSG.ViewComponents
{
    public partial class SimpleLinkGrid : DataGridView, IViewComponent
    {
        private IController controller;
        private IViewComponentHelper viewComponentHelper;
        
        private Int32 rootId = -1;
        private Int32 componentId = -1;
        
        private String sourceLink = String.Empty;
        private String link = String.Empty;

        private String componentFrom = String.Empty;
        private String componentTo = String.Empty;

        private String parameter = String.Empty;
        private String parameterCategory = String.Empty;

        private Dictionary<int, XPathExpression> compiledExpressions = new Dictionary<int, XPathExpression>();
        
        public SimpleLinkGrid()
        {
            viewComponentHelper = new ViewComponentHelper(this);
            InitializeComponent();
        }

        public SimpleLinkGrid(IContainer container)
        {
            container.Add(this);

            viewComponentHelper = new ViewComponentHelper(this);
            InitializeComponent();
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

        public String ComponentFrom
        {
            get
            {
                return componentFrom;
            }
            set
            {
                componentFrom = value;
            }
        }

        public String ComponentTo
        {
            get
            {
                return componentTo;
            }
            set
            {
                componentTo = value;
            }
        }

        public String Parameter
        {
            get
            {
                return parameter;
            }
            set
            {
                parameter = value;
            }
        }

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

        #region Overrides

        protected override void OnCellValueChanged(DataGridViewCellEventArgs e)
        {
            if (e != null)
            {
                // Update according to column index!
                switch (e.ColumnIndex)
                {
                    case 1:
                        {
                            Int32 linkId = (Int32)this.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
                            try
                            {
                                Int32 value = Int32.Parse((String)this.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

                                controller.UpdateParameters(linkId, String.Format("{0}.{1}", this.parameterCategory, this.parameter), value.ToString(), eParamParentType.Link);
                            }
                            catch (FormatException fe)
                            {
                                System.Windows.Forms.MessageBox.Show(fe.Message, "Format Exception");
                                UpdateViewComponent();
                            }
                            catch (Exception ex)
                            {
                                System.Windows.Forms.MessageBox.Show(ex.Message, "Exception");
                                UpdateViewComponent();
                            }
                        }
                        break;
                }
            }

            base.OnCellValueChanged(e);
        }
        
        protected override bool ProcessDialogKey(Keys keyData)
        {
            Keys key = (keyData & Keys.KeyCode);
            if (key == Keys.Enter)
            {
                return this.ProcessTabKey(keyData);
            }
            return base.ProcessDialogKey(keyData);
        }

        #endregion

        #region IViewComponent Members

        public IController Controller
        {
            get
            {
                return controller;
            }
            set
            {
                controller = value;
            }
        }

        public void UpdateViewComponent()
        {
            if (controller == null)
                return;
            if (componentId < 0)
            {
                this.SuspendLayout();
                this.Columns.Clear();
                this.ResumeLayout();
                return;
            }

            this.SuspendLayout();
            controller.TurnViewUpdateOff();

            this.Columns.Clear();
            ComponentOptions compOptions = new ComponentOptions();

            IXPathNavigable iNavComponent = controller.GetComponentAndChildren(this.rootId, sourceLink, compOptions);
            XPathNavigator navComponent = iNavComponent.CreateNavigator();
            // The xpath here needs to be configurable if the component is to remain totally reusable.
            XPathNodeIterator itComponent = navComponent.Select(String.Format("/Components/Component/Component[@Type='{0}']", this.componentFrom));
            
            this.Columns.Add(this.componentTo, this.componentTo);
            this.Columns.Add("Parameter", this.parameter);
            this.Columns[0].ReadOnly = true;

            //this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Might want this to be configurrable for links that are not dynamic... however the AME will soon be able to detect when to do this.
            String dynamicLink = controller.GetDynamicLinkType(link, this.componentId.ToString());

            while (itComponent.MoveNext())
            {
                String id = itComponent.Current.GetAttribute("ID", itComponent.Current.NamespaceURI);
                String name = itComponent.Current.GetAttribute("Name", itComponent.Current.NamespaceURI);
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell cellFirst = new DataGridViewTextBoxCell();
                cellFirst.Tag = id;
                cellFirst.Value = name;
                row.Cells.Add(cellFirst);
                Int32 linkId = this.controller.GetLinkID(this.componentId, Int32.Parse(id), dynamicLink);

                if (linkId < 0)
                    linkId = controller.Connect(componentId, componentId, Int32.Parse(id), dynamicLink);

                if (linkId >= 0)
                {
                    IXPathNavigable iNavLinkParameters = controller.GetParametersForLink(linkId);
                    XPathNavigator navLinkParameters = iNavLinkParameters.CreateNavigator();
                    XPathNavigator navLinkParameter = navLinkParameters.SelectSingleNode(String.Format("/LinkParameters/Parameter[@category='{0}']/Parameter[@displayedName='{1}']", this.parameterCategory, this.parameter));
                    String value = navLinkParameter.GetAttribute("value", navLinkParameter.NamespaceURI);
                    DataGridViewTextBoxCell cellLast = new DataGridViewTextBoxCell();
                    cellLast.Tag = linkId;
                    cellLast.Value = value;
                    row.Cells.Add(cellLast);
                }
                else
                {
                    //throw new Exception("Error in Indentity to Indentity links");
                    System.Windows.Forms.MessageBox.Show("Error with data!", "Error");
                    //this.Rows[index].Visible = false;
                    return;
                }
                this.Rows.Add(row);
            }

            foreach (DataGridViewColumn column in this.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            controller.TurnViewUpdateOn(false, false);
            this.ResumeLayout();
        }

        public IViewComponentHelper IViewHelper
        {
            get { return viewComponentHelper; }
        }

        #endregion
    }
}
