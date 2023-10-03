using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace magazyn_kuba_inz.Models.Application;

public class ObservableObject: INotifyPropertyChanged, INotifyPropertyChanging
{
    public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => { };
    public event PropertyChangingEventHandler? PropertyChanging = (sender, e) => { };

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    protected void OnPropertiesChanged(string[]? names = null)
    {
        if(names != null)
            names.ToList().ForEach((o) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(o) ));
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

