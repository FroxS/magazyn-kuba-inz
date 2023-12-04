using Warehouse.Core.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Data;
using System;
using System.Collections.Generic;
using Warehouse.Conventers;
using System.Collections.Specialized;
using System.Windows.Shapes;
using System.Linq;
using Warehouse.Core.Interface;
using Warehouse.Core.Helpers;
using Warehouse.Models;
using Warehouse.AttachedProperty;
using System.Windows.Controls.Primitives;
using Warehouse.Theme;

namespace Warehouse.Controls;

public enum ECreatorMode
{
    None,
    WayGeneratorMode,
    RackCreateMode,
}

/// <summary>
/// Interaction logic for WareHouseArea.xaml
/// </summary>
public partial class WareHouseArea : UserControl
{
    #region Private Fields

    private bool _isDragging = false;

    private Point _startPoint;

    private Line _creatingPointLine = new Line();

    private Point mousePosition;

    private FrameworkElement _movingElement = null;

    private IBaseObject _connectWidthPoint = null;

    private RackObject _rackToPoint = null;

    private WayPointObject _selectedPoint = null;

    private FrameworkElement _selectedElement = null;

    private ObservableCollection<WayPointObject[]> _wayPointConnections = new ObservableCollection<WayPointObject[]>();

    private ObservableCollection<KeyValuePair<WayPointObject, RackObject>> _wayPointToRacks = new ObservableCollection<KeyValuePair<WayPointObject, RackObject>>();

    private Path wayPath;

    private List<FrameworkElement> wayElements = new List<FrameworkElement>();

    #endregion

    #region Public Properties

    public double AreaWidth
    {
        get { return (double)GetValue(AreaWidthProperty); }
        set { SetValue(AreaWidthProperty, value); }
    }

    public double ZoomFactor
    {
        get { return (double)GetValue(ZoomFactorProperty); }
        set { SetValue(ZoomFactorProperty, value); }
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

    public IBaseObject SelectedObject
    {
        get { return (IBaseObject)GetValue(SelectedObjectProperty); }
        set { SetValue(SelectedObjectProperty, value); }
    }

    public WayPointObject StartObject
    {
        get { return (WayPointObject)GetValue(StartObjectProperty); }
        set { SetValue(StartObjectProperty, value); }
    }

    public double Zoom
    {
        get { return (double)GetValue(ZoomProperty); }
        set { SetValue(ZoomProperty, value); }
    }

    public ECreatorMode Mode
    {
        get { return (ECreatorMode)GetValue(ModeProperty); }
        set { SetValue(ModeProperty, value); }
    }

    public bool CrossVisible
    {
        get { return (bool)GetValue(CrossVisibleProperty); }
        set { SetValue(CrossVisibleProperty, value); }
    }

    public SolidColorBrush PointBrush
    {
        get { return (SolidColorBrush)GetValue(PointBrushProperty); }
        set { SetValue(PointBrushProperty, value); }
    }

    public SolidColorBrush StartPointBrush
    {
        get { return (SolidColorBrush)GetValue(StartPointBrushProperty); }
        set { SetValue(StartPointBrushProperty, value); }
    }

    public SolidColorBrush LineBrush
    {
        get { return (SolidColorBrush)GetValue(LineBrushProperty); }
        set { SetValue(LineBrushProperty, value); }
    }

    public SolidColorBrush LineToRackBrush
    {
        get { return (SolidColorBrush)GetValue(LineToRackBrushProperty); }
        set { SetValue(LineToRackBrushProperty, value); }
    }

    public SolidColorBrush HallBackground
    {
        get { return (SolidColorBrush)GetValue(HallBackgroundProperty); }
        set { SetValue(HallBackgroundProperty, value); }
    }

    public double LineStroke
    {
        get { return (double)GetValue(LineStrokeProperty); }
        set { SetValue(LineStrokeProperty, value); }
    }

    public double PointDiameter
    {
        get { return (double)GetValue(PointDiameterProperty); }
        set { SetValue(PointDiameterProperty, value); }
    }

    public double KeyStep
    {
        get { return (double)GetValue(KeyStepProperty); }
        set { SetValue(KeyStepProperty, value); }
    }

    public Func<RackObject, bool> CanDeleteRack
    {
        get { return (Func<RackObject, bool>)GetValue(CanDeleteRackProperty); }
        set { SetValue(CanDeleteRackProperty, value); }
    }

    public bool CanEdit
    {
        get { return (bool)GetValue(CanEditProperty); }
        set { SetValue(CanEditProperty, value); }
    }

    public WayResult Way
    {
        get { return (WayResult)GetValue(WayProperty); }
        set { SetValue(WayProperty, value); }
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
        DependencyProperty.Register(nameof(SelectedObject), typeof(IBaseObject), typeof(WareHouseArea), new PropertyMetadata(null));

    public static readonly DependencyProperty ZoomFactorProperty =
       DependencyProperty.Register(nameof(ZoomFactor), typeof(double), typeof(WareHouseArea), new PropertyMetadata(1.2d));

    public static readonly DependencyProperty ZoomProperty =
       DependencyProperty.Register(nameof(Zoom), typeof(double), typeof(WareHouseArea), new PropertyMetadata(100d));

    public static readonly DependencyProperty ModeProperty =
       DependencyProperty.Register(nameof(Mode), typeof(ECreatorMode), typeof(WareHouseArea), new PropertyMetadata(ECreatorMode.None));

    public static readonly DependencyProperty CrossVisibleProperty =
       DependencyProperty.Register(nameof(CrossVisible), typeof(bool), typeof(WareHouseArea), new PropertyMetadata(false));

    public static readonly DependencyProperty PointBrushProperty =
       DependencyProperty.Register(nameof(PointBrush), typeof(SolidColorBrush), typeof(WareHouseArea), new PropertyMetadata(Brushes.Black));

    public static readonly DependencyProperty StartPointBrushProperty =
       DependencyProperty.Register(nameof(StartPointBrush), typeof(SolidColorBrush), typeof(WareHouseArea), new PropertyMetadata(Brushes.Green));

    public static readonly DependencyProperty LineBrushProperty =
       DependencyProperty.Register(nameof(LineBrush), typeof(SolidColorBrush), typeof(WareHouseArea), new PropertyMetadata(Brushes.Black));

    public static readonly DependencyProperty LineToRackBrushProperty =
       DependencyProperty.Register(nameof(LineToRackBrush), typeof(SolidColorBrush), typeof(WareHouseArea), new PropertyMetadata(Brushes.Green));

    public static readonly DependencyProperty HallBackgroundProperty =
       DependencyProperty.Register(nameof(HallBackground), typeof(SolidColorBrush), typeof(WareHouseArea), new PropertyMetadata(Brushes.White));

    public static readonly DependencyProperty LineStrokeProperty =
      DependencyProperty.Register(nameof(LineStroke), typeof(double), typeof(WareHouseArea), new PropertyMetadata(2.0d));

    public static readonly DependencyProperty PointDiameterProperty =
      DependencyProperty.Register(nameof(PointDiameter), typeof(double), typeof(WareHouseArea), new PropertyMetadata(10.0d));

    public static readonly DependencyProperty KeyStepProperty =
      DependencyProperty.Register(nameof(KeyStep), typeof(double), typeof(WareHouseArea), new PropertyMetadata(10.0d));

    public static readonly DependencyProperty StartObjectProperty = 
        DependencyProperty.Register(nameof(StartObject), typeof(WayPointObject), typeof(WareHouseArea), new PropertyMetadata(null));

    public static readonly DependencyProperty CanDeleteRackProperty =
        DependencyProperty.Register(nameof(CanDeleteRack), typeof(Func<RackObject, bool>), typeof(WareHouseArea), new PropertyMetadata(null));

    public static readonly DependencyProperty WayProperty =
        DependencyProperty.Register(nameof(Way), typeof(WayResult), typeof(WareHouseArea), new PropertyMetadata(null, WayChanged));

    public static readonly DependencyProperty CanEditProperty =
        DependencyProperty.Register(nameof(CanEdit), typeof(bool), typeof(WareHouseArea), new UIPropertyMetadata(true,null,CanEditChanged));

    public static readonly DependencyProperty RacksProperty =
        DependencyProperty.Register(
            nameof(Racks), 
            typeof(ObservableCollection<RackObject>), 
            typeof(WareHouseArea),
            new UIPropertyMetadata(
                    null,
                    WayPointsPropertyChanged,//RacksPropertyChanged,
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
        {
            if (wha.Racks == null)
                return;

            wha.UpdateConnections();

            if (e.NewValue is INotifyCollectionChanged coll)
                coll.CollectionChanged += (obj, args) => { wha.UpdateConnections(); };
        }
    }

    private static object CanEditChanged(DependencyObject d, object baseValue)
    {
        if(d is WareHouseArea wha && baseValue is bool flag)
        {
            string[] ContextMenuNames = { "RackContextMenu", "LineContextMenu", "WayPointsContextMenu", "LinesToRacksContextMenu", "CanvasContextMenu" };
            foreach(string ContextMenuName in ContextMenuNames)
                if (wha.TryFindResource(ContextMenuName) is ContextMenu cm)
                    cm.Visibility = flag ? Visibility.Visible : Visibility.Collapsed;
        }

        return baseValue;
    }

    private static void WayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is WareHouseArea wha && e.NewValue is WayResult way)
        {
            if(wha.Racks != null)/// W tej metodzie są wyszukiwane stojaki 
                wha.DrawWay(); 
        }
    }

    #endregion

    #region EventMethods

    private void ModeBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton tb && tb.Tag is ECreatorMode cm)
        {
            if(Mode == cm)
            {
                Mode = ECreatorMode.None;
                tb.IsChecked = false;
            }
            else
            {
                Mode = cm;
            }
        }
    }

    private void MenuItem_Initialized(object sender, EventArgs e)
    {
        if (sender is MenuItem mi)
        {
            SetBinding(mi, MenuItem.IsEnabledProperty, this, nameof(Mode), new IsNotEqualToParamConventer(), convParam: ECreatorMode.WayGeneratorMode);
        }
    }
    
    private void SetAsStart_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement fe && fe.DataContext is WayPointObject wpo)
        { 
            foreach(var point in WayPoints)
            {
                point.IsStartPoint = false;

            }
            wpo.IsStartPoint = true;
            UpdateConnections();
        }
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
                if(CanDeleteRack == null || CanDeleteRack.Invoke(ro))
                {
                    Racks.Remove(ro);
                    UpdateConnections();
                }
            }else if (fe.DataContext is KeyValuePair<WayPointObject, RackObject> par)
            {
                par.Value.WayPoints.Remove(par.Key);
                par.Key.Racks.Remove(par.Value);
                UpdateConnections();
            }
        }
        e.Handled = true;

    }

    private void ConnectWidth_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement fe && (Mode != ECreatorMode.WayGeneratorMode))
        {
            if ( fe.DataContext is IBaseObject wpo)
            {
                _connectWidthPoint = wpo;
            }
        }
        e.Handled = true;
    }

    private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (Mode == ECreatorMode.WayGeneratorMode)
        {
            AddPoint(e.GetPosition(wareHouseArea));
        }

        if (Mode == ECreatorMode.RackCreateMode)
        {
            AddRack(e.GetPosition(wareHouseArea));
        }
        e.Handled = true;
    }

    public void Object_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
       
        if (sender is FrameworkElement obj)
        {
            SelectedObject = obj?.DataContext as IBaseObject;

            if (_connectWidthPoint != null && obj?.DataContext is IBaseObject toConnect)
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

                if (toConnect is RackObject toROConnect)
                {
                    if (_connectWidthPoint is WayPointObject connectWPO)
                    {
                        connectWPO.AddConnection(ref toROConnect);
                    }
                }

                UpdateConnections();
                _connectWidthPoint = null;
            }
            _isDragging = true;
            _movingElement = obj;
            _startPoint = e.GetPosition(wareHouseArea);
            _selectedElement = obj;
            if (obj?.DataContext is WayPointObject wpo)
            {
                _selectedPoint = wpo;
            }
        }
        e.Handled = true;
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

            IBaseObject baseobj = _movingElement?.DataContext as IBaseObject;

            if (baseobj == null)
            {
                e.Handled = true;
                return;
            }
                

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

        if(!_isDragging && _connectWidthPoint != null)
        {
            AddTmpLine(currentPoint, _connectWidthPoint.Position);
        }
        else
        {
             wareHouseArea.Children.Remove(_creatingPointLine);
        }

        e.Handled = true;
    }

    private void Object_KeyDown(object sender, KeyEventArgs e)
    {
        if (_selectedElement != null && _selectedElement.DataContext is IBaseObject bo)
        {
            Point pozition = bo.Position;
            switch (e.Key)
            {
                case Key.Left:
                    pozition.X -= KeyStep;
                    MoveElement(pozition, _selectedElement);
                    break;
                case Key.Right:
                    pozition.X += KeyStep;
                    MoveElement(pozition, _selectedElement);
                    break;
                case Key.Up:
                    pozition.Y -= KeyStep;
                    MoveElement(pozition, _selectedElement);
                    break;
                case Key.Down:
                    pozition.Y += KeyStep;
                    MoveElement(pozition, _selectedElement);
                    break;
            }
        }
    }

    private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _isDragging = false;
        _movingElement = null;
        e.Handled = true;
    }

    private void Canvas_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {

        if (Keyboard.Modifiers == ModifierKeys.Control)
        {
            if (e.Delta > 0)
            {
                // Zoom in
                wareHouseArea.LayoutTransform = new ScaleTransform(wareHouseArea.LayoutTransform.Value.M11 * ZoomFactor, wareHouseArea.LayoutTransform.Value.M22 * ZoomFactor);
            }
            else
            {
                // Zoom out
                wareHouseArea.LayoutTransform = new ScaleTransform(wareHouseArea.LayoutTransform.Value.M11 / ZoomFactor, wareHouseArea.LayoutTransform.Value.M22 / ZoomFactor);
            }
            Zoom = wareHouseArea.LayoutTransform.Value.M11 * 100;
            e.Handled = true;
        }
    }

    private void Add_Point_Click(object sender, RoutedEventArgs e)
    {
        AddPoint(mousePosition);
    }

    private void Add_Rack_Click(object sender, RoutedEventArgs e)
    {
        AddRack(mousePosition);
    }

    private void GetPozitionEvent(object sender, RoutedEventArgs e)
    {
        mousePosition = Mouse.GetPosition(wareHouseArea);
    }

    #endregion

    #region Elements methods

    private void AddRack(Point point)
    {
        RackObject rack = new RackObject(Guid.NewGuid(), point);
        Racks?.Add(rack);
        SelectedObject = rack;
        UpdateConnections();
    }

    private void AddPoint(Point point, bool connect = true)
    {
        WayPointObject wayPoint = new WayPointObject(point);

        if (connect)
        {
            if (_selectedPoint == null)
                _selectedPoint = wayPoint.FoundTheNearestPoint(WayPoints);

            if (_selectedPoint != null)
                wayPoint.AddConnection(ref _selectedPoint);
        }

        WayPoints?.Add(wayPoint);
        _selectedPoint = wayPoint;
        UpdateConnections();
    }

    private void AddTmpLine(Point start, Point end)
    {
        _creatingPointLine.X1 = start.X;
        _creatingPointLine.Y1 = start.Y;
        _creatingPointLine.X2 = end.X;
        _creatingPointLine.Y2 = end.Y;
        _creatingPointLine.Stroke = LineBrush;
        _creatingPointLine.StrokeThickness = LineStroke;
        _creatingPointLine.Opacity = .5d;
        Panel.SetZIndex(_creatingPointLine, -1);
        if (!wareHouseArea.Children.Contains(_creatingPointLine))
            wareHouseArea.Children.Add(_creatingPointLine);
    }

    private void UpdateConnections()
    {
        if (WayPoints != null)
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
  
            wayPointConnections.ItemsSource = GetNotDuplicatedPoints(WayPointConnections);
            wayPointToRacks.ItemsSource = WayPointToRacks;
            wayPointNode.UpdateDefaultStyle();
            wayPointNode.UpdateLayout();
            DrawWay();
        }
    }

    private bool MoveElement(Point point, FrameworkElement element)
    {
        if (!CanEdit)
            return false;
        if ((point.X >= 0 && point.X <= wareHouseArea.ActualWidth - element.ActualWidth &&
            point.Y >= 0 && point.Y <= wareHouseArea.ActualHeight - element.ActualHeight) &&
            element.DataContext is IBaseObject rack)
        {
            rack.X = point.X;
            rack.Y = point.Y;
            return true;
        }
        else
            return false;
    }

    private List<WayPointObject[]> GetNotDuplicatedPoints(IEnumerable<WayPointObject[]> connections)
    {
        List<WayPointObject[]> newlist = new List<WayPointObject[]>();

        foreach(WayPointObject[] connection in connections)
        {
            if (!newlist.Any(
                x => (x[0].Position == connection[0].Position && x[1].Position == connection[1].Position)
                    || (x[0].Position == connection[1].Position && x[1].Position == connection[0].Position)
                ))
                newlist.Add(connection);
        }

        return newlist;
    }

    
    private void DrawWay()
    {
        if(Way != null)
        {
            List<WayObject> items = Way.GetPath();

            foreach (var step in wayElements)
                wareHouseArea.Children.Remove(step);

            wayElements.Clear();
            int wayStep = 0;
            wayPath = new Path() { Stroke = Brushes.Red, StrokeThickness = 1};
            GeometryGroup data = new GeometryGroup() { Children = new GeometryCollection() };
            PathGeometry pathGeometry = new PathGeometry() { Figures = new PathFigureCollection() };

            WayObject? lastRack = null;
            FrameworkElement? rackControl = null;
            List<Product>? products = null;
            for (int i = 1; i < items.Count; i++)
            {
                WayObject p1 = items[i - 1];
                WayObject p2 = items[i];

                if(p2.Type == EWayElementType.Rack)
                {
                    lastRack = p2;
                }
                if (p2.Type == EWayElementType.Point && p1.Type == EWayElementType.Product)
                {
                    p1 = lastRack;
                    lastRack = null;
                }
                if (p1.Position != null && p2.Position != null)
                {
                    PathFigure line = new PathFigure() {
                        StartPoint = p1.Position.Value,
                        Segments = new PathSegmentCollection
                        {
                            new BezierSegment(p1.Position.Value, GetCenterPoint(p1.Position.Value, p2.Position.Value,10), p2.Position.Value, true),
                        }
                    };
                    var wheelPoz = GetCenterPoint(p1.Position.Value, p2.Position.Value, 15);
                    var stepName = GetViewBox($"{++wayStep}");
                    Canvas.SetTop(stepName, wheelPoz.Y - 5);
                    Canvas.SetLeft(stepName, wheelPoz.X - 5);
                    wayElements.Add(stepName);
                    pathGeometry.Figures.Add(line);
                }

                if (items[i].Type == EWayElementType.Product && lastRack != null)
                {
                    if (products == null)
                        products = new List<Product>();

                    products.Add(new Product() { ID = items[i].Itemid.Value, Name = items[i].Name});
                    if(!(items[i+1] is Product))
                    {
                        rackControl = GetRack(Racks.FirstOrDefault(x => x.ID == lastRack.Itemid), products);
                        Canvas.SetTop(rackControl, lastRack.Position.Value.Y);
                        Canvas.SetLeft(rackControl, lastRack.Position.Value.X);

                        wayElements.Add(rackControl);
                        products = null;
                    }
                }


            }
            data.Children.Add(pathGeometry);
            wayPath.Data = data;

            foreach (var step in wayElements)
                wareHouseArea.Children.Add(step);

            if (!wareHouseArea.Children.Contains(wayPath))
                wareHouseArea.Children.Add(wayPath);

            
        }
    }

    //private void DrawWayOLD()
    //{
    //    if (Way != null)
    //    {
    //        List<WayObject> items = Way.GetPath();

    //        foreach (var step in wayElements)
    //            wareHouseArea.Children.Remove(step);

    //        wayElements.Clear();
    //        int wayStep = 0;
    //        wayPath = new Path() { Stroke = Brushes.Red, StrokeThickness = 1 };
    //        GeometryGroup data = new GeometryGroup() { Children = new GeometryCollection() };
    //        PathGeometry pathGeometry = new PathGeometry() { Figures = new PathFigureCollection() };

    //        RackObject? lastRack = null;
    //        FrameworkElement? rackControl = null;
    //        List<Product> products = null;
    //        for (int i = 1; i < items.Count; i++)
    //        {
    //            IBaseObject? p1 = items[i - 1] as IBaseObject;
    //            IBaseObject? p2 = items[i] as IBaseObject;

    //            if (p2 is RackObject rack)
    //            {
    //                lastRack = rack;
    //            }
    //            if (p2 is WayPointObject && p1 == null)
    //            {
    //                p1 = lastRack;
    //                lastRack = null;
    //            }
    //            if (p1 != null && p2 != null)
    //            {
    //                PathFigure line = new PathFigure()
    //                {
    //                    StartPoint = p1.Position,
    //                    Segments = new PathSegmentCollection
    //                    {
    //                        new BezierSegment(p1.Position, GetCenterPoint(p1.Position,p2.Position,10), p2.Position, true),
    //                    }
    //                };
    //                var wheelPoz = GetCenterPoint(p1.Position, p2.Position, 15);
    //                var stepName = GetViewBox($"{++wayStep}");
    //                Canvas.SetTop(stepName, wheelPoz.Y - 5);
    //                Canvas.SetLeft(stepName, wheelPoz.X - 5);
    //                wayElements.Add(stepName);
    //                pathGeometry.Figures.Add(line);
    //            }

    //            if (items[i] is Product product && lastRack != null)
    //            {
    //                if (products == null)
    //                    products = new List<Product>();

    //                products.Add(product);
    //                if (!(items[i + 1] is Product))
    //                {
    //                    rackControl = GetRack(lastRack, products);
    //                    Canvas.SetTop(rackControl, lastRack.Y);
    //                    Canvas.SetLeft(rackControl, lastRack.X);

    //                    wayElements.Add(rackControl);
    //                    products = null;
    //                }
    //            }


    //        }
    //        data.Children.Add(pathGeometry);
    //        wayPath.Data = data;

    //        foreach (var step in wayElements)
    //            wareHouseArea.Children.Add(step);

    //        if (!wareHouseArea.Children.Contains(wayPath))
    //            wareHouseArea.Children.Add(wayPath);


    //    }
    //}

    #endregion

    #region Helpers

    private Point GetCenterPoint(Point p1, Point p2, double move = 0)
    {
        Point vector = new Point(p2.X - p1.X, p2.Y - p1.Y);
        double vectorLength = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        vector.X /= vectorLength;
        vector.Y /= vectorLength;
        return new Point(p1.X + vector.X * (vectorLength / 2) - move * vector.Y,
                             p1.Y + vector.Y * (vectorLength / 2) + move * vector.X);
    }

    private Viewbox GetViewBox (string text)
    {
        return new Viewbox() { Child = new TextBlock() { Text = text, Foreground = this.Foreground } };
    }

    private FrameworkElement GetRack(RackObject rack, List<Product> items = null)
    {
        Grid grid = new Grid();
        Rectangle rectangle = new Rectangle();
        rectangle = SetBinding(rectangle, Rectangle.FillProperty, rack, nameof(rack.Color));
        rectangle = SetBinding(rectangle, Rectangle.WidthProperty, rack, nameof(rack.Width));
        rectangle = SetBinding(rectangle, Rectangle.HeightProperty, rack, nameof(rack.Heigth));
        rectangle.SetValue(MarginCorrection.ValueProperty, true);
        grid.Children.Add(rectangle);

        if(items != null)
        {
            // Tworzenie Popup
            Popup popup = new Popup();
            popup.Placement = PlacementMode.Mouse; // Popup będzie położony względem pozycji myszki
            popup.AllowsTransparency = true;

            // Tworzenie zawartości Popup, na przykład, możesz dodać TextBlock z tekstem
            StackPanel sp = new StackPanel()
            {
                Background = ColorsNavigation.GetColorBrush(ColorType.ButtonBackgroundColor),
                Orientation = Orientation.Vertical
            };
            foreach(Product prod in items)
            {
                TextBlock popupContent = new TextBlock() {
                    Margin = new Thickness(5)
                };
                popupContent.Text = $"{prod.Name}";
                sp.Children.Add(popupContent);
            }
            

            // Dodanie zawartości do Popup
            popup.Child = sp;

            // Dodajemy obsługę wydarzenia MouseEnter na Grid, aby wyświetlić Popup
            grid.MouseEnter += (sender, e) =>
            {
                if (!popup.IsOpen)
                {
                    popup.IsOpen = true;
                }
            };

            // Dodajemy obsługę wydarzenia MouseLeave na Grid, aby schować Popup
            grid.MouseLeave += (sender, e) =>
            {
                if (popup.IsOpen)
                {
                    popup.IsOpen = false;
                }
            };
        }

        return grid;
    }

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

}