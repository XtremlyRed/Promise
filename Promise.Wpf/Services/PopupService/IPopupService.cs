using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static Promise.Wpf.PopupService;

namespace Promise.Wpf;

/// <summary>
/// an <see langword="interface"/> of <see cref="IPopupService"/>
/// </summary>
public interface IPopupService
{
    /// <summary>
    /// show message in main popup host
    /// </summary>
    /// <param name="content"></param>
    /// <param name="title"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    ValueTask ShowAsync(string content, string? title = null, PopupContext? config = null);

    /// <summary>
    /// confirm message in main popup host
    /// </summary>
    /// <param name="content"></param>
    /// <param name="title"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    ValueTask<ButtonResult> ConfirmAsync(string content, string? title = null, PopupContext? config = null);

    /// <summary>
    /// show message in <paramref name="hostedName"/> popup host
    /// </summary>
    /// <param name="hostedName"></param>
    /// <param name="content"></param>
    /// <param name="title"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    ValueTask ShowAsyncIn(string hostedName, string content, string? title = null, PopupContext? config = null);

    /// <summary>
    /// confirm message in <paramref name="hostedName"/> popup host
    /// </summary>
    /// <param name="hostedName"></param>
    /// <param name="content"></param>
    /// <param name="title"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    ValueTask<ButtonResult> ConfirmAsyncIn(string hostedName, string content, string? title = null, PopupContext? config = null);

    /// <summary>
    /// popup visual in main popup host
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="visual"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    ValueTask<T> PopupAsync<T>(Visual visual, PopupParameter? parameter = null);

    /// <summary>
    ///  popup visual in <paramref name="hostedName"/> popup host
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="hostedName"></param>
    /// <param name="visual"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    ValueTask<T> PopupAsyncIn<T>(string hostedName, Visual visual, PopupParameter? parameter = null);
}
