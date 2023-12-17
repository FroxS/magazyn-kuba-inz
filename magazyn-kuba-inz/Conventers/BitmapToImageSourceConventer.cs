using Warehouse.Core.Helpers;
using Warehouse.Theme.Conventers;
using System;
using System.Globalization;
using Warehouse.Models;
using Warehouse.Core.Models;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace Warehouse.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class BitmapToImageSourceConventer : BaseValueConventer<BitmapToImageSourceConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
		if (value is Bitmap bitmap)
			return ImageSourceFromBitmap(bitmap);

		return value;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

	[DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool DeleteObject([In] IntPtr hObject);

	public ImageSource ImageSourceFromBitmap(Bitmap bmp)
	{
		var handle = bmp.GetHbitmap();
		try
		{
			return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
		}
		finally { DeleteObject(handle); }
	}

}
