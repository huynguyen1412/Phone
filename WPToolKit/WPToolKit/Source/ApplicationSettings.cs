using System;
using System.IO.IsolatedStorage;
using System.Collections.Generic;

namespace WPToolKit.Source
{
    public sealed class ApplicationSettings
    {
        private readonly IsolatedStorageSettings appSettings;
        public ApplicationSettings() {
             appSettings =
                IsolatedStorageSettings.ApplicationSettings;
        }
        /// <summary>
        /// Gets the count of setting in the dictionary
        /// </summary>
        /// <remarks></remarks>
        public int Count {
            get {
                return appSettings.Count;
            }
        }
        /// <summary>
        /// Initializes the settings to the values provided by a dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="settings">The settings.</param>
        /// <remarks>Used to populate the application with default settings</remarks>
        public void InitializeSettings<TKey, TValue>(Dictionary<TKey, TValue> settings) where TKey : class {

            foreach (KeyValuePair<TKey, TValue> item in settings) {
                appSettings[item.Key as string] = item.Value;
            }

            Save();
        }
        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <remarks>Overloaded Index operators</remarks>
        public object this[string key] {
            get {
                string result;

                if (key == null) {
                    throw new ArgumentNullException();
                }

                appSettings.TryGetValue(key, out result);

                return result;
            }
            set {
                if (key == null) {
                    throw new ArgumentNullException();
                }

                appSettings[key] = value;
            }
        }
        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Remove(string key) {

            if (key == null) {
                throw new ArgumentNullException();
            }

            return appSettings.Remove(key);
        }
        /// <summary>
        /// Removes all settings in the dictionary and attempts to save the empty settings dictionary.
        /// </summary>
        /// <remarks>Removes all the keys and saves the emptied storage</remarks>
        public void Reset() {
            appSettings.Clear();
            Save();
        }
        /// <summary>
        /// Internal Save function that wraps the exception.
        /// </summary>
        /// <remarks></remarks>
        private void Save() {
            
            try {
                appSettings.Save();
            }
            catch (IsolatedStorageException e) {
#if DEBUG
                Console.WriteLine("ApplicationSettings:" + e);
#endif
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        public class ApplicationSettingsComparator : EqualityComparer<string>
        {
            // Assumes class is sealed
            public override bool Equals(string a, string b) {
                return String.Compare(a, b, StringComparison.Ordinal) == 0;
            }
            public override int GetHashCode(string a) {
                if (a == null) {
                    throw new ArgumentNullException();
                }
                return a.GetHashCode();
            }
        }
    }
}
