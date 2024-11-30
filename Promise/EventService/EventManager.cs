using System.Collections.Concurrent;
using System.Diagnostics;

namespace Promise;

/// <summary>
/// a class of <see cref="EventManager"/>
/// </summary>
public class EventManager : IEventManager
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private List<object> asyncEvents = new List<object>();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private List<object> syncEvents = new List<object>();

    /// <summary>
    /// get async event
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IAsyncEvent<T> GetAsyncEvent<T>()
    {
        if (TryFind<T>(asyncEvents, out var asyncEvent) == false)
        {
            lock (this)
            {
                if (TryFind<T>(asyncEvents, out asyncEvent) == false)
                {
                    asyncEvent = new AsyncEvent<T>();
                    asyncEvents.Add(asyncEvent);
                }
            }
        }

        return asyncEvent;

        static bool TryFind<T1>(List<object> asyncEvents, out IAsyncEvent<T1> asyncEvent)
        {
            for (int i = asyncEvents.Count - 1; i >= 0; i--)
            {
                if (asyncEvents[i] is IAsyncEvent<T1> asyncEvent1)
                {
                    asyncEvent = asyncEvent1;

                    return true;
                }
            }
            asyncEvent = default!;
            return false;
        }
    }

    /// <summary>
    /// get event
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IEvent<T> GetEvent<T>()
    {
        if (TryFind<T>(syncEvents, out var syncEvent) == false)
        {
            lock (this)
            {
                if (TryFind<T>(syncEvents, out syncEvent) == false)
                {
                    syncEvent = new Event<T>();
                    syncEvents.Add(syncEvent);
                }
            }
        }

        return syncEvent;

        static bool TryFind<T1>(List<object> syncEvents, out IEvent<T1> asyncEvent)
        {
            for (int i = syncEvents.Count - 1; i >= 0; i--)
            {
                if (syncEvents[i] is IEvent<T1> asyncEvent1)
                {
                    asyncEvent = asyncEvent1;

                    return true;
                }
            }
            asyncEvent = default!;
            return false;
        }
    }
}

/// <summary>
/// a <see langword="class"/> of <see cref="Event{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public class Event<T> : IEvent<T>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IDictionary<string, List<object>> eventMaps = new ConcurrentDictionary<string, List<object>>();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const string COMMON_CHANNEL = "{5466872F-B015-4CE0-A640-6FBE2A986B1F}";

    /// <summary>
    /// publish
    /// </summary>
    /// <param name="event"></param>
    public void Publish(T @event)
    {
        Publish(COMMON_CHANNEL, @event);
    }

    /// <summary>
    /// publish
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="event"></param>
    public void Publish(string channel, T @event)
    {
        _ = channel ?? throw new ArgumentNullException(nameof(channel));

        if (eventMaps.TryGetValue(channel, out List<object>? subs) == false)
        {
            eventMaps[channel] = subs = new List<object>();
        }

        for (int i = subs.Count - 1; i >= 0; i--)
        {
            if (i >= subs.Count)
            {
                continue;
            }

            if (subs[i] is Subscription<T> sub)
            {
                sub.Invoke(@event);
            }
        }
    }

    /// <summary>
    /// subscribe
    /// </summary>
    /// <param name="subscribe"></param>
    /// <param name="threadPolicy"></param>
    /// <returns></returns>
    public IUnsubscrible Subscribe(Action<T> subscribe, EventThreadPolicy threadPolicy = EventThreadPolicy.Current)
    {
        return Subscribe(COMMON_CHANNEL, subscribe, threadPolicy);
    }

    /// <summary>
    /// subscribe
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="subscribe"></param>
    /// <param name="threadPolicy"></param>
    /// <returns></returns>
    public IUnsubscrible Subscribe(string channel, Action<T> subscribe, EventThreadPolicy threadPolicy = EventThreadPolicy.Current)
    {
        _ = channel ?? throw new ArgumentNullException(nameof(channel));

        if (eventMaps.TryGetValue(channel, out List<object>? subs) == false)
        {
            eventMaps[channel] = subs = new List<object>();
        }

        Subscription<T> sub = new(channel, subscribe, threadPolicy, SynchronizationContext.Current);

        subs.Add(sub);

        return new Unsubscrible(subs, sub);
    }

    private record Subscription<TE>(string Channel, Action<TE> Subscribe, EventThreadPolicy ThreadPolicy, SynchronizationContext? Context)
    {
        public void Invoke(TE parameter)
        {
            switch (ThreadPolicy)
            {
                case EventThreadPolicy.Current:
                    Context?.Post((_) => Subscribe(parameter), null);
                    break;
                case EventThreadPolicy.PublishThread:
                    Subscribe(parameter);
                    break;
                case EventThreadPolicy.NewThread:
                    _ = ThreadPool.QueueUserWorkItem(_ => Subscribe(parameter));
                    break;
            }
        }
    }
}

/// <summary>
/// a <see langword="class"/> of <see cref="AsyncEvent{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public class AsyncEvent<T> : IAsyncEvent<T>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IDictionary<string, List<object>> eventMaps = new ConcurrentDictionary<string, List<object>>();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const string COMMON_CHANNEL = "{34ED5A8B-F218-45D9-8E4A-EE60EC5563AD}";

    /// <summary>
    /// publish
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    public async Task PublishAsync(T @event)
    {
        await PublishAsync(COMMON_CHANNEL, @event);
    }

    /// <summary>
    /// publich
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="event"></param>
    /// <returns></returns>
    public async Task PublishAsync(string channel, T @event)
    {
        _ = channel ?? throw new ArgumentNullException(nameof(channel));

        if (eventMaps.TryGetValue(channel, out List<object>? subs) == false)
        {
            eventMaps[channel] = subs = new List<object>();
        }

        for (int i = subs.Count - 1; i >= 0; i--)
        {
            if (i >= subs.Count)
            {
                continue;
            }

            if (subs[i] is SubscriptionAsync<T> sub)
            {
                await sub.InvokeAsync(@event);
            }
        }
    }

    /// <summary>
    /// subscribe
    /// </summary>
    /// <param name="subscribe"></param>
    /// <param name="threadPolicy"></param>
    /// <returns></returns>
    public IUnsubscrible Subscribe(Func<T, Task> subscribe, EventThreadPolicy threadPolicy = EventThreadPolicy.Current)
    {
        return Subscribe(COMMON_CHANNEL, subscribe, threadPolicy);
    }

    /// <summary>
    /// subscribe
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="subscribe"></param>
    /// <param name="threadPolicy"></param>
    /// <returns></returns>
    public IUnsubscrible Subscribe(string channel, Func<T, Task> subscribe, EventThreadPolicy threadPolicy = EventThreadPolicy.Current)
    {
        _ = channel ?? throw new ArgumentNullException(nameof(channel));

        if (eventMaps.TryGetValue(channel, out List<object>? subs) == false)
        {
            eventMaps[channel] = subs = new List<object>();
        }

        SubscriptionAsync<T> sub = new(channel, subscribe, threadPolicy, SynchronizationContext.Current);

        subs.Add(sub);

        return new Unsubscrible(subs, sub);
    }

    private record SubscriptionAsync<TE>(string Channel, Func<TE, Task> Subscribe, EventThreadPolicy ThreadPolicy, SynchronizationContext? Context)
    {
        public async Task InvokeAsync(TE parameter)
        {
            switch (ThreadPolicy)
            {
                case EventThreadPolicy.Current:
                    Context?.Post(async (_) => await Subscribe(parameter), null);
                    break;
                case EventThreadPolicy.PublishThread:
                    await Subscribe(parameter);
                    break;
                case EventThreadPolicy.NewThread:
                    _ = ThreadPool.QueueUserWorkItem(async _ => await Subscribe(parameter));
                    break;
            }
        }
    }
}

/// <summary>
/// a <see langword="class"/> of <see cref="Unsubscrible"/>
/// </summary>
public class Unsubscrible : IUnsubscrible
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private List<object> eventMaps;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private object @event;

    /// <summary>
    ///
    /// </summary>
    /// <param name="eventMaps"></param>
    /// <param name="event"></param>
    public Unsubscrible(List<object> eventMaps, object @event)
    {
        this.eventMaps = eventMaps;
        this.@event = @event;
    }

    void IDisposable.Dispose()
    {
        if (eventMaps is not null && eventMaps.Count > 0 && @event is not null)
        {
            _ = eventMaps.Remove(@event);
            eventMaps = null!;
            @event = null!;
        }
    }

    void IUnsubscrible.Unsubscribe()
    {
        ((IDisposable)this).Dispose();
    }
}
