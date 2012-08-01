using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WPToolKit;

namespace BaffleCore.Source
{
    public class GameBoard
    {
        public Dice GameDice { get; set; }

        public GameBoard() {

            DieFace[] die01 = { new DieFace("A"), new DieFace("J"), new DieFace("B"), 
                           new DieFace("O"), new DieFace("B"), new DieFace("O")};
            DieFace[] die02 = { new DieFace("Z"), new DieFace("N"), new DieFace("R"), 
                           new DieFace("N"), new DieFace("H"), new DieFace("L")};
            DieFace[] die03 = { new DieFace("G"), new DieFace("A"), new DieFace("E"), 
                           new DieFace("N"), new DieFace("E"), new DieFace("A")};
            DieFace[] die04 = { new DieFace("N"), new DieFace("S"), new DieFace("I"), 
                           new DieFace("E"), new DieFace("U"), new DieFace("E")};

            DieFace[] die05 = { new DieFace("T"), new DieFace("S"), new DieFace("E"), 
                           new DieFace("O"), new DieFace("S"), new DieFace("I")};
            DieFace[] die06 = { new DieFace("Y"), new DieFace("I"), new DieFace("D"), 
                           new DieFace("S"), new DieFace("T"), new DieFace("T")};
            DieFace[] die07 = { new DieFace("G"), new DieFace("H"), new DieFace("W"), 
                           new DieFace("N"), new DieFace("E"), new DieFace("E")};
            DieFace[] die08 = { new DieFace("P"), new DieFace("K"), new DieFace("A"), 
                           new DieFace("S"), new DieFace("F"), new DieFace("F")};

            DieFace[] die09 = { new DieFace("V"), new DieFace("T"), new DieFace("H"), 
                           new DieFace("R"), new DieFace("W"), new DieFace("E")};
            DieFace[] die10 = { new DieFace("L"), new DieFace("R"), new DieFace("E"), 
                           new DieFace("D"), new DieFace("Y"), new DieFace("V")};
            DieFace[] die11 = { new DieFace("D"), new DieFace("L"), new DieFace("X"), 
                           new DieFace("R"), new DieFace("I"), new DieFace("E")};
            DieFace[] die12 = { new DieFace("C"), new DieFace("P"), new DieFace("O"), 
                           new DieFace("A"), new DieFace("S"),  new DieFace("H")};


            DieFace[] die13 = {new DieFace("A"), new DieFace("T"), new DieFace("T"), 
                           new DieFace("O"), new DieFace("W"), new DieFace("O")};
            DieFace[] die14 = {new DieFace("M"), new DieFace("I"), new DieFace("Qu"), 
                           new DieFace("U"), new DieFace("H"), new DieFace("N")};
            DieFace[] die15 = {new DieFace("T"), new DieFace("L"), new DieFace("R"), 
                           new DieFace("T"), new DieFace("E"), new DieFace("Y")};
            DieFace[] die16 = {new DieFace("T"), new DieFace("U"), new DieFace("I"), 
                           new DieFace("M"), new DieFace("O"),  new DieFace("C")};

            Die[] arrayOfDie = {new Die(die01), new Die(die02), new Die(die03), new Die(die04),
                                new Die(die05), new Die(die06), new Die(die07), new Die(die08),
                                new Die(die09), new Die(die10), new Die(die11), new Die(die12),
                                new Die(die13), new Die(die14), new Die(die15), new Die(die16)};
            
            GameDice = new Dice(arrayOfDie);
        }

        public void Roll() {
            GameDice.Roll();
        }

        public DieFace[] GetCurrentSet() {
            return GameDice.GetCurrentSet();
        }
    }
}
