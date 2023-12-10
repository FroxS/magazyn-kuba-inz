using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Warehouse.Models.Attribute;

namespace Warehouse.Theme.Controls
{
    public class MyDataGrid : DataGrid
    {
        #region Dependency Property

        public ICollectionView Collection { get; set; }

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        //public IEnumerable FilterItems
        //{
        //    get { return (IEnumerable)GetValue(FilterItemsProperty); }
        //    set { SetValue(FilterItemsProperty, value); }
        //}

        #endregion

        #region Dependency

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register(nameof(SearchText), typeof(string), typeof(MyDataGrid), new PropertyMetadata(null, SeachChanged));

        //public static readonly DependencyProperty FilterItemsProperty =
        //    DependencyProperty.Register(nameof(FilterItems), typeof(IEnumerable), typeof(MyDataGrid), new PropertyMetadata(null, ItemsHanged));

        private static void ItemsHanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MyDataGrid mydataGrid && e.NewValue is IEnumerable list)
                mydataGrid.SetCollection(list);
        }

        private static void SeachChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MyDataGrid mydataGrid)
                mydataGrid?.Collection?.Refresh();
        }


        #endregion

        #region Constructor

        public MyDataGrid()
        {
        }

        #endregion

        #region Filter

        private bool Filter(object item)
        {
            if (item != null && !string.IsNullOrEmpty(SearchText))
            {
                PropertyInfo[] properties = item.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (Attribute.IsDefined(property, typeof(FilterColumnAttribute)))
                    {
                        string? val = property.GetValue(item)?.ToString();
                        if (!string.IsNullOrEmpty(val))
                        {
                            return val.ToLower().Contains(SearchText.ToLower());
                        }
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region Event

        //protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        //{
        //    SetCollection(ItemsSource);
            
        //}

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            SetCollection(newValue);
        }

        protected void SetCollection(IEnumerable items)
        {
            // Przechwyć oryginalny ItemsSource
            ICollectionView originalItemsSource = items as ICollectionView;

            if (Collection != null)
                Collection.Filter -= Filter;

            // Utwórz nowy widok kolekcji
            Collection = CollectionViewSource.GetDefaultView(items);

            // Sprawdź, czy oryginalny ItemsSource jest ICollectionView
            if (originalItemsSource != null)
            {
                // Usuń handler zdarzeń dla oryginalnego ItemsSource
                ((INotifyCollectionChanged)originalItemsSource).CollectionChanged -= ItemsSource_CollectionChanged;
            }

            // Dodaj handler zdarzeń dla nowego ItemsSource
            ((INotifyCollectionChanged)Collection).CollectionChanged += ItemsSource_CollectionChanged;


            Collection.Filter += Filter;
            SetBinding(this, ItemsSourceProperty, this, nameof(Collection));

        }
        private void ItemsSource_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            
        }

        #endregion

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
    }
}
