using System.Windows.Input;
using Warehouse.Core.Helpers;
using Warehouse.Core.Interface;
using Warehouse.Models;
using Warehouse.Models.Enums;

namespace Warehouse.Service;

public class MessageService : ObservableObject, IMessageService
{
    #region Private properties

    private string? _message;
    private EMessageType _type;

    #endregion

    #region Public properties

    public string? Message
    {
        get => _message;
        private set
        {
            _message = value;
            OnPropertyChanged(nameof(Message));
        }
    }

    public EMessageType Type
    {
        get => _type;
        private set
        {
            _type = value;
            OnPropertyChanged(nameof(Type));
        }
    }

    #endregion

    #region Command

    public ICommand HideMessageCommand { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public MessageService()
    {
        HideMessageCommand = new RelayCommand(() => Message = null);
    }

    #endregion

    #region Public methods

    public void AddMessage(string message, EMessageType type = EMessageType.Warning)
    {
        Type = type;
        Message = message;
    }

    public void Clear()
    {
        HideMessageCommand.Execute(null);
    }

    #endregion

}