using Warehouse.Theme.Conventers;
using System;
using System.Globalization;
using System.ComponentModel;
using System.Reflection;
using Warehouse.Core.Properties;

namespace Warehouse.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class EnumToTextConventer : BaseValueConventer<EnumToTextConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Enum myEnum = value as Enum;
        if (myEnum == null)
            return null;
        string description = GetEnumDescription(myEnum);
        return translate(description);
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

    #region Private Helpers

    /// <summary>
    /// Method to translate text
    /// </summary>
    /// <param name="text">Text to translate</param>
    /// <returns></returns>
    private string translate(string text)
    {
        try
        {
            var t = string.Empty;
            t = Resources.ResourceManager.GetString(text, Core.Properties.Resources.Culture);
            return string.IsNullOrEmpty(t) ? text : t;
        }
        catch { return text; }
    }

    /// <summary>
    /// MEthod to get all decctiption value from Enum if exist
    /// </summary>
    /// <param name="enumObj"></param>
    /// <returns></returns>
    private string GetEnumDescription(Enum enumObj)
    {
        FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

        object[] attribArray = fieldInfo.GetCustomAttributes(false);

        if (attribArray.Length == 0)
        {
            return enumObj.ToString();
        }
        else
        {
            DescriptionAttribute attrib = attribArray[0] as DescriptionAttribute;
            return attrib.Description;
        }
    }

    #endregion

}
