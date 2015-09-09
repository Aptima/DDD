using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SeamateAdapter.ItemGenerators
{
    class StimulusScriptGenerator
    {
        static int uid = 0;

        public void Generate(String outputFilename) {

            //minute, DM, vesselID,"stimuli,stimuli"

            //Example Data for 20 min seaside scenario 2
            //Right now I create this with "replace" in notepad.
            object[][] spreadsheet = 

new object[][] {
new object [] {1,"BAMS",2469,"Move, Reveal"}, new object [] {
1,"BAMS",7738,"Move, Reveal"}, new object [] {
1,"BAMS",2779,"Move, Reveal"}, new object [] {
1,"BAMS",1604,"Move, Reveal"}, new object [] {
1,"BAMS",2228,"Move, Reveal"}, new object [] {
1,"BAMS",6348,"Move, Reveal"}, new object [] {
1,"BAMS",2994,"Move, Reveal"}, new object [] {
1,"FireScout",7648,"Move, Reveal"}, new object [] {
1,"FireScout",1818,"Move, Reveal"}, new object [] {
1,"FireScout",4633,"Move, Reveal"}, new object [] {
1,"FireScout",5069,"Move, Reveal"}, new object [] {
1,"FireScout",3734,"Move, Reveal"}, new object [] {
1,"FireScout",2213,"Move, Reveal"}, new object [] {
1,"FireScout",4475,"Move, Reveal"}, new object [] {
1,"FireScout",4633,"Move, Reveal"}, new object [] {
2,"BAMS",8736,"Move, Reveal"}, new object [] {
2,"BAMS",1961,"Move, Reveal"}, new object [] {
2,"BAMS",5553,"Move, Reveal"}, new object [] {
2,"BAMS",5958,"Move, Reveal"}, new object [] {
2,"FireScout",8893,"Move, Reveal"}, new object [] {
3,"BAMS",2228,"Move"}, new object [] {
3,"FireScout",1848,"Move, Reveal"}, new object [] {
4,"BAMS",1135,"Reveal"}, new object [] {
4,"FireScout",3502,"Move, Reveal"}, new object [] {
5,"FireScout",7648,"Move"}, new object [] {
5,"FireScout",1818,"Move, Reveal"}, new object [] {
6,"BAMS",2377,"Move, Reveal"}, new object [] {
6,"FireScout",3593,"Move, Reveal"}, new object [] {
7,"BAMS",8948,"Move, Reveal"}, new object [] {
7,"BAMS",3106,"Move, Reveal"}, new object [] {
7,"FireScout",3593,"Move"}, new object [] {
7,"FireScout",6074,"Move, Reveal"}, new object [] {
7,"FireScout",6077,"Move, Reveal"}, new object [] {
7,"FireScout",9167,"Move, Reveal"}, new object [] {
7,"FireScout",4318,"Move, Reveal"}, new object [] {
7,"FireScout",7679,"Move, Reveal"}, new object [] {
8,"FireScout",7232,"Move, Reveal"}, new object [] {
9,"BAMS",7112,"Move, Reveal"}, new object [] {
10,"FireScout",3170,"Move, Reveal"}, new object [] {
11,"BAMS",4940,"Move, Reveal"}, new object [] {
11,"BAMS",5304,"Move, Reveal"}, new object [] {
11,"BAMS",5965,"Move, Reveal"}, new object [] {
11,"BAMS",8560,"Move, Reveal"}, new object [] {
11,"FireScout",1606,"Move, Reveal"}, new object [] {
11,"FireScout",7151,"Move, Reveal"}, new object [] {
13,"BAMS",1212,"Move, Reveal"}, new object [] {
13,"BAMS",9809,"Move, Reveal"}, new object [] {
13,"BAMS",4685,"Move, Reveal"}, new object [] {
13,"BAMS",1608,"Move, Reveal"}, new object [] {
13,"BAMS",3537,"Move, Reveal"}, new object [] {
13,"FireScout",4322,"Move, Reveal"}, new object [] {
13,"FireScout",4257,"Move, Reveal"}, new object [] {
13,"FireScout",3170,"Move, Reveal"}, new object [] {
14,"BAMS",1621,"Move, Reveal"}, new object [] {
14,"FireScout",7626,"Move, Reveal"}, new object [] {
15,"FireScout",6081,"Move, Reveal"}, new object [] {
15,"BAMS",3957,"Move, Reveal"}, new object [] {
15,"BAMS",9081,"Move, Reveal"}, new object [] {
15,"BAMS",1966,"Move, Reveal"}, new object [] {
15,"FireScout",8940,"Move, Reveal"}, new object [] {
15,"FireScout",6015,"Move, Reveal"}, new object [] {
18,"BAMS",9081,"Move"}, new object [] {
18,"BAMS",1604,"Move, Reveal"}, new object [] {
18,"BAMS",4306,"Move, Reveal"}, new object [] {
18,"FireScout",2471,"Move, Reveal"}, new object [] {
18,"FireScout",4758,"Move, Reveal"}, new object [] {
18,"FireScout",4257,"Move"}, new object [] {
19,"BAMS",2285,"Move, Reveal"}, new object [] {
19,"BAMS",9228,"Move, Reveal"}, new object [] {
19,"BAMS",6290,"Move, Reveal"}};


            List<ScheduledItem> allScheduledItems = new List<ScheduledItem>();
            List<T_Item>allItems = new List<T_Item>();
            foreach (object[] line in spreadsheet)
            {
                if (line.Length < 4)
                    continue;   //allows you to paste in and ignore empty lines (with no stimuli)
                int minute = (int)line[0];
                int time = minute *60;
                String dm_id = MakeDMID((String)line[1]);
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
                    item.Parameters.FF_Difficulty =1.0;
                    item.Parameters.TT_Difficulty = 1.0;
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
            System.IO.File.WriteAllText(@"C:\Work\SEAMATE\"+outputFilename+".xml", seamateItems.Serialize()); 
        }

        public String MakeDMID(String id)
        {
            if (id.ToLower().Contains("firescout")) return "Firescout DM";
            if (id.ToLower().Contains("bams")) return "BAMS DM";
            else return "Individual DM";
        }
    }
}
