using System.Windows;
using System.Windows.Media;

namespace HrtzImageViewer.Extensions
{
    public class ButtonIcon
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof (Geometry), typeof (ButtonIcon),
                new PropertyMetadata(default(Geometry)));

        public static void SetIcon(UIElement element, Geometry value)
        {
            element.SetValue(IconProperty, value);
        }

        public static Geometry GetIcon(UIElement element)
        {
            return (Geometry)element.GetValue(IconProperty);
        }
    }
}
