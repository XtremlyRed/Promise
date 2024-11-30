using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.BindingFlags;

namespace Promise;

/// <summary>
/// <see cref="Enum"/> analyzer
/// </summary>
/// <typeparam name="T"></typeparam>
public static class EnumDescriptor<T>
    where T : struct, Enum
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    static EnumAttribute[] enumAttributes = Array.Empty<EnumAttribute>();

    /// <summary>
    ///
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    static EnumDescriptor()
    {
        if (typeof(T).IsEnum == false)
        {
            throw new ArgumentException("T must be an enumerated type");
        }

        var values = Enum.GetValues(typeof(T)).OfType<T>().ToArray();

        Values = new ReadOnlyCollection<T>(values);

        var fieldInfos = typeof(T).GetFields(Public | Static).Where(i => i.IsStatic).ToArray();

        FieldInfos = new ReadOnlyCollection<FieldInfo>(fieldInfos);

        Names = new ReadOnlyCollection<string>(fieldInfos.Select(i => i.Name).ToArray());

        enumAttributes = new EnumAttribute[fieldInfos.Length];

        for (int i = 0; i < fieldInfos.Length; i++)
        {
            var fieldInfo = fieldInfos[i];

            var value = (T)fieldInfo.GetValue(null)!;

            var attributes = fieldInfo.GetCustomAttributes().ToArray();

            var enumAttribute = new EnumAttribute(value, attributes);

            enumAttributes[i] = enumAttribute;
        }
    }

    /// <summary>
    /// all values
    /// </summary>
    public static readonly IReadOnlyList<T> Values;

    /// <summary>
    /// all fieldInfos
    /// </summary>
    public static readonly IReadOnlyList<FieldInfo> FieldInfos;

    /// <summary>
    /// all enum name
    /// </summary>
    public static readonly IReadOnlyList<string> Names;

    /// <summary>
    /// get target <see cref="Attribute"/> maps
    /// </summary>
    /// <typeparam name="Attribute"></typeparam>
    /// <returns></returns>
    public static IEnumerable<EnumAttribute<Attribute>> GetAttributes<Attribute>()
        where Attribute : System.Attribute
    {
        for (int i = 0; i < enumAttributes.Length; i++)
        {
            for (int j = 0; j < enumAttributes[i].Attributes.Length; j++)
            {
                if (enumAttributes[i].Attributes is Attribute attribute)
                {
                    yield return new EnumAttribute<Attribute>(enumAttributes[i].Value, attribute);
                    break;
                }
            }
        }
    }

    record struct EnumAttribute(T Value, Attribute[] Attributes);

    /// <summary>
    /// enum attribute
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="Value"></param>
    /// <param name="Attribute"></param>
    public record struct EnumAttribute<TAttribute>(T Value, TAttribute Attribute)
        where TAttribute : Attribute;
}
