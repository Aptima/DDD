using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

namespace Aptima.Asim.DDD.Client
{
    class DDD_Global
    {
        public string ApplicationDirectory = string.Empty;
        private static DDD_Global _instance = null;
        private string _PlayerID = string.Empty;
        private string _HostName = string.Empty;
        private string _clientPath = "DDDClient";
        private string _TerminalID = string.Empty;
        private string _ImageLib = "ImageLib";
        private string _MapName = string.Empty;
        private string _MapLocation = string.Empty;
        private string _ScenarioName = string.Empty;
        private string _ScenarioDescription = string.Empty;
        private string _PlayerBrief = string.Empty;
        private double _GameSpeed = 1.0;
        private int _Port = 0;
        private bool _AssetTransferEnabled = true;

        private String _strVoiceServerHostname = string.Empty;
        private int _iVoiceServerPort = 0;
        private String _strConaitoServerPasswd = string.Empty;
        private bool _voiceChatEnabled = false;

        private float _rangeFinderX = 0.0F;
        private float _rangeFinderY = 0.0F;
        private float _rangeFinderXDisplay = 0.0F;
        private float _rangeFinderYDisplay = 0.0F;
        private bool _rangeFinderEnabled = true;
        private String _rangeFinderDistance = string.Empty;
        private String _rangeFinderIntensity = string.Empty;

        private Dictionary<String, List<String>> _voiceChannels = new Dictionary<string,List<string>>();

        private NetworkClient _nc = null;
        private SimulationModelInfo _SimModel = null;

        private List<string> _dm_list = new List<string>();
        private string _rangeFinderUnitDisplayFormat = "{0:0}"; //can also be "{0:0.00}";


        private Boolean _isObserver = false;

        private Boolean _isForkReplay = false;
        private Boolean _hasAuthenticated = false;
        #region properties

        public float RangeFinderXDisplay
        {
            get { return _rangeFinderXDisplay; }
            set { _rangeFinderXDisplay = value; }
        }
        public float RangeFinderYDisplay
        {
            get { return _rangeFinderYDisplay; }
            set { _rangeFinderYDisplay = value; }
        }
        public float RangeFinderX
        {
            get { return _rangeFinderX; }
            set { _rangeFinderX = value; }
        }
        public float RangeFinderY
        {
            get { return _rangeFinderY; }
            set { _rangeFinderY = value; }
        }
        public bool RangeFinderEnabled
        {
            get { return _rangeFinderEnabled; }
            set { _rangeFinderEnabled = value; }
        }
        public string RangeFinderDistanceString
        {
            get { return _rangeFinderDistance; }
            set { _rangeFinderDistance = value; }
        }
        public string RangeFinderIntensityString
        {
            get { return _rangeFinderIntensity; }
            set { _rangeFinderIntensity = value; }
        }
        public string RangeFinderDisplayFormat
        {
            get { return _rangeFinderUnitDisplayFormat; }
            set { _rangeFinderUnitDisplayFormat = value; }
        }
        public string GetRangeFinderDisplayString()
        {
            if (!RangeFinderEnabled)
                return String.Empty;
            if (RangeFinderDistanceString.Trim() == string.Empty)
                return string.Empty;
            if (RangeFinderIntensityString == string.Empty)
                return String.Format("R: {0} m.", RangeFinderDistanceString);
            return String.Format("R: {0}m.;  I: {1}",RangeFinderDistanceString, RangeFinderIntensityString);
        }
        /// <summary>
        /// Calculates the 2-D distance between the selected asset and the current RangeFinder values (mouse over defined)
        /// </summary>
        /// <param name="xPos">x position of the currently selected asset</param>
        /// <param name="yPos">y position of the currently selected asset</param>
        public void SetRangeFinderDistance(float xPos, float yPos)
        {
            if (!RangeFinderEnabled)
                return; //turned off
            if (RangeFinderDistanceString == string.Empty)
                return; //mouse off of map OR object unselected, when mouse back on OR object selected set to "  ";
            double distance = Math.Sqrt(Convert.ToDouble((Math.Pow(xPos - RangeFinderX,2) + Math.Pow(yPos - RangeFinderY,2))));
            _rangeFinderDistance = String.Format(_rangeFinderUnitDisplayFormat, distance);
        }

        public DDD_RangeRings RangeRings
        {
            get { return DDD_RangeRings.GetInstance(); }
        }

        public List<string> DM_List
        {
            get
            {
                lock (this)
                {
                    return new List<string>(_dm_list);
                }
            }
            set
            {
                lock (this)
                {
                    _dm_list = value;
                }
            }
        }

        public string ImageLibrary
        {
            get
            {
                lock (this)
                {
                    return _ImageLib;
                }
            }
            set
            {
                lock (this)
                {
                    if (value != null)
                    {
                        if (value.Length > 0)
                        {
                            _ImageLib = value;
                        }
                    }
                }
            }
        }

        public string PlayerID
        {
            get
            {
                lock (this)
                {
                    return _PlayerID;
                }
            }
            set
            {
                lock (this)
                {
                    _PlayerID = value;
                }
            }
        }
        public string HostName
        {
            get
            {
                lock (this)
                {
                    return _HostName;
                }
            }
            set
            {
                lock (this)
                {
                    _HostName = value;
                }
            }
        }
        public string ClientPath
        {
            get
            {
                lock (this)
                {
                    return _clientPath;
                }
            }
            set
            {
                lock (this)
                {
                    _clientPath = value;
                }
            }
        }
        public string DDDClientShareFolder
        {
            get { return String.Format("\\\\{0}\\{1}\\", _HostName, _clientPath); }
        }
        public string TerminalID
        {
            get
            {
                lock (this)
                {
                    return _TerminalID;
                }
            }
            set
            {
                lock (this)
                {
                    _TerminalID = value;
                }
            }
        }
        public string MapName
        {
            get
            {
                lock (this)
                {
                    return _MapName;
                }
            }
            set
            {
                lock (this)
                {
                    _MapName = value;
                }
            }
        }
        public string MapLocation
        {
            get
            {
                lock (this)
                {
                    return _MapLocation;
                }
            }
            set
            {
                lock (this)
                {
                    _MapLocation = value;
                }
            }
        }
        public string ScenarioName
        {
            get
            {
                lock (this)
                {
                    return _ScenarioName;
                }
            }
            set
            {
                lock (this)
                {
                    _ScenarioName = value;
                }
            }
        }
        public string ScenarioDescription
        {
            get
            {
                lock (this)
                {
                    return _ScenarioDescription;
                }
            }
            set
            {
                lock (this)
                {
                    _ScenarioDescription = value;
                }
            }
        }
        public string PlayerBrief
        {
            get
            {
                lock (this)
                {
                    return _PlayerBrief;
                }
            }
            set
            {
                lock (this)
                {
                    _PlayerBrief = value;
                }
            }
        }
        public double GameSpeed
        {
            get
            {
                lock (this)
                {
                    return _GameSpeed;
                }
            }
            set
            {
                lock (this)
                {
                    _GameSpeed = value;
                }
            }
        }
        public int Port
        {
            get
            {
                lock (this)
                {
                    return _Port;
                }
            }
            set
            {
                lock (this)
                {
                    _Port = value;
                }
            }
        }
        public TagPositionEnum TagPosition = TagPositionEnum.INVISIBLE;

        //public NetworkClient nc
        //{
        //    set
        //    {
        //        _nc = value;
        //    }
        //}

        public bool IsConnected
        {
            get
            {
                try
                {
                    return _nc.IsConnected();
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool AssetTransferEnabled
        {
            get
            {
                try
                {
                    return _AssetTransferEnabled;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            set
            {
                lock(this)
                {
                    _AssetTransferEnabled = value;
                }
            }
        }
        public SimulationModelInfo SimModel
        {
            get
            {
                lock (this)
                {
                    return _SimModel;
                }
            }
            set
            {
                lock (this)
                {
                    _SimModel = value;
                }
            }
        }


        public static DDD_Global Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DDD_Global();
                }
                return _instance;
            }
        }

        public String VoiceServerHostname
        {
            get
            {
                return _strVoiceServerHostname;
            }
            set
            {
                _strVoiceServerHostname = value;
            }
        }

        public int VoiceServerPort
        {
            get
            {
                return _iVoiceServerPort;
            }
            set
            {
                _iVoiceServerPort = value;
            }
        }

        public String ConaitoServerPassword
        {
            get
            {
                return _strConaitoServerPasswd;
            }
            set
            {
                _strConaitoServerPasswd = value;
            }
        }

        public bool VoiceChatEnabled
        {
            get
            {
                return _voiceChatEnabled;
            }
            set
            {
                _voiceChatEnabled = value;
            }
        }

        public Dictionary<String, List<String>> VoiceChannels
        {
            get { return _voiceChannels; }
            set { _voiceChannels = value; }
        }

        public NetworkClient NetClient
        {
            get { return _nc; }
        }

        public Boolean IsObserver
        {
            get { return _isObserver; }
            set { _isObserver = value; }
        }

        public Boolean IsForkReplay
        {
            get { return _isForkReplay; }
            set { _isForkReplay = value; }
        }
        public Boolean HasAuthenticated
        {
            get { return _hasAuthenticated; }
            set { _hasAuthenticated = value; }
        }

        #endregion properties

        private DDD_Global() {
            _nc = new Aptima.Asim.DDD.CommonComponents.NetworkTools.NetworkClient();
        }

        
        public bool Connect(string hostname, int port)
        {
            try
            {
                return _nc.Connect(hostname, port);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Connect Error");
                return false;
            }
        }
        public void Disconnect()
        {
            try
            {
                if (_nc != null)
                {
                    if (_nc.IsConnected())
                    {
                        _nc.Disconnect();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        public void Subscribe(string subscription_event)
        {
            try
            {
                _nc.Subscribe(subscription_event);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<SimulationEvent> GetEvents()
        {
            try
            {
                return _nc.GetEvents();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public void PutEvent(SimulationEvent sim_event)
        {
            try
            {
                _nc.PutEvent(sim_event);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


       
    }
}
