using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace TicTacToe {
    public partial class MainPage : PhoneApplicationPage {

        GameAI gameEngine;
        GameAI.opponent whoMovesNext = GameAI.opponent.human;

        public bool ComputerMoveFirst {
            get { return (bool)GetValue(ComputerMoveFirstProperty); }
            set { SetValue(ComputerMoveFirstProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ComputerMoveFirst.  This enables animation, styling, binding, etc... 
        public static readonly DependencyProperty ComputerMoveFirstProperty =
                DependencyProperty.Register("ComputerMoveFirst", typeof(bool), typeof(MainPage), new PropertyMetadata(false));

        // Constructor
        public MainPage() {
            InitializeComponent();
            ContentPanel.DataContext = this;
            gameEngine = new GameAI();
            gameEngine.ResetBoard();
        }

        private void Square_Clicked(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Debug.WriteLine("Button " + b.Name + "Source");

            int row=0, col=0;
            GetRowColFromName(b.Name, ref row, ref col);


            GameAI.opponent player = GameAI.opponent.X, computer = GameAI.opponent.O;
            GameAI.value alpha = GameAI.value.oWins, beta = GameAI.value.xWins, winner = GameAI.value.unclear;

            SetSelectedGameSquare(b, player, row, col);

            if(ComputerMoveFirst == true) {
                player = GameAI.opponent.O; computer = GameAI.opponent.X;
            }

            int br=0, bc=0;
            GameAI.value res;

            whoMovesNext = GameAI.opponent.machine;

            res = gameEngine.GenerateMove(computer, ref br, ref bc, alpha, beta);
            
            
        }

        void GetRowColFromName(String name, ref int row, ref int col) {

            // Row/Col are the last two characters
            String s = name.Substring(2);
            short b = Convert.ToInt16(s);
            row = (b / 10);
            col = (b % 10);
        }

        bool SetSelectedGameSquare(Button button, GameAI.opponent toOpponent, int row, int col) {

            if(gameEngine.IsPositionEmpty(row, col) == false)
                return false;

            String imagePath;

            if(toOpponent == GameAI.opponent.X)
                imagePath = "/Images/X.png";
            else
                imagePath = "/Images/O.png";

            Image content = button.Content as Image;
            if(null != content) {
                content.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            }
            return true;
        }

        private void ComputerFirstOption_Checked(object sender, RoutedEventArgs e) {
            ComputerMoveFirst ^= true;
            Debug.WriteLine("Checked:" + ComputerMoveFirst);
        }

    }
}