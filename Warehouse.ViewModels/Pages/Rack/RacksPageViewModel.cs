using System.Collections.ObjectModel;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;
using Warehouse.ViewModel.Service;
using Warehouse.Core.Models;

namespace Warehouse.ViewModel.Pages;

public class RacksPageViewModel : BasePageViewModel
{
    #region Private fields

    private readonly IRackService _service;
    private ObservableCollection<RackObject> _racks;
    private RackObject _rack;

    #endregion

    #region Public Properties

    public ObservableCollection<RackObject> Racks 
    {
        get => _racks; 
        set { SetProperty(ref _racks, value, nameof(Racks)); }
    }

    public RackObject Rack
    {
        get => _rack;
        set { SetProperty(ref _rack, value, nameof(Rack)); }
    }

    #endregion

    #region Commands



    #endregion

    #region Constructors

    public RacksPageViewModel(IApp app, IRackService service) : base(app)
    {
        _service = service;
    }

    #endregion

    #region Public methods

    public override void OnPageOpen()
    {
        Racks = new ObservableCollection<RackObject>(_service.GetAll().Select(x => new RackObject(x.ID) { Flors = x.Flors, Width = x.Width, Heigth = x.Heigth }));
        Rack = Racks.FirstOrDefault();
    }

    #endregion

    #region Command Methods



    #endregion
}
