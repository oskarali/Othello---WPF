using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Othello.Models;

namespace Othello.Controllers
{
    /// <summary>
    /// manages a othello game session
    /// </summary>
    public class GameManager
    {
        /// <summary>
        /// the game board
        /// </summary>
        private GameBoard board;

        /// <summary>
        /// black player
        /// </summary>
        private Player player1;

        /// <summary>
        /// white player
        /// </summary>
        private Player player2;

        /// <summary>
        /// player whose turn it is
        /// </summary>
        private Player currentPlayer;

        /// <summary>
        /// raised when the board state changes and notifies GUI
        /// </summary>
        public event Action<Disk[,]> BoardUpdated;

        /// <summary>
        /// event to notify GUI when game has ended. (winner, blackscore, whitescore)
        /// </summary>
        public event Action<Player, int, int> GameOver;

        /// <summary>
        /// creates a new gamemanager with a fresh board
        /// </summary>
        public GameManager()
        {
            board = new GameBoard();
        }

        /// <summary>
        /// starts a new game with two players
        /// </summary>
        /// <param name ="p1"> player one (assigned black) </param>
        /// <param name ="p2"> player two (assigned white) </param>
        public void StartGame(Player p1, Player p2)
        {
            
            player1 = p1;
            player2 = p2;

            
            player1.Disk = Disk.Black;
            player2.Disk = Disk.White;

            
            currentPlayer = player1;

            
            board = new GameBoard();

            if (BoardUpdated != null)
            {
                BoardUpdated.Invoke(board.GetBoardState());
            }
        }

        /// <summary>
        /// plays one turn for the current player
        /// </summary>
        public async Task PlayTurnAsync()
        {
            
            if (board.IsGameOver())
            {
                EndGame();
                return;
            }

            
            List<Tuple<int, int>> validMoves = board.GetValidMoves(currentPlayer.Disk);

            
            if (validMoves.Count == 0)
            {
                SwitchPlayer();
                return;
            }

            Tuple<int, int> move = await Task.Run(() => currentPlayer.RequestMove(board, validMoves));

            
            if (move != null)
            {
                board.ExecuteMove(move.Item1, move.Item2, currentPlayer.Disk);

                
                if (BoardUpdated != null)
                {
                    BoardUpdated.Invoke(board.GetBoardState());
                }

                
                SwitchPlayer();
            }
        }

        /// <summary>
        /// switches to active player
        /// </summary>
        private void SwitchPlayer()
        {
            if (currentPlayer == player1)
            {
                currentPlayer = player2;
            }
            else
            {
                currentPlayer = player1;
            }
        }

        /// <summary>
        /// ends the game and decides who wins
        /// </summary>
        private void EndGame()
        {
            int blackScore = board.GetScore(Disk.Black);
            int whiteScore = board.GetScore(Disk.White);

            Player winner = null;

            if (blackScore > whiteScore)
            {
                winner = player1; 
            }
            else if (whiteScore > blackScore)
            {
                winner = player2; 
            }

            
            if (GameOver != null)
            {
               GameOver.Invoke(winner, blackScore, whiteScore);
            }
        }

        /// <summary>
        /// returns the current player
        /// </summary>
        /// <returns> returns the player whose turn it is </returns>
        public Player GetCurrentPlayer()
        {
            return currentPlayer;
        }

        /// <summary>
        /// gets a copy of the current board state
        /// </summary>
        /// <returns> returns the 8x8 disk grid </returns>
        public Disk[,] GetBoardState()
        {
            return board.GetBoardState();
        }
    }
}