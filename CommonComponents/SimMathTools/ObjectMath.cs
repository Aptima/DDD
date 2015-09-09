using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationObjectTools;

namespace Aptima.Asim.DDD.CommonComponents.SimMathTools
{
    public class ObjectMath
    {
        //find closest point
        public static Vec3D FindClosestPoint(Vec3D sensingPoint, Vec3D sensingDirection, Vec3D emittingPoint)
        {
            Vec3D closestPoint = new Vec3D(sensingPoint); //will add to this when t is defined
            double t = FindT(sensingPoint, emittingPoint, sensingDirection);

            closestPoint.X += t * sensingDirection.X;
            closestPoint.Y += t * sensingDirection.Y;
            closestPoint.Z += t * sensingDirection.Z;

            return closestPoint;//closestPoint;
        }

        //find t
        public static double FindT(Vec3D sensorPoint, Vec3D emitterPoint, Vec3D coneDirection)
        {
            double t = 0.0;

            t = coneDirection.X*(emitterPoint.X - sensorPoint.X);
            t += coneDirection.Y * (emitterPoint.Y - sensorPoint.Y);
            t += coneDirection.Z * (emitterPoint.Z - sensorPoint.Z);
            t /= (Math.Pow(coneDirection.X,2)+ Math.Pow(coneDirection.Y,2) + Math.Pow(coneDirection.Z, 2));

            return t;
        }

        //Find obstructions
        //public static List<string> FindObstructions(List<string> allObstructions, Vec3D sensorPoint, Vec3D emitterPoint)
        //{//should pass a list of SimulationObjectProxies?

        //    List<string> obstructionList = new List<string>();
        //    SimulationObject obj;
        //    //do obstruction calculations
        //    foreach (string region in allObstructions)
        //    { 
        //        obj = bbc
        //        Polygon3D reg = new Polygon3D(
        //    }

        //    return obstructionList;
        //    //return a list of ACVs?
        //}


        
        /// <summary>
        /// This method determines if 2 objects are within a certain range.  Returns true if
        /// the distance between the two objects is less than the range, otherwise returns false.  Offers easy
        /// calculation of two LocationValue objects.
        /// </summary>
        /// <param name="range">The max distance that can be between the two objects while still returning true.</param>
        /// <param name="objectA">The first object's location as a LocationValue.</param>
        /// <param name="objectB">The second object's location as a LocationValue.</param>
        /// <returns></returns>
        public static bool IsWithinRange(double range, LocationValue objectA, LocationValue objectB)
        {
            Vec3D objA = new Vec3D(objectA);
            Vec3D objB = new Vec3D(objectB);
            
            if (range > objA.ScalerDistanceTo(objB))
            {
                return true;
            }

            return false;
        }



        //detection
        public static DetectedAttributeValue Detection(LocationValue sensingLocation,
                                                        LocationValue emittingLocation,
                                                        DataValue emittingObjectAttribute,
                                                        List<ConeValue> sensingCones,
                                                        Dictionary<string, double> emitters,
                                                        List<SimulationObjectProxy> obstructions,
                                                        ref Random random)
        {
            DetectedAttributeValue returnDAV = new DetectedAttributeValue();
            Vec3D sensingPoint = new Vec3D(sensingLocation);
            Vec3D emittingPoint = new Vec3D(emittingLocation);
            Vec3D sensingDirection = new Vec3D(sensingPoint); //can't set to blank, just ignore.
            double coneAngle = 0.0;
            double angleBetweenSensorAndEmitter = 0.0;

            foreach(ConeValue cone in sensingCones)
            {
                sensingDirection.Set(cone.direction);
                Vec3D closestPoint = FindClosestPoint(sensingPoint, sensingDirection, emittingPoint);
                if (cone.extent > sensingPoint.ScalerDistanceTo(closestPoint))
                {//p* is within the cone's extent. 
                    
                    //determine if emitting point is within the spread of the cone.
                    coneAngle = cone.spread * Math.PI / 180;// System.Math.Atan(emittingPoint.ScalerDistanceTo(closestPoint) / sensingPoint.ScalerDistanceTo(closestPoint));//should be cone.spread
                    angleBetweenSensorAndEmitter = System.Math.Atan(emittingPoint.ScalerDistanceTo(closestPoint) / sensingPoint.ScalerDistanceTo(closestPoint));

                    if (coneAngle < angleBetweenSensorAndEmitter)
                    {
                        continue; //should go to next cone.
                    }
                    if (emitters.ContainsKey(cone.level))
                    {//emitter has an emission at the same level as the cone.
                        //returnDAV = (DetectedAttributeValue)ObfuscateAttributeValue(emittingObjectAttribute, emitters[cone.level], ref random);
                        returnDAV = (DetectedAttributeValue)FuzzAttributeValue(emittingObjectAttribute, emitters[cone.level], ref random);
    
                        return returnDAV;
                    }
                    else
                    { //emitter does not emit the same level as the cone.
                        //should this find next best, or just move on?
                    }
                }
            }

            return null;
        }
        private static DataValue ObfuscateAttributeValue(DataValue dv, double confidence, ref Random random)
        { //maybe pass level as well, have a sim model def of how each model affects clarity?

            DetectedAttributeValue returnValue = new DetectedAttributeValue();
            ((DetectedAttributeValue)returnValue).value = dv;
            ((DetectedAttributeValue)returnValue).stdDev = confidence;
            //check data type
            string attributeType = dv.dataType;
            
            //switch statement
            switch (attributeType)
            {
                case "LocationType":
                    ObfuscateLocationValue(ref returnValue, ref random);
                    break;
                case "VelocityType":
                    ObfuscateVelocityValue(ref returnValue, ref random);
                    break;
                case "IntegerType":
                    ObfuscateIntegerValue(ref returnValue, ref random);
                    break;
                case "DoubleType":
                    ObfuscateDoubleValue(ref returnValue, ref random);
                    break;
                case "StringType":
                    ObfuscateStringValue(ref returnValue, ref random);
                    break;
                default:
                    break;
            }

            return returnValue as DataValue;
        }
        private static void ObfuscateStringValue(ref DetectedAttributeValue targetDV, ref Random random)
        {
            double confidence = targetDV.stdDev;
            string strVal = ((StringValue)((DetectedAttributeValue)targetDV).value).value;
            double X = random.NextDouble();
            double Y = .5 - (X / 2);
            double Z = ZTransformer.ZTransformGivenProbability(Y);
            int strLen = strVal.Length;
            strLen = (int)Math.Round((Z * confidence) + strLen,0);
            char[] str;
            if (strLen <= strVal.Length)
            {
                str = strVal.ToCharArray(0, strLen);
            }
            else
            {
                for (int b = strVal.Length; b < strLen; b++)
                {
                    strVal += Convert.ToChar(random.Next(49, 122));
                }
                str = strVal.ToCharArray();
            }
            StringBuilder sb = new StringBuilder();
            for (int a = 0; a < strLen; a++)
            {
                int asciiVal = Convert.ToInt32(str[a]);
                asciiVal = (int)Math.Round((ZTransformer.ZTransformGivenProbability(.5 - (random.NextDouble() / 2)) * confidence) + asciiVal, 0);
                //str[a] = Convert.ToChar(asciiVal);
                sb.Append(Convert.ToChar(asciiVal));
            }
            //((StringValue)((DetectedAttributeValue)targetDV).attribute).value = str.ToString();
            ((StringValue)((DetectedAttributeValue)targetDV).value).value = sb.ToString();
            targetDV.stdDev = GetConfidence(Y) * 100;
        }
        private static void ObfuscateIntegerValue(ref DetectedAttributeValue targetDV, ref Random random)
        {
            double confidence = targetDV.stdDev;
            int intVal = ((IntegerValue)((DetectedAttributeValue)targetDV).value).value;
            double X = random.NextDouble();
            double Y = .5 - (X / 2);
            double Z = ZTransformer.ZTransformGivenProbability(Y);
            //double area = (double)(random.Next((int)(Z * 10), (int)(Z * -10)) / 10.0);

            //obfuscate int
            ((IntegerValue)((DetectedAttributeValue)targetDV).value).value = (int)(Z * confidence + intVal);
            //exit
            //targetDV.confidence = GetConfidence(ZTransformer.ZTransformGivenZ(area)) * 100;
            targetDV.stdDev = GetConfidence(Y) * 100;
        }
        private static void ObfuscateDoubleValue(ref DetectedAttributeValue targetDV, ref Random random)
        {
            double confidence = targetDV.stdDev;
            double dblVal = ((DoubleValue)((DetectedAttributeValue)targetDV).value).value;
            double X = random.NextDouble();
            double Y = .5 - (X/2);
            double Z = ZTransformer.ZTransformGivenProbability(Y);
            //double area = (double)(random.Next((int)(Z * 10), (int)(Z * -10)) / 10.0);
            

            //obfuscate int
            ((DoubleValue)((DetectedAttributeValue)targetDV).value).value = (double)(Z * confidence + dblVal);
            //exit
            targetDV.stdDev = GetConfidence(Y) * 100;
        }
        private static void ObfuscateVelocityValue(ref DetectedAttributeValue targetDV, ref Random random)
        { 
            double confidence = targetDV.stdDev;
            double vx = ((VelocityValue)((DetectedAttributeValue)targetDV).value).VX;
            double vy = ((VelocityValue)((DetectedAttributeValue)targetDV).value).VY;
            double vz = ((VelocityValue)((DetectedAttributeValue)targetDV).value).VZ;
            double X = random.NextDouble();
            double Y = .5 - (X / 2);
            double Z = ZTransformer.ZTransformGivenProbability(Y);
            //double area = (double)(random.Next((int)(Z * 10), (int)(Z * -10)) / 10.0);

            //obfuscate VX
            ((VelocityValue)((DetectedAttributeValue)targetDV).value).VX = (double)(Z * confidence + vx);
            //obfuscate VY
            ((VelocityValue)((DetectedAttributeValue)targetDV).value).VY = (double)(Z * confidence + vy);
            //obfuscate VZ
            ((VelocityValue)((DetectedAttributeValue)targetDV).value).VZ = (double)(Z * confidence + vz);
            //exit
            targetDV.stdDev = GetConfidence(Y) * 100;
        }
        private static void ObfuscateLocationValue(ref DetectedAttributeValue targetDV, ref Random random)
        {
            double confidence = targetDV.stdDev;
            double x = ((LocationValue)((DetectedAttributeValue)targetDV).value).X;
            double y = ((LocationValue)((DetectedAttributeValue)targetDV).value).Y;
            double z = ((LocationValue)((DetectedAttributeValue)targetDV).value).Z;
            double X = random.NextDouble();
            double Y = .5 - (X / 2);
            double Z = ZTransformer.ZTransformGivenProbability(Y);

            //obfuscate X
            ((LocationValue)((DetectedAttributeValue)targetDV).value).X = (double)(Z * confidence + x);
            //obfuscate Y
            ((LocationValue)((DetectedAttributeValue)targetDV).value).Y = (double)(Z * confidence + y);
            //obfuscate Z
            ((LocationValue)((DetectedAttributeValue)targetDV).value).Z = (double)(Z * confidence + z);
            //exit
            targetDV.stdDev = GetConfidence(Y) * 100;
        }
        private static double GetConfidence(double area)
        { 
            return (1 - Math.Abs(.5 - area) * 2);
        }
/* *************************************************************** */
        private static DataValue FuzzAttributeValue(DataValue dv, double confidence, ref Random random)
        { //maybe pass level as well, have a sim model def of how each model affects clarity?

            DetectedAttributeValue returnValue = new DetectedAttributeValue();
            ((DetectedAttributeValue)returnValue).value = dv;
            ((DetectedAttributeValue)returnValue).stdDev = confidence;
            //check data type
            string attributeType = dv.dataType;

            //switch statement
            switch (attributeType)
            {
                case "LocationType":
                    FuzzLocationValue(ref returnValue, ref random);
                    break;
                case "VelocityType":
                    FuzzVelocityValue(ref returnValue, ref random);
                    break;
                case "IntegerType":
                    FuzzIntegerValue(ref returnValue, ref random);
                    break;
                case "DoubleType":
                    FuzzDoubleValue(ref returnValue, ref random);
                    break;
                case "StringType":
                    FuzzStringValue(ref returnValue, ref random);
                    break;
                default:
                    break;
            }

            return returnValue as DataValue;
        }

        
        
        private static void FuzzDoubleValue(ref DetectedAttributeValue targetDV, ref Random random)
        {
            // not called if stdDev=0
            double stdDev = targetDV.stdDev;
            double dblVal = ((DoubleValue)((DetectedAttributeValue)targetDV).value).value;
            double r = random.NextDouble();// in [0,1]
            double z = ZInverse.FindZ(r);
         

            //add noise to double
            ((DoubleValue)((DetectedAttributeValue)targetDV).value).value = (double)(z * stdDev + dblVal);
            //exit
           // No: let smaller confidence be what chooses a value in ViewPro targetDV.confidence = 1 / stdDev; 
        }

     private static void FuzzVelocityValue(ref DetectedAttributeValue targetDV, ref Random random)
        {
            double stdDev = targetDV.stdDev;

            double VX = ((VelocityValue)((DetectedAttributeValue)targetDV).value).VX;
            double VY=((VelocityValue)((DetectedAttributeValue)targetDV).value).VY;
            double VZ = ((VelocityValue)((DetectedAttributeValue)targetDV).value).VZ;
            double r = random.NextDouble();// in [0,1]
         double vx = ZInverse.FindZ(r);
         r = random.NextDouble();
         double vy = ZInverse.FindZ(r);
         r = random.NextDouble();
         double vz = ZInverse.FindZ(r);

  
            //add noise to  VX
            ((VelocityValue)((DetectedAttributeValue)targetDV).value).VX = (double)(vx * stdDev + VX);
            //add noise toe VY
            ((VelocityValue)((DetectedAttributeValue)targetDV).value).VY = (double)(vy * stdDev + VY);
            //add noise to VZ
            ((VelocityValue)((DetectedAttributeValue)targetDV).value).VZ = (double)(vz * stdDev + VZ);
            //exit
            //No: let smaller confidence be what chooses a value in ViewProtargetDV.confidence= 1/stdDev;
        }

        private static void FuzzLocationValue(ref DetectedAttributeValue targetDV, ref Random random)
        {
            double stdDev = targetDV.stdDev;
            double X = ((LocationValue)((DetectedAttributeValue)targetDV).value).X;
            double Y = ((LocationValue)((DetectedAttributeValue)targetDV).value).Y;
            double Z = ((LocationValue)((DetectedAttributeValue)targetDV).value).Z;

            double r = random.NextDouble();// in [0,1]
            double x = ZInverse.FindZ(r);
            r = random.NextDouble();// in [0,1]
            double y = ZInverse.FindZ(r);
            r = random.NextDouble();// in [0,1]
            double z = ZInverse.FindZ(r);

         
      
            //obfuscate X
            ((LocationValue)((DetectedAttributeValue)targetDV).value).X = (double)(x * stdDev + X);
            //obfuscate Y
            ((LocationValue)((DetectedAttributeValue)targetDV).value).Y = (double)(y * stdDev + Y);
            //obfuscate Z
            ((LocationValue)((DetectedAttributeValue)targetDV).value).Z = (double)(z * stdDev + Z);
            //exit
            // No: let smaller confidence be what chooses a value in ViewPro   targetDV.stdDev = GetConfidence(Y) * 100;
        }

        private static void FuzzStringValue(ref DetectedAttributeValue targetDV, ref Random random)
        {
           // the input is the probability that a single character of the string will be transformed.
            //if transformed, it can become any character of the extended set with equal probability
            // in addition every nth (n=3) character might be preceded by a randomly
            // chosen character
            int growthFactor=3;
     string characters="abcdefghijklmnopqrstuvwxyz"
            +
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            +
            "0123456789"
            +
            "!@#$%^&*()?<>~";
            char[] cArray=characters.ToCharArray();
            double probability = targetDV.probability;
            if ((0 >= probability) || (probability > 1))
                return;// what do do about confidence here?

            string strVal = ((StringValue)((DetectedAttributeValue)targetDV).value).value;
              int strLen = strVal.Length;
            int newStrLen = strLen+ (int)((strLen + growthFactor) / growthFactor);
            
            StringBuilder newBuiltString = new StringBuilder(strLen, newStrLen);
            char[]strToChar  = strVal.ToCharArray();
            for (int i = 0; i < strLen; i++)
            {
                if (0 == i % growthFactor)
                {
                    // insert a character?
                    if (random.NextDouble() < probability) 
                        newBuiltString.Append(cArray[(int)random.Next(cArray.Length - 1)]);
                }
                if (random.NextDouble() >= probability)
                {
                    newBuiltString.Append(strToChar[i]);

                }
                else
                {
                    newBuiltString.Append(cArray[(int)random.Next(cArray.Length - 1)]);
                }
            }

            ((StringValue)((DetectedAttributeValue)targetDV).value).value = newBuiltString.ToString();
            // No: let smaller confidence be what chooses a value in ViewPro targetDV.confidence = 1 / probability;
        }
// If an attribute is fixed to integers, it is either ordinal or a count (probably)
        // Normal distribution doesn't apply
        private static void FuzzIntegerValue(ref DetectedAttributeValue targetDV, ref Random random)
        {
            double probability = targetDV.stdDev;
            int intVal = ((IntegerValue)((DetectedAttributeValue)targetDV).value).value;
 
            int newInt=intVal;
            if(probability<random.NextDouble())
            {
                newInt=random.Next(2*intVal);// 2* is "random" in the other sense
            }
            if (intVal < 0)
                newInt = -newInt;
            //obfuscate int
            ((IntegerValue)((DetectedAttributeValue)targetDV).value).value = newInt;
            //exit
         }
    }// end of class
/*************************************************************************/
 

    public static class ZTransformer
    {
        private static Dictionary<double, double> Ztable = new Dictionary<double, double>();
        public static double ZTransformGivenZ(double z)
        {
            if (Ztable.Count == 0)
                InitializeZTable();

            double roundedZ = Math.Truncate(z * 10) / 10;                 //Math.Round(z, 1);
            if (roundedZ < -3.4)
                return 0.0;
            if (roundedZ > 3.4)
                return 1.0;

            return Ztable[roundedZ];
        }
        public static double ZTransformGivenProbability(double probability)
        {
            if (Ztable.Count == 0)
                InitializeZTable();

            double lastDifference = 99.0;
            double currentDifference = 99.0;
            double lastZ = -3.4;
            double currentZ = -3.4;

            foreach (KeyValuePair<double, double> kvp in Ztable)
            {
                currentZ = kvp.Key;
                currentDifference = Math.Abs(probability - kvp.Value);
                if (currentDifference > lastDifference)
                {
                    return lastZ;
                }
                lastZ = currentZ;
                lastDifference = currentDifference;
            }
            return 3.5;
        }
        
        public static void InitializeZTable()
        {
            if (Ztable.Count > 0)
            {
                Ztable.Clear();
            }
            Ztable.Add(-3.4, 0.0003);
            Ztable.Add(-3.3, 0.0005);
            Ztable.Add(-3.2, 0.0007);
            Ztable.Add(-3.1, 0.0010);
            Ztable.Add(-3.0, 0.0014);
            Ztable.Add(-2.9, 0.0019);
            Ztable.Add(-2.8, 0.0026);
            Ztable.Add(-2.7, 0.0035);
            Ztable.Add(-2.6, 0.0047);
            Ztable.Add(-2.5, 0.0062);
            Ztable.Add(-2.4, 0.0082);
            Ztable.Add(-2.3, 0.0107);
            Ztable.Add(-2.2, 0.0139);
            Ztable.Add(-2.1, 0.0179);
            Ztable.Add(-2.0, 0.0228);
            Ztable.Add(-1.9, 0.0287);
            Ztable.Add(-1.8, 0.0359);
            Ztable.Add(-1.7, 0.0446);
            Ztable.Add(-1.6, 0.0548);
            Ztable.Add(-1.5, 0.0668);
            Ztable.Add(-1.4, 0.0808);
            Ztable.Add(-1.3, 0.0968);
            Ztable.Add(-1.2, 0.1151);
            Ztable.Add(-1.1, 0.1357);
            Ztable.Add(-1.0, 0.1587);
            Ztable.Add(-0.9, 0.1841);
            Ztable.Add(-0.8, 0.2119);
            Ztable.Add(-0.7, 0.2420);
            Ztable.Add(-0.6, 0.2743);
            Ztable.Add(-0.5, 0.3085);
            Ztable.Add(-0.4, 0.3446);
            Ztable.Add(-0.3, 0.3821);
            Ztable.Add(-0.2, 0.4207);
            Ztable.Add(-0.1, 0.4602);
            Ztable.Add(0.0, 0.5000);
            Ztable.Add(0.1, 0.5398);
            Ztable.Add(0.2, 0.5793);
            Ztable.Add(0.3, 0.6179);
            Ztable.Add(0.4, 0.6554);
            Ztable.Add(0.5, 0.6915);
            Ztable.Add(0.6, 0.7257);
            Ztable.Add(0.7, 0.7580);
            Ztable.Add(0.8, 0.7881);
            Ztable.Add(0.9, 0.8159);
            Ztable.Add(1.0, 0.8413);
            Ztable.Add(1.1, 0.8643);
            Ztable.Add(1.2, 0.8849);
            Ztable.Add(1.3, 0.9032);
            Ztable.Add(1.4, 0.9192);
            Ztable.Add(1.5, 0.9332);
            Ztable.Add(1.6, 0.9452);
            Ztable.Add(1.7, 0.9554);
            Ztable.Add(1.8, 0.9641);
            Ztable.Add(1.9, 0.9713);
            Ztable.Add(2.0, 0.9773);
            Ztable.Add(2.1, 0.9821);
            Ztable.Add(2.2, 0.9861);
            Ztable.Add(2.3, 0.9893);
            Ztable.Add(2.4, 0.9918);
            Ztable.Add(2.5, 0.9938);
            Ztable.Add(2.6, 0.9953);
            Ztable.Add(2.7, 0.9965);
            Ztable.Add(2.8, 0.9974);
            Ztable.Add(2.9, 0.9981);
            Ztable.Add(3.0, 0.9987);
            Ztable.Add(3.1, 0.9990);
            Ztable.Add(3.2, 0.9993);
            Ztable.Add(3.3, 0.9995);
            Ztable.Add(3.4, 0.9997);
        }


    }

    /* ************************************************************ */
    public static class ZInverse
    {
        public class TableEntry
        {
            public double zValue;
            public double probability;
            public TableEntry(double zValue, double probability)
            {
                this.zValue = zValue;
                this.probability = probability;
            }
        }
        /// <summary>
        /// TableEntry allows us to have the input and output for the table arranged so we can look at the values in order.
        /// </summary>
        private static List<TableEntry> invZTable = new List<TableEntry>();

        // Maps probabilities to z values
        public static void InitializeInvZTable()
        {
            if (invZTable.Count > 0)
                invZTable.Clear();
            invZTable.Add(new TableEntry(0.5000, 0.0));
            invZTable.Add(new TableEntry(0.5398, 0.1));
            invZTable.Add(new TableEntry(0.5793, 0.2));
            invZTable.Add(new TableEntry(0.6179, 0.3));
            invZTable.Add(new TableEntry(0.6554, 0.4));
            invZTable.Add(new TableEntry(0.6915, 0.5));
            invZTable.Add(new TableEntry(0.7257, 0.6));
            invZTable.Add(new TableEntry(0.7580, 0.7));
            invZTable.Add(new TableEntry(0.7881, 0.8));
            invZTable.Add(new TableEntry(0.8159, 0.9));
            invZTable.Add(new TableEntry(0.8413, 1.0));
            invZTable.Add(new TableEntry(0.8643, 1.1));
            invZTable.Add(new TableEntry(0.8849, 1.2));
            invZTable.Add(new TableEntry(0.9032, 1.3));
            invZTable.Add(new TableEntry(0.9192, 1.4));
            invZTable.Add(new TableEntry(0.9332, 1.5));
            invZTable.Add(new TableEntry(0.9452, 1.6));
            invZTable.Add(new TableEntry(0.9554, 1.7));
            invZTable.Add(new TableEntry(0.9641, 1.8));
            invZTable.Add(new TableEntry(0.9713, 1.9));
            invZTable.Add(new TableEntry(0.9773, 2.0));
            invZTable.Add(new TableEntry(0.9821, 2.1));
            invZTable.Add(new TableEntry(0.9861, 2.2));
            invZTable.Add(new TableEntry(0.9893, 2.3));
            invZTable.Add(new TableEntry(0.9918, 2.4));
            invZTable.Add(new TableEntry(0.9938, 2.5));
            invZTable.Add(new TableEntry(0.9953, 2.6));
            invZTable.Add(new TableEntry(0.9965, 2.7));
            invZTable.Add(new TableEntry(0.9974, 2.8));
            invZTable.Add(new TableEntry(0.9981, 2.9));
            invZTable.Add(new TableEntry(0.9987, 3.0));
            invZTable.Add(new TableEntry(0.9990, 3.1));
            invZTable.Add(new TableEntry(0.9993, 3.2));
            invZTable.Add(new TableEntry(0.9995, 3.3));
            invZTable.Add(new TableEntry(0.9997, 3.4));

        }
        /// <summary>
        /// Takes a probability (in [0,1]) and returns corresponding z (standard normal) value
        /// </summary>
        /// <param name="prob">in [0,1]</param>
        /// <returns></returns>
        public static double FindZ(double prob)
        {
            double returnZ;
            int returnZIndex;
            double lookUp = Math.Max(Math.Min(prob, 1.0), 0.0);
            lookUp = ZTransformer.ZTransformGivenZ(lookUp);
            Boolean lowerHalf = false;
            if (lookUp < .5)
            {
                lookUp = 1 - lookUp;
                lowerHalf = true;
            }
            returnZIndex = invZTable.Count - 1;
            for (int i = 0; i < invZTable.Count - 1; i++)
            {
                if (lookUp <= invZTable[i].probability)
                {
                    returnZIndex = i;
                    break;
                }
            }
            returnZ = invZTable[returnZIndex].zValue;
            if (lowerHalf)
                returnZ = -returnZ;
            return returnZ;
        }
    }
}
