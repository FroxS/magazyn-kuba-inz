using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Warehouse.Core.Interface;
using Warehouse.ViewModel;

namespace Warehouse.Controls
{
    /// <summary>
    /// Interaction logic for TopBar.xaml
    /// </summary>
    public partial class TopBar : UserControl
    {

        public Window MainWindow
        {
            get { return (Window)GetValue(MainWindowProperty); }
            set { SetValue(MainWindowProperty, value); }
        }

        public IApp Application
        {
            get { return (IApp)GetValue(ApplicationProperty); }
            set { SetValue(ApplicationProperty, value); }
        }

        public bool ShowNavigationButtons
        {
            get { return (bool)GetValue(ShowNavigationButtonsProperty); }
            set { SetValue(ShowNavigationButtonsProperty, value); }
        }

        public static readonly DependencyProperty ApplicationProperty =
            DependencyProperty.Register(nameof(Application), typeof(IApp), typeof(TopBar), new PropertyMetadata(null));

        public static readonly DependencyProperty MainWindowProperty =
            DependencyProperty.Register(nameof(MainWindow), typeof(Window), typeof(TopBar), new PropertyMetadata(null));

        public static readonly DependencyProperty ShowNavigationButtonsProperty =
            DependencyProperty.Register(nameof(ShowNavigationButtons), typeof(bool), typeof(TopBar), new PropertyMetadata(true));

        public TopBar()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(MainWindow != null)
            {
                if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
                {
                    if (MainWindow.WindowState == WindowState.Maximized)
                        MainWindow.WindowState = WindowState.Normal;
                    else
                        MainWindow.WindowState = WindowState.Maximized;
                }
                else if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                {
                    MainWindow.DragMove();
                }
            }
            
        }
    }
}
