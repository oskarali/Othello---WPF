using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Othello.Models
{

    /// <summary>
    /// baseclass for all othello players
    /// </summary>
    public abstract class Player
    {

        /// <summary>
        /// players name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// players disk color
        /// </summary>
        public Disk Disk { get; set; }

        /// <summary>
        /// requests a move from player
        /// </summary>
        /// <param name ="board" > current game board </param>
        /// <param name ="validMoves" > list of valid moves availlable </param>
        /// <returns> returns the selected move as a (row, column) tuple </returns>
        public abstract Tuple<int, int> RequestMove(GameBoard board, List<Tuple<int, int>> validMoves);
    }
}
