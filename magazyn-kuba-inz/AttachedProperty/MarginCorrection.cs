using magazyn_kuba_inz.Theme.AttachedProperty;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace magazyn_kuba_inz.AttachedProperty;

/// <summary>
/// The MonitorPassword attached property for a <see cref="PasswordBox"/>
/// </summary>
public class MarginCorrection : BaseAttachedProperty<MarginCorrection, bool>
{
    public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var element = sender as FrameworkElement;

        if (element == null)
            return;
        //element.SizeChanged -= Element_SizeChanged;
        if ((bool)e.NewValue)
        {
            element.SizeChanged += Element_SizeChanged;
            UpdateMargin(element);
        }
    }

    private void Element_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (sender is FrameworkElement element)
            UpdateMargin(element);
    }

    private void UpdateMargin(FrameworkElement element)
    {
        element.Margin = new Thickness(
            -((element.Width.Equals(double.NaN) ? 0 : element.Width) / 2d),
            -((element.Height.Equals(double.NaN) ? 0 : element.Height) / 2d),
            element.Margin.Right,
            element.Margin.Bottom
        );
    }

}



