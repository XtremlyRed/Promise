using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Promise;

/// <summary>
/// a class of <see cref="Defer"/>
/// </summary>
public static class Defer
{
    /// <summary>
    /// creata a new defer task by <paramref name="timeSpan"/>
    /// </summary>
    /// <param name="timeSpan">delay <paramref name="timeSpan"/></param>
    /// <returns></returns>
    public static IDeferredBehavior Deferred(TimeSpan timeSpan)
    {
        _ = (timeSpan <= TimeSpan.Zero) ? throw new ArgumentOutOfRangeException(nameof(timeSpan)) : 0;

        return new DeferredBehavior(timeSpan);
    }

    /// <summary>
    /// creata a new defer task by <paramref name="milliseconds"/>
    /// </summary>
    /// <param name="milliseconds">delay <paramref name="milliseconds"/></param>
    /// <returns></returns>
    public static IDeferredBehavior Deferred(double milliseconds)
    {
        _ = (milliseconds <= 0) ? throw new ArgumentOutOfRangeException(nameof(milliseconds)) : 0;

        return new DeferredBehavior(TimeSpan.FromMilliseconds(milliseconds));
    }

    [DebuggerDisplay("deferred {timeSpan}")]
    class DeferredBehavior : IDeferredBehavior
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan timeSpan;

        internal DeferredBehavior(TimeSpan timeSpan)
        {
            this.timeSpan = timeSpan;
        }

        public IDeferredToken Invoke(Action action, bool invokeInCurrentThread = false, string? deferName = null)
        {
            var behavior = new InvokeActionDeferredBehavior(action, this.timeSpan, invokeInCurrentThread, deferName);

            return new DeferredToken(behavior);
        }

        public IDeferredToken Invoke(Func<Task> taskfunc, bool invokeInCurrentThread = false, string? deferName = null)
        {
            var behavior = new InvokeFuncTaskDeferredBehavior(taskfunc, this.timeSpan, invokeInCurrentThread, deferName);

            return new DeferredToken(behavior);
        }

        public IDeferredToken Invoke<T>(T parameter, Action<T> action, bool invokeInCurrentThread = false, string? deferName = null)
        {
            var behavior = new InvokeActionWithParameterDeferredBehavior<T>(action, parameter, this.timeSpan, invokeInCurrentThread, deferName);

            return new DeferredToken(behavior);
        }

        public IDeferredToken Invoke<T>(T parameter, Func<T, Task> taskfunc, bool invokeInCurrentThread = false, string? deferName = null)
        {
            var behavior = new InvokeFuncTaskWithParameterDeferredBehavior<T>(taskfunc, parameter, this.timeSpan, invokeInCurrentThread, deferName);

            return new DeferredToken(behavior);
        }

        public IDeferredToken Invoke(Action<IDeferredContext> action, bool invokeInCurrentThread = false, string? deferName = null)
        {
            var behavior = new InvokeActionDeferredBehavior2(action, this.timeSpan, invokeInCurrentThread, deferName);

            return new DeferredToken(behavior);
        }

        public IDeferredToken Invoke(Func<IDeferredContext, Task> taskfunc, bool invokeInCurrentThread = false, string? deferName = null)
        {
            var behavior = new InvokeFuncTaskDeferredBehavior2(taskfunc, this.timeSpan, invokeInCurrentThread, deferName);

            return new DeferredToken(behavior);
        }

        public IDeferredToken Invoke<T>(T parameter, Action<IDeferredContext, T> action, bool invokeInCurrentThread = false, string? deferName = null)
        {
            var behavior = new InvokeActionWithParameterDeferredBehavior2<T>(action, parameter, this.timeSpan, invokeInCurrentThread, deferName);

            return new DeferredToken(behavior);
        }

        public IDeferredToken Invoke<T>(T parameter, Func<IDeferredContext, T, Task> taskfunc, bool invokeInCurrentThread = false, string? deferName = null)
        {
            var behavior = new InvokeFuncTaskWithParameterDeferredBehavior2<T>(taskfunc, parameter, this.timeSpan, invokeInCurrentThread, deferName);

            return new DeferredToken(behavior);
        }
    }

    class InvokeActionDeferredBehavior : DeferredBehaviorBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action action;

        public InvokeActionDeferredBehavior(Action action, TimeSpan timeSpan, bool invokeInCurrentThread, string? deferName)
            : base(timeSpan, invokeInCurrentThread, deferName)
        {
            this.action = action;
        }

        protected override void Invoke(IDeferredContext deferredContext)
        {
            action?.Invoke();
        }

        public override void Dispose()
        {
            action = null!;
            base.Dispose();
        }
    }

    class InvokeFuncTaskDeferredBehavior : DeferredBehaviorBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Func<Task> taskfunc;

        public InvokeFuncTaskDeferredBehavior(Func<Task> taskfunc, TimeSpan timeSpan, bool invokeInCurrentThread, string? deferName)
            : base(timeSpan, invokeInCurrentThread, deferName)
        {
            this.taskfunc = taskfunc;
        }

        protected override async void Invoke(IDeferredContext deferredContext)
        {
            if (taskfunc is not null)
            {
                await taskfunc();
            }
        }

        public override void Dispose()
        {
            taskfunc = null!;
            base.Dispose();
        }
    }

    class InvokeActionWithParameterDeferredBehavior<T> : DeferredBehaviorBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action<T> action;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T parameter;

        public InvokeActionWithParameterDeferredBehavior(Action<T> action, T parameter, TimeSpan timeSpan, bool invokeInCurrentThread, string? deferName)
            : base(timeSpan, invokeInCurrentThread, deferName)
        {
            this.action = action;
            this.parameter = parameter;
        }

        protected override void Invoke(IDeferredContext deferredContext)
        {
            if (action is not null)
                action(parameter);
        }

        public override void Dispose()
        {
            action = null!;
            parameter = default!;
            base.Dispose();
        }
    }

    class InvokeFuncTaskWithParameterDeferredBehavior<T> : DeferredBehaviorBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Func<T, Task> taskfunc;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T parameter;

        public InvokeFuncTaskWithParameterDeferredBehavior(Func<T, Task> taskfunc, T parameter, TimeSpan timeSpan, bool invokeInCurrentThread, string? deferName)
            : base(timeSpan, invokeInCurrentThread, deferName)
        {
            this.taskfunc = taskfunc;
            this.parameter = parameter;
        }

        protected override async void Invoke(IDeferredContext deferredContext)
        {
            if (taskfunc is not null)
            {
                await taskfunc(parameter);
            }
        }

        public override void Dispose()
        {
            taskfunc = null!;
            parameter = default!;
            base.Dispose();
        }
    }

    class InvokeActionDeferredBehavior2 : DeferredBehaviorBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action<IDeferredContext> action;

        public InvokeActionDeferredBehavior2(Action<IDeferredContext> action, TimeSpan timeSpan, bool invokeInCurrentThread, string? deferName)
            : base(timeSpan, invokeInCurrentThread, deferName)
        {
            this.action = action;
        }

        protected override void Invoke(IDeferredContext deferredContext)
        {
            action?.Invoke(deferredContext);
        }

        public override void Dispose()
        {
            action = null!;
            base.Dispose();
        }
    }

    class InvokeFuncTaskDeferredBehavior2 : DeferredBehaviorBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Func<IDeferredContext, Task> taskfunc;

        public InvokeFuncTaskDeferredBehavior2(Func<IDeferredContext, Task> taskfunc, TimeSpan timeSpan, bool invokeInCurrentThread, string? deferName)
            : base(timeSpan, invokeInCurrentThread, deferName)
        {
            this.taskfunc = taskfunc;
        }

        protected override async void Invoke(IDeferredContext deferredContext)
        {
            if (taskfunc is not null)
            {
                await taskfunc(deferredContext);
            }
        }

        public override void Dispose()
        {
            taskfunc = null!;
            base.Dispose();
        }
    }

    class InvokeActionWithParameterDeferredBehavior2<T> : DeferredBehaviorBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action<IDeferredContext, T> action;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T parameter;

        public InvokeActionWithParameterDeferredBehavior2(Action<IDeferredContext, T> action, T parameter, TimeSpan timeSpan, bool invokeInCurrentThread, string? deferName)
            : base(timeSpan, invokeInCurrentThread, deferName)
        {
            this.action = action;
            this.parameter = parameter;
        }

        protected override void Invoke(IDeferredContext deferredContext)
        {
            if (action is not null)
                action(deferredContext, parameter);
        }

        public override void Dispose()
        {
            action = null!;
            parameter = default!;
            base.Dispose();
        }
    }

    class InvokeFuncTaskWithParameterDeferredBehavior2<T> : DeferredBehaviorBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Func<IDeferredContext, T, Task> taskfunc;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T parameter;

        public InvokeFuncTaskWithParameterDeferredBehavior2(Func<IDeferredContext, T, Task> taskfunc, T parameter, TimeSpan timeSpan, bool invokeInCurrentThread, string? deferName)
            : base(timeSpan, invokeInCurrentThread, deferName)
        {
            this.taskfunc = taskfunc;
            this.parameter = parameter;
        }

        protected override async void Invoke(IDeferredContext deferredContext)
        {
            if (taskfunc is not null)
            {
                await taskfunc(deferredContext, parameter);
            }
        }

        public override void Dispose()
        {
            taskfunc = null!;
            parameter = default!;
            base.Dispose();
        }
    }

    [DebuggerDisplay("{DeferName,nq} deferred:{TimeSpan}")]
    abstract class DeferredBehaviorBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        TimeSpan TimeSpan;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string? DeferName = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SynchronizationContext? synchronizationContext;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        long behaviorVersion = 0;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool disposedValue;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DeferredContext? deferredContext;

        protected DeferredBehaviorBase(TimeSpan timeSpan, bool invokeInCurrentThread, string? deferName)
        {
            TimeSpan = timeSpan;
            DeferName = deferName;
            synchronizationContext = invokeInCurrentThread ? SynchronizationContext.Current : null;

            Restart();
        }

        public virtual void Dispose()
        {
            try
            {
                disposedValue = true;
                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null!;
            }
            catch (Exception) { }
        }

        public void Restart()
        {
            _ = (disposedValue) ? throw new ObjectDisposedException("deferredBehavior") : 0;

            if (deferredContext is not null)
            {
                deferredContext.IsAbandoned = true;
            }

            try
            {
                var @new = new CancellationTokenSource();
                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = @new;
            }
            catch (Exception) { }

            var newMap = new Map(this, Interlocked.Increment(ref behaviorVersion));

            ThreadPool.QueueUserWorkItem(
                static async o =>
                {
                    var newMap = (Map)o!;

                    if (newMap.Version != newMap.Behavior.behaviorVersion)
                    {
                        return;
                    }

                    if ((await newMap.Behavior.SafeDelay()))
                    {
                        DeferredContext deferredContext = newMap.Behavior.deferredContext = new DeferredContext()
                        {
                            BeginTime = DateTime.Now,
                            DeferName = newMap.Behavior.DeferName,
                            DeferTime = newMap.Behavior.TimeSpan,
                            IsAbandoned = false,
                        };

                        newMap.Behavior.InnerInvoke(deferredContext);
                    }
                },
                newMap
            );
        }

        private void InnerInvoke(DeferredContext deferredContext)
        {
            _ = (disposedValue) ? throw new ObjectDisposedException("deferredBehavior") : 0;

            if (synchronizationContext is null)
            {
                deferredContext.BeginTime = DateTime.Now;
                deferredContext.ThreadId = Thread.CurrentThread.ManagedThreadId;

                Invoke(deferredContext);
                return;
            }

            synchronizationContext.Post(
                o =>
                {
                    var map2 = (Map2)o!;
                    map2.DeferredContext.BeginTime = DateTime.Now;
                    map2.DeferredContext.ThreadId = Thread.CurrentThread.ManagedThreadId;
                    map2.Behavior.Invoke(map2.DeferredContext);
                },
                new Map2(this, deferredContext)
            );
        }

        protected abstract void Invoke(IDeferredContext deferredContext);

        private async Task<bool> SafeDelay()
        {
            try
            {
                _ = (disposedValue) ? throw new ObjectDisposedException("deferredBehavior") : 0;

                await Task.Delay(TimeSpan, cancellationTokenSource.Token).ConfigureAwait(false);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        record Map(DeferredBehaviorBase Behavior, long Version);

        record Map2(DeferredBehaviorBase Behavior, DeferredContext DeferredContext);
    }

    class DeferredToken : IDeferredToken
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DeferredBehaviorBase deferredBehavior;
        private bool disposedValue;

        internal DeferredToken(DeferredBehaviorBase deferredBehavior)
        {
            this.deferredBehavior = deferredBehavior;
        }

        public void Restart()
        {
            _ = (disposedValue) ? throw new ObjectDisposedException(nameof(deferredBehavior)) : 0;

            deferredBehavior?.Restart();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    deferredBehavior?.Dispose();
                    deferredBehavior = null!;
                }

                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    class DeferredContext : IDeferredContext
    {
        public bool IsAbandoned { get; set; }
        public string? DeferName { get; set; }
        public int ThreadId { get; set; }
        public DateTime BeginTime { get; set; }
        public TimeSpan DeferTime { get; set; }
    }
}

/// <summary>
/// an interface of <see cref="IDeferredBehavior"/>
/// </summary>
public interface IDeferredBehavior
{
    /// <summary>
    /// invoke taskfunc
    /// </summary>
    /// <param name="action"></param>
    /// <param name="invokeInCurrentThread"></param>
    /// <param name="deferName"></param>
    /// <returns></returns>
    IDeferredToken Invoke(Action action, bool invokeInCurrentThread = false, string? deferName = null);

    /// <summary>
    /// invoke  <paramref name="taskfunc"/>
    /// </summary>
    /// <param name="taskfunc"></param>
    /// <param name="invokeInCurrentThread"></param>
    /// <param name="deferName"></param>
    /// <returns></returns>
    IDeferredToken Invoke(Func<Task> taskfunc, bool invokeInCurrentThread = false, string? deferName = null);

    /// <summary>
    /// invoke <paramref name="action"/> with <typeparamref name="T"/> <paramref name="parameter"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="invokeInCurrentThread"></param>
    /// <param name="action"></param>
    /// <param name="deferName"></param>
    /// <returns></returns>
    IDeferredToken Invoke<T>(T parameter, Action<T> action, bool invokeInCurrentThread = false, string? deferName = null);

    /// <summary>
    /// invoke <paramref name="taskfunc"/> with <typeparamref name="T"/> <paramref name="parameter"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="invokeInCurrentThread"></param>
    /// <param name="taskfunc"></param>
    /// <param name="deferName"></param>
    /// <returns></returns>
    IDeferredToken Invoke<T>(T parameter, Func<T, Task> taskfunc, bool invokeInCurrentThread = false, string? deferName = null);

    /// <summary>
    /// invoke taskfunc
    /// </summary>
    /// <param name="action"></param>
    /// <param name="invokeInCurrentThread"></param>
    /// <param name="deferName"></param>
    /// <returns></returns>
    IDeferredToken Invoke(Action<IDeferredContext> action, bool invokeInCurrentThread = false, string? deferName = null);

    /// <summary>
    /// invoke  <paramref name="taskfunc"/>
    /// </summary>
    /// <param name="taskfunc"></param>
    /// <param name="invokeInCurrentThread"></param>
    /// <param name="deferName"></param>
    /// <returns></returns>
    IDeferredToken Invoke(Func<IDeferredContext, Task> taskfunc, bool invokeInCurrentThread = false, string? deferName = null);

    /// <summary>
    /// invoke <paramref name="action"/> with <typeparamref name="T"/> <paramref name="parameter"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="invokeInCurrentThread"></param>
    /// <param name="action"></param>
    /// <param name="deferName"></param>
    /// <returns></returns>
    IDeferredToken Invoke<T>(T parameter, Action<IDeferredContext, T> action, bool invokeInCurrentThread = false, string? deferName = null);

    /// <summary>
    /// invoke <paramref name="taskfunc"/> with <typeparamref name="T"/> <paramref name="parameter"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="invokeInCurrentThread"></param>
    /// <param name="taskfunc"></param>
    /// <param name="deferName"></param>
    /// <returns></returns>
    IDeferredToken Invoke<T>(T parameter, Func<IDeferredContext, T, Task> taskfunc, bool invokeInCurrentThread = false, string? deferName = null);
}

/// <summary>
/// an interface of <see cref="IDeferredToken"/>
/// </summary>
public interface IDeferredToken : IDisposable
{
    /// <summary>
    /// restart
    /// </summary>
    void Restart();
}

/// <summary>
/// an interface of <see cref="IDeferredContext"/>
/// </summary>
public interface IDeferredContext
{
    /// <summary>
    /// is abandoned
    /// </summary>
    bool IsAbandoned { get; }

    /// <summary>
    ///  defer name
    /// </summary>
    public string? DeferName { get; }

    /// <summary>
    /// thread id
    /// </summary>
    public int ThreadId { get; }

    /// <summary>
    ///invoke time
    /// </summary>
    public DateTime BeginTime { get; }

    /// <summary>
    /// defer time
    /// </summary>
    public TimeSpan DeferTime { get; }
}
