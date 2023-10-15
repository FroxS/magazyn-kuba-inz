using System.ComponentModel;

namespace Warehouse.Core.Interface;

public interface IBaseViewModel: IDataErrorInfo, INotifyPropertyChanged, INotifyPropertyChanging
{
    bool _CanValidate { get; }
    bool IsTaskRunning { get; set; }
}