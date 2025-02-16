using System.Windows;
using System.Windows.Media;

namespace Promise.Wpf;

/// <summary>
/// an <see langword="interface"/> of <see cref="IViewLocator"/>
/// </summary>
public interface IViewLocator
{
    /// <summary>
    /// local view
    /// </summary>
    /// <param name="viewToken"></param>
    /// <returns></returns>
    Visual Locate(string viewToken);
}

/// <summary>
/// a <see langword="class"/> of <see cref="ViewLocator"/>
/// </summary>
public static class ViewLocator
{
    [DBA(Never)]
    internal static IViewLocator? viewLocator;

    /// <summary>
    /// set popup view locator when used
    /// </summary>
    /// <param name="viewLocator"></param>
    public static void SetViewLocator(IViewLocator viewLocator)
    {
        ViewLocator.viewLocator = viewLocator ?? throw new ArgumentNullException(nameof(viewLocator));
    }

    /// <summary>
    /// set popup view locator when used
    /// </summary>
    /// <param name="viewLocatorFunc"></param>
    public static void SetViewLocator(Func<string, Visual> viewLocatorFunc)
    {
        _ = viewLocatorFunc ?? throw new ArgumentNullException(nameof(viewLocator));

        ViewLocator.viewLocator = new _ViewLocator(viewLocatorFunc);
    }

    internal class _ViewLocator : IViewLocator
    {
        Func<string, Visual> func;

        public _ViewLocator(Func<string, Visual> func)
        {
            this.func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public Visual Locate(string viewToken)
        {
            return func(viewToken);
        }
    }
}
