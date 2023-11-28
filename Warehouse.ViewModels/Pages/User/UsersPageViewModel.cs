using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Warehouse.Core.Helpers;
using Warehouse.Core.Interface;
using Warehouse.Models;
using Warehouse.Models.Enums;

namespace Warehouse.ViewModel.Pages;

public class UsersPageViewModel : BasePageSearchItemsViewModel<User>
{
    #region Fields

    private IUserService _userService => Application.GetService<IUserService>();

    #endregion

    #region Properties

    #endregion

    #region Command

    public ICommand DeleteItemsCommand { get; private set; }

    public ICommand AddItemCommand { get; private set; }

    #endregion

    #region Constructors

    public UsersPageViewModel(IApp app): base(app)
    {
        Page = Models.Page.EApplicationPage.Users;
        AddItemCommand = new AsyncRelayCommand<IList>(DeleteItems);
    }

    #endregion

    #region  Command methods

    private async Task DeleteItems(IList items)
    {
        if (_items == null)
            return;
        try
        {
            if (Application.GetDialogService().AskUser(Core.Properties.Resources.AreYouSure) == EDialogResult.Yes)
            {
                bool flag = true;
                List<User> index = new List<User>();
                foreach (User pg in items)
                {
                    await _userService.DeleteAsync(pg.ID);
                    index.Add(pg);
                };
                index.ForEach(o => Items.Remove(o));
                flag = await _userService.SaveAsync();

                if (!flag)
                {
                    Application.ShowSilentMessage(Core.Properties.Resources.FailedToRemove, EMessageType.Warning);
                }
                else
                {
                    Application.ShowSilentMessage(Core.Properties.Resources.SuccessfulRemoved, EMessageType.Ok);
                    OnPropertyChanged(nameof(Items));
                }
            }
        }
        catch (Exception ex)
        {
            Application.ShowSilentMessage(ex.Message);
        }
    }

    #endregion

    #region Private helpers

    public async override void OnPageOpen()
    {
        try
        {
            IsTaskRunning = true;

            Items = new ObservableCollection<User>(await _userService.GetAllAsync());

            IsTaskRunning = false;
        }
        catch(Exception ex)
        {
            Application.CatchExeption(ex);
        }
        finally
        {
            IsTaskRunning = false;
        }
    }

    #endregion

}
