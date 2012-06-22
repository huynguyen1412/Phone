using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace TicTacToe {
    public partial class MainPage : PhoneApplicationPage
    {
        private GameAI gameEngine;
        private GameAI.value state;
        GameAI.opponent player = GameAI.opponent.X, computer = GameAI.opponent.O;

        private bool gameOver;
        private IsolatedStorageSettings appSettings;
        private const String playerWins = "PlayerWins";
        private const String phoneWins = "PhoneWins";
        private const String draws = "Draws";


        public String Stats {
            get { return (String)GetValue(StatsProperty); }
            set { SetValue(StatsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ComputerMoveFirst.  This enables animation, styling, binding, etc... 
        public static readonly DependencyProperty StatsProperty =
                DependencyProperty.Register("Stats", typeof(String), typeof(MainPage), null);
         
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
            DataContext = this;
            InitializeSettings();

            gameEngine = new GameAI();
            ResetGame();
            DisplayResults(GameAI.value.draw);

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

            DisplayResults(v);
            return v;
        }

        void DisplayResults(GameAI.value v) {

            // update screen
            if (v == GameAI.value.oWins)
                GameResult.Text = "O Wins!";
            else if (v == GameAI.value.xWins)
                GameResult.Text = "X Wins!";
            else if (gameOver == true)
                GameResult.Text = "Draw!";

            // Update settings
            UpdateSettings(v);
        }
        void InitializeSettings() {

            appSettings = IsolatedStorageSettings.ApplicationSettings;

            if (appSettings.Contains(playerWins) == false) {
                appSettings.Add(playerWins, 0);
            }

            if (appSettings.Contains(phoneWins) == false) {
                appSettings.Add(phoneWins, 0);
            }

            if (appSettings.Contains(draws) == false) {
                appSettings.Add(draws, 0);
            }
        }
        void UpdateSettings(GameAI.value v) {

            if (ComputerMoveFirst == true) {
                if (v == GameAI.value.xWins) {
                    UpdateStorage(phoneWins);
                }
                else if (v == GameAI.value.oWins) {
                    UpdateStorage(playerWins);
                }
            }
            else {
                if (v == GameAI.value.xWins) {
                    UpdateStorage(playerWins);
                }
                else if (v == GameAI.value.oWins) {
                    UpdateStorage(phoneWins);

                }
            }

            if (v == GameAI.value.draw && gameOver == true) {
                UpdateStorage(draws);
            }

            int p, c, d;
            appSettings.TryGetValue<int>(playerWins, out p);
            appSettings.TryGetValue<int>(phoneWins, out c);
            appSettings.TryGetValue<int>(draws, out d);

            Stats = "Player:" + p + " Phone:" + c + " Draw:" + d;
        }
        void UpdateStorage(string key) {

            int value = 0;
            appSettings.TryGetValue<int>(key, out value);
            value++;
            appSettings[key] = value;;
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

            Random rndNumber = new Random();

            int bestRow = (rndNumber.Next() % 3);
            int bestColumn = (rndNumber.Next() % 3);
            Debug.WriteLine("bestRow:" + bestRow + "bestCol:" + bestColumn);
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
            } else {
                appBarButton.IconUri = new Uri("/Images/comp.png", UriKind.Relative);
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
    }
}