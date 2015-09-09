using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Threading;
using System.Drawing;

namespace Aptima.Asim.DDD.Client
{
    public enum EnabledLevels
    {
        DISABLED = 0,
        ENABLED = 1,
        WHILE_SELECTED = 2,
    }

    public class DDD_RangeRings
    {
        private RangeRingDisplayInfo _sensorRings;
        private RangeRingDisplayInfo _vulnerabilityRings;
        private RangeRingDisplayInfo _capabilityRings;
        private static DDD_RangeRings _instance = null;
        private static int _defaultRingNodeComplexity = 12; //12 nodes form a circle for ring, 30 degrees apart
        private static int _defaultRingColor = System.Drawing.Color.Red.ToArgb();
        private static int _defaultOpacity = 51; //of 255
        private static Aptima.Asim.DDD.Client.Common.GLCore.Canvas _canvasPtr = null;

        public static void SetCanvasPtr(Aptima.Asim.DDD.Client.Common.GLCore.Canvas c)
        {
            _canvasPtr = c;
        }

        private static object _textureLock = new object();
        private static Dictionary<string, object> _objectLocks = null;
        private static Dictionary<string, Dictionary<string, Texture>> _textureCollection = null;

        private static void TransformMetersToPixels(float radius, float h_metersPerPixel, float v_metersPerPixel, out float width, out float height)
        {
            //radius is initially in meters.  
            width = radius;
            height = radius;

            //change to pixels
            width /= h_metersPerPixel;
            height /= v_metersPerPixel;

            //mult by 2 for diameters.
            width *= 2;
            height *= 2;

            ScaleDownWidthHeight(ref width, ref height);
        }

        /// <summary>
        /// Width and Height can be at most 4096 each, due to Texture limitations
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private static void ScaleDownWidthHeight(ref float width, ref float height)
        {
            //AD: Great for showing an "estimated" ring, should display message to user.
            if (width > height)
            {
                if (width > 4096)
                {
                    double ratio = width / 4096;
                    width = (float)Math.Floor(width / ratio);
                    height = (float)Math.Floor(height / ratio);
                    Console.WriteLine("Width was above 4096 pixels, size reduced to {0} x {1}", width, height);
                }
            }
            else
            {
                if (height > 4096)
                {
                    double ratio = height / 4096;
                    width = (float)Math.Floor(width / ratio);
                    height = (float)Math.Floor(height / ratio);
                    Console.WriteLine("Width was above 4096 pixels, size reduced to {0} x {1}", width, height);
                }
            }
        }

        private static String EncodeRingNameType(string ringType, string ringName)
        {
            return string.Format("{0}:{1}", ringType, ringName);
        }

        private static void ParseRingNameType(string input, out string ringType, out string ringName)
        {
            char[] parser = { ':' };
            string[] parts = input.Split(parser);
            ringType = string.Empty;
            ringName = string.Empty;

            if (parts.Length > 0)
            {
                ringType = parts[0];
            }
            if (parts.Length > 1)
            {
                ringName = parts[1];
            }

        }

        /* not done yet.
         * System.Threading.Monitor.TryEnter(this,
         */

        public RangeRingDisplayInfo SensorRangeRings
        {
            get { return _sensorRings; }
        }
        public RangeRingDisplayInfo VulnerabilityRangeRings
        {
            get { return _vulnerabilityRings; }
        }
        public RangeRingDisplayInfo CapabilityRangeRings
        {
            get { return _capabilityRings; }
        }
        public static int RingComplexity
        {
            get { return _defaultRingNodeComplexity; }
            set { _defaultRingNodeComplexity = value; }
        }

        private DDD_RangeRings()
        {
            Reinitialize();
        }

        public static DDD_RangeRings GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DDD_RangeRings();
                _instance.Reinitialize();
            }

            return _instance;
        }

        public void Reinitialize()
        {
            _sensorRings = new RangeRingDisplayInfo();
            _sensorRings.RingColor = System.Drawing.Color.Green.ToArgb();
            _sensorRings.Opacity = _defaultOpacity;
            _sensorRings.CurrentlySelectedID = String.Empty;
            _vulnerabilityRings = new RangeRingDisplayInfo();
            _vulnerabilityRings.RingColor = System.Drawing.Color.Blue.ToArgb();
            _vulnerabilityRings.Opacity = _defaultOpacity;
            _vulnerabilityRings.CurrentlySelectedID = String.Empty;
            _capabilityRings = new RangeRingDisplayInfo();
            _capabilityRings.RingColor = System.Drawing.Color.Red.ToArgb();
            _capabilityRings.Opacity = _defaultOpacity;
            _capabilityRings.CurrentlySelectedID = String.Empty;
            _textureCollection = new Dictionary<string, Dictionary<string, Texture>>();
            _objectLocks = new Dictionary<string, object>();
        }

        public void DeSelectObject()
        {
            _sensorRings.CurrentlySelectedID = String.Empty;
            _vulnerabilityRings.CurrentlySelectedID = String.Empty;
            _capabilityRings.CurrentlySelectedID = String.Empty;
        }


    }

    public class RangeRingDisplayInfo
    {
        private EnabledLevels _enabled = EnabledLevels.DISABLED;
        private bool _displayUnmanagedAssets = false;
        private int _color = Color.White.ToArgb();
        private int _opacity = 51; //out of 255, this is 20%;
        private string _selectedId = string.Empty;

        public EnabledLevels EnabledLevel
        {
            get { return _enabled; }
            set { _enabled = value; }
        }
        public bool DisplayUnmanagedAssets
        {
            get { return _displayUnmanagedAssets; }
            set { _displayUnmanagedAssets = value; }
        }
        public int RingColor
        {
            get { return _color; }
            set { _color = value; }
        }
        public int OpaqueRingColor
        {
            get { return Color.FromArgb(_opacity, Color.FromArgb(_color)).ToArgb(); }
        }
        public String CurrentlySelectedID
        {
            get { return _selectedId; }
            set { _selectedId = value; }
        }
        public int Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }
        public void SetOpacityFromPercentage(int percentOpaque)
        {
            float op = percentOpaque * 2.55f;
            _opacity = Convert.ToInt32(Math.Round(op));
        }
        public int GetOpacityAsPercentage(int precisionDigits)
        {
            float percentOpaque = (float)_opacity / 255;
            percentOpaque = (float)Math.Round(percentOpaque, precisionDigits);
            int r = Convert.ToInt32(percentOpaque * 100);

            return r;
        }

        public RangeRingDisplayInfo()
        { }

        public bool IsDisplayAllowed(string selectedIdentifier, bool isAssetUnmanaged)
        {
            if (_enabled == EnabledLevels.DISABLED)
                return false;

            if (_enabled == EnabledLevels.WHILE_SELECTED)
            {
                if (selectedIdentifier != _selectedId)
                { //this object is not the currently selected object, dont display ring
                    return false;
                }
            }

            if (_displayUnmanagedAssets || isAssetUnmanaged)
            {//if "_display" is true, display all, or if the asset is managed by you, display it. 
                return true;
            }

            return false;
        }
    }
}
