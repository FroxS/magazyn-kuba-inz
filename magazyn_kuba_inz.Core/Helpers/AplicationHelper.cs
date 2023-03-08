using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace magazyn_kuba_inz.Core.Helpers;

public static class AplicationHelper
{

    #region Public static properties

    public static string GetAplicationPath() => AppDomain.CurrentDomain.BaseDirectory;

    #endregion

}