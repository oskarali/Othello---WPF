using System.Windows;

namespace Othello.Views
{
	/// <summary>
	/// dialog that shows the winner and final scores
	/// </summary>
	public partial class WinnerDialog : Window
	{

		/// <summary>
		/// builds the dialog and fills in the winner and score text
		/// </summary>
		/// <param name="winnerName"> name of the winning player </param>
		/// <param name="blackScore"> final score for black </param>
		/// <param name="whiteScore"> final score for white </param>
		public WinnerDialog(string winnerName, int blackScore, int whiteScore)
		{
			InitializeComponent();

			
			txtWinnerMessage.Text = "Winner: " + winnerName + "!";

			
			txtBlackScore.Text = blackScore.ToString();
			txtWhiteScore.Text = whiteScore.ToString();
		}
	}
}
