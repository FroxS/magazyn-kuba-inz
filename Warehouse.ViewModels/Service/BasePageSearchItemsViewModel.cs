﻿using Warehouse.Core.Interface;
using System.Collections.ObjectModel;
using Warehouse.ViewModel.Service;
using System.ComponentModel;
using System.Windows.Data;
using System.Reflection;
using Warehouse.Models.Attribute;
using System.Windows.Input;

namespace Warehouse.ViewModel.Pages;

public class BasePageSearchItemsViewModel<Item> : BasePageViewModel 
{
    #region Private fields

    protected ObservableCollection<Item> _items;
    protected Item? _selectedItem;
    protected string? _searchString;
    protected bool _canAddNew = true;
    protected bool _canDelete = true;
    #endregion

    #region Public Properties

    public ICollectionView Collection { get; protected set; }

    public virtual ObservableCollection<Item> Items
    {
        get => _items;
        protected set {
            SetProperty(ref _items, value, nameof(Items),
                () =>
                {
                    if(Collection != null)
                        Collection.Filter -= FilterCollection;
                    Collection = CollectionViewSource.GetDefaultView(value);
                    IEnumerable<GroupDescription> groupsBy = GroupBy();
                    if(groupsBy != null)
                    {
                        foreach (GroupDescription groupBy in groupsBy)
                            Collection.GroupDescriptions.Add(groupBy);
                    }
                    Collection.Filter += FilterCollection;
                }
            );
        }
    }

    public virtual Item? SelectedItem
    {
        get => _selectedItem;
        set { Item? old = _selectedItem; SetProperty(ref _selectedItem, value, onChanged: () => OnItemChanged(old, value)); }
    }

    public virtual string? SearchString
    {
        get => _searchString;
        set { SetProperty(ref _searchString, value, onChanged: () => Collection.Refresh()); }
    }

    public virtual bool CanAddNew
    {
        get => _canAddNew;
        protected set { SetProperty(ref _canAddNew, value); }
    }

    public virtual bool CanDelete
    {
        get => _canDelete;
        protected set { SetProperty(ref _canDelete, value); }
    }

    #endregion

    #region Command

    public ICommand DeleteItemsCommand { get; protected set; }

    public ICommand AddItemCommand { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="app">Application</param>
    public BasePageSearchItemsViewModel(IApp app) : base(app)
    {
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="app">Application</param>
    public BasePageSearchItemsViewModel(IApp app, BaseViewModel parent) : base(app, parent)
    {
    }

    #endregion

    #region Private methods

    protected bool FilterCollection(object value)
    {
        if (value is Item item && item != null && !string.IsNullOrEmpty(SearchString))
            return Filter(item,SearchString);
        else
            return true;
    }

    protected virtual bool Filter(Item value, string search)
    {
        if(value != null && !string.IsNullOrEmpty(search))
        {
            PropertyInfo[] properties = typeof(Item).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (Attribute.IsDefined(property, typeof(FilterColumnAttribute)))
                {
                    string? val = property.GetValue(value)?.ToString();
                    if(!string.IsNullOrEmpty(val))
                    {
                        return val.ToLower().Contains(search.ToLower());
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

    protected virtual void OnItemChanged(Item? oldItem, Item? newItem) { }
    
    protected virtual IEnumerable<GroupDescription>? GroupBy()
    {
        return null;
    }

    #endregion

}
