using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Warehouse.Theme.Controls
{
    public class ModalControl : ContentControl
    {

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(ModalControl), new PropertyMetadata(false));

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }


        static ModalControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModalControl), new FrameworkPropertyMetadata(typeof(ModalControl)));
            BackgroundProperty.OverrideMetadata(typeof(ModalControl), new FrameworkPropertyMetadata(CreateDefaultBackground()));
        }

        private static object CreateDefaultBackground()
        {
            return new SolidColorBrush(Colors.Black) { Opacity = 0.5 };
        }
    }
}
