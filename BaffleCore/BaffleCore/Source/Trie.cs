using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BaffleCore.Source
{
    public class TrieNode
    {
        public const int Size = 16;
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
        public void Add(String s) {
            TrieNode node = root;

            foreach (var c in s) {

                Char cc = Char.ToUpper(c);
                int index = MapCharacter(cc);
                Debug.Assert(index != -1);
                Debug.Assert(node != null);

                if (node.SubTrieNode[index] != null) {
                    // we have that letter in the Trie, so get the next node in the path
                    node = node.SubTrieNode[index];
                }
                else {
                    // we don't have the letter, so create a new node and assign the letter
                    var newNode = new TrieNode();
                    node.SubTrieNode[index] = newNode;
                    newNode.Character = cc;
                    node = newNode;
                }
            }

            node.IsWord = true;
            Count++;
        }
        public List<String> EnumerateAllWords() {
            var list = new List<String>();

            const string runningString = "";
            EnumerateAllWords(root, runningString, list);
            return list;
        }
        public void Empty() {
            root = null;
            Count = 0;
        }
        public bool Contains(String s) {
            TrieNode n = root;
            s = s.ToUpper();

            foreach(var ch in s) {
                int idx = MapCharacter(ch);
                if (idx == -1) { return false; }

                n = n.SubTrieNode[idx];
                if (n == null) { return false; }

                if (n.IsWord) { return true; }
            }

            return false;
        }
        private void EnumerateAllWords(TrieNode n, String runningString, List<String> runningList) {

            for (var i=0; i < TrieNode.Size; i++) {
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
    }
}
