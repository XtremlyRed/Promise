using System.Collections.Concurrent;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Promise.Wpf.Services.NotificationService;

namespace Promise.Wpf;

/// <summary>
/// a <see langword="class"/> of <see cref="NotificationService"/>
/// </summary>
public class NotificationService : INotificationService
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly ConcurrentDictionary<string, NofityHosted> hostedStorages = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal static EasyEvent<PubEventArgs> eventService = new();

    /// <summary>
    /// notify message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async ValueTask NotifyAsync(string message, TimeSpan timeSpan)
    {
        var mainHost = GetMainHost(null!, true);

        await mainHost.NotifyAsync(message, timeSpan);
    }

    /// <summary>
    /// notify message
    /// </summary>
    /// <param name="hostedName"></param>
    /// <param name="message"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async ValueTask NotifyAsyncIn(string hostedName, string message, TimeSpan timeSpan)
    {
        _ = hostedName ?? throw new ArgumentNullException(nameof(hostedName));

        var mainHost = GetMainHost(hostedName!, false);

        await mainHost.NotifyAsync(message, timeSpan);
    }

    /// <summary>
    /// get notification template
    /// example template
    /// <code>
    /// &lt;UserControl&gt;
    ///     &lt;TextBlock Text="{Binding}"/&gt;
    /// &lt;/UserControl&gt;
    /// </code>
    /// </summary>
    public static DataTemplate GetNotificationTemplate(DependencyObject obj)
    {
        return (DataTemplate)obj.GetValue(NotificationTemplateProperty);
    }

    /// <summary>
    /// set notification template
    /// example template
    /// <code>
    /// &lt;UserControl&gt;
    ///     &lt;TextBlock Text="{Binding}"/&gt;
    /// &lt;/UserControl&gt;
    /// </code>
    /// </summary>
    public static void SetNotificationTemplate(DependencyObject obj, DataTemplate value)
    {
        obj.SetValue(NotificationTemplateProperty, value);
    }

    /// <summary>
    /// notification template
    /// example template
    /// <code>
    /// &lt;UserControl&gt;
    ///     &lt;TextBlock Text="{Binding}"/&gt;
    /// &lt;/UserControl&gt;
    /// </code>
    /// </summary>
    public static readonly DependencyProperty NotificationTemplateProperty = DependencyProperty.RegisterAttached("NotificationTemplate", typeof(DataTemplate), typeof(NotificationService), new PropertyMetadata(null));

    /// <summary>
    /// get main hosted
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <returns></returns>
    public static bool GetIsMainHosted(AdornerDecorator adornerDecorator)
    {
        return (bool)adornerDecorator.GetValue(IsMainHostedProperty);
    }

    /// <summary>
    /// set main hosted
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <param name="value"></param>
    public static void SetIsMainHosted(AdornerDecorator adornerDecorator, bool value)
    {
        adornerDecorator.SetValue(IsMainHostedProperty, value);
    }

    /// <summary>
    /// main hosted
    /// </summary>
    public static readonly DependencyProperty IsMainHostedProperty = DependencyProperty.RegisterAttached("IsMainHosted", typeof(bool), typeof(NotificationService), new PropertyMetadata(false));

    /// <summary>
    /// get hosted name
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <returns></returns>
    public static string GetHostedName(AdornerDecorator adornerDecorator)
    {
        return (string)adornerDecorator.GetValue(HostedNameProperty);
    }

    /// <summary>
    /// set host name
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <param name="value"></param>
    public static void SetHostedName(AdornerDecorator adornerDecorator, string value)
    {
        adornerDecorator.SetValue(HostedNameProperty, value);
    }

    /// <summary>
    /// host name
    /// </summary>
    public static readonly DependencyProperty HostedNameProperty = DependencyProperty.RegisterAttached(
        "HostedName",
        typeof(string),
        typeof(NotificationService),
        new PropertyMetadata(
            null,
            static (s, e) =>
            {
                if (s is not AdornerDecorator adornerDecorator)
                {
                    return;
                }

                if (e.OldValue is string oldHostedName)
                {
                    _ = hostedStorages.TryRemove(oldHostedName, out _);
                }

                if (e.NewValue is not string hostedName)
                {
                    return;
                }
                WeakReference weak = new(adornerDecorator);

                hostedStorages[hostedName] = new NofityHosted(weak);
            }
        )
    );

    private NofityHosted GetMainHost(string targetHostedName, bool isHosted)
    {
        foreach (KeyValuePair<string, NofityHosted> item in hostedStorages)
        {
            if (item.Value.Reference.Target is AdornerDecorator decorator)
            {
                if (isHosted && GetIsMainHosted(decorator))
                {
                    return item.Value;
                }

                if (isHosted == false && GetHostedName(decorator) == targetHostedName)
                {
                    return item.Value;
                }
            }
        }
        var popupIdentity = isHosted ? "main notify host" : $"notify host : {targetHostedName}";
        throw new InvalidOperationException($"{popupIdentity} not configured");
    }

    internal record PubEventArgs(string content, PopupContext Context);

    record NofityHosted(WeakReference Reference)
    {
        SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// notify
        /// </summary>
        /// <param name="message"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        internal async Task NotifyAsync(string message, TimeSpan timeSpan)
        {
            try
            {
                await semaphore.WaitAsync();

                if (Reference.Target is not AdornerDecorator decorator)
                {
                    throw new InvalidOperationException("notification host has expired");
                }

                UIElement uielement = default!;

                DataTemplate? datatemplate = GetNotificationTemplate(decorator);

                if (datatemplate is not null)
                {
                    uielement = new ContentControl()
                    {
                        DataContext = message,
                        ContentTemplate = datatemplate,
                        Content = message,
                    };
                }
                else
                {
                    uielement = new NotifyContainer() { DataContext = message };
                }

                AdornerLayer layer = AdornerLayer.GetAdornerLayer(decorator);

                using ContentAdorner contentAdorner = new(uielement, decorator);

                layer.Add(contentAdorner);

                TaskCompletionSource<bool> taskCompletion = new TaskCompletionSource<bool>();

                ThreadPool.QueueUserWorkItem(o =>
                {
                    Thread.Sleep(timeSpan); // wait for the specified time
                    taskCompletion.SetResult(true);
                });

                await taskCompletion.Task;

                layer.Remove(contentAdorner);
            }
            finally
            {
                _ = semaphore.Release(1);
            }
        }
    }
}
