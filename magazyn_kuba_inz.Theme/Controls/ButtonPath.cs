using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Warehouse.Theme.Controls
{
    public class ButtonPath : Button
    {
        #region Dependency Property

        public Geometry IconPath
        {
            get { return (Geometry)GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public Thickness PathPadding
        {
            get { return (Thickness)GetValue(PathPaddingProperty); }
            set { SetValue(PathPaddingProperty, value); }
        }

        #endregion

        #region Dependency

        public static readonly DependencyProperty IconPathProperty =
            DependencyProperty.Register(nameof(IconPath), typeof(Geometry), typeof(ButtonPath), new PropertyMetadata(null));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(ButtonPath), new PropertyMetadata(null));

        public static readonly DependencyProperty CornerRadiusProperty =
           DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ButtonPath), new PropertyMetadata(new CornerRadius(0)));

        public static readonly DependencyProperty PathPaddingProperty =
           DependencyProperty.Register(nameof(PathPadding), typeof(Thickness), typeof(ButtonPath), new PropertyMetadata(new Thickness(0)));


        #endregion

        #region Constructor

        static ButtonPath()
        {

        }

        #endregion
    }
}
