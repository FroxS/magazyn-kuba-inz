using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Interface;
using Warehouse.Core.Models;
using Warehouse.Models;

namespace Warehouse.Core.Helpers
{
    public class DijkstraResult
    {
        public List<IBaseObject> Path { get; set; }
        public double TotalDistance { get; set; }
    }

    public static class DijkstraAlgorithm
    {
        public static DijkstraResult FindShortestPath(
        WayPointObject start, List<WayPointObject> allWayPoints, List<StorageItem> products)
        {
            Dictionary<WayPointObject, double> distances = new Dictionary<WayPointObject, double>();
            Dictionary<WayPointObject, WayPointObject> previous = new Dictionary<WayPointObject, WayPointObject>();
            HashSet<WayPointObject> unvisitedWayPoints = new HashSet<WayPointObject>(allWayPoints);

            distances[start] = 0;

            while (unvisitedWayPoints.Count > 0)
            {
                WayPointObject current = distances
                    .Where(d => unvisitedWayPoints.Contains(d.Key))
                    .OrderBy(d => d.Value)
                    .First()
                    .Key;

                unvisitedWayPoints.Remove(current);

                foreach (WayPointObject neighbor in current.Connections)
                {
                    double alt = distances[current] + current.GetDistance(neighbor);
                    if (alt < distances.GetValueOrDefault(neighbor, double.PositiveInfinity))
                    {
                        distances[neighbor] = alt;
                        previous[neighbor] = current;
                    }
                }
            }

            List<IBaseObject> path = new List<IBaseObject>();
            double totalDistance = 0;
            WayPointObject currentWayPoint = start;

            while (previous.ContainsKey(currentWayPoint))
            {
                WayPointObject previousWayPoint = previous[currentWayPoint];
                double distanceToPrevious = currentWayPoint.GetDistance(previousWayPoint);
                totalDistance += distanceToPrevious;
                path.Add(currentWayPoint);
                currentWayPoint = previousWayPoint;
            }

            path.Add(start);
            path.Reverse();

            // Add racks and products to the path
            //foreach (StorageItem product in products)
            //{
            //    RackObject rack = currentWayPoint.Racks.FirstOrDefault(r => r.Items.Contains(product));
            //    if (rack != null)
            //    {
            //        path.Add(rack);
            //        currentWayPoint = rack;
            //    }
            //}

            DijkstraResult result = new DijkstraResult
            {
                Path = path,
                TotalDistance = totalDistance
            };

            return result;
        }
    }
}