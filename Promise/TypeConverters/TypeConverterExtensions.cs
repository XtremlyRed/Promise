using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Promise;

/// <summary>
/// a <see langword="class"/> of <see cref="TypeConverterExtensions"/>
/// </summary>
public static class TypeConverterExtensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type, TypeConverter>> typeConvertMaps = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly string[] typeCodeNames = Enum.GetNames(typeof(TypeCode));

    /// <summary>
    /// convert <see cref="object"/> to <typeparamref name="To"/>
    /// </summary>
    /// <typeparam name="To"></typeparam>
    /// <param name="from"></param>
    /// <returns></returns>
    /// <exception cref="InvalidCastException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static To ConvertTo<To>(this object? from)
    {
        if (from is To to)
        {
            return to;
        }

        if (ConvertTo(from, typeof(To)) is To toValue)
        {
            return toValue;
        }

        throw new InvalidCastException("type conversion unsuccessful");
    }

    /// <summary>
    /// convert <see cref="object"/> to  <paramref name="toType"/> />
    /// </summary>
    /// <param name="from"></param>
    /// <param name="toType"></param>
    /// <returns></returns>
    /// <exception cref="InvalidCastException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static object ConvertTo(this object? from, Type toType)
    {
        _ = from ?? throw new ArgumentNullException(nameof(from));

        if (from is IConvertible convertible && typeCodeNames.Contains(toType.Name))
        {
            var converted = convertible.ToType(toType, CultureInfo.CurrentCulture);

            if (converted is not null)
            {
                return converted;
            }
        }

        Type fromType = from.GetType();

        if (typeConvertMaps.TryGetValue(fromType, out ConcurrentDictionary<Type, TypeConverter>? fromTypeConverterStorages) == false)
        {
            typeConvertMaps[fromType] = fromTypeConverterStorages = new ConcurrentDictionary<Type, TypeConverter>();
        }

        if (fromTypeConverterStorages.TryGetValue(toType, out TypeConverter? toTypeConverter) == false)
        {
            toTypeConverter = TypeDescriptor.GetConverter(toType) ?? throw new InvalidOperationException("type converter not registered");

            fromTypeConverterStorages[toType] = toTypeConverter;
        }

        if (toTypeConverter.CanConvertFrom(fromType))
        {
            return toTypeConverter.ConvertFrom(from)!;
        }

        throw new InvalidOperationException($"type converter from {fromType} to {toType} not registered");
    }

    /// <summary>
    /// appen custom converter
    /// </summary>
    /// <typeparam name="TFrom"></typeparam>
    /// <typeparam name="TTo"></typeparam>
    /// <param name="converter"></param>
    public static void AppendConverter<TFrom, TTo>(Func<TFrom, TTo> converter)
    {
        var fromType = typeof(TFrom);
        var toType = typeof(TTo);

        if (typeConvertMaps.TryGetValue(fromType, out ConcurrentDictionary<Type, TypeConverter>? targetTypeConverterMaps) == false)
        {
            typeConvertMaps[fromType] = targetTypeConverterMaps = new ConcurrentDictionary<Type, TypeConverter>();
        }

        targetTypeConverterMaps[toType] = new CustomTypeConverter<TFrom, TTo>(converter);
    }

    private class CustomTypeConverter<TFrom, TTo> : TypeConverter
    {
        Func<TFrom, TTo> converter;
        static Type FromType = typeof(TFrom);
        static Type ToType = typeof(TTo);

        public CustomTypeConverter(Func<TFrom, TTo> converter)
        {
            this.converter = converter;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if (sourceType == FromType)
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is TFrom from)
            {
                return converter(from);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
