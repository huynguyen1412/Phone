using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using System.ComponentModel;

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
                StreamReader stream = new StreamReader(TitleContainer.OpenStream("dictionary.dat"));
                XmlSerializer xmlFormat = new XmlSerializer(typeof(string[]));

                // Store object in a local file.
                Content = xmlFormat.Deserialize(stream) as string[];

                // create a hash the size of the dictionary
                if (_dictionaryHash == null) {
                    _dictionaryHash = new Dictionary<string, bool>(Content.Length);
                }

                foreach (string key in Content) {
                    _dictionaryHash[key] = true;
                }
            }

            private void ReadDictionaryComplete(object sender, RunWorkerCompletedEventArgs e) {
            }
        }
}
