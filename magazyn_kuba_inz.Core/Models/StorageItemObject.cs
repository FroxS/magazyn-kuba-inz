using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Models;

namespace Warehouse.Core.Models
{
    public class StorageItemObject : StorageItem
    {
        #region Private properties

        #endregion

        #region Public properties

        public override Product? Product 
        { 
            get => base.Product;
            set { base.Product = value; OnPropertyChanged(nameof(Product)); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public StorageItemObject()
        {

        }

        #endregion
    }
}