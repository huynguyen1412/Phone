using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace TicTacToe {
    public partial class MainPage : PhoneApplicationPage {

        GameAI gameEngine;
        bool gameOver;
        GameAI.value state;

 
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
            ResetGame();
        }

        private void ResetGame() {
            gameOver = false;
            state = GameAI.value.unclear;
            GameResult.Text = "";

            gameEngine.ResetBoard();
            Button b;

            for (int i=0; i < gameEngine.NumberOfRows(); i++) {
                for (int x=0; x < gameEngine.NumberOfColumns(); x++) {
                    b = FindButton(i,x);
                    b.Content = null;
                }
            }
        }

        private void Square_Clicked(object sender, RoutedEventArgs e)
        {
            if (gameOver == true)
                return;

            Button b = (Button)sender;
            Debug.WriteLine("Button " + b.Name + "Source");

            GameAI.opponent player = GameAI.opponent.X, computer = GameAI.opponent.O;
            GameAI.value alpha = GameAI.value.oWins, beta = GameAI.value.xWins;

            if(ComputerMoveFirst == true) {
                player = GameAI.opponent.O; computer = GameAI.opponent.X;
            }

            int row = 0, col = 0;
            GetRowColFromName(b.Name, ref row, ref col);
            SetSelectedGameSquare(b, player, row, col);
                                                                                                                                       
            int bestRow=0, bestColumn=0;
            GameAI.value res;

            res = gameEngine.GenerateMove(computer, ref bestRow, ref bestColumn, alpha, beta);

            b = FindButton(bestRow, bestColumn);

            if (b != null) {
                SetSelectedGameSquare(b, computer, bestRow, bestColumn);
            }

            state = EvaulateGame();

            Debug.WriteLine("Evaluate:" + state);
            Debug.WriteLine("Checked:" + ComputerMoveFirst);
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
			    gameOver = true;
                v = GameAI.value.xWins;
		    }
		    break;

		    case GameAI.value.oWins:	{
			    gameOver = true;
                v = GameAI.value.oWins;
		    }
		    break;
			
		    default:
			    gameOver = false;
			    break;
	    }

            if(v == GameAI.value.oWins)
                GameResult.Text = "O Wins!";
            else if(v == GameAI.value.xWins)
                GameResult.Text = "X Wins!";
            else if(gameOver == true)
                GameResult.Text = "Draw!";
        
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

        private void ComputerGoesFirst_Click(object sender, EventArgs e) {

            int bestRow = 0, bestColumn = 0;
            gameEngine.GenerateMove(GameAI.opponent.X, ref bestRow, ref bestColumn, GameAI.value.oWins, GameAI.value.xWins);

            Button b = FindButton(bestRow, bestColumn);

            if (b != null) {
                SetSelectedGameSquare(b, GameAI.opponent.X, bestRow, bestColumn);
            }

            Debug.WriteLine("Checked:" + ComputerMoveFirst);
        }

         private void ApplicationBarIconButton_AI_Opponent(object sender, EventArgs e) {
            ComputerMoveFirst ^= true;

            ApplicationBarIconButton appBarButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            if(ComputerMoveFirst == true) {
                appBarButton.IconUri = new Uri("/Images/person.png", UriKind.Relative);
                appBarButton.Text = "Opponent";
            } else {
                appBarButton.IconUri = new Uri("/Images/comp.png", UriKind.Relative);
                appBarButton.Text = "Phone";
            }

            // Opponent was changed, restart the game.
            ApplicationBarIconButton_Restart(sender, e);
        }

        private void ApplicationBarIconButton_Restart(object sender, EventArgs e) {
            ResetGame();

            if(gameOver == false) {
                if(ComputerMoveFirst) {
                    ComputerGoesFirst_Click(sender, e);
                }
            }
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e) {

        }
    }
}