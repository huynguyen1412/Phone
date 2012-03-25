using System;
using System.Windows;
using System.Windows.Data;

namespace Notepad {
    public class BooltoVisibilityConverter:IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
        
            bool boolValue;
            if(bool.TryParse(value.ToString(), out boolValue)) {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            else {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            Visibility visibilityValue = Visibility.Collapsed;

            try {
                visibilityValue = (Visibility)Enum.Parse(typeof(Visibility), (string)value, true);
                return visibilityValue;
            }
            catch(Exception) {
                return visibilityValue;
            }

        }
    }
}
