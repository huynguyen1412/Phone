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
    public partial class ColorColumn : UserControl {

        #region @property string Label;
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(ColorColumn), new PropertyMetadata(OnLabelChanged));

        public string Label {
            get { return GetValue(LabelProperty) as string; }
            set { SetValue(LabelProperty, value); }
        }
        #endregion
        #region @property byte Value;
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(byte), typeof(ColorColumn), new PropertyMetadata((byte)0, OnValueChanged));

        public byte Value {
            get { return (byte)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        public event RoutedPropertyChangedEventHandler<byte> ValueChanged;

         static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) {
            (obj as ColorColumn).OnValueChanged((byte)args.OldValue, (byte)args.NewValue);
        }

        private void OnValueChanged(byte oldValue, byte newValue) {
            slider.Value = newValue;
            colorValue.Text = newValue.ToString("X2");
            slider.Foreground = colorLabel.Foreground;

            if (ValueChanged != null) {
                ValueChanged(this, new RoutedPropertyChangedEventArgs<byte>(oldValue, newValue));                
            }
        }

        static void OnLabelChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) {
            (obj as ColorColumn).colorLabel.Text = args.NewValue as string;
        }

        private void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            Value = (byte)e.NewValue;
        }

        public ColorColumn() {
            InitializeComponent();
        }
    }
}
