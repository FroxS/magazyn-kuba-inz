using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Warehouse.Theme;

namespace Warehouse.Helper;

public static class ImageHelper
{
    public static BitmapImage GetImage(this byte[] imageData)
    {
        if (imageData == null || imageData.Length == 0) return null;
        var image = new BitmapImage();
        using (var mem = new MemoryStream(imageData))
        {
            mem.Position = 0;
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = null;
            image.StreamSource = mem;
            image.EndInit();
        }
        image.Freeze();
        return image;
    }

    public static ImageSource GetImage(this Geometry gm) => new DrawingImage(new GeometryDrawing(Brushes.Transparent, new Pen(ColorsNavigation.GetColorBrush(ColorType.FontColor), 1), gm));

}

