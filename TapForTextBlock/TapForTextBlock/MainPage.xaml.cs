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

namespace TapForTextBlock {
    public partial class MainPage : PhoneApplicationPage {
        Random rand = new Random();

        // Constructor
        public MainPage() {
            InitializeComponent();
        }

        protected override void OnManipulationStarted(ManipulationStartedEventArgs e) {

            // C# 3.0 syntax to initialize the properties of a newly created object.
            //  Benefits?  no need to prefix the object name before the property
            // ex newTextBlock.Text becomes Text
            // Note, they are separated by commas and the trailing braces needs a semi-colon
            TextBlock newTextBlock = new TextBlock() {
                Text = "Hello, Windows Phone 7",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(
                    (ContentPanel.ActualWidth - txtBlk.ActualWidth) * rand.NextDouble(), (ContentPanel.ActualHeight - txtBlk.ActualHeight) * rand.NextDouble(), 0, 0)
            };

            ContentPanel.Children.Add(newTextBlock);
            e.Complete();
            e.Handled = true;
            base.OnManipulationStarted(e);

        }

    }

    
}