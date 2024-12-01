namespace Promise.Wpf;

/// <summary>
/// a <see langword="class"/> of <see cref="IDialogServiceExtensions"/>
/// </summary>
public static class IDialogServiceExtensions
{
    /// <summary>
    /// <para>show dialog visual in main dialog host </para>
    /// <para>Use <see cref="IViewLocator"/> to locate the view</para>
    /// <para>Before using this method, please first set the <see cref="IViewLocator"/> using method <see cref="ViewLocator.SetViewLocator(IViewLocator)"/></para>
    /// </summary>
    /// <param name="dialogService"></param>
    /// <param name="visualToken"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static bool? ShowDialog(this IDialogService dialogService, string visualToken, DialogParameter? parameter = null)
    {
        _ = ViewLocator.viewLocator ?? throw new InvalidOperationException("invalid visual locator");

        var visual = ViewLocator.viewLocator.Locate(visualToken);

        return dialogService.ShowDialog(visual, parameter);
    }

    /// <summary>
    /// <para>show visual in main dialog host </para>
    /// <para>Use <see cref="IViewLocator"/> to locate the view</para>
    /// <para>Before using this method, please first set the <see cref="IViewLocator"/> using method <see cref="ViewLocator.SetViewLocator(IViewLocator)"/></para>
    /// </summary>
    /// <param name="dialogService"></param>
    /// <param name="visualToken"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static void Show(this IDialogService dialogService, string visualToken, DialogParameter? parameter = null)
    {
        _ = ViewLocator.viewLocator ?? throw new InvalidOperationException("invalid visual locator");

        var visual = ViewLocator.viewLocator.Locate(visualToken);

        dialogService.Show(visual, parameter);
    }
}
