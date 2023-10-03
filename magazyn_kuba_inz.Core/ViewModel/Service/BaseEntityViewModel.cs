﻿using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Models.WareHouse;
using System.Windows.Input;

namespace magazyn_kuba_inz.Core.ViewModel.Service;

public class BaseEntityViewModel<T> : BaseViewModel where T: BaseEntity
{

    #region Private properties

    protected readonly IBaseService<T> _service;

    protected T _entity;

    protected bool _saved = true;

    protected bool _enabled = false;

    protected string? message;

    #endregion

    #region Public Properties

    public string? Message
    {
        get => message;
        set
        {
            if (message != value)
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
    }

    public virtual uint Lp
    {
        get => _entity.Lp;
        set
        {
            if (_entity.Lp == value)
                return;
            Saved = false;
            _entity.Lp = value;
            OnPropertyChanged(nameof(Lp));
        }
    }

    public virtual DateTime CreatedAt
    {
        get => _entity.CreatedAt;
    }

    public Guid ID
    {
        get => _entity.ID;
    }

    public bool Saved
    {
        get => _saved;
        protected set
        {
            _saved = value;
            OnPropertyChanged(nameof(Saved));
        }
    }

    public bool Enabled
    {
        get => _enabled;
        protected set
        {
            _enabled = value;
            OnPropertyChanged(nameof(Enabled));
        }
    }

    #endregion

    #region ICommand 

    public ICommand SaveCommand { get; protected set; }

    public ICommand EditCommand { get; protected set; }

    #endregion

    #region Constructor

    public BaseEntityViewModel(IBaseService<T> service,T entity)
    {
        _service = service;
        _entity = entity;
        Enabled = false;
        SaveCommand = new AsyncRelayCommand<bool>((o) => SaveAsync(), o => Enabled && !Saved);
        EditCommand = new RelayCommand((o) => SetEnabled(true), (o) => !Enabled);
    }

    #endregion

    #region Protected methods

    protected virtual string[] GetpropsNameToFireOnSave() { return null; }

    #endregion

    #region Public methods

    /// <summary>
    /// Method to check is entity is valid. If Entity is valid just return null 
    /// else returns message
    /// </summary>
    /// <returns></returns>
    public virtual string? Valid() => null;

    public virtual void SetEnabled(bool enable = false)
    {
        Enabled = enable;
    }

    public virtual async Task<bool> SaveAsync()
    {
        try
        {
            if (Saved)
                return Saved;

            _CanValidate = true;
            OnPropertiesChanged(GetpropsNameToFireOnSave());
            Message = Valid();
            if(Message == null)
            {
                //return Saved = await _service.AddAsync(Entity); ;
                return Saved = await _service.SaveAsync(); ;
            }
            return Saved = false;
        }
        catch
        {
            return Saved = false;
        }
      
    }

    public virtual bool Save()
    {
        try
        {
            if (Saved)
                return Saved;
            _CanValidate = true;
            OnPropertiesChanged(GetpropsNameToFireOnSave());

            Saved = _service.Save();
            return Saved;
        }
        catch
        {
            return Saved = false;
        }
    }

    public virtual void OnLoad() { }

    public virtual T Get() => _entity;

    #endregion

}

