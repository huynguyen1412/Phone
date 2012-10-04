using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using BaffleCore.Source;
using Microsoft.Phone.Controls;

namespace BaffleCore {
    public partial class MainPage : PhoneApplicationPage {
        static public PrefixTree dictionary;
        private GameBoard gb;
        private Dictionary<string, bool> wordList;

        // Constructor
        public MainPage() {
            InitializeComponent();


            // PrefixTree takes a few seconds to create, so do it here
            dictionary = new PrefixTree();
            dictionary.CreateDictionaryHash();

            gb = new GameBoard();
            gb.Roll();

        }

        private void List_Click(object sender, RoutedEventArgs e) {

            while (!MainPage.dictionary.Ready) {
                  Thread.Sleep(10);
            }
            wordList = gb.ResolveWords(MainPage.dictionary, gb.GetCurrentSet());
            WordList.ItemsSource = wordList;
            NumberOfWords.DataContext = wordList;

        }

        private void Reset_Click(object sender, RoutedEventArgs e) {
            gb.Roll();
        }
    }
}