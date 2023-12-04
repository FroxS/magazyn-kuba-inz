namespace Warehouse.Core.Interface
{
    public interface ISingleCardPage : IBasePageViewModel
    {
        public bool ExistPage(IBasePageViewModel page);
    }
}
