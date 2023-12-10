using Warehouse.Models.Enums;
using Warehouse.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using Warehouse.Core.Interface;
using System.Windows.Data;
using System.ComponentModel;

namespace Warehouse.ViewModel.Pages;

public class ProductsPageViewModel : 
    BasePageWIthItemsViewModel<
        Product,
        ProductViewModel,
        IProductService>
{
    #region Private fields

    private ISupplierService _supplierService => Application.GetService<ISupplierService>();
    private IProductGroupService _productgroupService => Application.GetService<IProductGroupService>();
    private IProductStatusService _productStatusService => Application.GetService<IProductStatusService>();

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="app">Application service</param>
    /// <param name="service">Service of product group item</param>
    public ProductsPageViewModel(
        IApp app, 
        IProductService service) 
        : base(app, service)    
    {
        Page = Models.Page.EApplicationPage.Products;
    }

    #endregion

    #region Filter

    protected override bool Filter(Product value, string search)
    {
        if (value?.Name?.ToLower().Contains(search?.ToLower()) ?? true)
            return true;
        else
            return false;
    }

    protected override IEnumerable<GroupDescription>? GroupBy()
    {
        List<GroupDescription> groups = new List<GroupDescription>();
        groups.Add(new PropertyGroupDescription($"{nameof(Product.Group)}.{nameof(Product.Group.Name)}"));
        groups.Add(new PropertyGroupDescription($"{nameof(Product.Supplier)}.{nameof(Product.Supplier.Name)}"));
        return groups;
    }

    #endregion

    #region Public methods

    public override ProductViewModel GetVM(ref Product? item, ProductViewModel? lastVm = null)
    {
        if (item == null)
            return lastVm;
        bool flag = false;
        if (lastVm != null && !(lastVm?.Saved ?? true))
        {
            string? message = lastVm.Valid();
            if (message != null)
            {
                Application.ShowSilentMessage($"{message}", EMessageType.Warning);
                return lastVm;
            }
            ;

            var task = Task.Run(async () => await lastVm.SaveAsync());
            Task.WaitAll(task);
            if (!task.Result)
            {
                Application.ShowSilentMessage($"Nie udało się zapisać danych", EMessageType.Warning);
                return lastVm;
            }
        }
        ProductViewModel newVM = new ProductViewModel(
            _service, 
            item,
            _productgroupService.GetAll(),
            _supplierService.GetAll(),
            _productStatusService.GetAll(),
            Application);
        return newVM;
    }

    public async override void OnPageOpen()
    {
        try
        {
            CanChangePage = false;
            Application.IsTaskRunning = true;
            var selectedGuid = SelectedItemViewModel?.ID;
            _selectedItemViewModel = null;
            List<Product> pgList = await _service.GetAllAsync();
            Items = new ObservableCollection<Product>(pgList);
            if (selectedGuid.HasValue)
            {
                SelectedItem = Items.FirstOrDefault(x => x.ID == selectedGuid.Value);
            }
            CanChangePage = true;
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(SelectedItemViewModel));
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
        finally { Application.IsTaskRunning = false; }
    }

    #endregion

    #region Command Methods

    public override async Task AddItem()
    {
        try
        {
            bool flag = true;
            if (SelectedItemViewModel != null)
                flag = await SelectedItemViewModel.SaveAsync();

            if (!flag)
            {
                Application.ShowSilentMessage($"Nie można zapisać");
                return;
            }
            IsTaskRunning = true;
            Application.GetInnerDialogService().AddProductInnerDialog(async (o) =>
            {
                if (o == null)
                    return;

                var newvm = await _service.AddAsync(o);
                await _service.SaveAsync();
                Application.GetDispather().Invoke(() => { Items.Add(o); });
                SelectedItem = o;
                OnPropertyChanged(nameof(SelectedItemViewModel));
                Application.ShowSilentMessage($"Udało się dodać produkt {_selectedItemViewModel.Name}", EMessageType.Ok);
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally { Application.IsTaskRunning = false; }
    }

    public override async Task DeleteItems(IList items)
    {
        if (items == null)
            return;
        try
        {
            if(MessageBox.Show("Czy jesteś pewien ?","Pytanie", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsTaskRunning = true;
                Product existWareHouse = null;
                foreach (Product p in items)
                    if(_service.ExistOnWareHouse(p.ID))
                    {
                        existWareHouse = p;
                        break;
                    }
                

                if(existWareHouse != null)
                {
                    Application.ShowSilentMessage($"Produkt [{existWareHouse.Name}] jest na magazynie, Nie można go usunąć", EMessageType.Warning);
                    IsTaskRunning = false;
                    return;
                }

                bool flag = true;
                List<Product> index = new List<Product>();
                foreach (Product pg in items)
                {
                    if (await _service.DeleteAsync(pg.ID))
                        index.Add(pg);
                    else
                    {
                        flag = false;
                    }
                    
                };
                index.ForEach(o => Items.Remove(o));
                flag = await _service.SaveAsync();
                IsTaskRunning = false;
                if (!flag)
                {
                    if(index.Count == 0)
                        Application.ShowSilentMessage("Nie udało się usunąć", EMessageType.Warning);
                    else
                        Application.ShowSilentMessage($"Nie udało się usunąć {items.Count - index.Count} z {items.Count} elementów", EMessageType.Warning);
                }
                else
                {
                    Application.ShowSilentMessage("Udało się usunąć", EMessageType.Ok);
                    _selectedItemViewModel = null;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }
        catch (Exception ex)
        {
            Application.ShowSilentMessage(ex.Message);
        }
        finally
        {
            IsTaskRunning = false;
        }
    }

    #endregion
}
