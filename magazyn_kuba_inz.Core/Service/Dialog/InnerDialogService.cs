using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.InnerDialog;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.WareHouse;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace magazyn_kuba_inz.Core.Service.Dialog
{
    public class InnerDialogService : BaseViewModel, IInnerDialogService
    {
        #region Private fields

        private bool _isInnerDialogOpen = false;

        private BaseViewModel? _innerDialogVM;

        private readonly IServiceProvider _service;

        #endregion

        #region Public properties

        public bool IsInnerDialogOpen
        {
            get => _isInnerDialogOpen;
            private set
            {
                _isInnerDialogOpen = value;
                OnPropertyChanged(nameof(IsInnerDialogOpen));
            }
        }

        public BaseViewModel? InnerDialogVM
        {
            get => _innerDialogVM;
            private set
            {
                _innerDialogVM = value;
                OnPropertyChanged(nameof(InnerDialogVM));
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public InnerDialogService(IServiceProvider service)
        {
            _service = service;
        }

        #endregion

        #region DialogMethods

        public void AddProductInnerDialog(Action<Product> OnResult)
        {
            var suppliers= _service.GetRequiredService<ISupplierService>().GetAll();
            var groupsTask = _service.GetRequiredService<IProductGroupService>().GetAll();
            var statusesTask = _service.GetRequiredService<IProductStatusService>().GetAll();
            OpenInnerDialog(
                new AddProductInnerDialogViewModel(
                    _service.GetRequiredService<IApp>(),
                    _service.GetRequiredService<IProductService>(),
                    groupsTask,
                    suppliers,
                    statusesTask
                ), 
                OnResult
            );
        }

        public void AddItemStateInnerDialog(Action<ItemState> OnResult)
        {
            OpenInnerDialog(
                new AddItemStateInnerDialogViewModel(
                    _service.GetRequiredService<IApp>(),
                    _service.GetRequiredService<IItemStateService>()
                ),
                OnResult
            );
        }

        public void AddProductGroupInnerDialog(Action<ProductGroup> OnResult)
        {
            OpenInnerDialog(
                new AddProductGroupInnerDialogViewModel(
                    _service.GetRequiredService<IApp>(),
                    _service.GetRequiredService<IProductGroupService>()
                ),
                OnResult
            );
        }

        public void AddSupplierInnerDialog(Action<Supplier> OnResult)
        {
            OpenInnerDialog(
                new AddSupplierInnerDialogViewModel(
                    _service.GetRequiredService<IApp>(),
                    _service.GetRequiredService<ISupplierService>()
                ),
                OnResult
            );
        }

        public void AddProductStatusInnerDialog(Action<ProductStatus> OnResult)
        {
            OpenInnerDialog(
                new AddProductStatusInnerDialogViewModel(
                    _service.GetRequiredService<IApp>(),
                    _service.GetRequiredService<IProductStatusService>()
                ),
                OnResult
            );
        }

        public void AddProductToStateInnerDialog(Product product, List<ItemState> leftStates,Action<WareHouseItem> OnResult)
        {
            OpenInnerDialog(
                new AddProductToStateInnerDialogViewModel(
                    _service.GetRequiredService<IApp>(),
                    product,
                    leftStates
                ),
                OnResult
            );
        }

        public void YesNoInnerDialog(string message, Action<DialogResult> OnResult)
        {
            OpenInnerDialog(
                new YesNoInnerDialogViewModel(
                    message,
                    _service.GetRequiredService<IApp>()
                ),
                OnResult
            );
        }

        public async Task<StorageUnit> AddStorageUnitInnerDialog()
        {
            return await OpenInnerDialogAsync(
                new AddStorageUnitInnerDialogViewModel(
                    _service.GetRequiredService<IApp>(), 
                    _service.GetRequiredService<IStorageUnitService>()
                    )
                );
        }

        #endregion


        #region Public methods

        public void OpenInnerDialog<T>(BaseInnerDialogViewModel<T> vm, Action<T> OnClose)
        {
            InnerDialogVM = vm;
            IsInnerDialogOpen = true;
            Task.Run(() =>
            {
                while (true)
                {
                    if (vm.DialogResult)
                    {
                        OnClose.Invoke(vm.Result);
                        return;
                    }
                }
            });
        }

        public async Task<T> OpenInnerDialogAsync<T>(BaseInnerDialogViewModel<T> vm)
        {
            var tcs = new TaskCompletionSource<T>();
            vm.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "DialogResult" && vm.DialogResult)
                {
                    tcs.SetResult(vm.Result);
                }
            };

            InnerDialogVM = vm;
            IsInnerDialogOpen = true;
            return await tcs.Task;
        }

        public void OpenInnerDialog<T>(BaseInnerDialogViewModel<T> vm)
        {
            InnerDialogVM = vm;
            IsInnerDialogOpen = true;
        }

        public void CloseInnerDialog()
        {
            InnerDialogVM = null;
            IsInnerDialogOpen = false;
        }

        

        #endregion
    }
}
