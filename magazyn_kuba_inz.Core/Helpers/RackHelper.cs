using Warehouse.Core.Models;
using Warehouse.Models;

namespace Warehouse.Core.Helpers;

public static class RackHelper
{
    public static string GetName(this RackObject rack)
    {
        return $"{rack.Flors}/{0}";
    }
}