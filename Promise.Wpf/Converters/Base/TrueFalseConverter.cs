using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Promise.Wpf;

/// <summary>
/// a class of <see cref="TrueFalseConverter{T}"/>
/// </summary>
/// <seealso cref="IValueConverter" />

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class TrueFalseConverter<T> : ValueConverterBase<T>, IValueConverter
{
    /// <summary>
    /// true value
    /// </summary>
    public object True
    {
        get => GetValue(TrueProperty)!;
        set => SetValue(TrueProperty, value);
    }

    /// <summary>
    ///  false value
    /// </summary>
    public object False
    {
        get => GetValue(FalseProperty)!;
        set => SetValue(FalseProperty, value);
    }

    /// <summary>
    /// The true property
    /// </summary>
    private static readonly DependencyProperty TrueProperty = DependencyProperty.Register("True", typeof(object), typeof(TrueFalseConverter<T>), new PropertyMetadata(null));

    /// <summary>
    /// The false property
    /// </summary>
    private static readonly DependencyProperty FalseProperty = DependencyProperty.Register("False", typeof(object), typeof(TrueFalseConverter<T>), new PropertyMetadata(null));
}
