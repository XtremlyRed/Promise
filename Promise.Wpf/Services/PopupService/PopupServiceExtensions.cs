using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Promise.Wpf;

/// <summary>
/// <see cref="IPopupService"/> extensions
/// </summary>
public static class PopupServiceExtensions
{
    /// <summary>
    /// show async
    /// </summary>
    /// <param name="popupService"></param>
    /// <param name="content"></param>
    /// <param name="title"></param>
    /// <param name="buttonContents"></param>
    /// <returns></returns>
    public static async ValueTask ShowAsync(this IPopupService popupService, string content, string? title = null, params string[] buttonContents)
    {
        PopupContext context = buttonContents;
        await popupService.ShowAsync(content, title, context);
    }

    /// <summary>
    /// show async
    /// </summary>
    /// <param name="popupService"></param>
    /// <param name="content"></param>
    /// <param name="title"></param>
    /// <param name="buttonContents"></param>
    /// <returns></returns>
    public static async ValueTask<ButtonResult> ConfirmAsync(this IPopupService popupService, string content, string? title = null, params string[] buttonContents)
    {
        PopupContext context = buttonContents;
        var result = await popupService.ConfirmAsync(content, title, context);

        return result;
    }

    /// <summary>
    ///  show async in <paramref name="hostedName"/>
    /// </summary>
    /// <param name="popupService"></param>
    /// <param name="hostedName"></param>
    /// <param name="content"></param>
    /// <param name="title"></param>
    /// <param name="buttonContents"></param>
    /// <returns></returns>
    public static async ValueTask ShowAsyncIn(this IPopupService popupService, string hostedName, string content, string? title = null, params string[] buttonContents)
    {
        PopupContext context = buttonContents;
        await popupService.ShowAsyncIn(hostedName, content, title, context);
    }

    /// <summary>
    /// show async in <paramref name="hostedName"/>
    /// </summary>
    /// <param name="popupService"></param>
    /// <param name="content"></param>
    /// <param name="hostedName"></param>
    /// <param name="title"></param>
    /// <param name="buttonContents"></param>
    /// <returns></returns>
    public static async ValueTask<ButtonResult> ConfirmAsyncIn(this IPopupService popupService, string content, string hostedName, string? title = null, params string[] buttonContents)
    {
        PopupContext context = buttonContents;
        var result = await popupService.ConfirmAsyncIn(hostedName, content, title, context);

        return result;
    }

    /// <summary>
    /// popup visual in main popup host
    /// </summary>
    /// <param name="popupService"></param>
    /// <param name="visual"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static async ValueTask<object> PopupAsync(this IPopupService popupService, Visual visual, PopupParameter? parameter = null)
    {
        var popupResult = await popupService.PopupAsync<object>(visual, parameter);
        return popupResult;
    }

    /// <summary>
    ///  popup visual in <paramref name="hostedName"/> popup host
    /// </summary>
    /// <param name="popupService"></param>
    /// <param name="hostedName"></param>
    /// <param name="visual"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static async ValueTask<object> PopupAsyncIn(this IPopupService popupService, string hostedName, Visual visual, PopupParameter? parameter = null)
    {
        var popupResult = await popupService.PopupAsyncIn<object>(hostedName, visual, parameter);
        return popupResult;
    }

    /// <summary>
    /// <para>popup visual in main popup host </para>
    /// <para>Use <see cref="IViewLocator"/> to locate the view</para>
    /// <para>Before using this method, please first set the <see cref="IViewLocator"/> using method <see cref="ViewLocator.SetViewLocator(IViewLocator)"/></para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="popupService"></param>
    /// <param name="visualToken"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static async ValueTask<T> PopupAsync<T>(this IPopupService popupService, string visualToken, PopupParameter? parameter = null)
    {
        _ = ViewLocator.viewLocator ?? throw new InvalidOperationException("invalid visual locator");

        var visual = ViewLocator.viewLocator.Locate(visualToken);

        return await popupService.PopupAsync<T>(visual, parameter);
    }

    /// <summary>
    /// <para>popup visual in <paramref name="hostedName"/> popup host </para>
    /// <para>Use <see cref="IViewLocator"/> to locate the view</para>
    /// <para>Before using this method, please first set the <see cref="IViewLocator"/> using method <see cref="ViewLocator.SetViewLocator(IViewLocator)"/></para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="popupService"></param>
    /// <param name="hostedName"></param>
    /// <param name="visualToken"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static async ValueTask<T> PopupAsyncIn<T>(this IPopupService popupService, string hostedName, string visualToken, PopupParameter? parameter = null)
    {
        _ = ViewLocator.viewLocator ?? throw new InvalidOperationException("invalid visual locator");

        var visual = ViewLocator.viewLocator.Locate(visualToken);

        return await popupService.PopupAsyncIn<T>(hostedName, visual, parameter);
    }

    /// <summary>
    /// <para>popup visual in main popup host </para>
    /// <para>Use <see cref="IViewLocator"/> to locate the view</para>
    /// <para>Before using this method, please first set the <see cref="IViewLocator"/> using method <see cref="ViewLocator.SetViewLocator(IViewLocator)"/></para>
    /// </summary>
    /// <param name="popupService"></param>
    /// <param name="visualToken"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static async ValueTask<object> PopupAsync(this IPopupService popupService, string visualToken, PopupParameter? parameter = null)
    {
        _ = ViewLocator.viewLocator ?? throw new InvalidOperationException("invalid visual locator");

        var visual = ViewLocator.viewLocator.Locate(visualToken);

        return await popupService.PopupAsync<object>(visual, parameter);
    }

    /// <summary>
    /// <para>popup visual in <paramref name="hostedName"/> popup host </para>
    /// <para>Use <see cref="IViewLocator"/> to locate the view</para>
    /// <para>Before using this method, please first set the <see cref="IViewLocator"/> using method <see cref="ViewLocator.SetViewLocator(IViewLocator)"/></para>
    /// </summary>
    /// <param name="popupService"></param>
    /// <param name="hostedName"></param>
    /// <param name="visualToken"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static async ValueTask<object> PopupAsyncIn(this IPopupService popupService, string hostedName, string visualToken, PopupParameter? parameter = null)
    {
        _ = ViewLocator.viewLocator ?? throw new InvalidOperationException("invalid visual locator");

        var visual = ViewLocator.viewLocator.Locate(visualToken);

        return await popupService.PopupAsyncIn<object>(hostedName, visual, parameter);
    }
}
