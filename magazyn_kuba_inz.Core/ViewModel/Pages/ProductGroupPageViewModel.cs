using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.WareHouse;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace magazyn_kuba_inz.Core.ViewModel.Pages;

public class ProductGroupPageViewModel : BasePageViewModel
{
    #region Public Properties

    public ObservableCollection<Product> Products { get; private set; }

    #endregion

    #region Command

    #endregion

    #region Constructors
    public ProductGroupPageViewModel(IApp app) : base(app)
    {
        
    }

    #endregion
}
