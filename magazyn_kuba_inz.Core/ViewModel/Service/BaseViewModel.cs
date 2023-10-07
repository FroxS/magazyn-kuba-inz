using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace magazyn_kuba_inz.Core.ViewModel.Service;

public class BaseViewModel : ValidationViewModel, INotifyPropertyChanged, INotifyPropertyChanging
{
    private bool isTaskRunning = false;
    public override bool _CanValidate { get; protected set; } = false;
    public virtual bool IsTaskRunning { get => isTaskRunning; set { isTaskRunning = value; OnPropertyChanged(nameof(IsTaskRunning)); } }
}

