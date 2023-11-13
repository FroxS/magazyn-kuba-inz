using System;
using System.Windows.Input;

namespace Warehouse.Core.Helpers
{
    public class RelayCommand<T> : ICommand
    {
        #region Private Properties

        protected readonly Predicate<T> _canExecute;
        protected readonly Action<T> _execute;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<T>? execute): this(execute, null)
        {
            _execute = execute;
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            _execute = execute;
            _canExecute = canExecute;
        }

        public virtual bool CanExecute(object? parameter) => _canExecute == null || _canExecute((T)parameter);

        public virtual void Execute(object? parameter) => _execute((T)parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action execute) : base((o) => execute(), (o) => true)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute) : base((o) => execute(), (o) => canExecute())
        {
            
        }
    }

    public class AsyncRelayCommand : ICommand
    {
        #region Private Properties

        private readonly Action<Exception>? _onException;

        private bool _isExecuting;

        private readonly Func<Task> _callback;

        protected readonly Predicate<object> _canExecute;

        #endregion

        #region Public properties

        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                _isExecuting = value;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Constructors

        public AsyncRelayCommand(Func<Task> callback, Action<Exception>? onException = null)
        {
            _onException = onException;
            _callback = callback;
        }

        public AsyncRelayCommand(Func<Task> callback, Predicate<object> canExecute, Action<Exception>? onException = null)
        {
            _onException = onException;
            _callback = callback;
            _canExecute = canExecute;
        }

        #endregion

        #region Private methods

        protected async Task ExecuteAsync(object parameter)
        {
            await _callback();
        }

        #endregion

        #region Public methods

        public bool CanExecute(object parameter) => !IsExecuting && (_canExecute == null || _canExecute.Invoke(parameter));

        public async void Execute(object parameter)
        {
            IsExecuting = true;
            try
            {
                await ExecuteAsync(parameter);
            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }

            IsExecuting = false;
        }

        #endregion

    }

    public class AsyncRelayCommand<T> : ICommand
    {
        #region Private Properties

        protected readonly Predicate<T>? _canExecute;

        private readonly Action<Exception>? _onException;

        private bool _isExecuting;

        private readonly Func<T,Task> _callback;

        #endregion

        #region Public properties

        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                _isExecuting = value;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Constructors

        public AsyncRelayCommand(Func<T, Task> callback, Predicate<T>? canExecute = null, Action<Exception>? onException = null)
        {
            _onException = onException;
            _callback = callback;
        }

        #endregion

        #region Private methods

        protected async Task ExecuteAsync(object parameter)
        {
            if(parameter is T par)
            {
                await _callback(par);
            }
            else
            {
                await _callback(default(T));
            }
        }

        #endregion

        #region Public methods

        public bool CanExecute(object parameter) => !IsExecuting;

        public async void Execute(object parameter)
        {
            IsExecuting = true;

            try
            {
                await ExecuteAsync(parameter);
            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }

            IsExecuting = false;
        }

        #endregion

    }

}
