using System.Globalization;
using System.IO;
using Warehouse.Theme;

namespace Warehouse.Core.Models.Settings;

[Serializable]
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

    #region Helpers

    private string GetFile()
    {
        string file = Path.Combine(GetApplicationPath(), FILENAME);
        if (!File.Exists(file))
        {
            using(FileStream fs = File.Create(file))
            {

            }
        }
            
        return file;
    }

    #endregion

    #region Saving

    public override void Save()
    {
        string json = GetJson();
        string filePath = GetFile();
        File.WriteAllText(filePath, json);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(json);
        }
    }

    public override void Load()
    {
        string filePath = GetFile();
        UserSettings? obj = GetData<UserSettings>(File.ReadAllText(filePath) );
        if (obj == null)
            return;
		CopyProperties(obj);

    }

    #endregion
}


