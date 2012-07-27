﻿using System;
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
    }
}
