using System.Windows.Media;

namespace magazyn_kuba_inz.Core.Helpers;

public static class Conventer
{

    public static Color GetContrastColor(this Color inputColor)
    {
        // Obliczamy średnią jasność koloru (luminancję)
        double luminance = (0.299 * inputColor.R + 0.587 * inputColor.G + 0.114 * inputColor.B) / 255;

        // Wybieramy kolor kontrastowy w zależności od luminancji
        return luminance > 0.5 ? Color.FromRgb(0,0,0) : Color.FromRgb(255, 255, 255);
    }

    public static Brush GetContrastBrush(this Brush inputBrush)
    {
        SolidColorBrush solidColorBrush = inputBrush as SolidColorBrush;

        if (solidColorBrush != null)
        {
            // Pobieramy kolor z pędzla SolidColorBrush
            Color inputColor = solidColorBrush.Color;

            // Obliczamy średnią jasność koloru (luminancję)
            double luminance = (0.299 * inputColor.R + 0.587 * inputColor.G + 0.114 * inputColor.B) / 255;

            // Wybieramy kolor kontrastowy w zależności od luminancji
            return luminance > 0.5 ? Brushes.Black : Brushes.White;
        }
        else
        {
            // Jeśli pędzel nie jest SolidColorBrush, zwracamy pusty pędzel
            return new SolidColorBrush(Colors.Transparent);
        }
    }

}