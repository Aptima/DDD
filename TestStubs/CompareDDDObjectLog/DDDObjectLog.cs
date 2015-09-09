using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CompareDDDState
{
    public class DDDObjectLog
    {
        string m_path = null;
        public String FilePath
        {
            get { return m_path; }
        }
        public DDDObjectLog(string path)
        {
            m_path = path;
        }

        string GetToken(char delimiter,ref string input)
        {
            int i = input.IndexOf(delimiter);

            if (i < 0)
            {
                return String.Empty;
            }

            string tok = input.Substring(0, i);
            input = input.Remove(0, i + 1);

            return tok;
        }

        public int GetMaxTime()
        {
            int t = 0;
            StreamReader file = new StreamReader(FilePath);
            while (!file.EndOfStream)
            {
                string line = file.ReadLine();
                if (line == String.Empty)
                {
                    continue;
                }

                string wallClockStr = GetToken(':', ref line);
                string timeStr = GetToken(',', ref line);
                string objectName = GetToken(',', ref line);
                string attName = GetToken(',', ref line);
                string attValue = line;

                int time = Convert.ToInt32(timeStr);
                if (time > t)
                {
                    t = time;
                }

            }
            return t;
        }

        public Dictionary<string, Dictionary<string, string>> GetStateAtTime(int t)
        {

            StreamReader file = new StreamReader(FilePath);

            Dictionary<string, Dictionary<string, string>> state = new Dictionary<string, Dictionary<string, string>>();

            while (!file.EndOfStream)
            {
                string line = file.ReadLine();
                if (line == String.Empty)
                {
                    continue;
                }

                string wallClockStr = GetToken(':', ref line);
                string timeStr = GetToken(',', ref line);
                string objectName = GetToken(',', ref line);
                string attName = GetToken(',', ref line);
                string attValue = line;

                int time = Convert.ToInt32(timeStr);

                if (time <= (t * 1000))
                {

                    if (!state.ContainsKey(objectName))
                    {
                        state[objectName] = new Dictionary<string, string>();
                    }
                    state[objectName][attName] = attValue;

                }
                else
                {
                    return state;
                }
            }


            return state;
        }
    }

    
}
