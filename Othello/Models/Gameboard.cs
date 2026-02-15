using System;
using System.Collections.Generic;

namespace Othello.Models
{
	/// <summary>
	/// represents the othello board (8x8) with logic for moves, turns and the current state of the game.
	/// </summary>
	public class GameBoard
	{
		// 8x8 field
		private Disk[,] board;

		/// <summary>
		/// initializes the othello board and places the starting bricks.
		///</summary>
		public GameBoard()
		{
			board = new Disk[8, 8];
			// 8 x 8 array
			for (int row = 0; row < 8; row++)
			{
				for (int col = 0; col < 8; col++)
				{
					board[row, col] = Disk.None;
				}
			}
			//start-disk
			board[3, 3] = Disk.White;
			board[3, 4] = Disk.Black;
			board[4, 3] = Disk.Black;
			board[4, 4] = Disk.White;

		}

		/// <summary>
		/// Gets a copy of the current state of the game board.
		/// </summary>
		/// <returns>
		/// A cloned (8x8) array of <see cref="Disk"/> that can be used by the GUI without altering the original.
		/// </returns>
		public Disk[,] GetBoardState()
		{
			return (Disk[,])board.Clone(); //return copy for the GUI
		}

		/// <summary>
		/// count numbers of disks for a player
		/// </summary>
		/// <param name="player">The player whos bricks are counted (<see cref="Disk.Black"/> or <see cref="Disk.White"/>).</param>
		/// <returns>Number of squares on the board that belongs to the player.</returns>
		public int GetScore(Disk player)
		{
			int count = 0;
			for (int row = 0; row < 8; row++)
			{
				for (int col = 0; col < 8; col++)
				{
					if (board[row, col] == player)
					{ count++; }
				}

			}
			return count;
		}
		/// <summary>
		/// return list of valid moves based don the current position of the player.
		/// </summary>
		/// <param name="player">The player that will do the move</param>
		/// <returns>
		/// A list of position tuples <c>(row, column)</c> representing valid moves.
		/// The list can be empty if no valid moves exist.
		/// </returns>
		public List<Tuple<int, int>> GetValidMoves(Disk player)
		{
			List<Tuple<int, int>> validMoves = new List<Tuple<int, int>>();

			// loop trough all cells on board
			for (int row = 0; row < 8; row++)
			{
				for (int col = 0; col < 8; col++)
				{   //check if cell is empty
					if (board[row, col] == Disk.None)
					{    //check if move at (row, col) captures oppnenets disk 
						if (IsValidMove(row, col, player))
						{
							validMoves.Add(Tuple.Create(row, col));
						}
					}
				}
			}
			return validMoves;
		}
		/// <summary>
		/// checking if placing the disk at a given position is valid.
		/// Avalid move must flip atleast one opposing disk.
		/// </summary>
		/// <param name="row">Board row index (0-7).</param>
		/// <param name="col">Board column index (0-7).</param>
		/// <param name="player">The player attempting the move.</param>
		/// <returns>
		/// <c>true</c> if the move is valid otherwise <c>false</c>.
		/// </returns>
		public bool IsValidMove(int row, int col, Disk player)
		{
			//get opponents color ä
			Disk opponent;
			if (player == Disk.Black)
			{
				opponent = Disk.White;
			}
			else
			{
				opponent = Disk.Black;
			}

			//check all 8 direcetions 
			int[] directions = { -1, 0, 1 };

			foreach (int dRow in directions)
			{
				foreach (int dCol in directions)
				{
					// skip 0,0
					if (dRow == 0 && dCol == 0)
						continue;

					//check if this direction caputures anything 
					if (CheckDirection(row, col, dRow, dCol, player, opponent))
					{
						return true; //found atleast one
					}
				}
			}
			return false;
		}

		/// <summary>
        /// checks one direction for a valid capture chain
        /// </summary>
		/// <param name ="row"> start row </param>
		/// <param name ="col"> start column </param>
		/// <param name ="dRow"> row step </param>
		/// <param name ="dCol"> column step </param>
		/// <param name ="player"> current player </param>
		/// <param name ="opponent"> opponent player </param>
		/// <returns> returns true if the direction gives a capture</returns>
		private bool CheckDirection(int row, int col, int dRow, int dCol, Disk player, Disk opponent)
		{
			int r = row + dRow;
			int c = col + dCol;
			bool foundOpponent = false;

			// move while on board
			while (r >= 0 && r < 8 && c >= 0 && c < 8)
			{
				if (board[r, c] == Disk.None)
				{
					return false; //empty cell
				}
				else if (board[r, c] == opponent)
				{
					foundOpponent |= true; //found opponent disk 
					r += dRow;
					c += dCol;
				}
				else if (board[r, c] == player)
				{
					return foundOpponent;
				}
			}
			return false; //redge edge 
		}

        /// <summary>
        /// places a disk and flips all captured disks
        /// </summary>
        /// <param name ="row"> row index of the move </param>
        /// <param name ="col"> column index of the move </param>
        /// <param name ="player"> the player making the move </param>
        public void ExecuteMove(int row, int col, Disk player)
		{
			//place disk on board
			board[row, col] = player;

			//get opponent color
			Disk opponent;
			if(player == Disk.Black)
			{
				opponent = Disk.White;
			}
			else
			{
				opponent = Disk.Black;	
			}

			//check all 8 derections
			int[] directions = { -1, 0, 1 };

			foreach (int dRow in directions)
			{
				foreach (int dCol in directions)
				{
					//skp 0,0
					if(dRow == 0 && dCol == 0)
					{
						continue;
					}
					FlipDirection(row, col, dRow, dCol, player, opponent);

                }
			}
		}

        /// <summary>
        /// flips opponent disks along one direction starting at a move
        /// </summary>
		/// <param name ="row"> start row </param>
		/// <param name ="col"> start column </param>
		/// <param name ="dRow"> row step </param>
		/// <param name ="dCol"> column step </param>
		/// <param name ="player"> current player </param>
		/// <param name ="opponent"> opponent player </param>
        private void FlipDirection(int row, int col, int dRow, int dCol, Disk player, Disk opponent)
		{
			//check if disk needed to be flipped 
			if(!CheckDirection(row, col, dRow, dCol, player, opponent))
				{ return; }

			//start from next cell
			int r = row + dRow;
			int c = col + dCol;

			// move in direction 
			while(r >= 0 && r < 8 && c >= 0 && c < 8)
			{
				if (board[r, c] == opponent)
				{
					board[r, c] = player; //flip disk
					r += dRow;
					c += dCol;
				}
				else
				{
					break; // stop flipping 
				}
			}
		}

		/// <summary>
		/// checks if the game has ended
		/// </summary>
		/// <returns> returns true if board is full or neither player has a valid move </returns>
		public bool IsGameOver()
		{
			//check if board if full 
			int emptyCells = 0;
			for(int row = 0; row < 8;  row++)
			{
				for(int col = 0; col < 8; col++)
				{
					if (board[row, col] == Disk.None)
					{
						emptyCells++;
					}
				}
			}

			//if no empty cells game over
			if( emptyCells == 0 )
			{ 
				return true; 
			}

			// check if any player has valid moves 
			List<Tuple<int, int>> blackMoves = GetValidMoves(Disk.Black);
            List<Tuple<int, int>> whiteMoves = GetValidMoves(Disk.White);

			//if neither player can move game is over 
			if ( blackMoves.Count == 0 && whiteMoves.Count == 0)
			{
				return true;
			}

			//game continues
			return false;

        }
	}
}
