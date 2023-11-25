using Warehouse.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Warehouse.Core.Models;
using Microsoft.EntityFrameworkCore;
using Warehouse.Core.Interface;
using Warehouse.Repository.Interfaces;

namespace Warehouse.Service;

internal class HallService : BaseServiceWithRepository<IHallRepository,Hall>, IHallService
{
    #region Private fields

    private readonly IRackService _rackService;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructo
    /// </summary>
    public HallService(IHallRepository repozitory, IRackService rackService, IApp app) : base(repozitory, app)
    {
        _rackService = rackService;
    }

    #endregion

    #region Public Method

    public HallObject GetHallObject(Guid id)
    {
        HallObject hallobj = new HallObject(id);
        Hall? hall = _repozitory.GetById(x => x.Include(i => i.Racks),id);
        if (hall == null)
            return null;

        if(hall.Data != null)
            hallobj = GetData<HallObject>(hall.Data);
        IEnumerable<RackObject> racksTODell = hallobj.Racks.Where(x => !hall.Racks.Any(db => db.ID == x.ID));
        foreach(RackObject rackTODell in new List<RackObject>(racksTODell))
            hallobj.Racks.Remove(rackTODell);

        return hallobj;
    }

    public override void Update(Hall model)
    {
        if (IsHallOk(model) != null)
            return;
        UpdatehHllObject(GetData<HallObject>(model.Data));
    }

    public override bool Add(Hall entity)
    {
        string? message = IsHallOk(entity);
        if (message != null)
            return false;
        HallObject obj = GetData<HallObject>(entity.Data);
        if (!UpdateRacks(obj))
            return false;
        entity.Name = obj.Name;
        return base.Add(entity);
    }

    public bool UpdatehHllObject(HallObject obj)
    {
        Hall hall = _repozitory.GetById(obj.Id);
        if(hall == null) return false;
        string? message = IsHallOk(obj);

        if (message != null)
            return false;

        hall.Name = obj.Name;

        hall.Data = GetData(obj);

        if (!UpdateRacks(obj))
            return false;
        base.Update(hall);
        return true;
    }

    public override bool Save()
    {
        return base.Save() && _rackService.Save();
    }

    public bool UpdateRacks(HallObject hall)
    {

        var racks = hall.Racks;
        var inDatabase = _rackService.GetAll().Where(x => x.ID_Hall == hall.Id);
        List<Guid> ToDel = new List<Guid>();
        foreach (Rack rack in inDatabase)
        {
            if (!racks.Any(x => x.ID == rack.ID))
                ToDel.Add(rack.ID);
        }
        if (!ToDel.Select(_rackService.CanDeleteRack).All(b => b))
            return false;

        ToDel.ForEach((o) => {
            _rackService.Delete(o);
        });


        foreach (Rack rack in hall.Racks)
        {
            rack.ID_Hall = hall.Id;
            Rack found = inDatabase.FirstOrDefault(x => x.ID == rack.ID);//_rackService.GetById(rack.ID);
            if (found == null)
            {
                _rackService.Add(rack);
            }
            else
            {
                found.Width = rack.Width;
                found.Heigth = rack.Heigth;
                found.Lp = rack.Lp;
                found.Corridor = rack.Corridor;
                found.Row = rack.Row;
                found.CreatedAt = rack.CreatedAt;
                found.Modified = rack.Modified;
                found.AmountSpace = rack.AmountSpace;
                found.Deepth = rack.Deepth;
                found.Flors = rack.Flors;
                found.Direction = rack.Direction;
                found.ID_Hall = rack.ID_Hall;
                _rackService.Update(found);
            } 
        }

        return true;
    }

    public Hall GetHall(HallObject obj)
    {
        Hall hall = _repozitory.GetById(obj.Id);
        if (hall == null)
            hall = new Hall() { ID = obj.Id, Name = obj.Name};
        hall.Data = GetData(obj);
        return hall;
    }

    public string? IsHallOk(Hall hall) => IsHallOk(GetData<HallObject>(hall.Data));

    public string? IsHallOk(HallObject obj)
    {
        var racks = obj.Racks;
        var points = obj.WayPoints;
        if (points.Count(x => x.IsStartPoint) != 1)
            return "Może być tylko jeden punkt startowy";

        int count = racks.Count;

        foreach(RackObject rack in racks)
        {
            foreach (RackObject rack1 in racks)
            {
                if (rack == rack1)
                    continue;
                if (rack.Intersects(rack1))
                {
                    return $"Rack {rack.Name} intersect on rack {rack1.Name}";
                }
            }

            if ((rack.WayPoints?.Count ?? 0) == 0)
                return $"Rack {rack.Name} don't have assing any point";

            foreach(WayPointObject point in rack.WayPoints)
            {
                if (!points.Contains(point))
                {
                    return $"Point: x:{point.X}, y:{point.Y} assing to rack {rack.Name} don't exist";
                }
            }

        }
        return null;
    }

    public override List<Hall> GetAll()
    {
        return _repozitory.GetAll(x => x.Include(i => i.Racks));
    }

    #endregion

    #region Private Method

    protected override T GetData<T>(byte[] data)
    {
        T hallObj = base.GetData<T>(data);

        var racks = _rackService.GetAll();
        foreach (var rack in (hallObj as HallObject).Racks)
        {
            var found = racks.FirstOrDefault(x => x.ID == rack.ID);
            if (found != null)
            {
                rack.Width = found.Width;
                rack.Heigth = found.Heigth;
                rack.Lp = found.Lp;
                rack.Corridor = found.Corridor;
                rack.Row = found.Row;
                rack.CreatedAt = found.CreatedAt;
                rack.Modified = found.Modified;
                rack.AmountSpace = found.AmountSpace;
                rack.Deepth = found.Deepth;
                rack.Flors = found.Flors;
                rack.Direction = found.Direction;
                rack.ID_Hall = found.ID_Hall;
            }
        }
        return hallObj;
    }

    #endregion
}