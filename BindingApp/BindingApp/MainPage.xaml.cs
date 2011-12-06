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
using System.Windows.Data;
using System.Globalization;

namespace BindingApp {
    public partial class MainPage : PhoneApplicationPage {
        // Constructor
        public MainPage() {
            InitializeComponent();
            //Binding binding = new Binding();
            //binding.ElementName = "Slider";
            //binding.Path = new PropertyPath("Value");
            //txtBlock.SetBinding(TextBlock.TextProperty, binding);
        }
    }

    public class TruncationConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (targetType == typeof(string) && parameter is string) {
                return String.Format(parameter as string, value);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value;
        }
    }
}