using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AttackCollectionValueTester
{
    
    
    /// <summary>
    ///This is a test class for AttackCollectionValueTest and is intended
    ///to contain all AttackCollectionValueTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AttackCollectionValueTest
    {
        private static string LoadableXML = "<AttackCollectionType><AttackType><CapabilityName>Missile</CapabilityName><AttackStartTime>8</AttackStartTime><AttackTimeWindow>5</AttackTimeWindow><TargetObjectId>MyTarget</TargetObjectId><AttackingObjectId>MyAttacker</AttackingObjectId><PercentageApplied>60</PercentageApplied><IsSelfDefense>False</IsSelfDefense></AttackType><AttackType><CapabilityName>Guns</CapabilityName><AttackStartTime>10</AttackStartTime><AttackTimeWindow>5</AttackTimeWindow><TargetObjectId>MyTarget</TargetObjectId><AttackingObjectId>MyAttacker</AttackingObjectId><PercentageApplied>80</PercentageApplied><IsSelfDefense>False</IsSelfDefense></AttackType><AttackType><CapabilityName>Missile</CapabilityName><AttackStartTime>11</AttackStartTime><AttackTimeWindow>5</AttackTimeWindow><TargetObjectId>MyTarget</TargetObjectId><AttackingObjectId>MyAttacker</AttackingObjectId><PercentageApplied>40</PercentageApplied><IsSelfDefense>False</IsSelfDefense></AttackType></AttackCollectionType>";


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ToXML
        ///</summary>
        [TestMethod()]
        public void ToXMLTest()
        {
            AttackCollectionValue target = new AttackCollectionValue();
            target.FromXML(LoadableXML);
            string expected = LoadableXML;
            string actual;
            actual = target.ToXML();
            Assert.AreEqual(expected, actual);

        }


        /// <summary>
        ///A test for RemoveAttack
        ///</summary>
        [TestMethod()]
        public void RemoveAttackTest1()
        {
            AttackCollectionValue target = new AttackCollectionValue();
            target.FromXML(LoadableXML);
            AttackCollectionValue.AttackValue attack = null; 
            bool expected = true; 
            bool actual;
            int remainingCapabilityPercentage = 40;
            attack = target.GetCurrentAttacks()[1];
            actual = target.RemoveAttack(attack);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(remainingCapabilityPercentage, target.GetCurrentAttacks()[1].percentageApplied);
        }

        /// <summary>
        ///A test for RemoveAttack
        ///</summary>
        [TestMethod()]
        public void RemoveAttackTest()
        {
            AttackCollectionValue target = new AttackCollectionValue();
            target.FromXML(LoadableXML);
            string capabilityName = "Missile"; 
            string targetObjectId = "MyTarget"; 
            string attackingObjectId = "MyAttacker"; 
            int attackStartTime = 8;
            int remainingCapabilityPercentage = 40;
            bool expected = true; 
            bool actual;
            actual = target.RemoveAttack(capabilityName, targetObjectId, attackingObjectId, attackStartTime);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(remainingCapabilityPercentage, target.GetCurrentAttacks()[1].percentageApplied);
        }

        /// <summary>
        ///A test for GetCurrentPercentageApplied
        ///</summary>
        [TestMethod()]
        public void GetCurrentPercentageAppliedTest()
        {
            AttackCollectionValue target = new AttackCollectionValue(); 
            string capabilityName = "Missile";
            target.FromXML(LoadableXML);
            int expected = 100; 
            int actual;
            actual = target.GetCurrentPercentageApplied(capabilityName);
            Assert.AreEqual(expected, actual);

            capabilityName = "Guns";
            expected = 80;
            actual = target.GetCurrentPercentageApplied(capabilityName);
            Assert.AreEqual(expected, actual);

            capabilityName = "Nonexistant";
            expected = 0;
            actual = target.GetCurrentPercentageApplied(capabilityName);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for FromXML
        ///</summary>
        [TestMethod()]
        public void FromXMLTest()
        {
            AttackCollectionValue target = new AttackCollectionValue();
            bool caughtException = false;

            try
            {
                target.FromXML(LoadableXML);
            }
            catch (System.Exception ex)
            {
                caughtException = true;
            }
            Assert.IsTrue(!caughtException, "Error creating FromXML");
            Assert.IsTrue(target.GetCurrentAttacks().Count == 3, "Incorrect Amount of attacks populated");
            Assert.IsTrue(target.GetCurrentPercentageApplied("Missile") == 100, "Incorrect Percentage Applied for Missile");

            target.FromXML("");
            Assert.IsTrue(target.GetCurrentAttacks().Count == 0, "Incorrect Amount of attacks populated");
            Assert.IsTrue(target.GetCurrentPercentageApplied("Missile") == 0, "Incorrect Percentage Applied for Missile");
        }

        /// <summary>
        ///A test for AddAttack
        ///</summary>
        [TestMethod()]
        public void AddAttackTest()
        {
            AttackCollectionValue target = new AttackCollectionValue();
            AttackCollectionValue.AttackValue attack = new AttackCollectionValue.AttackValue(8, 5, "MyTarget", "MyAttacker", "Missile", 60, false); 
            AttackCollectionValue.AttackValue secondAttack = new AttackCollectionValue.AttackValue(10, 5, "MyTarget", "MyAttacker", "Guns", 80, false);
            AttackCollectionValue.AttackValue thirdAttack = new AttackCollectionValue.AttackValue(11, 5, "MyTarget", "MyAttacker", "Missile", 60, false);
            string errorMessage = string.Empty; 
            string errorMessageExpected = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = true; 
            bool actual;
            //Add Attack 1
            actual = target.AddAttack(attack, out errorMessage);
            Assert.AreEqual(errorMessageExpected, errorMessage);
            Assert.AreEqual(expected, actual);

            //Add Attack 2
            actual = target.AddAttack(secondAttack, out errorMessage);
            Assert.AreEqual(errorMessageExpected, errorMessage);
            Assert.AreEqual(expected, actual);

            //Add Attack 3
            errorMessageExpected = "Applying 40% instead of 60%, as that's all available";
            actual = target.AddAttack(thirdAttack, out errorMessage);
            Assert.AreEqual(errorMessageExpected, errorMessage);
            Assert.AreEqual(expected, actual);
           
            //Add Attack 4
            AttackCollectionValue.AttackValue fourthAttack = new AttackCollectionValue.AttackValue(12, 5, "MyTarget", "MyAttacker", "Missile", 60, false);
            expected = false;
            errorMessageExpected = "There is no more available percentage to apply for this capability.  Try again later.";
            actual = target.AddAttack(thirdAttack, out errorMessage);
            Assert.AreEqual(errorMessageExpected, errorMessage);
            Assert.AreEqual(expected, actual);
            string serialized = target.ToXML();

            Assert.AreEqual(serialized, LoadableXML);
        }

        /// <summary>
        ///A test for AttackCollectionValue Constructor
        ///</summary>
        [TestMethod()]
        public void AttackCollectionValueConstructorTest()
        {
            AttackCollectionValue target = new AttackCollectionValue();
            Assert.IsTrue(target != null,"AttackCollectionValue did not correctly construct the object");
        }
    }
}
