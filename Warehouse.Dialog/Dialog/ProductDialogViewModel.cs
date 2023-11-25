using Warehouse.Core.Interface;
using Warehouse.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Reflection;
using Warehouse.Models.Attribute;

namespace Warehouse.Dialog
{
    internal class ProductDialogViewModel : DialogViewModelBase<Product>
    {
        #region Private properties

        private ObservableCollection<Product> _products;

        private Product _product;

        private string _searchString;

        #endregion

        #region Public properties

        public ICollectionView Collection { get; private set; }

        public ObservableCollection<Product> Products 
        {
            get => _products;
            set {
                SetProperty(ref _products, value, nameof(Products),
                () =>
                {
                    if (Collection != null)
                        Collection.Filter -= FilterCollection;
                    Collection = CollectionViewSource.GetDefaultView(value);
                    Collection.Filter += FilterCollection;
                }
            );
            }
        }

        public Product Product
        {
            get => _product;
            set { _product = value; OnPropertyChanged(nameof(Product)); }
        }

        public virtual string SearchString
        {
            get => _searchString;
            set { SetProperty(ref _searchString, value, nameof(SearchString), () => Collection.Refresh()); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductDialogViewModel(IProductService service, string message) : base(message)
        {
            Products = new ObservableCollection<Product>(service.GetAll());
            Product = Products.FirstOrDefault();
        }

        #endregion

        #region Filter

        private bool FilterCollection(object value)
        {
            if (value is Product item && item != null && !string.IsNullOrEmpty(SearchString))
                return Filter(item, SearchString);
            else
                return true;
        }

        protected virtual bool Filter(Product value, string search)
        {
            if (value != null && !string.IsNullOrEmpty(search))
            {
                PropertyInfo[] properties = typeof(Product).GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (Attribute.IsDefined(property, typeof(FilterColumnAttribute)))
                    {
                        string? val = property.GetValue(value)?.ToString();
                        if (!string.IsNullOrEmpty(val))
                        {
                            return val.ToLower().Contains(search.ToLower());
                        }
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region Command methods

        protected override void ok()
        {
            DialogResult = Product;
            if (DialogResult == null)
                return;
            base.ok();
        }

        #endregion
    }
}