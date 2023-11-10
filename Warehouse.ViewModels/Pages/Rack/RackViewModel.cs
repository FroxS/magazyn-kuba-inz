using Warehouse.ViewModel.Service;
using Warehouse.Models.Enums;
using Warehouse.Models;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class RackViewModel : BaseEntityViewModel<Rack>
{
    #region Public Properties

    public double Heigth
    {
        get => _entity.Heigth;
        set
        {
            SetProperty(() => Heigth, value, 
                () =>
                { 
                    if(_entity.Heigth != value)
                        Saved = false;
                    _entity.Heigth = value;
                }
            );
        }
    }

    public double Width
    {
        get => _entity.Width;
        set
        {
            SetProperty(() => Width, value,
                () =>
                {
                    if (_entity.Width != value)
                        Saved = false;
                    _entity.Width = value;
                }
            );
        }
    }

    public double Deepth
    {
        get => _entity.Deepth;
        set
        {
            SetProperty(() => Deepth, value,
                () =>
                {
                    if (_entity.Deepth != value)
                        Saved = false;
                    _entity.Deepth = value;
                }
            );
        }
    }

    public int Flors
    {
        get => _entity.Flors;
        set
        {
            SetProperty(() => Flors, value,
                () =>
                {
                    if (_entity.Flors != value)
                        Saved = false;
                    _entity.Flors = value;
                }
            );
        }
    }

    public int AmountSpace
    {
        get => _entity.AmountSpace;
        set
        {
            SetProperty(() => AmountSpace, value,
                () =>
                {
                    if (_entity.AmountSpace != value)
                        Saved = false;
                    _entity.AmountSpace = value;
                }
            );
        }
    }

    public int Corridor
    {
        get => _entity.Corridor;
        set
        {
            SetProperty(() => Corridor, value,
                () =>
                {
                    if (_entity.Corridor != value)
                        Saved = false;
                    _entity.Corridor = value;
                }
            );
        }
    }

    public EDir Direction
    {
        get => _entity.Direction;
        set
        {
            SetProperty(() => Direction, value,
                () =>
                {
                    if (_entity.Direction != value)
                        Saved = false;
                    _entity.Direction = value;
                }
            );
        }
    }

    #endregion

    #region Constructors
    public RackViewModel(IRackService service, Rack rack, IApp app) : base(service, rack, app)
    {
        Saved = true;
    }

    #endregion

    #region Private methods

    public override string? Valid()
    {
        string message = null;

        message = GettErrors(nameof(Heigth));
        if (!string.IsNullOrWhiteSpace(message))
            return message;

        return base.Valid();
    }

    #endregion

    #region Command Methods


    #endregion

}
