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
    private bool isDragging = false;
    private Point startPoint;
    private Rectangle selectedRectangle = null;
    public WareHouseArea()
    {
        InitializeComponent();
    }

    private void DodajProstokat_Click(object sender, RoutedEventArgs e)
    {
        Rectangle rectangle = new Rectangle
        {
            Width = 50,
            Height = 50,
            Fill = Brushes.Blue
        };

        Canvas.SetLeft(rectangle, 50);
        Canvas.SetTop(rectangle, 50);

        rectangle.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;

        canvas.Children.Add(rectangle);
    }

    private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is Rectangle rectangle)
        {
            isDragging = true;
            selectedRectangle = rectangle;
            startPoint = e.GetPosition(canvas);
        }
    }



    private void Canvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (isDragging && selectedRectangle != null)
        {
            Point currentPoint = e.GetPosition(canvas);
            double offsetX = currentPoint.X - startPoint.X;
            double offsetY = currentPoint.Y - startPoint.Y;

            double newLeft = Canvas.GetLeft(selectedRectangle) + offsetX;
            double newTop = Canvas.GetTop(selectedRectangle) + offsetY;

            Canvas.SetLeft(selectedRectangle, newLeft);
            Canvas.SetTop(selectedRectangle, newTop);

            startPoint = currentPoint;
        }
    }

    private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        isDragging = false;
        selectedRectangle = null;
    }
}


