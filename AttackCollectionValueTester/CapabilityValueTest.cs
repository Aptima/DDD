using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AttackCollectionValueTester
{
    
    
    /// <summary>
    ///This is a test class for CapabilityValueTest and is intended
    ///to contain all CapabilityValueTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CapabilityValueTest
    {


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
        ///A test for GetOrderedEffectsByCapability
        ///</summary>
        [TestMethod()]
        public void GetOrderedEffectsByCapabilityTest()
        {
            CapabilityValue target = new CapabilityValue(); // TODO: Initialize to an appropriate value
            CapabilityValue.Effect ef = new CapabilityValue.Effect("Capability1", 1000.0, 10, 1);
            target.effects.Add(ef);
            ef = new CapabilityValue.Effect("Capability2", 2000.0, 20, 1);
            target.effects.Add(ef);
            ef = new CapabilityValue.Effect("Capability1", 2000.0, 20, 1);
            target.effects.Add(ef);
            ef = new CapabilityValue.Effect("Capability1", 500.0, 5, 0.5);
            target.effects.Add(ef);
            ef = new CapabilityValue.Effect("Capability2", 555.0, 6, 0.6);
            target.effects.Add(ef);
            ef = new CapabilityValue.Effect("Capability1", 5000.0, 50, 1);
            target.effects.Add(ef);

            //this shows that the function returns the correctly ordered list for Capability1
            List<CapabilityValue.Effect> actual;
            actual = target.GetOrderedEffectsByCapability("Capability1");
            Assert.AreEqual(actual.Count, 4);
            Assert.AreEqual(actual[0].range, 500.0);
            Assert.AreEqual(actual[1].range, 1000.0);
            Assert.AreEqual(actual[0].intensity, 5);
            Assert.AreEqual(actual[1].intensity, 10);

            //this shows that the function returns the correctly ordered list for Capability2
            actual = target.GetOrderedEffectsByCapability("Capability2");
            Assert.AreEqual(actual.Count, 2);
            Assert.AreEqual(actual[0].range, 555.0);
            Assert.AreEqual(actual[1].range, 2000.0);
            Assert.AreEqual(actual[0].intensity, 6);
            Assert.AreEqual(actual[1].intensity, 20);

//this shows that removal from the returned list doesn't affect the structure of the internal list.
            actual.RemoveAt(1); 

            actual = target.effects;
            Assert.AreEqual(actual.Count, 6);
            Assert.AreEqual(actual[0].range, 1000.0);
            Assert.AreEqual(actual[1].range, 2000.0);
            Assert.AreEqual(actual[0].intensity, 10);
            Assert.AreEqual(actual[1].intensity, 20);
        }

        [TestMethod()]
        public void TestAttackProcessorSimAttackObjectLogic()
        { 
            //SETUP
            CapabilityValue target = new CapabilityValue(); 
            CapabilityValue.Effect ef = new CapabilityValue.Effect("Capability1", 1000.0, 10, 1);
            target.effects.Add(ef);
            ef = new CapabilityValue.Effect("Capability2", 2000.0, 20, 1);
            target.effects.Add(ef);
            ef = new CapabilityValue.Effect("Capability1", 2000.0, 20, 1);
            target.effects.Add(ef);
            ef = new CapabilityValue.Effect("Capability1", 500.0, 5, 0.5);
            target.effects.Add(ef);
            ef = new CapabilityValue.Effect("Capability2", 555.0, 6, 0.6);
            target.effects.Add(ef);
            ef = new CapabilityValue.Effect("Capability1", 5000.0, 50, 1);
            target.effects.Add(ef);

            double[] ranges = { 200.0, 800.0, 1500.0, 5000.0 };
            int[] expectedIntensities = {  5, 10, 20, 50 };

            //TEST
            for (int x = 0; x < ranges.Length; x++)//double distance in ranges)
            {
                List<CapabilityValue.Effect> capabilities = target.GetOrderedEffectsByCapability("Capability1");
                bool testComplete = false;
                foreach (CapabilityValue.Effect eff in capabilities)
                {
                    if (eff.name == "Capability1" && ranges[x] <= eff.range)
                    {
                        //int r = random.Next(0, 100);
                        //if (r <= ((int)(eff.probability * 100)))
                        //{
                        //appliedIntensity = (int)(eff.intensity * newAttack.percentageApplied / 100);
                        //newAttack.appliedIntensity = appliedIntensity;
                        //targetVul.ApplyEffect(eff.name, appliedIntensity, distance, ref random);

                        //}
                        //break outside of the if because if the probability failed, you dont want a second chance with a different range.
                        Assert.AreEqual(eff.intensity, expectedIntensities[x]);
                        testComplete = true;
                        break; //break because we only want to apply the first range found that satisfied the distance restraint
                    }
                }
                Assert.IsTrue(testComplete, System.String.Format("Incorrect value when testing for range {0}", ranges[x]));
            }
            List<CapabilityValue.Effect> capability = target.GetOrderedEffectsByCapability("Capability1");
            bool testCompleted = false;
            foreach (CapabilityValue.Effect eff in capability)
            {
                if (eff.name == "Capability1" && 6000 <= eff.range)
                {
                    //int r = random.Next(0, 100);
                    //if (r <= ((int)(eff.probability * 100)))
                    //{
                    //appliedIntensity = (int)(eff.intensity * newAttack.percentageApplied / 100);
                    //newAttack.appliedIntensity = appliedIntensity;
                    //targetVul.ApplyEffect(eff.name, appliedIntensity, distance, ref random);

                    //}
                    //break outside of the if because if the probability failed, you dont want a second chance with a different range.
                    
                    testCompleted = true;
                    break; //break because we only want to apply the first range found that satisfied the distance restraint
                }
            }
            Assert.IsFalse(testCompleted, System.String.Format("Incorrect value when testing for range {0}", 6000));
        }
    }
}
