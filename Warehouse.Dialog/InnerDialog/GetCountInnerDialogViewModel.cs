using Warehouse.Core.Helpers;
using Warehouse.Core.Interface;
using System.Windows.Input;

namespace Warehouse.InnerDialog;

public class GetCountInnerDialogViewModel : BaseInnerDialogViewModel<double?>
{
    #region Private fields

    private double _count;

    #endregion

    #region Public properties

    public double Count 
    { 
        get => _count;
        set {
            _count = value;
            OnPropertyChanged(nameof(Count));
        }
    }

    #endregion

    #region Commands

    public ICommand NoCommand { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public GetCountInnerDialogViewModel(IApp app, double count = 1) : base(app)
    {
        Count = count;
        NoCommand = new RelayCommand(No); ;
        Result = null;
        Title = "Podaj ilosć";
    }

    #endregion

    #region Command Methods

    protected override void Submit()
    {
        Result = Count;
        base.Submit();
    }

    private void No()
    {
        Result = null;
        base.Submit();
    }

    #endregion
}