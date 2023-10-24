using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using Warehouse.Models;
using System.Windows.Media;
using System;

namespace Warehouse.Controls;


/// <summary>
/// Interaction logic for WareHouseArea.xaml
/// </summary>
public partial class RackControl : UserControl
{
    #region Private Fields

    #endregion

    #region Public Properties

    public Rack Rack
    {
        get { return (Rack)GetValue(RackProperty); }
        set { SetValue(RackProperty, value); }
    }

    public StorageItemPackage SelectedPackage
    {
        get { return (StorageItemPackage)GetValue(SelectedPackageProperty); }
        set { SetValue(SelectedPackageProperty, value); }
    }

    public SolidColorBrush RackBrush
    {
        get { return (SolidColorBrush)GetValue(RackBrushProperty); }
        set { SetValue(RackBrushProperty, value); }
    }

    public SolidColorBrush BoxBrush
    {
        get { return (SolidColorBrush)GetValue(BoxBrushProperty); }
        set { SetValue(BoxBrushProperty, value); }
    }

    public ObservableCollection<Flor> Flors { get; set; }

    #endregion

    #region Dependency Property

    public static readonly DependencyProperty RackProperty =
        DependencyProperty.Register(nameof(Rack), typeof(Rack), typeof(RackControl), new PropertyMetadata(null, RackChanged,RackCoreChanged));

    public static readonly DependencyProperty RackBrushProperty =
       DependencyProperty.Register(nameof(RackBrush), typeof(SolidColorBrush), typeof(RackControl), new PropertyMetadata(Brushes.Orange));

    public static readonly DependencyProperty BoxBrushProperty =
       DependencyProperty.Register(nameof(BoxBrush), typeof(SolidColorBrush), typeof(RackControl), new PropertyMetadata(Brushes.SaddleBrown));

    public static readonly DependencyProperty SelectedPackageProperty =
       DependencyProperty.Register(nameof(SelectedPackage), typeof(StorageItemPackage), typeof(RackControl), new PropertyMetadata(null));


    #endregion

    #region Constructor

    public RackControl()
    {
        InitializeComponent();
    }

    #endregion

    #region Dependecy Methods

    private static void RackChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RackControl control && e.NewValue is Rack ro)
        {
            control.UpdateRack(ro);
        }
    }

    private static object RackCoreChanged(DependencyObject d, object baseValue)
    {
        if (d is RackControl control && baseValue is Rack ro)
        {
            control.UpdateRack(ro);
        }
        return baseValue;
    }


    #endregion

    #region EventMethods

    private void Box_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement obj && obj.DataContext is StorageItemPackage sip)
            SelectedPackage = sip;
    }

    #endregion

    #region Elements methods

    internal void UpdateRack(Rack ro)
    {
        var flors = new ObservableCollection<Flor>();

        if(ro != null)
        {
            for (int i = 0; i < ro.Flors; i++)
            {
                var flor = new Flor();
                flor.Items = new ObservableCollection<StorageItemPackage>();
                flors.Add(flor);
            }

            if (flors.Count > 0 && ro.StorageItems != null)
            {
                foreach (var item in ro.StorageItems)
                {
                    int itemFlor = item.Flor;

                    var flor = flors[0];
                    if (flors.Count > itemFlor)
                        flor = flors[itemFlor];

                    flor.Items.Add(item);
                }
            }
        }
        
        Flors = flors;
        rackItemsControl.ItemsSource = Flors;
    }

    #endregion

    #region Helpers

    private T SetBinding<T>(T element, DependencyProperty dp, object obj, string PropName, IValueConverter conv = null, object convParam = null) where T : FrameworkElement
    {
        element.SetBinding(dp, new Binding(PropName)
        {
            Source = obj,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            Converter = conv,
            ConverterParameter = convParam
        });
        return element;
    }

    #endregion

    public class Flor
    {
        public ObservableCollection<StorageItemPackage> Items { get; set; }
        public Flor() { }
    }

    
}


