using System;
using System.Linq;
using BaffleCore.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BaffleCoreTest.Unit_Test {

    [TestClass]
    public class DiceTest {
        readonly DieFace[] _faceA = { new DieFace("A"), new DieFace("B"), new DieFace("C"), 
                            new DieFace("D"), new DieFace("E"), new DieFace("F")};

        readonly DieFace[] _faceB = { new DieFace("A"), new DieFace("B"), new DieFace("C"), 
                            new DieFace("D"), new DieFace("E"), new DieFace("F")};

        readonly DieFace[] _faceC = { new DieFace("A"), new DieFace("B"), new DieFace("C"), 
                            new DieFace("D"), new DieFace("E"), new DieFace("F")};

        String[] _names = { "A", "B", "C", "D", "E", "F" };
        private const String NamesConcat = "ABCDEF";

        [TestInitialize]
        public void Setup() {
        }
        [TestMethod]
        public void TestDiceConstruction() {
            Die[] die = { new Die(_faceA) };
            var dice = new Dice(die);
            Assert.IsTrue(dice.ListOfDie.Count == 1);
            Assert.IsTrue(dice.ListOfDie[0] == die[0]);
        }
        [TestMethod]
        public void TestDiceRoll() {
            Die[] threeFaces = { new Die(_faceA), new Die(_faceB), new Die(_faceC)};

            var dice = new Dice(threeFaces);
            dice.Roll();

            foreach (Die d in dice.ListOfDie) {
                String sum = d.ListOfFaces.Aggregate("", (current, s) => current + s.FaceCharacter);
                Assert.IsTrue(sum.CompareTo(NamesConcat) != 0);
            }
        }

        [TestMethod]
        public void GameBoard() {

            var gb = new GameBoard();
            gb.Roll();
            DieFace[] set = gb.GetCurrentSet();
        }
    }
}
