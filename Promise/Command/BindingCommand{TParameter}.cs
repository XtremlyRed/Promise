using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows.Input;

namespace Promise;

/// <summary>
/// <see cref="IBindingCommand{TParameter}"/>
/// </summary>
/// <typeparam name="TParameter"></typeparam>
public interface IBindingCommand<in TParameter> : ICommand
{
    /// <summary>
    /// can execute command
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    bool CanExecute(TParameter parameter);

    /// <summary>
    /// execute command
    /// </summary>
    /// <param name="parameter"></param>
    void Execute(TParameter parameter);

    /// <summary>
    /// is executing
    /// </summary>
    bool IsExecuting { get; }
}

/// <summary>
/// BindingCommand
/// </summary>
/// <typeparam name="TParameter"></typeparam>
public class BindingCommand<TParameter> : BindingCommandBase<TParameter>, ICommand, IBindingCommand<TParameter>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Action<TParameter> execute;

    /// <summary>
    /// create a new command
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public BindingCommand(Action<TParameter> execute, Func<TParameter, bool>? canExecute = null)
        : base(canExecute)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="parameter"></param>

    protected override void _Execute(TParameter parameter)
    {
        try
        {
            IsExecuting = true;
            RaiseCanExecuteChanged();
            execute.Invoke(parameter);
        }
        catch (Exception ex)
        {
            if (BindingCommand.globalCommandExceptionCallback is null)
            {
                throw;
            }
            BindingCommand.globalCommandExceptionCallback.Invoke(ex);
        }
        finally
        {
            IsExecuting = false;
            RaiseCanExecuteChanged();
        }
    }

    /// <summary>
    /// execute command with <typeparamref name="TParameter"/> <paramref name="parameter"/>
    /// </summary>
    /// <param name="parameter"></param>
    public void Execute(TParameter parameter)
    {
        _Execute(parameter);
    }

    /// <summary>
    /// can execute
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute(TParameter parameter)
    {
        return base._CanExecute(parameter);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="commandAction"></param>
    public static implicit operator BindingCommand<TParameter>(Action<TParameter> commandAction)
    {
        return new BindingCommand<TParameter>(commandAction);
    }
}
