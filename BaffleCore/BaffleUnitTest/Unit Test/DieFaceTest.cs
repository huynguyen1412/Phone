using System;
using BaffleCore;
using BaffleCore.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaffleCoreTest.Unit_Test {

    [TestClass]
    public class DieFaceTest {

        [TestInitialize]
        public void Setup() {
        }
        [TestMethod]
        public void TestDieFaceConstruction() {

            DieFace die = new DieFace("Qu");
            String dieFace = die.FaceCharacter;
            Assert.AreEqual("Qu", dieFace);
        }
    }
}
