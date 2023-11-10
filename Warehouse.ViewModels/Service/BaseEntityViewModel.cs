using Warehouse.Core.Helpers;
using Warehouse.Core.Interface;
using Warehouse.Models;
using System.Windows.Input;
using Warehouse.Models.Enums;

namespace Warehouse.ViewModel.Service;

public class BaseEntityViewModel<T> : BaseViewModel where T: BaseEntity
{

    #region Private properties

    protected readonly IBaseService<T> _service;

    protected T _entity;

    protected bool _saved = true;

    protected string? message;

    protected IApp _app;

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

    public bool Enabled => CanEdit();

    #endregion

    #region ICommand 

    public ICommand SaveCommand { get; protected set; }

    #endregion

    #region Constructor

    public BaseEntityViewModel(IBaseService<T> service,T entity, IApp app)
    {
        _service = service;
        _entity = entity;
        _app = app;
        SaveCommand = new AsyncRelayCommand<bool>((o) => SaveAsync(), o => Enabled && !Saved);
    }

    #endregion

    #region Protected methods

    protected virtual string[] GetpropsNameToFireOnSave() { return null; }

    protected bool CanEdit()
    {
        User? user = _app?.User;
        if(user != null)
        {
            if ((user.Type & (EUserType.Admin | EUserType.Boss | EUserType.Employee_Office)) != 0)
                return true;
        }
        return false;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Method to check is entity is valid. If Entity is valid just return null 
    /// else returns message
    /// </summary>
    /// <returns></returns>
    public virtual string? Valid() => null;

    public virtual async Task<bool> SaveAsync()
    {
        try
        {
            if (_service == null)
                return true;
            if (Saved)
                return Saved;

            _CanValidate = true;
            OnPropertiesChanged(GetpropsNameToFireOnSave());
            Message = Valid();
            if(Message == null)
            {
                _service.Update(_entity);
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
            _service.Update(_entity);
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

