using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShowValueUnderMouse.Command
{
    public class RelayCommand : ICommand
    {
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            m_Execute = execute ?? throw new ArgumentNullException(nameof(execute));
            m_CanExecute = canExecute;
        }

        private readonly Action m_Execute;
        private readonly Func<bool>? m_CanExecute;

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return m_CanExecute == null || m_CanExecute();
        }

        public void Execute(object? parameter)
        {
            m_Execute.Invoke();
        }
    }
}
