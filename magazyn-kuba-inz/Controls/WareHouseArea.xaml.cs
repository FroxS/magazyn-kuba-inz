using magazyn_kuba_inz.Core.Models;
using magazyn_kuba_inz.Core.Helpers;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

    public RackObject SelectedRacks
    {
        get { return (RackObject)GetValue(SelectedRacksProperty); }
        set { SetValue(SelectedRacksProperty, value); }
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

    public static readonly DependencyProperty SelectedRacksProperty =
        DependencyProperty.Register(nameof(SelectedRacks), typeof(RackObject), typeof(WareHouseArea), new PropertyMetadata(null));

    public static readonly DependencyProperty RacksProperty =
        DependencyProperty.Register(
            nameof(Racks), 
            typeof(ObservableCollection<RackObject>), 
            typeof(WareHouseArea),
            new UIPropertyMetadata(
                    null,
                    RacksPropertyChanged,
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

    private static void RacksPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if(d is WareHouseArea control && e.NewValue is ObservableCollection<RackObject> items)
        {
            control.wareHouseArea.Children.Clear();
            
            foreach(RackObject item in items)
            {
                control.AddRack(item);
            }
        }
    }

    #endregion

    private FrameworkElement GetRectangle(RackObject rack)
    {
        Grid control = new Grid() 
        {
            Background = rack?.Color ?? Brushes.Blue,
            DataContext = rack
        };
        control.Children.Add(new Rectangle
        {
            //Width = CalcWidth(rack.Width),
            //Height = CalcHeight(rack.Height),
            Width = rack.Width,
            Height = rack.Height,
            Fill = rack?.Color ?? Brushes.Blue
        });

        control.Children.Add(new Label
        {
            Content = $"{rack.Width}x{rack.Height}", 
            Foreground = rack.Color.GetContrastBrush(),
            VerticalContentAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center
        }); 

        return control;
    }
    

    private void AddRack(RackObject rack)
    {
        FrameworkElement rectangle = GetRectangle(rack);
        //Point newPoz = CalcPoint(rack.Position);
        Point newPoz = rack.Position;
        Canvas.SetLeft(rectangle, newPoz.X);
        Canvas.SetTop(rectangle, newPoz.Y);
        rectangle.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;
        wareHouseArea.Children.Add(rectangle);
    }

    private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement rectangle)
        {
            _isDragging = true;
            _movingElement = rectangle;
            _startPoint = e.GetPosition(wareHouseArea);
            SelectedRacks = rectangle?.DataContext as RackObject;
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

            if (newX >= 0 && newX <= wareHouseArea.ActualWidth - _movingElement.ActualWidth &&
            newY >= 0 && newY <= wareHouseArea.ActualHeight - _movingElement.ActualHeight)
            {
                Canvas.SetLeft(_movingElement, newX);
                Canvas.SetTop(_movingElement, newY);
                if (_movingElement.DataContext is RackObject rack)
                    rack.Position = new Point(newX, newY);
                //rack.Position = CalcBackPoint(new Point(newX, newY));
                _startPoint = currentPoint;
            } 
        }
    }

    private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _isDragging = false;
        _movingElement = null;
    }

    //private Point CalcPoint(Point point)
    //{
    //    double x = point.X;
    //    double y = point.Y;
    //    x = CalcWidth(x);
    //    y = CalcHeight(y);
    //    return new Point(x,y); ;
    //}

    //private Point CalcBackPoint(Point point)
    //{
    //    double x = point.X;
    //    double y = point.Y;
    //    x = CalcBackWidth(x);
    //    y = CalcBackHeight(y);
    //    return new Point(x, y); ;
    //}

    private void AddMessage(string message)
    {
        string prev = Message.Text;
        Message.Text = $"{message};\n{prev}";
    }

    //private double CalcWidth(double width) => wareHouseArea.ActualWidth > 0 ? (width * AreaWidth / wareHouseArea.ActualWidth) : width;

    //private double CalcBackWidth(double width) => wareHouseArea.ActualWidth > 0 ? (width * wareHouseArea.ActualWidth / AreaWidth) : width;

    //private double CalcHeight(double height) => wareHouseArea.ActualHeight > 0 ? (height * AreaHeight / wareHouseArea.ActualHeight) : height;

    //private double CalcBackHeight(double height) => wareHouseArea.ActualHeight > 0 ? (height * wareHouseArea.ActualHeight / AreaHeight) : height;

    private void mainControl_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if(Racks != null)
        {
            wareHouseArea.Children.Clear();

            foreach (RackObject item in Racks)
            {
                AddRack(item);
            }
        }
    }
}


