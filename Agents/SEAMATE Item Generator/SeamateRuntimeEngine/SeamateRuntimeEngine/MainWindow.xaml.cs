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
using SeamateAdapter.ItemGenerators;
using AdaptiveSelector;

namespace SeamateAdapter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ICloseable
    {
        private class CPEPair
        {
            public double FF_CPE = 0.0;
            public double TT_CPE = 0.0;
            public int LastTimeUpdated = -1;
            public String DM_ID = "";
            public CPEPair()
            { }
            public CPEPair(String dmId, double ffCpe, double ttCpe, int time)
            {
                DM_ID = dmId;
                FF_CPE = ffCpe;
                TT_CPE = ttCpe;
                LastTimeUpdated = time;
            }
        }
        private delegate void ToggleEnabledDelegate(UIElement element, bool isEnabled);
        private delegate void SingleStringDelegate(TextBox tb, String text);
        private DDDAdapter _ddd; //plug-n-playable interface if we ever want to diverge from DDD
        private Dictionary<String, T_Item> _items;
        private Dictionary<String, T_Item> _ptItems; //for hard coded pre-test items, not to be repeated in the dynamic scen
        private ScheduledItemsList _timelineEvents;
        private const string _APPLICATION_NAME = "SEAMATE Runtime Engine";
        private const string _APPLICATION_COMPILE_DATE = "05-21-2012";
        private String _dddHostname = Environment.MachineName;
        private int _dddPort = 1;
        private String _dddShare = "DDDClient";
        private int _time = 0;
        private Dictionary<String, CPEPair> _dmCPEs;
        private AdaptiveSelector.AdaptiveSelector _itemSelector;
        private int _lastCPEUpdateTime = -1;
        private Boolean _readyToSendItems = true;//default to true so we always send the first scheduled item
        public MainWindow()
        {
            _items = new Dictionary<string, T_Item>();
            _ptItems = new Dictionary<string, T_Item>();
            _timelineEvents = new ScheduledItemsList();
            _dmCPEs = new Dictionary<string, CPEPair>();
            _ddd = new DDDAdapter();
            _ddd.SetTickCallback(new DDDAdapter.TimeTickDelegate(TimeTick));
            InitializeComponent();
            LoadCommandLineArgs();
            tbHostname.Text = _dddHostname;
            tbPort.Text = _dddPort.ToString();
            tbShareName.Text = _dddShare;
            _itemSelector = new AdaptiveSelector.AdaptiveSelector(new ChallengeMethod(), "");
        }

        private void LoadCommandLineArgs()
        {
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                String hostname, port, shareFolder;
                try
                {
                    hostname = Environment.GetCommandLineArgs()[1];
                    _dddHostname = hostname;
                    port = Environment.GetCommandLineArgs()[2];
                    _dddPort = Int32.Parse(port);
                    shareFolder = Environment.GetCommandLineArgs()[3];
                    _dddShare = shareFolder;

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error reading command line args");
                }
            }
        }
        private bool _hasFinishedPreTest = false;
        private void UpdateCPE(String dmId, double ffCpe, double ttCpe, int time)
        { 
            
            if (!_dmCPEs.ContainsKey(dmId))
            {
                _dmCPEs.Add(dmId, new CPEPair(dmId, ffCpe, ttCpe, time));
            }
            else
            {
                _dmCPEs[dmId].FF_CPE = ffCpe;
                _dmCPEs[dmId].TT_CPE = ttCpe;
                _dmCPEs[dmId].LastTimeUpdated = time;
                
            }
            _lastCPEUpdateTime = time;
            AppendToInfoBox(tbInfoBox, String.Format("{0} CPEs updated to: FindFix={1}, TrackTarget={2}\r\n", dmId, ffCpe, ttCpe));
            //if they have a queued item waiting, send it now!

            if (_timelineEvents.Count() > 0 && !_hasFinishedPreTest)
            {
                _readyToSendItems = true; //next time tick, will check for next scheduled item
                return; //only adaptively choose if there are no more scheduled items remaining.
            }
            _hasFinishedPreTest = true;
            //Do stuff!
            List<int> usedItemIds = new List<int>();
            CellRange nextItemRange = _itemSelector.GetNextItem(ffCpe, ttCpe);
            T_Item nextItem = SelectNextItemByRange(nextItemRange, dmId);
            int failedAttempts = 0;
            while (nextItem == null && _items.Count > usedItemIds.Count)//0)
            {failedAttempts++;
                usedItemIds.Add(nextItemRange.CellNumber);
                //get a similar cell range and try again
                nextItemRange = _itemSelector.GetNextItem(ffCpe, ttCpe, usedItemIds, failedAttempts);
                nextItem = SelectNextItemByRange(nextItemRange, dmId);
                
            }
            if (nextItem == null)
            {
                Console.WriteLine("Serious issues where we can't find a good fit");
                nextItem = _items[Math.Min(_items.Count, 5).ToString()];
            }
            //Thread.Sleep(1000); //TEMP
            ScheduledItem si = new ScheduledItem();
            si.DM_ID = dmId;
            si.ID = nextItem.ID;
            si.Time = time;
            _timelineEvents.Add(si);
            _readyToSendItems = true;
            //SendItem(nextItem, dmId, time);
            AppendToInfoBox(tbInfoBox, String.Format("{3}: Next Item ({4}) selected for {0}: FindFixDifficulty={1}, TrackTargetDifficulty={2}\r\n", dmId, nextItem.Parameters.FF_Difficulty, nextItem.Parameters.TT_Difficulty, time, nextItem.ID));
        }
        private T_Item SelectNextItemByRange(CellRange cellInfo, String dmId)
        {
            if (_items == null)
                return null;
            List<T_Item> matchingItems = new List<T_Item>();
            int ind = 0;
            foreach (T_Item item in _items.Values)
            {
                if (item.Parameters.FF_Difficulty >= cellInfo.Dimension1Start && item.Parameters.FF_Difficulty <= cellInfo.Dimension1End &&
                    item.Parameters.TT_Difficulty >= cellInfo.Dimension2Start && item.Parameters.TT_Difficulty <= cellInfo.Dimension2End)
                {
                    matchingItems.Add(item.DeepCopy());
                }
            }
            if (matchingItems.Count == 0)
                return null;
            Random r = new Random();
            ind = Convert.ToInt32(Math.Round(r.NextDouble() * (matchingItems.Count()-1)));
            return matchingItems[ind];
        }
        private void SendItem(T_Item item, String dmId, int time)
        {
            //AppendToInfoBox(tbInfoBox, String.Format("{3}: Next Item ({4}) selected for {0}: FindFixDifficulty={1}, TrackTargetDifficulty={2}\r\n", dmId, nextItem.Parameters.FF_Difficulty, nextItem.Parameters.TT_Difficulty, time, nextItem.ID));
            String infoText = String.Format("{3}: Next Item ({4}) selected for {0}: FindFixDifficulty={1}, TrackTargetDifficulty={2}\r\n", dmId, item.Parameters.FF_Difficulty, item.Parameters.TT_Difficulty, time, item.ID);
            _ddd.SendItemInfo(dmId, item.ID, item.Parameters.FF_Difficulty, item.Parameters.TT_Difficulty, time);
            StringBuilder sb = new StringBuilder();
            if (item.Parameters.Crossing)
            {
                sb.AppendFormat("Crossing;");
            }
            else
            {
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

                PirateGenerator pirateGenerator = new PirateGenerator(_ddd);
                pirateGenerator.Generate(item, dmId);

                MerchantGenerator merchantGenerator = new MerchantGenerator(_ddd);
                merchantGenerator.Generate(item, dmId);

                CourseGenerator courseGenerator = new CourseGenerator(_ddd);
                courseGenerator.Generate(item, dmId);

                InterceptGenerator interceptGenerator = new InterceptGenerator(_ddd);
                interceptGenerator.Generate(item, dmId);

            }


            foreach (ActionBase action in item.Action)
            {
                if (action != null && !(action is T_ScriptedItem))
                    _ddd.AddDDDEventToQueue(action.ToDDDEvent(_time, _ddd));

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
                    _ddd.SendStimulusEvent(item.ID, dmId, objectID, stimType, _time, item.Parameters.FF_Difficulty, item.Parameters.TT_Difficulty);
                }
            }
            AppendToInfoBox(tbInfoBox, infoText);
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {//connect
            String host = tbHostname.Text;
            String shareName = tbShareName.Text;
            String port = tbPort.Text;
            buttonConnect.IsEnabled = false;
            _ddd.StartConnect(host, shareName, Int32.Parse(port), new DDDAdapter.ConnectCompleteDelegate(ConnectComplete));
            _ddd.SetCPECallback(new DDDAdapter.UpdateCPEDelegate(UpdateCPE));
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
                if (!i.ID.Contains("PT-"))
                    _items.Add(i.ID, i);
                else
                    _ptItems.Add(i.ID, i);
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
        protected void Close()
        { 
        //AD, TODO
        }
        private void TimeTick(int time)
        {
            _time = time;
            if (!_readyToSendItems)
                return;
            try
            {

                //check timeline for a new item to process
                List<T_Item> items = new List<T_Item>();
                List<ScheduledItem> itemsToProcess = null;// new List<ScheduledItem>();
                itemsToProcess = _timelineEvents.GetItemsUpTo(time);
               // itemsToProcess[4].DM_ID = "!!";
                if (itemsToProcess == null)
                    return;
                //Thread.Sleep(2000);
                foreach (ScheduledItem i in itemsToProcess)
                {
                    T_Item myItem = null;
                    //i.DM_ID;
                    if (_items.ContainsKey(i.ID))
                    {
                        myItem = _items[i.ID];
                    }
                    else if (_ptItems.ContainsKey(i.ID))
                    {
                        myItem = _ptItems[i.ID];
                    }
                    else
                        continue;
                    
                        _ddd.SendItemInfo(i.DM_ID, i.ID, myItem.Parameters.FF_Difficulty, myItem.Parameters.TT_Difficulty, time);
                        String infoText = String.Format("Processing item id={0}; For DM={1}; Current Time={2}\r\n", i.ID, i.DM_ID, i.Time);
                        //AD: TODO: Need to add in specific strings that describe the actions taking place in the item
                        items.Add(myItem.DeepCopy());
                        T_Item item = myItem.DeepCopy();
                        StringBuilder sb = new StringBuilder();
                        if (item.Parameters.Crossing)
                        {
                            sb.AppendFormat("Crossing;");
                        }
                        else
                        {
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

                            PirateGenerator pirateGenerator = new PirateGenerator(_ddd);
                            pirateGenerator.Generate(item, i.DM_ID);

                            MerchantGenerator merchantGenerator = new MerchantGenerator(_ddd);
                            merchantGenerator.Generate(item, i.DM_ID);

                            CourseGenerator courseGenerator = new CourseGenerator(_ddd);
                            courseGenerator.Generate(item, i.DM_ID);

                            InterceptGenerator interceptGenerator = new InterceptGenerator(_ddd);
                            interceptGenerator.Generate(item, i.DM_ID);

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
                                _ddd.SendStimulusEvent(i.ID, i.DM_ID, objectID, stimType, time, item.Parameters.FF_Difficulty, item.Parameters.TT_Difficulty);
                            }
                        }
                        AppendToInfoBox(tbInfoBox, infoText);
                        _readyToSendItems = false;//re-set for next time;
                    

                }

            }
            catch (Exception ex)
            {
                ErrorWindow.ShowDialog(ex, this);
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
