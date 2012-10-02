using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using System;

namespace BaffleCore.Source {
   
    public class PrefixTree {
        private Trie prefixTreeTable;

        private string[] Content { get; set; }
        public Trie PrefixTreeTable() {
            return prefixTreeTable;
        }

        public bool Ready { get; set; }
        public bool Contains(String s) {
            return prefixTreeTable.Contains(s);
        }
        public bool Contains(char[] s) {
            return prefixTreeTable.Contains(s);
        }
        public bool PrefixExist(char[] s) {
            return prefixTreeTable.PrefixExist(s);
        }

        public List<String> EnumerateWordsBeginWith(char c) {
            return prefixTreeTable.EnumerateAllWordsBeginWith(c.ToString(CultureInfo.InvariantCulture));
        }

        public List<String> EnumerateWordsBeginWith(String prefix) {
            return prefixTreeTable.EnumerateAllWordsBeginWith(prefix);
        } 

        public PrefixTree() {
            Content = null;
            prefixTreeTable = null;
            Ready = false;
        }
        public void CreateDictionaryHash() {

            var prefixTreeThread = new BackgroundWorker();
            prefixTreeThread.DoWork += ReadPrefixTree;
            prefixTreeThread.RunWorkerCompleted += ReadPrefixTreeComplete;
            prefixTreeThread.RunWorkerAsync();
        }
        private void ReadPrefixTree(object sender, DoWorkEventArgs e) {

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

            if (prefixTreeTable == null) {
                if (Content != null) {
                    // remember, Q is a token for Qu
                    prefixTreeTable = new Trie("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                }
            }

            if (Content == null) {
                return;
            }
            foreach (string word in Content) {
                Debug.Assert(prefixTreeTable != null, "prefixTreeTable != null");
                prefixTreeTable.Add(word);
            }
            Ready = true;
        }
        private void ReadPrefixTreeComplete(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Error == null && e.Cancelled == false) {
                Content = null;  // free the raw serialize data, Trie table is created.
            }
        }
    }
}
