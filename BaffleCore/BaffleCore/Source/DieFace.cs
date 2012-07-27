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
    public class DieFace
    {
        public String FaceCharacter { get; set; }

        // Possible Properties
        // Glyph, Size, Text, Color

        // Methods

        // Construction
        public DieFace(String faceCharacter) {
            FaceCharacter = faceCharacter;
        }
    }
}
