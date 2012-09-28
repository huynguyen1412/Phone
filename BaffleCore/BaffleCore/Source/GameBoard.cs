
using System.Collections.Generic;
using System.Diagnostics;

namespace BaffleCore.Source
{
    public class GameBoard
    {
        public Dice GameDice { get; set; }
        public GameBoard() {

            DieFace[] die01 = { new DieFace("A"), new DieFace("J"), new DieFace("B"), 
                           new DieFace("O"), new DieFace("B"), new DieFace("O")};
            DieFace[] die02 = { new DieFace("Z"), new DieFace("N"), new DieFace("R"), 
                           new DieFace("N"), new DieFace("H"), new DieFace("L")};
            DieFace[] die03 = { new DieFace("G"), new DieFace("A"), new DieFace("E"), 
                           new DieFace("N"), new DieFace("E"), new DieFace("A")};
            DieFace[] die04 = { new DieFace("N"), new DieFace("S"), new DieFace("I"), 
                           new DieFace("E"), new DieFace("U"), new DieFace("E")};

            DieFace[] die05 = { new DieFace("T"), new DieFace("S"), new DieFace("E"), 
                           new DieFace("O"), new DieFace("S"), new DieFace("I")};
            DieFace[] die06 = { new DieFace("Y"), new DieFace("I"), new DieFace("D"), 
                           new DieFace("S"), new DieFace("T"), new DieFace("T")};
            DieFace[] die07 = { new DieFace("G"), new DieFace("H"), new DieFace("W"), 
                           new DieFace("N"), new DieFace("E"), new DieFace("E")};
            DieFace[] die08 = { new DieFace("P"), new DieFace("K"), new DieFace("A"), 
                           new DieFace("S"), new DieFace("F"), new DieFace("F")};

            DieFace[] die09 = { new DieFace("V"), new DieFace("T"), new DieFace("H"), 
                           new DieFace("R"), new DieFace("W"), new DieFace("E")};
            DieFace[] die10 = { new DieFace("L"), new DieFace("R"), new DieFace("E"), 
                           new DieFace("D"), new DieFace("Y"), new DieFace("V")};
            DieFace[] die11 = { new DieFace("D"), new DieFace("L"), new DieFace("X"), 
                           new DieFace("R"), new DieFace("I"), new DieFace("E")};
            DieFace[] die12 = { new DieFace("C"), new DieFace("P"), new DieFace("O"), 
                           new DieFace("A"), new DieFace("S"),  new DieFace("H")};


            DieFace[] die13 = {new DieFace("A"), new DieFace("T"), new DieFace("T"), 
                           new DieFace("O"), new DieFace("W"), new DieFace("O")};
            DieFace[] die14 = {new DieFace("M"), new DieFace("I"), new DieFace("Qu"), 
                           new DieFace("U"), new DieFace("H"), new DieFace("N")};
            DieFace[] die15 = {new DieFace("T"), new DieFace("L"), new DieFace("R"), 
                           new DieFace("T"), new DieFace("E"), new DieFace("Y")};
            DieFace[] die16 = {new DieFace("T"), new DieFace("U"), new DieFace("I"), 
                           new DieFace("M"), new DieFace("O"),  new DieFace("C")};

            Die[] arrayOfDie = {new Die(die01), new Die(die02), new Die(die03), new Die(die04),
                                new Die(die05), new Die(die06), new Die(die07), new Die(die08),
                                new Die(die09), new Die(die10), new Die(die11), new Die(die12),
                                new Die(die13), new Die(die14), new Die(die15), new Die(die16)};
            
            GameDice = new Dice(arrayOfDie);
        }

        private int index = 0;
        private char[] word = new char[20];
        private List<string> wordList;
        private AdjacencyMap graph;
        private PrefixTree dict;

        public void Roll() {
            GameDice.Roll();
        }

        public DieFace[] GetCurrentSet() {
            return GameDice.GetCurrentSet();
        }

        public List<string> ResolveWords(PrefixTree dictionary, DieFace[] currentBoard) {
            int[,] nodes = { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 0xA, 0xB, 0xC }, { 0xD, 0xE, 0xF, 0x10 } };
            var letters = new char[4,4];
            int x = 0;
            int y = 0;
            foreach (var dieFace in currentBoard) {
                letters[x, y++] = dieFace.FaceCharacter[0];
                if (y == 4) {
                    y = 0;
                    x++;
                }
            }
            wordList= new List<string>();
            dict = dictionary;
            graph = BuildGraph<char>(nodes, letters);

            foreach (var t in nodes) {
                AdjacencyNode node = graph.map[t];
                node.Visted = 1;
                Debug.Assert(index == 0);
                word[index] = node.NodeContent;
                BuildCombinations(node, word);
                node.Visted = 0;
                word[index + 1] = (char)0;
            }

            return wordList;
        }

        class AdjacencyNode
        {
            public char NodeContent;

            public List<int> Adjacency;
            public int Visted { get; set; }
            public int NodeNumber { get; set; }

        }
        class AdjacencyMap
        {
            public Dictionary<int, AdjacencyNode> map;
            public AdjacencyMap() {
                map = new Dictionary<int, AdjacencyNode>(16);
            }
            public void AddNode(AdjacencyNode node) {
                map.Add(node.NodeNumber, node);
            }
        }
        private void BuildCombinations(AdjacencyNode node, char[] runnungWord) {
            var list = node.Adjacency;
            string w;

            foreach (int nn in list) {
                AdjacencyNode al = graph.map[nn];
                
                if (al.Visted == 1)
                    continue;

                ++index;
                runnungWord[index] = al.NodeContent;
                if (dict.Contains(runnungWord)) {
                    wordList.Add(new string(runnungWord));
                }

                al.Visted = 1;
                BuildCombinations(al, runnungWord);
                runnungWord[index] = (char)0;
                --index;
                al.Visted = 0;
            }
        }
        private AdjacencyMap BuildGraph<T>(int[,] array, char[,] arrayOfContent) {

            #region NEIGHBORS_ARRAY
            // neighbors to each node in a 4x4 matrix moving clockwise
            int[,] neighbors = {
                                   {01, 11, 10,-01,-01,-01,-01,-01}, // 0,0
                                   {02, 12, 11, 10, 00,-01,-01,-01}, // 0,1
                                   {03,13,12,11,01,-01,-01-01,-01},  // 0,2
                                   {13,12,02,-01,-01,-01,-01,-01},   // 0,3
                                   {11,21,20,00,01,-01,-01,-01},     // 1,0
                                   {12,22,21,20,10,00,01,02},        // 1,1
                                   {13,23,22,21,11,01,02,03},        // 1,2
                                   {23,22,12,02,03,-01,-01,-01},     // 1,3
                                   {21,31,30,10,11,-01,-01,-01},     // 2,0
                                   {22,32,31,30,20,10,11,12},        // 2,1
                                   {23,33,32,31,21,11,12,13},        // 2,2
                                   {33,32,22,12,13,-01,-01,-01},     // 2,3
                                   {31,20,21,-01,-01,-01,-01,-01},   // 3,0
                                   {32,30,20,21,22,-01,-01,-01},     // 3,1
                                   {33,31,21,22,23,-01,-01,-01},     // 3,2
                                   {32,22,23,-01,-01,-01,-01,-01}    // 3,3
                               };
            #endregion
            var map = new AdjacencyMap();
            int xx = 0; int yy = 0;
            int x; int y;

            for (x = 0; x < 4; x++) {
                for (y = 0; y < 4; y++) {
                    var node = new AdjacencyNode {
                        NodeNumber = array[x, y],
                        Adjacency = new List<int>(),
                        NodeContent = arrayOfContent[x, y]
                    };

                    while (yy < 8 && neighbors[xx, yy] != -1) {
                        var m = neighbors[xx, yy] / 10;
                        var n = neighbors[xx, yy] % 10;
                        node.Adjacency.Add(array[m, n]);
                        ++yy;
                    }
                    ++xx;
                    yy = 0;
                    map.AddNode(node);
                }
            }
            return map;
        }
    }
}
