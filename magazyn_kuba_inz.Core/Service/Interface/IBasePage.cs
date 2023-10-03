

namespace magazyn_kuba_inz.Core.Service.Interface
{
    public interface IBasePage : IBaseUIElement
    {
    }

    public interface IBaseUIElement
    {
        object? ViewModelObject { get; set; }
    }
}
