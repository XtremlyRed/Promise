using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Promise.Wpf;

namespace Promise.Wpf.Services.PopupService;

/// <summary>
/// PopupContainer.xaml 的交互逻辑
/// </summary>
internal partial class PopupContainer : UserControl
{
    public PopupContainer()
    {
        InitializeComponent();

        Loaded += PopupContainer_Loaded;
    }

    private void PopupContainer_Loaded(object sender, RoutedEventArgs e)
    {
        Loaded -= PopupContainer_Loaded;

        var timeSpan = TimeSpan.FromMilliseconds(500);

        ColorAnimation colorAnimation = new ColorAnimation() { To = Color.FromArgb(80, 0, 0, 0), Duration = timeSpan };

        ContainerBackground.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
    }

    private void Btn_Container_Loaded(object sender, RoutedEventArgs e)
    {
        var panel = sender as Panel;
        if (panel!.Children.Count <= 2)
        {
            panel.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
        }
    }

    private void Click(object sender, RoutedEventArgs e) { }

    private void Button_Loaded(object sender, RoutedEventArgs e) { }
}
