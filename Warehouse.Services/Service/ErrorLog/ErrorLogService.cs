using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Helpers;

namespace Warehouse.Service.ErrorLog;

public static class ErrorLogService
{
    #region Private properties

    private static readonly string FileName = "warehouse.log";

    #endregion

    #region Public static methods

    public async static void ErrorLogAsync(Exception exeption)
    {
        UnicodeEncoding uniencoding = new UnicodeEncoding();
        string filePath = Path.Combine(AplicationHelper.GetAplicationPath(), FileName);

        byte[] message = uniencoding.GetBytes(GetMessage(exeption));

        using (FileStream SourceStream = File.Open(filePath, FileMode.OpenOrCreate))
        {
            SourceStream.Seek(0, SeekOrigin.End);
            await SourceStream.WriteAsync(message, 0, message.Length);
        }
    }

    public static void ErrorLog(Exception exeption)
    {
        UnicodeEncoding uniencoding = new UnicodeEncoding();
        string filePath = Path.Combine(AplicationHelper.GetAplicationPath(), FileName);

        byte[] message = uniencoding.GetBytes(GetMessage(exeption));

        int t = 0;

        using (FileStream SourceStream = File.Open(filePath, FileMode.OpenOrCreate))
        {
            SourceStream.Seek(0, SeekOrigin.End);
            SourceStream.Write(message, 0, message.Length);
        }
    }


    #endregion

    #region Public static methods

    private static string GetMessage(Exception exeption)
    {
        string message = exeption.Message;

        while (exeption.InnerException != null)
        {
            message += $"\t{GetMessage(exeption.InnerException)}\n";
        }
        return message;
    }


    #endregion
}