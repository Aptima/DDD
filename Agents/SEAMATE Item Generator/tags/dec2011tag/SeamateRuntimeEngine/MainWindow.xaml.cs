using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SeamateAdapter;
using System.Threading;
using SeamateAdapter.DDD;
using Microsoft.Win32;
using SeamateRuntimeEngine.ItemGenerators;

namespace SeamateRuntimeEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void ToggleEnabledDelegate(UIElement element, bool isEnabled);
        private delegate void SingleStringDelegate(TextBox tb, String text);
        private DDDAdapter _ddd; //plug-n-playable interface if we ever want to diverge from DDD
        private Dictionary<String, T_Item> _items;
        private ScheduledItemsList _timelineEvents;
        private const string _APPLICATION_NAME = "SEAMATE Runtime Engine";
        private const string _APPLICATION_COMPILE_DATE = "12-07-2011";

        public MainWindow()
        {
            _items = new Dictionary<string, T_Item>();
            _timelineEvents = new ScheduledItemsList();
            _ddd = new DDDAdapter();
            _ddd.SetTickCallback(new DDDAdapter.TimeTickDelegate(TimeTick));
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {//connect
            String host = tbHostname.Text;
            String shareName = tbShareName.Text;
            String port = tbPort.Text;
            buttonConnect.IsEnabled = false;
            _ddd.StartConnect(host, shareName, Int32.Parse(port), new DDDAdapter.ConnectCompleteDelegate(ConnectComplete));
            /*
            if (!cResult)
            {
                MessageBox.Show("DDD Connection failed");
                return;
            }*/
        }
        protected void ConnectComplete(bool success, String message)
        {
            if (success)
            {
                ToggleEnabled(buttonDisconnect, true);
            }
        }
        private void buttonDisconnect_Click(object sender, RoutedEventArgs e)
        {
            if (_ddd != null)
            {
                _ddd.WaitShutdownConnection();
                ((Button)sender).IsEnabled = false;
                button1.IsEnabled = true;
            }
        }
        private void ToggleEnabled(UIElement e, bool isEnabled)
        {
            if (this.Dispatcher.Thread == Thread.CurrentThread)
            {
                e.IsEnabled = isEnabled;
            }
            else
            {
                this.Dispatcher.Invoke(new ToggleEnabledDelegate(ToggleEnabled), e, isEnabled);
            }
        }
        private void AppendToInfoBox(TextBox e, String appendedText)
        {
            if (this.Dispatcher.Thread == Thread.CurrentThread)
            {
                e.AppendText(appendedText);
                e.ScrollToEnd();
            }
            else
            {
                this.Dispatcher.Invoke(new SingleStringDelegate(AppendToInfoBox), e, appendedText);
            }        
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_ddd != null)
            {
                _ddd.WaitShutdownConnection();
            }
        }

        private void LoadItems(String filePath)
        {
            T_SeamateItems items = ItemParser.ParseItemsConfiguration(filePath);
            foreach (T_Item i in items.Items)
            {
                if (i.Action != null)
                { 
                    foreach(object o in i.Action)
                    {
                        if (o is T_Reveal)
                        {
                            ((T_Reveal)o).TimeAfter *= 1000;
                        }else  if (o is T_Move)
                        {
                            ((T_Move)o).TimeAfter *= 1000;
                        }else  if (o is T_Interaction)
                        {
                            ((T_Interaction)o).TimeAfter *= 1000;
                        }else  if (o is T_ChangeState)
                        {
                            ((T_ChangeState)o).TimeAfter *= 1000;
                        }
                    }
                }

                _items.Add(i.ID, i);
            }
            foreach (ScheduledItem si in items.Timeline)
            {
                si.Time *= 1000;
                _timelineEvents.Add(si);
            }
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            bool? b = ofd.ShowDialog(this);
            if(b != null)
            {
                if (b.Value)
                {
                    String path = ofd.FileName;
                    try
                    {
                        LoadItems(path);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Loading Items file: " + ex.Message);
                        ((Button)sender).IsEnabled = true;
                        return;
                    }
                    labelItemLoadStatus.Content = "Items loaded from " + ofd.SafeFileName;
                    return;
                }
            }
            ((Button)sender).IsEnabled = true;
        }

        private void TimeTick(int time)
        {
                    
            //check timeline for a new item to process
            List<T_Item> items = new List<T_Item>();
            List<ScheduledItem> itemsToProcess = null;// new List<ScheduledItem>();
            itemsToProcess = _timelineEvents.GetItemsUpTo(time);
            if (itemsToProcess == null)
                return;
            foreach(ScheduledItem i in itemsToProcess)
            {
                //i.DM_ID;
                if (_items.ContainsKey(i.ID))
                {
                    String infoText = String.Format("Processing item id={0}; For DM={1}; Current Time={2}\r\n", i.ID, i.DM_ID, i.Time);
                    //AD: TODO: Need to add in specific strings that describe the actions taking place in the item
                    items.Add(_items[i.ID].DeepCopy());
                    T_Item item = _items[i.ID].DeepCopy();
                    StringBuilder sb = new StringBuilder();
                    if (item.Parameters.Crossing)
                    {
                        sb.AppendFormat("Crossing;");
                    }
                    else {
                        sb.AppendFormat("Non-Crossing;");
                    }
                    if (item.Parameters.Groupings == T_Groupings.Two)
                    {
                        sb.AppendFormat("Two-Grouping;");
                    }
                    else
                    {
                        sb.AppendFormat("One-Grouping;");
                    }
                    if (item.Parameters.PlayerResources == T_ResourceAvailability.Available)
                    {
                        sb.AppendFormat("PlayerHasResources;");
                    }
                    else
                    {
                        sb.AppendFormat("PlayerHas_NO_Resources;");
                    }
                    if (item.Parameters.TeammateResources == T_ResourceAvailability.Available)
                    {
                        sb.AppendFormat("TeammateHasResources;");
                    }
                    else
                    {
                        sb.AppendFormat("TeammateHas_NO_Resources;");
                    }
                   // if (item.Parameters.ThreatTypeSpecified)
                   // {
                        if (item.Parameters.ThreatType == T_ThreatType.Imminent)
                        {
                            sb.AppendFormat("Imminent;");
                        }
                        else
                        {
                            sb.AppendFormat("Non-imminent;");
                        }
                    //}
                    if (item.Parameters.Threat == T_Threat.Ambiguous)
                    {
                        sb.AppendFormat("AmbiguousThreat;");
                    }
                    else if (item.Parameters.Threat == T_Threat.Unambiguous)
                    {
                        sb.AppendFormat("UnambiguousThreat;");
                    }
                    else
                    {
                        sb.AppendFormat("Non-Threat;");
                    }

                    infoText = String.Format("{0}\t{1}\r\n", infoText, sb.ToString());
                    if (item.Action.Count() == 0)
                    {
                        List<DDDAdapter.SeamateObject> itemsTriedInThreatGenerator = new List<DDDAdapter.SeamateObject>();
                        while (true) {
                            //Threat generator -- generates or chooses ONE pirate or friendly vessel in DM domain
                            //So either creates a RevealEvent or chooses existing vessel, always makes MoveEvent
                            //Location of this vessel is determined here.
                            ThreatGenerator thrg = new ThreatGenerator(_ddd);
                            itemsTriedInThreatGenerator = thrg.GenerateWithTriedItems(item, i.DM_ID, itemsTriedInThreatGenerator);

                            //Make extra merchants, ensuring at least one if "imminent" threat.  Choose locations for them all. (or choose them by location if reusing)
                            GroupingGenerator gg = new GroupingGenerator(_ddd);
                            gg.Generate(item, i.DM_ID);

                            // For each vessel, picks course/destination, IFF, and speed based on ambiguity table, grouping, and crossing constraints.
                            AmbiguityGenerator ag = new AmbiguityGenerator(_ddd);
                            bool worked = ag.SpecialGenerate(item, i.DM_ID);
                            if (worked)
                                break;
                            else 
                                item.Action = new object[0];

                        }

                        //Intercept course plotted if necessary, now we know all the other vessels and their positions and courses.
                        ThreatTypeGenerator ttg = new ThreatTypeGenerator(_ddd);
                        ttg.Generate(item, i.DM_ID);
                    }

                    
                    foreach (ActionBase action in item.Action)
                    {
                        if (action != null && !(action is T_ScriptedItem))
                            _ddd.AddDDDEventToQueue(action.ToDDDEvent(time, _ddd));

                        //ADAMS STUB: Might not work 100%, Lisa please fix if it won't work.
                        String stimType = "";
                        String objectID = "";
                        if (action is T_Reveal)
                        {
                            stimType = "Reveal";
                            objectID = ((T_Reveal)action).ID;
                            if (((T_Reveal)action).Location.Item is Aptima.Asim.DDD.CommonComponents.DataTypeTools.LocationValue)
                            {
                                if (((Aptima.Asim.DDD.CommonComponents.DataTypeTools.LocationValue)((T_Reveal)action).Location.Item).exists == false)
                                {
                                    Console.WriteLine("Uh oh, null location");
                                }
                            }
                            String IFF = "";
                            if (((T_Reveal)action).StartupParameters.Items.Length > 0)
                            {
                                for (int c = 0; c < ((T_Reveal)action).StartupParameters.Items.Length; c++)
                                {
                                    if (((T_Reveal)action).StartupParameters.Items[c] == "ObjectName")
                                    {
                                        IFF = "Squawking=" + ((T_Reveal)action).StartupParameters.Items[c + 1];
                                    }
                                    c++; //get past the "value" item, which is the even numbered entry
                                }
                            }
                            infoText = String.Format("{0}\tREVEAL; object_id={1}; Owned_by={2}; Revealed_at={3}\r\n", infoText, objectID, ((T_Reveal)action).Owner, ((T_Reveal)action).Location.ToString());
                        
                        }
                        else if (action is T_Move)
                        {
                            stimType = "Move";
                            objectID = ((T_Move)action).ID;
                            infoText = String.Format("{0}\tMOVE; object_id={1}; Throttle={2:0.00}; Destination={3}\r\n", infoText, objectID, ((T_Move)action).Throttle, ((T_Move)action).Location.ToString());
                        }
                        else if (action is T_ScriptedItem)
                        {

                            //TODO: check to see that the object is not dead -Lisa
                            objectID = ((T_ScriptedItem)action).ID;
                            stimType = ((T_ScriptedItem)action).ActionType;
                        }

                        if (stimType != "")
                        {
                            _ddd.SendStimulusEvent(i.ID, i.DM_ID, objectID, stimType, time);
                        }
                    }
                    AppendToInfoBox(tbInfoBox, infoText);
                }

            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, this.ToAboutString(), "About " + _APPLICATION_NAME);
        }
        private String ToAboutString()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            StringBuilder sb = new StringBuilder();
            sb.Append(_APPLICATION_NAME + "\r\n");
            sb.Append("Version: "+ assembly.GetName().Version.ToString());
            sb.Append("\r\nCompiled on: " + _APPLICATION_COMPILE_DATE);
            return sb.ToString();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text File (*.txt)|*.txt";
            sfd.InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;
            sfd.OverwritePrompt = true;
            sfd.Title = "Save info log file";
            bool? b = sfd.ShowDialog(this);
            if (b != null)
            {
                if (b.Value == true)
                {
                    System.IO.File.WriteAllText(sfd.FileName, tbInfoBox.Text);
                }
            }
        }
    }
}
