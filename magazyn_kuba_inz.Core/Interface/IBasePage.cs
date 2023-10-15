namespace Warehouse.Service.Interface
{
    public interface IBasePage : IBaseUIElement
    {
    }

    public interface IBaseUIElement
    {
        object? ViewModelObject { get; set; }
    }
}
