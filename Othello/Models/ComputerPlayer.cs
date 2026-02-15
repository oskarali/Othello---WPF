using System.Threading; //thread.sleep
using System; //random

namespace Othello.Models
{
    /// <summary>
    /// Represents a computer-controlled player that make automatic moves.
    /// </summary>
    public class ComputerPlayer : Player
    {
        /// <summary>
        /// select move from valid moves
        /// </summary>
        /// <param name="board">The current state of the game board</param>
        /// <param name="validMoves">A list of valid moves the player can make</param>
        /// <returns>
        /// A tuple containing the selected row and column coordinates for the move.
        ///</returns>
        public override Tuple<int, int> RequestMove(GameBoard board, List<Tuple<int, int>> validMoves)
        {
            Thread.Sleep(1000); //sleep for 1 sec

            Random random = new Random(); //random generator
            int index = random.Next(0, validMoves.Count); //random index
            Tuple<int, int> move = validMoves[index];
            return move;
        }
    }
}   