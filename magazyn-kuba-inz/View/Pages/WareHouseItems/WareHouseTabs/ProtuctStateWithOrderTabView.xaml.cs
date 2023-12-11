using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Warehouse.View.Pages
{
	/// <summary>
	/// Interaction logic for ProtuctStateWithOrderTabView.xaml
	/// </summary>
	public partial class ProtuctStateWithOrderTabView : UserControl
    {
        public ProtuctStateWithOrderTabView()
        {
            InitializeComponent();
        }

		private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
		{
            if(sender is CheckBox checkBox && checkBox.DataContext is CollectionViewGroup coll)
            {

                if (checkBox.IsChecked ?? false)
                {
                    foreach( var item in coll.Items)
                    {
						datagrid.SelectedItems.Add(item);

					}
                }
                else
                {
					foreach (var item in coll.Items)
					{
						datagrid.SelectedItems.Remove(item);

					}
				}

				

			}
        }
    }
}
