using Warehouse.Models.Page;

namespace Warehouse.Creator.Service
{
    public enum EPageStep 
    {
        Step1,  // Database data
        Step2,  // Product group data
        Step3,  // Suppliers data
        Step4,  // Product status data 
        Step5,  // Products data 
        Step6,  // Item state data 
        Step7,  // Warehouse Creator 
    }
}