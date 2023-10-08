using magazyn_kuba_inz.Core.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Data;
using System;
using System.Collections.Generic;
using magazyn_kuba_inz.Conventers;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Windows.Shapes;

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

    private BaseObject _connectWidthPoint = null;

    private RackObject _rackToPoint = null;

    private WayPointObject _selectedPoint = null;

    private ObservableCollection<WayPointObject[]> _wayPointConnections = new ObservableCollection<WayPointObject[]>();

    private ObservableCollection<KeyValuePair<WayPointObject, RackObject>> _wayPointToRacks = new ObservableCollection<KeyValuePair<WayPointObject, RackObject>>();

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
    public ObservableCollection<WayPointObject[]> WayPointConnections
    {
        get => _wayPointConnections;
        private set => _wayPointConnections = value;
    }

    public ObservableCollection<KeyValuePair<WayPointObject, RackObject>> WayPointToRacks
    {
        get => _wayPointToRacks;
        private set => _wayPointToRacks = value;
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

    public bool WayGeneratorMode
    {
        get { return (bool)GetValue(WayGeneratorModeProperty); }
        set { SetValue(WayGeneratorModeProperty, value); }
    }

    public bool CrossVisible
    {
        get { return (bool)GetValue(CrossVisibleProperty); }
        set { SetValue(CrossVisibleProperty, value); }
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

    public static readonly DependencyProperty WayGeneratorModeProperty =
       DependencyProperty.Register(nameof(WayGeneratorMode), typeof(bool), typeof(WareHouseArea), new PropertyMetadata(false));

    public static readonly DependencyProperty CrossVisibleProperty =
       DependencyProperty.Register(nameof(CrossVisible), typeof(bool), typeof(WareHouseArea), new PropertyMetadata(false));

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
                    WayPointsPropertyChanged,
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

    private static void WayPointsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is WareHouseArea wha)
            wha.UpdateConnections();
    }

    #endregion

    #region EventMethods

    private void MenuItem_Initialized(object sender, EventArgs e)
    {
        if (sender is MenuItem mi)
            SetBinding(mi, MenuItem.IsEnabledProperty, this, nameof(WayGeneratorMode), new InverseBoolConventer());
    }

    private void Usun_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement fe)
        {
            if(fe.DataContext is WayPointObject wpo)
            {
                WayPoints.Remove(wpo);
                UpdateConnections();
            }else if(fe.DataContext is WayPointObject[] connections)
            {
                foreach(WayPointObject point in WayPoints)
                {
                    if (point.Connections.Contains(connections[0]))
                        point.Connections.Remove(connections[0]);

                    if (point.Connections.Contains(connections[1]))
                        point.Connections.Remove(connections[1]);
                }
                UpdateConnections();
            }else if(fe.DataContext is RackObject ro)
            {
                Racks.Remove(ro);
                UpdateConnections();
            }
        }

        
    }

    private void ConnectWidth_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement fe && (!WayGeneratorMode))
        {
            if ( fe.DataContext is BaseObject wpo)
            {
                _connectWidthPoint = wpo;
            }
        }
    }

    private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (WayGeneratorMode)
        {
            WayPointObject wayPoint = new WayPointObject(e.GetPosition(wareHouseArea));

            if (_selectedPoint  == null)
                _selectedPoint = wayPoint.FoundTheNearestPoint(WayPoints); 

            wayPoint.AddConnection(ref _selectedPoint);

            WayPoints?.Add(wayPoint);
            _selectedPoint = wayPoint;
            UpdateConnections();
        }
    }

    public void Object_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement obj)
        {
            if (_connectWidthPoint != null && obj?.DataContext is BaseObject toConnect)
            {
                if (toConnect is WayPointObject toconnectWPO)
                {
                    if (_connectWidthPoint is WayPointObject connectWPO)
                    {
                        connectWPO.AddConnection(ref toconnectWPO);
                    }

                    if (_connectWidthPoint is RackObject connectRO )
                    {
                        connectRO.AddConnection(ref toconnectWPO);
                    }
                }
                UpdateConnections();
                _connectWidthPoint = null;
            }

            if (_connectWidthPoint != null && obj?.DataContext is RackObject toROConnect)
            {
                
                if (_connectWidthPoint is WayPointObject connectWPO)
                {
                    connectWPO.AddConnection(ref toROConnect);
                }
                UpdateConnections();
                _connectWidthPoint = null;
            }

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
        Point currentPoint = e.GetPosition(wareHouseArea);

        if (CrossVisible)
        {
            // Aktualizacja pozycji linii poziomej
            HorizontalCrossLine.Y1 = currentPoint.Y;
            HorizontalCrossLine.Y2 = currentPoint.Y;

            // Aktualizacja pozycji linii pionowej
            VerticalCrossLine.X1 = currentPoint.X;
            VerticalCrossLine.X2 = currentPoint.X;

            CursorCrossText.Content = $"X:{currentPoint.X:F2} Y:{currentPoint.Y:F2}";
            Canvas.SetLeft(CursorCrossText, currentPoint.X);
            Canvas.SetTop(CursorCrossText, currentPoint.Y);
        }

        if (_isDragging && _movingElement != null)
        {
            double offsetX = currentPoint.X - _startPoint.X;
            double offsetY = currentPoint.Y - _startPoint.Y;

            BaseObject baseobj = _movingElement?.DataContext as BaseObject;

            if (baseobj == null) 
                return;

            double newX = baseobj.X + offsetX;
            double newY = baseobj.Y + offsetY;

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

    private void UpdateConnections()
    {
        if(WayPoints != null)
        {
            List<Action> delAct = new List<Action>();

            WayPointConnections = new ObservableCollection<WayPointObject[]>();
            WayPointToRacks = new ObservableCollection<KeyValuePair<WayPointObject, RackObject>>();
            foreach (WayPointObject point in WayPoints)
            {
                if (point.Connections != null)
                {
                    foreach(WayPointObject connection in point.Connections)
                    {
                        if (WayPoints.Contains(connection))
                            WayPointConnections.Add(new WayPointObject[2] { point, connection });
                        else
                        {
                            delAct.Add(() => { point.Connections.Remove(connection); });
                        }
                        
                    }
                }

                if (point.Racks != null)
                {
                    foreach (RackObject rack in point.Racks)
                    {
                        if (Racks.Contains(rack))
                        {
                            var keyValPar = new KeyValuePair<WayPointObject, RackObject>(point,rack);
                            WayPointToRacks.Add(keyValPar);
                        }  
                        else
                        {
                            delAct.Add(() => { point.Racks.Remove(rack); });
                        }

                    }
                }
            }


            foreach (var act in delAct)
                act.Invoke();

            wayPointConnections.ItemsSource = WayPointConnections;
            wayPointToRacks.ItemsSource = WayPointToRacks;
        }
    }

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

    

    private T SetBinding<T>(T element, DependencyProperty dp, object obj, string PropName, IValueConverter conv = null) where T : FrameworkElement
    {
        element.SetBinding(dp, new Binding(PropName)
        {
            Source = obj,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            Converter = conv
        });
        return element;
    }

    #endregion
}


