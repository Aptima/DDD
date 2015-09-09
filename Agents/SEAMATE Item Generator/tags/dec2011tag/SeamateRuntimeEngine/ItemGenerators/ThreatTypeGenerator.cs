using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeamateAdapter.DDD;
using System.Collections;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

namespace SeamateRuntimeEngine.ItemGenerators
{
    public class ThreatTypeGenerator : Generator
    {
        public ThreatTypeGenerator(DDDAdapter ddd)
            : base(ddd)
        {
            
        }

        /// <summary>
        /// Calculates an intercept course for "imminent threat" pirates
        /// </summary>
        /// <param name="currentItem"></param>
        /// <param name="dmID"></param>

        public override void Generate(T_Item currentItem, String dmID)
        {
            if (currentItem.Parameters.ThreatType == T_ThreatType.Nonimminent)
                return;

            Dictionary<T_Move, T_Reveal> dict = GetActionsAsDictionary(currentItem.Action);
            //This dictionary is a copy
            Dictionary<T_Move, T_Reveal> newDict = new Dictionary<T_Move,T_Reveal>(dict);

            //Find that pirate
            T_Move move = null;
            T_Reveal reveal = null;
            foreach (T_Move key in dict.Keys)
            {
                if (dict[key] == null)
                {
                    if (ddd.GetSeamateObject(key.ID).Owner == "Pirate DM")
                    {
                        move = key;
                        reveal = dict[key];
                        newDict.Remove(key);
                        break;
                    }
                }
                else
                {
                    if (dict[key].Owner == "Pirate DM")
                    {
                        move = key;
                        reveal = dict[key];
                        newDict.Remove(key);
                        break;
                    }
                }
            }
            if (move == null) return;



            move = SetToInterceptCourse(move, reveal, newDict);


            //Reset the pirate's move and reveal in dictionary.
            newDict[move] = reveal;

            //Translate dictionary back into action array.
            currentItem.Action = GetActionsFromDictionary(newDict);
        }

        public T_Move SetToInterceptCourse(T_Move move, T_Reveal reveal, Dictionary<T_Move, T_Reveal> merchantsDict)
        {
            //Pick the closest merchant, either newly revealed or existing.
            double distance = 1000000000000000;
            T_Move closestKey = null;
            Vec2D location = new Vec2D(GetLocation(move, reveal));
            foreach (T_Move key in merchantsDict.Keys)
            {
                double thisDistance = new Vec2D(location).ScalerDistanceTo(new Vec2D(GetLocation(key,merchantsDict[key])));
                if (thisDistance < distance)
                {
                    closestKey = key;
                    distance = thisDistance;
                }
            } 

            double merchantSpeed = closestKey.Throttle*GetMaxSpeed(closestKey);
            Vec2D merchantStart = new Vec2D(GetLocation(closestKey, merchantsDict[closestKey]));
            Vec2D merchantDestination = new Vec2D((LocationValue)closestKey.Location.Item);
            Vec2D interceptPoint = GetInterceptPoint(merchantStart, merchantDestination, merchantSpeed, location, GetMaxSpeed(move));

            move.Location = new T_Location();
            move.Location.Item = interceptPoint.ToLocationValue();
            Console.WriteLine("setting pirate " + move.ID + " on intercept course to merchant " + closestKey.ID);
            return move;
        }
    }

}
