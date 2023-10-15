namespace Warehouse.Core.Models;

public enum NavItemType
{
    Dashboard,
    Settings,
    LogOut,
    Clients,
    Delivery
}

public class NavItem
{
    #region Public properties

    public NavItemType Type { get; set; }

    public string Name { get; set; }

    #endregion

    #region Constructors

    public NavItem(NavItemType type, string name)
    {
        Type = type;
        Name = name;
    }

    #endregion

}

