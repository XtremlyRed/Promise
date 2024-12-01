using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Promise.Wpf;

/// <summary>
/// a class of <see cref="RangeConverter{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class RangeConverter<T> : TrueFalseConverter<T>
    where T : IComparable
{
    /// <summary>
    /// min value
    /// </summary>
    public T MinValue
    {
        get { return (T)GetValue(MinValueProperty)!; }
        set { SetValue(MinValueProperty, value); }
    }

    /// <summary>
    /// min value
    /// </summary>

    public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(T), typeof(RangeConverter<T>), new PropertyMetadata(default(T)!));

    /// <summary>
    /// max value
    /// </summary>
    public T MaxValue
    {
        get { return (T)GetValue(MaxValueProperty)!; }
        set { SetValue(MaxValueProperty, value); }
    }

    /// <summary>
    /// max value
    /// </summary>

    public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(T), typeof(RangeConverter<T>), new PropertyMetadata(default(T)!));

    /// <summary>
    /// value convert
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected override object? Convert(T value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value.CompareTo(MinValue) >= 0 && MaxValue.CompareTo(value) >= 0)
        {
            return True;
        }

        return False;
    }
}
