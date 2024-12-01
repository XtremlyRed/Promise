using System.Windows;
using System.Windows.Media;

namespace Promise.Wpf;

/// <summary>
/// an <see langword="interface"/> of <see cref="IViewModelLocator"/>
/// </summary>
public interface IViewModelLocator
{
    /// <summary>
    /// view model locate
    /// </summary>
    /// <param name="viewToken"></param>
    /// <returns></returns>
    object Locate(string viewToken);

    /// <summary>
    /// view model locate
    /// </summary>
    /// <param name="visual"></param>
    /// <returns></returns>
    object Locate(Visual visual);
}

/// <summary>
/// a <see langword="class"/> of <see cref="ViewModelLocator"/>
/// </summary>
public static class ViewModelLocator
{
    [DBA(Never)]
    internal static IViewModelLocator? viewModelLocator;

    /// <summary>
    /// set popup view model locator when used
    /// </summary>
    /// <param name="viewModelLocator"></param>
    public static void SetViewModelLocator(IViewModelLocator viewModelLocator)
    {
        ViewModelLocator.viewModelLocator = viewModelLocator ?? throw new ArgumentNullException(nameof(viewModelLocator));
    }

    /// <summary>
    /// set auto aware
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetAutoAware(DependencyObject obj, bool value)
    {
        obj.SetValue(AutoAwareProperty, value);
    }

    /// <summary>
    /// view model auto aware
    /// </summary>
    public static readonly DependencyProperty AutoAwareProperty = DependencyProperty.RegisterAttached(
        "AutoAware",
        typeof(bool),
        typeof(ViewModelLocator),
        new PropertyMetadata(
            false,
            (s, e) =>
            {
                if (s is Visual visual && e.NewValue is bool autoAware && autoAware)
                {
                    if (viewModelLocator is null)
                    {
                        throw new InvalidOperationException("invalid view model locator");
                    }

                    var viewModel = viewModelLocator.Locate(visual);

                    if (viewModel is not null && s is FrameworkElement element)
                    {
                        element.DataContext = viewModel;
                    }
                }
            }
        )
    );
}
