using Warehouse.Repository.Interfaces;
using Warehouse.Core.Interface;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Warehouse.Core.Helpers;
using Newtonsoft.Json;
using Warehouse.Models.Enums;
using System.Text;

namespace Warehouse.Service;

internal class OrderService : BaseServiceWithRepository<IOrderRepository,Order>, IOrderService
{
    #region Private fields

    private readonly IStorageItemRepository _storageItemRepo;


    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public OrderService(IOrderRepository repozitory, IStorageItemRepository storageItemRepo, IApp app) : base(repozitory, app)
    {
        _storageItemRepo = storageItemRepo;
    }

    #endregion

    #region Private helpers

    private IEnumerable<OrderProduct> GetAllProductsWithItems(Guid orderID)
    {
        return _repozitory.GetById(x => x.Include(i => i.Items).ThenInclude(i => i.StorageItem).ThenInclude(i => i.State), orderID)?.Items ?? new List<OrderProduct>();
    }

    #endregion

    #region Public Method

    public List<OrderProduct> GetProducts(Guid id)
    {
        return _repozitory.GetById(i => i.Include(i => i.Items).ThenInclude(x => x.Product), id)?.Items;
    }

    public string GetNewOrderName()
    {
        var all = _repozitory.GetAll();
        return FindNewOrderName(all.Select(x => x.Name));
    }

    private string FindNewOrderName(IEnumerable<string> listaNazw)
    {
        int najwiekszyNumer = 0;
        int rok = DateTime.Now.Year;

        // Przeszukaj istniejące nazwy w celu znalezienia największego numeru w danym roku
        foreach (string nazwa in listaNazw)
        {
            if (Regex.Match(nazwa, $@"ORD/{rok}/(\d+)").Success)
            {
                int numer = int.Parse(Regex.Match(nazwa, $@"ORD/{rok}/(\d+)").Groups[1].Value);
                if (numer > najwiekszyNumer)
                {
                    najwiekszyNumer = numer;
                }
            }
        }

        // Wygeneruj nową nazwę zwiększając numer o 1
        string nowaNazwa = $"ORD/{rok}/{najwiekszyNumer + 1}";

        return nowaNazwa;
    }

    public void SetWay(List<WayObject> way, Order order)
    {
        byte[] data = GetData(way);
        order.OrderWay = data;
        Update(order);
    }

    public List<WayObject>? GetWay(Order order)
    {
        if(order.OrderWay == null) 
            return null;

        return GetData<List<WayObject>>(order.OrderWay);
    }

    public bool Reserv(Order order, IWareHouseService service, ref string message)
    {
        if(order == null) return false;
        if (IsReserved(order.ID))
        {
            message = "Zamównienie jest juz zarezerwowane";
            return false;
        }
            

        order.Items = GetProducts(order);
        List<StorageItem> aviable = service.GetProductsByState(EState.Available);

        if(aviable.Count < order.Items.Count)
        {
            message = "Brak wystarczającej ilości produktów na magazynie";
            return false;
        }

        StorageItem[] assing = aviable.Take(order.Items.Count).ToArray();

        int i = 0;

        List<StorageItem> alreadyReserved = new List<StorageItem>();

        foreach (OrderProduct product in order.Items)
        {
            StorageItem ass = assing[i++];
            if (product.ID_StorageItem != Guid.Empty)
            {
                StorageItem reserved = _storageItemRepo.GetById(x => x.Include(i => i.State),product.ID_StorageItem);
                if(reserved != null && reserved.State.State >= EState.Reserved)
                {
                    product.StorageItem = reserved;
                    product.ID_StorageItem = reserved.ID;
                    alreadyReserved.Add(reserved);
                    continue;
                }
            }
            product.StorageItem = ass;
            product.ID_StorageItem = ass.ID;
        }
        Update(order);
        if (Save())
        {
            message = service.MoveProductToState(EState.Reserved, order.Items.Select(x => x.StorageItem).Where(x => !alreadyReserved.Contains(x)).ToArray());
            if(message != null)
            {
                foreach (OrderProduct product in order.Items)
                {
                    product.StorageItem = null;
                    product.ID_StorageItem = Guid.Empty;
                }
                Update(order);
                Save();
                return false;
            }
            else
            {
                return service.Save();
            }
        }
        return false;
    }

    private List<OrderProduct> GetProducts(Order order)
    {
        if (order.Items == null)
            return _repozitory.GetById(x => x.Include(i => i.Items), order.ID)?.Items ?? new List<OrderProduct>();
        else
            return order.Items;
    }

    public bool SetAsPrepared(Order order, IWareHouseService service)
    {
        if (order == null) return false;
        if (!IsReserved(order.ID))
            return false;

        order.Items = GetProducts(order);

        string? message = null;
        message = service.MoveProductToState(EState.Prepared, order.Items.Select(x => x.StorageItem).ToArray());
        if (message == null)
            return true;
        else
            return false;
    }

    public bool IsReserved(Guid id)
    {
        IEnumerable<OrderProduct> items = GetAllProductsWithItems(id);
        return items.Count() > 0 && !items.Any(x => x.StorageItem == null) && !items.Any(x => x.StorageItem?.State?.State < EState.Reserved);
    }

    public bool IsPrepared(Guid id)
    {
        IEnumerable<OrderProduct> items = GetAllProductsWithItems(id);
        return items.Count() > 0 && !items.Any(x => x.StorageItem == null) && !items.Any(x => x.StorageItem?.State?.State < EState.Prepared);
    }

    public double TotalPrice(Order order)
    {
        if (order == null)
            return 0;

        IProductService productService = GetApp().GetService<IProductService>();
        double price = 0;
        price = GetProducts(order).Select(x => productService.GetPrice(x.ID_Product)).Sum();

        price += price * (order.Margin / 100);
        return price;
    }

    public double TotalPrice(Guid id)
    {
        Order? order = _repozitory.GetById(x => x.Include(o => o.Items).ThenInclude(o => o.Product), id);;
        if (order == null)
            return 0;

        return TotalPrice(order);
    }

    protected override T GetData<T>(byte[] data)
    {
        string json = Encoding.UTF8.GetString(data);
        T obj = JsonConvert.DeserializeObject<T>(json);
        return obj;
    }

    protected override byte[] GetData<T>(T obj, Action<Newtonsoft.Json.JsonSerializer> onSerialize = null)
    {
        string json = JsonConvert.SerializeObject(obj);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        return bytes;
    }

    #endregion
}