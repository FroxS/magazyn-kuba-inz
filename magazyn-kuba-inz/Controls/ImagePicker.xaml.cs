using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Warehouse.Helper;

namespace Warehouse.Controls
{
    /// <summary>
    /// Interaction logic for ImagePicker.xaml
    /// </summary>
    public partial class ImagePicker : UserControl 
    {
        #region Private fields

        private bool _clicked = false;

        #endregion

        #region Public properties

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        #endregion

        #region Dependency

        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(ImagePicker), new PropertyMetadata(null, ImageChanged));


        #endregion

        #region Constructors

        public ImagePicker()
        {
            InitializeComponent();
            MouseEnter += ImagePicker_MouseEnter;
            LostFocus += Popup_LostFocus;
            MouseDown += ImagePicker_MouseDown;
            LoadDefaultImage();
        }

        #endregion

        #region Property changed

        private static void ImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImagePicker im )
            {
                if (e.NewValue is ImageSource image)
                {
                    im.imBorder.Background = new ImageBrush() { ImageSource = image };
                }

                if (e.NewValue is Geometry data)
                {
                    im.imBorder.Background = new ImageBrush() { ImageSource = data.GetImage() };
                }
            }
                
        }

        #endregion

        #region Events

        private void ImagePicker_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _clicked = true;
            ShowPopupBorder();
        }

        private void Popup_LostFocus(object sender, RoutedEventArgs e)
        {
            _clicked = false;
            HidePopupBorder();
        }

        private void ImagePicker_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
            ShowPopupBorder();
            MouseLeave += ImagePicker_MouseLeave;
            
        }

        private void ImagePicker_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!_clicked)
            {
                HidePopupBorder();
                MouseLeave -= ImagePicker_MouseLeave;
            }
            
        }

        #endregion 

        #region Helpers

        private void ShowPopupBorder()
        {
            popupBorder.Opacity = 1d;
            VisualStateManager.GoToState(popupBorder, "Visible", true);
        }

        private void HidePopupBorder()
        {
            popupBorder.Opacity = 0d;
            VisualStateManager.GoToState(popupBorder, "Collapsed", true);
        }

        private void LoadDefaultImage()
        {
            Geometry? data = null;
            if (Application.Current.Resources["User"] is Style style)
            {
                foreach (Setter t in style.Setters)
                {
                    if (t.Property.Name == "Data")
                    {
                        if (t.Value is Geometry g)
                        {
                            data = g;
                            break;
                        }
                    }
                }
            }

            if(data != null)
                imBorder.Background = new ImageBrush() { ImageSource = data.GetImage() };

        }

        #endregion
    }
}
