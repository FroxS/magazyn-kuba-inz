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

    public ICommand EdituserCommand { get; private set; }

    #endregion

    #region Constructors

    public UsersPageViewModel(IApp app): base(app)
    {
        Page = Models.Page.EApplicationPage.Users;
        DeleteItemsCommand = new AsyncRelayCommand<IList>(DeleteItems);
        AddItemCommand = new AsyncRelayCommand(Add);
        EdituserCommand = new RelayCommand<User>(Edit);
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

    private void Edit(User user)
    {
        try
        {
            if (user == null)
                return;


            Application.Navigation.OpenUser(user);

        }
        catch (Exception ex) { Application.CatchExeption(ex); }
        
    }

    #endregion

    #region Private helpers

    private async Task Add()
    {
        try
        {
            User added = await Application.GetInnerDialogService().GetUser();

            if (added == null)
                return;

            IUserService service = Application.GetService<IUserService>();

            service.Add(added);
            service.Save();
            Items.Add(added);
            SelectedItem = added;
            Application.ClearSilentMessage();

        }catch(Exception ex)
        {
            Application.CatchExeption(ex);
        }
    }

    public override void OnPageClose()
    {
        _userService.Save();
    }

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

    protected override void OnItemChanged(User? oldItem, User? newItem)
    {
        
    }

    #endregion

}
