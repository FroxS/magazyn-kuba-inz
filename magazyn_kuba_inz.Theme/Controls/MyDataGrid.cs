using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
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

        public Dictionary<DataGridColumn, bool> GroupByDefinition { get; protected set; }

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public IEnumerable FilterItems
        {
            get { return (IEnumerable)GetValue(FilterItemsProperty); }
            set { SetValue(FilterItemsProperty, value); }
        }

        #endregion

        #region Dependency

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register(nameof(SearchText), typeof(string), typeof(MyDataGrid), new PropertyMetadata(null, SeachChanged));

        public static readonly DependencyProperty FilterItemsProperty =
            DependencyProperty.Register(nameof(FilterItems), typeof(IEnumerable), typeof(MyDataGrid), new PropertyMetadata(null, ItemsHanged));

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
            ContextMenu = GetContextMenu();
            ContextMenuOpening += MyDataGrid_ContextMenuOpening;
        }

        private void MyDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ContextMenu = GetContextMenu();
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

        private ContextMenu GetContextMenu()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem meniItemGroupBy = new MenuItem() { Header = "Group by", };
            if (GroupByDefinition == null)
                GroupByDefinition = new Dictionary<DataGridColumn, bool>();
            foreach (DataGridColumn column in Columns) 
            {
                if (column is DataGridBoundColumn boundColumn)
                {
                    var binding = boundColumn.Binding as Binding;
                    if (binding != null)
                    {
                        string bindingPath = binding.Path.Path;

                    }
                }
                if (!GroupByDefinition.ContainsKey(column))
                {
                    GroupByDefinition.Add(column, false);
                }
                MenuItem mi = new MenuItem() { Header = column.Header, IsCheckable = true };
                mi.IsChecked = GroupByDefinition[column];
                mi.Checked += (x,y) => {
                    if(x is MenuItem mi)
                    {
                        GroupByDefinition[column] = mi.IsChecked;
                        UpdateCollectionGroup();
                    }     
                };
                mi.Unchecked += (x, y) => {
                    if (x is MenuItem mi)
                    {
                        GroupByDefinition[column] = mi.IsChecked;
                        UpdateCollectionGroup();
                    }
                };

                meniItemGroupBy.Items.Add(mi);
            }
            menu.Items.Add(meniItemGroupBy);
            return menu;
        }

        private void UpdateCollectionGroup()
        {
            if(GroupByDefinition != null)
            {
                Collection.GroupDescriptions.Clear();

                foreach(var groupby in GroupByDefinition.Where(x => x.Value))
                {
                    DataGridColumn column = groupby.Key;
                    if (column is DataGridBoundColumn boundColumn)
                    {
                        var binding = boundColumn.Binding as Binding;

                        if (binding != null)
                        {
                            string bindingPath = binding.Path.Path;
                            Collection.GroupDescriptions.Add(new PropertyGroupDescription(bindingPath));
                        }
                    }  
                }
                //Collection.Refresh();
            }
        }

        #endregion

        #region Event


        protected void SetCollection(IEnumerable items)
        {
            // Przechwyć oryginalny ItemsSource
            ICollectionView originalItemsSource = items as ICollectionView;

            if (Collection != null && Collection.Filter != null)
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

            if (GroupByDefinition != null)
            {
                Collection.GroupDescriptions.Clear();

                foreach (var groupby in GroupByDefinition.Where(x => x.Value))
                {
                    DataGridColumn column = groupby.Key;
                    if (column is DataGridBoundColumn boundColumn)
                    {
                        var binding = boundColumn.Binding as Binding;

                        if (binding != null)
                        {
                            string bindingPath = binding.Path.Path;
                            Collection.GroupDescriptions.Add(new PropertyGroupDescription(bindingPath));
                        }
                    }
                }
            }

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
