using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;


namespace Aptima.Asim.DDD.Client.Common.GLCore.PathController
{
    public class Path_Linear:BasePathObject
    {
        public bool IsRunning
        {
            get
            {
                return _running;
            }
        }
        public Vector3 StartPosition
        {
            get
            {
                return _start_position;
            }
        }
        public Vector3 Destination
        {
            get
            {
                return _destination;
            }
        }
        public float AngleOfAttack
        {
            get
            {
                if (IsRunning)
                {
                    return _angle_of_attack;
                }
                else
                {
                    return _angle_of_attack;
                }
            }
        }
        public double TTD = 0;
        public string TTDStr = string.Empty;
        public string FullTTDStr = string.Empty;

        #region Private Members

        private bool _running;

        private float _angle;
        private float _max_distance;
        private float _deltaX;
        private float _deltaY;
        private float _deltaZ;
       
        private Vector3 _start_position;
        private Vector3 _destination;
        private float _angle_of_attack;

        private float _vertical_angle_of_attack;
        private float _vertical_angle;
        private float _max_vertical_distance;

        // 90 degrees in radians
        private static float Radians90deg = 90 * 0.0174532925f;
        // 270 degrees in radians
        private static float Radians270deg = 270 * 0.0174532925f;
        #endregion



        public Path_Linear()
        {
            this._angle = 0;
            this._max_distance = 0;
            this._deltaX = 0;
            this._deltaY = 0;
            this._deltaZ = 0;
            this._vertical_angle_of_attack = 0;
            this._vertical_angle = 0;
            this._running = false;
            _start_position = Vector3.Empty;
            _destination = Vector3.Empty;
        }


        public void InitializeLineFormula(float start_xpos, float start_ypos, float start_zpos, float end_xpos, float end_ypos, float end_zpos, float rate_of_speed)
        {
            _start_position.X = start_xpos;
            _start_position.Y = start_ypos;
            _start_position.Z = start_zpos;

            _destination.X = end_xpos;
            _destination.Y = end_ypos;
            _destination.Z = end_zpos;

            this.Rate = rate_of_speed;
            this.ResetTimer();
            _running = true;

            _deltaX = _destination.X - _start_position.X;
            _deltaY = _destination.Y - _start_position.Y;
            _deltaZ = _destination.Z - _start_position.Z;
            float lx = Math.Abs(_deltaX);
            float ly = Math.Abs(_deltaY);
            float lz = Math.Abs(_deltaZ);
            _angle = (float)Math.Atan(ly/lx);
            if (_angle == 0)
            {
                _max_distance = lx;
            }
            else
            {
                _max_distance = (float)(ly / Math.Sin(_angle));
            }
            _angle_of_attack = CalculateAngleOfAttack(_angle, _deltaX, _deltaY);

            //calculate vertical angle infos
            if (lz > 0)
            {//if no change in altitude, 2-d math should be fine.
                _vertical_angle = (float)Math.Atan(lz / _max_distance);
                if (_vertical_angle == 0)
                {
                    _max_vertical_distance = _max_distance; //??
                }
                else
                {

                    _max_vertical_distance = (float)(lz / Math.Sin(_vertical_angle));
                    Console.WriteLine(String.Format("lx = {0}\nly = {1}\nlz = {2}\nVert Distance = {3}", lx, ly, lz, _max_vertical_distance));
                }
                _vertical_angle_of_attack = CalculateAngleOfAttack(_vertical_angle, _max_distance, _deltaZ);

                Console.WriteLine(String.Format("Changing altitude:\nStart Position:\nX={0}\nY={1}\nZ={2}\nDestination Position:\nX={3}\nY={4}\nZ={5}\rRate of Speed: {10}\nAngle of attack={6}\nVertical Angle={7}\nHorizontal distance={8}\nVertical Distance={9}", start_xpos, start_ypos, start_zpos, end_xpos, end_ypos, end_zpos, _angle_of_attack, _vertical_angle, _max_distance, _max_vertical_distance, rate_of_speed));

            }
        }

        public void CalculateNewPosition(ref Vector3 NewPosition)
        {
            float distance;
            float twoDdistance = CalculateDistance();
            float twoDhorizontalDistance = SolveForHorizontalDistance(twoDdistance);
            float twoDaltDistance = SolveForVerticalAltitude(twoDdistance);

            distance = twoDhorizontalDistance;
            if (TTD != Math.Truncate(CalculateTTD(_max_distance - distance)))
            {
                TTD = Math.Truncate(CalculateTTD(_max_distance - distance));
                TTDStr = string.Format("{0} sec", TTD);
                FullTTDStr = string.Format("Time To Destination: {0} sec", TTD);
            }

            if (distance >= _max_distance)
            {
                _running = false;
                this._angle = 0;
                this._vertical_angle = 0;
                this._max_vertical_distance = 0;
                this._max_distance = 0;
                this._deltaX = 0;
                this._deltaY = 0;
                this._deltaZ = 0;
                _start_position = _destination;
                _destination = Vector3.Empty;
                NewPosition.X = _start_position.X;
                NewPosition.Y = _start_position.Y;
                NewPosition.Z = _start_position.Z;
                return;
            }

            switch (_deltaX < 0)
            {
                case true:
                    NewPosition.X = _start_position.X - SolveForX(distance);
                    break;
                case false:
                    NewPosition.X = _start_position.X + SolveForX(distance);
                    break;
            }
            switch (_deltaY < 0)
            {
                case true:
                    NewPosition.Y = _start_position.Y - SolveForY(distance);
                    break;
                case false:
                    NewPosition.Y = _start_position.Y + SolveForY(distance);
                    break;
            }

            switch (_deltaZ < 0)
            {
                case true:
                    NewPosition.Z = _start_position.Z - twoDaltDistance;// -SolveForY(distance);
                    break;
                case false:
                    NewPosition.Z = _start_position.Z + twoDaltDistance;// +SolveForY(distance);
                    break;
            }
        }


        public void StopPathCalculator()
        {
            if (_running)
            {
                _running = false;
                this._angle = 0;
                this._max_distance = 0;
                this._deltaX = 0;
                this._deltaY = 0;
                this._deltaZ = 0;
                this._vertical_angle = 0;
                this._vertical_angle_of_attack = 0;
                _start_position = _destination;
                _destination = Vector3.Empty;
            }
        }

        private float SolveForHorizontalDistance(float Distance)
        {
            return Distance * (float)Math.Cos(_vertical_angle);
        }

        private float SolveForVerticalAltitude(float Distance)
        {
            return Distance * (float)Math.Sin(_vertical_angle);
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


    }
}
