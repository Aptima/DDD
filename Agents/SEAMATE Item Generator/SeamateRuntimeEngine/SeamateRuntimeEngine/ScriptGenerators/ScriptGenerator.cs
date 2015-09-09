using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeamateAdapter.ItemGenerators
{
    class ScriptGenerator
    {
        T_Item[] items;
        Random random = new Random();

        public ScriptGenerator()
        {
            //Make list of all items
            items = new T_Item[72];
            for (int i = 1; i <73; i++)
            {
                T_Item item = makeItem(i);
                items.SetValue(item, i - 1);
            }

        }

        public void Generate(Dictionary<String, int[]> dmAndItems, String filename) {

            /*
            
            ScriptGenerator sg = new ScriptGenerator();
            int[] bamsItems = new int[] { 9, 26, 33, 23, 29, 10, 37, 65, 16, 39, 32, 4, 31, 21, 53, 30, 59, 8, 17, 38 };
            int[] fireScoutItems = new int[] { 45, 19, 35, 58, 17, 46, 68, 71, 11, 37, 7, 43, 14, 57, 61, 20, 55, 12, 64, 66 };
            Dictionary<String,int[]> dict = new Dictionary<String, int[]>();
            dict.Add("BAMS DM", bamsItems);
            dict.Add("Firescout DM", fireScoutItems);
            sg.Generate(dict, "CalibrationScenario2");
             * */

             
            List<ScheduledItem> allScheduledItems = new List<ScheduledItem>();
            foreach (String dm_id in dmAndItems.Keys)
            {
                ScheduledItem[] scheduledItems = MakeScheduledItemsForDM(dm_id, dmAndItems[dm_id]);
                allScheduledItems.AddRange(scheduledItems);
            }
            T_SeamateItems seamateItems = new T_SeamateItems();
            seamateItems.Timeline = allScheduledItems.ToArray();
            seamateItems.Items = items;
            System.IO.File.WriteAllText(@"C:\Work\SEAMATE\"+filename+".xml", seamateItems.Serialize()); 
        }

        public ScheduledItem[] MakeScheduledItemsForDM (String dm_id, int[] items) {
            ScheduledItem[] scheduledItems = new ScheduledItem[items.Length];
            for (int i=0;i<items.Length;i++)
            {
                ScheduledItem scheduledItem = new ScheduledItem();   
                scheduledItem.Time = i * 60; //makes items at exactly every 60s now -Lisa 12/20/11
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
            item.Parameters.TT_Difficulty = (double)id/72;
            item.Parameters.FF_Difficulty = (double)id / 72;

            return item;
        }
        
    }
}
