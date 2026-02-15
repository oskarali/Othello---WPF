using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Othello.Commands;
using Othello.Controllers;
using Othello.Models;
using Othello.Views;

namespace Othello.ViewModel
{
    /// <summary>
    /// ViewModel for the main Othello game window.
    /// provides data bindings and command logic for the UI.
    ///</summary>
    public class GameWindowViewModel : INotifyPropertyChanged
    {
        private GameManager gameManager;
        private Disk[,] boardState;
        private string player1Name;
        private string player2Name;
        private int blackScore;
        private int whiteScore;
        private string currentPlayerName;
        private bool isGameActive;

        /// <summary>
        /// Represents the current state of the Othello board.
        /// bound to the UI board display.
        /// </summary>
        public Disk[,] BoardState
        {
            get { return boardState; }
            set
            {
                boardState = value;
                OnPropertyChanged("BoardState");
            }
        }

        /// <summary>
        /// Player 1 name. 
        /// </summary>
        public string Player1Name
        {
            get { return player1Name; }
            set
            {
                player1Name = value;
                OnPropertyChanged("Player1Name");
            }
        }
        /// <summary>
        /// Player 2 name
        /// </summary>
        public string Player2Name
        {
            get { return player2Name; }
            set
            {
                player2Name = value;
                OnPropertyChanged("Player2Name");
            }
        }

        /// <summary>
        /// The current score for the black player.
        /// </summary>
        public int BlackScore
        {
            get { return blackScore; }
            set
            {
                blackScore = value;
                OnPropertyChanged("BlackScore");
            }
        }

        /// <summary>
        /// The current score for the white player.
        /// </summary>
        public int WhiteScore
        {
            get { return whiteScore; }
            set
            {
                whiteScore = value;
                OnPropertyChanged("WhiteScore");
            }
        }

        /// <summary>
        /// The name of the players whose turn it currently is.
        /// </summary>
        public string CurrentPlayerName
        {
            get { return currentPlayerName; }
            set
            {
                currentPlayerName = value;
                OnPropertyChanged("CurrentPlayerName");
            }
        }

        /// <summary>
        /// Indicates if the game is currently active.
        /// used to enable or disable certain UI elements.
        /// </summary>
        public bool IsGameActive
        {
            get { return isGameActive; }
            set
            {
                isGameActive = value;
                OnPropertyChanged("IsGameActive");
            }
        }

        /// <summary>
        /// command bound to the "New Game" button in the UI.
        /// used to start a new game.
        /// </summary>
        public ICommand NewGameCommand { get; set; }

        /// <summary>
        /// command to the "Exit" button in the UI.
        /// Used to close the application.
        /// </summary>
        public ICommand ExitCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindowViewModel"/> class.
        /// Sets up events handlers, commands and default values.
        /// </summary>
        public GameWindowViewModel()
        {
            gameManager = new GameManager();
            gameManager.BoardUpdated += GameManager_BoardUpdated;
            gameManager.GameOver += GameManager_GameOver;

            NewGameCommand = new RelayCommand(StartNewGame);
            ExitCommand = new RelayCommand(ExitApplication);

            IsGameActive = false;
            Player1Name = "Player 1";
            Player2Name = "Player 2";
            BlackScore = 2;
            WhiteScore = 2;
        }

        /// <summary>
        /// Start a new Othello game by opening the setup dialog
        /// and initalizing the player and game board.
        /// </summary>
        private void StartNewGame()
        {
            SetupGameDialog setupDialog = new SetupGameDialog();
            if (setupDialog.ShowDialog() == true)
            {
                
                Player1Name = setupDialog.Player1Name;
                Player2Name = setupDialog.Player2Name;

                
                Player player1;
                Player player2;

                if (setupDialog.IsPlayer1Human)
                {
                    player1 = new HumanPlayer { Name = Player1Name };
                }
                else
                {
                    player1 = new ComputerPlayer { Name = Player1Name };
                }

                if (setupDialog.IsPlayer2Human)
                {
                    player2 = new HumanPlayer { Name = Player2Name };
                }
                else
                {
                    player2 = new ComputerPlayer { Name = Player2Name };
                }

                
                gameManager.StartGame(player1, player2);
                IsGameActive = true;
                CurrentPlayerName = player1.Name;

                
                RunGameLoop();
            }
        }

        /// <summary>
        /// runs the main asynchronous game loop while the game is active.
        /// Updates the current player and board state automatically.
        /// </summary>
        private async void RunGameLoop()
        {
            while (IsGameActive)
            {
                await gameManager.PlayTurnAsync();

                if (gameManager.GetCurrentPlayer() != null)
                {
                    CurrentPlayerName = gameManager.GetCurrentPlayer().Name;
                }

                UpdateScores();
            }
        }

        /// <summary>
        /// Updates the players scores based on the state of the game board.
        ///</summary>
        private void UpdateScores()
        {
            if (BoardState != null)
            {
                int blackCount = 0;
                int whiteCount = 0;

                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        if (BoardState[row, col] == Disk.Black)
                        {
                            blackCount++;
                        }
                        else if (BoardState[row, col] == Disk.White)
                        {
                            whiteCount++;
                        }
                    }
                }

                BlackScore = blackCount;
                WhiteScore = whiteCount;
            }
        }

        /// <summary>
        /// Exit application when the "Exit button is pressed.
        /// </summary>
        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Gets the current player from the <see cref="GameManager"/>.
        /// </summary>
        /// <returns>The player whose turn it is.</returns>
        public Player GetCurrentPlayer()
        {
            return gameManager.GetCurrentPlayer();
        }

        /// <summary>
        /// Returns the current <see cref="GameManager"/> instance.
        ///</summary>
        /// <returns>The game manager controlling the logic.</returns>
        public GameManager GetGameManager()
        {
            return gameManager;
        }

        /// <summary>
        /// Handles the <see cref="GameManager.BoardUpdated"/> event.
        /// Updates the UI-bound <see cref="BoardState"/>
        ///</summary>
        /// <param name="newBoardState">The new board state to display.</param>
        private void GameManager_BoardUpdated(Disk[,] newBoardState)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                BoardState = newBoardState;
                UpdateScores();
            });
        }

        /// <summary>
        /// Handles the <see cref="GameManager.GameOver"/> event.
        /// Displays the winner or a draw when the game is finished. 
        /// </summary>
        /// <param name="winner">The winning player, or null if it is a draw.</param>
        /// <param name="blackscore">The final score of the black player.</param>
        /// <param name="whitescore">The final score of the white player.</param>
        private void GameManager_GameOver(Player winner, int blackScore, int whiteScore)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                IsGameActive = false;

                if (winner == null)
                {
                    
                    DrawnDialog drawnDialog = new DrawnDialog(blackScore, whiteScore);
                    drawnDialog.ShowDialog();
                }
                else
                {
                    
                    WinnerDialog winnerDialog = new WinnerDialog(winner.Name, blackScore, whiteScore);
                    winnerDialog.ShowDialog();
                }
            });
        }

        /// <summary>
        /// Occurs when a bound property is changed.
        /// Required for data binding updates in WPF.
        ///</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies listeners that a property value have been changed.
        ///</summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
