﻿using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Windows;

namespace Promise.Wpf;

/// <summary>
/// a class of <see cref="MediaConverter{From,To}"/>
/// </summary>
public abstract class MediaConverter<From, To> : IValueConverter
    where From : notnull
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly ConcurrentDictionary<From, To> storages = new();

    /// <summary>
    /// null value
    /// </summary>
    public object? Null { get; set; }

    /// <summary>
    /// media convert
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="InvalidCastException"></exception>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not From fromValue)
        {
            return Null!;
        }

        if (storages.TryGetValue(fromValue, out To? targetValue) == false)
        {
            storages[fromValue] = targetValue = ConvertFrom(fromValue);
        }

        return targetValue!;
    }

    object IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// convert from
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    protected abstract To ConvertFrom(From from);
}
