using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Promise;

/// <summary>
/// <see cref="IBindingCommandAsync"/>
/// </summary>
public interface IBindingCommandAsync : ICommand
{
    /// <summary>
    /// can execute
    /// </summary>
    /// <returns></returns>
    bool CanExecute();

    /// <summary>
    /// execute command async
    /// </summary>
    /// <returns></returns>

    Task ExecuteAsync();

    /// <summary>
    /// is executing
    /// </summary>
    bool IsExecuting { get; }
}

/// <summary>
/// <see cref="BindingCommandAsync"/>
/// </summary>
public class BindingCommandAsync : BindingCommandBase<object>, IBindingCommandAsync
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Func<Task> execute;

    /// <summary>
    /// create a new command
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public BindingCommandAsync(Func<Task> execute, Func<bool>? canExecute = null)
        : base(canExecute is null ? null! : indexer => canExecute())
    {
        this.execute = execute ?? throw new Exception(nameof(execute));
    }

    /// <summary>
    /// Executes the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    protected override async void _Execute(object parameter)
    {
        await ExecuteAsync();
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
    /// execute command async
    /// </summary>
    /// <returns></returns>
    public async Task ExecuteAsync()
    {
        try
        {
            IsExecuting = true;
            RaiseCanExecuteChanged();

            await execute();
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
    public static implicit operator BindingCommandAsync(Func<Task> commandAction)
    {
        return new BindingCommandAsync(commandAction);
    }
}
