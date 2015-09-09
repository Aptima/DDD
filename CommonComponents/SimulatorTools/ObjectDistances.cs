using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

namespace Aptima.Asim.DDD.CommonComponents.SimulatorTools
{
    public class ObjectDistances
    {
        //no need for thread safety yet.
        private static Dictionary<String, Dictionary<String, double>> _objectDistances = new Dictionary<string, Dictionary<string, double>>();
        private static Dictionary<String, Vec3D> _objectLocations = new Dictionary<string, Vec3D>();
        private static Dictionary<String, double> _objectMaxSensorRanges = new Dictionary<string, double>();
        private static Dictionary<String, List<String>> _objectsInSensorRanges = new Dictionary<String, List<String>>();

        public static void UpdateObjectSensorRange(String objID, double maxSensorRange)
        {
            if (_objectMaxSensorRanges.ContainsKey(objID))
            {
                _objectMaxSensorRanges[objID] = maxSensorRange;
            }
            else
            {
                _objectMaxSensorRanges.Add(objID, maxSensorRange);
            }
            UpdateObjectsInSensorRange(objID);
        }

        public static List<string> GetObjectsInSensorRange(String sensingObjectID)
        {
            if (!_objectsInSensorRanges.ContainsKey(sensingObjectID))
                return new List<string>();

            return _objectsInSensorRanges[sensingObjectID];
        }

        //when the sensor range changes
        private static void UpdateObjectsInSensorRange(String sensingObjectID)
        {
            List<string> inRange = new List<string>();
            if (!_objectMaxSensorRanges.ContainsKey(sensingObjectID))
                return;
            if (!_objectDistances.ContainsKey(sensingObjectID))
                return;

            double? d;
            double maxRange = _objectMaxSensorRanges[sensingObjectID];



            foreach (String s in _objectDistances[sensingObjectID].Keys)
            {
                d = GetScalarDistanceBetweenObjects(sensingObjectID, s);
                if (d != null)
                {
                    if (d.Value <= maxRange)
                        inRange.Add(s);
                }
            }
            if (!_objectsInSensorRanges.ContainsKey(sensingObjectID))
            {
                _objectsInSensorRanges.Add(sensingObjectID, new List<string>());
            }
            _objectsInSensorRanges[sensingObjectID].Clear();
            _objectsInSensorRanges[sensingObjectID].AddRange(inRange);
        }

        private static void UpdateOthersForNewLocation(String newlyMovedObject)
        {
            if (!_objectDistances.ContainsKey(newlyMovedObject))
                return;
            
            double? d;
            foreach (string s in _objectDistances[newlyMovedObject].Keys)
            {
                d = GetScalarDistanceBetweenObjects(s, newlyMovedObject); //gets the null right
                if (d != null)
                {
                    if (!_objectMaxSensorRanges.ContainsKey(s))
                        continue;
                    if (!_objectsInSensorRanges.ContainsKey(s))
                        _objectsInSensorRanges.Add(s, new List<string>());
                    if (d.Value <= _objectMaxSensorRanges[s] && !_objectsInSensorRanges[s].Contains(newlyMovedObject))
                    {
                        _objectsInSensorRanges[s].Add(newlyMovedObject);
                    }
                    else if (_objectsInSensorRanges.ContainsKey(newlyMovedObject))
                    {
                        _objectsInSensorRanges[s].Remove(newlyMovedObject);
                    }

                }
            }
        }

        public static void UpdateObjectLocation(String objID, Vec3D loc)
        {
            if (_objectLocations.ContainsKey(objID))
            {
                _objectLocations[objID].X = loc.X;
                _objectLocations[objID].Y = loc.Y;
                _objectLocations[objID].Z = loc.Z;
            }
            else
            {
                _objectLocations.Add(objID, new Vec3D(loc));
            }
            double dist;
            foreach (String o in _objectLocations.Keys)
            {
                if (o == objID)
                    continue;
                dist = _objectLocations[o].ScalerDistanceTo(loc);
                SetScalarDistanceBetweenTwoObjects(o, objID, dist);
            }
            UpdateOthersForNewLocation(objID);
            UpdateObjectsInSensorRange(objID); //might be a bit expensive to do this often
        }

        private static void SetScalarDistanceBetweenTwoObjects(String obj1, String obj2, double val)
        {
            if (!_objectDistances.ContainsKey(obj1))
            {
                _objectDistances.Add(obj1, new Dictionary<string, double>());
            }
            if (!_objectDistances.ContainsKey(obj2))
            {
                _objectDistances.Add(obj2, new Dictionary<string, double>());
            }

            if (!_objectDistances[obj1].ContainsKey(obj2))
            {
                _objectDistances[obj1].Add(obj2, val);
            }
            else
            {
                _objectDistances[obj1][obj2] = val;
            }

            if (!_objectDistances[obj2].ContainsKey(obj1))
            {
                _objectDistances[obj2].Add(obj1, val);
            }
            else
            {
                _objectDistances[obj2][obj1] = val;
            }

        }

        public static void ClearScalarDistanceBetweenTwoObjects(String obj1, String obj2)
        {
            try
            {
                _objectDistances[obj1].Remove(obj2);
            }
            catch (Exception ex)
            { }
            try
            {
                _objectDistances[obj2].Remove(obj1);
            }
            catch (Exception ex)
            { }
        }

        public static void RemoveAnObject(String objID)
        {
            if (_objectDistances.ContainsKey(objID))
                _objectDistances.Remove(objID);

            foreach (String key in _objectDistances.Keys)
            {
                if (_objectDistances[key].ContainsKey(objID))
                {
                    _objectDistances[key].Remove(objID);
                }
            }
            if (_objectsInSensorRanges.ContainsKey(objID))
                _objectsInSensorRanges.Remove(objID);

            foreach (String key in _objectDistances.Keys)
            {
                _objectDistances[key].Remove(objID);
            }

            _objectMaxSensorRanges.Remove(objID);
            _objectLocations.Remove(objID);
        }

        public static double? GetScalarDistanceBetweenObjects(String obj1, String obj2)
        {
            if (_objectDistances == null)
                return null;
            if (_objectDistances.ContainsKey(obj1))
            {
                if (_objectDistances[obj1].ContainsKey(obj2))
                    return _objectDistances[obj1][obj2];
            }
            if (_objectDistances.ContainsKey(obj2))
            {
                if (_objectDistances[obj2].ContainsKey(obj1))
                    return _objectDistances[obj2][obj1];
            }

            return null;
        }
    }
}
