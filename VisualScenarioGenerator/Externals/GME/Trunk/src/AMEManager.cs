using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using AME.Controllers;
using System.Web.Compilation;
using System.ComponentModel;

namespace AME
{
    public sealed partial class AMEManager
    {
        private static volatile AMEManager instance;
        private static object syncRoot = new Object();
        private static List<String> knownAssemblies = new List<String>();
        private static Dictionary<String, Type> knownTypes = new Dictionary<String, Type>();

        // indexed by model (database) name, then controller name
        private Dictionary<String, Dictionary<String, IController>> controllers = new Dictionary<String, Dictionary<String, IController>>();

        private AMEManager()
        {
            AMEManager.initLogger();
        }

        public static AMEManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AMEManager();
                    }
                }
                return instance;
            }
        }

        public static void initLogger()
        {
            //XmlConfigurator.ConfigureAndWatch(new FileInfo("Config/log4net.config"));
            //ILog logger = LogManager.GetLogger(typeof(GMEManager));
            //logger.Debug("test");
        }

        /// <summary>
        /// model name is database name
        /// </summary>
        /// <param name="type"></param>
        /// <param name="configuration"></param>
        /// <param name="modelName"></param>
        public void CreateModel(DataModelTypes type, DatabaseConfiguration configuration, String modelName)
        {
            AME.Model.Model model = null;

            switch (type)
            {
                //case DataModelTypes.MySql:
                //    model = new AME.Model.MySqlModel(configuration.Server, configuration.Port, configuration.Username, configuration.Password, configuration.Database);
                //    break;
                case DataModelTypes.SqlServerExpress:
                    model = new AME.Model.SqlServerExpressModel(configuration.Server, configuration.AuthenticationMode, configuration.Username, configuration.Password, configuration.Database);
                    model.ModelConfigurationName = modelName;
                    break;
                //case DataModelTypes.OleDb:
                //    model = new AME.Model.OleDbModel(configuration.Server, configuration.Port, configuration.Username, configuration.Password, configuration.Database);
                //    break;
            }

            if (!controllers.ContainsKey(modelName))
            {
                controllers.Add(modelName, new Dictionary<string, IController>());
            }


            CreateControllers("", modelName, model);
        }

        public void ClearControllers()
        {
            controllers.Clear();
        }
    
        private bool ControllerExists(String name)
        {
            foreach (Dictionary<String, IController> controllerList in controllers.Values)
            {
                foreach (String existingName in controllerList.Keys)
                {
                    if (name.Equals(existingName))
                    {
                        MessageBox.Show("Controller conflict.  Controller with configuration: " + name + " already exists");
                        return true;
                    }
                }
            }
            return false;
        }

        private void CreateControllers(String assemblyString, String modelName, AME.Model.Model model)
        {
            if (assemblyString != null && assemblyString != "")
            {
                if (assemblyString.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase) || 
                    assemblyString.EndsWith(".exe", StringComparison.CurrentCultureIgnoreCase))
                {
                    assemblyString = assemblyString.Substring(0, assemblyString.Length-4);
                }

                if (!knownAssemblies.Contains(assemblyString))
                {
                    knownAssemblies.Add(assemblyString);
                }
            }

            List<String> configurationNames = model.GetConfigurationNames();
            foreach (String configurationName in configurationNames)
            {
                String controllerType = model.GetConfigurationControllerName(configurationName);

                if (!ControllerExists(configurationName))
                {
                    Object obj = AMEManager.CreateObject(controllerType, "AME.Controllers.", new object[] { model, configurationName });
                    if (obj != null)
                    {
                        IController controller = (IController)obj;
                        controllers[modelName].Add(configurationName, controller);
                    }
                    else
                   { 
                        controllers.Clear();
                        controllers = null;
                        MessageBox.Show("Controllers have not been created, check configuration!", "Error");
                    }
                }
            }
        }

        public static Type GetType(String type)
        {
            return GetType(type, "");
        }

        public static Type GetType(String type, String componentType)
        {
            if (knownTypes.ContainsKey(type))
            {
                return knownTypes[type];
            }

            Type loadedType = null;
            Assembly assembly = null;

            String prefix = "Config.Parameters.";
            String componentAndParameter = prefix + componentType + "+" + type; // the "+" is needed
            String globalParameter = prefix + type;

            loadedType = Type.GetType(type); // AME or .NET type (mscorlib)
            
            if (loadedType == null)
            {
                assembly = Assembly.GetEntryAssembly(); // try application assembly; confugration.cs

                if (assembly != null) // we're in winforms
                {
                    loadedType = SearchAssemblyForType(assembly, type, componentAndParameter, globalParameter);
                }
                else // webforms will return null for entry assembly. BuildManager is a workaround and knows how to find types
                // from ASP.NET projects
                {
                    loadedType = BuildManager.GetType(type, false);

                    if (loadedType == null)
                    {
                        loadedType = BuildManager.GetType(componentAndParameter, false);

                        if (loadedType == null)
                        {
                            loadedType = BuildManager.GetType(globalParameter, false);
                        }
                    }
                }

                if (loadedType == null)
                {
                    // finally, try all of the assemblies seen inside configurations
                    foreach (String knownAssembly in knownAssemblies)
                    {
                        assembly = Assembly.Load(knownAssembly);

                        loadedType = SearchAssemblyForType(assembly, type, componentAndParameter, globalParameter);

                        if (loadedType != null)
                        {
                            break;
                        }
                    }

                    if (loadedType == null)
                    {
                        throw new Exception("Could not find type: " + type);
                    }
                }
            }

            knownTypes.Add(type, loadedType);
            return loadedType;
        }

        public static Type SearchAssemblyForType(Assembly assembly, String type, String componentAndParameter, String globalParameter)
        {
            Type returnType = null;
            returnType = assembly.GetType(type);

            if (returnType == null)
            {
                returnType = assembly.GetType(componentAndParameter);

                if (returnType == null)
                {
                    returnType = assembly.GetType(globalParameter);
                }
            }
            return returnType;
        }
        
        public static Object CreateObject(String type, Object[] args)
        {
            return CreateObject(type, "", args); // if no assembly is provided, search for the type
        }

        public static Object CreateObject(String type, String ameNamespace, Object[] args)
        {
            Object obj = null;
            Assembly assembly = null;

            try
            {
                assembly = Assembly.GetExecutingAssembly(); // Try AME assembly and namespace first
                String ameType = type;
                if (ameNamespace != null && ameNamespace != "")
                {
                    ameType = ameNamespace + type;
                }
                obj = Activator.CreateInstance(assembly.GetType(ameType), args);
            }
            catch (Exception) 
            {
                try
                {
                    assembly = Assembly.GetEntryAssembly(); // try application assembly
                    if (assembly != null) // we're in winforms
                    {
                        obj = Activator.CreateInstance(assembly.GetType(type), args);
                    }
                    else // webforms will return null for entry assembly. BuildManager is a workaround and knows how to find types
                         // from ASP.NET projects
                    {
                        obj = Activator.CreateInstance(BuildManager.GetType(type, true), args);
                    }
                }
                catch (Exception)
                {
                    // finally, try all of the assemblies seen inside configurations
                    foreach (String knownAssembly in knownAssemblies)
                    {
                        try
                        {
                            assembly = Assembly.Load(knownAssembly);
                            obj = Activator.CreateInstance(assembly.GetType(type), args);
                            if (obj != null)
                            {
                                return obj;
                            }
                        }
                        catch (Exception) { }
                    }

                    if (obj == null)
                    {
                        throw new Exception("Could not create object of type: " + type);
                    }
                }
            }
            return obj;
        }

        public IController Get(String configuration)
        {
            // return the first match
            foreach (Dictionary<String, IController> controllerList in controllers.Values)
            {
                if (controllerList.ContainsKey(configuration))
                {
                    return controllerList[configuration];
                }

            }
            MessageBox.Show("Unknown controller type.  Check configuration file", "Error");
            return null;
        }

        public class DatabaseConfiguration
        {
            private String server;

            public String Server
            {
                get { return server; }
                set { server = value; }
            }
            private String database;

            public String Database
            {
                get { return database; }
                set { database = value; }
            }
            private String username;

            public String Username
            {
                get { return username; }
                set { username = value; }
            }
            private String password;

            public String Password
            {
                get { return password; }
                set { password = value; }
            }
            private Int32 port;

            public Int32 Port
            {
                get { return port; }
                set { port = value; }
            }

            private AuthenticationMode mode;
            public AuthenticationMode AuthenticationMode
            {
                get { return mode; }
                set { mode = value; }
            }

        }
        public enum AuthenticationMode
        {
            [Description("Windows Authentication")]
            WindowsAuthentication,
            [Description("SQL Server Authentication")]
            SQLServerAuthentication,

        }
        public class EnumDescription
        {
            /// <summary>
            /// Get the description attribute for one enum value
            /// </summary>
            /// <param name="value">>Enum value
            /// <returns>The description attribute of an enum, if any</returns>
            public static string GetDescription(Enum value)
            {
                FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
            }

            /// <summary>
            /// Gets a list of key/value pairs for an enum, using the description attribute as value
            /// </summary>
            /// <param name="enumType">>typeof(your enum type)
            /// <returns>A list of KeyValuePairs with enum values and descriptions</returns>
            public static List<KeyValuePair<String, Enum>> GetValuesAndDescription(System.Type enumType)
            {
                List<KeyValuePair<String, Enum>> kvPairList = new List<KeyValuePair<String, Enum>>();

                foreach (Enum enumValue in Enum.GetValues(enumType))
                {
                    kvPairList.Add(new KeyValuePair<String, Enum>(GetDescription(enumValue), enumValue));
                }

                return kvPairList;
            }
        }

        public enum DataModelTypes
        {
            MySql,
            SqlServerExpress,
            OleDb
        }
    }
}
