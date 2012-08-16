using System;
using System.Collections.Generic;

namespace BaffleCore.Source
{
    public class Die
    {
        public IList<DieFace> ListOfFaces { get; set; }
        public DieFace Face {
            get
            {
                // return one of faces randomly.  Assume a roll occured
                var rand = new Random();
                if (ListOfFaces != null) {
                    return ListOfFaces[rand.Next(ListOfFaces.Count - 1)];
                }
                return null;
            }
        }

        // Construction
        public Die(IList<DieFace> listOfFaces) {
            ListOfFaces = listOfFaces;
        }

        // Methods
        static public void Roll<T>(IList<T> list) {
            var rand = new Random();
            for (int i = 0; i < list.Count; i++) {
                T tmp = list[i];
                int r = rand.Next(list.Count - 1);
                list[i] = list[r];
                list[r] = tmp;
            }
        }
    }
}
