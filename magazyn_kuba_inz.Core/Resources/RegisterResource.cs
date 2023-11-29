using Warehouse.Models.Enums;

namespace Warehouse.Core.Resources;

public sealed record RegisterResource(string Login, string Email, string Password, string Name, EUserType type = EUserType.Employee_WareHouse, bool IsActive = false);