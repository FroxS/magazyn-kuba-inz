using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models;
using magazyn_kuba_inz.Models.Interfaces;
using magazyn_kuba_inz.Models.WareHouse;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Input;

namespace magazyn_kuba_inz.Core.ViewModel.Login;

public class LoginViewModel : BaseViewModel
{
    #region Private  Properties

    private readonly IApp app;

    private string messages = "";

    private string? login;

    #endregion

    #region Public Properties

    public IUser? User { get; private set; }

    public string Messages { get => messages; private set { messages = value; OnPropertyChanged(nameof(Messages)); } }

    public string? Login { get => login; set { login = value; OnPropertyChanged(nameof(Login)); } }

    public SecureString? SecurePassword { private get; set; }

    #endregion

    #region Public Commands

    public ICommand MinimizeCommand { get; private set; }
    public ICommand ExitCommand { get; private set; }
    public ICommand LoginCommand { get; private set; }

    #endregion

    #region Constructors

    public LoginViewModel(IApp app) : base()
    {
        LoginCommand = new RelayCommand<IWindow>(submit);
        MinimizeCommand = new RelayCommand<IWindow>(minimize);
        ExitCommand = new RelayCommand(o => exit());
        this.app = app;
    }

    #endregion

    #region Commands methods

    private void submit(IWindow window)
    {
        try
        {
            Messages = "";
            if (string.IsNullOrEmpty(Login))
            {
                Messages = "Podaj login";
                return;
            }

            if (SecurePassword == null || SecurePassword.Length <= 0)
            {
                Messages = "Podaj hasło";
                return;
            }

            if (!SecureStringEqual(SecurePassword, getSecureString("admin")))
            {
                Messages = "Hasło niepoprawne";
                return;
            }

            User = new User();


            if (User != null)
                window.DialogResult = true;
        }
        catch
        {
            Messages = "Wystąpił błąd";
        }

    }

    private void minimize(IWindow window)
    {
        window.WindowState = System.Windows.WindowState.Minimized;
    }

    private void exit()
    {
        app.Exit();
    }

    #endregion

    #region Private Helper

    private bool SecureStringEqual(SecureString s1, SecureString s2)
    {
        if (s1 == null)
        {
            throw new ArgumentNullException("s1");
        }
        if (s2 == null)
        {
            throw new ArgumentNullException("s2");
        }

        if (s1.Length != s2.Length)
        {
            return false;
        }

        IntPtr bstr1 = IntPtr.Zero;
        IntPtr bstr2 = IntPtr.Zero;

        RuntimeHelpers.PrepareConstrainedRegions();

        try
        {
            bstr1 = Marshal.SecureStringToBSTR(s1);
            bstr2 = Marshal.SecureStringToBSTR(s2);

            unsafe
            {
                for (Char* ptr1 = (Char*)bstr1.ToPointer(), ptr2 = (Char*)bstr2.ToPointer();
                    *ptr1 != 0 && *ptr2 != 0;
                     ++ptr1, ++ptr2)
                {
                    if (*ptr1 != *ptr2)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        finally
        {
            if (bstr1 != IntPtr.Zero)
            {
                Marshal.ZeroFreeBSTR(bstr1);
            }

            if (bstr2 != IntPtr.Zero)
            {
                Marshal.ZeroFreeBSTR(bstr2);
            }
        }
    }

    private SecureString getSecureString(string text)
    {
        if (text == null)
            throw new ArgumentNullException("password");

        var securePassword = new SecureString();

        foreach (char c in text)
            securePassword.AppendChar(c);

        securePassword.MakeReadOnly();
        return securePassword;
    }

    #endregion


}
