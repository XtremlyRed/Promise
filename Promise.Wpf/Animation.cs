using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Promise.Wpf;

public static class Animation
{
    public static AnimationPlayer GetPlayer(UIElement element)
    {
        return (AnimationPlayer)element.GetValue(PlayerProperty);
    }

    public static void SetPlayer(UIElement element, AnimationPlayer value)
    {
        element.SetValue(PlayerProperty, value);
    }

    public static readonly DependencyProperty PlayerProperty = DependencyProperty.RegisterAttached(
        "Player",
        typeof(AnimationPlayer),
        typeof(Animation),
        new PropertyMetadata(
            null,
            (s, e) =>
            {
                if (s is not FrameworkElement element || e.NewValue is not AnimationPlayer player)
                {
                    return;
                }

                EventModeFactory.Register(element, player.EventMode, player.Settings!);
                //ignore
            }
        )
    );
}

[DefaultProperty(nameof(Settings))]
[ContentProperty(nameof(Settings))]
public class AnimationPlayer : DependencyObject
{
    public AnimationPlayer()
    {
        Settings = new AnimationSettingCollection();
    }

    public AnimationSettingCollection Settings
    {
        get { return (AnimationSettingCollection)GetValue(SettingsProperty); }
        set { SetValue(SettingsProperty, value); }
    }

    public static readonly DependencyProperty SettingsProperty = DependencyProperty.Register("Settings", typeof(AnimationSettingCollection), typeof(AnimationPlayer), new PropertyMetadata(null));

    public EventMode EventMode
    {
        get { return (EventMode)GetValue(EventModeProperty); }
        set { SetValue(EventModeProperty, value); }
    }

    public static readonly DependencyProperty EventModeProperty = DependencyProperty.Register("EventMode", typeof(EventMode), typeof(AnimationPlayer), new PropertyMetadata(EventMode.Loaded));
}

[MarkupExtensionReturnType(typeof(AnimationPlayer))]
public class Player : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var player = new AnimationPlayer();

        if (Setting is not null)
        {
            player.Settings.Add(Setting);
        }
        player.EventMode = EventMode;
        return player;
    }

    public AnimationSetting? Setting { get; set; }

    public EventMode EventMode { get; set; }
}

public class AnimationSettingCollection : FreezableCollection<AnimationSetting>, IList, ICollection<AnimationSetting>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private WeakReference<FrameworkElement> elementReference = default!;

    /// <summary>
    /// Attaches the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    internal void Attach(FrameworkElement element)
    {
        elementReference = new WeakReference<FrameworkElement>(element);
    }

    /// <summary>
    /// Adds the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    int IList.Add(object? value)
    {
        if (value is not AnimationSetting setter)
        {
            return -1;
        }

        ((ICollection<AnimationSetting>)this).Add(setter);
        return 1;
    }

    /// <summary>
    /// Adds the specified setter.
    /// </summary>
    /// <param name="setter">The setter.</param>
    void ICollection<AnimationSetting>.Add(AnimationSetting setter)
    {
        if (setter is null)
        {
            return;
        }

        base.Add(setter);
    }
}

public abstract class AnimationSetting : DependencyObject
{
    public EasingType? EasingType
    {
        get => (EasingType?)GetValue(EasingTypeProperty);
        set => SetValue(EasingTypeProperty, value);
    }

    public static readonly DependencyProperty EasingTypeProperty = DependencyProperty.Register("EasingType", typeof(EasingType?), typeof(AnimationSetting), new PropertyMetadata(default(EasingType?)!));

    public EventMode EventMode
    {
        get => (EventMode)GetValue(EventModeProperty);
        set => SetValue(EventModeProperty, value);
    }

    public static readonly DependencyProperty EventModeProperty = DependencyProperty.Register("EventMode", typeof(EventMode), typeof(AnimationSetting), new PropertyMetadata(EventMode.Loaded));

    public EasingMode? EasingMode
    {
        get { return (EasingMode?)GetValue(EasingModeProperty); }
        set { SetValue(EasingModeProperty, value); }
    }

    public static readonly DependencyProperty EasingModeProperty = DependencyProperty.Register("EasingMode", typeof(EasingMode?), typeof(AnimationSetting), new PropertyMetadata(null));

    public Duration Duration
    {
        get => (Duration)GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(Duration), typeof(AnimationSetting), new PropertyMetadata(new Duration()));

    public TimeSpan? BeginTime
    {
        get => (TimeSpan?)GetValue(BeginTimeProperty);
        set => SetValue(BeginTimeProperty, value);
    }

    public static readonly DependencyProperty BeginTimeProperty = DependencyProperty.Register("BeginTime", typeof(TimeSpan?), typeof(AnimationSetting), new PropertyMetadata(default(TimeSpan?)));

    public FillBehavior? FillBehavior
    {
        get => (FillBehavior?)GetValue(FillBehaviorProperty);
        set => SetValue(FillBehaviorProperty, value);
    }

    public static readonly DependencyProperty FillBehaviorProperty = DependencyProperty.Register("FillBehavior", typeof(FillBehavior?), typeof(AnimationSetting), new PropertyMetadata(default(FillBehavior?)));

    public double? SpeedRatio
    {
        get => (double?)GetValue(SpeedRatioProperty);
        set => SetValue(SpeedRatioProperty, value);
    }

    public static readonly DependencyProperty SpeedRatioProperty = DependencyProperty.Register("SpeedRatio", typeof(double?), typeof(AnimationSetting), new PropertyMetadata(default(double?)));

    public RepeatBehavior? RepeatBehavior
    {
        get => (RepeatBehavior?)GetValue(RepeatBehaviorProperty);
        set => SetValue(RepeatBehaviorProperty, value);
    }

    public static readonly DependencyProperty RepeatBehaviorProperty = DependencyProperty.Register("RepeatBehavior", typeof(RepeatBehavior?), typeof(AnimationSetting), new PropertyMetadata(default(RepeatBehavior?)));

    public double? DecelerationRatio
    {
        get => (double?)GetValue(DecelerationRatioProperty);
        set => SetValue(DecelerationRatioProperty, value);
    }

    public static readonly DependencyProperty DecelerationRatioProperty = DependencyProperty.Register("DecelerationRatio", typeof(double?), typeof(AnimationSetting), new PropertyMetadata(default(double?)));

    public double? AccelerationRatio
    {
        get => (double?)GetValue(AccelerationRatioProperty);
        set => SetValue(AccelerationRatioProperty, value);
    }

    public static readonly DependencyProperty AccelerationRatioProperty = DependencyProperty.Register("AccelerationRatio", typeof(double?), typeof(AnimationSetting), new PropertyMetadata(default(double?)));

    public ICommand CompletedCommand
    {
        get => (ICommand)GetValue(CompletedCommandProperty);
        set => SetValue(CompletedCommandProperty, value);
    }

    public static readonly DependencyProperty CompletedCommandProperty = DependencyProperty.Register("CompletedCommand", typeof(ICommand), typeof(AnimationSetting), new PropertyMetadata(default(ICommand?)));

    public object? CompletedCommandParameter
    {
        get => (object?)GetValue(CompletedCommandParameterProperty);
        set => SetValue(CompletedCommandParameterProperty, value);
    }

    public static readonly DependencyProperty CompletedCommandParameterProperty = DependencyProperty.Register(
        "CompletedCommandParameter",
        typeof(object),
        typeof(AnimationSetting),
        new PropertyMetadata(default(object?))
    );

    internal abstract bool Init(FrameworkElement element, out AnimationTimeline timeline);
}

public abstract class AnimationSetting<T> : AnimationSetting
{
    public T? From
    {
        get => (T?)GetValue(FromProperty);
        set => SetValue(FromProperty, value);
    }

    public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(T?), typeof(AnimationSetting<T>), new PropertyMetadata(null!));

    public T? To
    {
        get => (T?)GetValue(ToProperty);
        set => SetValue(ToProperty, value);
    }

    public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(T?), typeof(AnimationSetting<T>), new PropertyMetadata(null!));
}

#region property animations


public abstract class AnimationSetting<T, TTimeline> : AnimationSetting<T>
    where TTimeline : AnimationTimeline, new()
{
    public DependencyProperty Property
    {
        get => (DependencyProperty)GetValue(PropertyProperty);
        set => SetValue(PropertyProperty, value);
    }

    public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register("Property", typeof(DependencyProperty), typeof(AnimationSetting<T, TTimeline>), new PropertyMetadata(null));

    internal override bool Init(FrameworkElement element, out AnimationTimeline timeline)
    {
        timeline = Animationhelper<T, TTimeline>.GetTimeline(this);

        Storyboard.SetTargetProperty(timeline, new PropertyPath(Property.Name));

        Storyboard.SetTarget(timeline, element);

        return true;
    }
}

public class Int16AnimationSetting : AnimationSetting<Int16?, Int16Animation> { }

public class Int32AnimationSetting : AnimationSetting<Int32?, Int32Animation> { }

public class Int64AnimationSetting : AnimationSetting<Int64?, Int64Animation> { }

public class DoubleAnimationSetting : AnimationSetting<double?, DoubleAnimation> { }

public class FloatAnimationSetting : AnimationSetting<float?, SingleAnimation> { }

public class ColorAnimationSetting : AnimationSetting<Color?, ColorAnimation> { }

public class SizeAnimationSetting : AnimationSetting<Size?, SizeAnimation> { }

public class VectorAnimationSetting : AnimationSetting<Vector?, VectorAnimation> { }

public class Vector3DAnimationSetting : AnimationSetting<Vector3D?, Vector3DAnimation> { }

public class PointAnimationSetting : AnimationSetting<Point?, PointAnimation> { }

public class Point3DAnimationSetting : AnimationSetting<Point3D?, Point3DAnimation> { }

public class QuaternionAnimationSetting : AnimationSetting<Quaternion?, QuaternionAnimation> { }

public class Rotation3DAnimationSetting : AnimationSetting<Rotation3D?, Rotation3DAnimation> { }

public class ThicknessAnimationSetting : AnimationSetting<Thickness?, ThicknessAnimation> { }

public class BrushAnimationSetting : AnimationSetting<Color?, ColorAnimation>
{
    internal override bool Init(FrameworkElement element, out AnimationTimeline timeline)
    {
        timeline = default!;

        if (element.GetValue(Property) is not SolidColorBrush solidBrush || solidBrush.IsFrozen)
        {
            if (this.From is null)
            {
                return false;
            }

            solidBrush = new SolidColorBrush { Color = this.From.Value, Opacity = 1 };

            element.SetCurrentValue(Property, solidBrush);
        }

        this.From ??= solidBrush.Color;

        timeline = Animationhelper<Color?, ColorAnimation>.GetTimeline(this);

        var path = $"({element.GetType().Name}.{Property.Name}).(SolidColorBrush.Color)";

        Storyboard.SetTargetProperty(timeline, new PropertyPath(path));

        Storyboard.SetTarget(timeline, element);

        return true;
    }
}

#endregion

#region transition animation

public abstract class TransitionAnimationSetting<T, TTimeline> : AnimationSetting<T>
    where TTimeline : AnimationTimeline, new()
{
    [DebuggerBrowsable(Never)]
    internal abstract string AnimationPath { get; }

    internal override bool Init(FrameworkElement element, out AnimationTimeline timeline)
    {
        var group = new TransformGroup();
        group.Children.Add(new RotateTransform());
        group.Children.Add(new ScaleTransform());
        group.Children.Add(new TranslateTransform());

        element.SetCurrentValue(UIElement.RenderTransformProperty, group);
        element.SetCurrentValue(UIElement.RenderTransformOriginProperty, new System.Windows.Point(0.5, 00.5));

        var path = $"({element.GetType().Name}.RenderTransform).(TransformGroup.Children){AnimationPath}";

        timeline = Animationhelper<T, TTimeline>.GetTimeline(this);

        Storyboard.SetTargetProperty(timeline, new PropertyPath(path));

        Storyboard.SetTarget(timeline, element);

        return true;
    }
}

public class FadeAnimationSetting : AnimationSetting
{
    [DebuggerBrowsable(Never)]
    static DependencyProperty FadeProperty = FrameworkElement.OpacityMaskProperty;

    internal override bool Init(FrameworkElement element, out AnimationTimeline timeline)
    {
        ColorAnimation colorAnimation = new ColorAnimation() { From = this.To == FadeType.Display ? Colors.Transparent : Colors.White, To = this.To == FadeType.Display ? Colors.White : Colors.Transparent };

        Animationhelper<Color, ColorAnimation>.GetTimeline(this, colorAnimation);

        if (element.GetValue(FadeProperty) is not SolidColorBrush solidBrush || solidBrush.IsFrozen)
        {
            solidBrush = new SolidColorBrush { Color = colorAnimation.From.Value, Opacity = 1 };

            element.SetCurrentValue(FadeProperty, solidBrush);
        }

        var path = $"({element.GetType().Name}.{FadeProperty.Name}).(SolidColorBrush.Color)";

        Storyboard.SetTargetProperty(colorAnimation, new PropertyPath(path));

        Storyboard.SetTarget(colorAnimation, element);

        timeline = colorAnimation;

        return true;
    }

    public FadeType To
    {
        get { return (FadeType)GetValue(ToProperty); }
        set { SetValue(ToProperty, value); }
    }

    public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(FadeType), typeof(FadeAnimationSetting), new PropertyMetadata());

    public enum FadeType
    {
        Display,
        Hide,
    }
}

public class RotateAnimationSetting : TransitionAnimationSetting<double, DoubleAnimation>
{
    [DebuggerBrowsable(Never)]
    internal override string AnimationPath => "[0].(RotateTransform.Angle)";
}

public class ScaleAnimationSetting : TransitionAnimationSetting<double, DoubleAnimation>
{
    [DebuggerBrowsable(Never)]
    internal override string AnimationPath => $"[1].(ScaleTransform.Scale{Direction})";

    public Direction Direction
    {
        get { return (Direction)GetValue(DirectionProperty); }
        set { SetValue(DirectionProperty, value); }
    }

    public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(Direction), typeof(ScaleAnimationSetting), new PropertyMetadata(Direction.X));
}

public class TranslateAnimationSetting : TransitionAnimationSetting<double, DoubleAnimation>
{
    [DebuggerBrowsable(Never)]
    internal override string AnimationPath => $"[2].(TranslateTransform.{Direction})";

    public Direction Direction
    {
        get { return (Direction)GetValue(DirectionProperty); }
        set { SetValue(DirectionProperty, value); }
    }

    public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(Direction), typeof(TranslateAnimationSetting), new PropertyMetadata(Direction.X));
}

public enum Direction
{
    X,
    Y,
}

#endregion



#region EventMode


internal static class EventModeFactory
{
    public static void Register(FrameworkElement uiElement, EventMode eventMode, ICollection<AnimationSetting> animationSettings)
    {
        if (GetAnimationSettings(uiElement) is not IDictionary<EventMode, List<AnimationSetting>> storages)
        {
            SetAnimationSettings(uiElement, storages = new Dictionary<EventMode, List<AnimationSetting>>());
        }

        if (storages.TryGetValue(eventMode, out List<AnimationSetting>? timelines) == false)
        {
            storages[eventMode] = timelines = [];
        }

        timelines.AddRange(animationSettings);

        if (GetEventModes(uiElement) is not List<EventMode> eventModes)
        {
            SetEventModes(uiElement, eventModes = []);
        }

        if (eventModes.Contains(eventMode))
        {
            return;
        }

        eventModes.Add(eventMode);

        switch (eventMode)
        {
            case EventMode.Loaded:

                uiElement.Loaded += UiElement_Loaded;
                static void UiElement_Loaded(object sender, RoutedEventArgs e)
                {
                    Begin(sender as FrameworkElement, EventMode.Loaded);
                }

                break;
            case EventMode.MouseEnter:

                uiElement.MouseEnter += UiElement_MouseEnter;

                static void UiElement_MouseEnter(object sender, MouseEventArgs e)
                {
                    Begin(sender as FrameworkElement, EventMode.MouseEnter);
                }

                break;
            case EventMode.MouseLeave:

                uiElement.MouseLeave += UiElement_MouseLeave;

                static void UiElement_MouseLeave(object sender, MouseEventArgs e)
                {
                    Begin(sender as FrameworkElement, EventMode.MouseEnter);
                }

                break;
            case EventMode.DataContextChanged:

                uiElement.DataContextChanged += UiElement_DataContextChanged;

                static void UiElement_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
                {
                    Begin(sender as FrameworkElement, EventMode.DataContextChanged);
                }

                break;
            case EventMode.GotFocus:

                uiElement.GotFocus += UiElement_GotFocus;

                static void UiElement_GotFocus(object sender, RoutedEventArgs e)
                {
                    Begin(sender as FrameworkElement, EventMode.GotFocus);
                }

                break;
            case EventMode.LostFocus:
                uiElement.LostFocus += UiElement_LostFocus;

                static void UiElement_LostFocus(object sender, RoutedEventArgs e)
                {
                    Begin(sender as FrameworkElement, EventMode.LostFocus);
                }

                break;
            default:
                break;
        }

        static void Begin(FrameworkElement? framework, EventMode eventMode)
        {
            if (framework is null || GetAnimationSettings(framework).TryGetValue(eventMode, out List<AnimationSetting>? animationSettings) == false)
            {
                return;
            }

            Storyboard storyboard = new Storyboard();

            for (int i = 0; i < animationSettings!.Count; i++)
            {
                if (animationSettings[i].Init(framework, out var timeline))
                {
                    storyboard.Children.Add(timeline);
                }
            }

            storyboard.Begin();
        }
    }

    static IDictionary<EventMode, List<AnimationSetting>> GetAnimationSettings(FrameworkElement obj)
    {
        return (obj?.GetValue(AnimationSettingsProperty) as IDictionary<EventMode, List<AnimationSetting>>)!;
    }

    static void SetAnimationSettings(FrameworkElement obj, IDictionary<EventMode, List<AnimationSetting>> value)
    {
        obj.SetValue(AnimationSettingsProperty, value);
    }

    static readonly DependencyProperty AnimationSettingsProperty = DependencyProperty.RegisterAttached(
        "AnimationSettings",
        typeof(IDictionary<EventMode, List<AnimationSetting>>),
        typeof(EventModeFactory),
        new PropertyMetadata(null)
    );

    static List<EventMode> GetEventModes(FrameworkElement obj)
    {
        return (List<EventMode>)obj.GetValue(EventModesProperty);
    }

    static void SetEventModes(FrameworkElement obj, List<EventMode> value)
    {
        obj.SetValue(EventModesProperty, value);
    }

    static readonly DependencyProperty EventModesProperty = DependencyProperty.RegisterAttached("EventModes", typeof(List<EventMode>), typeof(EventModeFactory), new PropertyMetadata(null));
}

#endregion


#region animation helper

internal static class Animationhelper<T, TTimeline>
    where TTimeline : AnimationTimeline, new()
{
    static PropertyInfo from = typeof(TTimeline).GetProperty("From", BindingFlags.Instance | BindingFlags.Public)!;
    static PropertyInfo to = typeof(TTimeline).GetProperty("To", BindingFlags.Instance | BindingFlags.Public)!;

    internal static TTimeline GetTimeline(AnimationSetting<T> animation)
    {
        TTimeline timeline = new TTimeline();

        Set(timeline, animation.From, (s, e) => from.SetValue(s, e));
        Set(timeline, animation.To, (s, e) => to.SetValue(s, e));

        GetTimeline(animation, timeline);

        return timeline;
    }

    internal static TTimeline GetTimeline(AnimationSetting animation, TTimeline timeline)
    {
        Set(timeline, animation.Duration, (s, e) => s.Duration = e);
        Set(timeline, animation.BeginTime, (s, e) => s.BeginTime = e);
        Set(timeline, animation.DecelerationRatio, (s, e) => s.DecelerationRatio = e!.Value);
        Set(timeline, animation.AccelerationRatio, (s, e) => s.AccelerationRatio = e!.Value);
        Set(timeline, animation.SpeedRatio, (s, e) => s.SpeedRatio = e!.Value);
        Set(timeline, animation.FillBehavior, (s, e) => s.FillBehavior = e!.Value);
        Set(timeline, animation.RepeatBehavior, (s, e) => s.RepeatBehavior = e!.Value);

        if (timeline is DoubleAnimation @double)
        {
            @double.EasingFunction = GetEasingFunction(animation);
        }

        return timeline;
    }

    static void Set<TV, TT>(TT timeline, TV? value, Action<TT, TV> action)
    {
        if (value is not null)
        {
            action(timeline, value);
        }
    }

    /// <summary>
    /// Gets the easing function.
    /// </summary>
    /// <param name="frameworkElement">The framework element.</param>
    /// <returns></returns>
    internal static IEasingFunction? GetEasingFunction(AnimationSetting animation)
    {
        EasingFunctionBase? easingFunctionBase = animation.EasingType switch
        {
            Wpf.EasingType.Back => new BackEase(),
            Wpf.EasingType.Bounce => new BounceEase(),
            Wpf.EasingType.Circle => new CircleEase(),
            Wpf.EasingType.Cubic => new CubicEase(),
            Wpf.EasingType.Elastic => new ElasticEase(),
            Wpf.EasingType.Exponential => new ElasticEase(),
            Wpf.EasingType.Quadratic => new QuadraticEase(),
            Wpf.EasingType.Quartic => new QuarticEase(),
            Wpf.EasingType.Quintic => new QuinticEase(),
            Wpf.EasingType.Sine => new SineEase(),
            _ => null,
        };

        if (easingFunctionBase is not null && animation.EasingMode is not null)
        {
            easingFunctionBase.EasingMode = animation.EasingMode.Value;
        }

        return easingFunctionBase;
    }
}

#endregion

/// <summary>
/// easing type
/// </summary>
public enum EasingType
{
    /// <summary>
    /// The none
    /// </summary>
    None,

    /// <summary>
    /// The back
    /// </summary>
    Back,

    /// <summary>
    /// The bounce
    /// </summary>
    Bounce,

    /// <summary>
    /// The circle
    /// </summary>
    Circle,

    /// <summary>
    /// The cubic
    /// </summary>
    Cubic,

    /// <summary>
    /// The elastic
    /// </summary>
    Elastic,

    /// <summary>
    /// The exponential
    /// </summary>
    Exponential,

    /// <summary>
    /// The quadratic
    /// </summary>
    Quadratic,

    /// <summary>
    /// The quartic
    /// </summary>
    Quartic,

    /// <summary>
    /// The quintic
    /// </summary>
    Quintic,

    /// <summary>
    /// The sine
    /// </summary>
    Sine,
}

/// <summary>
/// event mode
/// </summary>
public enum EventMode
{
    /// <summary>
    /// The loaded
    /// </summary>
    Loaded,

    /// <summary>
    /// The unloaded
    /// </summary>
    Unloaded,

    /// <summary>
    /// The mouse enter
    /// </summary>
    MouseEnter,

    /// <summary>
    /// The mouse leave
    /// </summary>
    MouseLeave,

    /// <summary>
    /// The data context changed
    /// </summary>
    DataContextChanged,

    /// <summary>
    /// The got focus
    /// </summary>
    GotFocus,

    /// <summary>
    /// The lost focus
    /// </summary>
    LostFocus,
}
