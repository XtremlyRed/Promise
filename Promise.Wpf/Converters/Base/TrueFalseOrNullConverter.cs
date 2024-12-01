using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Promise.Wpf;

/// <summary>
/// a class of <see cref="TrueFalseOrNullConverter{T}"/>
/// </summary>
/// <seealso cref="IValueConverter" />

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class TrueFalseOrNullConverter<T> : TrueFalseConverter<T>, IValueConverter
{
    /// <summary>
    ///  null value
    /// </summary>
    public object? Null
    {
        get { return GetValue(NullProperty); }
        set { SetValue(NullProperty, value); }
    }

    /// <summary>
    /// The null property
    /// </summary>
    public static readonly DependencyProperty NullProperty = DependencyProperty.Register("Null", typeof(object), typeof(TrueFalseOrNullConverter<T>), new PropertyMetadata(null));
}
