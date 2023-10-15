using Warehouse.Core.Models;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.InnerDialog;

public class GetHallInnerDialogViewModel : BaseInnerDialogViewModel<HallObject>
{
    #region Private properties

    public HallObject? _hall;

    #endregion

    #region Public properties

    [Required(ErrorMessage = "Name is required.")]
    public HallObject? Hall
    {
        get => _hall;
        set
        {
            _hall = value;
            OnPropertyChanged(nameof(Hall));
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public GetHallInnerDialogViewModel(IApp app, HallObject obj = null) : base(app)
    {
        Result = null;
        Hall = obj ?? new HallObject(Guid.NewGuid()) { Width = 1000, Height = 1000, Name = "Hala" };
    }

    #endregion

    #region Public Methods

    protected override void Submit()
    {
        Result = null;
        
        if(Hall?.Width <= 0 || Hall?.Height <= 0)
        {
            CustomMessage.Add(nameof(Hall), "Nieodpowiednia wysokość oraz szerokość hali");
        }

        Result = Hall;
        base.Submit();
    }

    #endregion

}