using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Promise.Wpf;

/// <summary>
/// a <see langword="class"/> of <see cref="DialogService"/>
/// </summary>
public class DialogService : IDialogService
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

    /// <summary>
    /// show window
    /// </summary>
    /// <param name="visual"></param>
    /// <param name="parameter"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Show(Visual visual, DialogParameter? parameter = null)
    {
        _ = dialogWindiw ?? throw new ArgumentNullException(nameof(dialogWindiw));
        _ = visual ?? throw new ArgumentNullException(nameof(visual));

        semaphoreSlim.Wait();

        var dialogWindow = InnerInit(visual, parameter);

        dialogWindow.Show();
    }

    /// <summary>
    /// show dialog
    /// </summary>
    /// <param name="visual"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool? ShowDialog(Visual visual, DialogParameter? parameter = null)
    {
        _ = dialogWindiw ?? throw new ArgumentNullException(nameof(dialogWindiw));
        _ = visual ?? throw new ArgumentNullException(nameof(visual));

        semaphoreSlim.Wait();

        var dialogWindow = InnerInit(visual, parameter);

        return dialogWindow.ShowDialog();
    }

    /// <summary>
    /// init window
    /// </summary>
    /// <param name="visual"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    private Window InnerInit(Visual visual, DialogParameter? parameter = null)
    {
        dialogWindiw!.Content = visual;
        dialogWindiw.Closed += DialogWindiw_Closed;

        if (visual is IDialogAware aware)
        {
            aware.RequestCloseEvent += Aware_RequestCloseEvent;
            aware.Opened(parameter);
        }

        dialogWindiw.Owner = Application.Current.MainWindow;

        return dialogWindiw;

        void DialogWindiw_Closed(object? sender, EventArgs e)
        {
            if (sender is not Window windiw)
            {
                return;
            }

            windiw!.Closed -= DialogWindiw_Closed;

            if (windiw.Content is IDialogAware dialogAware)
            {
                dialogAware.RequestCloseEvent += Aware_RequestCloseEvent;
                dialogAware.Closed();
            }
            else if (windiw.Content is FrameworkElement element && element.DataContext is IDialogAware aware)
            {
                aware.RequestCloseEvent += Aware_RequestCloseEvent;
                aware.Closed();
            }

            semaphoreSlim.Release();
        }

        void Aware_RequestCloseEvent(object obj)
        {
            dialogWindiw?.Close();
        }
    }

    static Window? dialogWindiw;

    /// <summary>
    /// set dialog window container
    /// </summary>
    /// <param name="dialogWindiw"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void SetDialogWindiw(Window dialogWindiw)
    {
        DialogService.dialogWindiw = dialogWindiw ?? throw new ArgumentNullException(nameof(dialogWindiw));
    }
}

/// <summary>
/// an <see langword="interface"/> of <see cref="IDialogAware"/>
/// </summary>
public interface IDialogAware
{
    /// <summary>
    /// dialog opened
    /// </summary>
    /// <param name="parameter"></param>
    void Opened(DialogParameter? parameter);

    /// <summary>
    /// dialog closed
    /// </summary>
    void Closed();

    /// <summary>
    /// request close <see langword="event"/>
    /// </summary>

    event Action<object>? RequestCloseEvent;
}
