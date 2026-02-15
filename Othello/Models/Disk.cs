using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.Models
{
    /// <summary>
    /// Represents possible disk states on the othello board
    /// </summary>
    public enum Disk
    {
        /// <summary>
        /// no disk placed
        /// </summary>
        None,

        /// <summary>
        /// black player disk
        /// </summary>
        Black,

        /// <summary>
        /// white player disk
        /// </summary>
        White
    }
}
