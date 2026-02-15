using System;
using System.Collections.Generic;
using System.Threading;

namespace Othello.Models
{
    // Takes input from human player through GUI
    public class HumanPlayer : Player
    {
        // Selected move from user click
        public Tuple<int, int> SelectedMove { get; set; }

        // Valid moves for current turn
        public List<Tuple<int, int>> ValidMoves { get; set; }

        // Waits for user to click a valid move
        public override Tuple<int, int> RequestMove(GameBoard board, List<Tuple<int, int>> validMoves)
        {
            // Save valid moves
            ValidMoves = validMoves;

            // Reset selected move
            SelectedMove = null;

            // Vänta tills användaren klickar på en ruta
            while (SelectedMove == null)
            {
                Thread.Sleep(100); // vänta 100ms
            }

            return SelectedMove;
        }
    }
}
