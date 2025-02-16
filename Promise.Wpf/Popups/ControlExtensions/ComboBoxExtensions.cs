using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using static System.Reflection.BindingFlags;

namespace Promise.Wpf;

/// <summary>
/// a <see langword="class"/> of <see cref="ComboBoxExtensions"/>
/// </summary>
public static class ComboBoxExtensions
{
    /// <summary>
    /// get <see langword="enum"/> value from <paramref name="comboBox"/>
    /// </summary>
    /// <param name="comboBox"></param>
    /// <returns></returns>
    public static object? GetEnumValue(ComboBox comboBox)
    {
        return (object?)comboBox.GetValue(EnumValueProperty);
    }

    /// <summary>
    /// set <see langword="enum"/> <paramref name="value"/> to <paramref name="comboBox"/>
    /// </summary>
    /// <param name="comboBox"></param>
    /// <param name="value"></param>
    public static void SetEnumValue(ComboBox comboBox, object? value)
    {
        comboBox.SetValue(EnumValueProperty, value);
    }

    /// <summary>
    /// <see langword="enum"/> value
    /// </summary>
    public static readonly DependencyProperty EnumValueProperty = DependencyProperty.RegisterAttached(
        "EnumValue",
        typeof(object),
        typeof(ComboBoxExtensions),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            (s, e) =>
            {
                if (s is ComboBox comboBox && e.NewValue?.GetHashCode() != e.OldValue?.GetHashCode())
                {
                    Initialize(comboBox, null!, e.NewValue, null);
                }
            }
        )
    );

    /// <summary>
    /// get <see langword="enum"/> type from <see cref="ComboBox"/>
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Type GetEnumType(ComboBox obj)
    {
        return (Type)obj.GetValue(EnumTypeProperty);
    }

    /// <summary>
    /// set <see langword="enum"/> type to <see cref="ComboBox"/>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetEnumType(ComboBox obj, Type value)
    {
        obj.SetValue(EnumTypeProperty, value);
    }

    /// <summary>
    /// <see langword="enum"/> type
    /// </summary>
    public static readonly DependencyProperty EnumTypeProperty = DependencyProperty.RegisterAttached(
        "EnumType",
        typeof(Type),
        typeof(ComboBoxExtensions),
        new PropertyMetadata(
            null,
            (s, e) =>
            {
                if (s is not ComboBox comboBox)
                {
                    return;
                }

                if (e.NewValue is Type enumType)
                {
                    Initialize(comboBox, enumType, null, null);
                }

                if (e.OldValue is not Type oldEnumType)
                {
                    comboBox.SelectionChanged += ComboBox_SelectionChanged;
                }
            }
        )
    );

    /// <summary>
    /// display ignores
    /// </summary>
    /// <param name="comboBox"></param>
    /// <returns></returns>
    public static IEnumerable GetIgnores(ComboBox comboBox)
    {
        return (IEnumerable)comboBox.GetValue(IgnoresProperty);
    }

    /// <summary>
    /// display ignores
    /// </summary>
    /// <param name="comboBox"></param>
    /// <param name="value"></param>
    public static void SetIgnores(ComboBox comboBox, IEnumerable value)
    {
        comboBox.SetValue(IgnoresProperty, value);
    }

    /// <summary>
    /// display ignores
    /// </summary>
    public static readonly DependencyProperty IgnoresProperty = DependencyProperty.RegisterAttached(
        "Ignores",
        typeof(IEnumerable),
        typeof(ComboBoxExtensions),
        new PropertyMetadata(
            null,
            (s, e) =>
            {
                if (s is ComboBox comboBox)
                {
                    Initialize(comboBox, null!, null, e.NewValue as IEnumerable);
                }
            }
        )
    );

    /// <summary>
    /// initialize display item
    /// </summary>
    /// <param name="comboBox"></param>
    /// <param name="enumType"></param>
    /// <param name="enumValue"></param>
    /// <param name="ignores"></param>
    private static void Initialize(ComboBox comboBox, Type enumType, object? enumValue, IEnumerable? ignores)
    {
        if (enumType is not null)
        {
            comboBox.Items.Clear();

            var sourceItems = enumType
                .GetFields(Public | Static)
                .Where(x => x.IsStatic && x.IsPublic)
                .Where(x => x is not null)
                .Select(x => new
                {
                    IsBrowsable = x.GetCustomAttribute<BrowsableAttribute>()?.Browsable ?? true,
                    DisplayName = x.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? x.Name,
                    Value = x.GetValue(null),
                })
                .Where(x => x.IsBrowsable)
                .Select(x => new DisplayItem(x.Value!, x.DisplayName))
                .ToArray();

            comboBox.DisplayMemberPath = nameof(DisplayItem.DisplayName);

            comboBox.ItemsSource = new ObservableCollection<DisplayItem>(sourceItems);
        }

        if (ignores is not null && comboBox.ItemsSource is IList<DisplayItem> ignoreDisplayItems)
        {
            foreach (var ignore in ignores)
            {
                if (ignore is null || ignore.GetType() != GetEnumType(comboBox))
                {
                    continue;
                }

                var hashCode = ignore?.GetHashCode();

                for (int i = ignoreDisplayItems.Count - 1; i >= 0; i--)
                {
                    if (ignoreDisplayItems[i].GetHashCode() == hashCode)
                    {
                        ignoreDisplayItems.RemoveAt(i);
                    }
                }
            }
        }

        if (enumValue is not null)
        {
            int hashCode = enumValue.GetHashCode();

            if (comboBox.SelectedItem is DisplayItem item && item.GetHashCode() == hashCode)
            {
                return;
            }

            if (comboBox.ItemsSource is IList<DisplayItem> items)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].GetHashCode() == hashCode)
                    {
                        comboBox.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox comboBox || comboBox.SelectedItem is not DisplayItem item || comboBox.ItemsSource is not IList<DisplayItem> items)
        {
            return;
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == item)
            {
                comboBox.SetCurrentValue(ComboBoxExtensions.EnumValueProperty, item.Value);

                break;
            }
        }
    }

    /// <summary>
    /// a class of <see cref="DisplayItem"/>
    /// </summary>
    public class DisplayItem
    {
        [DBA(Never)]
        int hashCode;

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="displayName"></param>
        public DisplayItem(object value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
            hashCode = Value?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// display value
        /// </summary>
        public object? Value { get; }

        /// <summary>
        /// display name
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// to string
        /// </summary>
        /// <returns></returns>
        public override string ToString() => DisplayName;

        /// <summary>
        /// get hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => hashCode;

        /// <summary>
        /// equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(DisplayItem left, DisplayItem right)
        {
            if (left is null || right is null)
            {
                return false;
            }
            return left.GetHashCode() == right.GetHashCode();
        }

        /// <summary>
        /// not equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(DisplayItem left, DisplayItem right)
        {
            return (left == right) == false;
        }

        /// <summary>
        /// equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            return obj is DisplayItem item && this == item;
        }
    }
}
