using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using Warehouse.Core.Helpers;
using Warehouse.Core.Interface;
using Warehouse.Models;
using Warehouse.Models.Page;

namespace Warehouse.ViewModels.Navigation
{
    public class CustomMenuItem : ObservableObject
    {
        #region Private properties

        private ObservableCollection<CustomMenuItem> _items;

        private string _hader;

        private EApplicationPage _pageType;

        private readonly INavigation _nav;

        #endregion

        #region Public properties

        public ObservableCollection<CustomMenuItem> Items 
        {
            get => _items;
            set { SetProperty(ref _items, value, nameof(Items), () => OnPropertyChanged(nameof(ContainsSubMenu))); }
        }

        public string Header
        {
            get => _hader;
            set { SetProperty(ref _hader, value, nameof(Header)); }
        }

        public EApplicationPage PageType
        {
            get => _pageType;
            set { SetProperty(ref _pageType, value, nameof(PageType)); }
        }

        public bool ContainsSubMenu => !((Items?.Count ?? 0) == 0);

        public ICommand ClickCommand { get; protected set; }

        public string ResourceIconName { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomMenuItem(INavigation nav, EApplicationPage page)
        {
            _nav = nav;
            PageType = page;
            ClickCommand = new RelayCommand(() => _nav.SetPage(PageType));
        }

        public CustomMenuItem(INavigation nav, EApplicationPage page, string header) : this(nav,page)
        {
            Header = header;
        }

        #endregion
    }
}