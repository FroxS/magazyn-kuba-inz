using System.Collections.ObjectModel;
using Warehouse.Core.Interface;
using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System;
using System.Windows.Input;
using Warehouse.Core.Helpers;
using System.Windows;

namespace Warehouse.ViewModel.Pages;

public class RacksPageViewModel : BasePageViewModel 
{ 
    #region Private fields

    private ObservableCollection<Rack> _racks;
    private Rack _rack;
    private RackEditViewModel _rackViewModel;
    private IRackService _rackService;

    #endregion

    #region Public Properties

    public ObservableCollection<Rack> Racks 
    {
        get => _racks; 
        set { SetProperty(ref _racks, value); }
    }

    public Rack Rack
    {
        get => _rack;
        set { SetProperty(ref _rack, value, onChanged: () =>{ if (value != null && RackViewModel != null) RackViewModel.ChangeRack(value.ID); }); }
    }

    public RackEditViewModel RackViewModel
    {
        get => _rackViewModel;
        set { SetProperty(ref _rackViewModel, value); }
    }

    #endregion

    #region Command

    public ICommand OpenRackEditorCommand { get; private set; }

    #endregion


    #region Constructors

    public RacksPageViewModel(IApp app) : base(app)
    {
        Page = Models.Page.EApplicationPage.Racks;
        OpenRackEditorCommand = new RelayCommand<Rack>(OpenRackTab); 
    }

    private void OpenRackTab(Rack rack)
    {
        try
        {
            if (rack == null)
                return;

            Application.Navigation.OpenNewTabPage(new RackEditViewModel(rack.ID, Application));

        }
        catch (Exception ex) { Application.CatchExeption(ex); }
    }


    #endregion

    #region Public methods

    public override void OnPageOpen()
    {
        _rackService = Application.GetService<IRackService>();
        Racks = new ObservableCollection<Rack>(_rackService.GetAllWithItems());
        Rack = Racks.FirstOrDefault();
        RackViewModel = new RackEditViewModel(Rack?.ID ?? Guid.Empty, Application);
    }

    #endregion

    #region Command Methods

    #endregion
}
