using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;
//using Aptima.Asim.DDD.VoIPClient.VoIPClientControlLib;

using Microsoft.DirectX.Direct3D;

namespace Aptima.Asim.DDD.Client.Controller
{
    public class RangeRingInfo
    {
        public string Name;
        public string Type;
        public bool IsWeapon;
        public Dictionary<int, int> rangeIntensities;

        public RangeRingInfo()
        { 
            this.Name = String.Empty;
            this.Type = String.Empty;
            this.IsWeapon = false;
            this.rangeIntensities = new Dictionary<int,int>();
        }
        public RangeRingInfo(RangeRingDisplayValue rrdv)
        {
            this.Name = rrdv.name;
            this.Type = rrdv.type;
            this.IsWeapon = rrdv.isWeapon;
            this.rangeIntensities = new Dictionary<int, int>();// (rrdv.rangeIntensities);
            Console.Write("Ranges: ");
            foreach (int range in rrdv.rangeIntensities.Keys)
            {
                Console.Write("({0}:{1}) ", range, rrdv.rangeIntensities[range]);
                this.rangeIntensities.Add(range, rrdv.rangeIntensities[range]);
            }
            Console.WriteLine();
        }
        public int GetIntensityForRange(int range)
        {
            int intensity = -1;
            int count = 1;
            foreach (KeyValuePair<int, int> kvp in this.rangeIntensities)
            {
                if (kvp.Key >= range && kvp.Value > intensity)
                {
                   // if (count == 1)
                   // {
                        intensity = kvp.Value; //if smallest range is bigger than current distance.
                   // }
                    //return intensity; //returns the previous intensity which satisfied input_range > range.
                }
                //intensity = kvp.Value;
            }

            return intensity; //if the loop couldnt find one case where the range was >= the incoming range, then no ranges will succeed.
        }
    }
    public struct ViewProMotionUpdate
    {
        public string ObjectId;
        public string PlayerId;
        public string Icon;
        public string OwnerID;
        public bool IsWeapon;
        public bool HideObject;
        public int StartX;
        public int StartY;
        public int StartZ;
        public int PlayerColor;
        public int DestinationX;
        public int DestinationY;
        public int DestinationZ;
        public double Throttle;
        public double MaxSpeed;
    }
    public struct ViewProAttributeUpdate
    {
        public string ObjectId;
        public string PlayerId;
        public string ParentId;
        public string OwnerId;
        public string ClassName;
        public string Classification;
        public string State;
        public string IconName;
        public string Tag;
        public int LocationX;
        public int LocationY;
        public int LocationZ;
        public double Throttle;
        public double FuelCapacity;
        public double FuelAmount;
        public double MaxSpeed;
        public string[] CapabilityAndWeapons;
        public string[] SubPlatforms;
        public string[] Vulnerabilities;
        public string[] Sensors;
        public Dictionary<string, DataValue> CustomAttributes;
        //sensor range rings
        public Dictionary<string, RangeRingInfo> SensorRangeRings;
        //capability range rings
        public Dictionary<string, RangeRingInfo> CapabilityRangeRings;
        //vulnerability range rings
        public Dictionary<string, RangeRingInfo> VulnerabilityRangeRings;
    }

    public partial class GUIController : ICommand, IVoiceClientEventCommunicator
    {
        private string _PlayerName;
        private string _HostName; 
        private int _exception_color = System.Drawing.Color.Red.ToArgb();
        private SimulationModelInfo _SimModel;

        private SimulationEvent _MoveEvent;
        private SimulationEvent _AttackEvent;
        private SimulationEvent _SubPEvent;
        private SimulationEvent _ChatRequest;
        private SimulationEvent _WBRequest;
        private SimulationEvent _HandshakeRequest;
        private SimulationEvent _RequestChatRoom;
        private SimulationEvent _requestClassification;
        private SimulationEvent _AuthenticationRequest;
        private SimulationEvent _TransferObjectRequest;
        private SimulationEvent _DockingToOtherRequest;
        private SimulationEvent _ChangeTagRequest;
        private SimulationEvent _VoiceClientEventRequest;
        private SimulationEvent _ScreenPositionUpdate;
        private SimulationEvent _MeasureSelectedObject;
        private SimulationEvent _MeasureWeaponSelected;
        private SimulationEvent _MeasureTabSelected;

        private List<SimulationEvent> DemoEvents;

        private ViewProMotionUpdate _viewpro_motion_update;
        private ViewProAttributeUpdate _viewpro_attr_update;

        private bool has_server;
        public bool InformServerDisconnect
        {
            get
            {
                lock (this)
                {
                    return has_server;
                }
            }
            set
            {
                lock (this)
                {
                    has_server = value;
                }
            }
        }
        private IController _receiver = null;


        public GUIController(string playername, string hostname, SimulationModelInfo model)
        {
            _PlayerName = playername;
            _SimModel = model;
            _HostName = hostname;


            if (!DDD_Global.Instance.IsConnected)
            {
                InformServerDisconnect = false;
                lock (this)
                {
                    DemoEvents = new List<SimulationEvent>();
                }
            }
            else
            {
                InformServerDisconnect = true;
            }
        }


        private string Fmt_ErrorMsg(string header, string error)
        {
            return string.Format("ControllerException({0}): {1}", header, error);
        }

        public void ScenarioControllerLoop(IController receiver)
        {
            _receiver = receiver;
            UTM_Mapping.HorizonalMetersPerPixel = 8.49990671075954650f;
            UTM_Mapping.VerticalMetersPerPixel = 8.56718696599567760f;


            try
            {


                while (true)
                {
                    if (DDD_Global.Instance.IsConnected)
                    {
                        List<SimulationEvent> events = DDD_Global.Instance.GetEvents();



                        foreach (SimulationEvent vpEvent in events)
                        {


                            switch (vpEvent.eventType)
                            {
                                case "SimulationTimeEvent":
                                    try
                                    {
                                        receiver.TimeTick(((StringValue)vpEvent.parameters["SimulationTime"]).value);
                                        receiver.SendMapUpdate(false);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.StackTrace);
                                        throw new ControllerException(Fmt_ErrorMsg("SimulationTimeEvent", e.Message));
                                    }
                                    break;




                                case "ScoreUpdate":
                                    try
                                    {
                                        if (((StringValue)vpEvent.parameters["DecisionMakerID"]).value == DDD_Global.Instance.PlayerID)
                                        {
                                            string score_name = ((StringValue)vpEvent.parameters["ScoreName"]).value;
                                            double score_value = ((DoubleValue)vpEvent.parameters["ScoreValue"]).value;
                                            receiver.ScoreUpdate(score_name, score_value);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("ScoreUpdate", e.Message));
                                    }
                                    break;




                                case "GameSpeed":
                                    try
                                    {
                                        DDD_Global.Instance.GameSpeed = ((DoubleValue)vpEvent.parameters["SpeedFactor"]).value;
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("GameSpeed", e.Message));
                                    }
                                    break;




                                case "CreateChatRoom":
                                    try
                                    {
                                        string room_name = ((StringValue)vpEvent.parameters["RoomName"]).value;
                                        List<string> membership_list = ((StringListValue)vpEvent.parameters["MembershipList"]).strings;
                                        if (membership_list != null)
                                        {
                                            if (membership_list.Contains(DDD_Global.Instance.PlayerID))
                                            {
                                                if (room_name != null)
                                                {
                                                    if (room_name.Length > 0)
                                                    {
                                                        receiver.CreateChatRoom("chat - " + room_name, room_name, membership_list);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("CreateChatRoom", e.Message));
                                    }
                                    break;




                                case "FailedToCreateChatRoom":
                                    try
                                    {
                                        string message = ((StringValue)vpEvent.parameters["Message"]).value;
                                        string dm_id = ((StringValue)vpEvent.parameters["SenderDM_ID"]).value;
                                        if (message != null)
                                        {
                                            if (dm_id == DDD_Global.Instance.PlayerID)
                                            {
                                                receiver.FailedToCreateChatRoom(message, dm_id);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("FailedToCreateChatRoom", e.Message));
                                    }
                                    break;




                                case "CloseChatRoom":
                                    try
                                    {
                                        string room_name = ((StringValue)vpEvent.parameters["RoomName"]).value;
                                        if (room_name != null)
                                        {
                                            receiver.CloseChatRoom(room_name);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("CloseChatRoom", e.Message));
                                    }
                                    break;

                                case "CreateWhiteboardRoom":
                                    try
                                    {
                                        string room_name = ((StringValue)vpEvent.parameters["RoomName"]).value;
                                        string senderDM = ((StringValue)vpEvent.parameters["SenderDM_ID"]).value;
                                        List<string> membership_list = ((StringListValue)vpEvent.parameters["MembershipList"]).strings;
                                        if (membership_list != null)
                                        {
                                            if (membership_list.Contains(DDD_Global.Instance.PlayerID))
                                            {
                                                if (room_name != null)
                                                {
                                                    if (room_name.Length > 0)
                                                    {
                                                        receiver.CreateWhiteboardRoom("whiteboard - " + room_name, room_name, membership_list, senderDM);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("CreateWhiteboardRoom", e.Message));
                                    }
                                    break;

                                case "ViewProMotionUpdate":
                                    try
                                    {
                                        if (((StringValue)vpEvent.parameters["ObjectID"]).value != null)
                                        {
                                            _viewpro_motion_update.ObjectId = ((StringValue)vpEvent.parameters["ObjectID"]).value;
                                            _viewpro_motion_update.HideObject = !((LocationValue)vpEvent.parameters["Location"]).exists;
                                            _viewpro_motion_update.Icon = ((StringValue)vpEvent.parameters["IconName"]).value;
                                            _viewpro_motion_update.IsWeapon = ((BooleanValue)vpEvent.parameters["IsWeapon"]).value;

                                            _viewpro_motion_update.Throttle = ((DoubleValue)vpEvent.parameters["Throttle"]).value;
                                            _viewpro_motion_update.MaxSpeed = ((DoubleValue)vpEvent.parameters["MaximumSpeed"]).value;
                                            _viewpro_motion_update.StartX = UTM_Mapping.HorizontalMetersToPixels((float)((LocationValue)vpEvent.parameters["Location"]).X);
                                            _viewpro_motion_update.StartY = UTM_Mapping.VerticalMetersToPixels((float)((LocationValue)vpEvent.parameters["Location"]).Y);
                                            _viewpro_motion_update.StartZ = (int)((LocationValue)vpEvent.parameters["Location"]).Z;
                                            if ((((LocationValue)vpEvent.parameters["DestinationLocation"]).X == 0) && (((LocationValue)vpEvent.parameters["DestinationLocation"]).Y == 0) &&
                                                (((LocationValue)vpEvent.parameters["DestinationLocation"]).Z == 0))
                                            {
                                                _viewpro_motion_update.StartX = _viewpro_motion_update.DestinationX;
                                                _viewpro_motion_update.StartY = _viewpro_motion_update.DestinationY;
                                                _viewpro_motion_update.StartZ = _viewpro_motion_update.DestinationZ;
                                            }
                                            else
                                            {
                                                _viewpro_motion_update.DestinationX = UTM_Mapping.HorizontalMetersToPixels((float)((LocationValue)vpEvent.parameters["DestinationLocation"]).X);
                                                _viewpro_motion_update.DestinationY = UTM_Mapping.VerticalMetersToPixels((float)((LocationValue)vpEvent.parameters["DestinationLocation"]).Y);
                                                _viewpro_motion_update.DestinationZ = (int)((LocationValue)vpEvent.parameters["DestinationLocation"]).Z;
                                            }
                                            receiver.ViewProMotionUpdate(_viewpro_motion_update);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("ViewProMotionUpdate", e.Message));
                                    }
                                    break;




                                case "ViewProInitializeObject":
                                    try
                                    {
                                        if (((StringValue)vpEvent.parameters["ObjectID"]).value != null)
                                        {
                                            _viewpro_motion_update.ObjectId = ((StringValue)vpEvent.parameters["ObjectID"]).value;
                                            _viewpro_motion_update.PlayerId = ((StringValue)vpEvent.parameters["TargetPlayer"]).value;
                                            _viewpro_motion_update.HideObject = !((LocationValue)vpEvent.parameters["Location"]).exists;
                                            _viewpro_motion_update.Icon = ((StringValue)vpEvent.parameters["IconName"]).value;
                                            _viewpro_motion_update.IsWeapon = ((BooleanValue)vpEvent.parameters["IsWeapon"]).value;
                                            _viewpro_motion_update.OwnerID = ((StringValue)vpEvent.parameters["OwnerID"]).value;
                                            _viewpro_motion_update.PlayerColor = ((IntegerValue)vpEvent.parameters["LabelColor"]).value;

                                            if (_viewpro_motion_update.PlayerId == DDD_Global.Instance.PlayerID)
                                            {
                                                _viewpro_motion_update.StartX = UTM_Mapping.HorizontalMetersToPixels((float)((LocationValue)vpEvent.parameters["Location"]).X);
                                                _viewpro_motion_update.StartY = UTM_Mapping.VerticalMetersToPixels((float)((LocationValue)vpEvent.parameters["Location"]).Y);
                                                _viewpro_motion_update.StartZ = (int)((LocationValue)vpEvent.parameters["Location"]).Z;

                                                receiver.ViewProInitializeUpdate(_viewpro_motion_update);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("ViewProInitializeObject", e.Message + e.StackTrace));
                                    }
                                    break;


                                case "ViewProActiveRegionUpdate":
                                    try
                                    {
                                        string ObjectId = ((StringValue)vpEvent.parameters["ObjectID"]).value;
                                        if (ObjectId != null)
                                        {
                                            int color = ((IntegerValue)vpEvent.parameters["DisplayColor"]).value;
                                            bool visible = ((BooleanValue)vpEvent.parameters["IsVisible"]).value;
                                            List<PolygonValue.PolygonPoint> points = ((PolygonValue)vpEvent.parameters["Shape"]).points;
                                            List<CustomVertex.TransformedColored> pointlist = new List<CustomVertex.TransformedColored>();
                                            foreach (PolygonValue.PolygonPoint point in points)
                                            {
                                                pointlist.Add(new CustomVertex.TransformedColored(UTM_Mapping.HorizontalMetersToPixels((float)point.X), UTM_Mapping.VerticalMetersToPixels((float)point.Y), 1f, 1, color));
                                            }
                                            receiver.ActiveRegionUpdate(ObjectId, visible, color, pointlist);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("ViewProActiveRegionUpdate", e.Message + e.StackTrace));
                                    }
                                    break;


                                case "ViewProAttributeUpdate":
                                    if (receiver is DDD_MainWinFormInterface)
                                    {
                                        while (((DDD_MainWinFormInterface)receiver).IsCanvasReady() == false)
                                        {
                                            Console.WriteLine("Canvas not ready...");
                                            System.Threading.Thread.Sleep(300);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("receiver isn't win form interface.");
                                    }
                                    try
                                    {
                                        if (((StringValue)vpEvent.parameters["ObjectID"]).value != null)
                                        {
                                            _viewpro_attr_update.ObjectId = ((StringValue)vpEvent.parameters["ObjectID"]).value;
                                            _viewpro_attr_update.PlayerId = ((StringValue)vpEvent.parameters["TargetPlayer"]).value;
                                            _viewpro_attr_update.CapabilityAndWeapons = null;

                                            if (vpEvent.parameters.ContainsKey("Attributes"))
                                            {
                                                if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("State"))
                                                {
                                                    _viewpro_attr_update.State = ((StringValue)((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes["State"]).value;
                                                }
                                                else
                                                {
                                                    _viewpro_attr_update.State = string.Empty;
                                                }

                                                if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("Throttle"))
                                                {
                                                    _viewpro_attr_update.Throttle = ((DoubleValue)((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes["Throttle"]).value;
                                                }
                                                else
                                                {
                                                    _viewpro_attr_update.Throttle = -1;
                                                }

                                                if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("IconName"))
                                                {
                                                    _viewpro_attr_update.IconName = ((StringValue)((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes["IconName"]).value;
                                                }
                                                else
                                                {
                                                    _viewpro_attr_update.IconName = string.Empty;
                                                }

                                                if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("ParentObjectID"))
                                                {
                                                    _viewpro_attr_update.ParentId = ((StringValue)((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes["ParentObjectID"]).value;
                                                }
                                                else
                                                {
                                                    _viewpro_attr_update.ParentId = string.Empty;
                                                }

                                                if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("ClassName"))
                                                {
                                                    _viewpro_attr_update.ClassName = ((StringValue)((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes["ClassName"]).value;
                                                }
                                                else
                                                {
                                                    _viewpro_attr_update.ClassName = string.Empty;
                                                }
                                                if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("CurrentClassification"))
                                                {
                                                    _viewpro_attr_update.Classification = ((StringValue)((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes["CurrentClassification"]).value;
                                                }
                                                else
                                                {
                                                    _viewpro_attr_update.Classification = string.Empty;
                                                }
                                            }

                                            if (_viewpro_attr_update.PlayerId == DDD_Global.Instance.PlayerID)
                                            {
                                                _viewpro_attr_update.OwnerId = ((StringValue)vpEvent.parameters["OwnerID"]).value;

                                                if (vpEvent.parameters.ContainsKey("Attributes"))
                                                {
                                                    List<string> cap_list = new List<string>();
                                                    if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("CapabilitiesList"))
                                                    {
                                                        cap_list = ((StringListValue)((AttributeCollectionValue)vpEvent["Attributes"])["CapabilitiesList"]).strings;
                                                        _viewpro_attr_update.CapabilityAndWeapons = cap_list.ToArray();
                                                    }
                                                    else
                                                    {
                                                        _viewpro_attr_update.CapabilityAndWeapons = null;
                                                    }
                                                    List<string> dock_list = new List<string>();
                                                    if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("DockedObjects"))
                                                    {
                                                        dock_list = ((StringListValue)((AttributeCollectionValue)vpEvent["Attributes"])["DockedObjects"]).strings;
                                                        _viewpro_attr_update.SubPlatforms = dock_list.ToArray();
                                                    }
                                                    else
                                                    {
                                                        _viewpro_attr_update.SubPlatforms = null;
                                                    }
                                                    List<string> vulnerabilities = new List<string>();
                                                    if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("VulnerabilityList"))
                                                    {
                                                        vulnerabilities = ((StringListValue)((AttributeCollectionValue)vpEvent["Attributes"])["VulnerabilityList"]).strings;
                                                        _viewpro_attr_update.Vulnerabilities = vulnerabilities.ToArray();
                                                    }
                                                    else
                                                    {
                                                        _viewpro_attr_update.Vulnerabilities = null;
                                                    }
                                                    List<string> sensors = new List<string>();
                                                    if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("SensorList"))
                                                    {
                                                        sensors = ((StringListValue)((AttributeCollectionValue)vpEvent["Attributes"])["SensorList"]).strings;
                                                        _viewpro_attr_update.Sensors = sensors.ToArray();
                                                    }
                                                    else
                                                    {
                                                        _viewpro_attr_update.Sensors = null;
                                                    }

                                                }

                                                if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("MaximumSpeed"))
                                                {
                                                    _viewpro_attr_update.MaxSpeed = ((DoubleValue)((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes["MaximumSpeed"]).value;
                                                }

                                                if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("FuelCapacity"))
                                                {
                                                    _viewpro_attr_update.FuelCapacity = ((DoubleValue)((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes["FuelCapacity"]).value;
                                                }
                                                else
                                                {
                                                    _viewpro_attr_update.FuelCapacity = -1;
                                                }
                                                if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("FuelAmount"))
                                                {
                                                    _viewpro_attr_update.FuelAmount = ((DoubleValue)((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes["FuelAmount"]).value;
                                                }
                                                else
                                                {
                                                    _viewpro_attr_update.FuelAmount = -1;
                                                }

                                                if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("CustomAttributes"))
                                                {
                                                    _viewpro_attr_update.CustomAttributes = ((CustomAttributesValue)((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes["CustomAttributes"]).attributes;
                                                }
                                                else
                                                {
                                                    _viewpro_attr_update.CustomAttributes = null;
                                                }

                                                if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("Location"))
                                                {
                                                    if (((LocationValue)((AttributeCollectionValue)vpEvent.parameters["Attributes"])["Location"]).exists)
                                                    {
                                                        _viewpro_attr_update.LocationX = (int)((LocationValue)((AttributeCollectionValue)vpEvent.parameters["Attributes"])["Location"]).X;
                                                        _viewpro_attr_update.LocationY = (int)((LocationValue)((AttributeCollectionValue)vpEvent.parameters["Attributes"])["Location"]).Y;
                                                        _viewpro_attr_update.LocationZ = (int)((LocationValue)((AttributeCollectionValue)vpEvent.parameters["Attributes"])["Location"]).Z;
                                                    }
                                                    else
                                                    {
                                                        _viewpro_attr_update.LocationX = 0;
                                                        _viewpro_attr_update.LocationY = 0;
                                                        _viewpro_attr_update.LocationZ = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    _viewpro_attr_update.LocationX = 0;
                                                    _viewpro_attr_update.LocationY = 0;
                                                    _viewpro_attr_update.LocationZ = 0;
                                                }

                                                if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("InitialTag"))
                                                {
                                                    _viewpro_attr_update.Tag = ((StringValue)((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes["InitialTag"]).value;
                                                }
                                                else
                                                {
                                                    _viewpro_attr_update.Tag = null;
                                                }

                                                if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("SensorRangeRings"))
                                                {
                                                    Console.WriteLine("Sensor Range Ring received in VPAU");
                                                    //Console.WriteLine("ToString: {0}", ((CustomAttributesValue)((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes["SensorRangeRings"]).ToString());
                                                    CustomAttributesValue acv = ((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes["SensorRangeRings"] as CustomAttributesValue;
                                                    Console.WriteLine("Sensor Range Ring extracted in VPAU");
                                                    _viewpro_attr_update.SensorRangeRings = new Dictionary<string,RangeRingInfo>();
                                                    foreach (string sensorName in acv.attributes.Keys)
                                                    { 
                                                        //do stuff!
                                                        Console.WriteLine("Sensor '{0}' received in VPAU", sensorName);
                                                       _viewpro_attr_update.SensorRangeRings.Add(sensorName, new RangeRingInfo(acv[sensorName] as RangeRingDisplayValue));
                                                    }
                                                    
                                                }
                                                else
                                                {
                                                    _viewpro_attr_update.SensorRangeRings = null;
                                                }

                                                if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("CapabilityRangeRings"))
                                                {
                                                    Console.WriteLine("Capability Range Rings received in VPAU");
                                                    CustomAttributesValue acv = ((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes["CapabilityRangeRings"] as CustomAttributesValue;
                                                    _viewpro_attr_update.CapabilityRangeRings = new Dictionary<string, RangeRingInfo>();
                                                    Console.WriteLine("{0} ", ((StringValue)vpEvent.parameters["ObjectID"]).value);
                                                    foreach (string capabilityName in acv.attributes.Keys)
                                                    {
                                                        //do stuff!
                                                        _viewpro_attr_update.CapabilityRangeRings.Add(capabilityName, new RangeRingInfo(acv[capabilityName] as RangeRingDisplayValue));
                                                    }
                                                }
                                                else
                                                {
                                                    _viewpro_attr_update.CapabilityRangeRings = null;
                                                }

                                                if (((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes.ContainsKey("VulnerabilityRangeRings"))
                                                {
                                                    Console.WriteLine("Vulnerability Range Rings received in VPAU");
                                                    CustomAttributesValue acv = ((AttributeCollectionValue)vpEvent.parameters["Attributes"]).attributes["VulnerabilityRangeRings"] as CustomAttributesValue;
                                                    _viewpro_attr_update.VulnerabilityRangeRings = new Dictionary<string, RangeRingInfo>();
                                                    Console.WriteLine("{0} ", ((StringValue)vpEvent.parameters["ObjectID"]).value);
                                                    foreach (string vulnerabilityName in acv.attributes.Keys)
                                                    {
                                                        //do stuff!
                                                        _viewpro_attr_update.VulnerabilityRangeRings.Add(vulnerabilityName, new RangeRingInfo(acv[vulnerabilityName] as RangeRingDisplayValue));
                                                    }
                                                }
                                                else
                                                {
                                                    _viewpro_attr_update.VulnerabilityRangeRings = null;
                                                }

                                                receiver.ViewProAttributeUpdate(_viewpro_attr_update);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("ViewProAttributeUpdate", e.Message));
                                    }
                                    break;




                                case "ViewProStopObjectUpdate":
                                    try
                                    {
                                        receiver.ViewProStopObjectUpdate(((StringValue)vpEvent["ObjectID"]).value);
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("ViewProStopObjectUpdate", e.Message));
                                    }
                                    break;




                                case "ClientRemoveObject":
                                    try
                                    {
                                        if (((StringValue)vpEvent.parameters["TargetPlayer"]).value == DDD_Global.Instance.PlayerID)
                                        {
                                            receiver.RemoveObject(((StringValue)vpEvent.parameters["ObjectID"]).value);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("ClientRemoveObject", e.Message));
                                    }

                                    break;




                                case "TextChat":
                                    try
                                    {
                                        string user_id = ((StringValue)vpEvent.parameters["UserID"]).value;
                                        string target_id = ((StringValue)vpEvent.parameters["TargetUserID"]).value;
                                        string chat_body = ((StringValue)(vpEvent.parameters["ChatBody"])).value;
                                        if ((user_id != null) && (target_id != null))
                                        {
                                            receiver.TextChatRequest(user_id, chat_body, target_id);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("TextChat", e.Message));
                                    }
                                    break;

                                case "WhiteboardLine":
                                    try
                                    {
                                        string object_id = ((StringValue)vpEvent.parameters["ObjectID"]).value;
                                        string user_id = ((StringValue)vpEvent.parameters["UserID"]).value;
                                        string target_id = ((StringValue)vpEvent.parameters["TargetUserID"]).value;
                                        LocationValue start = (LocationValue)vpEvent.parameters["StartLocation"];
                                        LocationValue end = (LocationValue)vpEvent.parameters["EndLocation"];
                                        int mode = ((IntegerValue)(vpEvent.parameters["Mode"])).value;
                                        double width = ((DoubleValue)(vpEvent.parameters["Width"])).value;
                                        double originalScale = ((DoubleValue)(vpEvent.parameters["OriginalScale"])).value;
                                        int color = ((IntegerValue)(vpEvent.parameters["Color"])).value;
                                        string text = ((StringValue)vpEvent.parameters["Text"]).value;
                                        if ((user_id != null) && (target_id != null))
                                        {
                                            receiver.WhiteboardLine(object_id, user_id, mode, start, end, width, originalScale, color, text, target_id);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("Whiteboard", e.Message));
                                    }
                                    break;

                                case "WhiteboardClear":
                                    try
                                    {
                                        string user_id = ((StringValue)vpEvent.parameters["UserID"]).value;
                                        string target_id = ((StringValue)vpEvent.parameters["TargetUserID"]).value;
                                        if ((user_id != null) && (target_id != null))
                                        {
                                            receiver.WhiteboardClear(user_id, target_id);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("Whiteboard", e.Message));
                                    }
                                    break;

                                case "WhiteboardClearAll":
                                    try
                                    {
                                        string user_id = ((StringValue)vpEvent.parameters["UserID"]).value;
                                        string target_id = ((StringValue)vpEvent.parameters["TargetUserID"]).value;
                                        if ((user_id != null) && (target_id != null))
                                        {
                                            receiver.WhiteboardClearAll(user_id, target_id);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("Whiteboard", e.Message));
                                    }
                                    break;

                                case "WhiteboardUndo":
                                    try
                                    {
                                        string object_id = ((StringValue)vpEvent.parameters["ObjectID"]).value;
                                        string user_id = ((StringValue)vpEvent.parameters["UserID"]).value;
                                        string target_id = ((StringValue)vpEvent.parameters["TargetUserID"]).value;
                                        if ((user_id != null) && (target_id != null))
                                        {
                                            receiver.WhiteboardUndo(object_id, user_id, target_id);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("Whiteboard", e.Message));
                                    }
                                    break;

                                case "ClientMeasure_ScreenView":
                                    try
                                    {
                                        string user_id = ((StringValue)vpEvent.parameters["UserID"]).value;
                                        int originX = ((IntegerValue)vpEvent.parameters["Origin-X"]).value;
                                        int originY = ((IntegerValue)vpEvent.parameters["Origin-Y"]).value;
                                        int screenSizeWidth = ((IntegerValue)vpEvent.parameters["ScreenSizeWidth"]).value;
                                        int screenSizeHeight = ((IntegerValue)vpEvent.parameters["ScreenSizeHeight"]).value;
                                        double screenZoom = ((DoubleValue)vpEvent.parameters["ScreenZoom"]).value;
                                        if (user_id != null)
                                        {
                                            receiver.WhiteboardScreenView(user_id, originX, originY, screenSizeWidth, screenSizeHeight, screenZoom);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("Whiteboard", e.Message));
                                    }
                                    break;


                                case "WhiteboardSyncScreenView":
                                    try
                                    {
                                        string user_id = ((StringValue)vpEvent.parameters["UserID"]).value;
                                        string whiteboard_id = ((StringValue)vpEvent.parameters["WhiteboardID"]).value;
                                        string target_id = ((StringValue)vpEvent.parameters["TargetUserID"]).value;
                                        if ((user_id != null) && (target_id != null) & (whiteboard_id != null))
                                        {
                                            receiver.WhiteboardSyncScreenView(user_id, target_id, whiteboard_id);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("Whiteboard", e.Message));
                                    }
                                    break;

                                case "HandshakeAvailablePlayers":
                                    try
                                    {
                                        if (((StringValue)vpEvent.parameters["TargetTerminalID"]).value == DDD_Global.Instance.TerminalID && DDD_Global.Instance.HasAuthenticated)
                                        {

                                            StringListValue s = vpEvent.parameters["AvailablePlayers"] as StringListValue;
                                            DDD_Global.Instance.DM_List = ((StringListValue)vpEvent.parameters["Players"]).strings;
                                            if (s != null)
                                            {
                                                if (s.strings.Count > 0)
                                                {
                                                    receiver.HandshakeAvailablePlayers(s.strings.ToArray());
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("HandshakeAvailablePlayers", e.Message));
                                    }
                                    break;




                                case "HandshakeInitializeGUI":
                                    try
                                    {
                                        if (((StringValue)vpEvent.parameters["TerminalID"]).value == DDD_Global.Instance.TerminalID)
                                        {
                                            UTM_Mapping.HorizonalMetersPerPixel = (float)(((DoubleValue)vpEvent.parameters["HorizontalPixelsPerMeter"]).value);
                                            UTM_Mapping.VerticalMetersPerPixel = (float)(((DoubleValue)vpEvent.parameters["VerticalPixelsPerMeter"]).value);
                                            DDD_Global.Instance.PlayerID = ((StringValue)vpEvent.parameters["PlayerID"]).value;
                                            DDD_Global.Instance.ImageLibrary = ((StringValue)vpEvent.parameters["IconLibrary"]).value;
                                            DDD_Global.Instance.MapName = ((StringValue)vpEvent.parameters["MapName"]).value;
                                            DDD_Global.Instance.MapLocation = string.Format("{0}\\MapLib\\{1}", DDD_Global.Instance.DDDClientShareFolder, DDD_Global.Instance.MapName);
                                            DDD_Global.Instance.ScenarioName = ((StringValue)vpEvent.parameters["ScenarioName"]).value;
                                            DDD_Global.Instance.ScenarioDescription = ((StringValue)vpEvent.parameters["ScenarioDescription"]).value;
                                            DDD_Global.Instance.PlayerBrief = ((StringValue)vpEvent.parameters["PlayerBrief"]).value;
                                            //DDD_Global.Instance.VoiceServerHostname = ((StringValue)vpEvent.parameters["VoiceChatServerName"]).value;
                                            DDD_Global.Instance.VoiceServerHostname = _HostName;
                                            DDD_Global.Instance.VoiceServerPort = ((IntegerValue)vpEvent.parameters["VoiceChatServerPort"]).value;
                                            DDD_Global.Instance.ConaitoServerPassword = ((StringValue)vpEvent.parameters["VoiceChatUserPassword"]).value;
                                            DDD_Global.Instance.VoiceChatEnabled = ( ( BooleanValue )vpEvent.parameters["VoiceChatEnabled"] ).value;
                                            DDD_Global.Instance.IsObserver = ((BooleanValue)vpEvent.parameters["IsObserver"]).value;
                                            DDD_Global.Instance.IsForkReplay = ((BooleanValue)vpEvent.parameters["IsForkReplay"]).value;
                                            if (vpEvent.parameters.ContainsKey("DefaultDisplayLabels"))
                                            {
                                                String val = ((StringValue)vpEvent.parameters["DefaultDisplayLabels"]).value;
                                                ((DDD_MainWinFormInterface)receiver).toggleDisplayLabels(val);

                                            }
                                            if (vpEvent.parameters.ContainsKey("DefaultDisplayTags"))
                                            {
                                                String val = ((StringValue)vpEvent.parameters["DefaultDisplayTags"]).value;
                                                ((DDD_MainWinFormInterface)receiver).toggleTagDisplay(val);
                                            }

                                            System.Console.WriteLine( "DDD_Global.Instance.VoiceChatEnabled = {0}", DDD_Global.Instance.VoiceChatEnabled );

                                            receiver.HandshakeInitializeGUI();
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("HandshakeInitializeGUI", e.Message));
                                    }
                                    break;

                                case "HandshakeInitializeGUIDone":
                                    //This is when another user joins, we need to send them our current screen coords
                                    if (((StringValue)vpEvent.parameters["PlayerID"]).value != DDD_Global.Instance.PlayerID)
                                    {
                                        try
                                        {
                                            receiver.SendMapUpdate(true);
                                        }
                                        catch (Exception e)
                                        {
                                            throw new ControllerException(Fmt_ErrorMsg("HandshakeInitializeGUIDone", e.Message));
                                        }
                                    }
                                    break;
                                case "PauseScenario":
                                    receiver.PauseScenario();
                                    break;

                                case "ResumeScenario":
                                    receiver.ResumeScenario();
                                    break;
                                case "StopScenario":
                                    receiver.StopScenario();
                                    break;
                                case "StopReplay":
                                    receiver.StopReplay();
                                    break;



                                case "ViewProAttackUpdate":
                                    try
                                    {
                                        receiver.AttackUpdate(((StringValue)vpEvent.parameters["AttackerID"]).value,
                                                                                        ((StringValue)vpEvent.parameters["TargetID"]).value,
                                                                                        ((IntegerValue)vpEvent.parameters["Time"]).value,
                                                                                        ((IntegerValue)vpEvent.parameters["AttackEndTime"]).value);
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("ViewProAttackUpdate", e.Message));
                                    }
                                    break;




                                case "AuthenticationResponse":
                                    try
                                    {
                                        bool authentication_result = ((BooleanValue)vpEvent.parameters["Success"]).value;
                                        string message = ((StringValue)vpEvent.parameters["Message"]).value;
                                        string terminal_id = ((StringValue)vpEvent.parameters["TerminalID"]).value;
                                        if (terminal_id == DDD_Global.Instance.TerminalID)
                                        {
                                            if (message == null)
                                            {
                                                message = string.Empty;
                                            }
                                            if (authentication_result)
                                            {
                                                DDD_Global.Instance.HasAuthenticated = true;
                                            }
                                            receiver.AuthenticationResponse(authentication_result, message);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new Exception(Fmt_ErrorMsg("AuthenticationResponse", e.Message));
                                    }
                                    break;





                                case "SystemMessage":
                                    try
                                    {
                                        if (((StringValue)vpEvent.parameters["PlayerID"]).value != null)
                                        {
                                            if (((StringValue)vpEvent.parameters["PlayerID"]).value == DDD_Global.Instance.PlayerID)
                                            {
                                                receiver.SystemMessageUpdate(((StringValue)vpEvent["Message"]).value, ((IntegerValue)vpEvent["TextColor"]).value);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("SystemMessage", e.Message));
                                    }
                                    break;

                                case "TransferObject":
                                    try
                                    {
                                        if (((StringValue)vpEvent.parameters["UserID"]).value != null 
                                            && ((StringValue)vpEvent.parameters["DonorUserID"]).value != null)
                                        {
                                            if (((StringValue)vpEvent.parameters["DonorUserID"]).value == DDD_Global.Instance.PlayerID)
                                            {
                                                receiver.TransferObjectToOther(((StringValue)vpEvent.parameters["ObjectID"]).value);
                                            }
                                            else if (((StringValue)vpEvent.parameters["UserID"]).value == DDD_Global.Instance.PlayerID)
                                            {
                                                receiver.TransferObjectToMe(((StringValue)vpEvent.parameters["ObjectID"]).value);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("TransferObject", e.Message));
                                    }
                                    break;
                                case "ClientSideAssetTransferAllowed":
                                    try
                                    {
                                        DDD_Global.Instance.AssetTransferEnabled = ((BooleanValue)vpEvent["EnableAssetTransfer"]).value;
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("ClientSideAssetTransferAllowed", e.Message));
                                    }
                                    break;

                                case "CreateVoiceChannel":
                                    try
                                    {
                                        if ( receiver is IVoiceClientController )
                                        {
                                            string strChannelName = ((StringValue)vpEvent["ChannelName"]).value;
                                            List<string> astrMembershipList = ( ( StringListValue )vpEvent.parameters["MembershipList"] ).strings;
                                            if ( astrMembershipList != null )
                                            {
                                                if ( astrMembershipList.Contains( DDD_Global.Instance.PlayerID ) )
                                                {
                                                    if ( strChannelName.Length > 0 )
                                                    {
                                                        DDD_Global.Instance.VoiceChannels[strChannelName] = astrMembershipList;
                                                        ( ( IVoiceClientController )receiver ).NotifyCreatedVoiceChannel( strChannelName, astrMembershipList );
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch ( Exception e )
                                    {
                                        throw new ControllerException( Fmt_ErrorMsg( "CreateVoiceChannel", e.Message ) );
                                    }
                                    break;
                                case "CloseVoiceChannel":
                                    try
                                    {
                                        if ( receiver is IVoiceClientController )
                                        {
                                            string strChannelName = ( ( StringValue )vpEvent["ChannelName"] ).value;
                                            if ( strChannelName.Length > 0 )
                                            {
                                                if (DDD_Global.Instance.VoiceChannels.ContainsKey(strChannelName))
                                                {
                                                    DDD_Global.Instance.VoiceChannels.Remove(strChannelName);
                                                }
                                                ( ( IVoiceClientController )receiver ).NotifyClosedVoiceChannel( strChannelName );
                                            }
                                        }
                                    }
                                    catch ( Exception e )
                                    {
                                        throw new ControllerException( Fmt_ErrorMsg( "CloseVoiceChannel", e.Message ) );
                                    }
                                    break;

                                case "JoinVoiceChannel":
                                    try
                                    {
                                        if ( receiver is IVoiceClientController )
                                        {
                                            // Check to see if it's my event, otherwise ignore it
                                            if ( 0 == DDD_Global.Instance.PlayerID.CompareTo( ( ( StringValue )vpEvent["DecisionMakerID"] ).value ) )
                                            {
                                                string strChannelName = ( ( StringValue )vpEvent["ChannelName"] ).value;
                                                if ( strChannelName.Length > 0 )
                                                {
                                                    ( ( IVoiceClientController )receiver ).NotifyJoinVoiceChannel( strChannelName );
                                                }
                                            }
                                        }
                                    }
                                    catch ( Exception e )
                                    {
                                        throw new ControllerException( Fmt_ErrorMsg( "JoinVoiceChannel", e.Message ) );
                                    }
                                    break;

                                case "LeaveVoiceChannel":
                                    try
                                    {
                                        if ( receiver is IVoiceClientController )
                                        {
                                            // Check to see if it's my event, otherwise ignore it
                                            if ( 0 == DDD_Global.Instance.PlayerID.CompareTo( ( ( StringValue )vpEvent["DecisionMakerID"] ).value ) )
                                            {
                                                string strChannelName = ( ( StringValue )vpEvent["ChannelName"] ).value;
                                                if ( strChannelName.Length > 0 )
                                                {
                                                    ( ( IVoiceClientController )receiver ).NotifyLeaveVoiceChannel( strChannelName );
                                                }
                                            }
                                        }
                                    }
                                    catch ( Exception e )
                                    {
                                        throw new ControllerException( Fmt_ErrorMsg( "LeaveVoiceChannel", e.Message ) );
                                    }
                                    break;

                                case "StartedTalking":
                                    try
                                    {
                                        if (receiver is IVoiceClientController)
                                        {
                                            string strUsername = ((StringValue)vpEvent["Speaker"]).value;
                                            string strChannelName = ((StringValue)vpEvent["ChannelName"]).value;
                                            if (strChannelName.Length > 0)
                                            {
                                                ((IVoiceClientController)receiver).NotifyStartedTalking(strUsername, strChannelName);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("StartedTalking", e.Message));
                                    }
                                    break;
                                case "PlayVoiceMessage":
                                    break;
                                case "StoppedTalking":
                                    try
                                    {
                                        if (receiver is IVoiceClientController)
                                        {
                                            string strUsername = ((StringValue)vpEvent["Speaker"]).value;
                                            string strChannelName = ((StringValue)vpEvent["ChannelName"]).value;
                                            if (strChannelName.Length > 0)
                                            {
                                                ((IVoiceClientController)receiver).NotifyStoppedTalking(strUsername, strChannelName);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("StoppedTalking", e.Message));
                                    }
                                    break;
                                case "MuteUser":
                                    try
                                    {
                                        if (receiver is IVoiceClientController)
                                        {
                                            string strUsername = ((StringValue)vpEvent["Speaker"]).value;
                                            string strChannelName = ((StringValue)vpEvent["ChannelName"]).value;
                                            if (strChannelName.Length > 0)
                                            {
                                                ((IVoiceClientController)receiver).NotifyMuteUser(strUsername, strChannelName);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("MuteUser", e.Message));
                                    }
                                    break;
                                case "UnmuteUser":
                                    try
                                    {
                                        if (receiver is IVoiceClientController)
                                        {
                                            string strUsername = ((StringValue)vpEvent["Speaker"]).value;
                                            string strChannelName = ((StringValue)vpEvent["ChannelName"]).value;
                                            if (strChannelName.Length > 0)
                                            {
                                                ((IVoiceClientController)receiver).NotifyUnmuteUser(strUsername, strChannelName);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("UnmuteUser", e.Message));
                                    }
                                    break;
                                case "ForkReplayStarted":
                                    DDD_Global.Instance.IsForkReplay = true;
                                    break;
                                case "ForkReplayFinished":
                                    DDD_Global.Instance.IsForkReplay = false;
                                    break;
                                case "InitializeClassifications":
                                    //Time, Classifications
                                    try
                                    {
                                        List<String> classificationEnumeration = ((StringListValue)vpEvent["Classifications"]).strings;
                                        receiver.SetClassifications(classificationEnumeration);                                        
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ControllerException(Fmt_ErrorMsg("UnmuteUser", e.Message));
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }
                        System.Threading.Thread.Sleep(100); //control this loop so it doesn't chew through processor cycles
                    }
                    else
                    {
                        if (InformServerDisconnect || DemoEvents == null)
                        {
                            // TBD - through server lost exception.
                            throw new ServerDisconnectException("The client and server have been disconnected.");
                        }
                        lock (this)
                        {
                            foreach (SimulationEvent vpEvent in DemoEvents)
                            {
                                switch (vpEvent.eventType)
                                {
                                    case "TimeTick":
                                        break;

                                    case "MoveObjectRequest":
                                        if (((StringValue)vpEvent.parameters["ObjectID"]).value != null)
                                        {
                                            _viewpro_motion_update.ObjectId = ((StringValue)vpEvent.parameters["ObjectID"]).value;
                                            _viewpro_motion_update.DestinationX = UTM_Mapping.HorizontalMetersToPixels((float)((LocationValue)vpEvent.parameters["DestinationLocation"]).X);
                                            _viewpro_motion_update.DestinationY = UTM_Mapping.VerticalMetersToPixels((float)((LocationValue)vpEvent.parameters["DestinationLocation"]).Y);
                                            _viewpro_motion_update.DestinationZ = (int)((LocationValue)vpEvent.parameters["DestinationLocation"]).Z;
                                            _viewpro_motion_update.StartX = _viewpro_motion_update.DestinationX;
                                            _viewpro_motion_update.StartY = _viewpro_motion_update.DestinationY;
                                            _viewpro_motion_update.StartZ = _viewpro_motion_update.DestinationZ;
                                            _viewpro_motion_update.Throttle = ((DoubleValue)vpEvent.parameters["Throttle"]).value;
                                            _viewpro_motion_update.Icon = "F16";

                                            receiver.ViewProMotionUpdate(_viewpro_motion_update);
                                        }
                                        break;


                                    case "ClientAttackRequest":
                                        if (((StringValue)vpEvent.parameters["AttackingObjectID"]).value != null)
                                        {
                                            receiver.AttackUpdate(((StringValue)vpEvent.parameters["AttackingObjectID"]).value,
                                                ((StringValue)(vpEvent.parameters["TargetObjectID"])).value, 10000, 20000);
                                        }

                                        break;

                                    case "TextChatRequest":
                                        receiver.TextChatRequest(((StringValue)vpEvent.parameters["UserID"]).value,
                                            ((StringValue)(vpEvent.parameters["ChatBody"])).value,
                                            ((StringValue)(vpEvent.parameters["TargetUserID"])).value);
                                        break;


                                }
                            }
                            DemoEvents.Clear();
                        }
                    }
                }
            }
            catch (ControllerException controller_exception)
            {
                System.Windows.Forms.MessageBox.Show(controller_exception.Message, "Controller Exception");
                DisconnectDecisionMaker(DDD_Global.Instance.PlayerID);
                return;
            }
            catch (ServerDisconnectException server_disconnect)
            {
                System.Windows.Forms.MessageBox.Show(server_disconnect.Message, "Network Warning");
                DisconnectDecisionMaker(DDD_Global.Instance.PlayerID);
                return;
            }
            catch (System.Threading.ThreadInterruptedException)
            {
                DisconnectDecisionMaker(DDD_Global.Instance.PlayerID);
                return;
            }
            catch (System.Threading.ThreadAbortException)
            {
                DisconnectDecisionMaker(DDD_Global.Instance.PlayerID);
                return;
            }
            catch (Exception exception)
            {
                DisconnectDecisionMaker(DDD_Global.Instance.PlayerID);
                return;
            }

            
        }
        
    }
    
}
