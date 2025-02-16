using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace Promise.Wpf;

internal class ContentAdorner : System.Windows.Documents.Adorner, IDisposable
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Visual visual;

    public ContentAdorner(Visual visual, UIElement adornedElement)
        : base(adornedElement)
    {
        this.visual = visual;

        AddVisualChild(visual);
    }

    protected override int VisualChildrenCount => 1;

    protected override Visual GetVisualChild(int index)
    {
        return visual;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        (visual as UIElement)?.Arrange(new Rect(finalSize));
        return finalSize;
    }

    public void Dispose()
    {
        if (visual is not null)
        {
            RemoveVisualChild(visual);
        }

        visual = null!;
    }
}
