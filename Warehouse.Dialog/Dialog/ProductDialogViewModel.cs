using Warehouse.Core.Interface;
using Warehouse.Models;
using System.Collections.ObjectModel;
using Warehouse.Service.Interface;

namespace Warehouse.Dialog
{
    internal class ProductDialogViewModel : DialogViewModelBase<Product>
    {
        #region Private properties

        private ObservableCollection<Product> _products;

        private Product _product;

        private string _search;

        #endregion

        #region Public properties

        public ObservableCollection<Product> Products 
        {
            get => _products;
            set { _products = value; OnPropertyChanged(nameof(Products)); }
        }

        public Product Product
        {
            get => _product;
            set { _product = value; OnPropertyChanged(nameof(Product)); }
        }

        public string Search
        {
            get => _search;
            set { _search = value; OnPropertyChanged(nameof(Search)); }
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