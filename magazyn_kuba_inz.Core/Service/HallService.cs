using magazyn_kuba_inz.Core.Models;
using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Models.WareHouse;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;

namespace magazyn_kuba_inz.Core.Service;

public class HallService : BaseServiceWithRepository<IHallRepository,Hall>, IHallService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public HallService(IHallRepository repository) :base(repository)
    {
    }

    #endregion

    #region Public Method

    public HallObject GetHallObject(Guid id)
    {
        HallObject hallobj = new HallObject();
        var hall = _repozitory.GetById(id);
        if (hall == null)
            return null;

        if(hall.Data != null)
        {
            hallobj = GetData(hall.Data);
        }

        return hallobj;
    }

    public byte[] GetData(HallObject hall)
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

    public HallObject GetData(byte[] data)
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

    #endregion
}