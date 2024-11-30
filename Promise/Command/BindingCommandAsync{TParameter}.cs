using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Promise;

/// <summary>
/// <see cref="IBindingCommandAsync{TParameter}"/>
/// </summary>
/// <typeparam name="TParameter"></typeparam>
public interface IBindingCommandAsync<in TParameter> : ICommand
{
    /// <summary>
    /// can execute command
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    bool CanExecute(TParameter parameter);

    /// <summary>
    /// execute command async
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>

    Task ExecuteAsync(TParameter parameter);

    /// <summary>
    /// is executing
    /// </summary>
    bool IsExecuting { get; }
}

/// <summary>
/// <see cref="BindingCommandAsync{TParameter}"/>
/// </summary>
/// <typeparam name="TParameter"></typeparam>
public class BindingCommandAsync<TParameter> : BindingCommandBase<TParameter>, IBindingCommandAsync<TParameter>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Func<TParameter, Task> execute;

    /// <summary>
    /// create a new command
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public BindingCommandAsync(Func<TParameter, Task> execute, Func<TParameter, bool>? canExecute = null)
        : base(canExecute)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
    }

    /// <summary>
    /// Executes the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    protected override async void _Execute(TParameter parameter)
    {
        await ExecuteAsync(parameter);
    }

    /// <summary>
    /// can execute
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute(TParameter parameter)
    {
        return _CanExecute(parameter);
    }

    /// <summary>
    /// execute command with <typeparamref name="TParameter"/> <paramref name="parameter"/> async
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public async Task ExecuteAsync(TParameter parameter)
    {
        try
        {
            IsExecuting = true;

            RaiseCanExecuteChanged();

            await execute(parameter);
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
    ///
    /// </summary>
    /// <param name="commandAction"></param>
    public static implicit operator BindingCommandAsync<TParameter>(Func<TParameter, Task> commandAction)
    {
        return new BindingCommandAsync<TParameter>(commandAction);
    }
}
