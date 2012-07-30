using System;
using BaffleCore;
using BaffleCore.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BaffleCoreTest.Unit_Test {

    [TestClass]
    public class DiceTest {

        DieFace[] faceA = { new DieFace("A"), new DieFace("B"), new DieFace("C"), 
                            new DieFace("D"), new DieFace("E"), new DieFace("F")};
        DieFace[] faceB = { new DieFace("A"), new DieFace("B"), new DieFace("C"), 
                            new DieFace("D"), new DieFace("E"), new DieFace("F")};
        DieFace[] faceC = { new DieFace("A"), new DieFace("B"), new DieFace("C"), 
                            new DieFace("D"), new DieFace("E"), new DieFace("F")};

        String[] names = { "A", "B", "C", "D", "E", "F" };
        String names_concat = "ABCDEF";

        [TestInitialize]
        public void Setup() {
        }
        [TestMethod]
        public void TestDiceConstruction() {
            Die[] die = { new Die(faceA) };
            Dice dice = new Dice(die);
            Assert.IsTrue(dice.ArrayOfDie.Length == 1);
            Assert.IsTrue(dice.ArrayOfDie[0] == die[0]);
        }
        [TestMethod]
        public void TestDiceRoll() {
            Die[] threeFaces = { new Die(faceA), new Die(faceB), new Die(faceC)};

            Dice dice = new Dice(threeFaces);
            dice.Roll();

            foreach (Die d in dice.ArrayOfDie) {
                String sum = "";
                foreach (DieFace s in d.ArrayOfFaces) {
                    sum += s.FaceCharacter;
                }
                Assert.IsTrue(sum.CompareTo(names_concat) != 0);
            }
        }

        [TestMethod]
        public void GameBoard() {

            GameBoard gb = new GameBoard();
            gb.GameDice.ToString();
            Console.WriteLine("***************************** Roll ****************************************");
            gb.Roll();
            gb.GameDice.ToString();
        }
    }
}
