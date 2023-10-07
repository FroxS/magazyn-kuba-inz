using magazyn_kuba_inz.Core.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Data;
using magazyn_kuba_inz.Models.WareHouse.Object;

namespace magazyn_kuba_inz.Controls;

/// <summary>
/// Interaction logic for WareHouseArea.xaml
/// </summary>
public partial class WareHouseArea : UserControl
{
    #region Private Fields

    private bool _isDragging = false;

    private Point _startPoint;

    private FrameworkElement _movingElement = null;

    private WayPointObject _selectedPoint = null;

    private double zoomFactor = 1.2;

    #endregion

    #region Public Properties

    public double AreaWidth
    {
        get { return (double)GetValue(AreaWidthProperty); }
        set { SetValue(AreaWidthProperty, value); }
    }
    public double AreaHeight
    {
        get { return (double)GetValue(AreaHeightProperty); }
        set { SetValue(AreaHeightProperty, value); }
    }

    public ObservableCollection<RackObject> Racks
    {
        get { return (ObservableCollection<RackObject>)GetValue(RacksProperty); }
        set { SetValue(RacksProperty, value); }
    }

    public ObservableCollection<WayPointObject> WayPoints
    {
        get { return (ObservableCollection<WayPointObject>)GetValue(WayPointsProperty); }
        set { SetValue(WayPointsProperty, value); }
    }

    public BaseObject SelectedObject
    {
        get { return (BaseObject)GetValue(SelectedObjectProperty); }
        set { SetValue(SelectedObjectProperty, value); }
    }

    public double Zoom
    {
        get { return (double)GetValue(ZoomProperty); }
        set { SetValue(ZoomProperty, value); }
    }

    #endregion

    #region Dependency Property

    public static readonly DependencyProperty AreaWidthProperty =
        DependencyProperty.Register(
            nameof(AreaWidth), 
            typeof(double), 
            typeof(WareHouseArea),
            new UIPropertyMetadata(
                    1000d,
                    (obj, par) => { if (obj is WareHouseArea wha && par.NewValue is double val) wha.wareHouseArea.Width = val; },
                    null));

    public static readonly DependencyProperty AreaHeightProperty =
        DependencyProperty.Register(
            nameof(AreaHeight), 
            typeof(double), 
            typeof(WareHouseArea),
            new UIPropertyMetadata(
                    1000d,
                    (obj,par) => { if (obj is WareHouseArea wha && par.NewValue is double val) wha.wareHouseArea.Height = val; },
                    null)
            );

    public static readonly DependencyProperty SelectedObjectProperty =
        DependencyProperty.Register(nameof(SelectedObject), typeof(BaseObject), typeof(WareHouseArea), new PropertyMetadata(null));

    public static readonly DependencyProperty ZoomProperty =
       DependencyProperty.Register(nameof(Zoom), typeof(double), typeof(WareHouseArea), new PropertyMetadata(100d));


    public static readonly DependencyProperty RacksProperty =
        DependencyProperty.Register(
            nameof(Racks), 
            typeof(ObservableCollection<RackObject>), 
            typeof(WareHouseArea),
            new UIPropertyMetadata(
                    null,
                    null,//RacksPropertyChanged,
                    null)
            );

    public static readonly DependencyProperty WayPointsProperty =
        DependencyProperty.Register(
            nameof(WayPoints),
            typeof(ObservableCollection<WayPointObject>),
            typeof(WareHouseArea),
            new UIPropertyMetadata(
                    null,
                    null,//WayPointsPropertyChanged,
                    null)
            );

    #endregion

    #region Constructor

    public WareHouseArea()
    {
        InitializeComponent();
    }

    #endregion

    #region Dependecy Methods

    #endregion

    #region EventMethods

    private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (WayGeneratorMode.IsChecked ?? false)
        {
            WayPointObject wayPoint = new WayPointObject(e.GetPosition(wareHouseArea));
            WayPoints?.Add(wayPoint);
        }
    }


    public void Object_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement obj)
        {
            _isDragging = true;
            _movingElement = obj;
            _startPoint = e.GetPosition(wareHouseArea);
            SelectedObject = obj?.DataContext as BaseObject;
            if(obj?.DataContext is WayPointObject wpo)
            {
                _selectedPoint = wpo;
            }
        }
    }

    private void Canvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (_isDragging && _movingElement != null)
        {
            Point currentPoint = e.GetPosition(wareHouseArea);
            double offsetX = currentPoint.X - _startPoint.X;
            double offsetY = currentPoint.Y - _startPoint.Y;

            double newX = Canvas.GetLeft(_movingElement) + offsetX;
            double newY = Canvas.GetTop(_movingElement) + offsetY;

            Canvas parentCanvas = _movingElement.Parent as Canvas;

            Point relativePosition = _movingElement.TransformToVisual(wareHouseArea).Transform(new Point(0, 0));
            newX = relativePosition.X + offsetX;
            newY = relativePosition.Y + offsetY;
            //_movingElement.RenderTransform.
            if (MoveElement(new Point(newX, newY), _movingElement))
            {
                _startPoint = currentPoint;
            }
            else
            {
                _isDragging = false;
                _movingElement = null;
            }
        }
    }

    private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _isDragging = false;
        _movingElement = null;
    }

    private void Canvas_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers == ModifierKeys.Control)
        {
            if (e.Delta > 0)
            {
                // Zoom in
                wareHouseArea.LayoutTransform = new ScaleTransform(wareHouseArea.LayoutTransform.Value.M11 * zoomFactor, wareHouseArea.LayoutTransform.Value.M22 * zoomFactor);
            }
            else
            {
                // Zoom out
                wareHouseArea.LayoutTransform = new ScaleTransform(wareHouseArea.LayoutTransform.Value.M11 / zoomFactor, wareHouseArea.LayoutTransform.Value.M22 / zoomFactor);
            }
            Zoom = wareHouseArea.LayoutTransform.Value.M11 * 100;
            e.Handled = true;
        }
    }

    #endregion

    #region Elements methods

    private bool MoveElement(Point point, FrameworkElement element)
    {
        if ((point.X >= 0 && point.X <= wareHouseArea.ActualWidth - element.ActualWidth &&
            point.Y >= 0 && point.Y <= wareHouseArea.ActualHeight - element.ActualHeight) &&
            element.DataContext is BaseObject rack)
        {
            rack.X = point.X;
            rack.Y = point.Y;
            return true;
        }
        else
            return false;
    }

    #endregion

    #region Helpers

    private T SetBinding<T>(T element, DependencyProperty dp, object obj, string PropName) where T : FrameworkElement
    {
        element.SetBinding(dp, new Binding(PropName)
        {
            Source = obj,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        });
        return element;
    }

    #endregion

    
}


