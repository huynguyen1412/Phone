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
        bool gameOver;
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
            gameOver = false;
            gameEngine = new GameAI();
            gameEngine.ResetBoard();
        }

        private void Square_Clicked(object sender, RoutedEventArgs e)
        {

            if (gameOver == true)
                return;

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

            int bestRow=0, bestColumn=0;
            GameAI.value res;

            whoMovesNext = GameAI.opponent.machine;
            res = gameEngine.GenerateMove(computer, ref bestRow, ref bestColumn, alpha, beta);

            b = FindButton(bestRow, bestColumn);

            if (b != null) {
                SetSelectedGameSquare(b, computer, bestRow, bestColumn);
            }

            GameAI.value v = EvaulateGame();

            Debug.WriteLine("Evaluate:" + v);
        }


        GameAI.value EvaulateGame() {

            GameAI.value v = GameAI.value.draw;

            switch (gameEngine.evaluate()) {
		    case GameAI.value.draw:{
			    if (gameEngine.GetBoardState() == GameAI.boardState.fullBoard) {
				    gameOver = true;
                    v = GameAI.value.draw;
			    }
		    }
		    break;
	
		    case GameAI.value.xWins: {				
			    gameOver = false;
                v = GameAI.value.xWins;
		    }
		    break;

		    case GameAI.value.oWins:	{
			    gameOver = false;
                v = GameAI.value.oWins;
		    }
		    break;
			
		    default:
			    gameOver = false;
			    break;
	    }

            return v;
    }



        void GetRowColFromName(String name, ref int row, ref int col) {

            // Row/Col are the last two characters
            String s = name.Substring(2);
            short b = Convert.ToInt16(s);
            row = (b / 10);
            col = (b % 10);
        }

        Button FindButton(int row, int col) {

            // Generate the button name
            String buttonName = String.Format("GS" + row.ToString() + col.ToString());
            Button button = null;

            object b = ContentPanel.FindName(buttonName);

            if (b is Button) {
                button = (Button)b;
            }

            return button;
        }

        bool SetSelectedGameSquare(Button button, GameAI.opponent toOpponent, int row, int col) {

            if(gameEngine.IsPositionEmpty(row, col) == false)
                return false;

            gameEngine.SetBoardPositionToOwner(row, col, 
                toOpponent == GameAI.opponent.X ? GameAI.slotState.ownedByX: GameAI.slotState.ownedByO);

            // Determine which image to use
            String imagePath;
            if(toOpponent == GameAI.opponent.X)
                imagePath = "/Images/X.png";
            else
                imagePath = "/Images/O.png";

            // Add the image to the button
            button.Content = new Image();
            Image content = button.Content as Image;
            if (null != content) {
                content.Stretch = Stretch.Fill;
                content.Width = button.Width - 10;
                content.Height = button.Height - 10;
                content.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

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