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

using System.Xml;
using System.Xml.XPath;

using AGT;
using AGT.Scenes;
using AGT.Motion;

using Microsoft.DirectX;

using System.Threading;
using VSG.PreviewSimulator;

using AGT.Mapping;


namespace VSG.ViewComponentPanels
{

    public partial class PreviewViewComponentPanel : AME.Views.View_Component_Packages.ViewComponentPanel, AME.Views.View_Component_Packages.IViewComponentPanel, IDisposable
    {
        private ViewPanelHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private int _lastTick = 0;
        private int _warning_count = 0;
        private int _revealed_unit_count = 0;

        private static string _description = "Press play to begin simulation.";

        private bool SceneInitialized = false;

        private int _rootID = -1;
        private VSGController vsgController;

        private static string Playfield_XPath = "/Components/Component/Component[@Type='Playfield']";

        private static string Mapfile_Xpath = Playfield_XPath + "//*[@displayedName='Map Filename']";
        private static string Iconlib_XPath = Playfield_XPath + "//*[@displayedName='Icon Library']";
        private static string HorizontalScale_XPath = Playfield_XPath + "//*[@displayedName='Horizontal Scale']";
        private static string VerticalScale_XPath = Playfield_XPath + "//*[@displayedName='Vertical Scale']";

        private static string Species_XPath = "Components/Component[@Type='CreateEvent' and @Name='{0}']/Component[@Type='Species']";

        private static string DM_XPath = ".//*[@Type='DecisionMaker']";

        private static string DM_Color_XPath = ".//*[@displayedName='Color']";
        
        private static string CreateEvent_XPath = ".//*[@Type='CreateEvent']";

        private static string RevealEvent_XPath = "./Component[@Type='RevealEvent']";
        private static string RevealEventTime_XPath = ".//Parameter[@category='RevealEvent']/Parameter[@displayedName='Time']";

        private static string RevealEventX_XPath = ".//Parameter[@category='InitialLocation']/Parameter[@displayedName='X']";
        private static string RevealEventY_XPath = ".//Parameter[@category='InitialLocation']/Parameter[@displayedName='Y']";

        private static string MoveEvent_XPath = "./Component[@Type='MoveEvent']";

        private static string MoveEventTime_XPath = ".//Parameter[@displayedName='Time']";
        private static string MoveEventThrottle_XPath = ".//Parameter[@displayedName='Throttle']";

        private static string MoveEventDestX_XPath = ".//Parameter[@displayedName='X']";
        private static string MoveEventDestY_XPath = ".//Parameter[@displayedName='Y']";

        private static string ReiterateEvent_XPath = "//*[@Type='ReiterateEvent']";
        private static string ReiterateEventTime_XPath = ".//Parameter[@displayedName='Time']";

        private VSG.PreviewSimulator.PreviewSimulator _sim;

        public PreviewViewComponentPanel()
        {
            myHelper = new ViewPanelHelper(this, UpdateType.Component);

            InitializeComponent();
            customTabPage1.Description = _description;

            _sim = new VSG.PreviewSimulator.PreviewSimulator(agT_CanvasControl1);
            _sim.TickUpdateCallback = new TickUpdateDelegate(this.UpdateUI);

            vsgController = (VSGController)AMEManager.Instance.Get("VSG");
            vsgController.RegisterForUpdate(this);

            comboBox1.SelectedIndex = 3;
            
        }

        public void SuspendScene()
        {
            if (SceneInitialized && !agT_CanvasControl1.Suspended)
            {
                this.agT_CanvasControl1.SuspendFramework();
                _sim.StopSim();
                ResetInterface();
            }
        }
        public void ResumeScene()
        {
            if (SceneInitialized && agT_CanvasControl1.Suspended)
            {
                this.agT_CanvasControl1.ResumeFramework();
            }
        }
        public void ReleaseScene()
        {
            _sim.StopSim();
            ResetInterface();
            this.agT_CanvasControl1.SuspendFramework();
            agT_CanvasControl1.Dispose();
        }

        
        public void UpdateUI()
        {
            if (!InvokeRequired)
            {
                lock (this)
                {
                    customTabPage1.Description = string.Format("Current Tick: {0}", _sim.CurrentTick);
                }
            }
            else
            {
                BeginInvoke(new TickUpdateDelegate(this.UpdateUI));
            }
        }


        #region AME Integration

        public void UpdateViewComponent()
        {
            button1.Enabled = false;
            
            if (this.Parent != null)
            {
                ComponentOptions comp = new ComponentOptions();
                comp.CompParams = true;

                IXPathNavigable document = this.vsgController.GetComponentAndChildren(_rootID, "Scenario", comp);
                XPathNavigator navigator = document.CreateNavigator();

                ResetWarnings();
                _sim.ClearScenario();
                if (ReadScenarioResources(navigator))
                {
                    if (ReadUnits(navigator) > 0)
                    {
                        ReadReiterate(navigator);
                        button1.Enabled = true;
                    }
                    else
                    {
                        WritelnWarningMessage("No \'Revealed\' units have been detected.  Nothing available to Preview.");
                    }
                    if (SceneInitialized)
                    {
                        _sim.SetSceneState(AGT.GameFramework.SceneState.INIT);
                        agT_CanvasControl1.RestartCurrentScene();
                    }
                    else
                    {
                        agT_CanvasControl1.StartFramework();
                        SceneInitialized = true;
                    }
                }
                WriteWarningCount();
            }
        }

        private void ResetWarnings()
        {
            richTextBox1.Clear();
            _warning_count = 0;
        }


        private void OverWriteMessage(int number, int size)
        {
            richTextBox1.Focus();
            richTextBox1.Select(richTextBox1.Text.Length - size, size);
            richTextBox1.SelectedText = string.Format("{0, -" + size + "}", number);
            richTextBox1.ScrollToCaret();
        }

        private void WritelnMessage(string message)
        {
            richTextBox1.AppendText(message + "\n");
            richTextBox1.ScrollToCaret();
        }
        private void WritelnWarningMessage(string message)
        {
            WritelnMessage("WARNING: "+message);
            _warning_count++;
        }
        private void WriteWarningCount()
        {
            WritelnMessage(string.Format("({0}) Warnings.", _warning_count));
            if (_warning_count > 0)
            {
                MessageBox.Show("Please refer to Preview Messages for details on the warnings and suggested resolution.", 
                    string.Format("{0} Warnings Encountered", _warning_count),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #region XML Processing
        private bool ReadScenarioResources(XPathNavigator navigator)
        {
            string map_file = string.Empty;
            string icon_library = string.Empty;
            string hor_scaleStr = string.Empty;
            string ver_scaleStr = string.Empty;

            bool retval = true;

            XPathNodeIterator map = navigator.Select(Mapfile_Xpath);
            if (map.MoveNext())
            {
                XPathNavigator node = map.Current;
                map_file = VSG.ConfigFile.VSGConfig.MapDir + @"\" + node.GetAttribute("value", node.NamespaceURI);
                WritelnMessage(string.Format("Map File: {0}", map_file));
            }
            else
            {
                map_file = string.Empty;
                WritelnWarningMessage(string.Format("Unable to detect a Map Selection, Preview will be unavailable."));
                retval = false;
            }

            XPathNodeIterator icon = navigator.Select(Iconlib_XPath);
            if (icon.MoveNext())
            {
                XPathNavigator node = icon.Current;
                icon_library = VSG.ConfigFile.VSGConfig.IconDir + @"\" + node.GetAttribute("value", node.NamespaceURI) + ".dll";
               WritelnMessage(string.Format("Icon Library: {0}", icon_library));
            }
            else
            {
                icon_library = string.Empty;
                WritelnWarningMessage("Unable to detect an Icon Library, Preview will be unavailable.");
                retval = false;
            }

            XPathNodeIterator hor_scale = navigator.Select(HorizontalScale_XPath);
            if (hor_scale.MoveNext())
            {
                XPathNavigator node = hor_scale.Current;
                hor_scaleStr = node.GetAttribute("value", node.NamespaceURI);
                WritelnMessage(string.Format("Horizontal Scale: {0}", hor_scaleStr));
            }
            else
            {
                hor_scaleStr = "1";
                WritelnWarningMessage("Unable to detect a Horizontal Scale, default scale of 1 will be used.");
            }

            XPathNodeIterator ver_scale = navigator.Select(VerticalScale_XPath);
            if (ver_scale.MoveNext())
            {
                XPathNavigator node = ver_scale.Current;
                ver_scaleStr = node.GetAttribute("value", node.NamespaceURI);
                WritelnMessage(string.Format("Vertical Scale: {0}", ver_scaleStr));
            }
            else
            {
                ver_scaleStr = "1";
                WritelnWarningMessage("Unable to detect a Vertical Scale, default scale of 1 will be used.");
            }

            // Take the average of the two scales, horizontal and vertical.
            _sim.MapScale = (float)((Double.Parse(hor_scaleStr) + Double.Parse(ver_scaleStr)) * .5f );

            if (retval)
            {
                return _sim.SetSceneResources(icon_library, map_file);
            }
            return retval;
        }

        private string GetUnitName(int id)
        {
            ComponentOptions comp = new ComponentOptions();
            comp.CompParams = true;
            IXPathNavigable event_id = this.vsgController.GetComponentAndChildren(id, "EventID", comp);
            XPathNavigator event_nav = event_id.CreateNavigator();

            XPathNavigator unit_node = event_nav.SelectSingleNode(".//Component[@Type='CreateEvent']");
            if (unit_node != null)
            {
                return unit_node.GetAttribute("Name", unit_node.NamespaceURI);
            }

            return string.Empty;
        }

        private double GetFullyFunctionalMaxSpeed(int id)
        {
            ComponentOptions comp = new ComponentOptions();
            comp.CompParams = true;

            IXPathNavigable species_type = this.vsgController.GetComponentAndChildren(id, "SpeciesType", comp);
            XPathNavigator species_nav = species_type.CreateNavigator();

            // Foreach species in list check the MaxSpeed
            XPathNodeIterator listofSpecies;
            listofSpecies = species_nav.Select("//*[@Type='Species']");

            foreach (XPathNavigator node in listofSpecies)
            {
                int new_id = Convert.ToInt32(node.GetAttribute("ID", node.NamespaceURI));
                comp.LevelDown = 1;
                IXPathNavigable state = this.vsgController.GetComponentAndChildren(new_id, "Scenario", comp);

                XPathNavigator state_nav = state.CreateNavigator();

                XPathNavigator override_maxspeed = state_nav.SelectSingleNode(".//*[@Name='FullyFunctional']/ComponentParameters/Parameter/Parameter[@displayedName='OverrideMaxSpeed']");
                bool bOverrideMaxSpeed = Convert.ToBoolean(override_maxspeed.GetAttribute("value", override_maxspeed.NamespaceURI));

                XPathNavigator existing_species = state_nav.SelectSingleNode(".//Parameter[@category='Species']/Parameter[@displayedName='ExistingSpecies']");
                bool bExistingSpecies = Convert.ToBoolean(existing_species.GetAttribute("value", existing_species.NamespaceURI));

                if (bOverrideMaxSpeed || (!bExistingSpecies))
                {
                    XPathNavigator speed_node = state_nav.SelectSingleNode(".//*[@Name='FullyFunctional']/ComponentParameters/Parameter/Parameter[@displayedName='MaxSpeed']");
                    if (speed_node != null)
                    {
                        return Convert.ToDouble(speed_node.GetAttribute("value", speed_node.NamespaceURI));
                    }
                }
            }
            return 0;
        }


        private string GetFullyFunctionalIcon(int id)
        {
            ComponentOptions comp = new ComponentOptions();
            comp.CompParams = true;

            IXPathNavigable species_type = this.vsgController.GetComponentAndChildren(id, "SpeciesType", comp);
            XPathNavigator species_nav = species_type.CreateNavigator();

            // Foreach species in list check the MaxSpeed
            XPathNodeIterator listofSpecies;
            listofSpecies = species_nav.Select("//*[@Type='Species']");

            foreach (XPathNavigator node in listofSpecies)
            {
                int new_id = Convert.ToInt32(node.GetAttribute("ID", node.NamespaceURI));
                comp.LevelDown = 1;
                IXPathNavigable state = this.vsgController.GetComponentAndChildren(new_id, "Scenario", comp);

                XPathNavigator state_nav = state.CreateNavigator();

                XPathNavigator override_icon = state_nav.SelectSingleNode(".//*[@Name='FullyFunctional']/ComponentParameters/Parameter/Parameter[@displayedName='OverrideIcon']");
                bool bOverrideIcon = Convert.ToBoolean(override_icon.GetAttribute("value", override_icon.NamespaceURI));

                XPathNavigator existing_species = state_nav.SelectSingleNode(".//Parameter[@category='Species']/Parameter[@displayedName='ExistingSpecies']");
                bool bExistingSpecies = Convert.ToBoolean(existing_species.GetAttribute("value", existing_species.NamespaceURI));

                if (bOverrideIcon || (!bExistingSpecies))
                {
                    XPathNavigator icon_node = state_nav.SelectSingleNode(".//*[@Name='FullyFunctional']/ComponentParameters/Parameter/Parameter[@displayedName='Icon']");
                    if (icon_node != null)
                    {
                        return icon_node.GetAttribute("value", icon_node.NamespaceURI);
                    }
                }
            }
            return "Unknown.png";
        }

        private int ReadUnits(XPathNavigator navigator)
        {
            int unit_count = 0;
            string DMColor = string.Empty;
            string IconName = string.Empty;
          
            XPathNodeIterator listofRootElements;
            listofRootElements = navigator.Select(CreateEvent_XPath);
            WritelnMessage("Loading Units: 00000");

            foreach (XPathNavigator node in listofRootElements)
            {
                string unit_name = node.GetAttribute("Name", node.NamespaceURI);
                int id = Convert.ToInt32(node.GetAttribute("ID", node.NamespaceURI));

                XPathNavigator dm_node = node.SelectSingleNode(DM_XPath);
                string dm_name = dm_node.GetAttribute("Name", node.NamespaceURI);
                try
                {
                    DMColor = dm_node.SelectSingleNode(DM_Color_XPath).GetAttribute("value", dm_node.NamespaceURI);
                }
                catch (Exception)
                {
                    WritelnWarningMessage(string.Format("No color assigned to DM \'{0}\', Preview will be unavailable."));
                    return 0;
                }
                if (DMColor != null)
                {
                    string icon_name = string.Empty;

                    ComponentOptions comp = new ComponentOptions();
                    comp.CompParams = true;
                    IXPathNavigable create_event_doc = this.vsgController.GetComponentAndChildren(id, "CreateEventKind", comp);
                    XPathNavigator create_event_nav = create_event_doc.CreateNavigator();

                    XPathNavigator species_node = create_event_nav.SelectSingleNode(string.Format(Species_XPath, unit_name));

                    if (species_node != null)
                    {
                        string species_name = species_node.GetAttribute("Name", species_node.NamespaceURI);


                        int id2 = Convert.ToInt32(species_node.GetAttribute("ID", species_node.NamespaceURI));
                        _sim.UnitMaxSpeeds.Add(unit_name, GetFullyFunctionalMaxSpeed(id2));

                      
                        icon_name = GetFullyFunctionalIcon(id2);
                        if (icon_name == "Unknown.png")
                        {
                            WritelnWarningMessage(string.Format("No icon defined for Unit \'{0}\'.  This may be corrected by setting the Icon " +
                                "parameter for the Fully Functional State of species \'{1}\', in " +
                                "the Scenario Elements View.", unit_name, species_name));
                        }
                    }
                    else
                    {
                        WritelnWarningMessage(string.Format("No Species Type defined for Unit {0}. This may be corrected in the Scenario Director View.", unit_name));
                        _sim.UnitMaxSpeeds.Add(unit_name, 0);
                    }

                    // Get Reveal Event;
                    XPathNavigator reveal_event = node.SelectSingleNode(RevealEvent_XPath);
                    if (reveal_event != null)
                    {
                        RevealEvent reveal = Xml_ReadRevealEvent(node, unit_name);
                        if (reveal != RevealEvent.Empty)
                        {
                            unit_count ++;
                            reveal.Icon = icon_name;
                            reveal.ColorName = DMColor;

                            _sim.AddEvent(reveal);
                            Xml_ReadMoveEvent(node.Select(MoveEvent_XPath), null);
                        }
                    }
                    OverWriteMessage(unit_count, 5);
                }
            }
            WritelnMessage("");
            return unit_count;
        }


        private void ReadReiterate(XPathNavigator navigator)
        {
            XPathNodeIterator listofRootElements;
            listofRootElements = navigator.Select(ReiterateEvent_XPath);

            int count = 0;
            WritelnMessage("Loading Unit Reiterate Events: 00000\n");

            foreach (XPathNavigator node in listofRootElements)
            {
                Xml_ReadReiterate(node);
                count++;
                OverWriteMessage(count, 5);
            }
            WritelnMessage("");
        }

        private RevealEvent Xml_ReadRevealEvent(XPathNavigator navigator, string unit_name)
        {
            RevealEvent reveal_event = new RevealEvent();

            reveal_event.Name = unit_name;
            reveal_event.ID = navigator.GetAttribute("ID", navigator.NamespaceURI);
            if (reveal_event.ID == string.Empty)
            {
                return RevealEvent.Empty;
            }


            XPathNavigator time_node = navigator.SelectSingleNode(RevealEventTime_XPath);
            try
            {
                reveal_event.Time = (int)Convert.ToDecimal(time_node.GetAttribute("value", time_node.NamespaceURI));
            }
            catch (Exception)
            {
                reveal_event.Time = 0;
            }

            XPathNavigator x_node = navigator.SelectSingleNode(RevealEventX_XPath);
            try
            {
                reveal_event.X = (float)(Convert.ToDouble(x_node.GetAttribute("value", x_node.NamespaceURI)));
                reveal_event.X = UTM_Mapping.HorizontalMetersToPixels(reveal_event.X); 
            }
            catch (Exception)
            {
                reveal_event.X = 0;
            }

            XPathNavigator y_node = navigator.SelectSingleNode(RevealEventY_XPath);
            try
            {
                reveal_event.Y = (float)(Convert.ToDouble(y_node.GetAttribute("value", y_node.NamespaceURI)));
                reveal_event.Y = UTM_Mapping.VerticalMetersToPixels(reveal_event.Y);
            }
            catch (Exception)
            {
                reveal_event.Y = 0;
            }

            reveal_event.Z = 0;

            return reveal_event;
        }

        private void Xml_ReadMoveEvent(XPathNodeIterator iterator, AbstractPreviewEvent abstract_event)
        {
            int last_move_timetick = -1;

            if (iterator != null)
            {
                foreach (XPathNavigator move_node in iterator)
                {
                    MoveEvent move_event = new MoveEvent();

                    move_event.MoveDuration = -1;

                    move_event.ID = move_node.GetAttribute("ID", move_node.NamespaceURI);
                    move_event.Name = GetUnitName(Convert.ToInt32(move_event.ID));

                    XPathNavigator time_node = move_node.SelectSingleNode(MoveEventTime_XPath);
                    try
                    {
                        move_event.Time = (int)Convert.ToDecimal(time_node.GetAttribute("value", time_node.NamespaceURI));
                    }
                    catch (Exception)
                    {
                        move_event.Time = 0;
                    }


                    XPathNavigator throttle_node = move_node.SelectSingleNode(MoveEventThrottle_XPath);
                    try
                    {
                        move_event.Throttle = (float)(Convert.ToDecimal(throttle_node.GetAttribute("value", throttle_node.NamespaceURI)));
                    }
                    catch (Exception)
                    {
                        move_event.Throttle = 100f;
                    }

                    XPathNavigator x_node = move_node.SelectSingleNode(MoveEventDestX_XPath);
                    try
                    {
                        move_event.X = (float)(Convert.ToDouble(x_node.GetAttribute("value", x_node.NamespaceURI)));
                        move_event.X = UTM_Mapping.HorizontalMetersToPixels(move_event.X);
                    }
                    catch (Exception)
                    {
                        move_event.X = 0f;
                    }

                    XPathNavigator y_node = move_node.SelectSingleNode(MoveEventDestY_XPath);
                    try
                    {
                        move_event.Y = (float)(Convert.ToDouble(y_node.GetAttribute("value", y_node.NamespaceURI)));
                        move_event.Y = UTM_Mapping.VerticalMetersToPixels(move_event.Y);
                    }
                    catch (Exception)
                    {
                        move_event.Y = 0f;
                    }

                    move_event.Z = 0f;

                    if (last_move_timetick >= 0)
                    {
                        MoveEvent mv = _sim.FindMoveEvent(last_move_timetick, move_event.Name);
                        if (mv != MoveEvent.Empty)
                        {
                            mv.MoveDuration = move_event.Time - mv.Time;
                        }
                    }

                    if (abstract_event == null)
                    {
                        _sim.AddEvent(move_event);
                    }
                    else
                    {
                        if (abstract_event is ReiterateEvent)
                        {
                            _sim.AddEvent(move_event, (ReiterateEvent)abstract_event);
                        }
                    }
                    last_move_timetick = move_event.Time;
                }
            }
        }

        private void Xml_ReadReiterate(XPathNavigator navigator)
        {
            if (navigator != null)
            {
                ReiterateEvent reiterate = new ReiterateEvent();
                reiterate.Name = "Reiterate";
                reiterate.Parent = _sim;
                XPathNavigator time_node = navigator.SelectSingleNode(ReiterateEventTime_XPath);
                try
                {
                    reiterate.Time = (int)Convert.ToDecimal(time_node.GetAttribute("value", time_node.NamespaceURI));
                    Xml_ReadMoveEvent(navigator.Select(MoveEvent_XPath), reiterate);
                    _sim.AddEvent(reiterate);
                }
                catch (Exception)
                {
                    reiterate.Time = 0;
                }
            }
        }
        #endregion

        public int RootId
        {
            get
            {
                return _rootID;
            }
            set
            {
                _rootID = value;
            }
        }
        #endregion
        
        

        private void ResetInterface()
        {
            button1.Text = "Start Preview";
            numericUpDown2.Enabled = true;
            numericUpDown1.Enabled = true;
            label1.Enabled = true;
            label3.Enabled = true;
            customTabPage1.Description = _description;
            _sim.PauseSim();

            _sim.ClearScene();
            _sim.StepTo((int)numericUpDown1.Value, false);
        }

        #region Winforms Event Handlers
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start Preview")
            {
                button1.Text = "Stop";
                numericUpDown2.Enabled = false;
                numericUpDown1.Enabled = false;
                label1.Enabled = false;
                label3.Enabled = false;

                _sim.ClearScene();
                _sim.UnpauseSim();

                _sim.StepTo((int)numericUpDown1.Value, true);
                _sim.StartSim((int)numericUpDown1.Value);
            }
            else
            {
                button1.Text = "Start Preview";
                numericUpDown2.Enabled = true;
                numericUpDown1.Enabled = true;
                label1.Enabled = true;
                label3.Enabled = true;
                customTabPage1.Description = _description;
                _sim.PauseSim();
                _sim.StopSim();
                _sim.ClearScene();
                _sim.StepTo((int)numericUpDown1.Value, false);
            }

        }
        
        private float ConvertPercentString(string input)
        {
            input = input.Replace("%", string.Empty);
            if (input != string.Empty)
            {
                return (float)(Convert.ToDouble(input) / 100);
            }
            return 1.0f;
        }
        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            float pct = ConvertPercentString(comboBox1.Text);
            if (pct > 0)
            {
                _sim.SetSceneScale(pct);
            }
        }

        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                float pct = ConvertPercentString(comboBox1.Text);
                if (pct > 0)
                {
                    _sim.SetSceneScale(pct);
                }
            }
        }
        #endregion

        private void numericUpDown1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _sim.ClearScene();
                _sim.StepTo((int)numericUpDown1.Value, false);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _sim.ClearScene();
            _sim.StepTo((int)numericUpDown1.Value, false);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            _sim.SimSpeed = (int)numericUpDown2.Value;
        }



        #region IDisposable Members

        public void Dispose()
        {
            if (_sim != null)
            {
                _sim.Dispose();
            }
        }

        #endregion

    }









}

