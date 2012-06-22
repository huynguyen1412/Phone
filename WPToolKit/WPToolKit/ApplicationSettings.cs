using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;

namespace WPToolKit
{
    public class ApplicationSettings
    {
        private IsolatedStorageSettings appSettings;

        public ApplicationSettings() {
             appSettings =
                IsolatedStorageSettings.ApplicationSettings;
        }

        public object this[string s] {
            get {
                string result;

                if (s == null) {
                    throw new ArgumentNullException();
                }

                appSettings.TryGetValue<string>(s, out result);

                return result;
            }
            set {
                if (s == null) {
                    throw new ArgumentNullException();
                }

                appSettings[s] = value;
            }
        }

        public bool Remove(string s) {

            if (s == null) {
                throw new ArgumentNullException();
            }

            return appSettings.Remove(s);
        }

        public void Reset() {
            appSettings.Clear();
            appSettings.Save();
        }
    }
}
