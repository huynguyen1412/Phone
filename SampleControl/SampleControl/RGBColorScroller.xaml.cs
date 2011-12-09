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

namespace SampleControl {
    public partial class RGBColorScroller : UserControl {

        #region @property Color Color;
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(RGBColorScroller), new PropertyMetadata(Colors.Gray, OnColorChanged));

        public Color Color {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        #endregion

        public event RoutedPropertyChangedEventHandler<Color> ColorChanged;

        private void OnColorColumnValueChanged(object sender, RoutedPropertyChangedEventArgs<byte> args) {
            Color = Color.FromArgb(255, redColumn.Value, greenColumn.Value, blueColumn.Value);
        }

        static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) {
            (obj as RGBColorScroller).OnColorChanged((Color)args.OldValue, (Color)args.NewValue);
        }

        protected virtual void OnColorChanged(Color oldValue, Color newValue) {
            redColumn.Value = newValue.R;
            greenColumn.Value = newValue.G;
            blueColumn.Value = newValue.B;

            if (ColorChanged != null) {
                ColorChanged(this, new RoutedPropertyChangedEventArgs<Color>(oldValue, newValue));
            }
        }


        public RGBColorScroller() {
            InitializeComponent();
        }
    }
}
