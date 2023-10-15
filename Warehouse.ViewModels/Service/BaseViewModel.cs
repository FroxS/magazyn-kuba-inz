using System.ComponentModel;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Service;

public class BaseViewModel : ValidationViewModel, INotifyPropertyChanged, INotifyPropertyChanging, IBaseViewModel
{
    private bool isTaskRunning = false;
    public override bool _CanValidate { get; protected set; } = false;
    public virtual bool IsTaskRunning { get => isTaskRunning; set { isTaskRunning = value; OnPropertyChanged(nameof(IsTaskRunning)); } }
}

