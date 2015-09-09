using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using VSG.Controllers;
using VSG.ViewComponents;

using AME;
using AME.Controllers;
using AME.Views.View_Components;
using AME.Nodes;
using Northwoods.Go;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;
using VSG.Helpers;

namespace VSG.ViewComponentPanels
{
    public partial class ScenarioPlayfieldViewComponentPanel : AME.Views.View_Component_Packages.ViewComponentPanel, AME.Views.View_Component_Packages.IViewComponentPanel
    {
        private ViewPanelHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private VSGController vsgController;
        private Guid guidMap; // Map image handle.

        private String myLinkType;

        public ScenarioPlayfieldViewComponentPanel()
        {
            myHelper = new ViewPanelHelper(this);

            InitializeComponent();

            vsgController = (VSGController)AMEManager.Instance.Get("VSG");
            vsgController.RegisterForUpdate(this);

            myLinkType = vsgController.ConfigurationLinkType;

            customTreeView1.Controller = vsgController;
            customTreeView1.AddCustomTreeRoot(myLinkType);
            customTreeView1.SetCustomTreeRootXsl(myLinkType, "RegionsTree.xsl");

            textBoxLRVertexList.Controller = vsgController;
            textBoxReferencePoint.Controller = vsgController;
            checkBoxDynamicRegion.Controller = vsgController;
            textBoxARSpeedMultiplier.Controller = vsgController;
            textBoxARVertexList.Controller = vsgController;
            textBoxActiveRegionRelativeVertexList.Controller = vsgController;
            textBoxARStart.Controller = vsgController;
            textBoxAREnd.Controller = vsgController;
            checkBoxARBlocksMovement.Controller = vsgController;
            checkBoxARIsVisible.Controller = vsgController;
            enumBoxARColor.Controller = vsgController;
            linkBoxARSensorsBlocked.Controller = vsgController;

            diagramPanel1.Controller = vsgController;
            foreach (Diagram d in diagramPanel1.Diagrams)
            {
                d.ObjectSingleClicked += new GoObjectEventHandler(d_ObjectSingleClicked);
                d.FillPolygon = false;
                d.OutlinePolygon = true;
            }

            // events for linking on points text box entry
            textBoxARVertexList.Leave += new EventHandler(textBoxARVertexList_Leave);
            textBoxARVertexList.KeyDown += new KeyEventHandler(textBoxARVertexList_KeyDown);

            textBoxLRVertexList.Leave += new EventHandler(textBoxLRVertexList_Leave);
            textBoxLRVertexList.KeyDown += new KeyEventHandler(textBoxLRVertexList_KeyDown);
        }

        private void textBoxARVertexList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CheckForDiagramLink(textBoxARVertexList.ComponentId);
            }
        }

        private void textBoxARVertexList_Leave(object sender, EventArgs e)
        {
            CheckForDiagramLink(textBoxARVertexList.ComponentId);
        }

        private void textBoxLRVertexList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CheckForDiagramLink(textBoxLRVertexList.ComponentId);
            }
        }

        private void textBoxLRVertexList_Leave(object sender, EventArgs e)
        {
            CheckForDiagramLink(textBoxLRVertexList.ComponentId);
        }

        private void CheckForDiagramLink(int componentID)
        {
            if (diagramPanel1.Diagrams.Count >=1)
            {
                Diagram first = diagramPanel1.Diagrams[0];

                // add the link on point entry if it doesn't exist
                if (!first.Controller.LinkExists(first.RootID, componentID, first.DiagramName))
                {
                    first.Controller.Connect(first.RootID, first.RootID, componentID, first.DiagramName);
                }
            }
        }

        public void UpdateViewComponent()
        {
            if (myHelper.LatestEventFromController == UpdateType.Component)
            {
                cEvent_ComponentUpdate();
            }
            else if (myHelper.LatestEventFromController == UpdateType.Parameter) 
            {
                cEvent_ParameterUpdate();
            }
            else if (myHelper.LatestEventFromController == UpdateType.ComponentAndParameter)
            {
                updateMap();
                UpdateCoordinateTransform();
                customTreeView1.UpdateViewComponent();

                textBoxLRVertexList.UpdateViewComponent();
                textBoxReferencePoint.UpdateViewComponent();
                checkBoxDynamicRegion.UpdateViewComponent();
                textBoxARSpeedMultiplier.UpdateViewComponent();
                textBoxActiveRegionRelativeVertexList.UpdateViewComponent();
                textBoxARVertexList.UpdateViewComponent();
                textBoxARStart.UpdateViewComponent();
                textBoxAREnd.UpdateViewComponent();
                checkBoxARBlocksMovement.UpdateViewComponent();
                checkBoxARIsVisible.UpdateViewComponent();
                enumBoxARColor.UpdateViewComponent();
                linkBoxARSensorsBlocked.UpdateViewComponent();

                diagramPanel1.UpdateViewComponent();
            }
         
        }

        private void cEvent_ComponentUpdate()
        {
            if (this.Parent != null)
            {
                updateMap();
                UpdateCoordinateTransform();
                customTreeView1.UpdateViewComponent();
                diagramPanel1.UpdateViewComponent();
            }
        }

        private void cEvent_ParameterUpdate()
        {
            if (this.Parent != null)
            {
                updateMap();
                UpdateCoordinateTransform();

                textBoxLRVertexList.UpdateViewComponent();
                
                checkBoxDynamicRegion.UpdateViewComponent();
                textBoxARSpeedMultiplier.UpdateViewComponent();
                textBoxARVertexList.UpdateViewComponent();
                String referencePoint, updatedVertexList, currentReferencePoint = "0, 0", vertexList = textBoxARVertexList.Text;
                if (textBoxReferencePoint.Text != String.Empty)
                {
                    currentReferencePoint = textBoxReferencePoint.Text;
                }
                bool island = panelLandRegion.Visible; //terribly hacky way to determine if we're updating a land or active region...
               // if (!island)
                //{
                    PolygonHelper.DeriveReferenceFromVertexList(currentReferencePoint, vertexList, out referencePoint, out updatedVertexList);
                    //set
                    textBoxActiveRegionRelativeVertexList.Text = updatedVertexList;
                    textBoxReferencePoint.Text = referencePoint;
                    textBoxActiveRegionRelativeVertexList.SetParameterValue(false);
                    textBoxReferencePoint.SetParameterValue(false);
                    textBoxActiveRegionRelativeVertexList.UpdateViewComponent();
                    textBoxReferencePoint.UpdateViewComponent();
               // }
                textBoxARStart.UpdateViewComponent();
                textBoxAREnd.UpdateViewComponent();
                checkBoxARBlocksMovement.UpdateViewComponent();
                checkBoxARIsVisible.UpdateViewComponent();
                enumBoxARColor.UpdateViewComponent();
                linkBoxARSensorsBlocked.UpdateViewComponent();

                diagramPanel1.UpdateViewComponent();
            }
        }

        private void UpdateCoordinateTransform()
        {
            if (vsgController != null && vsgController.CoordinateTransform != null)
            {
                foreach (Diagram d in diagramPanel1.Diagrams)
                {
                    if (d.CoordinateTransformer != null && !d.CoordinateTransformer.Equals(vsgController.CoordinateTransform))
                    {
                        d.ResetCRC(); // reset so we see new coords.
                    }
                    d.CoordinateTransformer = vsgController.CoordinateTransform;
                }
            }
        }

        private void updateMap()
        {
            if (this.Parent != null)
            {
                try
                {
                    if (vsgController != null && vsgController.CurrentMap != null)
                    {
                        //if (guidMap != vsgController.CurrentMapLocation.GetType().GUID)
                        //{
                            this.diagramPanel1.SetDiagramBackground(vsgController.CurrentMap, "ScenarioRegions");
                            guidMap = vsgController.CurrentMapLocation.GetType().GUID;
                        //}
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error in Map Background", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #region IViewComponentPanel Members
        private int _rootId = -1;
        public int RootId
        {
            get
            {
                return _rootId;
            }
            set
            {
                _rootId = value;
                customTreeView1.SetCustomTreeRootId(myLinkType, _rootId);
                customTreeView1.SetCustomTreeRootDisplayId(myLinkType, _rootId);

                textBoxLRVertexList.ComponentId = -1;

                textBoxARSpeedMultiplier.ComponentId = -1;
                textBoxActiveRegionRelativeVertexList.ComponentId = -1;
                textBoxARVertexList.ComponentId = -1;
                textBoxReferencePoint.ComponentId = -1;
                checkBoxDynamicRegion.ComponentId = -1;
                textBoxARStart.ComponentId = -1;
                textBoxAREnd.ComponentId = -1;
                checkBoxARBlocksMovement.ComponentId = -1;
                checkBoxARIsVisible.ComponentId = -1;
                enumBoxARColor.ComponentId = -1;
                linkBoxARSensorsBlocked.DisplayRootId = -1;

                diagramPanel1.SetID(_rootId);
                diagramPanel1.UpdateViewComponent();
                customTreeView1.UpdateViewComponent();
                
                textBoxLRVertexList.UpdateViewComponent();

                textBoxARSpeedMultiplier.UpdateViewComponent();
                textBoxActiveRegionRelativeVertexList.UpdateViewComponent();
                textBoxARVertexList.UpdateViewComponent();
                textBoxReferencePoint.UpdateViewComponent();
                checkBoxDynamicRegion.UpdateViewComponent();
                textBoxARStart.UpdateViewComponent();
                textBoxAREnd.UpdateViewComponent();
                checkBoxARBlocksMovement.UpdateViewComponent();
                checkBoxARIsVisible.UpdateViewComponent();
                enumBoxARColor.UpdateViewComponent();
                linkBoxARSensorsBlocked.UpdateViewComponent();

                updateMap();
            }
        }

        #endregion

        private void d_ObjectSingleClicked(object sender, GoObjectEventArgs e)
        {
            GoObject clickedObject = e.GoObject;

            if (clickedObject.ParentNode != null && clickedObject.ParentNode is HasNodeID)
            {
                DrawingUtility.SuspendDrawing(panelRegion);

                Int32 newSelected = ((HasNodeID)clickedObject.ParentNode).NodeID;
                String newSelectedName = vsgController.GetComponentName(newSelected);
                customTabPage3.Description = newSelectedName + " Properties";

                customTreeView1.DoNewSelectionWithID(newSelected, myLinkType);

                showRegionPanel(newSelected);

                textBoxLRVertexList.ComponentId = newSelected;
                textBoxLRVertexList.UpdateViewComponent();
                textBoxARStart.ComponentId = newSelected;
                textBoxAREnd.ComponentId = newSelected;
                checkBoxARBlocksMovement.ComponentId = newSelected;
                checkBoxARIsVisible.ComponentId = newSelected;
                enumBoxARColor.ComponentId = newSelected;
                linkBoxARSensorsBlocked.DisplayRootId = vsgController.ScenarioId;
                linkBoxARSensorsBlocked.ConnectRootId = newSelected;
                linkBoxARSensorsBlocked.ConnectFromId = newSelected;
                if (!panelLandRegion.Visible)
                {
                    textBoxARSpeedMultiplier.ComponentId = newSelected;
                    textBoxActiveRegionRelativeVertexList.ComponentId = newSelected;
                    textBoxARVertexList.ComponentId = newSelected;
                    textBoxReferencePoint.ComponentId = newSelected;
                    checkBoxDynamicRegion.ComponentId = newSelected;
                    textBoxARSpeedMultiplier.UpdateViewComponent();
                    textBoxActiveRegionRelativeVertexList.UpdateViewComponent();
                    textBoxARVertexList.UpdateViewComponent();
                    textBoxReferencePoint.UpdateViewComponent();
                    checkBoxDynamicRegion.UpdateViewComponent();
                }
                

                
                textBoxARStart.UpdateViewComponent();
                textBoxAREnd.UpdateViewComponent();
                checkBoxARBlocksMovement.UpdateViewComponent();
                checkBoxARIsVisible.UpdateViewComponent();
                enumBoxARColor.UpdateViewComponent();
                linkBoxARSensorsBlocked.UpdateViewComponent();

                DrawingUtility.ResumeDrawing(panelRegion);
            }
        }

        private void customTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            DrawingUtility.SuspendDrawing(panelRegion);

            ProcessingNode pNode = e.Node as ProcessingNode;
            customTabPage3.Description = pNode.Name + " Properties";

            showRegionPanel(pNode.NodeID);

            switch (pNode.NodeType)
            {
                case "LandRegion":

                    textBoxLRVertexList.ComponentId = pNode.NodeID;
                    textBoxLRVertexList.UpdateViewComponent();
                    break;
                case "ActiveRegion":

                    textBoxARSpeedMultiplier.ComponentId = pNode.NodeID;
                    textBoxActiveRegionRelativeVertexList.ComponentId = pNode.NodeID;
                    textBoxARVertexList.ComponentId = pNode.NodeID;
                    textBoxReferencePoint.ComponentId = pNode.NodeID;
                    checkBoxDynamicRegion.ComponentId = pNode.NodeID;
                    textBoxARStart.ComponentId = pNode.NodeID;
                    textBoxAREnd.ComponentId = pNode.NodeID;
                    checkBoxARBlocksMovement.ComponentId = pNode.NodeID;
                    checkBoxARIsVisible.ComponentId = pNode.NodeID;
                    enumBoxARColor.ComponentId = pNode.NodeID;
                    linkBoxARSensorsBlocked.DisplayRootId = vsgController.ScenarioId;
                    linkBoxARSensorsBlocked.ConnectRootId = pNode.NodeID;
                    linkBoxARSensorsBlocked.ConnectFromId = pNode.NodeID;

                        textBoxARSpeedMultiplier.UpdateViewComponent();
                        textBoxActiveRegionRelativeVertexList.UpdateViewComponent();
                        textBoxARVertexList.UpdateViewComponent();
                    textBoxReferencePoint.UpdateViewComponent();
                checkBoxDynamicRegion.UpdateViewComponent();
                        textBoxARStart.UpdateViewComponent();
                        textBoxAREnd.UpdateViewComponent();
                        checkBoxARBlocksMovement.UpdateViewComponent();
                        checkBoxARIsVisible.UpdateViewComponent();
                        enumBoxARColor.UpdateViewComponent();
                        linkBoxARSensorsBlocked.UpdateViewComponent();
                    break;
            }

            diagramPanel1.SelectNodeWithID(pNode.NodeID);

            DrawingUtility.ResumeDrawing(panelRegion);
        }

        private void customTreeView1_ItemAdd(string addItemString, int nodeID, int linkID, string itemType, string itemName, string linkType)
        {
            DrawingUtility.SuspendDrawing(panelRegion);

            customTreeView1.DoNewSelectionWithID(nodeID, myLinkType);
            customTabPage3.Description = itemName + " Properties";

            showRegionPanel(nodeID);

            switch (itemType)
            {
                case "LandRegion":
                    textBoxLRVertexList.ComponentId = nodeID;
                    textBoxLRVertexList.UpdateViewComponent();
                    break;
                case "ActiveRegion":
                    textBoxARSpeedMultiplier.ComponentId = nodeID;
                    textBoxActiveRegionRelativeVertexList.ComponentId = nodeID;
                    textBoxARVertexList.ComponentId = nodeID;
                    textBoxReferencePoint.ComponentId = nodeID;
                    checkBoxDynamicRegion.ComponentId = nodeID;
                    textBoxARStart.ComponentId = nodeID;
                    textBoxAREnd.ComponentId = nodeID;
                    checkBoxARBlocksMovement.ComponentId = nodeID;
                    checkBoxARIsVisible.ComponentId = nodeID;
                    enumBoxARColor.ComponentId = nodeID;
                    linkBoxARSensorsBlocked.DisplayRootId = vsgController.ScenarioId;
                    linkBoxARSensorsBlocked.ConnectRootId = nodeID;
                    linkBoxARSensorsBlocked.ConnectFromId = nodeID;

                    textBoxARSpeedMultiplier.UpdateViewComponent();
                    textBoxActiveRegionRelativeVertexList.UpdateViewComponent();
                    textBoxARVertexList.UpdateViewComponent();
                    textBoxReferencePoint.UpdateViewComponent();
                checkBoxDynamicRegion.UpdateViewComponent();
                    textBoxARStart.UpdateViewComponent();
                    textBoxAREnd.UpdateViewComponent();
                    checkBoxARBlocksMovement.UpdateViewComponent();
                    checkBoxARIsVisible.UpdateViewComponent();
                    enumBoxARColor.UpdateViewComponent();
                    linkBoxARSensorsBlocked.UpdateViewComponent();
                    break;
            }

            DrawingUtility.ResumeDrawing(panelRegion);
        }

        private void showRegionPanel(Int32 id)
        {
            String type = vsgController.GetComponentType(id);

            switch (type)
            {
                case "LandRegion":
                    if (!panelRegion.Controls.Contains(panelLandRegion))
                    {
                        DrawingUtility.SuspendDrawing(panelRegion);
                        panelActiveRegion.Visible = false;
                        panelLandRegion.Visible = true;
                        panelRegion.Controls.Clear();
                        panelRegion.Controls.Add(panelLandRegion);
                        DrawingUtility.ResumeDrawing(panelRegion);
                    }
                    break;

                case "ActiveRegion":
                    if (!panelRegion.Controls.Contains(panelActiveRegion))
                    {
                        DrawingUtility.SuspendDrawing(panelRegion);
                        panelLandRegion.Visible = false;
                        panelActiveRegion.Visible = true;
                        panelRegion.Controls.Clear();
                        panelRegion.Controls.Add(panelActiveRegion);
                        DrawingUtility.ResumeDrawing(panelRegion);
                    }
                    break;
                
                default:
                    panelRegion.Controls.Clear();
                    break;
            }
        }

        private void enumBoxARColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (enumBoxARColor.SelectedItem != null)
            {
                try
                {
                    colorSwatch.BackColor = Color.FromName(enumBoxARColor.SelectedItem.ToString());
                }
                catch (Exception)
                {
                    colorSwatch.BackColor = SystemColors.Control;
                }
            }
        }

        private void customParameterTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}

