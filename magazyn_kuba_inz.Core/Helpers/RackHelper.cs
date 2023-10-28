using Warehouse.Core.Models;
using Warehouse.Models;

namespace Warehouse.Core.Helpers;

public static class RackHelper
{
    public static string GetName(this Rack rack)
    {
        return $"{rack.Flors}/{rack.Corridor}/{rack.Row}/{rack.Direction.ToString().FirstOrDefault()}";
    }
}