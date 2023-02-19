using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace magazyn_kuba_inz.Core.ViewModel.Service;

public class BaseViewModel : INotifyPropertyChanged, INotifyPropertyChanging
{
    public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => { };
    public event PropertyChangingEventHandler? PropertyChanging = (sender, e) => { };

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    protected void OnPropertyChanging([CallerMemberName] string? name = null)
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));
    }
}

