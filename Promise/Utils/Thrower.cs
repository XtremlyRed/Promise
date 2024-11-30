using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Promise;

/// <summary>
///
/// </summary>
public static class Thrower
{
    /// <summary>
    /// <see langword="throw"/> <see langword="when"/> <paramref name="string"/> is null or empty
    /// </summary>
    /// <param name="string"></param>
    /// <param name="argumentName"></param>
    /// <param name="caller"></param>
    /// <param name="callerFileName"></param>
    /// <param name="callerLineNumner"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void IsNullOrEmpty(
        string? @string,
        string? argumentName = null,
        [CallerMemberName] string? caller = null,
        [CallerFilePath] string? callerFileName = null,
        [CallerLineNumber] int? callerLineNumner = null
    )
    {
        if (string.IsNullOrEmpty(@string) == false)
        {
            return;
        }

        var argu = string.IsNullOrWhiteSpace(argumentName) ? caller : argumentName;

        const string nullOeEmptyMessage = "{0} is null or empty in file {1} at line {2}.";

        throw new ArgumentException(string.Format(nullOeEmptyMessage, argu, callerFileName, callerLineNumner));
    }

    /// <summary>
    /// throw <see langword="when"/> <paramref name="string"/> is null or white space
    /// </summary>
    /// <param name="string"></param>
    /// <param name="argumentName"></param>
    /// <param name="caller"></param>
    /// <param name="callerFileName"></param>
    /// <param name="callerLineNumner"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void IsNullOrWhiteSpace(
        string? @string,
        string? argumentName = null,
        [CallerMemberName] string? caller = null,
        [CallerFilePath] string? callerFileName = null,
        [CallerLineNumber] int? callerLineNumner = null
    )
    {
        if (string.IsNullOrWhiteSpace(@string) == false)
        {
            return;
        }

        var argu = string.IsNullOrWhiteSpace(argumentName) ? caller : argumentName;

        const string nullOeEmptyMessage = "{0} is null or empty in file {1} at line {2}.";

        throw new ArgumentException(string.Format(nullOeEmptyMessage, argu, callerFileName, callerLineNumner));
    }

    /// <summary>
    /// throw <see langword="when"/> <paramref name="object"/> is null
    /// </summary>
    /// <param name="object"></param>
    /// <param name="argumentName"></param>
    /// <param name="callerFileName"></param>
    /// <param name="callerLineNumner"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void IsNull<T>(T? @object, string? argumentName = null, [CallerFilePath] string? callerFileName = null, [CallerLineNumber] int? callerLineNumner = null)
        where T : class
    {
        if (@object is not null)
        {
            return;
        }

        var argu = string.IsNullOrWhiteSpace(argumentName) ? "object" : argumentName;

        const string nullOeEmptyMessage = "{0}:{1} is null in file {1} at line {2}.";

        throw new ArgumentException(string.Format(nullOeEmptyMessage, argu, callerFileName, callerLineNumner));
    }
}
