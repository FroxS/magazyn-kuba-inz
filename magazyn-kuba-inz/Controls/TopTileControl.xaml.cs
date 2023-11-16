using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Warehouse.Controls
{
    /// <summary>
    /// Interaction logic for TopTileControl.xaml
    /// </summary>
    public partial class TopTileControl : UserControl, ICommandSource
    {
        #region Property

        public string TopText
        {
            get { return (string)GetValue(TopTextProperty); }
            set { SetValue(TopTextProperty, value); }
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public Geometry Icon
        {
            get { return (Geometry)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return (Geometry)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        #endregion

        #region Dependency

        public static readonly DependencyProperty TopTextProperty =
            DependencyProperty.Register(nameof(TopText), typeof(string), typeof(TopTileControl), new PropertyMetadata(""));

        public static readonly DependencyProperty DescriptionProperty =
           DependencyProperty.Register(nameof(Description), typeof(string), typeof(TopTileControl), new PropertyMetadata(""));

        public static readonly DependencyProperty IconProperty =
           DependencyProperty.Register(nameof(Icon), typeof(Geometry), typeof(TopTileControl), new PropertyMetadata(null));

        public static readonly DependencyProperty CommandProperty =
           DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(TopTileControl), new PropertyMetadata(null, CommandChanged));

        public static readonly DependencyProperty CommandParameterProperty =
           DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(TopTileControl), new PropertyMetadata(null));

        public static readonly DependencyProperty CommandTargetProperty =
           DependencyProperty.Register(nameof(CommandTarget), typeof(IInputElement), typeof(TopTileControl), new PropertyMetadata(null));

        #endregion


        #region Constructor

        public TopTileControl()
        {
            InitializeComponent();
            MouseDown += TopTileControl_MouseDown;
        }

        #endregion

        #region Event

        private void TopTileControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Command.CanExecute(CommandParameter)){
                Command.Execute(CommandParameter);
            }
        }

        private static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is TopTileControl topTile)
            {
                topTile.IsEnabled = topTile.Command.CanExecute(topTile.CommandParameter);
                topTile.Command.CanExecuteChanged += Command_CanExecuteChanged;
            }
        }

        private static void Command_CanExecuteChanged(object? sender, EventArgs e)
        {
            if (sender is TopTileControl topTile)
            {

                
            }
        }

        #endregion

    }
}
