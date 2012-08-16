using System;
using System.Linq;
using BaffleCore.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaffleCoreTest.Unit_Test {

    
    [TestClass]
    public class DieTest {
        readonly DieFace[] _faces = { new DieFace("A"), new DieFace("B"), new DieFace("C"), 
                            new DieFace("D"), new DieFace("E"), new DieFace("F")};

        readonly String[] _names = { "A", "B", "C", "D", "E", "F" };
        private const String NamesConcat = "ABCDEF";

        [TestInitialize]
        public void Setup() {
        }
        [TestMethod]
        public void TestDieConstruction() {
            int x=0;
            foreach (String s in _names) {
                Assert.IsTrue(s.CompareTo(_faces[x].FaceCharacter) == 0);
                ++x;
            }
        }
        [TestMethod]
        public void TestDieRoll() {
            var d = new Die(_faces);
            Die.Roll(d.ListOfFaces);

            String sum = _faces.Aggregate("", (current, s) => current + s.FaceCharacter);
            Assert.IsTrue(String.Compare(sum, NamesConcat) != 0);
        }
    }
}
