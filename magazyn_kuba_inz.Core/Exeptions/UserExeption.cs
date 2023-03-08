namespace magazyn_kuba_inz.Core.Exeptions;

public class DataException : Exception
{

    #region Public properties

    public object? Data;
    public string? WrongProp;
    public Action Test;

    #endregion

    #region Constructors

    /// <summary>   
    /// Default construc
    /// </summary>
    public DataException(string message, object? data= null, string? wrongProp = null, Action methodToShow = null) : base(message)
    {    
        Data = data;
        WrongProp = wrongProp;
        Test = methodToShow;
    }

    #endregion



}