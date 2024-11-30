using System.ComponentModel;

namespace System;

/// <summary>
/// string extensions
/// </summary>
/// 2023/12/19 15:22
public static class StringExtensions
{
    /// <summary>
    /// Check if the string is <see langword="null"/> or a whitespace character
    /// </summary>
    /// <param name="value">The value.</param>
    public static bool IsNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Check if the string is not <see langword="null"/> or a whitespace character
    /// </summary>
    /// <param name="value">The value.</param>
    public static bool IsNotNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value) == false;
    }

    /// <summary>
    /// Check if the string is <see langword="null"/>or empty
    /// </summary>
    /// <param name="value">The value.</param>
    public static bool IsNullOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Check if the string is not <see langword="null"/> or empty
    /// </summary>
    /// <param name="value">The value.</param>
    public static bool IsNotNullOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value) == false;
    }

    /// <summary>
    /// Joins the specified interval symbol.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <param name="intervalSymbol">The interval symbol.</param>
    /// <returns></returns>
    /// 2023/12/19 15:23
    public static string Join<T>(this IEnumerable<T> source, string intervalSymbol = ",")
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = intervalSymbol ?? throw new ArgumentNullException(nameof(intervalSymbol));

        return string.Join(intervalSymbol, source);
    }

    /// <summary>
    /// Joins the specified selector.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <param name="intervalSymbol"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string Join<T>(this IEnumerable<T> source, Func<T, string> selector, string intervalSymbol = ",")
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = selector ?? throw new ArgumentNullException(nameof(selector));
        _ = intervalSymbol ?? throw new ArgumentNullException(nameof(intervalSymbol));

        return string.Join(intervalSymbol, source.Select(selector));
    }
}
