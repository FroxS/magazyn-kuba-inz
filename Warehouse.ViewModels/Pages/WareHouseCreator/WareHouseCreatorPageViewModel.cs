﻿using Warehouse.Core.Helpers;
using Warehouse.Core.Models;
using Warehouse.ViewModel.Service;
using System.Windows.Input;
using Warehouse.Core.Interface;
using Warehouse.Models;
using System.Collections.ObjectModel;

namespace Warehouse.ViewModel.Pages;

public class WareHouseCreatorPageViewModel : BasePageViewModel
{
    #region Private fields

    protected IHallService _hallService;
    protected IRackService _rackService;
    protected HallObject _hall;
    protected bool _canEdit = false;
    protected bool _toSave = false;
    protected IBaseObject _selectedObject;
    protected Func<RackObject,bool> _canDeleteRack;

    #endregion

    #region Public properties

    public HallObject Hall
    {
        get => _hall;
        set => SetProperty(ref _hall, value); 
    }

    public IBaseObject SelectedObject
    {
        get => _selectedObject;
        set { SetProperty(ref _selectedObject, value, nameof(SelectedObject), 
            () =>
            {
                if(value != null && _selectedObject is RackObject ro)
                {
                    ro.Items = new ObservableCollection<StorageItem>(_rackService.GetAllPackages(ro.ID).SelectMany(x => x.Items));
                }
                _selectedObject.CanEdit = CanEdit;
            }
            ); 
        }
    }

    public Func<RackObject, bool> CanDeleteRack
    {
        get => _canDeleteRack;
        set => SetProperty(ref _canDeleteRack, value); 
    }

    public bool CanEdit
    {
        get => _canEdit;
        set => SetProperty(ref _canEdit, value); 
    }

    public bool ToSave
    {
        get => _toSave;
        set => SetProperty(ref _toSave, value); 
    }


    #endregion

    #region Commands

    public ICommand AddRackCommand { get; protected set; }

    public ICommand RemoveRackCommand { get; protected set; }

    public ICommand EditHallCommand { get; protected set; }

    public ICommand EditCommand { get; protected set; }

    public ICommand EditRackCommand { get; protected set; }

    #endregion

    #region Constructors

    public WareHouseCreatorPageViewModel(IApp app) : base(app)
    {
        Page = Models.Page.EApplicationPage.WareHouseCreator;
        _hallService = app.GetService<IHallService>();
        _rackService = app.GetService<IRackService>();
        EditHallCommand = new AsyncRelayCommand(() => EditHall(), (o) => CanEdit);
        EditCommand = new RelayCommand(() => { CanEdit = true; ToSave = true; }, () => !CanEdit) ;
        CanDeleteRack = (rack) => {
            bool flag = _rackService.CanDeleteRack(rack.ID);
            if (!flag)
                Application.ShowSilentMessage("Nie można usunąć stojaka, Prawdopodobnie posiada jakieś elementy");

            return flag;
        };

		EditRackCommand = new RelayCommand(() => EditRack(), () => SelectedObject is RackObject);
    }

    private void EditRack()
    {
        try
        {
            if(SelectedObject is RackObject rack)
                Application.Navigation.OpenNewTabPage(new RackEditViewModel(rack.ID, Application));
        }
        catch(Exception ex)
        {
            Application.CatchExeption(ex);
        }
    }

    #endregion

    #region Private methods

    public override void OnPageOpen()
    {
        base.OnPageOpen(); 
        var hall = _hallService.GetAll().FirstOrDefault();
        if (hall == null)
        {
            Application.GetInnerDialogService().GetHallInnerDialog((o) => {

                if (o == null)
                {
                    Application.Navigation.SetPage(Warehouse.Models.Page.EApplicationPage.DashBoard);
                    return;
                }
                var p1 = new WayPointObject(100, 100) { IsStartPoint = true };
                var p2 = new WayPointObject(200, 100);
                p1.AddConnection(ref p2);
                o.WayPoints.Add(p1);
                o.WayPoints.Add(p2);
                Hall = o;
            });
        }
        else
        {
            Hall = _hallService.GetHallObject(hall.ID);
        }
    }

    private async Task EditHall()
    {
        HallObject hall = await Application.GetInnerDialogService().GetHallInnerDialog(Hall);

        if (hall != null)
            Hall = hall;
    }

    public override void OnPageClose()
    {
        if (Hall == null || !ToSave)
            return;
        string? message = _hallService.IsHallOk(Hall);
        if (message != null)
        {
            Application.ShowSilentMessage(message);
            CanChangePage = false;
        }
        else
        {
            if (_hallService.Exist(Hall.Id))
            {
                CanChangePage = _hallService.UpdatehHllObject(Hall);
            }
            else
            {
                CanChangePage = _hallService.Add(_hallService.GetHall(Hall));
            }
            if (CanChangePage)
                _hallService.Save();
        } 
    }

    #endregion

    #region Command Methods

    
    #endregion

    #region Public Methods



    #endregion

    #region Private methods

    

    #endregion
}
