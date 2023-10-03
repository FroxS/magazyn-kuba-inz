using magazyn_kuba_inz.Core.Service.Dialog;
using System.Windows;

namespace magazyn_kuba_inz.View.Dialog
{
    /// <summary>
    /// Interaction logic for BaseDialogWindow.xaml
    /// </summary>
    public partial class BaseDialogWindow : Window , IDialogWindow
    {
        public FrameworkElement Control 
        { 
            get => ControlPresenter.Content as FrameworkElement;
            set {
                DataContext = value.DataContext;
                ControlPresenter.Content = value;
            }
        }

        public BaseDialogWindow()
        {
            InitializeComponent();
        }
    }
}
