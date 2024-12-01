using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using Promise.Wpf.Services.PopupService;

namespace Promise.Wpf;

/// <summary>
/// a <see langword="class"/> of <see cref="PopupService"/>
/// </summary>
public class PopupService : IPopupService
{
    [DebuggerBrowsable(Never)]
    private static readonly ConcurrentDictionary<string, PopupHosted> hostedStorages = new();

    [DebuggerBrowsable(Never)]
    internal static EasyEvent<PubEventArgs> eventService = new();

    /// <summary>
    /// get show/confirm popup template
    /// example template
    /// <code>
    /// &lt;UserControl&gt;
    ///    &lt;Grid&gt;
    ///        &lt;TextBlock Text="{binding Title}"&gt;
    ///        &lt;TextBlock Text="{binding Content}"&gt;
    ///        &lt;ItemsControl ItemsSource="{binding Buttons}"&gt;
    ///            &lt;ItemsControl.ItemTemplate&gt;
    ///                &lt;DataTemplate&gt;
    ///                    &lt;Button
    ///                        Command="{Binding DataContext.ClickCommand, RelativeSource={RelativeSource Mode=FindAncestor, AncestorLevel=1, AncestorType=ItemsControl}}"
    ///                        CommandParameter="{Binding}"
    ///                        Content="{Binding}"/&gt;
    ///                &lt;/DataTemplate &gt;
    ///            &lt;/ItemsControl.ItemTemplate&gt;
    ///        &lt;/ItemsControl&gt;
    ///    &lt;/Grid&gt;
    /// &lt;/UserControl&gt;
    /// </code>
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <returns></returns>
    public static DataTemplate GetPopupTemplate(AdornerDecorator adornerDecorator)
    {
        return (DataTemplate)adornerDecorator.GetValue(PopupTemplateProperty);
    }

    /// <summary>
    /// set show/confirm popup template
    /// example template
    /// <code>
    /// &lt;UserControl&gt;
    ///    &lt;Grid&gt;
    ///        &lt;TextBlock Text="{binding Title}"&gt;
    ///        &lt;TextBlock Text="{binding Content}"&gt;
    ///        &lt;ItemsControl ItemsSource="{binding Buttons}"&gt;
    ///            &lt;ItemsControl.ItemTemplate&gt;
    ///                &lt;DataTemplate&gt;
    ///                    &lt;Button
    ///                        Command="{Binding DataContext.ClickCommand, RelativeSource={RelativeSource Mode=FindAncestor, AncestorLevel=1, AncestorType=ItemsControl}}"
    ///                        CommandParameter="{Binding}"
    ///                        Content="{Binding}"/&gt;
    ///                &lt;/DataTemplate &gt;
    ///            &lt;/ItemsControl.ItemTemplate&gt;
    ///        &lt;/ItemsControl&gt;
    ///    &lt;/Grid&gt;
    /// &lt;/UserControl&gt;
    /// </code>
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <param name="value"></param>
    public static void SetPopupTemplate(AdornerDecorator adornerDecorator, DataTemplate value)
    {
        adornerDecorator.SetValue(PopupTemplateProperty, value);
    }

    /// <summary>
    /// show/confirm popup template
    /// example template
    /// <code>
    /// &lt;UserControl&gt;
    ///    &lt;Grid&gt;
    ///        &lt;TextBlock Text="{binding Title}"&gt;
    ///        &lt;TextBlock Text="{binding Content}"&gt;
    ///        &lt;ItemsControl ItemsSource="{binding Buttons}"&gt;
    ///            &lt;ItemsControl.ItemTemplate&gt;
    ///                &lt;DataTemplate&gt;
    ///                    &lt;Button
    ///                        Command="{Binding DataContext.ClickCommand, RelativeSource={RelativeSource Mode=FindAncestor, AncestorLevel=1, AncestorType=ItemsControl}}"
    ///                        CommandParameter="{Binding}"
    ///                        Content="{Binding}"/&gt;
    ///                &lt;/DataTemplate &gt;
    ///            &lt;/ItemsControl.ItemTemplate&gt;
    ///        &lt;/ItemsControl&gt;
    ///    &lt;/Grid&gt;
    /// &lt;/UserControl&gt;
    /// </code>
    /// </summary>
    public static readonly DependencyProperty PopupTemplateProperty = DependencyProperty.RegisterAttached("PopupTemplate", typeof(DataTemplate), typeof(PopupService), new PropertyMetadata(null));

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
    /// set hosted name
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <param name="value"></param>
    public static void SetHostedName(AdornerDecorator adornerDecorator, string value)
    {
        adornerDecorator.SetValue(HostedNameProperty, value);
    }

    /// <summary>
    /// hosted name
    /// </summary>
    public static readonly DependencyProperty HostedNameProperty = DependencyProperty.RegisterAttached(
        "HostedName",
        typeof(string),
        typeof(PopupService),
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

                hostedStorages[hostedName] = new PopupHosted(weak);
            }
        )
    );

    /// <summary>
    /// get main host
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <returns></returns>
    public static bool GetIsMainHosted(AdornerDecorator adornerDecorator)
    {
        return (bool)adornerDecorator.GetValue(IsMainHostedProperty);
    }

    /// <summary>
    /// set main host
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <param name="value"></param>
    public static void SetIsMainHosted(AdornerDecorator adornerDecorator, bool value)
    {
        adornerDecorator.SetValue(IsMainHostedProperty, value);
    }

    /// <summary>
    /// main host
    /// </summary>

    public static readonly DependencyProperty IsMainHostedProperty = DependencyProperty.RegisterAttached("IsMainHosted", typeof(bool), typeof(PopupService), new PropertyMetadata(null));

    #region interface
    /// <summary>
    /// show message in main popup host
    /// </summary>
    /// <param name="content"></param>
    /// <param name="title"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public async ValueTask ShowAsync(string content, string? title = null, PopupContext? config = null)
    {
        PopupHosted mainHost = GetMainHost(null!, true);

        PopupContext cfg = config ?? PopupContext.GetDefault(1);

        cfg.Content = content;
        cfg.Title = title ?? cfg.Title ?? "Inotification";

        _ = await mainHost.DisplayAsync(cfg);
    }

    /// <summary>
    /// confirm message in main popup host
    /// </summary>
    /// <param name="content"></param>
    /// <param name="title"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public async ValueTask<ButtonResult> ConfirmAsync(string content, string? title = null, PopupContext? config = null)
    {
        var mainHost = GetMainHost(null!, true);

        PopupContext cfg = config ?? PopupContext.GetDefault(3);

        cfg.Content = content;
        cfg.Title = title ?? cfg.Title ?? "Inotification";

        var buttonResult = await mainHost.DisplayAsync(cfg);

        return buttonResult;
    }

    /// <summary>
    /// show message in <paramref name="hostedName"/> popup host
    /// </summary>
    /// <param name="hostedName"></param>
    /// <param name="content"></param>
    /// <param name="title"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public async ValueTask ShowAsyncIn(string hostedName, string content, string? title = null, PopupContext? config = null)
    {
        _ = hostedName ?? throw new ArgumentNullException(nameof(hostedName));

        var mainHost = GetMainHost(hostedName!, false);

        PopupContext cfg = config ?? PopupContext.GetDefault(1);

        cfg.Content = content;
        cfg.Title = title ?? cfg.Title ?? "Inotification";

        var buttonResult = await mainHost.DisplayAsync(cfg);
    }

    /// <summary>
    /// confirm message in <paramref name="hostedName"/> popup host
    /// </summary>
    /// <param name="hostedName"></param>
    /// <param name="content"></param>
    /// <param name="title"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public async ValueTask<ButtonResult> ConfirmAsyncIn(string hostedName, string content, string? title = null, PopupContext? config = null)
    {
        _ = hostedName ?? throw new ArgumentNullException(nameof(hostedName));

        var mainHost = GetMainHost(hostedName!, false);

        PopupContext cfg = config ?? PopupContext.GetDefault(1);

        cfg.Content = content;
        cfg.Title = title ?? cfg.Title ?? "Inotification";

        var buttonResult = await mainHost.DisplayAsync(cfg);

        return buttonResult;
    }

    /// <summary>
    /// popup visual in main popup host
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="visual"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public async ValueTask<T> PopupAsync<T>(Visual visual, PopupParameter? parameter = null)
    {
        var mainHost = GetMainHost(null!, true);

        var popupResult = await mainHost.PopupAsync<T>(visual, parameter);

        return popupResult;
    }

    /// <summary>
    ///  popup visual in <paramref name="hostedName"/> popup host
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="hostedName"></param>
    /// <param name="visual"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public async ValueTask<T> PopupAsyncIn<T>(string hostedName, Visual visual, PopupParameter? parameter = null)
    {
        _ = hostedName ?? throw new ArgumentNullException(nameof(hostedName));

        var mainHost = GetMainHost(hostedName!, false);

        var popupResult = await mainHost.PopupAsync<T>(visual, parameter);

        return popupResult;
    }

    private PopupHosted GetMainHost(string targetHostedName, bool isHosted)
    {
        foreach (KeyValuePair<string, PopupHosted> item in hostedStorages)
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
        var popupIdentity = isHosted ? "main popup host" : $"popup host : {targetHostedName}";
        throw new InvalidOperationException($"{popupIdentity} not configured");
    }

    #endregion


    #region private

    internal record PubEventArgs(string content, PopupContext Context);

    private record PopupHosted(WeakReference Reference)
    {
        private readonly SemaphoreSlim semaphore = new(1, 1);

        public async ValueTask<ButtonResult> DisplayAsync(PopupContext context)
        {
            try
            {
                await semaphore.WaitAsync();

                if (Reference.Target is not AdornerDecorator decorator)
                {
                    throw new InvalidOperationException("popup host has expired");
                }

                UIElement uielement = default!;

                DataTemplate? datatemplate = GetPopupTemplate(decorator);

                if (datatemplate is not null)
                {
                    uielement = new ContentControl()
                    {
                        DataContext = context,
                        ContentTemplate = datatemplate,
                        Content = context,
                    };
                }
                else
                {
                    uielement = new PopupContainer() { DataContext = context };
                }

                AdornerLayer layer = AdornerLayer.GetAdornerLayer(decorator);

                using ContentAdorner contentAdorner = new(uielement, decorator);

                try
                {
                    layer.Add(contentAdorner);

                    TaskCompletionSource<string> taskCompletion = new();

                    using (
                        eventService.Subscribe(i =>
                        {
                            if (i.Context == context)
                            {
                                taskCompletion.SetResult(i.content);
                            }
                        })
                    )
                    {
                        var buttonContent = await taskCompletion.Task;

                        if (context.buttonResult.TryGetValue(buttonContent, out ButtonResult bottonResult))
                        {
                            return bottonResult;
                        }

                        throw new InvalidOperationException();
                    }
                }
                finally
                {
                    layer.Remove(contentAdorner);
                }
            }
            finally
            {
                _ = semaphore.Release(1);
            }
        }

        public async ValueTask<T> PopupAsync<T>(Visual visual, PopupParameter? parameter = null)
        {
            try
            {
                await semaphore.WaitAsync();

                if (Reference.Target is not AdornerDecorator decorator)
                {
                    throw new InvalidOperationException("popup host has expired");
                }

                AdornerLayer layer = AdornerLayer.GetAdornerLayer(decorator);
                using ContentAdorner contentAdorner = new(visual, decorator);

                try
                {
                    layer.Add(contentAdorner);

                    TaskCompletionSource<object> taskCompletion = new();

                    if (GetPopupAware(visual) is IPopupAware popupAware)
                    {
                        popupAware.Opened(parameter);

                        popupAware.RequestCloseEvent += PopupAware_RequestCloseEvent;

                        void PopupAware_RequestCloseEvent(object obj)
                        {
                            popupAware.RequestCloseEvent -= PopupAware_RequestCloseEvent;

                            popupAware.Closed();

                            taskCompletion.SetResult(obj);
                        }
                    }

                    var popupResult = await taskCompletion.Task;

                    if (popupResult is T target)
                    {
                        return target;
                    }

                    throw new InvalidCastException("invalid popup result type");
                }
                finally
                {
                    layer.Remove(contentAdorner);
                }
            }
            finally
            {
                _ = semaphore.Release(1);
            }

            static IPopupAware GetPopupAware(Visual visual)
            {
                if (visual is IPopupAware popupAware)
                {
                    return popupAware;
                }

                if (visual is FrameworkElement frameworkElement && frameworkElement.DataContext is IPopupAware aware)
                {
                    return aware;
                }
                return null!;
            }
        }
    }

    #endregion
}
