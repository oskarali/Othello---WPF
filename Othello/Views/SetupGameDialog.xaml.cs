using System.Windows;

namespace Othello.Views
{
    /// <summary>
    /// dialog used to set player names and types before starting a new game
    /// </summary>
    public partial class SetupGameDialog : Window
    {

        /// <summary>
        /// builds the dialog UI
        /// </summary>
        public SetupGameDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// gets the name for player 1
        /// </summary>
        public string Player1Name
        {
            get { return txtPlayer1Name.Text; }
        }

        /// <summary>
        /// gets the name for player 2
        /// </summary>
        public string Player2Name
        {
            get { return txtPlayer2Name.Text; }
        }

        /// <summary>
        /// checks if player 1 is a human
        /// </summary>
        public bool IsPlayer1Human
        {
            get { return rbPlayer1Human.IsChecked == true; }
        }

        /// <summary>
        /// checks if player 2 is a human
        /// </summary>
        public bool IsPlayer2Human
        {
            get { return rbPlayer2Human.IsChecked == true; }
        }

        /// <summary>
        /// handles the ok button click and validates input
        /// </summary>
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(txtPlayer1Name.Text))
            {
                MessageBox.Show("Please enter Player 1 name!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPlayer2Name.Text))
            {
                MessageBox.Show("Please enter Player 2 name!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

           
            this.DialogResult = true;
        }
    }
}
