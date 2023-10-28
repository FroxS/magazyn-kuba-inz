using Warehouse.Repository.Interfaces;
using Warehouse.Core.Interface;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Warehouse.Service;

internal class OrderService : BaseServiceWithRepository<IOrderRepository,Order>, IOrderService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public OrderService(IOrderRepository repository) :base(repository)
    {
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

    #endregion
}