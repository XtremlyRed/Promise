using System.Collections.Concurrent;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Promise.Wpf;

/// <summary>
///
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// notify message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="timeSpan">display timespan</param>
    ValueTask NotifyAsync(string message, TimeSpan timeSpan);

    /// <summary>
    /// notify message
    /// </summary>
    /// <param name="hostedName"></param>
    /// <param name="message"></param>
    /// <param name="timeSpan">display timespan</param>
    ValueTask NotifyAsyncIn(string hostedName, string message, TimeSpan timeSpan);
}
