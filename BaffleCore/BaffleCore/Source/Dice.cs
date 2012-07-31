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

namespace BaffleCore.Source
{
    
    public class Dice
    {
       

        public Die[] ArrayOfDie { get; set; }

        // Construction
        public Dice(Die[] arrayOfDie) {
            ArrayOfDie = arrayOfDie;
        }

        // Methods
        public void Roll() {
            foreach (Die d in ArrayOfDie) {
                d.Roll();
            }
        }

        public DieFace[] GetCurrentSet() {
            DieFace[] set = new DieFace[ArrayOfDie.Length];
            int pos = 0;

            foreach (Die d in ArrayOfDie) {
                set.SetValue(d.Face, pos++);
            }

            return set;
        }
    }
}
