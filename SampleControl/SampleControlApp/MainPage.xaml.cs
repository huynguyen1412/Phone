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
            colorScroller1.Color = Color.FromArgb(0xFF, 0x40, 0x80, 0xC0);

        }

        private void OnColorScrollerColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e) {
            Brush brush = new SolidColorBrush(e.NewValue);

            if (sender == colorScroller) {
                rectangle.Stroke = brush;
            }
            else if (sender == colorScroller1) {
                rectangle.Fill = brush;
            }
        }
    }
}