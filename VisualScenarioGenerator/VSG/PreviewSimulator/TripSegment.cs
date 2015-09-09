using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;


namespace VSG.PreviewSimulator
{
    class TripSegment
    {
        private int _StartTime;
        private int _EndTime;
        private float _Throttle;
        public Vector3 StartPosition = Vector3.Empty;
        public Vector3 EndPosition = Vector3.Empty;

        public int StartTime
        {
            get
            {
                return _StartTime;
            }
        }
        public int EndTime
        {
            get
            {
                return _EndTime;
            }
        }
        public float Throttle
        {
            get
            {
                return _Throttle;
            }
        }
        public TripSegment(int start_time, int duration, float throttle)
        {
            _StartTime = start_time;
            _EndTime = _StartTime + duration;
            _Throttle = throttle;
        }

        public bool IsWithinThisSegment(int now)
        {
            return ((now >= _StartTime) && (now <= _EndTime));
        }
        public int CalculateSegmentOffset(int now)
        {
            int offset = now - _StartTime;
            if (offset < 0)
            {
                return 0;
            }
            return offset;
        }
    }
}
