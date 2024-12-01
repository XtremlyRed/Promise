using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promise.Wpf;

internal class EasyEvent<T>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly List<object> eventMaps = new();

    public void Publish(T @event)
    {
        for (int i = eventMaps.Count - 1; i >= 0; i--)
        {
            if (eventMaps[i] is Subscription<T> sub)
            {
                sub.Invoke(@event);
            }
        }
    }

    public IDisposable Subscribe(Action<T> subscribe)
    {
        Subscription<T> sub = new(subscribe, SynchronizationContext.Current);

        eventMaps.Add(sub);

        return new Unsubscrible(eventMaps, sub);
    }

    private record Subscription<TE>(Action<TE> Subscribe, SynchronizationContext? Context)
    {
        public void Invoke(TE parameter)
        {
            Subscribe(parameter);
        }
    }

    public record Unsubscrible(List<object> eventMaps, object @event) : IDisposable
    {
        void IDisposable.Dispose()
        {
            if (eventMaps is not null && eventMaps.Count > 0 && @event is not null)
            {
                _ = eventMaps.Remove(@event);
            }
        }
    }
}
