using magazyn_kuba_inz.Conventers;
using magazyn_kuba_inz.Models.Page;
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
        public ApplicationPage CurrentPage
        {
            get => (ApplicationPage)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        /// <summary>
        /// Registers <see cref="CurrentPage"/> as a dependency property
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(
                nameof(CurrentPage), 
                typeof(ApplicationPage), 
                typeof(PageHost), 
                new UIPropertyMetadata(
                    default(ApplicationPage),
                    null, 
                    CurrentPagePropertyChanged)
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
            if(value is ApplicationPage page)
            {
                Frame? newPageFrame = (d as PageHost)?.NewPage;
                var oldPageContent = newPageFrame.Content;

                // Remove current page from new page frame
                newPageFrame.Content = null;

                if (App.AppHost?.Services != null)
                    newPageFrame.Content = page.ToBasePage(App.AppHost.Services);
            }
                
            
            return value;
        }

        #endregion
    }
}
