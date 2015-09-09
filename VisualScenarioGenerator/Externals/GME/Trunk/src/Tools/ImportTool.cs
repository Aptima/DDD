using System;
using System.Collections.Generic;
using System.Text;
using AME.Tools;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.IO;
using AME.Controllers;
using System.Data;
using System.Windows.Forms;
using AME.Adapters;
using AME;
using System.Threading;
using System.ComponentModel;
using AME.Controllers.Base.DataStructures;
using AME.Controllers.Base;
using AME.Views.View_Components;

namespace AME.Tools
{
    public class ImportTool : IImportTool
    {
        private XPathNavigator navDoc;

        private BackgroundWorker backgroundWorker;

        private Int32 m_rootId;
        //private Controller controller;
        //private RootController rootController;
        private Boolean putTypeInName = false;
        private Boolean putNameInDescription = false;
        private Boolean clearDB;
        private Boolean updateStatus;
        private Dictionary<String, Int32> componentHolder;
        private IViewComponent waitingComponent; 

        public static readonly String Delimitter = "_@_";

        private System.Threading.ManualResetEvent initEvent = new System.Threading.ManualResetEvent(false);
        public delegate void StepInvoker();


        public Int32 RootId
        {
            get { return m_rootId; }
        }

        public Boolean PutTypeInName
        {
            get { return putTypeInName; }
            set { putTypeInName = value; }
        }

        public Boolean PutNameInDescription
        {
            get { return putNameInDescription; }
            set { putNameInDescription = value; }
        }

        //public RootController RootController
        //{
        //    set { rootController = value; }
        //}

        //public Controller Controller
        //{
        //    set { controller = value; }
        //}

        public ImportTool()
        {
            backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
        }

        private delegate void DialogDelegate(String title, String message);

        private void CreateDialog(String title, String message)
        {
            pdialog = new ProgressDialog();
            pdialog.SetTitle(title);
            pdialog.SetMessage(message);
            pdialog.Show();
        }

        private ProgressDialog pdialog;

        // "normal" import case - clear DB, show dialog, clear cache, with the specified title and message
        public Boolean Import(IController controller, AME.Adapters.IImportAdapter adapter, String filename, Form topLevelForm, Boolean pClearDB)
        {
            IXPathNavigable iDocument = adapter.Process(filename);
            XmlDocument document = (XmlDocument)iDocument;
            return Import(controller, document, null, topLevelForm, pClearDB, true, "Import", "Importing");
        }

        // custom case - specify options
        public Boolean Import(IController validatingController, XmlDocument document, IViewComponent waitingFor, Form topLevelForm, Boolean pClearDB, Boolean pShowDialog , String title, String message)
        {
            clearDB = pClearDB;
            //document = new XmlDocument();
            //document.Load(@"C:\testFile.xml");
            if (!document.InnerXml.Equals(String.Empty) && validate(validatingController, document))
            {
                // Turn off all controller updates
                navDoc = document.CreateNavigator();
                XPathNodeIterator itConfigurations = navDoc.Select("/database/configuration");
                while (itConfigurations.MoveNext())
                {
                    String configuration = itConfigurations.Current.GetAttribute("name", itConfigurations.Current.NamespaceURI);
                    IController controller = AMEManager.Instance.Get(configuration);
                    if (controller == null)
                        return false;
                    updateStatus = controller.ViewUpdateStatus;

                    if (updateStatus)
                    {
                        controller.TurnViewUpdateOff();
                    }
                    controller.AllowProgrammaticCreation = false;
                    controller.UseDelayedValidation = true;
                    controller.CacheIndexLinkTypes();
                }

                if (pShowDialog)
                {
                    topLevelForm.Invoke(new DialogDelegate(CreateDialog), new object[] { title, message });
                }

                if (waitingFor != null)
                {
                    waitingComponent = waitingFor;
                }

                backgroundWorker.RunWorkerAsync(document.InnerXml);

                return true;
            }
            else
            {
                return false;
            }
        }

        private String getName(String name)
        {
            if (name.Contains(Delimitter))
            {
                Int32 index = name.LastIndexOf(Delimitter);
                return name.Substring(index + Delimitter.Length);
            }
            else
                return name;
        }

        private String getLink(IController controller, String name)
        {
            if (name.Contains(Delimitter))
            {
                Int32 index = name.IndexOf(Delimitter);
                String linkType = name.Substring(0, index);
                String componentId = name.Substring(index + Delimitter.Length);
                if (!componentId.Contains("-"))
                {

                    return controller.GetDynamicLinkType(linkType, componentHolder[componentId].ToString());
                }
                else
                {
                    String[] idsToParse = componentId.Split('-');

                    // see if everything we split is an ID
                    bool splitOK = true;
                    for (int i = 0; i < idsToParse.Length; i++)
                    {
                        if (!componentHolder.ContainsKey(idsToParse[i]))
                        {
                            splitOK = false;
                            break;
                        }
                    }

                    if (!splitOK)
                    {
                        // it's possible the name itself just has a hyphen in it, check that
                        if (componentHolder.ContainsKey(componentId))
                        {
                            return controller.GetDynamicLinkType(linkType, componentHolder[componentId].ToString());
                        }
                        else // the difficult case, we need to try and build IDs now from the split strings
                        {
                            List<String> idList = new List<String>();
                            idList.AddRange(idsToParse);

                            bool looking = true;
                            while (looking)
                            {
                                int listIndex = -1;
                                String thisValue = "";
                                String nextValue = "";

                                bool lookingTest = false;
                                for (int i = 0; i < idList.Count; i++)
                                {
                                    String test = idList[i];
                                    if (!componentHolder.ContainsKey(test))
                                    {
                                        lookingTest = true;
                                        // try combining this value with the next value
                                        if (i + 1 < idList.Count)
                                        {
                                            thisValue = test;
                                            nextValue = idList[i + 1];
                                            listIndex = i;
                                            break;
                                        }
                                        else // we've hit the end of the list, give up
                                        {
                                            lookingTest = false;
                                        }
                                    }
                                }

                                looking = lookingTest;

                                if (listIndex != -1)
                                {
                                    idList[listIndex] = thisValue + "-"+ nextValue;
                                    idList.RemoveAt(listIndex + 1);
                                }
                            }
                            return BuildDynamicFromSplitIDs(controller, linkType, idList.ToArray());
                        }
                    }
                    else // build the dynamic from the split IDs
                    {
                        return BuildDynamicFromSplitIDs(controller, linkType, idsToParse);
                    }
                }
            }
            else
            {
                return name;
            }
        }

        private String BuildDynamicFromSplitIDs(IController controller, String linkType, String[] idsToParse)
        {
            StringBuilder combinedID = new StringBuilder(5); // 2 hyphens, 3 dynamic IDs, can be more or less
            for (int i = 0; i < idsToParse.Length; i++)
            {
                combinedID.Append(componentHolder[idsToParse[i]]);
                if (i < idsToParse.Length - 1)
                {
                    combinedID.Append("-");
                }
            }
            return controller.GetDynamicLinkType(linkType, combinedID.ToString());
        }

        private Boolean validate(IController controller, XmlDocument document)
        {
            XmlNodeReader nodeReader = new XmlNodeReader(document);

            Boolean isValid = false;
            XmlReader schemaDatabase = controller.GetXSD("database.xsd");

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
                schemaDatabase.Close();
                nodeReader.Close();
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

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(e.Argument.ToString());
            XPathNavigator navigator = document.CreateNavigator();

            XPathNodeIterator itConfigurations = navigator.Select("/database/configuration");
            Boolean databaseInitialized = false;
            while (itConfigurations.MoveNext())
            {
                componentHolder = new Dictionary<String, Int32>();

                String configuration = itConfigurations.Current.GetAttribute("name", itConfigurations.Current.NamespaceURI);
                IController controller = AMEManager.Instance.Get(configuration);
                //Boolean isRootController = Boolean.Parse(itConfigurations.Current.GetAttribute("isRootController", itConfigurations.Current.NamespaceURI));

                if (!databaseInitialized && clearDB) 
                {
                    controller.InitializeDB();
                    databaseInitialized = true;
                    controller.ClearCache();
                    controller.CacheIndexLinkTypes();
                }

                XPathNodeIterator itComponents = itConfigurations.Current.Select("componentTable/component");
                XPathNodeIterator itComponentLinks = itConfigurations.Current.Select("linkTable/link");
                XPathNodeIterator itComponentParameters = itConfigurations.Current.Select("parameterTable/parameter");
                Int32 totalToImport = itComponents.Count + itComponentLinks.Count + itComponentParameters.Count;

                if (pdialog != null)
                {
                    pdialog.SetMinimum(0);
                    pdialog.SetMaximum(totalToImport);
                    pdialog.SetStep(1);
                }

                // Components
                //XPathNodeIterator itComponents = navigator.Select("//componentTable/component");

                List<ComponentInfo> bulkComponentInfo = new List<ComponentInfo>();
                List<LinkInfo> bulkLinkInfo = new List<LinkInfo>();
                List<ParameterInfo> bulkComponentParameterInfo = new List<ParameterInfo>();
                List<ParameterInfo> bulkLinkParameterInfo = new List<ParameterInfo>();
                List<String> componentHolderNames = new List<String>();
                List<String> linkHolderNames = new List<String>();
                Dictionary<String, String> namesWithParameters = new Dictionary<String, String>();
                String rootName = String.Empty;

                // send names with components to prevent unique clashes with default parameters
                foreach (XPathNavigator paramNav in itComponentParameters)
                {
                    String parameter_ParentId = paramNav.GetAttribute("parentId", String.Empty);
                    String parameter_Name = paramNav.GetAttribute("name", String.Empty);

                    namesWithParameters.Add(parameter_ParentId + parameter_Name, parameter_ParentId + parameter_Name);
                }

                while (itComponents.MoveNext())
                {
                    String component_Id = itComponents.Current.GetAttribute("id", String.Empty);
                    String component_Type = itComponents.Current.GetAttribute("type", String.Empty);
                    String component_Name = itComponents.Current.GetAttribute("name", String.Empty);
                    String component_Description = itComponents.Current.GetAttribute("description", String.Empty);
                    String component_EType = itComponents.Current.GetAttribute("etype", String.Empty);
                    String component_Root = itComponents.Current.GetAttribute("root", String.Empty);
                    if (String.IsNullOrEmpty(component_Root))
                        component_Root = "false";

                    Int32 cId;
                    Int32.TryParse(component_Id, out cId);
                    if (cId == 0)
                    {
                        //if (component_Type.Equals(rootController.RootComponentType))
                        if (XmlConvert.ToBoolean(component_Root))
                        {
                            //RootController rootController = controller as RootController;
                            //if (component_Type.Equals(rootController.RootComponentType))
                            //{

                            // REMOVE
                            String t = component_Type;
                            String n = putTypeInName ? component_Type : component_Name;
                            String d = putNameInDescription ? component_Name : component_Description;
                            //

                            rootName = component_Name;
                            
                            cId = controller.CreateComponent(t, getName(n), d);                            
                            componentHolder.Add(component_Name, cId);
                            if (pdialog != null)
                            {
                                backgroundWorker.ReportProgress(1, pdialog);
                            }
                            m_rootId = cId;                            

                            //    rootTypeHolder.Add(component_Name, component_Type);
                            //}
                            //else
                            //{
                            //    String t = component_Type;
                            //    String n = putTypeInName ? component_Type : component_Name;
                            //    String d = putNameInDescription ? component_Name : component_Description;

                            //    //component_ID = controller.CreateComponent(t, getName(n), d);
                            //    bulkComponentInfo.Add(new ComponentInfo(getName(n), t, d, component_EType, component_Name));
                            //    holderNames.Add(component_Name);
                            //    if (pdialog != null)
                            //    {
                            //        backgroundWorker.ReportProgress(1, pdialog);
                            //    }
                            //}
                        }
                        else
                        {
                            String t = component_Type;
                            String n = putTypeInName ? component_Type : component_Name;
                            String d = putNameInDescription ? component_Name : component_Description;

                            //component_ID = controller.CreateComponent(t, getName(n), d);
                            bulkComponentInfo.Add(new ComponentInfo(getName(n), t, d, component_EType, component_Name));
                            componentHolderNames.Add(component_Name);
                            if (pdialog != null)
                            {
                                backgroundWorker.ReportProgress(1, pdialog);
                            }
                        }
                    }
                    else
                    {
                        componentHolder.Add(component_Name, cId);
                    }
                    //componentHolder.Add(component_Name, component_ID);
                }

                List<Int32> returnIDs = controller.BulkCreateComponents(bulkComponentInfo, namesWithParameters);

                if (returnIDs.Count != componentHolderNames.Count)
                {
                     MessageBox.Show("Error during bulk creation - id mismatch");
                     cleanup();
                     return;
                }

                for (int i = 0; i < returnIDs.Count; i++)
                {
                    componentHolder.Add(componentHolderNames[i], returnIDs[i]);
                }

                Boolean clearedLinkTypes = false;
                // Component links
                //XPathNodeIterator itComponentLinks = navigator.Select("//linkTable/link");
                while (itComponentLinks.MoveNext())
                {
                    String link_Id = itComponentLinks.Current.GetAttribute("id", String.Empty);
                    String link_RootComponentId = itComponentLinks.Current.GetAttribute("rootComponentId", String.Empty);
                    String link_FromComponentId = itComponentLinks.Current.GetAttribute("fromComponentId", String.Empty);
                    String link_ToComponentId = itComponentLinks.Current.GetAttribute("toComponentId", String.Empty);
                    String link_Type = itComponentLinks.Current.GetAttribute("type", String.Empty);

                    String holderName = link_FromComponentId + link_ToComponentId + link_Type;

                    Int32 lId;
                    Int32.TryParse(link_Id, out lId);
                    if (lId == 0)
                    {
                        linkHolderNames.Add(holderName);
                        String lt = getLink(controller, link_Type);

                        bulkLinkInfo.Add(new LinkInfo(componentHolder[link_RootComponentId],
                                                      componentHolder[link_FromComponentId],
                                                      componentHolder[link_ToComponentId],
                                                      lt,
                                                      "",
                                                      holderName
                                                      ));

                        controller.ClearCache(lt);

                        if (!clearedLinkTypes)
                        {
                            clearedLinkTypes = true;
                        }

                        if (pdialog != null)
                        {
                            backgroundWorker.ReportProgress(1, pdialog);
                        }
                    }
                    else
                    {
                        componentHolder.Add(holderName, lId);
                    }
                }

                returnIDs = controller.BulkCreateLinks(bulkLinkInfo, namesWithParameters);

                if (returnIDs.Count != linkHolderNames.Count)
                {
                    MessageBox.Show("Error during bulk creation - id mismatch");
                    cleanup();
                    return;
                }

                for (int i = 0; i < returnIDs.Count; i++)
                {
                    componentHolder.Add(linkHolderNames[i], returnIDs[i]);
                }

                if (!clearedLinkTypes && itComponentParameters.Count > 0)
                {
                    // if no-one cleared (no links) and we have work to do on parameters,
                    // clear the cache
                    // because otherwise parameters that belong to components in trees won't be updated
                    controller.ClearCache();
                }

                Dictionary<String, String> existingParametersIndex = controller.GetParameterTableIndex();

                // Component Parameters
                //XPathNodeIterator itComponentParameters = navigator.Select("//parameterTable/parameter");
                StringBuilder keyBuilder;
                String key;
                while (itComponentParameters.MoveNext())
                {
                    String parameter_ParentId = itComponentParameters.Current.GetAttribute("parentId", String.Empty);
                    String parameter_Name = itComponentParameters.Current.GetAttribute("name", String.Empty);
                    String parameter_ParentType = itComponentParameters.Current.GetAttribute("parentType", String.Empty);
                    String parameter_Value = itComponentParameters.Current.GetAttribute("value", String.Empty);

                    // root components don't get created by bulk, so manually update their parameters
                    //if (isRootController)
                    //{
                    //RootController rootController = controller as RootController;
                    if (parameter_ParentId.Equals(rootName))
                    {
                        controller.UpdateParameters(componentHolder[parameter_ParentId], parameter_Name, parameter_Value, eParamParentType.Component);
                    }
                    else
                    {
                        int id = componentHolder[parameter_ParentId];

                        eParamParentType parent;
                        if (parameter_ParentType.Equals("Component"))
                        {
                            parent = eParamParentType.Component;
                        }
                        else if (parameter_ParentType.Equals("Link"))
                        {
                            parent = eParamParentType.Link;
                        }
                        else
                        {
                            parent = eParamParentType.Component;
                            MessageBox.Show("Unknown parent type: " + parameter_ParentType);
                        }

                        keyBuilder = new StringBuilder(3);
                        keyBuilder.Append(id);
                        keyBuilder.Append(parameter_ParentType);
                        keyBuilder.Append(parameter_Name);
                        key = keyBuilder.ToString();

                        if (existingParametersIndex.ContainsKey(key))
                        {
                            // delete before create
                            controller.DeleteParameter(id, parameter_ParentType, parameter_Name, false); // delete before create
                        }
                        bulkComponentParameterInfo.Add(new ParameterInfo(id, parameter_Name, parent, parameter_Value, ""));
                    }
                    //}
                    //else
                    //{
                    //    //controller.UpdateParameters(componentHolder[parameter_ParentId], parameter_Name, parameter_Value, eParamParentType.Component);
                    //    bulkParameterInfo.Add(new ParameterInfo(componentHolder[parameter_ParentId], parameter_Name, eParamParentType.Component, parameter_Value, ""));
                    //}
                    if (pdialog != null)
                    {
                        backgroundWorker.ReportProgress(1, pdialog);
                    }
                }

                controller.BulkCreateParameters(bulkComponentParameterInfo);
                controller.BulkCreateParameters(bulkLinkParameterInfo);
            }
            cleanup();
        }

        private void cleanup()
        {
            if (pdialog != null)
            {
                pdialog.SetDone();
            }
            BulkHelper.Signal();
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressDialog pdialog = e.UserState as ProgressDialog;
            pdialog.PerformStep();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (pdialog != null)
                {
                    pdialog.SetDone();
                }

                MessageBox.Show(e.Error.Message, "Import Error");

                m_rootId = -1; // Call this something defferent.

                XPathNodeIterator itConfigurations = navDoc.Select("/database/configuration");
                while (itConfigurations.MoveNext())
                {
                    String configuration = itConfigurations.Current.GetAttribute("name", itConfigurations.Current.NamespaceURI);
                    IController controller = AMEManager.Instance.Get(configuration);

                    controller.AllowProgrammaticCreation = true;
                    try
                    {
                        controller.UseDelayedValidation = false;
                    }
                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.Message, "Import Error");
                    }
                    finally
                    {
                        //rootController.AllowProgrammaticCreation = true;
                        controller.TurnViewUpdateOn(false, false);
                        //rootController.TurnViewUpdateOn(false, false);
                        controller.UpdateView();
                    }
                }
            }
            else
            {
                XPathNodeIterator itConfigurations = navDoc.Select("/database/configuration");
                while (itConfigurations.MoveNext())
                {
                    String configuration = itConfigurations.Current.GetAttribute("name", itConfigurations.Current.NamespaceURI);
                    IController controller = AMEManager.Instance.Get(configuration);

                    controller.AllowProgrammaticCreation = true;
                    //rootController.AllowProgrammaticCreation = true;
                    //pdialog.SetValue(10);

                    try
                    {
                        controller.UseDelayedValidation = false;
                    }
                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.Message, "Import Error");
                    }
                    finally
                    {
                        if (updateStatus)
                        {
                            controller.TurnViewUpdateOn();
                        }
                        else
                        {
                            if (waitingComponent != null)
                            {
                                waitingComponent.UpdateViewComponent();
                            }
                            else
                            {
                                controller.TurnViewUpdateOn();
                            }
                        }
                        
                        //pdialog.SetValue(20);
                        //rootController.TurnViewUpdateOn();
                        //pdialog.SetValue(20);
                    }
                }
            }
        }
    }
}
