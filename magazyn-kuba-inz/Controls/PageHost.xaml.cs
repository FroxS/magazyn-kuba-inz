using Warehouse.Conventers;
using System.Windows;
using System.Windows.Controls;
using Warehouse.Core.Interface;
using System;

namespace Warehouse.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy PageHost.xaml
    /// </summary>
    public partial class PageHost : UserControl
    {
        #region Dependency Properties

        /// <summary>
        /// The current page to show in the page host
        /// </summary>
        public IBasePageViewModel CurrentPage
        {
            get => (IBasePageViewModel)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        /// <summary>
        /// The current page to show in the page host
        /// </summary>
        public IServiceProvider ServiceProvider
        {
            get => (IServiceProvider)GetValue(ServiceProviderProperty);
            set => SetValue(ServiceProviderProperty, value);
        }

        /// <summary>
        /// Registers <see cref="CurrentPage"/> as a dependency property
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(
                nameof(CurrentPage), 
                typeof(IBasePageViewModel), 
                typeof(PageHost), 
                new UIPropertyMetadata(
                    null,
                    null, 
                    CurrentPagePropertyChanged)
                );

        public static readonly DependencyProperty ServiceProviderProperty =
            DependencyProperty.Register(
                nameof(ServiceProvider),
                typeof(IServiceProvider),
                typeof(PageHost),
                new UIPropertyMetadata(null));

        #endregion

        #region Constructors

        public PageHost()
        {
            InitializeComponent();
        }

        #endregion

        #region Property Changed Events

        /// <summary>
        /// Called when the <see cref="CurrentPage"/> value has changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static object CurrentPagePropertyChanged(DependencyObject d, object value)
        {
            if(value is IBasePageViewModel page )
            {
                Frame? newPageFrame = (d as PageHost)?.NewPage;
                var oldPageContent = newPageFrame.Content;

                // Remove current page from new page frame
                newPageFrame.Content = null;


                IServiceProvider provider = (d as PageHost)?.ServiceProvider;
                if (provider == null)
                    provider = App.AppHost?.Services;

                if (provider != null)
                {
                    var pageVIew = page.ToBasePage(provider);
                    //(d as PageHost).CurrentPageViewModel = pageVIew.ViewModelObject as BasePageViewModel;
                    //App.AppHost.Services.GetService<INavigation>().UpdateViewModel(pageVIew.ViewModelObject as BasePageViewModel);
                    newPageFrame.Content = pageVIew;
                }
                    
            }
                
            
            return value;
        }

        #endregion
    }
}
