using System.Collections.Generic;

namespace BaffleCore.Source
{
    
    public class Dice
    {
        public IList<Die> ListOfDie { get; set; }
        public bool QInSet;

        // Construction
        public Dice(IList<Die> listOfDie) {
            ListOfDie = listOfDie;
            QInSet = false;
        }

        // Methods
        public void Roll() {
            foreach(Die d in ListOfDie) {
                Die.Roll(d.ListOfFaces);
            }
        }

        public DieFace[] GetCurrentSet() {
            var set = new DieFace[ListOfDie.Count];
            int pos = 0;

            foreach (Die d in ListOfDie) {
                DieFace f = d.Face;
                if (f.FaceCharacter.CompareTo("Q") == 0) {
                    QInSet = true;
                }
                set.SetValue(f, pos++);
            }

            // Now mix the die so they change position.
            // This moves them around the board otherwise the 
            // same die is in the same array position.
            Die.Roll(set);
            return set;
        }
    }
}
