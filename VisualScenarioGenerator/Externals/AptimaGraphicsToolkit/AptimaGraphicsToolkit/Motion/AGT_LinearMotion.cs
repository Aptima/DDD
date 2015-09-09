using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using System.Runtime.InteropServices;

using System.Threading;

namespace AGT.Motion
{
    public class AGT_LinearMotion
    {

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        public static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        public static extern bool QueryPerformanceCounter(ref long PerformanceCount);


        public ThreadStart OnMoveComplete = null;

        public bool IsRunning
        {
            get
            {
                lock (this)
                {
                    return _running;
                }
            }
        }

        public Vector3 StartPosition
        {
            get
            {
                lock (this)
                {
                    return _start_position;
                }
            }
        }

        public Vector3 CurrentPosition
        {
            get
            {
                lock (this)
                {
                    return _current_position;
                }
            }
        }
        public Vector3 Destination
        {
            get
            {
                lock (this)
                {
                    return _destination;
                }
            }
        }
        
        public float AngleOfAttack
        {
            get
            {
                if (IsRunning)
                {
                    lock (this)
                    {
                        return _angle_of_attack;
                    }
                }
                return 0;
            }
        }
        public double MoveDuration
        {
            get
            {
                if (IsRunning)
                {
                    lock (this)
                    {
                        return _move_duration;
                    }
                }
                return 0;
            }
        }
        public double TTD = 0;

        #region Private Members
        private bool _running;

        private float _angle;
        private float _max_distance;
        private float _deltaX;
        private float _deltaY;
       
        private Vector3 _start_position;
        private Vector3 _destination;
        private Vector3 _current_position;

        private float _angle_of_attack;
        private float _rate;

        // 90 degrees in radians
        private static float Radians90deg = 90 * 0.0174532925f;
        // 270 degrees in radians
        private static float Radians270deg = 270 * 0.0174532925f;
        private long _start_tick = 0;
        private long _ticks_per_second = 0;

        private double _move_duration = 0;
        #endregion

       

        public AGT_LinearMotion(float meters_x, float meters_y, float meters_z)
        {
            this._angle = 0;
            this._max_distance = 0;
            this._deltaX = 0;
            this._deltaY = 0;
            this._move_duration = 0;
            this._running = false;
            this.OnMoveComplete = null;
            _start_position = new Vector3(meters_x, meters_y, meters_z);
            _destination = _current_position =  _start_position;           
        }

        public AGT_LinearMotion(float meters_x, float meters_y, float meters_z, ThreadStart move_complete)
        {
            this._angle = 0;
            this._max_distance = 0;
            this._deltaX = 0;
            this._deltaY = 0;
            this._move_duration = 0;
            this._running = false;
            this.OnMoveComplete = move_complete;
            _start_position = new Vector3(meters_x, meters_y, meters_z);
            _destination = _current_position = _start_position;           
        }

        public void ResetPosition(float x, float y, float z)
        {
            lock (this)
            {
                this._angle = 0;
                this._max_distance = 0;
                this._deltaX = 0;
                this._deltaY = 0;
                this._move_duration = 0;
                this._running = false;
                _start_position.X = x;
                _start_position.Y = y;
                _start_position.Z = z;
                _destination = _current_position = _start_position;
            }
        }

        public static double CalculateMoveDuration(float x1, float y1, float x2, float y2, float meters_per_second)
        {
            float _angle = 0;
            float _max_distance = 0;

            float _rate = meters_per_second;

            float _deltaX = x2 - x1;
            float _deltaY = y2 - y1;

            float lx = Math.Abs(_deltaX);
            float ly = Math.Abs(_deltaY);

            if (ly != 0)
            {
                _angle = (float)Math.Atan(ly / lx);
            }
            else
            {
                _angle = 0;
            }
            if (_angle == 0)
            {
                _max_distance = lx;
            }
            else
            {
                _max_distance = (float)(ly / Math.Sin(_angle));
            }
            return Math.Round( ((float)_max_distance / (float)_rate), 0);
        }

        public void MoveTo(float x, float y, float z, float meters_per_second)
        {
            lock (this)
            {
                _destination.X = x;
                _destination.Y = y;
                _destination.Z = z;

                QueryPerformanceCounter(ref _start_tick);
                QueryPerformanceFrequency(ref _ticks_per_second);

                _rate = meters_per_second;
                _running = true;

                _deltaX = _destination.X - _start_position.X;
                _deltaY = _destination.Y - _start_position.Y;

                float lx = Math.Abs(_deltaX);
                float ly = Math.Abs(_deltaY);

                if (ly != 0)
                {
                    _angle = (float)Math.Atan(ly / lx);
                }
                else
                {
                    _angle = 0;
                }
                if (_angle == 0)
                {
                    _max_distance = lx;
                }
                else
                {
                    _max_distance = (float)(ly / Math.Sin(_angle));
                }
                _angle_of_attack = CalculateAngleOfAttack(_angle, _deltaX, _deltaY);
                _move_duration = Math.Truncate((float)_max_distance / (float)_rate);
            }
        }

        public Vector3 InterpolatePosition(float elapsed_time)
        {
            lock (this)
            {
                float distance = CalculateDistance(elapsed_time);

                if (TTD != Math.Truncate(CalculateTTD(_max_distance - distance)))
                {
                    TTD = Math.Truncate(CalculateTTD(_max_distance - distance));
                }

                if (distance >= _max_distance)
                {
                    _running = false;
                    this._angle = 0;
                    this._max_distance = 0;
                    this._deltaX = 0;
                    this._deltaY = 0;
                    _start_position = _destination;
                    _destination = Vector3.Empty;

                    return _start_position;
                }

                switch (_deltaX < 0)
                {
                    case true:
                        _current_position.X = _start_position.X - SolveForX(distance);
                        break;
                    case false:
                        _current_position.X = _start_position.X + SolveForX(distance);
                        break;
                }
                switch (_deltaY < 0)
                {
                    case true:
                        _current_position.Y = _start_position.Y - SolveForY(distance);
                        break;
                    case false:
                        _current_position.Y = _start_position.Y + SolveForY(distance);
                        break;
                }
                return _current_position;
            }
        }
        


        public Vector3 ReCalculatePosition()
        {
            lock (this)
            {
                long current_tick = 0;
                QueryPerformanceCounter(ref current_tick);
                float distance = CalculateDistance((float)(current_tick - _start_tick) / (float)_ticks_per_second);

                if (TTD != Math.Truncate(CalculateTTD(_max_distance - distance)))
                {
                    TTD = Math.Truncate(CalculateTTD(_max_distance - distance));
                }

                if (distance >= _max_distance)
                {
                    _running = false;
                    this._angle = 0;
                    this._max_distance = 0;
                    this._deltaX = 0;
                    this._deltaY = 0;
                    _start_position = _destination;
                    _destination = Vector3.Empty;

                    if (OnMoveComplete != null)
                    {
                        System.Threading.Thread th = new System.Threading.Thread(OnMoveComplete);
                        th.Start();
                    }

                    return _start_position;
                }

                switch (_deltaX < 0)
                {
                    case true:
                        _current_position.X = _start_position.X - SolveForX(distance);
                        break;
                    case false:
                        _current_position.X = _start_position.X + SolveForX(distance);
                        break;
                }
                switch (_deltaY < 0)
                {
                    case true:
                        _current_position.Y = _start_position.Y - SolveForY(distance);
                        break;
                    case false:
                        _current_position.Y = _start_position.Y + SolveForY(distance);
                        break;
                }
                return _current_position;
            }
        }


        public void Stop()
        {
            lock (this)
            {
                if (_running)
                {
                    _running = false;
                    this._angle = 0;
                    this._max_distance = 0;
                    this._deltaX = 0;
                    this._deltaY = 0;
                    _destination =  _start_position = _current_position;
                    Console.WriteLine("Linear Motion STOP");
                    if (OnMoveComplete != null)
                    {
                        Console.WriteLine("Send notification");
                        System.Threading.Thread th = new System.Threading.Thread(OnMoveComplete);
                        th.Start();
                    }

                }
            }
        }


        #region Private Methods - Motion Calculators
        // Distance = Rate * Time;
        private float CalculateDistance(float elapsed_seconds)
        {
            return (_rate * elapsed_seconds);
        }

        private float CalculateTTD(float distance)
        {
                return distance / _rate;
        }

        private float SolveForY(float Distance)
        {
            return Distance * (float)Math.Sin(_angle);
        }

        private float SolveForX(float Distance)
        {
            float result = Distance * (float)Math.Cos(_angle);
            return result;
        }

        private float CalculateAngleOfAttack(float arcTan, float deltaX, float deltaY)
        {
            // 1 radian = 57.2957795
            // 1 degree = 0.0174532925 radians

            // Lower right quadrant
            if ((deltaX >= 0) && (deltaY  >= 0))
            {
                return (Radians90deg + arcTan);
            }
            // Upper right quadrant
            if ((deltaX >= 0) && (deltaY < 0))
            {
                return (Radians90deg - arcTan);
            }
            // Upper left quadrant
            if ((deltaX < 0) && (deltaY < 0))
            {
                return (Radians270deg + arcTan);
            }
            // Lower left quadrant
            if ((deltaX < 0) && (deltaY >= 0))
            {
                return (Radians270deg - arcTan);
            }
            return 0;
        }
        #endregion

    }
}
