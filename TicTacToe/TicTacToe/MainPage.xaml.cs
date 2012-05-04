using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace TicTacToe {
    public partial class MainPage : PhoneApplicationPage {

        GameAI gameEngine;
  
        // Constructor
        public MainPage() {
            InitializeComponent();
            gameEngine = new GameAI();
            gameEngine.ResetBoard();

        }

        private void Square_Clicked(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Debug.WriteLine("Button " + b.Name + "Source");

            //imgMyImage.Source = new BitmapImage(new Uri(“images/smiley.png”, UriKind.Relative));
            Image content = b.Content as Image;
            if (null != content) {
                content.Source = new BitmapImage(new Uri("/Images/O.png", UriKind.Relative));
            }
        }
    }
}