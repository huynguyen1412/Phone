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
using System.Collections.Generic;

namespace BaffleCore.Source
{
    
    public class Dice
    {
        public IList<Die> ListOfDie { get; set; }

        // Construction
        public Dice(IList<Die> listOfDie) {
            ListOfDie = listOfDie;
        }

        // Methods
        public void Roll() {
            foreach(Die d in ListOfDie) {
                Die.Roll(d.ListOfFaces);
            }
        }

        public DieFace[] GetCurrentSet() {
            DieFace[] set = new DieFace[ListOfDie.Count];
            int pos = 0;

            foreach (Die d in ListOfDie) {
                set.SetValue(d.Face, pos++);
            }

            // Now mix the die so they change position.
            // This moves them around the board otherwise the 
            // same die is in the same array position.
            Die.Roll(set);
            return set;
        }
    }
}
