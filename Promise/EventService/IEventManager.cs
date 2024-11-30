using System.Threading.Tasks;

namespace Promise;

/// <summary>
///  an interface of <see cref="IEventManager"/>
/// </summary>
public interface IEventManager
{
    /// <summary>
    /// get <typeparamref name="T"/> event
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IEvent<T> GetEvent<T>();

    /// <summary>
    /// get <typeparamref name="T"/> async event
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IAsyncEvent<T> GetAsyncEvent<T>();
}

/// <summary>
/// an interface of <see cref="IEvent{TEvent}"/>
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface IEvent<TEvent>
{
    /// <summary>
    /// subscribe event
    /// </summary>
    /// <param name="subscribe"></param>
    /// <param name="threadPolicy"></param>
    /// <returns></returns>
    IUnsubscrible Subscribe(Action<TEvent> subscribe, EventThreadPolicy threadPolicy = EventThreadPolicy.Current);

    /// <summary>
    /// publish @event
    /// </summary>
    /// <param name="event"></param>
    void Publish(TEvent @event);

    /// <summary>
    /// subscribe event to <paramref name="channel"/>
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="subscribe"></param>
    /// <param name="threadPolicy"></param>
    /// <returns></returns>
    IUnsubscrible Subscribe(string channel, Action<TEvent> subscribe, EventThreadPolicy threadPolicy = EventThreadPolicy.Current);

    /// <summary>
    /// publish @event to <paramref name="channel"/>
    /// </summary>
    /// <param name="event"></param>
    /// <param name="channel"></param>
    void Publish(string channel, TEvent @event);
}

/// <summary>
/// an interface of <see cref="IEvent{TEvent}"/>
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface IAsyncEvent<TEvent>
{
    /// <summary>
    /// subscribe event
    /// </summary>
    /// <param name="subscribe"></param>
    /// <param name="threadPolicy"></param>
    /// <returns></returns>
    IUnsubscrible Subscribe(Func<TEvent, Task> subscribe, EventThreadPolicy threadPolicy = EventThreadPolicy.Current);

    /// <summary>
    /// publish @event
    /// </summary>
    /// <param name="event"></param>

    Task PublishAsync(TEvent @event);

    /// <summary>
    ///  subscribe event to <paramref name="channel"/>
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="subscribe"></param>
    /// <param name="threadPolicy"></param>
    /// <returns></returns>
    IUnsubscrible Subscribe(string channel, Func<TEvent, Task> subscribe, EventThreadPolicy threadPolicy = EventThreadPolicy.Current);

    /// <summary>
    /// publish @event to <paramref name="channel"/>
    /// </summary>
    /// <param name="event"></param>
    /// <param name="channel"></param>

    Task PublishAsync(string channel, TEvent @event);
}

/// <summary>
/// event policy
/// </summary>
public enum EventThreadPolicy
{
    /// <summary>
    /// subscribe thread
    /// </summary>
    Current,

    /// <summary>
    /// publish thread
    /// </summary>
    PublishThread,

    /// <summary>
    /// new thread
    /// </summary>
    NewThread,
}

/// <summary>
/// an <see langword="interface"/> of <see cref="IUnsubscrible"/>
/// </summary>
public interface IUnsubscrible : IDisposable
{
    /// <summary>
    /// unsubscribe
    /// </summary>
    void Unsubscribe();
}
