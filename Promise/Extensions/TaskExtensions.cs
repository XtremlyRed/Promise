using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Threading;

/// <summary>
/// task  extensions
/// </summary>
/// 2024/1/29 14:01
public static class TaskExtensions
{
    /// <summary>
    /// Gets the awaiter.
    /// </summary>
    /// <param name="tasks">The tasks.</param>
    /// <returns></returns>
    /// 2023/12/12 13:52
    public static TaskAwaiter GetAwaiter(this TimeSpan tasks)
    {
        return Task.Delay(tasks).GetAwaiter();
    }

    /// <summary>
    /// Gets the awaiter.
    /// </summary>
    /// <param name="tasks">The tasks.</param>
    /// <returns></returns>
    /// 2023/11/27 8:02
    public static TaskAwaiter GetAwaiter(this IEnumerable<Task> tasks)
    {
        _ = tasks ?? throw new ArgumentNullException(nameof(tasks));

        return Task.WhenAll(tasks).GetAwaiter();
    }

    /// <summary>
    /// Gets the awaiter.
    /// </summary>
    /// <param name="tasks">The tasks.</param>
    /// <returns></returns>
    /// 2023/11/27 8:02
    public static TaskAwaiter<T[]> GetAwaiter<T>(this IEnumerable<Task<T>> tasks)
    {
        _ = tasks ?? throw new ArgumentNullException(nameof(tasks));

        return Task.WhenAll(tasks).GetAwaiter();
    }

    /// <summary>
    /// Gets the awaiter.
    /// </summary>
    /// <param name="taskFuncs">The task funcs.</param>
    /// <returns></returns>
    /// 2023/11/27 8:02
    public static TaskAwaiter GetAwaiter<T>(this IEnumerable<Func<T>> taskFuncs)
        where T : Task
    {
        _ = taskFuncs ?? throw new ArgumentNullException(nameof(taskFuncs));

        var awaiters = new List<Task>();

        foreach (var taskFunc in taskFuncs)
        {
            if (taskFunc is null)
            {
                continue;
            }

            var opt = new TaskOpt<T>(taskFunc);

            awaiters.Add(opt.TaskCompletion.Task);

            ThreadPool.QueueUserWorkItem(
                static async t =>
                {
                    try
                    {
                        await ((TaskOpt<T>)t!).Func();

                        ((TaskOpt<T>)t).TaskCompletion.TrySetResult(true);
                    }
                    catch (Exception ex)
                    {
                        ((TaskOpt<T>)t!).TaskCompletion.TrySetException(ex);
                    }
                },
                opt
            );
        }

        return Task.WhenAll(awaiters).GetAwaiter();
    }

    record TaskOpt<T>(Func<T> Func)
        where T : Task
    {
        public TaskCompletionSource<bool> TaskCompletion = new TaskCompletionSource<bool>();
    }
}
