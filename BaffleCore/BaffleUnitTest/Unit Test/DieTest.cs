using System;
using BaffleCore;
using BaffleCore.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaffleCoreTest.Unit_Test {

    
    [TestClass]
    public class DieTest {
        DieFace[] faces = { new DieFace("A"), new DieFace("B"), new DieFace("C"), 
                            new DieFace("D"), new DieFace("E"), new DieFace("F")};

        String[] names = { "A", "B", "C", "D", "E", "F" };
        String names_concat = "ABCDEF";

        [TestInitialize]
        public void Setup() {
        }
        [TestMethod]
        public void TestDieConstruction() {
            Die d = new Die(faces);

            int x=0;
            foreach (String s in names) {
                Assert.IsTrue(s.CompareTo(faces[x].FaceCharacter) == 0);
                ++x;
            }
        }
        [TestMethod]
        public void TestDieRoll() {
            Die d = new Die(faces);
            d.Roll();

            String sum = "";
            foreach (DieFace s in faces) {
                sum += s.FaceCharacter;
            }
            Assert.IsTrue(sum.CompareTo(names_concat) != 0);
        }
    }
}
