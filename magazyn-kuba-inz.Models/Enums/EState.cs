namespace Warehouse.Models.Enums;

public enum EState
{
    Delivery = 1,           /// Dostawa
    Received = 2,           /// Przyjęty        
    InStock = 4,            /// Na magazynie
    Available = 8,          /// Dostępny
    Reserved = 16,          /// Zarezerwowany (W zamówieniu ale nie przygotowany)
    Prepared = 32,          /// Przygotowane zamówienie
    TransportReady = 64,    /// Gotowy transport
    Transport = 128,        /// Transport
    Dispatched = 256,       /// Wydane
}
