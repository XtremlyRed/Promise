using System.Diagnostics;
using System.Windows.Media;

namespace Promise.Wpf;

/// <summary>
///
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// show window by <paramref name="visual"/>
    /// </summary>
    /// <param name="visual"></param>
    /// <param name="parameter"></param>
    void Show(Visual visual, DialogParameter? parameter = null);

    /// <summary>
    /// show dialog window <paramref name="visual"/>
    /// </summary>
    /// <param name="visual"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    bool? ShowDialog(Visual visual, DialogParameter? parameter = null);
}
