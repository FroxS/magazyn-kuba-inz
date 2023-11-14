using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Warehouse.Controls;

public class Chart : Canvas
{
    #region Private fields

    private Line xAxisLine, yAxisLine;
    private double xAxisStart = 50, yAxisStart = 50;
    private Point origin;

    #endregion

    #region Dependency Property

    public double[,] Model
    {
        get { return (double[,])GetValue(ModelProperty); }
        set { SetValue(ModelProperty, value); }
    }

    public SolidColorBrush LineNetColor
    {
        get { return (SolidColorBrush)GetValue(LineNetColorProperty); }
        set { SetValue(LineNetColorProperty, value); }
    }

    public SolidColorBrush LineColor
    {
        get { return (SolidColorBrush)GetValue(LineColorProperty); }
        set { SetValue(LineColorProperty, value); }
    }

    public int LinesCount
    {
        get { return (int)GetValue(LinesCountProperty); }
        set { SetValue(LinesCountProperty, value); }
    }

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    #endregion

    #region Dependency

    public static readonly DependencyProperty ModelProperty =
        DependencyProperty.Register(nameof(Model), typeof(double[,]), typeof(Chart), new PropertyMetadata(null,(o,i) => { if (o is Chart chart) chart.Paint(); }));

    public static readonly DependencyProperty LineNetColorProperty =
        DependencyProperty.Register(nameof(LineNetColor), typeof(SolidColorBrush), typeof(Chart), new PropertyMetadata(Brushes.Gray, (o, i) => { if (o is Chart chart) chart.Paint(); }));

    public static readonly DependencyProperty LineColorProperty =
       DependencyProperty.Register(nameof(LineColor), typeof(SolidColorBrush), typeof(Chart), new PropertyMetadata(Brushes.Red, (o, i) => { if (o is Chart chart) chart.Paint(); }));

    public static readonly DependencyProperty LinesCountProperty =
        DependencyProperty.Register(nameof(LinesCount), typeof(int), typeof(Chart), new PropertyMetadata(5, (o, i) => { if (o is Chart chart) chart.Paint(); }));

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(Chart), new PropertyMetadata(""));

    #endregion

    #region Constructor

    public Chart()
    {
        SizeChanged += Chart_SizeChanged;
        Loaded += Chart_Loaded;
        Paint();
    }

    #endregion

    #region Paint

    private void Paint()
    {
        try
        {
            Children.Clear();

            if (this.ActualWidth > 0 && this.ActualHeight > 0)
            {
                Label title = new Label() { Content = $"{Title}" };

                title.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                title.Arrange(new Rect(0, 0, title.DesiredSize.Width, title.DesiredSize.Height));

                Children.Add(title);
                Canvas.SetLeft(title, this.ActualWidth / 2 - title.ActualWidth /2);
                Canvas.SetTop(title, 5);

                DrawNet();

                if(Model != null && Model.GetLength(0) > 0 && Model.GetLength(1) == 2)
                {
                    double squareWidth = this.ActualWidth - 2 * xAxisStart;
                    double squareHeight = this.ActualHeight - 2 * yAxisStart;
                    double squareStartX = xAxisStart;
                    double squareStartY = yAxisStart;

                    double maxValX = GetMaxValue(0);
                    double maxValY = GetMaxValue(1);

                    double scaledXPrev = 0;
                    double scaledYPrev = 0;

                    for (int i = 0; i < Model.GetLength(0); i++)
                    {
                        double xValue = Model[i, 0];
                        double yValue = Model[i, 1];

                        double scaledX = squareStartX + (xValue / maxValX) * squareWidth;
                        double scaledY = squareStartY + squareHeight - (yValue / maxValY) * squareHeight;

                        if (i > 0)
                        {
                            var line = new Line()
                            {
                                X1 = scaledXPrev,
                                Y1 = scaledYPrev,
                                X2 = scaledX,
                                Y2 = scaledY,
                                Stroke = LineColor,
                                StrokeThickness = 2,
                            };
                            Children.Add(line);
                        }

                        var dot = new Ellipse()
                        {
                            Width = 6,
                            Height = 6,
                            Fill = LineColor,
                        };
                        ToolTip toolTip = new ToolTip();
                        toolTip.Content = yValue;
                        dot.ToolTip = toolTip;
                        Canvas.SetLeft(dot, scaledX - 3);
                        Canvas.SetTop(dot, scaledY - 3);
                        Children.Add(dot);
                        scaledXPrev = scaledX;
                        scaledYPrev = scaledY;
                    }
                
                }
            }
        }
        catch(Exception ex){
#if DEBUG
            MessageBox.Show(ex.Message);
#endif
        }
    }

    private void DrawNet()
    {
        if (this.ActualWidth > 0 && this.ActualHeight > 0)
        {
            double intervalX = (this.ActualWidth - 2 * xAxisStart) / LinesCount;
            double intervalY = (this.ActualHeight - 2 * yAxisStart) / LinesCount;
            double maxValX = -1;
            double maxValY = -1;
            if (Model != null)
            {
                maxValX = GetMaxValue(0);
                maxValY = GetMaxValue(1);
            }

            xAxisLine = new Line()
            {
                X1 = xAxisStart,
                Y1 = this.ActualHeight - yAxisStart,
                X2 = this.ActualWidth - xAxisStart,
                Y2 = this.ActualHeight - yAxisStart,
                Stroke = LineNetColor,
                Opacity = .5,
                StrokeThickness = 5,
            };

            yAxisLine = new Line()
            {
                X1 = xAxisStart,
                Y1 = yAxisStart,
                X2 = xAxisStart,
                Y2 = this.ActualHeight - yAxisStart,
                Stroke = LineNetColor,
                Opacity = .5,
                StrokeThickness = 5,
            };

            Children.Add(xAxisLine);
            Children.Add(yAxisLine);

            origin = new Point(xAxisLine.X1, yAxisLine.Y2);
            var xTextBlock0 = new TextBlock() { Text = $"{0}" };
            Children.Add(xTextBlock0);
            Canvas.SetLeft(xTextBlock0, origin.X);
            Canvas.SetTop(xTextBlock0, origin.Y + 5);

            var xValue = xAxisStart;
            double xPoint = origin.X + intervalX;
            while (Math.Round(xPoint) <= Math.Round(xAxisLine.X2))
            {
                var line = new Line()
                {
                    X1 = xPoint,
                    Y1 = yAxisStart,
                    X2 = xPoint,
                    Y2 = this.ActualHeight - yAxisStart,
                    Stroke = LineNetColor,
                    StrokeThickness = 2,
                    Opacity = 1,
                };

                Children.Add(line);

                if (maxValX > 0)
                    xValue = (maxValX / LinesCount) * ((xPoint - yAxisStart) / intervalX);

                if((100d / intervalX) < 1 )
                {
                    var textBlock = new TextBlock { Text = $"{xValue.ToString("n2")}", };

                    Children.Add(textBlock);
                    Canvas.SetLeft(textBlock, xPoint - 12.5);
                    Canvas.SetTop(textBlock, line.Y2 + 5);
                }

                

                xPoint += intervalX;
                xValue += intervalX;
            }

            var yTextBlock0 = new TextBlock() { Text = $"{0}" };
            Children.Add(yTextBlock0);
            Canvas.SetLeft(yTextBlock0, origin.X - 20);
            Canvas.SetTop(yTextBlock0, origin.Y - 10);

            var yValue = yAxisStart;
            double yPoint = origin.Y - intervalY;
            while (Math.Round(yPoint) >= Math.Round(yAxisLine.Y1))
            {
                var line = new Line()
                {
                    X1 = xAxisStart,
                    Y1 = yPoint,
                    X2 = this.ActualWidth - xAxisStart,
                    Y2 = yPoint,
                    Stroke = LineNetColor,
                    StrokeThickness = 2,
                    Opacity = 1,
                };

                Children.Add(line);

                if (maxValY > 0)
                    yValue = maxValY - (maxValY / LinesCount) * ((yPoint - xAxisStart) / intervalY);

                var textBlock = new TextBlock() { Text = $"{yValue.ToString("n2")}" };
                Children.Add(textBlock);
                Canvas.SetLeft(textBlock, line.X1 - 30);
                Canvas.SetTop(textBlock, yPoint - 10);

                yPoint -= intervalY;
                yValue += intervalY;
            }
        }
    }

    private double GetMaxValue(int pos = 0)
    {
        if (Model == null)
            return -1;
        int iloscWierszy = Model.GetLength(0);

        if (iloscWierszy == 0)
        {
            throw new ArgumentException("Tablica jest pusta.");
        }

        double najwieksza = Model[0, 0];

        for (int i = 0; i < iloscWierszy; i++)
        {
            double pierwszaWartosc = Model[i, pos];

            if (pierwszaWartosc > najwieksza)
            {
                najwieksza = pierwszaWartosc;
            }
        }

        return najwieksza;
    }

    #endregion


    #region Events

    private void Chart_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        Paint();
    }

    private void Chart_Loaded(object sender, RoutedEventArgs e)
    {
        Paint();
    }

    #endregion
}
