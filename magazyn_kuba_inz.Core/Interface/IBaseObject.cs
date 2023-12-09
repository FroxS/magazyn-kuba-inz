using System.Windows;

namespace Warehouse.Core.Interface
{
    public interface IBaseObject
    {
        bool IsSelected { get; set; }
        Point Position { get; }
        double X { get; set; }
        double Y { get; set; }
        bool CanEdit { get; set; }
    }
}