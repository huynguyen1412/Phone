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
    public class Die
    {
        public IList<DieFace> ListOfFaces { get; set; }
        public DieFace Face {
            get {
                // return one of faces randomly.  Assume a roll occured
                Random rand = new Random();
                //return ListOfFaces[rand.Next(ListOfFaces.Count - 1)];
                return ListOfFaces[0];
            }
        }

        // Construction
        public Die(IList<DieFace> listOfFaces) {
            ListOfFaces = listOfFaces;
        }

        // Methods
        static public void Roll<T>(IList<T> list) {
            Random rand = new Random();
            for (int i = 0; i < list.Count; i++) {
                T tmp = list[i];
                int r = rand.Next(list.Count - 1);
                list[i] = list[r];
                list[r] = tmp;
            }
        }
    }
}
