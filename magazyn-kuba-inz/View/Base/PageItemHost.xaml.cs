using System.Windows;
using System.Windows.Controls;

namespace Warehouse.View.Pages
{
    /// <summary>
    /// Interaction logic for PageItemHost.xaml
    /// </summary>
    public partial class PageItemHost : UserControl
    {
        public Control Child
        {
            get { return (Control)GetValue(ChildProperty); }
            set { SetValue(ChildProperty, value); }
        }

        public static readonly DependencyProperty ChildProperty =
            DependencyProperty.Register(nameof(Child), typeof(Control), typeof(PageItemHost), new PropertyMetadata(null));

        public PageItemHost()
        {
            InitializeComponent();
        }
    }
}
