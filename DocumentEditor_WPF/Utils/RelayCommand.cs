using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DocumentEditor_WPF.Utils
{
    internal class RelayCommand : ICommand
    {
        private Action _exec;

        public RelayCommand(Action exec)
        {
            _exec = exec;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => parameter is null;

        public void Execute(object? parameter)
        {
            _exec();
        }
    }

    internal class RelayCommand<T> : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action<T> _exec;

        public RelayCommand(Action<T> exec)
        {
            _exec = exec;
        }

        public bool CanExecute(object? parameter)
        {
            if (parameter is null || parameter is not T)
            {
                return false;
            }

            return true;
        }

        public void Execute(object? parameter)
        {
            _exec((T)parameter);
        }
    }
}
