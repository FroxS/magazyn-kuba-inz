using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace Warehouse.Theme
{
    public enum ColorScheme
    {
        Dark,
        Light
    }

    public enum ColorType
    {
        WrongColor,
        GoodColor,
        WarningColor,
        PrimaryColor,
        FontPrimaryColor,
        FontColor,
        BackgroundColor,
        ButtonBackgroundColor,
        SecondBackgroundColor,
        BorderColor,
        HoverBackgroundColor,
    }

    public static class ColorsNavigation
    {
        static ColorScheme _actualColorScheme = ColorScheme.Dark;
        static Dictionary<ColorType, string> ActualColor;
        private static Dictionary<ColorType, string> DarkColor { get; }
        private static Dictionary<ColorType, string> LightColor { get; }

        static ColorsNavigation()
        {
            DarkColor = new Dictionary<ColorType, string>
            {
                { ColorType.WrongColor, "#ff0000" },
                { ColorType.GoodColor, "#44c71c" },
                { ColorType.WarningColor, "#d9a011" },
                { ColorType.PrimaryColor, "#49c904" },
                { ColorType.FontPrimaryColor, "#171715" },
                { ColorType.FontColor, "#ffffff" },
                { ColorType.BackgroundColor, "#1c1c1a" },
                { ColorType.ButtonBackgroundColor, "#292927" },
                { ColorType.SecondBackgroundColor, "#171716" },
                { ColorType.BorderColor, "#3A4149" },
                { ColorType.HoverBackgroundColor, "#4a4a46" }
            };
            LightColor = new Dictionary<ColorType, string>
            {
                { ColorType.WrongColor, "#ffcccc" },
                { ColorType.GoodColor, "#8aff5b" },
                { ColorType.WarningColor, "#ffd566" },
                { ColorType.PrimaryColor, "#49c904" },
                { ColorType.FontPrimaryColor, "#4d4d4c" },
                { ColorType.FontColor, "#000000" },
                { ColorType.BackgroundColor, "#ffffff" },
                { ColorType.ButtonBackgroundColor, "#f2f2f2" },
                { ColorType.SecondBackgroundColor, "#e6e6e6" },
                { ColorType.BorderColor, "#d8d8d8" },
                { ColorType.HoverBackgroundColor, "#e5e5e5" }
            };
            ActualColor = DarkColor;
        }

        public static Dictionary<ColorType, string>? GetColorScheme(ColorScheme scheme)
        {
            switch (scheme)
            {
                case ColorScheme.Dark:
                    return DarkColor;
                case ColorScheme.Light:
                    return LightColor;
                default:
                    return null;
            }
        }

        public static ColorScheme GetColorScheme() => _actualColorScheme;

        public static void ChangeColor(Application app, ColorScheme scheme)
        {
            Dictionary<ColorType, string>? colorsDict = GetColorScheme(scheme);
            if (colorsDict == null)
            {
                Debugger.Break();//"ColorsScheme not found"
                return;
            }

            foreach (KeyValuePair<ColorType, string> colorDict in colorsDict)
            {
                try
                {
                    Color newColor = GetColor(colorDict.Value);
                    SolidColorBrush newColorBrush = new SolidColorBrush(newColor);
                    app.Resources[colorDict.Key.ToString()] = newColor;
                    app.Resources[colorDict.Key + "Brush"] = newColorBrush;
                    _actualColorScheme = scheme;
                }
                catch (Exception ex)
                {
                    
                }

            }
            ActualColor = colorsDict;
        }

        public static SolidColorBrush GetColorBrush(ColorType type) => new SolidColorBrush(GetColor(ActualColor[type]));

        private static Color GetColor(string hex) => (Color)ColorConverter.ConvertFromString(hex);
    }
}