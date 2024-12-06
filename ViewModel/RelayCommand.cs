using System;
using System.Windows.Input;

namespace InformacniSystemBanky.ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _executeWithParameter;
        private readonly Action _executeWithoutParameter;
        private readonly Func<object, bool> _canExecuteWithParameter;
        private readonly Func<bool> _canExecuteWithoutParameter;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _executeWithoutParameter = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecuteWithoutParameter = canExecute;
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _executeWithParameter = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecuteWithParameter = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteWithParameter != null)
                return _canExecuteWithParameter(parameter);
            return _canExecuteWithoutParameter == null || _canExecuteWithoutParameter();
        }

        public void Execute(object parameter)
        {
            if (_executeWithParameter != null)
                _executeWithParameter(parameter);
            else
                _executeWithoutParameter();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
