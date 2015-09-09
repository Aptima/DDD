using System;
using System.Collections.Generic;
using System.Text;

using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;



namespace Aptima.Asim.DDD.TestStubs.Scratch
{
    class Program
    {
        static void Main(string[] args)
        {


            Polygon2D poly = new Polygon2D();
            poly.AddVertex(new Vec2D(1,1));
            poly.AddVertex(new Vec2D(4, 1));
            poly.AddVertex(new Vec2D(2.5, 3));
            poly.AddVertex(new Vec2D(4, 3));
            poly.AddVertex(new Vec2D(4, 4));
            poly.AddVertex(new Vec2D(2, 3.5));

            bool r;
            r = Polygon2D.IsPointInside(poly,new Vec2D(5, 3)); //false
            r = Polygon2D.IsPointInside(poly, new Vec2D(2, 2)); //true
            r = Polygon2D.IsPointInside(poly, new Vec2D(3.5, 3.5)); //true
            r = Polygon2D.IsPointInside(poly, new Vec2D(1, 3.5)); //false
            r = Polygon2D.IsPointInside(poly, new Vec2D(3, 2.5)); //false

            r = Polygon2D.DoLinesIntersect(new Vec2D(0, 2), new Vec2D(3, 0), new Vec2D(1, 1), new Vec2D(3, 3)); //true
            r = Polygon2D.DoLinesIntersect(new Vec2D(0, 2), new Vec2D(3, 0), new Vec2D(2, 1), new Vec2D(3, 3)); //false

            DataValue dv = DataValueFactory.BuildValue("CapabilityType");
            ((CapabilityValue)dv).effects.Add(new CapabilityValue.Effect("foo",45,10,.50));
            ((CapabilityValue)dv).effects.Add(new CapabilityValue.Effect("foo", 100, 5, .25));

            string s = DataValueFactory.XMLSerialize(dv);

            DataValue dv2 = DataValueFactory.XMLDeserialize(s);

            DataValue dv3 = DataValueFactory.BuildValue("VulnerabilityType");

            VulnerabilityValue.Transition t = new VulnerabilityValue.Transition("dead");
            t.conditions.Add(new VulnerabilityValue.TransitionCondition("foo",50,0,0));
            t.conditions.Add(new VulnerabilityValue.TransitionCondition("bar", 20,0,0));

            ((VulnerabilityValue)dv3).transitions.Add(t);

            t = new VulnerabilityValue.Transition("hurt");
            t.conditions.Add(new VulnerabilityValue.TransitionCondition("foo", 25,0,0));
            ((VulnerabilityValue)dv3).transitions.Add(t);

            s = DataValueFactory.XMLSerialize(dv3);

            DataValue dv4 = DataValueFactory.XMLDeserialize(s);
            System.Console.WriteLine(s == DataValueFactory.XMLSerialize(dv4));

            DataValue dv5 = DataValueFactory.BuildValue("StateTableType");
            DataValue dv6 = DataValueFactory.BuildValue("AttributeCollectionType");
            ((StateTableValue)dv5).states["foo"] = DataValueFactory.BuildValue("DoubleType");
            ((AttributeCollectionValue)dv6).attributes["foo2"] = DataValueFactory.BuildValue("DoubleType");
            ((StateTableValue)dv5).states["bar"] = dv6;

            s = DataValueFactory.XMLSerialize(dv5);

            DataValue dv7 = DataValueFactory.XMLDeserialize(s);

            DataValue dv8 = DataValueFactory.BuildValue("StringListType");
            ((StringListValue)dv8).strings.Add("Foo");
            ((StringListValue)dv8).strings.Add("Bar");

            s = DataValueFactory.XMLSerialize(dv8);

            dv8 = DataValueFactory.XMLDeserialize(s);

            DataValue dv9 = DataValueFactory.BuildValue("PolygonType");
            ((PolygonValue)dv9).points.Add(new PolygonValue.PolygonPoint(0, 0));
            ((PolygonValue)dv9).points.Add(new PolygonValue.PolygonPoint(10.234, 34.097));
            ((PolygonValue)dv9).points.Add(new PolygonValue.PolygonPoint(10.234, 1.2));

            s = DataValueFactory.XMLSerialize(dv9);

            DataValue dv10 = DataValueFactory.XMLDeserialize(s);

        }
    }
}
