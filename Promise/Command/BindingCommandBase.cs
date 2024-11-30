using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using PropertyChanged;

namespace Promise;

/// <summary>
/// a class of <see cref="BindingCommandBase{T}"/>
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class BindingCommandBase<T> : ICommand, INotifyPropertyChanged
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly PropertyChangedEventArgs IsExecutingProperty = new(nameof(IsExecuting));

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Func<T, bool> canExecute;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool isExecuting;

    /// <summary>
    /// The synchronization context
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected SynchronizationContext SynchronizationContext = SynchronizationContext.Current!;

    /// <summary>
    ///
    /// </summary>
    /// <param name="canExecute"></param>
    protected BindingCommandBase(Func<T, bool>? canExecute)
    {
        this.canExecute = canExecute ?? (i => true);
    }

    /// <summary>
    /// is executing
    /// </summary>
    public bool IsExecuting
    {
        get => isExecuting;
        protected set
        {
            if (isExecuting != value)
            {
                isExecuting = value;
                PropertyChanged?.Invoke(this, IsExecutingProperty);
            }
        }
    }

    /// <summary>
    /// Raises the can execute changed.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// can execute changed event
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// property changed event
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    ///
    /// <summary>
    /// raise property changed
    /// </summary>
    /// <param name="propertyName"></param>
    protected virtual void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="parameter"></param>
    bool ICommand.CanExecute(object? parameter)
    {
        return _CanExecute(parameter is T target ? target : default!);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="parameter"></param>
    void ICommand.Execute(object? parameter)
    {
        _Execute(parameter is T target ? target : default!);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns>
    /// </returns>
    public virtual bool _CanExecute(T parameter)
    {
        if (IsExecuting)
        {
            return false;
        }
        return this.canExecute(parameter);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="parameter"></param>
    protected abstract void _Execute(T parameter);
}
