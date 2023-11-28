using Warehouse.Models.Enums;

namespace Warehouse.Core.Interface;

public interface IMessageService
{
    void AddMessage(string message, EMessageType type = EMessageType.Warning);
    void Clear();
}