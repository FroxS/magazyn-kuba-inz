using Warehouse.ViewModel.Service;
using Warehouse.Core.Interface;
using Warehouse.Core.Models.Settings;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Warehouse.ViewModel.Pages;

public class SettingsPageViewModel : BasePageViewModel
{
    #region Fields settings

    private UserSettings _userSettings;

    private GlobalSettings _globalSettings;

    private ObservableCollection<CultureInfo> _languages;

    #endregion 

    #region  Properties Settings

    public UserSettings UserSettings
    {
        get => _userSettings;
        set { SetProperty(ref _userSettings, value); }
    }

    public GlobalSettings GlobalSettings
    {
        get => _globalSettings;
        set { SetProperty(ref _globalSettings, value); }
    }

    public ObservableCollection<CultureInfo> Languages
    {
        get => _languages;
        set { SetProperty(ref _languages, value); }
    }

    #endregion

    #region Constructors

    public SettingsPageViewModel(IApp app) : base(app)
    {
        Page = Models.Page.EApplicationPage.Settings;
        Languages = new ObservableCollection<CultureInfo>(CultureInfo.GetCultures(CultureTypes.NeutralCultures));
    }

    #endregion

    #region Load

    public override void OnPageOpen()
    {
        GlobalSettings = Application.GetService<GlobalSettings>();
        UserSettings = Application.GetService<UserSettings>();
    }

    public override void OnPageClose()
    {
        GlobalSettings.Save();
        UserSettings.Save();
    }

    #endregion
}
