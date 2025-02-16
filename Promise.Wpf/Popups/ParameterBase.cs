using System.Diagnostics;
using System.Windows.Media;

namespace Promise.Wpf;

/// <summary>
/// a base <see langword="class"/> of <see cref="ParameterBase"/>
/// </summary>
public abstract class ParameterBase
{
    [DBA(Never)]
    Dictionary<string, object> variableTable = new Dictionary<string, object>();

    /// <summary>
    /// <see langword="try"/> get value by <paramref name="variableName"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="variableName"></param>
    /// <param name="targetValue"></param>
    /// <returns></returns>
    public bool TryGetValue<T>(string variableName, out T targetValue)
    {
        if (variableTable.TryGetValue(variableName, out var value) && value is T tar)
        {
            targetValue = tar;
            return true;
        }

        targetValue = default!;
        return false;
    }

    /// <summary>
    /// get value by <paramref name="variableName"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="variableName"></param>
    /// <returns></returns>
    public T GetValue<T>(string variableName)
    {
        return (T)variableTable[variableName];
    }

    /// <summary>
    /// set value by <paramref name="variableName"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="variableName"></param>
    /// <param name="value"></param>
    public void SetValue<T>(string variableName, T value)
    {
        variableTable[variableName] = value!;
    }

    /// <summary>
    /// <see langword="remove"/> <paramref name="variableName"/>
    /// </summary>
    /// <param name="variableName"></param>
    public void Remove(string variableName)
    {
        variableTable.Remove(variableName);
    }

    /// <summary>
    /// contains <paramref name="variableName"/>
    /// </summary>
    /// <param name="variableName"></param>
    /// <returns></returns>
    public bool Contains(string variableName)
    {
        return variableTable.ContainsKey(variableName);
    }

    /// <summary>
    /// clear all variables
    /// </summary>
    public void Clear()
    {
        variableTable.Clear();
    }
}
