using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using System;

namespace BaffleCore.Source {
   
    public class Dictionary {
            Dictionary<string, bool> _dictionaryHash;

            public string[] Content { get; set; }
            public Dictionary() {
                Content = null;
                _dictionaryHash = null;
            }
            public Dictionary<string, bool> DictionaryHash() {
                return _dictionaryHash;
            }
            public void CreateDictionaryHash() {

                BackgroundWorker dictionaryThread = new BackgroundWorker();
                dictionaryThread.DoWork += new DoWorkEventHandler(ReadDictionaryStream);
                dictionaryThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ReadDictionaryComplete);
                dictionaryThread.RunWorkerAsync();
            }
            private void ReadDictionaryStream(object sender, DoWorkEventArgs e) {

                Stream s=null;
                StreamReader stream = null;
                
                try {
                    s = TitleContainer.OpenStream("dictionary.dat");

                    if (s == null) {
                        throw new ArgumentException();
                    }

                    stream = new StreamReader(s);
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(string[]));

                    // Store object in a local file.
                    Content = xmlFormat.Deserialize(stream) as string[];
                }
                catch (Exception) {
                }
                finally {
                    s.Close();
                    stream.Close();
                }

                // create a hash the size of the dictionary
                if (_dictionaryHash == null) {
                    _dictionaryHash = new Dictionary<string, bool>(Content.Length);
                }

                foreach (string key in Content) {
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
