using System.Windows;

namespace Warehouse.Dialog.View
{
    /// <summary>
    /// Interaction logic for StorageUnitDialog.xaml
    /// </summary>
    public partial class StorageUnitDialog : Window 
    {
        internal StorageUnitDialog(StorageUnitDialogViewModel vm)
        {
            vm.Window = this;
            DataContext = vm;
            InitializeComponent();
        }
    }
}
