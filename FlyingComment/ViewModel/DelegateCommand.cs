using System;
using System.Windows.Input;

namespace FlyingComment.ViewModel
{
    public class DelegateCommand : ICommand
    {
        public Action<object> ExecuteHandler { get; private set; }
        public Func<object, bool> CanExecuteHandler { get; private  set; }

        public DelegateCommand(Action<object> Execute, Func<object, bool> CanExecute)
        {
            ExecuteHandler = Execute;
            CanExecuteHandler = CanExecute;
        }

        #region ICommand メンバー

        public bool CanExecute(object parameter)
        {
            var d = CanExecuteHandler;
            return d == null ? true : d(parameter);
        }

        public void Execute(object parameter)
        {
            var d = ExecuteHandler;
            if (d != null)
                d(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            var d = CanExecuteChanged;
            if (d != null)
                d(this, null);
        }

        #endregion
    }
}
