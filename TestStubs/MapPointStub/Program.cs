using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.MapDataTools;

namespace Aptima.Asim.DDD.TestStubs.MapPointStub
{
    class Program
    {
        static void Main(string[] args)
        {
            MapPoint p = new MapPoint("10U", 5247475, 442063);
            System.Console.WriteLine(p);

            double Lat, Long;

            Lat = p.Lat;
            Long = p.Long;

            p.SetPointUTM(5257475, 452063);

            Lat = p.Lat;
            Long = p.Long;

        }
    }
}
