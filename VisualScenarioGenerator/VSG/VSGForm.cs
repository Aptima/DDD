using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Win32;
using System.Diagnostics;

using VSG.Controllers;
using VSG.Dialogs;
using VSG.ViewComponentPanels;
using VSG.Adapters;
using AME;
using AME.Controllers;
using AME.Views.Forms;
using AME.Views.View_Component_Packages;
using AME.Views.View_Components;
using AME.Tools;
using AME.Adapters;

using System.Threading;
using VSG.ConfigFile;

namespace VSG
{
    public partial class VSGForm : Form, IViewComponentPanel
    {
        private ViewPanelHelper myHelper;

        public IViewComponentHelper IViewHelper
        {
            get { return myHelper; }
        }

        private RootController projectController;
        private VSGController vsgController;

        private bool _overrideCloseDialog = false;

        private ScenarioViewComponentPanel scenarioVCP;
        private ScenarioPlayfieldViewComponentPanel playfieldVCP;
        private ObjectTypesViewComponentPanel objectTypesVCP;
        private ScoringViewComponentPanel scoringVCP;
        private TimelineViewComponentPanel timelineVCP;
        private PreviewViewComponentPanel previewVCP;

        private Boolean databaseConnected = false;
        private Int32 scenarioId = 1;

        private ImportTool importTool;
        private bool importSuccess;
        private String scenarioFilename;
        private string urlToPDF = string.Format(@"{0}\Docs\{1}", Directory.GetCurrentDirectory(), "VSG_UsersGuide.pdf");
        private string urlToNoticeFile = string.Format(@"{0}\Docs\{1}", Directory.GetCurrentDirectory(), "AME_V1.0_notice_file.pdf");

        private String server;
        private String username;
        private String password;
        private String database;

        public VSGForm()
        {
            myHelper = new ViewPanelHelper(this, UpdateType.Component);

            Northwoods.Go.GoView.VersionName = "rzE62/owhCLF2aUeiLKrXu9IkaBeayX5novTMjrY6DEPgE/iMfM/uEA3UA1Wx+W+";
            Northwoods.Go.Layout.GoLayout.VersionName = "rzE62/owhCLF2aUeiLKrXinu2SJ+J8ESq5xVeNJZtNt2yjExtN/rV8jysRqYstkklTh++EOItN1YHTsC6oanuw==";

            InitializeComponent();
            UncheckToolstripItems();
        }

        public void UpdateViewComponent()
        {
            if (!this.Enabled)
            {
                this.Enabled = true;
                if (importTool.RootId != -1)
                {
                    openScenario(importTool.RootId);
                    this.Text = "Visual Scenario Generator - " + scenarioFilename;
                    this.BringToFront();
                }
                else
                    newScenario();
            }
        }

        private Boolean dbSetup()
        {
            SqlServerExpressSetupForm setup = new SqlServerExpressSetupForm();
            setup.ServerHost = System.Net.Dns.GetHostName() +  @"\SQLEXPRESS";
            setup.Username = "sa";
            setup.Database = "vsg";
            setup.StartPosition = FormStartPosition.CenterParent;
            setup.ShowDialog(this);

            // If click OK.
            if (setup.DialogResult == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;

                // Need these for the db dump
                server = setup.ServerHost;
                username = setup.Username;
                password = setup.Password;
                database = setup.Database;

                try
                {
                    AMEManager gme = AMEManager.Instance;
                    
                    AMEManager.DatabaseConfiguration dbConfiguration = new AMEManager.DatabaseConfiguration();
                    dbConfiguration.Server = setup.ServerHost;
                    dbConfiguration.Database = setup.Database;
                    dbConfiguration.Username = setup.Username;
                    dbConfiguration.Password = setup.Password;

                    gme.CreateModel(AME.AMEManager.DataModelTypes.SqlServerExpress, dbConfiguration, "vsg");
    
                    projectController = (RootController)gme.Get("Project");
                    vsgController = (VSGController)gme.Get("VSG");

                    //vsgController.ValidateAllSchemaLinks();

                    vsgController.RegisterForUpdate(this);

                    // Allow empty string as a legal value for all parameters
                    // do not return parameters that are empty string
                    projectController.IgnoreEmptyString = true;
                    vsgController.IgnoreEmptyString = true;

                    // Close any previously open projects (e.g. did someone already connect to a database?)
                    // to do...

                    // Create screens.
                    createViewComponentPanels();


                    //InitializeScenario(scenarioId);
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "DB setup failed");
                    return false;
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            else if (setup.DialogResult == DialogResult.Cancel)
            {
                Application.Exit();
                return false;
            }
            else
            {
                return false;
            }
        }

        private void openScenario(int scenID)
        {
            if (scenID < 0)
            {
                return;
            }
            UncheckToolstripItems(); // ensure scenario is checked
            toolStripButtonScenario.Checked = true;
            loadScreen(vsgController, scenarioVCP);

            // Set scenarioId in the vsgController.
            vsgController.ScenarioId = scenID;

            try
            {
                scenarioVCP.RootId = scenID;
                playfieldVCP.RootId = scenID;
                objectTypesVCP.RootId = scenID;
                scoringVCP.RootId = scenID;
                timelineVCP.RootId = scenID;
                previewVCP.RootId = scenID;

            }
            catch (Exception e)
            {
                MessageBox.Show("VSG cannot open the selected scenario.  \nSelect another scenario file or create a new Scenario. ", "Open Scenario Error");
            }
        }

        private void initializeScenario(int scenID)
        {
            if (scenID < 0)
            {
                return;
            }
            // Set scenarioId in the vsgController.
            vsgController.ScenarioId = scenID;

            try
            {
                // Create default scenario parameters.
                //projectController.UpdateParameters(scenID, "Scenario.Scenario Name", "Default Name", eParamParentType.Component);
                //projectController.UpdateParameters(scenID, "Scenario.Description", "", eParamParentType.Component);
                //projectController.UpdateParameters(scenID, "Scenario.Time To Attack", "10", eParamParentType.Component);

                scenarioVCP.RootId = scenID;
                playfieldVCP.RootId = scenID;
                objectTypesVCP.RootId = scenID;
                scoringVCP.RootId = scenID;
                timelineVCP.RootId = scenID;
                previewVCP.RootId = scenID;
            }
            catch (Exception e)
            {
                MessageBox.Show("VSG cannot initialize the selected scenario.  \nSelect another scenario file or create a new Scenario. ", "Initialize Scenario Error");
            }
        }

        private void newScenario()
        {
            // Load Scenario view
            UncheckToolstripItems();
            toolStripButtonScenario.Checked = true;
            loadScreen(vsgController, scenarioVCP);

            // Database must be connected before project can be created.
            if (databaseConnected)
            {
                // Creating a new project will initialize the connected database.
                projectController.InitializeDB();
                projectController.ClearCache();

                scenarioId = projectController.CreateRootComponent(projectController.RootComponentType, "");

                initializeScenario(scenarioId);

            }
            else
            {
                System.Windows.Forms.MessageBox.Show(this, "Database not connected!", "Create Scenario");
            }
        }

        private void createScenario()
        {
            // Load Scenario view
            UncheckToolstripItems();
            toolStripButtonScenario.Checked = true;

            vsgController.AllowNewData = false; // momentarily turn off controller
             //sending down data - bug fix for 
             //sending parameter info on leave  
             //event triggered by loadScreen
            loadScreen(vsgController, scenarioVCP);

            // Database must be connected before project can be created.
            if (databaseConnected)
            {
                // Creating a new project will initialize the connected database.
                if (System.Windows.Forms.MessageBox.Show(this, "Creating a new scenario will initialize the database. All data that has not been saved will be lost!", "Create Scenario", MessageBoxButtons.OKCancel).Equals(DialogResult.OK))
                {
                    // Create screens - reset to default states
                    createViewComponentPanels();

                    projectController.InitializeDB();
                    projectController.ClearCache();
                    vsgController.Initialize();

                    scenarioId = projectController.CreateRootComponent(projectController.RootComponentType, "");

                    initializeScenario(scenarioId);
                    loadScreen(vsgController, scenarioVCP);
                    this.Text = "Visual Scenario Generator v4.2";
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(this, "Database not connected!", "Create Scenario");
            }
            vsgController.AllowNewData = true;

        }

        private void loadScreen(Controller c, ViewComponentPanel panel)
        {
            if (panel != previewVCP)
            {
                if (previewVCP != null)
                {
                    previewVCP.SuspendScene();
                }
            }



            if (databaseConnected)
            {
                if (scenarioId >= 0)
                {
                    // We might not want to load same page.
                    if (!toolStripContainer1.ContentPanel.Controls.Contains(panel))
                    {
                        DrawingUtility.SuspendDrawing(toolStripContainer1.ContentPanel);
                        toolStripContainer1.ContentPanel.Controls.Clear();
                        toolStripContainer1.ContentPanel.Controls.Add(panel);
                        DrawingUtility.ResumeDrawing(toolStripContainer1.ContentPanel);

                        c.UpdateView();
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(this, "Scenario not created. Please create a scenario!", "View Selection");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(this, "Database not connected. Please connect to a database!", "View Selection");
            }


            if (panel == previewVCP)
            {
                if (previewVCP != null)
                {
                    previewVCP.ResumeScene();
                }
            }
        }

        private void createViewComponentPanels()
        {
            scenarioVCP = new ScenarioViewComponentPanel();
            objectTypesVCP = new ObjectTypesViewComponentPanel();
            scoringVCP = new ScoringViewComponentPanel();
            playfieldVCP = new ScenarioPlayfieldViewComponentPanel();
            timelineVCP = new TimelineViewComponentPanel();
            previewVCP = new PreviewViewComponentPanel();
        }

        private void openFile()
        {
            if (IsCurrentViewPreview())
            {
                UncheckToolstripItems(); // ensure scenario is checked
                toolStripButtonScenario.Checked = true;
                loadScreen(vsgController, scenarioVCP);
            }

            if (MessageBox.Show(this, "Opening an existing scenario will initialize the database. All data that has not been saved will be lost!", "Open Scenario", MessageBoxButtons.OKCancel).Equals(DialogResult.OK))
            {
                if (VSGConfig.OpenSaveDir == "")
                {
                    VSGConfig.OpenSaveDir = vsgController.DataPath;
                }
                openScenarioDialog.InitialDirectory = VSGConfig.OpenSaveDir;
                openScenarioDialog.FileName = String.Empty;

                DialogResult result = openScenarioDialog.ShowDialog();
                if (result.Equals(DialogResult.OK))
                {
                    VSGConfig.OpenSaveDir = Path.GetDirectoryName(openScenarioDialog.FileName);
                    VSGConfig.WriteFile();
                    createViewComponentPanels();
                    doImport(openScenarioDialog.FileName, openScenarioDialog.FilterIndex);
                    
                    
                }
            }

        }

        private void doImport(String filename, Int32 index)
        {
            switch (index)
            {
                case 1: // Scenario Files (*.xml)|*.xml
                    importTool = new ImportTool();
                    importTool.PutTypeInName = false;
                    importTool.PutNameInDescription = false;
                    // Dtermine the correct adapter to load.
                    XmlDocument document = new XmlDocument();
                    document.Load(filename);
                    AME.Adapters.IImportAdapter adapter = null;
                    String schema = document.DocumentElement.GetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                    switch (schema)
                    {
                        case "DDDSchema_4_1.xsd":
                            adapter = new Adapters.DDD4_1_Importer(vsgController);
                            break;
                        case "DDDSchema_4_1_1.xsd":
                            adapter = new Adapters.DDD4_1_Importer(vsgController);
                            break;
                        case "DDDSchema_4_0_2.xsd":
                            adapter = new Adapters.DDD4_0_2_Importer(vsgController);
                            break;
                        case "DDDSchema_4_2.xsd":
                            adapter = new Adapters.DDD4_2_Importer(vsgController);
                            break;
                        default:
                            adapter = new Adapters.DDD4_0_Importer(vsgController);
                            break;
                    }

                    //document.Schemas.
                    //Adapters.DDD4_0_Importer ddd4_0_Schema = new Adapters.DDD4_0_Importer();
 
                    importSuccess = importTool.Import(vsgController, adapter, filename, this, true);
                    if (importSuccess)
                    {
                        // Wait for import to finish.
                        // It will send a component update which will enable this back.
                        this.Enabled = false;
                        scenarioFilename = filename;
                    }
                    break;
                case 2: // Raw Scenario Files
                    //// The following commented out code is for imported dumped mysql
                    //try
                    //{
                    //    vsgController.ImportSql(filename);
                    //    ComponentOptions compOptions = new ComponentOptions();
                    //    compOptions.LevelDown = 1;
                    //    IXPathNavigable iNav = projectController.GetRootComponents(compOptions);
                    //    XPathNavigator nav = iNav.CreateNavigator();
                    //    if (nav != null)
                    //    {
                    //        XPathNavigator navScenario = nav.SelectSingleNode(String.Format("Components/Component[@Type='{0}']", projectController.RootComponentType));
                    //        if (navScenario != null)
                    //        {
                    //            String id = navScenario.GetAttribute("ID", navScenario.NamespaceURI);
                    //            openScenario(Int32.Parse(id)); // need to do this better;
                    //        }
                    //    }
                    //}
                    //catch (Exception e)
                    //{
                    //    MessageBox.Show(e.Message, "Export error");
                    //}
                    try
                    {
                        vsgController.DropDatabase();

                        SQLDMO.SQLServer2 sqlServer = new SQLDMO.SQLServer2Class();
                        sqlServer.Connect(server, username, password);
                        SQLDMO.Restore2 restore = new SQLDMO.Restore2Class();
                        restore.Devices = restore.Files;
                        restore.Files = "[" + filename + "]";
                        restore.Database = database;
                        restore.ReplaceDatabase = true;
                        restore.SQLRestore(sqlServer);
                        
                        ComponentOptions compOptions = new ComponentOptions();
                        compOptions.LevelDown = 1;
                        IXPathNavigable iNav = projectController.GetRootComponents(compOptions);
                        XPathNavigator nav = iNav.CreateNavigator();
                        if (nav != null)
                        {
                            XPathNavigator navScenario = nav.SelectSingleNode(String.Format("Components/Component[@Type='{0}']", projectController.RootComponentType));
                            if (navScenario != null)
                            {
                                String id = navScenario.GetAttribute("ID", navScenario.NamespaceURI);
                                openScenario(Int32.Parse(id)); // need to do this better;
                                scenarioFilename = filename;
                                this.Text = "Visual Scenario Generator - " + scenarioFilename;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Import error");
                    }
                    break;
            }
        }
       
        private void VSGForm_Load(object sender, EventArgs e)
        {
            SplashScreen splash = new SplashScreen(Program.GetProductVersionString());
            splash.ShowDialog();

            while ((databaseConnected = dbSetup()) == false)
            {
            }
            //openScenario(1); // just for testing.  change when start importing for real. Delete TestOpenScenario function!
            newScenario();
            this.WindowState = FormWindowState.Maximized;
            return;
        }
        
        private bool IsCurrentViewPreview()
        {
            return toolStripButtonPreview.Checked;
        }
        
        private void VSGForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_overrideCloseDialog)
            {
                if (MessageBox.Show("Are you sure you want to quit?", "Quitting the Visual Scenario Generator", MessageBoxButtons.YesNo) ==
    DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    if (previewVCP != null)
                    {
                        previewVCP.ReleaseScene();
                    }
            }
            }
            else
            {
                if (previewVCP != null)
                {
                    previewVCP.ReleaseScene();
                }
            }
        }

        private void saveFile()
        {
            if (IsCurrentViewPreview())
            {
                UncheckToolstripItems(); // ensure scenario is checked
                toolStripButtonScenario.Checked = true;
                loadScreen(vsgController, scenarioVCP);
            }

            if (VSGConfig.OpenSaveDir == "")
            {
                VSGConfig.OpenSaveDir = vsgController.DataPath;
            }
            saveScenarioDialog.InitialDirectory = VSGConfig.OpenSaveDir;
            saveScenarioDialog.FileName = String.Empty;

            DialogResult result = saveScenarioDialog.ShowDialog();
            if (result.Equals(DialogResult.OK))
            {
                VSGConfig.OpenSaveDir = Path.GetDirectoryName(saveScenarioDialog.FileName);
                VSGConfig.WriteFile();
                doExport(saveScenarioDialog.FileName, saveScenarioDialog.FilterIndex);
            }
        }

        private void doExport(String filename, Int32 index)
        {
            switch (index)
            {
                case 1: // Scenario Files (*.xml)|*.xml
                    this.Cursor = Cursors.WaitCursor;
                    FileInfo xmlFile = new FileInfo(filename);
                    if (!xmlFile.Extension.Equals(".xml", StringComparison.InvariantCultureIgnoreCase))
                    {
                        filename = xmlFile.FullName + ".xml";
                    }
                    ExportTool<IXPathNavigable> exportTool = new ExportTool<IXPathNavigable>();
                    ExportToXmlFileAdapter toXmlFileAdapter = new ExportToXmlFileAdapter(filename);
                    DDD_4_2_Exporter dddExporter = new DDD_4_2_Exporter(vsgController,
                                                                        projectController,
                                                                        true);

                    try
                    {
                        exportTool.Export(dddExporter, toXmlFileAdapter);
                        this.Text = "Visual Scenario Generator - " + filename;
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.InnerException.Message);
                    }
               
                    this.Cursor = Cursors.Default;
                    break;

                case 2: // Raw Scenario Files
                    this.Cursor = Cursors.WaitCursor;
                    //// The following commented out code is for dumping mysql
                    //FileInfo sqlFile = new FileInfo(filename);
                    //if (!sqlFile.Extension.Equals(".sql", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    filename = sqlFile.FullName + ".sql";
                    //}
                    //RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MySQL AB\MySQL Server 5.0");
                    //if (key.GetValue("Location") != null)
                    //{
                    //    String path = key.GetValue("Location").ToString();
                    //    Process process = new Process();
                    //    process.StartInfo.FileName = Path.Combine(path, @"bin\mysqldump.exe");
                    //    if (password.Equals(String.Empty))
                    //        process.StartInfo.Arguments = String.Format("-u {0} -r \"{1}\" {2}", username, filename, database);
                    //    else
                    //        process.StartInfo.Arguments = String.Format("-u {0} -p{1} -r \"{2}\" {3}", username, password, filename, database);
                    //    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    //    try
                    //    {
                    //        process.Start();
                    //        process.WaitForExit();
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        MessageBox.Show(e.Message, "Export error");
                    //    }
                    //}

                    try
                    {
                        FileInfo mdbFile = new FileInfo(filename);
                        if (!mdbFile.Extension.Equals(".bak", StringComparison.InvariantCultureIgnoreCase))
                        {
                            filename = mdbFile.FullName + ".bak";
                        }
                        SQLDMO.SQLServer2 sqlServer = new SQLDMO.SQLServer2Class();
                        sqlServer.Connect(server, username, password);
                        SQLDMO.Backup2 backup = new SQLDMO.Backup2Class();
                        backup.Devices = backup.Files;
                        backup.Files = "[" + filename + "]";
                        backup.Database = database;
                        backup.SQLBackup(sqlServer);

                        scenarioFilename = filename;
                        this.Text = "Visual Scenario Generator - " + scenarioFilename;

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Export error");
                    }
                    this.Cursor = Cursors.Default;
                    break;
            }
        }

        #region MenuAndButtonItems
        
        // New 
        private void toolStripMenuItemNew_Click(object sender, EventArgs e)
        {
            // Create new scenario.
            // Open Create Scenario dialogbox.
            createScenario();
            // Warn and delete or export current scenario database.
            // Create new scenarion component.
        }

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            // Create new scenario.
            // Open Create Scenario dialogbox.
            createScenario();
            // Warn and delete or export current scenario database.
            // Create new scenarion component.        
        }



        // Import Scenario 
        private void toolStripMenuItemImport_Click(object sender, EventArgs e)
        {
            ChangeFocus();
            openFile();
        }

        private void toolStripButtonImport_Click(object sender, EventArgs e)
        {
            ChangeFocus();
            openFile();
        }
        


        // Save Scenario
        private void toolStripMenuItemExport_Click(object sender, EventArgs e)
        {
            ChangeFocus();
            saveFile();
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            ChangeFocus();
            saveFile();
        }



        // Quit
        private void toolStripMenuItemQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        // Help Menu
        private void aboutVSG40ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeFocus();
            AboutDialog diag = new AboutDialog(urlToNoticeFile);
            diag.ShowDialog(this);
            //MessageBox.Show(String.Format("{0} Ver {1}.{2}\nCompiled on: {3}\n{4}",
            //    Program._productDisplayName, Program._productMajorVersion, Program._productMinorVersion,
            //    Program._buildDate, Program._moreInfo),
            //    String.Format("About {0} {1}.{2}", Program._productProductName,
            //    Program._productMajorVersion, Program._productMinorVersion));
        }

        private void supportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeFocus();
            MessageBox.Show(String.Format("Support e-mail address: {0}\nSupport phone number: {1}",
                Program._productEmail, Program._productSupportNumber), "Aptima support information");
        }

        private void licenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeFocus();
            string error = string.Empty;
            /*
            Program.CallKeyEntryForm(out error);
            if (error != string.Empty)
            {
                MessageBox.Show(error, "Error validating license key");
            }
             * */
        }
        
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeFocus();
            if (File.Exists(urlToPDF))
            {
                try
                {
                    Process.Start(urlToPDF);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error attempting to load User Guide");
                }
            }
            else
            {
                MessageBox.Show("User Guide not found at location: " + urlToPDF, "Error attempting to load User Guide");
            }
        }




        // **************************************
        // ** VIEWS
        // **************************************

        // Scenario View
        private void toolStripMenuItemScenario_Click(object sender, EventArgs e)
        {
            UncheckToolstripItems();
            ChangeFocus();
            toolStripButtonScenario.Checked = true;
            loadScreen(vsgController, scenarioVCP);
        }

        private void toolStripButtonScenario_Click(object sender, EventArgs e)
        {
            UncheckToolstripItems();
            ChangeFocus();
            toolStripButtonScenario.Checked = true;
            loadScreen(vsgController, scenarioVCP);
        }


        // Playfield View
        private void toolStripMenuItemPlayfield_Click(object sender, EventArgs e)
        {
            UncheckToolstripItems();
            ChangeFocus();
            toolStripButtonPlayfield.Checked = true;
            loadScreen(vsgController, playfieldVCP);
        }

        private void toolStripButtonPlayfield_Click(object sender, EventArgs e)
        {
            UncheckToolstripItems();
            ChangeFocus();
            toolStripButtonPlayfield.Checked = true;
            loadScreen(vsgController, playfieldVCP);
        }


        // Scenario Elements View
        private void toolStripMenuItemObjectTypes_Click(object sender, EventArgs e)
        {
            UncheckToolstripItems();
            ChangeFocus();
            toolStripButtonObjectTypes.Checked = true;
            loadScreen(vsgController, objectTypesVCP);
        }

        private void toolStripButtonObjectTypes_Click(object sender, EventArgs e)
        {
            UncheckToolstripItems();
            ChangeFocus();
            toolStripButtonObjectTypes.Checked = true;
            loadScreen(vsgController, objectTypesVCP);
        }


        // Scoring View
        private void toolStripMenuItemScoring_Click(object sender, EventArgs e)
        {
            UncheckToolstripItems();
            ChangeFocus();
            toolStripButtonScoring.Checked = true;
            loadScreen(vsgController, scoringVCP);
        }

        private void toolStripButtonScoring_Click(object sender, EventArgs e)
        {
            UncheckToolstripItems();
            ChangeFocus();
            toolStripButtonScoring.Checked = true;
            loadScreen(vsgController, scoringVCP);
        }


        // Scenario Director View
        private void toolStripMenuItemTimeline_Click(object sender, EventArgs e)
        {
            UncheckToolstripItems();
            ChangeFocus();
            toolStripButtonTimeline.Checked = true;
            // reset CRC for timeline so sensors show - MW
            timelineVCP.ResetCRC();
            loadScreen(vsgController, timelineVCP);
        }

        private void toolStripButtonTimeline_Click(object sender, EventArgs e)
        {
            UncheckToolstripItems();
            ChangeFocus();
            toolStripButtonTimeline.Checked = true;
            // reset CRC for timeline so sensors show - MW
            timelineVCP.ResetCRC();
            loadScreen(vsgController, timelineVCP);
        }


        // Preview View 
        private void toolStripMenuItemPreview_Click(object sender, EventArgs e)
        {
            UncheckToolstripItems();
            ChangeFocus();
            toolStripButtonPreview.Checked = true;
            loadScreen(vsgController, previewVCP);
        }

        private void toolStripButtonPreview_Click(object sender, EventArgs e)
        {
            UncheckToolstripItems();
            ChangeFocus();
            toolStripButtonPreview.Checked = true;
            loadScreen(vsgController, previewVCP);
        }



        /// <summary>
        ///  Unchecks the current  ViewToolStrip Button, necessary before changing views.
        /// </summary>
        private void UncheckToolstripItems()
        {
            foreach (ToolStripItem ctl in viewToolStrip.Items)
            {
                if (ctl is ToolStripButton)
                {
                    ToolStripButton btn = (ToolStripButton)ctl;
                    btn.Checked = false;
                }
            }
        }
        
        /// <summary>
        ///  Certain Winforms Controls may not automatically take focus, toolstip items and menus items in particular.
        ///  The AME needs "Leave" events to occur in order to know when to save changes.  The following "Hack"
        ///   insures that an arbitrary control receives focus, so the previously focus'd control can receive a "Leave"
        ///   event and write its contents to persistent storage.
        /// </summary>
        private void ChangeFocus()
        {
            this.viewToolStrip.Focus();
            this.viewToolStrip.Select();
        }


        #endregion

    }
}
