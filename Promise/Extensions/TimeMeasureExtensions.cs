using System.Diagnostics;

namespace System;

/// <summary>
///
/// </summary>
/// 2024/1/29 14:00
public static class TimeMeasureExtensions
{
    /// <summary>
    /// a stopwatch is used to obtain the execution time of a program
    /// </summary>
    /// <param name="invoker"></param>
    /// <param name="timerCallback"></param>
    public static void TimeMeasure(Action invoker, Action<int> timerCallback)
    {
        _ = timerCallback ?? throw new ArgumentNullException(nameof(timerCallback));
        _ = invoker ?? throw new ArgumentNullException(nameof(invoker));

        Stopwatch stop = Stopwatch.StartNew();
        try
        {
            invoker.Invoke();
        }
        finally
        {
            stop.Stop();
            timerCallback.Invoke((int)stop.ElapsedMilliseconds);
        }
    }

    /// <summary>
    /// a stopwatch is used to obtain the execution time of a program
    /// </summary>
    /// <param name="invoker"></param>
    /// <param name="timerCallback"></param>
    public static T TimeMeasure<T>(this Func<T> invoker, Action<int> timerCallback)
    {
        _ = timerCallback ?? throw new ArgumentNullException(nameof(timerCallback));
        _ = invoker ?? throw new ArgumentNullException(nameof(invoker));

        Stopwatch stop = Stopwatch.StartNew();
        try
        {
            return invoker.Invoke();
        }
        finally
        {
            stop.Stop();
            timerCallback.Invoke((int)stop.ElapsedMilliseconds);
        }
    }

    /// <summary>
    /// a stopwatch is used to obtain the execution time of a program
    /// </summary>
    /// <param name="invoker"></param>
    /// <param name="timerCallback"></param>
    public static async Task TimeMeasureAsync(Func<Task> invoker, Action<int> timerCallback)
    {
        _ = timerCallback ?? throw new ArgumentNullException(nameof(timerCallback));
        _ = invoker ?? throw new ArgumentNullException(nameof(invoker));

        Stopwatch stop = Stopwatch.StartNew();

        try
        {
            await invoker.Invoke();
        }
        finally
        {
            stop.Stop();
            timerCallback.Invoke((int)stop.ElapsedMilliseconds);
        }
    }

    /// <summary>
    /// a stopwatch is used to obtain the execution time of a program
    /// </summary>
    /// <param name="invoker"></param>
    /// <param name="timerCallback"></param>
    public static async Task<T> TimeMeasureAsync<T>(Func<Task<T>> invoker, Action<int> timerCallback)
    {
        _ = timerCallback ?? throw new ArgumentNullException(nameof(timerCallback));
        _ = invoker ?? throw new ArgumentNullException(nameof(invoker));

        Stopwatch stop = Stopwatch.StartNew();
        try
        {
            return await invoker.Invoke();
        }
        finally
        {
            stop.Stop();
            timerCallback.Invoke((int)stop.ElapsedMilliseconds);
        }
    }
}
