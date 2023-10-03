﻿using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Models.WareHouse;

namespace magazyn_kuba_inz.Core.Service;

public class RackService : BaseServiceWithRepository<IRackRepository, Rack>, IRackService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public RackService(IRackRepository ripository) :base(ripository)
    {
    }

    #endregion

    #region Public Method

    #endregion
}