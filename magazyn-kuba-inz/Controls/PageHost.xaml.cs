using magazyn_kuba_inz.Conventers;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.Page;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace magazyn_kuba_inz.Controls
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
        public EApplicationPage CurrentPage
        {
            get => (EApplicationPage)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        /// <summary>
        /// Registers <see cref="CurrentPage"/> as a dependency property
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(
                nameof(CurrentPage), 
                typeof(EApplicationPage), 
                typeof(PageHost), 
                new UIPropertyMetadata(
                    default(EApplicationPage),
                    null, 
                    CurrentPagePropertyChanged)
                );


        /// <summary>
        /// The current page to show in the page host
        /// </summary>
        public BasePageViewModel CurrentPageViewModel
        {
            get => (BasePageViewModel)GetValue(CurrentPageViewModelProperty);
            set => SetValue(CurrentPageViewModelProperty, value);
        }

        /// <summary>
        /// Registers <see cref="CurrentPage"/> as a dependency property
        /// </summary>
        public static readonly DependencyProperty CurrentPageViewModelProperty =
            DependencyProperty.Register(
                nameof(CurrentPageViewModel),
                typeof(BasePageViewModel),
                typeof(PageHost),
                null
                );



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
            if(value is EApplicationPage page)
            {
                Frame? newPageFrame = (d as PageHost)?.NewPage;
                var oldPageContent = newPageFrame.Content;

                // Remove current page from new page frame
                newPageFrame.Content = null;

                if (App.AppHost?.Services != null)
                {
                    var pageVIew = page.ToBasePage(App.AppHost.Services);
                    //(d as PageHost).CurrentPageViewModel = pageVIew.ViewModelObject as BasePageViewModel;
                    App.AppHost.Services.GetService<INavigation>().UpdateViewModel(pageVIew.ViewModelObject as BasePageViewModel);
                    newPageFrame.Content = pageVIew;
                }
                    
            }
                
            
            return value;
        }

        #endregion
    }
}
