using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace VSG.Helpers
{
    public class ClassificationDisplayRules
    {
        public List<ClassificationDisplayRule> Rules = new List<ClassificationDisplayRule>();

        public String ToXML()
        {
            StringBuilder sb = new StringBuilder("<ClassificationDisplayRules>");

            foreach (ClassificationDisplayRule r in Rules)
            {
                sb.Append(r.ToXML());
            }

            sb.Append("</ClassificationDisplayRules>");
            return sb.ToString();
        }
        private static Regex regex = new Regex(@"^<ClassificationDisplayRules>(<ClassificationDisplayRule>.*</ClassificationDisplayRule>)*</ClassificationDisplayRules>$");
        private static Regex innerRegex = new Regex(@"<ClassificationDisplayRule><State>(.*?)</State><Classification>(.*?)</Classification><DisplayIcon>(.*?)</DisplayIcon></ClassificationDisplayRule>");
        public static List<ClassificationDisplayRule> FromXML(String serializedString)
        {
            List<ClassificationDisplayRule> rules = new List<ClassificationDisplayRule>();

            Match m = regex.Match(serializedString);
            if (m.Success)
            {
                string s = m.Groups[1].Value;
                foreach (Match m2 in innerRegex.Matches(s))
                {
                    try
                    {
                        rules.Add(ClassificationDisplayRule.FromXML(m2.Value));
                    }
                    catch (Exception ex)
                    { }
                }
            }
            else
            {
            }

            return rules;
        }
    }
    public class ClassificationDisplayRule
    {
        public String StateName = "";
        public String Classification = "";
        public String IconName = "";

        public ClassificationDisplayRule()
        { }
        public ClassificationDisplayRule(String state, String classification, String icon)
        {
            StateName = state;
            Classification = classification;
            IconName = icon;
        }

        public String ToXML()
        {
            return String.Format("<ClassificationDisplayRule><State>{0}</State><Classification>{1}</Classification><DisplayIcon>{2}</DisplayIcon></ClassificationDisplayRule>", StateName, Classification, IconName);
        }

        private static Regex regex = new Regex(@"^<ClassificationDisplayRule><State>(.*?)</State><Classification>(.*?)</Classification><DisplayIcon>(.*?)</DisplayIcon></ClassificationDisplayRule>$");
        public static ClassificationDisplayRule FromXML(String serializedString)
        {
            ClassificationDisplayRule rule = new ClassificationDisplayRule();
            Match m = regex.Match(serializedString);
            if (m.Success)
            {
                rule.StateName = m.Groups[1].Value;
                rule.Classification = m.Groups[2].Value;
                rule.IconName = m.Groups[3].Value;
            }
            else
            {
               // throw new Exception(String.Format("Invalid XML string in {0}: {1}", dataType, xml));
            }

            return rule;
        }
    }
}
