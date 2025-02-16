using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Promise.Wpf;

/// <summary>
/// a <see langword="class"/> of <see cref="ViewModelLocator"/>
/// </summary>
public static class ViewModelLocator
{
    [DBA(Never)]
    internal static Func<Type, Type>? viewModelTypeLocator = DefaultViewTypeToViewModel;

    [DBA(Never)]
    internal static Func<Type, object>? viewModelLocator;

    /// <summary>
    /// set   view model locator when used
    /// </summary>
    /// <param name="viewModelTypeLocator"></param>
    public static void SetViewModelTypeLocator(Func<Type, Type> viewModelTypeLocator)
    {
        ViewModelLocator.viewModelTypeLocator = viewModelTypeLocator ?? throw new ArgumentNullException(nameof(viewModelTypeLocator));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="viewModelLocator"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void SetViewModelLocator(Func<Type, object> viewModelLocator)
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

                    var viewModelType = viewModelTypeLocator(visual.GetType());

                    var viewModel = viewModelLocator(viewModelType);

                    if (viewModel is not null && s is FrameworkElement element)
                    {
                        element.DataContext = viewModel;
                    }
                }
            }
        )
    );

    private static Type DefaultViewTypeToViewModel(Type viewType)
    {
        var viewName = viewType.FullName;
        viewName = viewName?.Replace(".Views.", ".ViewModels.");
        var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
        var suffix = viewName != null && viewName.EndsWith("View") ? "Model" : "ViewModel";
        var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}{1}, {2}", viewName, suffix, viewAssemblyName);
        return Type.GetType(viewModelName)!;
    }
}
