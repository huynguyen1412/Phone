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
using SlideToggle;

namespace TestApp {
    public partial class MainPage : PhoneApplicationPage {
        // Constructor
        public MainPage() {
            InitializeComponent();
            slideToggle2.IsChecked = true;
        }

        private void OnSlideToggle1Checked(object sender, RoutedEventArgs e) {
            TapSlideToggle toggle = sender as TapSlideToggle;
            option1TextBlock.Text = toggle.IsChecked ? "on" : "off";
            slideToggle2.IsEnabled = false;

        }

        private void OnSlideToggle2Checked(object sender, RoutedEventArgs e) {
          TapSlideToggle toggle = sender as TapSlideToggle;
            option2TextBlock.Text = toggle.IsChecked ? "on" : "off";

        }
    }
}