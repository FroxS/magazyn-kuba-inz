using System.Windows;
using System.Windows.Controls;

namespace Warehouse.Theme.Controls
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(System.Windows.Media.Brush),
                typeof(ColorPicker),
                new UIPropertyMetadata(
                    System.Windows.Media.Brushes.White,
                    (obj, args) => { if (args.NewValue is System.Windows.Media.Brush brush && obj is ColorPicker colorPickerControl) colorPickerControl.SetColor(brush); },
                    null));

        public System.Windows.Media.Brush SelectedColor
        {
            get { return (System.Windows.Media.Brush)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public ColorPicker()
        {
            InitializeComponent();
        }

        private void OpenColorDialog_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();

            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Wybrany kolor
                System.Drawing.Color selectedColor = colorDialog.Color;

                // Przetwarzamy wybrany kolor na kolor WPF
                System.Windows.Media.Color wpfColor = System.Windows.Media.Color.FromArgb(
                    selectedColor.A,
                    selectedColor.R,
                    selectedColor.G,
                    selectedColor.B
                );

                SetColor(new System.Windows.Media.SolidColorBrush(wpfColor));
                // Tutaj możesz użyć wpfColor do czegokolwiek
                // Na przykład, przypisz go do tła okna:
                //this.Background = new System.Windows.Media.SolidColorBrush(wpfColor);
            }
        }

        private void SetColor(System.Windows.Media.Brush color)
        {
            SelectedColor = btnPicker.Background = color;
        }
    }
}
