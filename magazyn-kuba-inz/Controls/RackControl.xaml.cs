using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using Warehouse.Models;
using System.Windows.Media;
using System.Linq;

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

    public ObservableCollection<SFlor> Flors { get; set; }

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
        if (sender is FrameworkElement obj && obj.DataContext is SPackage sip)
            SelectedPackage = sip.Package;
    }

    #endregion

    #region Elements methods

    internal void UpdateRack(Rack ro)
    {
        ObservableCollection<SFlor> flors = new ObservableCollection<SFlor>();
        if(ro != null)
        {
            for (int i = 0; i < ro.Flors; i++)
            {
                SFlor flor = new SFlor();
                flor.Items = new ObservableCollection<SPackage>();
                flors.Add(flor);
            }

            if (flors.Count > 0 && ro.StorageItems != null)
            {
                foreach (StorageItemPackage item in ro.StorageItems)
                {
                    int itemFlor = flors.Count - 1 - item.Flor;//  Math.Abs(item.Flor - flors.Count -1);

                    SFlor flor = flors[flors.Count-1];
                    if (flors.Count > itemFlor)
                        flor = flors[itemFlor];

                    flor.Items.Add(new SPackage(item));
                }
            }

            foreach(SFlor flor in flors)
            {
                flor.UpdateColumns();
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

    public class SFlor : ObservableObject
    {
        private ObservableCollection<SPackage> _items;
        private int _column = 1;

        public ObservableCollection<SPackage> Items 
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public int Column
        {
            get => _column;
            set => SetProperty(ref _column, value);
        }

        public SFlor() { }

        public void UpdateColumns()
        {
            double percent = Items.FirstOrDefault()?.Package?.StorageUnit?.SizeOfRack ?? 1;
            int cols = (int)(1.0 / percent);
            if (cols < 1)
            {
                cols = 1;
            }
            Column = cols;
        }
    }

    public class SPackage : ObservableObject
    {
        private bool _isSelected = false;
        private StorageItemPackage _package;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public StorageItemPackage Package
        {
            get => _package;
            private set => SetProperty(ref _package, value);
        }

        public SPackage(StorageItemPackage pack) 
        {
            Package = pack;
        }  
    }

    public class SItem : ObservableObject
    {
        private StorageItem _item;
        public StorageItem Item
        {
            get => _item;
            private set => SetProperty(ref _item, value);
        }

        public SItem(StorageItem item)
        {
            Item = item;
        }
    }
}


