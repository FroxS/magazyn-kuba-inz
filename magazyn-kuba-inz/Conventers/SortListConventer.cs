using Warehouse.Core.Models;
using Warehouse.Helper;
using Warehouse.Theme.Conventers;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Reflection;
using Warehouse.Core.Properties;
using System.Collections.Generic;

namespace Warehouse.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class SortListConventer : BaseValueConventer<SortListConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            if (value is Enum en)
                return SortEnumByName(value as Enum);

            if (value is Array)
                return SortEnumByName((value as Array).GetValue(1) as Enum);

            return value;
        }
        catch (Exception ex)
        {
            return value;
        }
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private IEnumerable<T> SortEnumByName<T>(T test) where T : Enum
    {
        return from e in Enum.GetValues(test.GetType()).Cast<T>()
               let nm = translate(GetEnumDescription(e))
               orderby nm
               select e;
    }

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

}
