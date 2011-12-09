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

namespace SampleControlApp {
    public partial class MainPage : PhoneApplicationPage {
        // Constructor
        public MainPage() {
            InitializeComponent();
            colorScroller.Color = Color.FromArgb(0xFF, 0xC0, 0x80, 0x40);
        }

        private void colorScroller_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e) {
            Brush brush = new SolidColorBrush(e.NewValue);

            if (sender == colorScroller) {
            }
        }

        private void colorScroller1_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e) {

        }
    }
}