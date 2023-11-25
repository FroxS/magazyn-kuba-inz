using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;
using Warehouse.Core.Interface;
using Warehouse.Core.Models;
using Warehouse.EF.Migrations;
using Warehouse.Models;

namespace Warehouse.Core.Helpers
{
    public enum EWayElementType
    {
        None,
        Point,
        Rack,
        Product
    }

    public class WayObject
    {
        public EWayElementType Type { get; set; }
        public Guid? Itemid { get; set; }
        public Point? Position { get; set; }
        public string? Name { get; set; }
        public WayObject()
        {
                
        }
    }

    public class WayResult
    {
        private readonly List<object> _path;
        public List<Product> Product => Get<Product>();

        public List<WayPointObject> Points => Get<WayPointObject>();

        public List<RackObject> Racks => Get<RackObject>();

        public List<object> Items => Get<object>();

        public WayResult(List<object> path) 
        {
            _path = path;
        }

        public double GetDistance()
        {
            double distance = 0;
            List<WayPointObject> points = Points;
            for (int i = 0; i < points.Count -1; i++)
            {
                distance += points[i].GetDistance(points[i+1]);
            }
            return distance;
        }

        private List<T> Get<T>()
        {
            var items = new List<T>();

            foreach(var path in _path)
            {
                if (path is T obj)
                    items.Add(obj);
            }

            return items;
        }

        public List<WayObject> GetPath()
        {
            List <WayObject> result = new List<WayObject>();
            foreach (var path in _path)
            {
                if (path is RackObject rack)
                    result.Add(new WayObject() { Type = EWayElementType.Rack, Itemid = rack.ID, Position = rack.Position });

                if (path is WayPointObject way)
                    result.Add(new WayObject() { Type = EWayElementType.Point, Position = way.Position });

                if (path is Product product)
                    result.Add(new WayObject() { Type = EWayElementType.Product, Itemid = product.ID, Name = product.Name });
            }
            return result;
        } 

    }

    public class DijkstraAlgorithm
    {
        #region Private fields

        private readonly double _maxThershold;

        private readonly WayPointObject _p_start;

        private readonly HallObject _hall;

        private readonly List<Product> _products;

        #endregion

        #region Constructor

        public DijkstraAlgorithm(WayPointObject p_start, HallObject hall, List<Product> products, double maxThershold = 2)
        {
            _maxThershold = maxThershold;
            _p_start = p_start;
            _hall = hall;
            _products = products.OrderByDescending(x => x.Weight).ToList();
        }

        #endregion

        #region Public methods

        public WayResult GetPath()
        {
            List<object> trace = new List<object>();
            IEnumerable<WayPointObject> points = _hall.WayPoints;
            
            List<Product> all = new List<Product>(_products);

            WayPointObject startForm = _p_start;

            while (!(all.Count == 0))
            {
                Product product = all.First();
                int indekofProduct = all.IndexOf(product);
                List<Product> productsToSearch = TakeSimilarProducts(product, all.Skip(indekofProduct + 1));
                List<KeyValuePair<Product, KeyValuePair<double, List<IBaseObject>>>> distances = new List<KeyValuePair<Product, KeyValuePair<double, List<IBaseObject>>>>();

                foreach (var productToSearch in productsToSearch)
                {
                    var result = GetShortestRoute(startForm, _hall, productToSearch);
                    distances.Add(new KeyValuePair<Product, KeyValuePair<double, List<IBaseObject>>>(productToSearch, result));
                }

                var found = distances.OrderBy(kv => kv.Value.Key).FirstOrDefault();

                trace.AddRange(found.Value.Value);

                int lastWayIndeks = found.Value.Value.Count;
                if (found.Value.Value[lastWayIndeks - 1] is WayPointObject wpo)
                    startForm = wpo;
                else
                    startForm = found.Value.Value[lastWayIndeks - 2] as WayPointObject;
                

                foreach(var dist in distances)
                {
                    trace.Add(dist.Key);
                    var indeksTODell = all.IndexOf(dist.Key);

                    all.RemoveAt(indeksTODell);
                }

                //var indeksTODell = all.IndexOf(found.Key);

                //all.RemoveAt(indeksTODell);
            }

            WayPointObject lastWayPoint = null;
            int i = 1;
            if(trace.Count> 0)
            {
                while (lastWayPoint == null)
                {
                    lastWayPoint = trace[trace.Count - i] as WayPointObject;
                    i++;
                }
            }
            
            var dttohome = GetDictryTable(lastWayPoint ?? _p_start, _hall.WayPoints);
            var toHome = GetWayFromTableDictry(dttohome, _p_start);
            trace.AddRange(toHome);
            return new WayResult(trace);
        }

        #endregion

        #region Private Methods

        private List<Product> TakeSimilarProducts(Product p, IEnumerable<Product> list)
        {
            List<Product> ret = new List<Product>();
            ret.Add(p);

            foreach (var listp in list)
            {
                if (p.Weight - listp.Weight <= _maxThershold)
                    ret.Add(listp);
                else
                    break;
            }
            return ret;
        }

        private KeyValuePair<double, List<IBaseObject>> GetShortestRoute(WayPointObject start, HallObject hall, Product product, bool addRackToResult = true)
        {
            Dictionary<WayPointObject, DictryTable<WayPointObject>> table = GetDictryTable(start, hall.WayPoints);
            ///Wyszykiwanie przystkich punktów przynależących do produktu
            var racks = hall.Racks.Where(x => x.HasPrduct(product));

            RackObject nearesRack = racks.First();
            WayPointObject nearestPoint = nearesRack.WayPoints.First();
            DictryTable<WayPointObject> tab = table[nearestPoint];
            foreach (RackObject rack in racks)
            {
                foreach (WayPointObject rackPoint in rack.WayPoints)
                {
                    var newFound = table[rackPoint];
                    if (newFound.Distance < tab.Distance)
                    {
                        nearesRack = rack;
                        nearestPoint = rackPoint;
                        tab = newFound;
                    }

                }
            }            
            List<IBaseObject> trace = GetWayFromTableDictry(table, nearestPoint);

            if (addRackToResult)
                trace.Add(nearesRack);
            return new KeyValuePair<double, List<IBaseObject>>(tab.Distance, trace);
        }

        //private Dictionary<WayPointObject, DictryTable<WayPointObject>> GetDictryTableOLD(WayPointObject start, IEnumerable<WayPointObject> points)
        //{
        //    Dictionary<WayPointObject, DictryTable<WayPointObject>> table = new Dictionary<WayPointObject, DictryTable<WayPointObject>>();
        //    foreach (var point in points)
        //    {
        //        table.Add(point, new DictryTable<WayPointObject>());
        //    }

        //    WayPointObject operatingPoint = start;
        //    table[operatingPoint].Distance = 0;
        //    /// Tworzenie tablicy Dictry
        //    while (!table.Values.All(x => x.Visited))
        //    {
        //        operatingPoint = table.Where(x => !x.Value.Visited).OrderBy(kv => kv.Value.Distance).FirstOrDefault().Key;

        //        foreach (WayPointObject conn in operatingPoint.Connections)
        //        {
        //            double dist = conn.GetDistance(operatingPoint) + table[operatingPoint].Distance;
        //            double curentDist = table[conn].Distance;
        //            if (curentDist > dist)
        //            {
        //                table[conn].Distance = dist;
        //                table[conn].Parent = operatingPoint;
        //            }
        //        }
        //        table[operatingPoint].Visited = true;
        //    }

        //    return table;
        //}

        private Dictionary<WayPointObject, DictryTable<WayPointObject>> GetDictryTable(WayPointObject start, IEnumerable<WayPointObject> points)
        {
            Dictionary<WayPointObject, DictryTable<WayPointObject>> table = new Dictionary<WayPointObject, DictryTable<WayPointObject>>();
            foreach (var point in points)
            {
                table.Add(point, new DictryTable<WayPointObject>());
            }

            WayPointObject operatingPoint = start;
            table[operatingPoint].Distance = 0;

            while (true)
            {
                WayPointObject closestPoint = null;
                double minDistance = double.MaxValue;

                foreach (var kvp in table)
                {
                    if (!kvp.Value.Visited && kvp.Value.Distance < minDistance)
                    {
                        closestPoint = kvp.Key;
                        minDistance = kvp.Value.Distance;
                    }
                }

                if (closestPoint == null)
                {
                    // Brak dostępnych wierzchołków do odwiedzenia.
                    break;
                }

                foreach (WayPointObject conn in closestPoint.Connections)
                {
                    double dist = closestPoint.GetDistance(conn) + table[closestPoint].Distance;
                    double currentDist = table[conn].Distance;
                    if (dist < currentDist)
                    {
                        table[conn].Distance = dist;
                        table[conn].Parent = closestPoint;
                    }
                }

                table[closestPoint].Visited = true;
            }

            return table;
        }

        private List<IBaseObject> GetWayFromTableDictry(Dictionary<WayPointObject, DictryTable<WayPointObject>> table, WayPointObject from)
        {
            List<IBaseObject> points = new List<IBaseObject>();

            WayPointObject parent = from;

            while (parent != null)
            {
                points.Insert(0, parent);
                DictryTable<WayPointObject> fromTab = table[parent];
                parent = fromTab.Parent;
            }

            return points;

        }

        #endregion

        #region Helper Class

        private class DictryTable<T> where T : class
        {
            public double Distance { get; set; } = double.MaxValue;
            public bool Visited { get; set; } = false;
            public T Parent { get; set; } = null;

        }

        #endregion

    }
}