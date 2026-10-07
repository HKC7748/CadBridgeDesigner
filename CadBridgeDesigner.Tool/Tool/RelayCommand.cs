using System;
using System.Windows.Input;

namespace CadBridgeDesigner.Tool.Tool
{
    public class RelayCommand : ICommand
    {
        private readonly Action _doSomething;
        private readonly Action<object> _doSomethingWithParameter;

        public RelayCommand(Action doSomething)
        {
            _doSomething = doSomething;
        }

        public RelayCommand(Action<object> doSomething)
        {
            _doSomethingWithParameter = doSomething;
        }
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter)
        {
            if (_doSomethingWithParameter != null)
            {
                _doSomethingWithParameter(parameter);
            }
            else
            {
                _doSomething?.Invoke();
            }
        }
    }
}