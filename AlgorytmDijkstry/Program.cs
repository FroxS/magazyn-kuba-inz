using System;
using System.Net.Http.Headers;
using System.Windows.Controls;
using Warehouse.Core.Interface;
using Warehouse.Core.Models;
using Warehouse.Models;

namespace AlgorytmDijkstry
{
    internal class Program
    {
        public static double MaxThershold = 2;

        static void Main(string[] args)
        {
            List<StorageItemPackage> items = new List<StorageItemPackage>();

            Product p1 = new Product() {ID = Guid.NewGuid(), Name = "P1", Weight = 7 };
            Product p2 = new Product() {ID = Guid.NewGuid(), Name = "P2", Weight = 10 };
            Product p3 = new Product() {ID = Guid.NewGuid(), Name = "P3", Weight = 15 };
            Product p4 = new Product() {ID = Guid.NewGuid(), Name = "P4", Weight = 5 };

            Product p5 = new Product() {ID = Guid.NewGuid(), Name = "P5", Weight = 5 };
            Product p6 = new Product() {ID = Guid.NewGuid(), Name = "P6", Weight = 5 };
            Product p7 = new Product() {ID = Guid.NewGuid(), Name = "P7", Weight = 4 };
            Product p8 = new Product() {ID = Guid.NewGuid(), Name = "P8", Weight = 4 };

            HallObject hall = GetHall();
            hall.Racks.FirstOrDefault(x => x.Name == "R1").StorageItems = new List<StorageItemPackage>() { GetPackage(5, p1) };
            hall.Racks.FirstOrDefault(x => x.Name == "R2").StorageItems = new List<StorageItemPackage>() { GetPackage(5, p2) };
            //hall.Racks.FirstOrDefault(x => x.Name == "R3").StorageItems = new List<StorageItemPackage>() { GetPackage(5, p3) };
            hall.Racks.FirstOrDefault(x => x.Name == "R3").StorageItems = new List<StorageItemPackage>() { GetPackage(5, p1) };
            hall.Racks.FirstOrDefault(x => x.Name == "R4").StorageItems = new List<StorageItemPackage>() { GetPackage(5, p4) };

            hall.Racks.FirstOrDefault(x => x.Name == "R5").StorageItems = new List<StorageItemPackage>() { GetPackage(5, p5) };
            hall.Racks.FirstOrDefault(x => x.Name == "R6").StorageItems = new List<StorageItemPackage>() { GetPackage(5, p6) };
            hall.Racks.FirstOrDefault(x => x.Name == "R7").StorageItems = new List<StorageItemPackage>() { GetPackage(5, p7) };
            hall.Racks.FirstOrDefault(x => x.Name == "R8").StorageItems = new List<StorageItemPackage>() { GetPackage(5, p8) };



            List<object> trace = null;
            WayPointObject start = hall.WayPoints.FirstOrDefault(x => x.IsStartPoint);
            List<Product> order = new List<Product>() { p1, p5, p8 };
            DijkstraAlgorithm alg = new DijkstraAlgorithm();
            trace = alg.GetPath(start, hall, order);
            foreach (var objtrace in trace)
            {
                if (objtrace is WayPointObject wp)
                    Console.WriteLine($"Punkt {wp.Name}");
                if (objtrace is RackObject r)
                    Console.WriteLine($"Stojak {r.Name}");
                if (objtrace is Product p)
                    Console.WriteLine($"Produkt {p.Name}");
            }

            //DisplayHall(hall);

            Console.ReadKey();
        }

        private static void DisplayHall(HallObject hall)
        {
            Console.WriteLine($"Hala o wymiarach {hall.Width} x {hall.Height}");

            foreach (WayPointObject point in hall.WayPoints)
            {
                foreach (WayPointObject assignPoins in point.Connections)
                {
                    //point.GetDistance(assignPoins);

                    Console.WriteLine($"{point.Name}->{assignPoins.Name}");
                }

                if (point.Racks.Count > 0)
                {
                    Console.WriteLine($"{point.Name} ma przypisane stojaki:");
                    foreach (RackObject rack in point.Racks)
                    {
                        Console.WriteLine($"\t - Stojak : {rack.Name}");

                        List<Product> products = rack.GetProducts();

                        if (products.Count > 0)
                        {
                            Console.WriteLine($"\tStojak {rack.Name} posiada produkty:");
                            foreach (Product product in products)
                            {
                                Console.WriteLine($"\t\t - Produkt {product.Name} ");
                            }
                        }

                    }
                }
            }
        }

        private static StorageItemPackage GetPackage(uint count ,params Product[] products)
        {
            StorageItemPackage package = StorageItemPackage.Get();
            package.Items = new List<StorageItem>();
            foreach(Product product in products)
            {
                for (int i = 0; i < count; i++)
                {
                    StorageItem item = StorageItem.Get();
                    item.Item = WareHouseItem.Get();
                    item.ID_Item = item.Item.ID;
                    item.Item.ID_Product = product.ID;
                    item.Item.Product = product;
                    package.Items.Add(item);
                }    
            }
            return package;
        }

        private static HallObject GetHall()
        {
            HallObject hall = new HallObject(Guid.NewGuid()) { Height = 1000d,Width = 1000d };

            WayPointObject a = new WayPointObject("A",1, 1) { IsStartPoint = true  };

            WayPointObject b = new WayPointObject("B", 1, 2);
            WayPointObject c = new WayPointObject("C", 1, 3);
            WayPointObject d = new WayPointObject("D", 2, 3);
            WayPointObject e = new WayPointObject("E", 3, 3);
            WayPointObject f = new WayPointObject("F", 3, 2);
            WayPointObject g = new WayPointObject("G", 4, 3);
            WayPointObject h = new WayPointObject("H", 4, 2);

            WayPointObject i = new WayPointObject("I", 1, 4);
            WayPointObject j = new WayPointObject("J", 1, 5);
            WayPointObject k = new WayPointObject("K", 2, 5);
            WayPointObject l = new WayPointObject("L", 2, 4);
            WayPointObject m = new WayPointObject("M", 3, 5);
            WayPointObject n = new WayPointObject("N", 4, 5);
            WayPointObject o = new WayPointObject("O", 4, 4);


            a.AddConnection(ref b);
            b.AddConnection(ref c);
            c.AddConnection(ref d);
            d.AddConnection(ref e);
            e.AddConnection(ref f);
            e.AddConnection(ref g);
            g.AddConnection(ref h);

            c.AddConnection(ref i);
            i.AddConnection(ref j);
            j.AddConnection(ref k);
            k.AddConnection(ref l);
            k.AddConnection(ref m);
            l.AddConnection(ref d);
            m.AddConnection(ref n);
            n.AddConnection(ref o);
            o.AddConnection(ref g);

            RackObject r1 = new RackObject(Guid.NewGuid()) { Name = "R1" };
            RackObject r2 = new RackObject(Guid.NewGuid()) { Name = "R2" };

            RackObject r3 = new RackObject(Guid.NewGuid()) { Name = "R3" };
            RackObject r4 = new RackObject(Guid.NewGuid()) { Name = "R4" };

            RackObject r5 = new RackObject(Guid.NewGuid()) { Name = "R5" };
            RackObject r6 = new RackObject(Guid.NewGuid()) { Name = "R6" };

            RackObject r7 = new RackObject(Guid.NewGuid()) { Name = "R7" };
            RackObject r8 = new RackObject(Guid.NewGuid()) { Name = "R8" };

            hall.AddRacks(r1, r2, r3, r4, r5, r6, r7, r8);

            r1.AddConnection(ref f);
            r2.AddConnection(ref f);
            r3.AddConnection(ref h);
            r4.AddConnection(ref h);

            r5.AddConnection(ref l);
            r6.AddConnection(ref l);
            r7.AddConnection(ref o);
            r8.AddConnection(ref o);

            hall.AddPoints(a, b, c, d, e, f, g, h, i, j, k, l, m, n ,o);
            return hall;
        }
    }

    public class DictryTable<T> where T : class
    {
        public double Distance { get; set; } = double.MaxValue;
        public bool Visited { get; set; } = false;
        public T Parent { get; set; } = null;

        public DictryTable()
        {
        }
    }

    public class DijkstraAlgorithm
    {
        #region Private fields

        private readonly double _maxThershold;

        #endregion

        #region Constructor

        public DijkstraAlgorithm(double maxThershold = 2)
        {
            _maxThershold = maxThershold;
        }

        #endregion

        #region Public methods

        public List<object> GetPath(WayPointObject p_start, HallObject hall, List<Product> products)
        {
            List<object> trace = new List<object>();
            IEnumerable<WayPointObject> points = hall.WayPoints;
            products = products.OrderByDescending(x => x.Weight).ToList();

            List<Product> all = new List<Product>(products);

            WayPointObject startForm = p_start;

            while (!(all.Count == 0))
            {
                Product product = all.First();
                List<Product> productsToSearch = TakeSimilarProducts(product, all.Where(x => x != product));
                List<KeyValuePair<Product, KeyValuePair<double, List<IBaseObject>>>> distances = new List<KeyValuePair<Product, KeyValuePair<double, List<IBaseObject>>>>();

                foreach (var productToSearch in productsToSearch)
                {
                    var result = GetShortestRoute(startForm, hall, productToSearch);
                    distances.Add(new KeyValuePair<Product, KeyValuePair<double, List<IBaseObject>>>(productToSearch, result));
                }

                var found = distances.OrderBy(kv => kv.Value.Key).FirstOrDefault();

                trace.AddRange(found.Value.Value);

                int lastWayIndeks = found.Value.Value.Count;
                if (found.Value.Value[lastWayIndeks - 1] is WayPointObject wpo)
                    startForm = wpo;
                else
                    startForm = found.Value.Value[lastWayIndeks - 2] as WayPointObject;
                trace.Add(found.Key);

                var indeksTODell = all.IndexOf(found.Key);

                all.RemoveAt(indeksTODell);
            }

            WayPointObject lastWayPoint = null;
            int i = 1;
            while (lastWayPoint == null)
            {
                lastWayPoint = trace[trace.Count - i] as WayPointObject;
                i++;
            }
            var dttohome = GetDictryTable(lastWayPoint, hall.WayPoints);
            var toHome = GetWayFromTableDictry(dttohome, p_start);
            trace.AddRange(toHome);
            return trace;
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

            //var dotest = table.Select(x => $"{x.Key.Name};{x.Value.Visited};{x.Value.Distance};{x.Value.Parent?.Name}");
            List<IBaseObject> trace = GetWayFromTableDictry(table, nearestPoint);

            if (addRackToResult)
                trace.Add(nearesRack);
            return new KeyValuePair<double, List<IBaseObject>>(tab.Distance, trace);
        }

        private Dictionary<WayPointObject, DictryTable<WayPointObject>> GetDictryTable(WayPointObject start, IEnumerable<WayPointObject> points)
        {
            Dictionary<WayPointObject, DictryTable<WayPointObject>> table = new Dictionary<WayPointObject, DictryTable<WayPointObject>>();
            foreach (var point in points)
            {
                table.Add(point, new DictryTable<WayPointObject>());
            }

            WayPointObject operatingPoint = start;
            table[operatingPoint].Distance = 0;
            /// Tworzenie tablicy Dictry
            while (!table.Values.All(x => x.Visited))
            {
                operatingPoint = table.Where(x => !x.Value.Visited).OrderBy(kv => kv.Value.Distance).FirstOrDefault().Key;

                foreach (WayPointObject conn in operatingPoint.Connections)
                {
                    double dist = conn.GetDistance(operatingPoint) + table[operatingPoint].Distance;
                    double curentDist = table[conn].Distance;
                    if (curentDist > dist)
                    {
                        table[conn].Distance = dist;
                        table[conn].Parent = operatingPoint;
                    }
                }
                table[operatingPoint].Visited = true;
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
                parent = table[parent].Parent;
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