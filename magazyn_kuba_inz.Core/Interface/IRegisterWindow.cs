using Warehouse.Models.Interfaces;

namespace Warehouse.Core.Interface;

public interface IRegisterWindow : IWindow
{
	IUser? GetUser();
	bool ExitOnSuccesfulRegister { get; set; }
	bool LoginOnSuccefulRegister { get; set; }
}
