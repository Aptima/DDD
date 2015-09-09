using System;
using System.Collections.Generic;
using System.Text;

namespace Aptima.Asim.DDD.CommonComponents.MapDataTools
{
    public class MapPoint
    {
        public string UTMZone;
        public double UTMNorthing;
        public double UTMEasting;
        public double Lat;
        public double Long;

        public MapPoint(double Lat, double Long)
        {
            SetPointLL(Lat, Long);
        }
        public MapPoint(string zone, double northing, double easting)
        {
            SetPointUTM(zone, northing, easting);
        }

        public void SetPointUTM(string zone, double northing, double easting)
        {
            UTMZone = zone;
            UTMNorthing = northing;
            UTMEasting = easting;

            CoordinateConversion.UTMtoLL(23, UTMNorthing, UTMEasting, UTMZone, out Lat, out Long);
        }
        public void SetPointUTM(double northing, double easting)
        {
            UTMNorthing = northing;
            UTMEasting = easting;

            CoordinateConversion.UTMtoLL(23, UTMNorthing, UTMEasting, UTMZone, out Lat, out Long);
        }

        public void SetPointLL(double Lat, double Long)
        {
            this.Lat = Lat;
            this.Long = Long;

            CoordinateConversion.LLtoUTM(23, Lat, Long, out UTMNorthing, out UTMEasting, out UTMZone);
        }

        public override string ToString()
        {
            string s = "LL(" + Lat.ToString() + "," + Long.ToString() + ") UTM(";
            s += UTMZone + "," + UTMNorthing.ToString() + "," + UTMEasting + ")";
            return s;
        }
    }
}
