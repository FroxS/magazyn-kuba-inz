using System.Windows;
using Warehouse.Core.Interface;
using Warehouse.Creator.ViewModel;

namespace Warehouse.Creator.View
{
    /// <summary>
    /// Interaction logic for CreatorWindow.xaml
    /// </summary>
    public partial class CreatorWindow : Window, IMainWindow
	{
        public CreatorWindow()
        {
            InitializeComponent();
        }
    }
}
