using Warehouse.Core.Helpers;
using Warehouse.ViewModel.Service;
using Warehouse.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows.Input;
using Warehouse.Core.Interface;
using Warehouse.Service.Interface;

namespace Warehouse.ViewModel.Pages;

public class ProductViewModel : BaseEntityViewModel<Product>
{
    #region Private fields

    private ObservableCollection<ProductGroup> _productGroups;
    private ObservableCollection<Supplier> _suppliers;
    private ObservableCollection<ProductStatus> _productStatuses;
    private byte[] _image;

    #endregion

    #region Public Properties

    [Required(ErrorMessage = "Name is required.")]
    public string? Name
    {
        get => _entity.Name;
        set
        {
            if (_entity.Name == value)
                return;
            Saved = false;
            _entity.Name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    [Required(ErrorMessage = "Description is required.")]
    public string? Description
    {
        get => _entity.Description;
        set
        {
            if (_entity.Description == value)
                return;
            Saved = false;
            _entity.Description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    [Required(ErrorMessage = "Price is required.")]
    public double Price
    {
        get => _entity.Price;
        set
        {
            if (_entity.Price == value)
                return;
            Saved = false;
            _entity.Price = value;
            OnPropertyChanged(nameof(Price));
        }
    }

    [Required(ErrorMessage = "Product group is required.")]
    public ProductGroup? Group
    {
        get => _entity.Group;
        set
        {
            if (_entity.Group == value && value == null)
                return;
            Saved = false;
            _entity.Group = value;
            _entity.ID_Group = value.ID;
            OnPropertyChanged(nameof(Group));
        }
    }

    [Required(ErrorMessage = "Product status is required.")]
    public ProductStatus? Status
    {
        get => _entity.Status;
        set
        {
            if (_entity.Status == value && value == null)
                return;
            Saved = false;
            _entity.Status = value;
            _entity.ID_Status = value.ID;
            OnPropertyChanged(nameof(Status));
        }
    }

    [Required(ErrorMessage = "Supplier is required.")]
    public Supplier? Supplier
    {
        get => _entity.Supplier;
        set
        {
            if (_entity.Supplier == value && value == null)
                return;
            Saved = false;
            _entity.Supplier = value;
            _entity.ID_Supplier = value.ID;
            OnPropertyChanged(nameof(Supplier));
        }
    }

    public byte[]? MainImage 
    { 
        get => _entity.Images?.FirstOrDefault()?.Img;
    }

    public ObservableCollection<ProductGroup> ProductGroups
    {
        get => _productGroups;
        set
        {
            _productGroups = value;
            OnPropertyChanged(nameof(ProductGroups));
        }
    }

    public ObservableCollection<Supplier> Suppliers
    {
        get => _suppliers;
        set
        {
            _suppliers = value;
            OnPropertyChanged(nameof(Suppliers));
        }
    }

    public ObservableCollection<ProductStatus> ProductStatuses
    {
        get => _productStatuses;
        set
        {
            _productStatuses = value;
            OnPropertyChanged(nameof(ProductStatuses));
        }
    }

    #endregion

    #region Commands

    public ICommand LoadImageCommand { get; set; }

    #endregion

    #region Constructors
    public ProductViewModel(
        IBaseService<Product> service, 
        Product product,
        List<ProductGroup> productGroups, 
        List<Supplier> suppliers, 
        List<ProductStatus> productStatuses
        ) : base(service, product)
    {
        Saved = true;
        LoadImageCommand = new AsyncRelayCommand(loadImage);
        ProductGroups = new ObservableCollection<ProductGroup>(productGroups);
        Suppliers = new ObservableCollection<Supplier>(suppliers);
        ProductStatuses = new ObservableCollection<ProductStatus>(productStatuses);
    }

    #endregion

    #region Commands Methods

    public async Task loadImage()
    {
        try
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                byte[] img = System.IO.File.ReadAllBytes(openFileDialog.FileName);
                _entity =  await (_service as IProductService).SetImage(_entity, img);
                OnPropertyChanged(nameof(MainImage));
            }
        }catch(Exception ex)
        {
            Message = "Bład podczas dodwania zdjęcia";
        }
        
    }

    #endregion

    #region Protected methods

    protected override string[] GetpropsNameToFireOnSave()
    {
        return new string[] {
            nameof(Description),
            nameof(Name),
            nameof(Price),
            nameof(Status),
            nameof(Supplier),
            nameof(Group)
        };
    }

    #endregion

    #region Public methods

    public override string? Valid()
    {
        string message = null;

        string[] props = GetpropsNameToFireOnSave();

        foreach (string prop in props)
        {
            message = GettErrors(prop);
            if (!string.IsNullOrWhiteSpace(message))
                return message;
        }

        var taks = _service.GetAll();
        if (taks.Find(o => o.Name == Name && ID != o.ID) != null)
            return $"Nazwa {Name} juz istnieje w bazie danych";

        return base.Valid();
    }

    #endregion
}
