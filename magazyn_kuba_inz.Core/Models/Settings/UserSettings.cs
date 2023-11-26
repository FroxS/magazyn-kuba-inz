using System.Globalization;
using System.IO;
using Warehouse.Theme;

namespace Warehouse.Core.Models.Settings;

public class UserSettings : BaseSettings
{
    #region Private properties

    private const string FILENAME = "UserSettings.json";

    private ColorScheme _colorScheme = ColorScheme.Dark;

    private CultureInfo? _language ;

    #endregion

    #region Public properties

    public ColorScheme ColorScheme
    {
        get => _colorScheme;
        set { SetProperty(ref _colorScheme, value); }
    }

    public CultureInfo? Language
    {
        get => _language;
        set { SetProperty(ref _language, value); }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public UserSettings()
    {
        
    }

    #endregion

    #region Saving

    public override void Save()
    {
        string json = GetJson();
        string filePath = Path.Combine(GetApplicationPath(), FILENAME);
        File.WriteAllText(filePath, json);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(json);
        }
    }

    #endregion
}