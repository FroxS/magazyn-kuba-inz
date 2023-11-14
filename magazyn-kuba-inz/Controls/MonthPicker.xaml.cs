using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Warehouse.Controls
{
    /// <summary>
    /// Interaction logic for MonthPicker.xaml
    /// </summary>
    public partial class MonthPicker : UserControl
    {
        private int currentIndex = 0;
        private const int PageSize = 50;

        public List<string> Months { get; set; }

        public ObservableCollection<int> Years { get; set; }

        public int Month
        {
            get { return (int)GetValue(MonthProperty); }
            set { SetValue(MonthProperty, value); }
        }

        public static readonly DependencyProperty MonthProperty =
            DependencyProperty.Register(nameof(Month), typeof(int), typeof(MonthPicker), new PropertyMetadata(DateTime.Now.Month));

        public int Year
        {
            get { return (int)GetValue(YearProperty); }
            set { SetValue(YearProperty, value); }
        }

        public static readonly DependencyProperty YearProperty =
            DependencyProperty.Register(nameof(Year), typeof(int), typeof(MonthPicker), new PropertyMetadata(DateTime.Now.Year));

        public MonthPicker()
        {
            InitializeComponent();

            
            Months = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).ToList();
            MonthCB.ItemsSource = Months;
            Years = new ObservableCollection<int>();

            // Inicjalizacja danych (możesz dostosować to do swoich potrzeb)
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 100; i <= currentYear + 100; i++)
            {
                Years.Add(i);
            }

            // Załaduj pierwsze 50 lat
            LoadItems(0, PageSize);

        }

        private void ComboBox_ScrollChanged(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
        {
            // Sprawdź, czy użytkownik przewija w dół
            if (e.VerticalChange > 0)
            {
                // Jeśli jesteśmy blisko końca, załaduj następne 50 lat
                if (currentIndex + PageSize >= Years.Count)
                {
                    LoadItems(currentIndex, PageSize);
                }
            }
            // Możesz również obsłużyć przewijanie w górę, jeśli jest to wymagane
        }

        private void LoadItems(int startIndex, int count)
        {
            for (int i = startIndex; i < startIndex + count && i < Years.Count; i++)
            {
                //YearsCB.Items.Add(Years[i]);

            }
            YearsCB.ItemsSource = Years;
            currentIndex += count;
        }
    }
}
