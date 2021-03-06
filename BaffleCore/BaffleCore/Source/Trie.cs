﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BaffleCore.Source {
    public class TrieNode {
        public const int Size = 26;
        public bool IsWord { get; set; }
        public char Character { get; set; }
        public TrieNode[] SubTrieNode;

        public TrieNode() {
            SubTrieNode = new TrieNode[Size];
            IsWord = false;
            Character = '0';
        }
    }
    public class Trie {
        private TrieNode root;
        private readonly Dictionary<char, int> map;

        public int NumberOfUniqueCharacters {
            get { return map.Count; }
        }
        public int Count { get; set; }
        public Trie(String characters) {
            map = new Dictionary<char, int>(TrieNode.Size);
            root = new TrieNode();

            // no default constructor;therefore, so null argument is not possible.  No need to test
            if (characters.Length == 0 || characters.Length > TrieNode.Size) {
                throw new ArgumentOutOfRangeException();
            }

            int x = 0;
            foreach (var c in characters) {
                if (!map.ContainsKey(Char.ToUpper(c))) {
                    map[Char.ToUpper(c)] = x;
                    ++x;
                }
            }
        }
        public int MapCharacter(char c) {
            int index;

            try {
                index = map[Char.ToUpper(c)];
            }
            catch (KeyNotFoundException) {
                index = -1;
            }

            return index;
        }
        private void EnumerateAllWords(TrieNode n, String runningString, List<String> runningList) {
            for (var i = 0; i < TrieNode.Size; i++) {
                if (n.SubTrieNode[i] == null) {
                    continue;
                }
                if (n.SubTrieNode[i].IsWord) {
                    // concat the letter and add it to the running list
                    runningList.Add(runningString + n.SubTrieNode[i].Character);
                }
                EnumerateAllWords(n.SubTrieNode[i], runningString + n.SubTrieNode[i].Character, runningList);
            }
        }
        public bool PrefixExist(char [] prefix) {
            var node = root;
            bool exist = true;

            foreach(var c in prefix) {
                
                // null characters are padded at the end
                if (c == 0) {
                    break;
                }

                int idx = MapCharacter(c);
                if (idx == -1) {
                    exist = false;
                    break;
                }
                Debug.Assert(node != null, "node != null");
                node = node.SubTrieNode[idx];
                if (node == null) {
                    exist =  false;
                }
            }

            return exist;
        }
        public List<String> EnumerateAllWordsBeginWith(String prefix) {
            var list = new List<String>();
            var node = root;

            Debug.Assert(node != null, "node != null");
            foreach (var c in prefix) {
                var idx = MapCharacter(c);

                if ( idx == -1) {
                    return list;
                }

                node = node.SubTrieNode[idx];

                if (node == null) {
                    return list;
                }
            }

            // the prefix could be a word, so add it to the list
            if (node.IsWord) {
                list.Add(prefix);
            }

            EnumerateAllWords(node, prefix, list);
            return list;
        } 
        public List<String> EnumerateAllWords() {
            var list = new List<String>();

            const string runningString = "";
            EnumerateAllWords(root, runningString, list);
            return list;
        }
        public void Add(String s) {
            TrieNode node = root;

            foreach (var c in s) {
             //   Char cc = Char.ToUpper(c);
                int index = MapCharacter(c);
                Debug.Assert(index != -1);
                Debug.Assert(node != null);

                if (node.SubTrieNode[index] == null) {
                    // we don't have the letter, so create a new node and assign the letter
                    var newNode = new TrieNode { Character = c };
                    node.SubTrieNode[index] = newNode;
                }
                node = node.SubTrieNode[index];
            }

            node.IsWord = true;
            Count++;
        }
        public void Empty() {
            root = null;
            Count = 0;
        }
        public bool Contains(String s) {
            int idx;
            bool word = true;
            TrieNode n = root;
            TrieNode oldNode = root;

            s = s.ToUpper();

                foreach (var ch in s) {
                    if (ch == 0) {
                        break;
                    }

                    idx = MapCharacter(ch);
                    if (idx == -1)
                        return false;

                    n = n.SubTrieNode[idx];

                    if (n == null) {
                        word = false;
                        break;
                    }
                    oldNode = n;
                }

            if (word && oldNode.IsWord) {return true; }

            return false;
        }
        public bool Contains(char[] s) {
            bool word = true;
            TrieNode n = root;
            TrieNode oldNode = root;

            foreach (char t in s) {
                if (t == 0) {
                    break;
                }

                int idx = MapCharacter(t);
                if (idx == -1)
                    return false;

                n = n.SubTrieNode[idx];

                if (n == null) {
                    word = false;
                    break;
                }
                oldNode = n;
            }

            if (word && oldNode.IsWord) { return true; }

            return false;
        }
    }
}