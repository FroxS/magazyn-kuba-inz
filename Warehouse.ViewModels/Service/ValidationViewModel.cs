using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Warehouse.Models;

namespace Warehouse.ViewModel.Service;

public abstract class ValidationViewModel : ObservableObject, IDataErrorInfo
{
    #region Protected propert

    public abstract bool _CanValidate { get; protected set; }

    public virtual Dictionary<string, string> CustomMessage { get; private set; } = new Dictionary<string, string>();

    #endregion

    #region Private properties

    string IDataErrorInfo.Error
    {
        get
        {
            throw new NotSupportedException("IDataErrorInfo.Error is not supported, use IDataErrorInfo.this[propertyName] instead.");
        }
    }
    string IDataErrorInfo.this[string propertyName] => GettErrors(propertyName);

    #endregion

    #region Private properties


    private object GetValue(string propertyName)
    {
        PropertyInfo propInfo = GetType().GetProperty(propertyName);
        return propInfo.GetValue(this);
    }

    protected string GettErrors(string propertyName)
    {
        if (!_CanValidate) return string.Empty;
        if (string.IsNullOrEmpty(propertyName))
        {
            throw new ArgumentException("Invalid property name", propertyName);
        }

        string error = string.Empty;
        if (CustomMessage != null)
        {
            if (CustomMessage.ContainsKey(propertyName))
            {
                error = CustomMessage[propertyName];
                CustomMessage.Remove(propertyName);
                return error;
            }
        }


        var value = GetValue(propertyName);
        var results = new List<ValidationResult>(1);
        var result = Validator.TryValidateProperty(
            value,
            new ValidationContext(this, null, null)
            {
                MemberName = propertyName
            },
            results);
        if (!result)
        {
            var validationResult = results.First();
            error = validationResult.ErrorMessage;
        }
        return error;
    }

    protected string[] GettErrors()
    {
        List<string> errors = new List<string>() ;

        PropertyInfo[] properties = GetType().GetProperties();
        foreach (PropertyInfo property in properties)
        {
            string error = GettErrors(property.Name);
            if(!string.IsNullOrEmpty(error))
                errors.Add(error);
        }
        return errors.ToArray();
    }

    #endregion

}