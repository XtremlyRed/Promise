using System;

namespace System;

/// <summary>
/// simple math extensions
/// </summary>
public static partial class MathExtensions
{
    /// <summary>
    /// get value from range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <returns></returns>
    public static long FromRange(this long value, long minValue = long.MinValue, long maxValue = long.MaxValue)
    {
        return value < minValue ? minValue
            : value > maxValue ? maxValue
            : value;
    }

    /// <summary>
    /// get value from range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <returns></returns>
    public static int FromRange(this int value, int minValue = int.MinValue, int maxValue = int.MaxValue)
    {
        return value < minValue ? minValue
            : value > maxValue ? maxValue
            : value;
    }

    /// <summary>
    /// get value from range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <returns></returns>
    public static byte FromRange(this byte value, byte minValue = byte.MinValue, byte maxValue = byte.MaxValue)
    {
        return value < minValue ? minValue
            : value > maxValue ? maxValue
            : value;
    }

    /// <summary>
    /// get value from range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <returns></returns>
    public static short FromRange(this short value, short minValue = short.MinValue, short maxValue = short.MaxValue)
    {
        return value < minValue ? minValue
            : value > maxValue ? maxValue
            : value;
    }

    /// <summary>
    /// get value from range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <returns></returns>
    public static float FromRange(this float value, float minValue = float.MinValue, float maxValue = float.MaxValue)
    {
        return value < minValue ? minValue
            : value > maxValue ? maxValue
            : value;
    }

    /// <summary>
    /// get value from range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <returns></returns>
    public static decimal FromRange(this decimal value, decimal minValue = decimal.MinValue, decimal maxValue = decimal.MaxValue)
    {
        return value < minValue ? minValue
            : value > maxValue ? maxValue
            : value;
    }

    /// <summary>
    /// get value from range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <returns></returns>
    public static double FromRange(this double value, double minValue = double.MinValue, double maxValue = double.MaxValue)
    {
        return value < minValue ? minValue
            : value > maxValue ? maxValue
            : value;
    }

    /// <summary>
    /// get value from range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <returns></returns>
    public static ulong FromRange(this ulong value, ulong minValue = ulong.MinValue, ulong maxValue = ulong.MaxValue)
    {
        return value < minValue ? minValue
            : value > maxValue ? maxValue
            : value;
    }

    /// <summary>
    /// get value from range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <returns></returns>
    public static uint FromRange(this uint value, uint minValue = uint.MinValue, uint maxValue = uint.MaxValue)
    {
        return value < minValue ? minValue
            : value > maxValue ? maxValue
            : value;
    }

    /// <summary>
    /// get value from range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <returns></returns>
    public static sbyte FromRange(this sbyte value, sbyte minValue = sbyte.MinValue, sbyte maxValue = sbyte.MaxValue)
    {
        return value < minValue ? minValue
            : value > maxValue ? maxValue
            : value;
    }

    /// <summary>
    /// get value from range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <returns></returns>
    public static ushort FromRange(this ushort value, ushort minValue = ushort.MinValue, ushort maxValue = ushort.MaxValue)
    {
        return value < minValue ? minValue
            : value > maxValue ? maxValue
            : value;
    }

    /// <summary>
    /// check value in range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <param name="includeEquals"></param>
    /// <returns></returns>
    public static bool InRange(this long value, long minValue, long maxValue, bool includeEquals = true)
    {
        return includeEquals ? (value >= minValue) && (value <= maxValue) : (value > minValue) && (value < maxValue);
    }

    /// <summary>
    /// check value in range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <param name="includeEquals"></param>
    /// <returns></returns>
    public static bool InRange(this int value, int minValue, int maxValue, bool includeEquals = true)
    {
        return includeEquals ? (value >= minValue) && (value <= maxValue) : (value > minValue) && (value < maxValue);
    }

    /// <summary>
    /// check value in range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <param name="includeEquals"></param>
    /// <returns></returns>
    public static bool InRange(this byte value, byte minValue, byte maxValue, bool includeEquals = true)
    {
        return includeEquals ? (value >= minValue) && (value <= maxValue) : (value > minValue) && (value < maxValue);
    }

    /// <summary>
    /// check value in range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <param name="includeEquals"></param>
    /// <returns></returns>
    public static bool InRange(this short value, short minValue, short maxValue, bool includeEquals = true)
    {
        return includeEquals ? (value >= minValue) && (value <= maxValue) : (value > minValue) && (value < maxValue);
    }

    /// <summary>
    /// check value in range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <param name="includeEquals"></param>
    /// <returns></returns>
    public static bool InRange(this float value, float minValue, float maxValue, bool includeEquals = true)
    {
        return includeEquals ? (value >= minValue) && (value <= maxValue) : (value > minValue) && (value < maxValue);
    }

    /// <summary>
    /// check value in range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <param name="includeEquals"></param>
    /// <returns></returns>
    public static bool InRange(this decimal value, decimal minValue, decimal maxValue, bool includeEquals = true)
    {
        return includeEquals ? (value >= minValue) && (value <= maxValue) : (value > minValue) && (value < maxValue);
    }

    /// <summary>
    /// check value in range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <param name="includeEquals"></param>
    /// <returns></returns>
    public static bool InRange(this double value, double minValue, double maxValue, bool includeEquals = true)
    {
        return includeEquals ? (value >= minValue) && (value <= maxValue) : (value > minValue) && (value < maxValue);
    }

    /// <summary>
    /// check value in range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <param name="includeEquals"></param>
    /// <returns></returns>
    public static bool InRange(this ulong value, ulong minValue, ulong maxValue, bool includeEquals = true)
    {
        return includeEquals ? (value >= minValue) && (value <= maxValue) : (value > minValue) && (value < maxValue);
    }

    /// <summary>
    /// check value in range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <param name="includeEquals"></param>
    /// <returns></returns>
    public static bool InRange(this uint value, uint minValue, uint maxValue, bool includeEquals = true)
    {
        return includeEquals ? (value >= minValue) && (value <= maxValue) : (value > minValue) && (value < maxValue);
    }

    /// <summary>
    /// check value in range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <param name="includeEquals"></param>
    /// <returns></returns>
    public static bool InRange(this sbyte value, sbyte minValue, sbyte maxValue, bool includeEquals = true)
    {
        return includeEquals ? (value >= minValue) && (value <= maxValue) : (value > minValue) && (value < maxValue);
    }

    /// <summary>
    /// check value in range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <param name="includeEquals"></param>
    /// <returns></returns>
    public static bool InRange(this ushort value, ushort minValue, ushort maxValue, bool includeEquals = true)
    {
        return includeEquals ? (value >= minValue) && (value <= maxValue) : (value > minValue) && (value < maxValue);
    }

    /// <summary>
    ///  get absolute value
    /// </summary>
    /// <param name="value">current value</param>
    /// <returns></returns>
    public static short Abs(this short value)
    {
        return Math.Abs(value);
    }

    /// <summary>
    ///  get absolute value
    /// </summary>
    /// <param name="value">current value</param>
    /// <returns></returns>
    public static sbyte Abs(this sbyte value)
    {
        return Math.Abs(value);
    }

    /// <summary>
    ///  get absolute value
    /// </summary>
    /// <param name="value">current value</param>
    /// <returns></returns>
    public static int Abs(this int value)
    {
        return Math.Abs(value);
    }

    /// <summary>
    ///  get  absolute value
    /// </summary>
    /// <param name="value">current value</param>
    /// <returns></returns>
    public static long Abs(this long value)
    {
        return Math.Abs(value);
    }

    /// <summary>
    ///  get absolute value
    /// </summary>
    /// <param name="value">current value</param>
    /// <returns></returns>
    public static float Abs(this float value)
    {
        return Math.Abs(value);
    }

    /// <summary>
    ///  get absolute value
    /// </summary>
    /// <param name="value">current value</param>
    /// <returns></returns>
    public static double Abs(this double value)
    {
        return Math.Abs(value);
    }

    /// <summary>
    ///  get absolute value
    /// </summary>
    /// <param name="value">current value</param>
    /// <returns></returns>
    public static decimal Abs(this decimal value)
    {
        return Math.Abs(value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <param name="decimals"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static decimal Round(this decimal value, int decimals = 2, MidpointRounding mode = MidpointRounding.AwayFromZero)
    {
        return Math.Round(value, decimals, mode);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <param name="decimals"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static double Round(this double value, int decimals = 2, MidpointRounding mode = MidpointRounding.AwayFromZero)
    {
        return Math.Round(value, decimals, mode);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <param name="decimals"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static float Round(this float value, int decimals = 2, MidpointRounding mode = MidpointRounding.AwayFromZero)
    {
        return (float)Math.Round((double)value, decimals, mode);
    }

    /// <summary>
    /// is number
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsNumber(object? obj)
    {
        return IsNumber(obj, out _);
    }

    /// <summary>
    /// is number
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="doubleValue"></param>
    /// <returns></returns>
    public static bool IsNumber(object? obj, out double doubleValue)
    {
        doubleValue = 0d;
        if (obj is null)
        {
            return false;
        }

        if (obj is sbyte @sbyte)
        {
            doubleValue = @sbyte;
            return true;
        }

        if (obj is byte @byte)
        {
            doubleValue = @byte;
            return true;
        }

        if (obj is short @short)
        {
            doubleValue = @short;
            return true;
        }
        if (obj is ushort @ushort)
        {
            doubleValue = @ushort;
            return true;
        }
        if (obj is int @int)
        {
            doubleValue = @int;
            return true;
        }
        if (obj is uint @uint)
        {
            doubleValue = @uint;
            return true;
        }
        if (obj is long @long)
        {
            doubleValue = @long;
            return true;
        }
        if (obj is ulong @ulong)
        {
            doubleValue = @ulong;
            return true;
        }
        if (obj is float @false)
        {
            doubleValue = @false;
            return true;
        }
        if (obj is double @double)
        {
            doubleValue = @double;
            return true;
        }
        if (obj is decimal @decimal)
        {
            doubleValue = (double)@decimal;
            return true;
        }

        return false;
    }
}
