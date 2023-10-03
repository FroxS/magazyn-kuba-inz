using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace magazyn_kuba_inz.Theme.Controls
{
    public class ItemMenuButton : Button
    {
        #region Dependency Property

        public Geometry IconPath
        {
            get { return (Geometry)GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }

        public bool ContainsSubMenu
        {
            get { return (bool)GetValue(ContainsSubMenuProperty); }
            set { SetValue(ContainsSubMenuProperty, value); }
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        
        public bool IsMenuOpen
        {
            get { return (bool)GetValue(IsMenuOpenProperty); }
            set { SetValue(IsMenuOpenProperty, value); }
        }

        public bool IsMenuSelected
        {
            get { return (bool)GetValue(IsMenuSelectedProperty); }
            set { SetValue(IsMenuSelectedProperty, value); }
        }

        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public object ClickCommandParameter
        {
            get { return (object)GetValue(ClickCommandParameterProperty); }
            set { SetValue(ClickCommandParameterProperty, value); }
        }

        #endregion

        #region Dependency

        public static readonly DependencyProperty IconPathProperty =
            DependencyProperty.Register("IconPath", typeof(Geometry), typeof(ItemMenuButton), new PropertyMetadata(null));

        public static readonly DependencyProperty ContainsSubMenuProperty =
            DependencyProperty.Register("ContainsSubMenu", typeof(bool), typeof(ItemMenuButton), new PropertyMetadata(false));

        public static readonly DependencyProperty IsMenuSelectedProperty =
            DependencyProperty.Register("IsMenuSelected", typeof(bool), typeof(ItemMenuButton), new PropertyMetadata(false));

        public static readonly DependencyProperty ClickCommandProperty =
            DependencyProperty.Register(
                "ClickCommand", 
                typeof(ICommand), 
                typeof(ItemMenuButton), 
                new PropertyMetadata(
                    null,
                    new PropertyChangedCallback(OnClickCommandChanged)
                ));

        public static readonly DependencyProperty ClickCommandParameterProperty =
            DependencyProperty.Register(
                "ClickCommandParameter",
                typeof(object),
                typeof(ItemMenuButton),
                new PropertyMetadata(
                    null
                ));

        public static readonly DependencyProperty IsMenuOpenProperty =
            DependencyProperty.Register("IsMenuOpen", typeof(bool), typeof(ItemMenuButton), new PropertyMetadata(false));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(ItemMenuButton), new PropertyMetadata(string.Empty));

        #endregion

        #region Constructor

        static ItemMenuButton()
        {
        }

        #endregion

        #region Private Methods

        private static void OnClickCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ItemMenuButton button)
            {
                //ClickCommandValue = (ICommand)d.GetValue(ClickCommandProperty);
                if (e.NewValue == null)
                    return;
                var findPath = button.Template.FindName("StackPanelMenuItem", button) as System.Windows.Controls.StackPanel;
                if(findPath!= null)
                {
                    button.UploadMouseOverTriger(findPath);
                }
            } 
        }

        public void UploadMouseOverTriger(UIElement panel)
        {
            panel.MouseDown -= Panel_MouseDown;
            panel.MouseDown += Panel_MouseDown;
        }

        private void Panel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ClickCommand == null)
                return;

            if(ClickCommand.CanExecute(ClickCommandParameter))
                ClickCommand.Execute(ClickCommandParameter);
        }


        #endregion
    }
}
