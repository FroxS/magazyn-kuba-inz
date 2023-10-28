using Warehouse.Models.Enums;
using Warehouse.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class ProductGroupsPageViewModel : 
    BasePageWIthItemsViewModel<
        ProductGroup, 
        ProductGroupViewModel,
        IProductGroupService>
{
    #region Public Properties

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="app">Application service</param>
    /// <param name="service">Service of product group item</param>
    public ProductGroupsPageViewModel(IApp app, IProductGroupService service) : base(app, service)
    {
        Page = Models.Page.EApplicationPage.ProductGroups;
    }

    #endregion

    #region Public methods

    public override ProductGroupViewModel GetVM(ref ProductGroup? item, ProductGroupViewModel? lastVm = null)
    {
        if (item == null)
            return lastVm;
        bool flag = false;
        if(lastVm != null && !(lastVm?.Saved ?? true))
        {
            string? message = lastVm.Valid();
            if(message != null)
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
        ProductGroupViewModel newVM = new ProductGroupViewModel(_service,item);
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
            List<ProductGroup> pgList = await _service.GetAllAsync();
            Items = new ObservableCollection<ProductGroup>(pgList);
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
            Application.GetInnerDialogService().AddProductGroupInnerDialog(async (o) => 
            {
                if (o == null)
                    return;

                var newvm =  await _service.AddAsync(o);
                await _service.SaveAsync();
                Application.GetDispather().Invoke(() => { Items.Add(o); });
                SelectedItem = o;
                OnPropertyChanged(nameof(SelectedItemViewModel));
                Application.ShowSilentMessage($"Udało się dodać grupę {_selectedItemViewModel.Name}", EMessageType.Ok);
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
                bool flag = true;
                List<ProductGroup> index = new List<ProductGroup>();
                foreach (ProductGroup pg in items)
                {
                    flag = await _service.DeleteAsync(pg.ID);

                    index.Add(pg);
                };
                index.ForEach(o => Items.Remove(o));
                flag = await _service.SaveAsync();

                if (!flag)
                {
                    Application.ShowSilentMessage("Nie udało się usunąć", EMessageType.Warning);
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
    }

    #endregion
}
