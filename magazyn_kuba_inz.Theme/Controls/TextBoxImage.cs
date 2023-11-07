using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Warehouse.Theme.Controls
{
    public class TextBoxImage : TextBox
    {
        #region Dependency Property

        public Geometry IconPath
        {
            get { return (Geometry)GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }

        #endregion

        #region Dependency

        public static readonly DependencyProperty IconPathProperty =
            DependencyProperty.Register(nameof(IconPath), typeof(Geometry), typeof(TextBoxImage), new PropertyMetadata(null));

        #endregion

        #region Constructor

        static TextBoxImage()
        {

        }

        #endregion
    }
}
