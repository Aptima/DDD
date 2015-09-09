using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

using Aptima.Asim.DDD.CommonComponents.NetworkTools;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Aptima.Asim.DDD.CommonComponents.SimulationModelTools;

using Chilkat;

namespace DDDEmailBridge
{
    class Program
    {
        public static NetworkClient dddNetworkClient = null;
        public static Chilkat.Imap imapServer = null;

        protected static void MyExitHandler(object sender, ConsoleCancelEventArgs args)
        {
            //args.Cancel = true;
            ExitApp();
            //Environment.Exit(0);
        }
        /*
        public string makeFileNameUnique(string dir,string fileName)
        {
            if (File.Exists())
            {
                string fileNameBase;
                System.IO.Directory.
            }
            else
            {
                return fileName;
            }
        }*/
        public static string UniqueDirName(string basedir,string dir)
        {

            if (Directory.Exists(String.Format("{0}\\{1}",basedir,dir)))
            {
                int i = 1;
                while (Directory.Exists(String.Format("{0}\\{1}_{2}", basedir, dir, i)))
                {
                    i++;
                }
                return String.Format("{0}_{1}", dir, i);
                
            }
            else
            {
                return dir;
            }
        }

        private static void ExitApp()
        {
            if (dddNetworkClient != null && dddNetworkClient.IsConnected())
            {
                dddNetworkClient.Disconnect();
                Console.WriteLine("Disconnecting from DDD Server...");
            }

            if (imapServer != null && imapServer.IsConnected())
            {
                imapServer.Disconnect();
                Console.WriteLine("Disconnecting from IMAP Server...");
            }
            Console.WriteLine("Exiting...");
        }
        static void Main(string[] args)
        {
            
            /* setup the exit handler */

            Console.TreatControlCAsInput = false;
            Console.CancelKeyPress += new ConsoleCancelEventHandler(MyExitHandler);

            /* read in the config file */

            if (args.Length != 2)
            {
                Console.WriteLine(String.Format("Usage: {0} [CONFIG_FILE] [DDD_SIMULATION_MODEL]",Environment.CommandLine));
                Environment.Exit(1);
            }

            string configFileName = args[0];
            Console.WriteLine(String.Format("Reading config file: {0}",configFileName));

            string simModelFile = args[1];
            SimulationModelInfo simModel = null;
            SimulationModelReader smr = new SimulationModelReader();
            try
            {
                simModel = smr.readModel(simModelFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }

            ConfigFile config = new ConfigFile();
            try
            {
                config.readFile(configFileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }

            try
            {
                config.verifyConfig();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }

            /* connect to the DDD Server */

            dddNetworkClient = new NetworkClient();


            if (!dddNetworkClient.Connect(config.dddServerHostname, config.dddServerPortNumber))
            {
                Environment.Exit(1);
            }

            /* connect to the IMAP server */

            imapServer = new Chilkat.Imap();
            imapServer.Port = config.emailServerPortNumber;
            imapServer.Ssl = config.emailServerUseSSL;

            bool success;

            // Anything unlocks the component and begins a fully-functional 30-day trial.
            success = imapServer.UnlockComponent("SAptimaIMAPMAIL_yyq2ULZCFw4G");
            if (success != true)
            {
                Console.WriteLine(imapServer.LastErrorText);
                ExitApp();
            }
           

            /* loop reading from the IMAP Server until user cancels or the DDD disconnects us */
            int count = config.emailCheckFrequency;
            while (dddNetworkClient.IsConnected())
            {
                if (count == config.emailCheckFrequency)
                {
                    Console.WriteLine("Checking email");
                    // Connect to an IMAP server.
                    success = imapServer.Connect(config.emailServerHostname);
                    if (success != true)
                    {
                        Console.WriteLine(imapServer.LastErrorText);
                        ExitApp();
                    }

                    // Login
                    success = imapServer.Login(config.emailUsername, config.emailPassword);
                    if (success != true)
                    {
                        Console.WriteLine(imapServer.LastErrorText);
                        ExitApp();
                    }

                    // Select an IMAP mailbox
                    success = imapServer.SelectMailbox("Inbox");
                    if (success != true)
                    {
                        Console.WriteLine(imapServer.LastErrorText);
                        ExitApp();
                    }
                    Chilkat.MessageSet messageSet;
                    // We can choose to fetch UIDs or sequence numbers.
                    bool fetchUids;
                    fetchUids = true;
                    /* downloading new emails from IMAP server */

                    messageSet = imapServer.Search("NOT SEEN", fetchUids);
                    if (messageSet == null)
                    {
                        Console.WriteLine(imapServer.LastErrorText);
                        ExitApp();
                    }
                    // Fetch the emails into a bundle object:
                    Chilkat.EmailBundle bundle;
                    bundle = imapServer.FetchBundle(messageSet);
                    if (bundle == null)
                    {

                        Console.WriteLine(imapServer.LastErrorText);
                        ExitApp();
                    }

                    imapServer.Disconnect();
                    int i;

                    Chilkat.Email email;

                    for (i = 0; i <= bundle.MessageCount - 1; i++)
                    {
                        email = bundle.GetEmail(i);

                        SimulationEvent ev = SimulationEventFactory.BuildEvent(ref simModel, "ExternalEmailReceived");
                        ((StringValue)ev["FromAddress"]).value = email.FromAddress;
  
                        for (int k = 0; k < email.NumTo; k++)
                        {
                            ((StringListValue)ev["ToAddresses"]).strings.Add(email.GetToAddr(k));
                            //Console.WriteLine("To:" + email.GetToAddr(k));
                        }
                        //Console.WriteLine("Wall clock time:" + email.LocalDate.ToString());
                        ((StringValue)ev["WallClockTime"]).value = email.LocalDate.ToString();
                        //Console.WriteLine("Subject:" + email.Subject);
                        ((StringValue)ev["Subject"]).value = email.Subject;

                        //Console.WriteLine("Body:" + email.Body);
                        ((StringValue)ev["Body"]).value = email.Body;
                        //Console.WriteLine("NumAttachments:" + email.NumAttachments.ToString());
                        string attachName;
                        string attachDir;
                        string dirPath;
                        for (int j = 0; j < email.NumAttachments; j++)
                        {
                            attachName = email.GetAttachmentFilename(j);
                            //Console.WriteLine("filename=" + email.GetAttachmentFilename(j));
                            attachDir = String.Format("Attachments{0}", email.LocalDate.ToString("yyyyMMddHHmmss"));
                            
                            dirPath = String.Format("{0}\\{1}", config.attachmentBaseDirectory, UniqueDirName(config.attachmentBaseDirectory, attachDir));
                            Directory.CreateDirectory(dirPath);
                            attachName = String.Format("{0}\\{1}", dirPath, email.GetAttachmentFilename(j));
                            ((StringListValue)ev["Attachments"]).strings.Add(attachName);
                            email.SaveAttachedFile(j, dirPath);
                        }

                        dddNetworkClient.PutEvent(ev);
                        Console.WriteLine(SimulationEventFactory.XMLSerialize(ev));
                    }
                    count = 0;
                }
                
                Thread.Sleep(1000);
                count++;
            }

            ExitApp();
        }
    }
}
