using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using AGT.Motion;

namespace VSG.PreviewSimulator
{
    class TripSchedule
    {
        private List<TripSegment> _Trip;
        private double _trip_duration = 0;

        public double TripDuration
        {
            get
            {
                return _trip_duration;
            }
        }
        public TripSchedule()
        {
            _Trip = new List<TripSegment>();
        }

        public void Clear()
        {
            _Trip.Clear();
            _trip_duration = 0;
        }

        public void InitializeTrip(List<MoveEvent> waypoints, Dictionary<string, double> speeds)
        {
            int last_index = waypoints.Count - 1;
            double duration = 0;
            TripSegment t1;

            Clear();

            for (int i = 1; i < waypoints.Count; i++)
            {

                duration = AGT_LinearMotion.CalculateMoveDuration(waypoints[i - 1].X, waypoints[i - 1].Y, waypoints[i].X, waypoints[i].Y, (waypoints[i].Throttle * (float)speeds[waypoints[i].Name]));

                t1 = new TripSegment((int)_trip_duration, (int)duration, waypoints[i].Throttle);
                t1.StartPosition = new Vector3(waypoints[i - 1].X, waypoints[i - 1].Y, waypoints[i - 1].Z);
                t1.EndPosition = new Vector3(waypoints[i].X, waypoints[i].Y, waypoints[i].Z);

                _Trip.Add(t1);
                _trip_duration = waypoints[i].Time + _trip_duration + duration;

            }


            duration = AGT_LinearMotion.CalculateMoveDuration(waypoints[last_index].X, waypoints[last_index].Y, waypoints[0].X, waypoints[0].Y,
                                    (waypoints[0].Throttle * (float)speeds[waypoints[0].Name]));
            t1 = new TripSegment((int)_trip_duration, (int)duration, waypoints[0].Throttle);
            t1.StartPosition = new Vector3(waypoints[last_index].X, waypoints[last_index].Y, waypoints[last_index].Z);
            t1.EndPosition = new Vector3(waypoints[0].X, waypoints[0].Y, waypoints[0].Z);
            _Trip.Add(t1);

            _trip_duration = waypoints[0].Time + _trip_duration + duration;

        }

        public Vector3 GetStartPosition(int now)
        {
            foreach (TripSegment s in _Trip)
            {
                if (s.IsWithinThisSegment(now))
                {
                    return s.StartPosition;
                }
            }
            return Vector3.Empty;
        }

        public Vector3 GetEndPosition(int now)
        {
            foreach (TripSegment s in _Trip)
            {
                if (s.IsWithinThisSegment(now))
                {
                    return s.EndPosition;
                }
            }
            return Vector3.Empty;
        }

        public float GetThrottle(int now)
        {
            foreach (TripSegment s in _Trip)
            {
                if (s.IsWithinThisSegment(now))
                {
                    return s.Throttle;
                }
            }
            return 0f;
        }
        public int GetEventIndex(int now)
        {
            foreach (TripSegment s in _Trip)
            {
                if (s.IsWithinThisSegment(now))
                {
                    return _Trip.IndexOf(s);
                }
            }
            return 0;
        }
        public Vector3 InterpolateTripPosition(int now, float max_speed)
        {

            foreach (TripSegment t in _Trip)
            {
                if (t.IsWithinThisSegment(now))
                {
                    AGT_LinearMotion motion_calculator = new AGT_LinearMotion(t.StartPosition.X, t.StartPosition.Y, t.StartPosition.Z);
                    motion_calculator.MoveTo(t.EndPosition.X, t.EndPosition.Y, t.EndPosition.Z, (t.Throttle * max_speed));

                    return motion_calculator.InterpolatePosition(t.CalculateSegmentOffset(now));
                }
            }
            //return Vector3.Empty;
            return _Trip[0].StartPosition;
        }

        public void ToString()
        {
            int index = 1;
            Console.WriteLine("Trip: Duration={0}", _trip_duration);
            foreach (TripSegment s in _Trip)
            {
                Console.WriteLine("Trip Segment {0}: Start={1}, End={2}", index, s.StartTime, s.EndTime);
                Console.WriteLine("Trip Start Position: " + s.StartPosition.ToString());
                Console.WriteLine("Trip End Position: " + s.EndPosition.ToString());
                index++;
            }
        }
    }

}
