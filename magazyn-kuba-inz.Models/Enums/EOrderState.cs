namespace Warehouse.Models.Enums;

public enum EOrderState
{
    Created = 1,           
    Reserved = 2,                  
    Prepared = 4,

    DeliveryCreated = 8,
    DeliveryPrepared = 16,
    Received = 32,

    Finish = 512
}



