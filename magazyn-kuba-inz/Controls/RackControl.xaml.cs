using Warehouse.Core.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Drawing;
using System;
using System.Collections.Specialized;

namespace Warehouse.Controls;


/// <summary>
/// Interaction logic for WareHouseArea.xaml
/// </summary>
public partial class RackControl : UserControl
{
    #region Private Fields

    #endregion

    #region Public Properties

    public RackObject Rack
    {
        get { return (RackObject)GetValue(RackProperty); }
        set { SetValue(RackProperty, value); }
    }

    public ObservableCollection<Flor> Flors { get; set; }

    #endregion

    #region Dependency Property

    public static readonly DependencyProperty RackProperty =
        DependencyProperty.Register(nameof(Rack), typeof(RackObject), typeof(RackControl), new PropertyMetadata(null, RackChanged,null));

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
        if (d is RackControl control && e.NewValue is RackObject ro)
        {
            var flors = new ObservableCollection<Flor>();
            for (int i = 0; i < ro.Flors; i++)
            {
                //double height = (control.rack.ActualHeight / (double)ro.Flors) * (double)i;
                var flor = new Flor();
                flor.Items = new ObservableCollection<Package>();
                flor.Items.Add(new Package());
                flor.Items.Add(new Package());
                flor.Items.Add(new Package());
                flor.Items.Add(new Package());
                flor.Items.Add(new Package());
                flor.Items.Add(new Package());
                flors.Add(flor);
            }

            control.Flors = flors;  

        }
    }


    #endregion

    #region EventMethods


    #endregion

    #region Elements methods

    private void UpdateRack()
    {

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

    public class Item
    {
        public Item() { }
    }

    public class Package
    {
        public ObservableCollection<Item> Items { get; set; }
        public Package() { }
    }

    public class Flor
    {
        public ObservableCollection<Package> Items { get; set; }
        public Flor() { }
    }

}


