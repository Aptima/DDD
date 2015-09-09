using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeamateAdapter.DDD;
using System.Collections;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimMathTools;

namespace SeamateAdapter.ItemGenerators
{
    public class InterceptGenerator : Generator
    {
        public InterceptGenerator(DDDAdapter ddd)
            : base(ddd)
        {
            
        }

        /// <summary>
        /// Calculates an intercept course for all pirates
        /// </summary>
        /// <param name="currentItem"></param>
        /// <param name="dmID"></param>

        public override void Generate(T_Item currentItem, String dmID)
        {

            Dictionary<T_Move, T_Reveal> dict = GetActionsAsDictionary(currentItem.Action);
            //Make new dictionaries; they will be copies containing either merchants or pirates 
            Dictionary<T_Move, T_Reveal> merchantsDict = new Dictionary<T_Move,T_Reveal>();
            Dictionary<T_Move, T_Reveal> piratesDict = new Dictionary<T_Move, T_Reveal>();

            //Copy pirates and merchants into dictionaries
            foreach (T_Move key in dict.Keys)
            {
                if (GetOwner(key, dict[key]) == "Pirate DM") piratesDict.Add(key, dict[key]);
                else merchantsDict.Add(key, dict[key]); 
            }

            if (piratesDict.Keys.Count == 0) return;

            //This dictionary of pirates is the one we'll use to save intercept courses.
            Dictionary<T_Move, T_Reveal> piratesWithInterceptCourse = new Dictionary<T_Move, T_Reveal>();

            //Set each pirate on the shortest intercept course to a newly revealed or existing merchant.
            foreach (T_Move pirateMove in piratesDict.Keys) {
                Vec2D pirateLocation = new Vec2D(GetLocation(pirateMove, piratesDict[pirateMove]));

                //=========Check newly revealed merchants created in this item to find closest ===========================//
                double timeToIntercept = 1000000000000000;
                Vec2D closestInterceptPoint = null;
                T_Move closestNewMerchant = null;

                foreach (T_Move merchantMove in merchantsDict.Keys)
                {
                    double merchantSpeed = merchantMove.Throttle * GetMaxSpeed(merchantMove);
                    Vec2D merchantStart = new Vec2D(GetLocation(merchantMove, merchantsDict[merchantMove]));
                    Vec2D merchantDestination = new Vec2D((LocationValue)merchantMove.Location.Item);
                    Vec2D interceptPoint = GetInterceptPoint(merchantStart, merchantDestination, merchantSpeed, pirateLocation, GetMaxSpeed(pirateMove));
                    double merchantTimeToIntercept = merchantStart.ScalerDistanceTo(interceptPoint) / merchantSpeed;
                    if (merchantTimeToIntercept < timeToIntercept)
                    {
                        closestNewMerchant = merchantMove;
                        closestInterceptPoint = interceptPoint;
                        timeToIntercept = merchantTimeToIntercept;
                    }
                }            

                //============Check merchants already revealed, see if one is closer ========================
                //TODO: make sure any merchants we will move in this round are not being compared
                DDDAdapter.SeamateObject closestRevealedMerchant = null;
             
                foreach (DDDAdapter.SeamateObject vessel in revealedSeaVessels)
                {
                    //Compare all the existing merchants' positions to see if they are closer.

                    //if (vessel.ID == closestNewMerchant.ID) continue;

                    if (vessel.Owner == "Merchant DM")
                    {
                        double merchantSpeed = vessel.Throttle * vessel.MaximumSpeed;
                        Vec2D merchantStart = new Vec2D(vessel.Location);
                        Vec2D merchantDestination = new Vec2D(vessel.DestinationLocation);
                        Vec2D interceptPoint = GetInterceptPoint(merchantStart, merchantDestination, merchantSpeed, pirateLocation, GetMaxSpeed(pirateMove));

                        double merchantTimeToIntercept = merchantStart.ScalerDistanceTo(interceptPoint) / merchantSpeed;
                        if (merchantTimeToIntercept < timeToIntercept)
                        {
                            closestNewMerchant = null;
                            closestRevealedMerchant = vessel;
                            closestInterceptPoint = interceptPoint;
                            timeToIntercept = merchantTimeToIntercept;
                        }
                    }
                    else continue; //ignore pirates or fleet ships
                }


                if (closestInterceptPoint == null)
                {
                    return;
                }
                //Make a new move for the pirate, containing the pirate's intercept course.
                T_Move moveWithInterceptCourse = new T_Move();
                moveWithInterceptCourse.ID = pirateMove.ID;
                moveWithInterceptCourse.Throttle = 1.0;
                moveWithInterceptCourse.Location = new T_Location();
                moveWithInterceptCourse.Location.Item = closestInterceptPoint.ToLocationValue();



                
                //Set the pirate and merchant's "Intent" relating to the intercept in their SimObjects
                if (closestNewMerchant != null)
                {
                    ddd.UpdateObjectAttribute(closestNewMerchant.ID, "Intent", "Being intercepted:" + pirateMove.ID + ":" + timeToIntercept, "AGENT");   //Merchant
                    ddd.UpdateObjectAttribute(pirateMove.ID, "Intent", "Intercepting:" + closestNewMerchant.ID + ":" + timeToIntercept, "AGENT");  //Pirate
                }
                else if (closestRevealedMerchant != null)
                {
                    ddd.UpdateObjectAttribute(closestRevealedMerchant.ID, "Intent", "Being intercepted:" + pirateMove.ID + ":" + timeToIntercept, "AGENT");   //Merchant
                    ddd.UpdateObjectAttribute(pirateMove.ID, "Intent", "Intercepting:" + closestRevealedMerchant.ID + ":" + timeToIntercept, "AGENT");  //Pirate
                }
                else
                    Console.Error.WriteLine("Fix intercept generator");
                 
             
                //Add the pirate's updated move and reveal to pirate dictionary.
                piratesWithInterceptCourse[moveWithInterceptCourse] = piratesDict[pirateMove];

            }

            //Add altered pirates back to merchants, and reset the action array.
            currentItem.Action = GetActionsFromDictionary(merchantsDict.Concat(piratesWithInterceptCourse).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));

        }

        private static bool onlyMerchant(DDDAdapter.SeamateObject vessel)
        {
            return vessel.Owner == "Merchant DM";
        }
    }

}
