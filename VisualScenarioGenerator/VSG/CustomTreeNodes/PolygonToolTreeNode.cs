using System;
using System.Collections.Generic;
using System.Text;
using AME.Controllers;
using AME.Views.View_Components;
using System.Xml.XPath;
using AME.Nodes;
using AME.Controllers.Base.Data_Structures;
using System.Windows.Forms;
using System.Drawing;

namespace ApplicationNodes
{
    public class PolygonToolTreeNode : ProcessingNode
    {
        public PolygonToolTreeNode(int p_id, String p_type, String p_name, String p_eType, String p_imagePath, String p_linkType)
            : base(p_id, p_type, p_name, p_eType, p_imagePath, p_linkType)
        {
        }

        private Diagram saveDrag = null;

        public override void process(Object dragTarget)
        {
            if (dragTarget is Diagram)
            {
                Diagram diagram = (Diagram)dragTarget;

                saveDrag = diagram; // save the diagram and instance reference for event below

                PolygonDrawingTool polyTool = new PolygonDrawingTool(diagram);

                diagram.Tool = polyTool;
                polyTool.PolygonCreatedWithPoints += new PolygonDrawingTool.OnPolygonCreatedWithPoints(polyTool_PolygonCreatedWithPoints);
                diagram.Tool.Start();
            }
        }

        private String ParameterToUpdate = "Location.Polygon Points";

        private void polyTool_PolygonCreatedWithPoints(string PointString)
        {
            if (saveDrag != null && NodeID != -1)
            {
                try
                {
                    if (saveDrag != null)
                    {
                        // this will throw an exception if an invalid string is used
                        //List<PointF> parsed = FunctionHelper.ParsePolygonString(PointString, saveDrag.CoordinateTransformer);

                        saveDrag.Controller.UpdateParameters(NodeID, ParameterToUpdate, PointString, eParamParentType.Component);
                        
                        // don't force link
                        if (!saveDrag.Controller.LinkExists(saveDrag.RootID, NodeID, saveDrag.DiagramName))
                        {
                            Int32 linkId = saveDrag.Controller.Connect(saveDrag.RootID, saveDrag.RootID, NodeID, saveDrag.DiagramName);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Invalid format for vertex list", e.Message);
                }
            }
        }
    }
}