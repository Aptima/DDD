using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace CompareDDDState
{
    class Program
    {
        static void Main(string[] args)
        {
            bool findFirst = true;

            if (args.Length < 2)
            {
                Console.WriteLine(String.Format("Usage: {0} [Object State File 1] [Object State File 2] [Time to compare -- optional]",Environment.CommandLine));
                return;
            }
            string dddObjectLogPath1 = args[0];
            string dddObjectLogPath2 = args[1];

            if (!File.Exists(dddObjectLogPath1))
            {
                Console.WriteLine(String.Format("{0} doesn't exist!",dddObjectLogPath1));
                return;
            }
            if (!File.Exists(dddObjectLogPath2))
            {
                Console.WriteLine(String.Format("{0} doesn't exist!",dddObjectLogPath2));
                return;
            }

            int timeToCompare = -1;

            if (args.Length == 3)
            {
                timeToCompare = Convert.ToInt32(args[2]);
                findFirst = false;
            }

            DDDObjectLog log1 = new DDDObjectLog(dddObjectLogPath1);
            DDDObjectLog log2 = new DDDObjectLog(dddObjectLogPath2);

            if (findFirst)
            {

                int tMax1 = log1.GetMaxTime();
                int tMax2 = log2.GetMaxTime();
                timeToCompare = tMax1 >= tMax2 ? tMax1 : tMax2;
                timeToCompare = timeToCompare / 1000;
                bool foundDiffs = false;

                for (int i = 0; i < timeToCompare + 1; i++)
                {
                    foundDiffs = FindDifferences(log1, log2, i);
                    if (foundDiffs)
                    {
                        Console.WriteLine(String.Format("******First difference at time: {0}", i));
                        return;
                    }
                }
            }
            else
            {
                FindDifferences(log1, log2, timeToCompare);

            }
        }

        static bool FindDifferences(DDDObjectLog log1, DDDObjectLog log2, int timeToCompare)
        {
            Dictionary<string, Dictionary<string, string>> state1 = log1.GetStateAtTime(timeToCompare);
            Dictionary<string, Dictionary<string, string>> state2 = log2.GetStateAtTime(timeToCompare);

            List<string> objectsOnlyIn1 = new List<string>();
            List<string> objectsOnlyIn2 = new List<string>();
            List<string> objectsInBoth = new List<string>();

            bool foundDifferences = false;

            foreach (string key in state1.Keys)
            {
                if (state2.ContainsKey(key))
                {
                    objectsInBoth.Add(key);
                }
                else
                {
                    objectsOnlyIn1.Add(key);
                }
            }
            foreach (string key in state2.Keys)
            {
                if (!state1.ContainsKey(key))
                {
                    objectsOnlyIn2.Add(key);
                }
            }

            foreach (string s in objectsOnlyIn1)
            {
                foundDifferences = true;
                Console.WriteLine(String.Format("{0} missing in {1}", s, log2.FilePath));
            }
            foreach (string s in objectsOnlyIn2)
            {
                foundDifferences = true;
                Console.WriteLine(String.Format("{0} missing in {1}", s, log1.FilePath));
            }

            foreach (string obID in objectsInBoth)
            {
                List<string> attsOnlyIn1 = new List<string>();
                List<string> attsOnlyIn2 = new List<string>();
                List<string> attsInBoth = new List<string>();

                foreach (string key in state1[obID].Keys)
                {
                    if (state2[obID].ContainsKey(key))
                    {
                        attsInBoth.Add(key);
                    }
                    else
                    {
                        attsOnlyIn1.Add(key);
                    }
                }
                foreach (string key in state2[obID].Keys)
                {
                    if (!state1[obID].ContainsKey(key))
                    {
                        attsOnlyIn2.Add(key);
                    }
                }

                foreach (string s in attsOnlyIn1)
                {
                    foundDifferences = true;
                    Console.WriteLine(String.Format("{0}:{1} is missing in {2}", obID, s, log2.FilePath));
                }
                foreach (string s in attsOnlyIn2)
                {
                    foundDifferences = true;
                    Console.WriteLine(String.Format("{0}:{1} is missing in {2}", obID, s, log1.FilePath));
                }

                foreach (string att in attsInBoth)
                {
                    if (state1[obID][att] != state2[obID][att])
                    {
                        foundDifferences = true;
                        Console.WriteLine(String.Format("diff found for {0}:{1} {2} != {3}", obID, att, state1[obID][att], state2[obID][att]));
                    }
                }
            }
            return foundDifferences;
        }

    }
    
}
