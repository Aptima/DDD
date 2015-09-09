using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeamateAdapter;

namespace TestProject
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ItemParserTest
    {
        public ItemParserTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestDDDConnector()
        {
            String dddHostname = "adziki";
            int dddPort = 8888;
            String dmId = "BAMS DM";
            double ffCpe = 0.25;
            double ttCpe = -0.25;

            bool res = PME_DDD_CPE_Publisher.CPEPublisher.Connect(dddHostname, dddPort);
            Assert.AreEqual(true, res);

            PME_DDD_CPE_Publisher.CPEPublisher.PublishCPE(dmId, ffCpe, ttCpe);

            PME_DDD_CPE_Publisher.CPEPublisher.Disconnect();
        
        }
        /*
        [TestMethod]
        public void TestMethod2()
        {
            string configPath = @"C:\Work\SVN\DDD\fresh phoenix\src\Agents\SEAMATE Item Generator\SeamateRuntimeEngine\SeamateRuntimeEngine\xsd\sample.xml";
            T_SeamateItems items = null;
            try
            {
                items = ItemParser.ParseItemsConfiguration(configPath);
            }
            catch (Exception ex)
            { 
                
            }
            foreach (T_Item item in items.Items)
            {
                Console.WriteLine("Item read!");
            }
        }
         */
    }
}
