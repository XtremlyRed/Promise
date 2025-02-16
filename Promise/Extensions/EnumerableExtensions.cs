using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace System.Linq;

/// <summary>
/// enumerable extensions
/// </summary>
public static partial class EnumerableExtensions
{
    /// <summary>
    /// Determines whether [is null or empty].
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns>
    ///   <c>true</c> if [is null or empty] [the specified source]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrEmpty(this IEnumerable? source)
    {
        if (source is null)
        {
            return true;
        }

        if (source is Array array)
        {
            return array.Length == 0;
        }
        else if (source is ICollection collection2)
        {
            return collection2.Count == 0;
        }

        foreach (object? _ in source)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether [is not null or empty].
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns>
    ///   <c>true</c> if [is not null or empty] [the specified source]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNotNullOrEmpty(this IEnumerable? source)
    {
        if (source is null)
        {
            return false;
        }

        if (source is Array array)
        {
            return array.Length != 0;
        }
        else if (source is ICollection collection2)
        {
            return collection2.Count != 0;
        }

        foreach (object? _ in source)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether [is null or empty].
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <param name="source">The source.</param>
    /// <returns>
    ///   <c>true</c> if [is null or empty] [the specified source]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource>? source)
    {
        if (source is null)
        {
            return true;
        }
        if (source is TSource[] array)
        {
            return array.Length == 0;
        }
        else if (source is IReadOnlyCollection<TSource> @readonly)
        {
            return @readonly.Count == 0;
        }
        else if (source is ICollection<TSource> collection)
        {
            return collection.Count == 0;
        }
        else if (source is ICollection collection2)
        {
            return collection2.Count == 0;
        }

        foreach (object? _ in source)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether [is not null or empty].
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <param name="source">The source.</param>
    /// <returns>
    ///   <c>true</c> if [is not null or empty] [the specified source]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNotNullOrEmpty<TSource>(this IEnumerable<TSource>? source)
    {
        if (source is null)
        {
            return false;
        }
        if (source is TSource[] array)
        {
            return array.Length != 0;
        }
        if (source is ICollection<TSource> collection)
        {
            return collection.Count != 0;
        }

        if (source is ICollection collection2)
        {
            return collection2.Count != 0;
        }

        foreach (object? _ in source)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Wheres if.
    /// </summary>
    /// <typeparam name="Target">The type of the arget.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="condition">if set to <c>true</c> [condition].</param>
    /// <param name="filter">The filter.</param>
    /// <returns></returns>
    /// 2024/2/1 10:59
    /// <exception cref="System.ArgumentNullException">
    /// source
    /// or
    /// filter
    /// </exception>
    public static IEnumerable<Target> WhereIf<Target>(this IEnumerable<Target> source, bool condition, Func<Target, bool> filter)
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = filter ?? throw new ArgumentNullException(nameof(filter));

        if (condition)
        {
            return source.Where(filter);
        }

        return source;
    }

    /// <summary>
    /// Get the position of an element in the collection and only return the position of the first matching element
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static int IndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> filter)
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = filter ?? throw new ArgumentNullException(nameof(filter));

        if (source is IList<TSource> array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (filter(array[i]))
                {
                    return i;
                }
            }
        }
        else if (source is IReadOnlyList<TSource> @readonly)
        {
            for (int i = 0; i < @readonly.Count; i++)
            {
                if (filter(@readonly[i]))
                {
                    return i;
                }
            }
        }
        var index = 0;

        foreach (TSource? item in source)
        {
            if (filter(item))
            {
                return index;
            }

            index++;
        }

        return -1;
    }

    /// <summary>
    /// try get item index of <see cref="IEnumerable{TSource}"/>
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="filter"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool TryIndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> filter, out int index)
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = filter ?? throw new ArgumentNullException(nameof(filter));

        if (source is IList<TSource> array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (filter(array[i]))
                {
                    index = i;
                    return true;
                }
            }
        }
        else if (source is IReadOnlyList<TSource> @readonly)
        {
            for (int i = 0; i < @readonly.Count; i++)
            {
                if (filter(@readonly[i]))
                {
                    index = i;
                    return true;
                }
            }
        }

        var forIndex = 0;
        index = -1;

        foreach (TSource? item in source)
        {
            if (filter(item))
            {
                index = forIndex;
                return true;
            }

            forIndex++;
        }

        return false;
    }

    /// <summary>
    /// Get the position of elements in the collection and return the positions of all matching elements
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<int> IndexOfMany<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> filter)
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = filter ?? throw new ArgumentNullException(nameof(filter));

        if (source is IList<TSource> array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (filter(array[i]))
                {
                    yield return i;
                }
            }

            yield break;
        }
        else if (source is IReadOnlyList<TSource> @readonly)
        {
            for (int i = 0; i < @readonly.Count; i++)
            {
                if (filter(@readonly[i]))
                {
                    yield return i;
                }
            }

            yield break;
        }
        else
        {
            var index = 0;

            foreach (TSource? item in source)
            {
                if (filter(item))
                {
                    yield return index;
                }

                index++;
            }
        }
    }

    /// <summary>
    /// paging
    /// </summary>
    /// <typeparam name="Target">The type of the arget.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="pageIndex">Index of the page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns></returns>
    /// 2024/2/1 11:00
    /// <exception cref="System.ArgumentNullException">source</exception>
    public static IEnumerable<Target> Paginate<Target>(this IEnumerable<Target> source, int pageIndex, int pageSize)
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));

        return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
    }

    /// <summary>
    /// Fors the each.
    /// </summary>
    /// <typeparam name="Target">The type of the arget.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="action">The taskfunc.</param>
    [DebuggerNonUserCode]
    public static void ForEach<Target>(this IEnumerable<Target> source, Action<Target> action)
    {
        if (source is null || action is null)
        {
            return;
        }

        if (source is Target[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                action(array[i]);
            }
        }
        else if (source is IReadOnlyList<Target> @readonly)
        {
            for (int i = 0; i < @readonly.Count; i++)
            {
                action(@readonly[i]);
            }
        }
        else if (source is IList<Target> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                action(list[i]);
            }

            return;
        }
        else
        {
            foreach (Target item in source)
            {
                action(item);
            }
        }
    }

    /// <summary>
    /// Fors the each.
    /// </summary>
    /// <typeparam name="Target">The type of the arget.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="action">The taskfunc.</param>
    [DebuggerNonUserCode]
    public static void ForEach<Target>(this IEnumerable<Target> source, Action<Target, int> action)
    {
        if (source is null || action is null)
        {
            return;
        }

        if (source is Target[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                action(array[i], i);
            }
        }
        else if (source is IReadOnlyList<Target> @readonly)
        {
            for (int i = 0; i < @readonly.Count; i++)
            {
                action(@readonly[i], i);
            }
        }
        else if (source is IList<Target> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                action(list[i], i);
            }
        }
        else
        {
            int index = 0;
            foreach (Target item in source)
            {
                index++;
                action(item, index);
            }
        }
    }

    /// <summary>
    /// for each
    /// </summary>
    /// <param name="source"></param>
    /// <param name="action"></param>
    public static void ForEach(this IEnumerable source, Action<object?> action)
    {
        if (source is null || action is null)
        {
            return;
        }

        if (source is IList list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                action(list[i]);
            }

            return;
        }

        foreach (var item in source)
        {
            action(item);
        }
    }

    /// <summary>
    /// for each
    /// </summary>
    /// <param name="source"></param>
    /// <param name="action"></param>
    public static void ForEach(this IEnumerable source, Action<object?, int> action)
    {
        if (source is null || action is null)
        {
            return;
        }

        if (source is IList list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                action(list[i], i);
            }

            return;
        }
        var index = 0;
        foreach (var item in source)
        {
            action(item, index++);
        }
    }

#if !NET6_0_OR_GREATER

    /// <summary>
    /// Segments the specified segment capacity.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <param name="targets">The targets.</param>
    /// <param name="segmentSize">The segment capacity.</param>
    /// <returns></returns>
    public static IEnumerable<TSource[]> Chunk<TSource>(this IEnumerable<TSource> targets, int segmentSize)
    {
        if (targets is null || segmentSize < 1)
        {
            yield break;
        }

        using IEnumerator<TSource> enumerator = targets.GetEnumerator();

        int currentIndex = 0;
        TSource[] ARRAY = new TSource[segmentSize];
        while (enumerator.MoveNext())
        {
            ARRAY[currentIndex] = enumerator.Current;
            if (++currentIndex == segmentSize)
            {
                yield return ARRAY;

                ARRAY = new TSource[segmentSize];
                currentIndex = 0;
            }
        }

        if (currentIndex > 0)
        {
            Array.Resize(ref ARRAY, currentIndex);

            yield return ARRAY;
        }
    }

    /// <summary>
    /// returns the maximum value in a generic sequence according to a specified key selector function.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static TSource? MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer = null)
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

        comparer ??= Comparer<TKey>.Default;

        using IEnumerator<TSource> e = source.GetEnumerator();

        if (e.MoveNext() == false)
        {
            throw new ArgumentNullException("source contains no elements");
        }

        TSource value = e.Current;
        TKey key = keySelector(value);

        if (default(TKey) is null)
        {
            if (key is null)
            {
                TSource firstValue = value;

                do
                {
                    if (e.MoveNext() == false)
                    {
                        return firstValue;
                    }

                    value = e.Current;
                    key = keySelector(value);
                } while (key is null);
            }

            while (e.MoveNext())
            {
                TSource nextValue = e.Current;
                TKey nextKey = keySelector(nextValue);
                if (nextKey is not null && comparer.Compare(nextKey, key) > 0)
                {
                    key = nextKey;
                    value = nextValue;
                }
            }
        }
        else
        {
            if (comparer == Comparer<TKey>.Default)
            {
                while (e.MoveNext())
                {
                    TSource nextValue = e.Current;
                    TKey nextKey = keySelector(nextValue);
                    if (Comparer<TKey>.Default.Compare(nextKey, key) > 0)
                    {
                        key = nextKey;
                        value = nextValue;
                    }
                }
            }
            else
            {
                while (e.MoveNext())
                {
                    TSource nextValue = e.Current;
                    TKey nextKey = keySelector(nextValue);
                    if (comparer.Compare(nextKey, key) > 0)
                    {
                        key = nextKey;
                        value = nextValue;
                    }
                }
            }
        }

        return value;
    }

    /// <summary>
    /// returns the minimum value in a generic sequence according to a specified key selector function.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static TSource? MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer = null)
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

        comparer ??= Comparer<TKey>.Default;

        using IEnumerator<TSource> e = source.GetEnumerator();

        if (e.MoveNext() == false)
        {
            throw new ArgumentNullException("source contains no elements");
        }

        TSource value = e.Current;
        TKey key = keySelector(value);

        if (default(TKey) is null)
        {
            if (key is null)
            {
                TSource firstValue = value;

                do
                {
                    if (!e.MoveNext())
                    {
                        return firstValue;
                    }

                    value = e.Current;
                    key = keySelector(value);
                } while (key is null);
            }

            while (e.MoveNext())
            {
                TSource nextValue = e.Current;
                TKey nextKey = keySelector(nextValue);
                if (nextKey is not null && comparer.Compare(nextKey, key) < 0)
                {
                    key = nextKey;
                    value = nextValue;
                }
            }
        }
        else
        {
            if (comparer == Comparer<TKey>.Default)
            {
                while (e.MoveNext())
                {
                    TSource nextValue = e.Current;
                    TKey nextKey = keySelector(nextValue);
                    if (Comparer<TKey>.Default.Compare(nextKey, key) < 0)
                    {
                        key = nextKey;
                        value = nextValue;
                    }
                }
            }
            else
            {
                while (e.MoveNext())
                {
                    TSource nextValue = e.Current;
                    TKey nextKey = keySelector(nextValue);
                    if (comparer.Compare(nextKey, key) < 0)
                    {
                        key = nextKey;
                        value = nextValue;
                    }
                }
            }
        }

        return value;
    }
#endif

    /// <summary>
    /// Clears the specified source.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    public static void Clear<T>(this IProducerConsumerCollection<T> source)
    {
        if (source is null || source.Count == 0)
        {
            return;
        }

        while (source.TryTake(out _)) { }
    }

    /// <summary>
    /// add items to collection
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="sources"></param>
    /// <param name="targets"></param>
    public static void AddRange<Target>(this Collection<Target> sources, params Target[] targets)
    {
        if (sources is null || targets is null || targets.Length == 0)
        {
            return;
        }

        for (int i = 0; i < targets.Length; i++)
        {
            sources.Add(targets[i]);
        }
    }

    /// <summary>
    /// add items to collection
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="sources"></param>
    /// <param name="targets"></param>
    public static void AddRange<Target>(this Collection<Target> sources, IEnumerable<Target> targets)
    {
        if (sources is null || targets is null)
        {
            return;
        }

        foreach (var item in targets)
        {
            sources.Add(item);
        }
    }

    /// <summary>
    /// to <see cref="IReadOnlyList{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> source)
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));

        if (source is IList<T> list2)
        {
            //
            return new ReadOnlyCollection<T>(list2);
        }

        return new ReadOnlyCollection<T>(source.ToArray());
    }

    /// <summary>
    /// to <see cref="ToReadOnlayDictionary{TKey, TValue}"/>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IReadOnlyDictionary<TKey, TValue> ToReadOnlayDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        where TKey : notnull
    {
        _ = dict ?? throw new ArgumentNullException(nameof(dict));

        if (dict is IReadOnlyDictionary<TKey, TValue> read)
        {
            //
            return read;
        }

        return new ReadOnlyDictionary<TKey, TValue>(dict);
    }
}
