using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Othello.Models;
using Othello.ViewModel;

namespace Othello
{
    /// <summary>
    /// main game window and board rendering
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// viewmodel bound to the window
        /// </summary>
        private GameWindowViewModel viewModel;

        /// <summary>
        /// size of each board cell in pixels
        /// </summary>
        private const int CellSize = 60;

        /// <summary>
        /// sets up bindings and event handlers
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            
            viewModel = new GameWindowViewModel();
            this.DataContext = viewModel;

            
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        /// <summary>
        /// redraws the board when the viewmodel updates
        /// </summary>
        /// <param name="sender"> event source </param>
        /// <param name="e"> property change args </param>
        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "BoardState")
            {
                DrawBoard();
            }
        }

        /// <summary>
        /// draws the 8x8 grid and disks on the canvas
        /// </summary>
        private void DrawBoard()
        {
            GameCanvas.Children.Clear();

            if (viewModel.BoardState == null)
            {
                return;
            }

            
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    
                    Rectangle cellBorder = new Rectangle
                    {
                        Width = CellSize,
                        Height = CellSize,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        Fill = Brushes.DarkGreen
                    };
                    Canvas.SetLeft(cellBorder, col * CellSize);
                    Canvas.SetTop(cellBorder, row * CellSize);
                    GameCanvas.Children.Add(cellBorder);

                    
                    Disk disk = viewModel.BoardState[row, col];
                    if (disk != Disk.None)
                    {
                        Ellipse diskShape = new Ellipse
                        {
                            Width = CellSize - 10,
                            Height = CellSize - 10,
                            Fill = disk == Disk.Black ? Brushes.Black : Brushes.White,
                            Stroke = Brushes.Gray,
                            StrokeThickness = 2
                        };
                        Canvas.SetLeft(diskShape, col * CellSize + 5);
                        Canvas.SetTop(diskShape, row * CellSize + 5);
                        GameCanvas.Children.Add(diskShape);
                    }
                }
            }
        }

        /// <summary>
        /// Handles mouse click events on the game canvas.
        /// Determines which cell has been clicked and attempts to perform a move.
        /// If it is a valid move for the human player.
        /// </summary>
        /// <param name="sender">The canvas receiving the mouse click event.</param>
        /// <param name="e">The mouse event arguments.</param>
        private void GameCanvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!viewModel.IsGameActive)
            {
                return;
            }

            
            Point clickPosition = e.GetPosition(GameCanvas);
            int col = (int)(clickPosition.X / CellSize);
            int row = (int)(clickPosition.Y / CellSize);

            
            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {
                
                Player currentPlayer = viewModel.GetCurrentPlayer();

                if (currentPlayer is HumanPlayer humanPlayer)
                {
                    Tuple<int, int> clickedMove = Tuple.Create(row, col);

                    
                    if (humanPlayer.ValidMoves != null && humanPlayer.ValidMoves.Contains(clickedMove))
                    {
                        
                        humanPlayer.SelectedMove = clickedMove;
                    }
                    else
                    {
                        
                        MessageBox.Show("Invalid move! Try again.", "Invalid Move");
                    }
                }
            }
        }
    }
}
