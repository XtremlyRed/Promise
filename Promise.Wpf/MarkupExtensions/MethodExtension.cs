using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using static System.Reflection.BindingFlags;

namespace Promise.Wpf;

/// <summary>
/// a class of <see cref="MethodExtension"/>
/// </summary>
/// <seealso cref="MarkupExtension" />
[MarkupExtensionReturnType(typeof(ICommand))]
public class MethodExtension : MarkupExtension
{
    const BF bf = Instance | Public | NonPublic | Static;

    /// <summary>
    /// initialize a new instance of <see cref="MethodExtension"/>
    /// </summary>
    public MethodExtension() { }

    /// <summary>
    /// initialize a new instance of <see cref="MethodExtension"/>
    /// </summary>
    /// <param name="methodName">method name</param>
    public MethodExtension(string methodName)
    {
        ExecuteMethodName = methodName;
    }

    /// <summary>
    /// initialize a new instance of <see cref="MethodExtension"/>
    /// </summary>
    /// <param name="methodName">method name</param>
    /// <param name="canMethodName">can method name</param>
    public MethodExtension(string methodName, string canMethodName)
    {
        ExecuteMethodName = methodName;
        CanExecuteMethodName = canMethodName;
    }

    /// <summary>
    /// method name
    /// </summary>

    [ConstructorArgument("Method")]
    public string? ExecuteMethodName { get; set; }

    /// <summary>
    /// can execute method name
    /// </summary>

    [ConstructorArgument("CanMethod")]
    public string? CanExecuteMethodName { get; set; }

    /// <summary>
    /// provider name
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (string.IsNullOrWhiteSpace(ExecuteMethodName))
        {
            throw new Exception($"invalid execute method name : {ExecuteMethodName} ");
        }

        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideValueTarget provider)
        {
            return default!;
        }

        if (provider.TargetObject is not FrameworkElement frameworkElement)
        {
            return default!;
        }

        if (frameworkElement.DataContext is null)
        {
            return new Command(frameworkElement, ExecuteMethodName!, CanExecuteMethodName!);
        }

        Command.Parse(frameworkElement, ExecuteMethodName!, CanExecuteMethodName!, out var method, out var canExecuteMethod);

        return new Command(frameworkElement.DataContext, method, canExecuteMethod!);
    }

    private class Command : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        MethodInfo? executeMethod;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        MethodInfo? canExecuteMethod;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        object? dataContext;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string? ExecuteMethodName;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string? CanExecuteMethodName;

        public Command(object dataContext, MethodInfo executeMethod, MethodInfo? canExecuteMethod)
        {
            this.dataContext = dataContext;
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public Command(FrameworkElement frameworkElement, string? executeMethodName, string? canExecuteMethodName)
        {
            frameworkElement.DataContextChanged += FrameworkElement_DataContextChanged;
            ExecuteMethodName = executeMethodName;
            CanExecuteMethodName = canExecuteMethodName;
        }

        private void FrameworkElement_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is not FrameworkElement frameworkElement)
            {
                return;
            }

            frameworkElement.DataContextChanged -= FrameworkElement_DataContextChanged;

            dataContext = e.NewValue;

            Command.Parse(frameworkElement, ExecuteMethodName!, CanExecuteMethodName!, out executeMethod, out canExecuteMethod);
        }

        /// <summary>
        /// can execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object? parameter)
        {
            if (canExecuteMethod is null)
            {
                return true;
            }

            return (bool)this.canExecuteMethod.Invoke(dataContext, new object[] { parameter! })!;
        }

        /// <summary>
        /// execute
        /// </summary>
        /// <param name="parameter">parameter</param>
        public void Execute(object? parameter)
        {
            object[] ps = this.executeMethod!.GetParameters().Length == 0 ? new object[0] : new object[] { parameter! };

            this.executeMethod.Invoke(dataContext, ps);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public static void Parse(FrameworkElement frameworkElement, string ExecuteMethodName, string CanExecuteMethodName, out MethodInfo method, out MethodInfo canMethod)
        {
            var dcType = frameworkElement.DataContext.GetType();

            method = dcType.GetMethod(ExecuteMethodName, bf) ?? throw new Exception($"invalid execute method name : {ExecuteMethodName} ");

            if (string.IsNullOrWhiteSpace(CanExecuteMethodName) == false)
            {
                canMethod = dcType.GetMethod(CanExecuteMethodName!, bf) ?? throw new Exception($"invalid execute method name : {CanExecuteMethodName} ");

                if (canMethod.ReturnType != typeof(bool))
                {
                    throw new Exception("invalid return type");
                }
            }

            canMethod = default!;
        }
    }
}
