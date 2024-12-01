using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Promise.Wpf;

/// <summary>
/// a class of <see cref="BooleanConverter"/>
/// </summary>
/// <seealso cref="TrueFalseConverter{Boolean}" />
public class BooleanConverter : TrueFalseConverter<bool>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    protected override object? Convert(bool value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value ? True : False;
    }
}

/// <summary>
/// a class of <see cref="BooleanToVisibilityConverter"/>
/// </summary>
/// <seealso cref="ValueConverterBase{Boolean}" />
public class BooleanToVisibilityConverter : ValueConverterBase<bool>
{
    /// <summary>
    /// convert
    /// </summary>
    /// <param name="boolValue"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected override object? Convert(bool boolValue, Type targetType, object? parameter, CultureInfo culture)
    {
        return boolValue ? Visibility.Visible : Visibility.Collapsed;
    }
}
