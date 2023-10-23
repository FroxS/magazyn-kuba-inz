using System.Windows;

namespace Warehouse.Dialog.View
{
    /// <summary>
    /// Interaction logic for AlertDialog.xaml
    /// </summary>
    public partial class AlertDialog : Window
    {
        internal AlertDialog(AlertDialogViewModel vm)
        {
            vm.Window= this;
            DataContext = vm;
            
            InitializeComponent();
        }
    }
}
