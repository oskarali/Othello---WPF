using System.Windows;

namespace Othello.Views
{
    /// <summary>
    /// Dialog shown when the game ends in a draw
    /// </summary>
    public partial class DrawnDialog : Window
    {

        /// <summary>
        /// builds the dialog and displays both player scores
        /// </summary>
        /// <param name ="blackScore"> score for black </param>
        /// <param name ="whiteScore"> score for white </param>
        public DrawnDialog(int blackScore, int whiteScore)
        {
            InitializeComponent();

            
            txtBlackScore.Text = blackScore.ToString();
            txtWhiteScore.Text = whiteScore.ToString();
        }
    }
}
