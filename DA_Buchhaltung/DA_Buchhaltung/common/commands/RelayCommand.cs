/*
 * Klasse: RelayCommand.cs
 * Author: Martin Osterwalder >> Bezug aus MSDN und nullskull (siehe weiter unten)
 * Implementierung des ICommand interface, ermöglich das Ausführen von Methoden, welche durch Commands aufgerufen werden
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DA_Buchhaltung.common.commands
{
    /// <summary>
    /// Referenz-Implementation von ICommand
    /// Gemäss MSDN http://msdn.microsoft.com/en-us/magazine/dd419663.aspx#id0090030
    /// Erweitert mit Parameter-Funktionalität http://www.nullskull.com/faq/905/pass-command-parameter-to-relaycommand.aspx
    /// </summary>
    public class RelayCommand<T> : ICommand
    {

        #region Fields

        readonly Action<T> _execute = null;
        readonly Action _executeSimple = null;
        readonly Predicate<T> _canExecute = null;

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action execute) : this(execute, null) { }

        public RelayCommand(Action<T> execute) : this(execute, null) { }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _executeSimple = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public void Execute()
        {
            _executeSimple();
        }

        #endregion // ICommand Members
    }
}
