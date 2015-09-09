using System;
using System.Collections.Generic;
using System.Text;
using GME.Adapters;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Xml.Xsl;
using System.Windows.Forms;
using Saxon.Api;
using System.IO;

namespace VSG.Adapters
{
    class DDD_4_0_Importer : IImportAdapter
    {
        private String path;
        private Int64 NUM_LANDREGIONS;
        private Int64 NUM_VERTICES;
        private Int64 NUM_ACTIVEREGIONS;
        private Int64 NUM_TEAMS;
        private Int64 NUM_DECISIONMAKERS;
        private Int64 NUM_NETWORKS;
        private Int64 NUM_SENSORS;
        private Int64 NUM_CONES;
        private Int64 NUM_DIRECTIONS;
        private Int64 NUM_GENERA;
        private Int64 NUM_SPECIES;
        private Int64 NUM_FULLYFUNCTIONALS;
        private Int64 NUM_STATEPARAMETERS;
        private Int64 NUM_CAPABILITIES;
        private Int64 NUM_SINGLETONVULNERABILITIES;
        private Int64 NUM_PROXIMITIES;
        private Int64 NUM_EFFECTS;
        private Int64 NUM_TRANSITIONS;
        private Int64 NUM_COMBOVULNERABILITIES;
        private Int64 NUM_CONTRIBUTIONS;
        private Int64 NUM_EMITTERS;
        private Int64 NUM_NORMALEMITTERS;
        private Int64 NUM_DEFINESTATES;
        private Int64 NUM_OPENCHATROOMS;
        private Int64 NUM_CLOSECHATROOMS;
        private Int64 NUM_DEFINEENGRAMS;
        private Int64 NUM_CHANGEENGRAMS;
        private Int64 NUM_REMOVEENGRAMS;
        private Int64 NUM_CREATEEVENTS;
        private Int64 NUM_SUBPLATFORMS;
        private Int64 NUM_ARMAMENTS;
        private Int64 NUM_DOCKS;
        private Int64 NUM_LAUNCHES;
        private Int64 NUM_LOCATIONS;
        private Int64 NUM_INITIALPARAMETERS;
        private Int64 NUM_ADOPTPLATFORMS;
        private Int64 NUM_REVEALEVENTS;
        private Int64 NUM_INITIALLOCATIONS;
        private Int64 NUM_STARTUPPARAMETERS;
        private Int64 NUM_ENGRAMRANGES;
        private Int64 NUM_MOVEEVENTS;
        private Int64 NUM_STATECHANGEEVENTS;
        private Int64 NUM_TRANSFEREVENTS;
        private Int64 NUM_LAUNCHEVENTS;
        private Int64 NUM_FLUSHEVENTS;
        private Int64 NUM_REITERATES;
        private Int64 NUM_REITERATETHESE;
        private Int64 NUM_RULES;
        private Int64 NUM_UNITS;
        private Int64 NUM_REGIONS;
        private Int64 NUM_OBJECTS;
        private Int64 NUM_SCORES;
        private Int64 NUM_COMPLETIONEVENTS;
        private Int64 NUM_DOTHESE;
        private Int64 NUM_SPECIESCOMPLETIONEVENTS;
        private Int64 NUM_DESTINATIONS;
        private Int64 NUM_RELATIVELOCATIONS;
        private Int64 NUM_ANEFFECTS;
        private Int64 NUM_RANGES;
        private Int64 NUM_PROBABILITIES;
        private Int64 NUM_STATES;
        private Int64 NUM_LEVELS;
        private Int64 NUM_VARIANCES;
        private Int64 NUM_PERCENTS;
        private Int64 NUM_PARAMETERS;
        private Int64 NUM_SETTINGS;

        #region IImportAdapter Members

        public string ConfigurationPath
        {
            set
            {
                path = value;
            }
            get
            {
                return path;
            }
        }

        public IXPathNavigable Process(string uriSource)
        {
            //XmlDocument document = new XmlDocument();
            //XPathNavigator navigator;
            //if (validate(uriSource, out navigator))
            //{
            //    // Build the xml file
            //    XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
            //    document.AppendChild(declaration);
            //    XmlElement root = document.CreateElement("database");

            //    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(document.NameTable);
            //    namespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            //    // Add schema information to root.
            //    XmlAttribute schema = document.CreateAttribute("xsi", "noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            //    schema.Value = Path.Combine(path, @"database.xsd"); ;
            //    root.SetAttributeNode(schema);

            //    XmlElement componentTable = document.CreateElement("componentTable");
            //    root.AppendChild(componentTable);
            //    XmlElement linkTable = document.CreateElement("linkTable");
            //    root.AppendChild(linkTable);
            //    XmlElement parameterTable = document.CreateElement("parameterTable");
            //    root.AppendChild(parameterTable);

            //    document.AppendChild(root);

                String xslt = path + @"\database.xslt";
                // Create a Processor instance.
                Processor processor = new Processor();

                // Load the source document
                XdmNode input = processor.NewDocumentBuilder().Build(new Uri(uriSource));

                // Create a transformer for the stylesheet.
                XsltTransformer transformer = processor.NewXsltCompiler().Compile(new Uri(xslt)).Load();

                // Set the root node of the source document to be the initial context node
                transformer.InitialContextNode = input;

                // Create a serializer
                Serializer serializer = new Serializer();
                MemoryStream memory = new MemoryStream();
                //serializer.SetOutputStream(new FileStream("sample.out", FileMode.Create, FileAccess.Write));
                serializer.SetOutputStream(memory);
                // Transform the source XML to System.out.
                transformer.Run(serializer);

                XmlDocument xmlMain = new XmlDocument();
                memory.Position = 0;
                xmlMain.Load(memory);
                memory.Dispose();

            //}

            return xmlMain;
        }

        #endregion

        private Boolean validate(String uri, out XPathNavigator nav)
        {
            XmlDocument document = new XmlDocument();
            document.Load(uri);

            nav = document.CreateNavigator();

            XmlNodeReader nodeReader = new XmlNodeReader(document);

            Boolean isValid = false;
            String schemaDatabase = Path.Combine(path, @"DDDSchema_4_0.xsd");

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(null, schemaDatabase);
            //settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            //settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(validationEventHandler);

            XmlReader reader = XmlReader.Create(nodeReader, settings);

            try
            {
                while (reader.Read()) ;
                isValid = true;
            }
            catch (System.Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Validation Error");
            }
            finally
            {
                reader.Close();
            }
            return isValid;
        }

        private void validationEventHandler(object sender, ValidationEventArgs args)
        {
            String message = String.Empty;

            switch (args.Severity)
            {
                case XmlSeverityType.Error:
                    message = "Import Error: " + args.Message;
                    break;
                case XmlSeverityType.Warning:
                    message = "Import Warning: " + args.Message;
                    break;
            }

            throw new System.Xml.Schema.XmlSchemaValidationException(args.Message);
        }

        private XmlElement getGrouping(XmlDocument document)
        {
            XmlElement section = document.CreateElement("grouping");

            XmlElement componentTable = document.CreateElement("componentTable");
            section.AppendChild(componentTable);
            XmlElement linkTable = document.CreateElement("linkTable");
            section.AppendChild(linkTable);
            XmlElement parameterTable = document.CreateElement("parameterTable");
            section.AppendChild(parameterTable);

            return section;
        }

    }
}
