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
    public class Die
    {
        public DieFace[] ArrayOfFaces { get; set; }

        // Construction
        public Die(DieFace[] arrayOfFaces) {
            ArrayOfFaces = arrayOfFaces;
        }

        // Methods
        public void Roll() {
        }
    }
}
