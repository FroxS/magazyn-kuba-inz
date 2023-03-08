using magazyn_kuba_inz.Models.Validaton;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace magazyn_kuba_inz.Core.ViewModel.Service;

public class BaseViewModel : ValidatorBase, INotifyPropertyChanged, INotifyPropertyChanging
{
    private bool isTaskRunning = false;

    public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => { };
    public event PropertyChangingEventHandler? PropertyChanging = (sender, e) => { };
    public override bool _CanValidate { get; protected set; } = false;
    public virtual bool IsTaskRunning { get => isTaskRunning; set { isTaskRunning = value; OnPropertyChanged(nameof(IsTaskRunning)); } }
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    protected void OnPropertyChanging([CallerMemberName] string? name = null)
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));
    }

    protected void NotifyPropChanged(params string[] propsName)
    {
        foreach (string prop in propsName)
        {
            if (string.IsNullOrEmpty(prop)) continue;
            OnPropertyChanged(prop);
        }
    }
}

