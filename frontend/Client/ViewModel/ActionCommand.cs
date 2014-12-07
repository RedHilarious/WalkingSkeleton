﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.ViewModel
{
    /// <summary>
    /// Default base class for action commands in the MVVM pattern.
    /// </summary>
    class ActionCommand : ICommand
    {
        private readonly Action<object> _executeHandler;
        private readonly Func<object, bool> _canExecuteHandler;

        public ActionCommand(Action<object> execute) : this(execute, null) { }

        public ActionCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("Execute cannot be null");
            }

            _executeHandler = execute;
            _canExecuteHandler = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _executeHandler(parameter);
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteHandler == null)
            {
                return true;
            }

            return _canExecuteHandler(parameter);
        }
    }
}
