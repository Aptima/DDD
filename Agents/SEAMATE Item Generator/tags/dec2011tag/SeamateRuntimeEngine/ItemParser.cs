using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SeamateRuntimeEngine
{
    public class ItemParser
    {
        public static T_SeamateItems ParseItemsConfiguration(String configPath)
        {
            TextReader tr = null;

            XmlTextReader xml = null;
            XmlValidatingReader validate = null;
            xml = new XmlTextReader(configPath);
            validate = new XmlValidatingReader(xml);
            validate.ValidationEventHandler += new ValidationEventHandler(xsdValidationHandler);
            while (validate.Read()) { }
            validate.Close();

            try
            {
                tr = new StreamReader(configPath);
                XmlSerializer serializer = new XmlSerializer(typeof(T_SeamateItems));
                T_SeamateItems config = (T_SeamateItems)serializer.Deserialize(tr);
                tr.Close();
                return config;
            }
            catch (Exception ex)
            {
                if (tr != null)
                {
                    tr.Close();
                }

                throw new Exception("Unable to read configuration file: " + configPath, ex);
            }

            return null;
        }
        private static void xsdValidationHandler(Object sender, ValidationEventArgs args)
        {
            //called only on error

            throw new Exception("Error validating configuration file: " + args.Message);
        }
    }
}
