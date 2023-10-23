using System.Windows;

namespace Warehouse.Dialog.View
{
    /// <summary>
    /// Interaction logic for ProductDialog.xaml
    /// </summary>
    public partial class ProductDialog : Window 
    {
        internal ProductDialog(ProductDialogViewModel vm)
        {
            vm.Window = this;
            DataContext = vm;
            InitializeComponent();
        }
    }
}
