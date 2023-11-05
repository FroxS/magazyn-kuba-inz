using Warehouse.Core.Interface;
using Warehouse.Models.Page;

namespace Warehouse.Core.Exeptions;

public class PageExeption : Exception
{

    #region Public properties

    public IBasePageViewModel PageVM { get; }

    public EApplicationPage Page { get; }

    #endregion

    #region Constructors

    /// <summary>   
    /// Default construc
    /// </summary>
    public PageExeption(string message, IBasePageViewModel pagevm) : base(message)
    {
        PageVM = pagevm;
    }

    /// <summary>   
    /// Default construc
    /// </summary>
    public PageExeption(string message, EApplicationPage page) : base(message)
    {
        Page = Page;
    }

    #endregion



}