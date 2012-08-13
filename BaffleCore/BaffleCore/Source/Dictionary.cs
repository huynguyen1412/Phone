using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using System;

namespace BaffleCore.Source {
   
    public class Dictionary {
            Dictionary<string, bool> _dictionaryHash;

            private string[] Content { get; set; }
            public Dictionary<string, bool> DictionaryHash() {
                return _dictionaryHash;
            }

            public Dictionary() {
                Content = null;
                _dictionaryHash = null;
            }
            public void CreateDictionaryHash() {

                var dictionaryThread = new BackgroundWorker();
                dictionaryThread.DoWork += ReadDictionary;
                dictionaryThread.RunWorkerCompleted += ReadDictionaryComplete;
                dictionaryThread.RunWorkerAsync();
            }
            private void ReadDictionary(object sender, DoWorkEventArgs e) {

                Stream s=null;
                StreamReader stream = null;
                
                try
                {
                    s = TitleContainer.OpenStream("dictionary.dat");

                    if (s == null) {
                        return;
                    }

                    stream = new StreamReader(s);
                    var xmlFormat = new XmlSerializer(typeof(string[]));
                    Content = xmlFormat.Deserialize(stream) as string[];
                    
                }
                catch (InvalidOperationException)
                {
                }
                finally {
                    if (s != null) {
                        s.Close();
                    }
                    if (stream != null) {
                        stream.Close();
                    }
                }

                // create a hash the size of the dictionary
                if (_dictionaryHash == null) {
                    if (Content != null) _dictionaryHash = new Dictionary<string, bool>(Content.Length);
                }

                if (Content == null) {
                    return;
                }
                foreach (string key in Content) {
                    Debug.Assert(_dictionaryHash != null, "_dictionaryHash != null");
                    _dictionaryHash[key] = true;
                }
            }
            private void ReadDictionaryComplete(object sender, RunWorkerCompletedEventArgs e) {
                if (e.Error == null && e.Cancelled == false) {
                    Content = null;  // free the raw serialize data, hash table is created.
                }
            }
        }
}
