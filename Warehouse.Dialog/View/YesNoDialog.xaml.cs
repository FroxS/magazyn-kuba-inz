using System.Windows;

namespace Warehouse.Dialog.View
{
    /// <summary>
    /// Interaction logic for YesNoDialog.xaml
    /// </summary>
    public partial class YesNoDialog : Window 
    {
        internal YesNoDialog(YesNoDialogViewModel vm)
        {
            vm.Window = this;
            DataContext = vm;
            InitializeComponent();
        }
    }
}
