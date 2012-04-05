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


namespace SlideToggle {
    public partial class TapSlideToggle : UserControl {

        #region @property bool IsChecked;
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(TapSlideToggle), new PropertyMetadata(false, OnIsCheckedChanged));

        
        
        public bool IsChecked {
            set {
                SetValue(IsCheckedProperty, value);
            }
            get {
                return (bool) GetValue(IsCheckedProperty);
            }
        }

        #endregion

        public event RoutedEventHandler Checked;
        public event RoutedEventHandler Unchecked;

        static TapSlideToggle() {
        //   IsEnabledProperty.OverrideMetadata(null, new PropertyMetadata(OnIsEnabledChanged));
        }

        public TapSlideToggle() {
            InitializeComponent();
            

        }

        static void OnIsCheckedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) {
            (obj as TapSlideToggle).OnIsCheckedChanged(args);
        }

        static void OnIsEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) {
        }

        void OnIsCheckedChanged(DependencyPropertyChangedEventArgs ars) {

            fillRectangle.Visibility = IsChecked ? Visibility.Visible : Visibility.Collapsed;
            sliderBorder.HorizontalAlignment = IsChecked ? HorizontalAlignment.Right : HorizontalAlignment.Left;

            if (IsChecked && Checked != null)
                Checked(this, new RoutedEventArgs());

            if (!IsChecked && Unchecked != null)
                Unchecked(this, new RoutedEventArgs());
        }

       
        protected override void OnManipulationStarted(ManipulationStartedEventArgs e) {
            e.Handled = true;
            base.OnManipulationStarted(e);
        }

        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e) {

            Point pt = e.ManipulationOrigin;

            if (pt.X > 0 && pt.X < this.ActualWidth && pt.Y > 0 && pt.Y < this.ActualHeight)
                IsChecked ^= true;

            e.Handled = true;
            base.OnManipulationCompleted(e);
        }
    }
}
