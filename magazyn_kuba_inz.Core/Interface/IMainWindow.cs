namespace Warehouse.Core.Interface;

public interface IMainWindow: IWindow
{
    object DataContext { get; set; }
}
