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
using System.Windows.Threading;

namespace HelloPhone {
    public partial class MainPage : PhoneApplicationPage {

        Random rand = new Random();
        Brush originalBrush;
        
        // Constructor
        public MainPage() {
            InitializeComponent();

            originalBrush = TxtBlock.Foreground;

            // set the initial time
            TimeDisplayText.Text = DateTime.Now.ToString();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler((sender, args) => {
                TimeDisplayText.Text = DateTime.Now.ToString();
            });
            timer.Start();
        }

        private void ContentPanel_SizeChanged(object sender, SizeChangedEventArgs e) {
            TxtBlock.Text = String.Format("Content Panel size: {0}\n" +
                                          "TitlePanel size: {1}\n" +
                                          "LayoutRoot size: {2}\n" +
                                          "MainPage size: {3}\n" +
                                          "Frame size: {4}\n" +
                                          "Orientation: {5}", e.NewSize,
                                          new Size(TitlePanel.ActualWidth, TitlePanel.ActualHeight),
                                          new Size(LayoutRoot.ActualWidth, LayoutRoot.ActualHeight),
                                          new Size(this.ActualWidth, this.ActualHeight),
                                          Application.Current.RootVisual.RenderSize,
                                          this.Orientation);

        }

        private void OnTextBlockManipulationStarted(object sender, ManipulationStartedEventArgs e) {

            Color clr = Color.FromArgb(255, (byte)rand.Next(256),
                                            (byte)rand.Next(256),
                                            (byte)rand.Next(256));
            TxtBlock.Foreground = new SolidColorBrush(clr);
            e.Handled = true;
            e.Complete();
        }

        protected override void OnManipulationStarted(ManipulationStartedEventArgs e) {

            TxtBlock.Foreground = originalBrush;
            e.Complete();
            base.OnManipulationStarted(e);
        }
    }
}