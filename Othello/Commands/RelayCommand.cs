using System;
using System.Windows.Input; //ICommand

namespace Othello.Commands
{
    /// <summary>
    /// command class that links UI actions to logic
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// creates new command with given action and optional condition
        /// </summary>
        /// <param name ="execute"> the action to run when command is executed </param>
        /// <param name ="canExecute"> optional function that determines if the command can run </param>
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// occurs when the ability of the command to execute changes
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove{ CommandManager.RequerySuggested -= value;}
        }

        /// <summary>
        /// checks if command can be executed
        /// </summary>
        /// <param name ="parameter"> command parameter but not used </param>
        /// <returns> returns true if can be executed otherwise fails </returns>
        public bool CanExecute(object parameter)
        {
            if(_canExecute == null)
            {
                return true;
            }
            else
            {
                return _canExecute();
            }
        }

        /// <summary>
        /// executes the command action
        /// </summary>
        /// <param name ="parameter"> command parameter but not used </param>
        public void Execute(object parameter)
        {
            _execute();
        }
    }
}