using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AME.Controllers;
using AME;
using System.Windows.Forms;

public class AMEInit
{
    /// <summary>
    /// Initializes the AME for an SQL Server EXPRESS DB
    /// Example catalog would be ".\\SQLEXPRESS" for SQLEXPRESS running on one's own machine
    /// Working directory should be a path that contains the Config directory
    /// The controller string is the name of a Controller to fetch
    /// Could also use GMEManager.Get after initializing for other controllers
    /// </summary>
    /// <param name="SQLServerCatalog"></param>
    /// <param name="Database"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static IController InitializeAMESQLServer(String model, String WorkingDirectory, String SQLServerCatalog, String Database, String Username, String Password, String Controller)
    {
        try
        {
            // paths, controller, model initialization 
            String configPath = WorkingDirectory + @"\Config";
            String docPath = WorkingDirectory + @"\Doc";
            String licPath = WorkingDirectory + @"\License";

            AMEManager.DatabaseConfiguration config = new AMEManager.DatabaseConfiguration();
            config.Server = SQLServerCatalog;   
            config.Username = Username;
            config.Password = Password;
            config.Database = Database;

            AMEManager cm = AMEManager.Instance;

            cm.CreateModel(AME.AMEManager.DataModelTypes.SqlServerExpress, config, model);
    
            return cm.Get(Controller);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error communicating with model!");
            return null;
        }
    }
}
