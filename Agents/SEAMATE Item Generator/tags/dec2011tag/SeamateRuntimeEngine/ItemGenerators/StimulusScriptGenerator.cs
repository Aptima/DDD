using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeamateRuntimeEngine.ItemGenerators
{
    class StimulusScriptGenerator
    {
        //T_Item[] items;
        //Random random = new Random();
        int uid = 0;
        //List<T_Item> itemList = new List<T_Item>();

        public StimulusScriptGenerator()
        {
            //Make list of all items
            //items = new T_Item[72];
            //for (int i = 1; i <73; i++)
            //{
            //    T_Item item = makeItem(i);
            //    items.SetValue(item, i - 1);
            //}

        }

        public void Generate(String filename) {

            /*
            
            StimulusScriptGenerator ssg = new StimulusScriptGenerator();
            ssg.Generate("SeasideStimulusScript1");
             * */

            object[][] spreadsheet = new object[][] {
new object[] {1,"BAMS",2469,"Reveal,Move"
}, new object[] {1,"BAMS",8893,"Reveal,Move"
}, new object[] {1,"BAMS",7738,"Reveal,Move"
}, new object[] {1,"BAMS",5553,"Reveal,Move"
}, new object[] {1,"BAMS",5962,"Reveal,Move"
}, new object[] {1,"BAMS",2377,"Reveal,Move"
}, new object[] {1,"BAMS",6348,"Reveal,Move"
}, new object[] {1,"BAMS",1604,"Reveal,Move"
}, new object[] {1,"FireScout",9228,"Reveal,Move"
}, new object[] {1,"FireScout",2588,"Reveal,Move"
}, new object[] {1,"FireScout",5951,"Reveal,Move"
}, new object[] {1,"FireScout",7679,"Reveal,Move"
}, new object[] {1,"FireScout",7151,"Reveal,Move"
}, new object[] {1,"FireScout",1606,"Reveal,Move"
}, new object[] {1,"FireScout",4633,"Reveal,Move"
}, new object[] {1,"FireScout",5069,"Reveal,Move"
}, new object[] {2,"BAMS",2643,"Reveal,Move"
}, new object[] {2,"FireScout",5069,
}, new object[] {2,"FireScout",3734,"Reveal,Move"
}, new object[] {3,"BAMS",4257,"Reveal,Move"
}, new object[] {3,"BAMS",1608,"Reveal,Move"
}, new object[] {3,"BAMS",1608,
}, new object[] {3,"FireScout",1608,
}, new object[] {3,"FireScout",7679,
}, new object[] {3,"FireScout",3734,"Move"
}, new object[] {4,"BAMS",4257,
}, new object[] {4,"BAMS",8736,"Reveal,Move"
}, new object[] {4,"BAMS",1961,"Reveal,Move"
}, new object[] {4,"BAMS",4257,
}, new object[] {4,"BAMS",8736,
}, new object[] {4,"BAMS",1961,
}, new object[] {4,"BAMS",4306,"Reveal,Move"
}, new object[] {4,"FireScout",7151,"Move"
}, new object[] {4,"FireScout",3734,"Move"
}, new object[] {5,"BAMS",2285,"Reveal,Move"
}, new object[] {5,"BAMS",2643,
}, new object[] {5,"BAMS",7738,
}, new object[] {5,"FireScout",9228,
}, new object[] {5,"FireScout",3734,
}, new object[] {5,"FireScout",1608,
}, new object[] {6,"BAMS",2377,
}, new object[] {6,"BAMS",1604,"Move"
}, new object[] {6,"BAMS",4257,"Move"
}, new object[] {6,"BAMS",8736,"Move"
}, new object[] {6,"BAMS",1961,"Move"
}, new object[] {6,"BAMS",6348,
}, new object[] {6,"FireScout",9809,"Reveal,Move"
}, new object[] {6,"FireScout",9809,
}, new object[] {6,"FireScout",7648,"Reveal,Move"
}, new object[] {6,"FireScout",1818,"Reveal,Move"
}, new object[] {6,"FireScout",3502,"Reveal,Move"
}, new object[] {7,"BAMS",6348,"Move"
}, new object[] {7,"BAMS",8062,"Reveal,Move"
}, new object[] {7,"FireScout",5069,
}, new object[] {7,"FireScout",9809,
}, new object[] {7,"FireScout",7648,
}, new object[] {7,"FireScout",1818,
}, new object[] {7,"FireScout",3502,
}, new object[] {7,"FireScout",7151,
}, new object[] {8,"BAMS",2469,"Move"
}, new object[] {8,"BAMS",8893,"Move"
}, new object[] {8,"BAMS",2643,"Move"
}, new object[] {8,"BAMS",4306,
}, new object[] {8,"BAMS",8062,
}, new object[] {8,"FireScout",4306,
}, new object[] {8,"FireScout",8062,
}, new object[] {8,"BAMS",8536,"Reveal,Move"
}, new object[] {8,"FireScout",4940,"Reveal,Move"
}, new object[] {9,"BAMS",7738,"Move"
}, new object[] {9,"BAMS",1604,"Move"
}, new object[] {9,"BAMS",4475,"Reveal,Move"
}, new object[] {9,"BAMS",1135,"Reveal,Move"
}, new object[] {9,"BAMS",2213,"Reveal,Move"
}, new object[] {9,"FireScout",3734,"Move"
}, new object[] {9,"FireScout",4306,
}, new object[] {9,"FireScout",8062,
}, new object[] {9,"FireScout",3593,"Reveal,Move"
}, new object[] {9,"FireScout",6290,"Reveal,Move"
}, new object[] {10,"BAMS",1135,
}, new object[] {10,"BAMS",2469,
}, new object[] {10,"BAMS",8893,
}, new object[] {10,"FireScout",2469,
}, new object[] {10,"FireScout",8893,
}, new object[] {10,"FireScout",3734,
}, new object[] {10,"FireScout",7648,"Move"
}, new object[] {10,"FireScout",1818,"Move"
}, new object[] {10,"FireScout",3502,"Move"
}, new object[] {11,"BAMS",1604,
}, new object[] {11,"BAMS",2643,
}, new object[] {11,"BAMS",7232,"Reveal,Move"
}, new object[] {11,"BAMS",7232,
}, new object[] {11,"BAMS",8940,"Reveal,Move"
}, new object[] {11,"BAMS",8560,"Reveal,Move"
}, new object[] {11,"FireScout",1604,
}, new object[] {11,"FireScout",2643,
}, new object[] {11,"FireScout",8062,
}, new object[] {12,"BAMS",6348,"Move"
}, new object[] {12,"BAMS",7494,"Reveal,Move"
}, new object[] {12,"BAMS",2468,"Reveal,Move"
}, new object[] {12,"BAMS",7112,"Reveal,Move"
}, new object[] {12,"FireScout",8948,"Reveal,Move"
}, new object[] {12,"FireScout",1510,"Reveal,Move"
}, new object[] {12,"FireScout",6077,"Reveal,Move"
}, new object[] {13,"BAMS",1135,"Move"
}, new object[] {13,"BAMS",7112,"Move"
}, new object[] {13,"FireScout",1606,
}, new object[] {13,"FireScout",4306,"Move"
}, new object[] {13,"FireScout",9809,"Move"
}, new object[] {14,"BAMS",2643,
}, new object[] {14,"BAMS",8893,
}, new object[] {14,"BAMS",7232,"Move"
}, new object[] {14,"BAMS",2228,"Reveal,Move"
}, new object[] {14,"BAMS",2471,"Reveal,Move"
}, new object[] {14,"FireScout",4306,
}, new object[] {14,"FireScout",1604,"Move"
}, new object[] {14,"FireScout",6081,"Reveal,Move"
}, new object[] {14,"FireScout",6074,"Reveal,Move"
}, new object[] {15,"BAMS",7112,
}, new object[] {15,"FireScout",3593,
}, new object[] {15,"FireScout",4322,"Reveal,Move"
}, new object[] {16,"BAMS",2213,
}, new object[] {16,"FireScout",2213,
}, new object[] {16,"FireScout",9809,"Move"
}, new object[] {16,"FireScout",6077,"Move"
}, new object[] {17,"BAMS",4685,"Reveal,Move"
}, new object[] {17,"FireScout",6077,
}, new object[] {17,"FireScout",3170,"Reveal,Move"
}, new object[] {17,"FireScout",2643,
}, new object[] {17,"FireScout",9809,"Move"
}, new object[] {18,"BAMS",5304,"Reveal,Move"
}, new object[] {18,"BAMS",2625,"Reveal,Move"
}, new object[] {18,"BAMS",8940,"Move"
}, new object[] {18,"FireScout",4306,"Move"
}, new object[] {18,"FireScout",1848,"Reveal,Move"
}, new object[] {18,"FireScout",3170,"Move"
}, new object[] {19,"BAMS",6348,"Move"
}, new object[] {19,"BAMS",5304,
}, new object[] {19,"BAMS",2625,
}, new object[] {19,"BAMS",8940,
}, new object[] {19,"BAMS",2471,
}, new object[] {19,"BAMS",2779,"Reveal,Move"
}, new object[] {19,"BAMS",9167,"Reveal,Move"
}, new object[] {19,"BAMS",6015,"Reveal,Move"
}, new object[] {19,"FireScout",2471,
}, new object[] {19,"FireScout",6074,"Move"
}, new object[] {19,"FireScout",8948,"Move"
}, new object[] {19,"FireScout",4322,
}, new object[] {19,"FireScout",1848,"Move"
}, new object[] {19,"FireScout",1212,"Reveal,Move"
}, new object[] {19,"FireScout",4318,"Reveal,Move"
}, new object[] {20,"BAMS",9167,
}, new object[] {20,"BAMS",1212,
}, new object[] {20,"BAMS",4318,
}, new object[] {20,"FireScout",6015,
}, new object[] {20,"FireScout",8948,}
};


             
            List<ScheduledItem> allScheduledItems = new List<ScheduledItem>();
            List<T_Item>allItems = new List<T_Item>();
            foreach (object[] line in spreadsheet)
            {
                if (line.Length < 4)
                    continue;
                int minute = (int)line[0];
                int time = minute *60;
                String dm_id = (String)line[1];
                int obj_id = (int)line[2];
                String actions = (String)line[3];
                if (actions == null)
                    continue;
                ScheduledItem scheduledItem = null;
                T_Item item = null;
                //see if we've already made an "ScheduledItem" that this action belongs to
                foreach (ScheduledItem maybeScheduledItem in allScheduledItems)
                {
                    if (maybeScheduledItem.DM_ID == dm_id && maybeScheduledItem.Time == time)
                    {
                        scheduledItem = maybeScheduledItem;
                        //find corresponding abstract "Item"
                        foreach (T_Item maybeItem in allItems) {
                            if (maybeItem.ID == scheduledItem.ID) {
                                item = maybeItem;
                            }
                        }
                        break;
                    }
                }
                if (scheduledItem == null)
                    //make new "scheduledItem" and "item" (uid)
                {
                    //Dummy item that contains a UID and empty actions list
                    item = new T_Item();
                    uid = uid+1;
                    item.ID = uid.ToString();
                    item.Action = new object[0];
                    item.Parameters = new Parameters();
                    item.Parameters.ThreatType = T_ThreatType.Nonimminent;
                    item.Parameters.Threat = T_Threat.Unambiguous;
                    item.Parameters.Crossing = false;
                    item.Parameters.Groupings = T_Groupings.One;
                    item.Parameters.PlayerResources = T_ResourceAvailability.Available;
                    item.Parameters.TeammateResources = T_ResourceAvailability.Available;
                    item.Parameters.Difficulty =1.0;
                    allItems.Add(item);


                    scheduledItem = new ScheduledItem();
                    scheduledItem.Time = time;
                    scheduledItem.DM_ID = dm_id;
                    scheduledItem.ID = uid.ToString();
                    allScheduledItems.Add(scheduledItem);
                }

                //OK.  We've verified that the scheduled item exists and we have the pointer to the item.
                //Add actions to item.Action:
                List<T_ScriptedItem> listToAddToAction = new List<T_ScriptedItem>();
                if (actions.Contains("Reveal"))
                {
                    T_ScriptedItem reveal = new T_ScriptedItem();
                    reveal.ID = obj_id.ToString();
                    reveal.ActionType = "Reveal";
                    listToAddToAction.Add(reveal);
                }
                if (actions.Contains("Move"))
                {
                    T_ScriptedItem move = new T_ScriptedItem();
                    move.ID = obj_id.ToString();
                    move.ActionType = "Move";
                    listToAddToAction.Add(move);
                }
                List<object> copyOfActions = item.Action.ToList();
                copyOfActions.AddRange(listToAddToAction);
                item.Action = copyOfActions.ToArray();
            }
            T_SeamateItems seamateItems = new T_SeamateItems();
            seamateItems.Timeline = allScheduledItems.ToArray();
            seamateItems.Items = allItems.ToArray();
            System.IO.File.WriteAllText(@"C:\Work\SEAMATE\"+filename+".xml", seamateItems.Serialize()); 
        }
        /*
        public ScheduledItem[] MakeScheduledItemsForDM (String dm_id, int[] items) {
            ScheduledItem[] scheduledItems = new ScheduledItem[items.Length];
            for (int i=0;i<items.Length;i++)
            {
                ScheduledItem scheduledItem = new ScheduledItem();
                if (i== 0)
                    scheduledItem.Time = random.Next(10);
                else
                    scheduledItem.Time = i * 60 + random.Next(-10, 10);
                scheduledItem.DM_ID = dm_id;
                scheduledItem.ID = items[i].ToString();
                scheduledItems.SetValue(scheduledItem, i);
            }
            return scheduledItems;
        }



        public T_Item makeItem(int id)
        {
            T_Item item = new T_Item();
            item.ID = id.ToString();
            item.Action = new object[0];
            item.Parameters = new Parameters();

            //Treat_type
            if (id < 41)
                item.Parameters.ThreatType = T_ThreatType.Nonimminent;
            else
                item.Parameters.ThreatType = T_ThreatType.Imminent;
            //Threat character
            if (id % 8 == 1 || id % 8 == 3 || id % 8 == 5 || id % 8 == 7)
                item.Parameters.Threat = T_Threat.Unambiguous;
            else
                item.Parameters.Threat = T_Threat.Ambiguous;
            //Crossing
            if (id % 8 < 5)
                item.Parameters.Crossing = false;
            else
                item.Parameters.Crossing = true;
            //GRoupings
            if (id % 8 < 3 || id % 8 == 5 || id % 8 == 6)
                item.Parameters.Groupings = T_Groupings.One;
            else
                item.Parameters.Groupings = T_Groupings.Two;
            //Resources
            if (id < 25 || (id > 40 && id < 57))
                item.Parameters.PlayerResources = T_ResourceAvailability.Available;
            else
                item.Parameters.PlayerResources = T_ResourceAvailability.Unavailable;
            //Partner resources
            if (id % 16 < 9)
                item.Parameters.TeammateResources = T_ResourceAvailability.Unavailable;
            else
                item.Parameters.TeammateResources = T_ResourceAvailability.Available;
            //Difficulty
            item.Parameters.Difficulty = (double)id/72;


            return item;
        }
        */
    }
}
