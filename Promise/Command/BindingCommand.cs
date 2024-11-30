using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Promise;

/// <summary>
/// <see cref="IBindingCommand"/>
/// </summary>
public interface IBindingCommand : ICommand
{
    /// <summary>
    /// can execute
    /// </summary>
    /// <returns></returns>
    bool CanExecute();

    /// <summary>
    /// execute command
    /// </summary>
    void Execute();

    /// <summary>
    /// is executing
    /// </summary>
    bool IsExecuting { get; }
}

/// <summary>
/// BindingCommand
/// </summary>
public class BindingCommand : BindingCommandBase<object>, IBindingCommand, ICommand
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Action execute;

    /// <summary>
    /// create a new command
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public BindingCommand(Action execute, Func<bool>? canExecute = null)
        : base(canExecute is null ? null : i => canExecute.Invoke())
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
    }

    /// <summary>
    /// can execute
    /// </summary>
    /// <returns></returns>
    public bool CanExecute()
    {
        return base._CanExecute(default!);
    }

    /// <summary>
    /// Executes the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    protected override void _Execute(object parameter)
    {
        try
        {
            base.IsExecuting = true;

            RaiseCanExecuteChanged();

            execute.Invoke();
        }
        catch (Exception ex)
        {
            if (globalCommandExceptionCallback is null)
            {
                throw;
            }
            globalCommandExceptionCallback.Invoke(ex);
        }
        finally
        {
            IsExecuting = false;
            RaiseCanExecuteChanged();
        }
    }

    /// <summary>
    /// execute sync command
    /// </summary>
    public void Execute()
    {
        _Execute(default!);
    }

    /// <summary>
    ///
    /// </summary>

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static Action<Exception>? globalCommandExceptionCallback;

    /// <summary>
    /// set global command exception callback
    /// </summary>
    /// <param name="globalCommandExceptionCallback"></param>
    public static void SetGlobalCommandExceptionCallback(Action<Exception> globalCommandExceptionCallback)
    {
        BindingCommand.globalCommandExceptionCallback = globalCommandExceptionCallback;
    }

    /// <summary>
    /// create relaycommand from  <see cref="Action"/> <paramref name="commandAction"/>
    /// </summary>
    /// <param name="commandAction"></param>
    public static implicit operator BindingCommand(Action commandAction)
    {
        return new BindingCommand(commandAction);
    }
}
