using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Warehouse.Models.Attribute;

namespace Warehouse.Theme.Controls
{

    public class MyDataGrid : DataGrid
    {
        #region Dependency Property

        public ICollectionView Collection { get; set; }

		public Dictionary<string, bool> GroupByDefinition { get; set; }

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

		public ObservableCollection<string> GroupBy
		{
			get { return (ObservableCollection<string>)GetValue(GroupByProperty); }
			set { SetValue(GroupByProperty, value); }
		}

		public ContextMenu HeaderContextMenu
		{
			get { return (ContextMenu)GetValue(HeaderContextMenuProperty); }
			set { SetValue(HeaderContextMenuProperty, value); }
		}

		#endregion

		#region Dependency

		public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register(nameof(SearchText), typeof(string), typeof(MyDataGrid), new PropertyMetadata(null, SeachChanged));

        public static readonly DependencyProperty FilterItemsProperty =
            DependencyProperty.Register(nameof(FilterItems), typeof(IEnumerable), typeof(MyDataGrid), new PropertyMetadata(null, ItemsHanged));

		public static readonly DependencyProperty GroupByProperty =
			DependencyProperty.Register(nameof(GroupBy), typeof(ObservableCollection<string>), typeof(MyDataGrid), new PropertyMetadata(new ObservableCollection<string>(), GroupByChanged));

		public static readonly DependencyProperty HeaderContextMenuProperty =
			DependencyProperty.Register(nameof(HeaderContextMenu), typeof(ContextMenu), typeof(MyDataGrid), new PropertyMetadata(null));

		private static void GroupByChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is MyDataGrid mydataGrid && e.NewValue is ObservableCollection<string> list)
            {
				mydataGrid.UpdateGroupBy(list);
			}	
		}

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

        private ContextMenu GetContextMenu()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem meniItemGroupBy = new MenuItem() { Header = "Group by", };
            if (GroupByDefinition == null)
				GroupByDefinition = new Dictionary<string, bool>();
            foreach (DataGridColumn column in Columns) 
            {
                if (column is DataGridBoundColumn boundColumn)
                {
                    var binding = boundColumn.Binding as Binding;
                    if (binding != null)
                    {
                        string bindingPath = binding.Path.Path;
                        if (!string.IsNullOrEmpty(bindingPath))
                        {
							if (!GroupByDefinition.ContainsKey(bindingPath))
							{
								GroupByDefinition.Add(bindingPath,false);
							}

							MenuItem mi = new MenuItem() { Header = column.Header, IsCheckable = true, Tag = bindingPath };
							mi.IsChecked = GroupByDefinition[bindingPath];
							mi.Checked += (x, y) => {
								if (x is MenuItem mi)
								{
									GroupByDefinition[bindingPath] = mi.IsChecked;
									UpdateCollectionGroup();
								}
							};
							mi.Unchecked += (x, y) => {
								if (x is MenuItem mi)
								{
									GroupByDefinition[bindingPath] = mi.IsChecked;
									UpdateCollectionGroup();
								}
							};
							meniItemGroupBy.Items.Add(mi);
						}

                    }
                }

            }
            menu.Items.Add(meniItemGroupBy);
            return menu;
        }

        private void UpdateGroupBy(ObservableCollection<string> list)
        {
            if (list == null)
                return;

			if (GroupByDefinition == null)
				GroupByDefinition = new Dictionary<string, bool>();

            GroupByDefinition.Clear();

			foreach (string item in list)
            {
                if(GroupByDefinition.ContainsKey(item))
                    GroupByDefinition[item] = true;
                else
					GroupByDefinition.Add(item, true);
			}

            UpdateCollectionGroup();
		}

        private void UpdateCollectionGroup()
        {
            if(GroupByDefinition != null)
            {
                Collection.GroupDescriptions.Clear();

                foreach(var groupby in GroupByDefinition.Where(x => x.Value))
                {
					Collection.GroupDescriptions.Add(new PropertyGroupDescription(groupby.Key));
				}
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
					Collection.GroupDescriptions.Add(new PropertyGroupDescription(groupby.Key));
				}
            }

            Collection.Filter += Filter;
            SetBinding(this, ItemsSourceProperty, this, nameof(Collection));

        }

        private void ItemsSource_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            
        }

		#endregion

		#region Helpers

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
}
