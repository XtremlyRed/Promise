using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using static Promise.Wpf.PopupContext;
using static Promise.Wpf.PopupService;

namespace Promise.Wpf;

/// <summary>
/// popup context
/// </summary>
public abstract class PopupContext : IEquatable<PopupContext>, IEquatable<object>
{
    [DebuggerBrowsable(Never)]
    static int configIndex = int.MinValue;

    [DebuggerBrowsable(Never)]
    private readonly int contextIndex;

    /// <summary>
    /// </summary>
    protected PopupContext()
    {
        contextIndex = Interlocked.Increment(ref configIndex);
    }

    [DebuggerBrowsable(Never)]
    internal List<ButtonItem> buttonItems = new List<ButtonItem>();

    /// <summary>
    /// display buttons
    /// </summary>
    public string[] Buttons => buttonItems.Select(i => i.ButtonContent).ToArray();

    /// <summary>
    /// primary button index
    /// </summary>
    public int PrimaryIndex { get; set; } = 0;

    /// <summary>
    /// popup title
    /// </summary>
    public string? Title { get; internal set; }

    /// <summary>
    /// popup content
    /// </summary>
    public string? Content { get; internal set; }

    /// <summary>
    /// button click command
    /// </summary>
    public virtual ICommand ClickCommand =>
        new Command(
            (btnContent) =>
            {
                var eventArgs = new PubEventArgs(btnContent!, this);
                eventService.Publish(eventArgs);
            }
        );

    internal static PopupContext GetDefault(int buttonCount = 3)
    {
        var dict = new Dictionary<ButtonResult, string>();

        if (buttonCount > 0)
            Add(dict, ButtonResult.Yes);
        if (buttonCount > 1)
            Add(dict, ButtonResult.No);
        if (buttonCount > 2)
            Add(dict, ButtonResult.Cancel);

        return dict!;

        static void Add(Dictionary<ButtonResult, string> dict, ButtonResult buttonResult)
        {
            dict[buttonResult] = buttonResult.ToString();
        }
    }

    /// <summary>
    /// <see langword="equals"/>
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        return obj is PopupContext context && contextIndex == context.contextIndex;
    }

    /// <summary>
    /// get haso code
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return contextIndex;
    }

    /// <summary>
    /// <see langword="equals"/>
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(PopupContext? other)
    {
        return other?.contextIndex == contextIndex;
    }

    /// <summary>
    /// <see langword="equals"/>  operator
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(PopupContext? left, PopupContext? right)
    {
        return left is not null && right is not null && left.contextIndex == right.contextIndex;
    }

    /// <summary>
    /// not <see langword="equals"/>  operator
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(PopupContext? left, PopupContext? right)
    {
        return left is null || right is null || left.contextIndex != right.contextIndex;
    }

    /// <summary>
    /// create by button content
    /// </summary>
    /// <param name="buttonContexts"></param>
    public static implicit operator PopupContext?(Dictionary<ButtonResult, string>? buttonContexts)
    {
        if (buttonContexts is null || buttonContexts.Count == 0)
        {
            return default!;
        }

        var builder = new PopupContextBuilder();

        foreach (var item in buttonContexts)
        {
            builder.Register(item.Value, item.Key);
        }

        return builder.Build();
    }

    /// <summary>
    /// a <see langword="class"/> of <see cref="Command"/>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    private record Command(Action<string> Callback) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        bool ICommand.CanExecute(object? parameter)
        {
            return true;
        }

        void ICommand.Execute(object? parameter)
        {
            CanExecuteChanged?.Invoke(this!, EventArgs.Empty);

            Callback?.Invoke(parameter is string str ? str : parameter?.ToString()!);
        }
    }

    internal record ButtonItem(string ButtonContent, ButtonResult ButtonResult, Action<ButtonResult>? ClickAction = null);
}

/// <summary>
/// a <see langword="class"/> of <see cref="PopupContextBuilder"/>
/// </summary>
public class PopupContextBuilder
{
    [DebuggerBrowsable(Never)]
    int primaryIndex;

    [DebuggerBrowsable(Never)]
    string title = default!;

    [DebuggerBrowsable(Never)]
    readonly List<ButtonItem> buttonItems = new List<ButtonItem>();

    private class InnerPopupConfig : PopupContext { }

    /// <summary>
    /// build
    /// </summary>
    /// <returns></returns>
    public PopupContext Build()
    {
        var context = new InnerPopupConfig();

        context.PrimaryIndex = this.primaryIndex;

        context.Title = this.title;

        context.buttonItems.AddRange(buttonItems);

        return context;
    }

    /// <summary>
    ///  <paramref name="primaryIndex"/>
    /// </summary>
    /// <param name="primaryIndex"></param>
    /// <returns></returns>
    public PopupContextBuilder PrimaryIndex(int primaryIndex)
    {
        this.primaryIndex = primaryIndex;
        return this;
    }

    /// <summary>
    /// <paramref name="title"/>
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    public PopupContextBuilder Title(string title)
    {
        this.title = title;
        return this;
    }

    /// <summary>
    /// register button
    /// </summary>
    /// <param name="buttonContext"></param>
    /// <param name="buttonResult"></param>
    /// <param name="click"></param>
    /// <returns></returns>
    public PopupContextBuilder Register(string buttonContext, ButtonResult buttonResult, Action<ButtonResult>? click = null)
    {
        ButtonItem buttonItem = new ButtonItem(buttonContext, buttonResult, click);

        buttonItems.Add(buttonItem);

        return this;
    }
}
