using magazyn_kuba_inz.Core.Models;
using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Models.WareHouse;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.IO;

namespace magazyn_kuba_inz.Core.Service;

public class HallService : BaseServiceWithRepository<IHallRepository,Hall>, IHallService
{
    #region Private fields

    private readonly IRackService _rackService;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public HallService(IHallRepository repository, IRackService rackService) :base(repository)
    {
        _rackService = rackService;
    }

    #endregion

    #region Public Method

    public HallObject GetHallObject(Guid id)
    {
        HallObject hallobj = new HallObject(id);
        var hall = _repozitory.GetById(id);
        if (hall == null)
            return null;

        if(hall.Data != null)
            hallobj = GetData(hall.Data);
        return hallobj;
    }

    public override void Update(Hall model)
    {
        if (IsHallOk(model) != null)
            return;
        UpdatehHllObject(GetData(model.Data));
    }

    public override bool Add(Hall entity)
    {
        string? message = IsHallOk(entity);
        if (message != null)
            return false;
        HallObject obj = GetData(entity.Data);
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
        var racks = GetRack(hall);
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
        foreach (Rack rack in GetRack(hall))
        {
            if (!_rackService.Exist(rack.ID))
            {
                _rackService.Add(rack);
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

    public string? IsHallOk(Hall hall) => IsHallOk(GetData(hall.Data));

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

    #endregion

    #region Private Method

    private byte[] GetData(HallObject hall)
    {
        MemoryStream ms = new MemoryStream();
        using (BsonDataWriter writer = new BsonDataWriter(ms))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented; //Format the output
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            serializer.Serialize(writer, hall);
        }

        return ms.ToArray();
    }

    private HallObject GetData(byte[] data)
    {
        HallObject hallObj;
        MemoryStream ms = new MemoryStream(data);
        using (BsonDataReader reader = new BsonDataReader(ms))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            hallObj = serializer.Deserialize<HallObject>(reader);
        }
        return hallObj;
    }

    private IEnumerable<Rack> GetRack(HallObject hall) => hall.Racks.Select(x => new Rack() { ID = x.Id, ID_Hall = hall.Id });

    #endregion
}