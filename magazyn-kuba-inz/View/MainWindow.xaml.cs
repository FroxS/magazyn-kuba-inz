using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Input;

namespace magazyn_kuba_inz.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IMainWindow
{
    public MainWindow(MainViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        App.AppHost?.Services.GetRequiredService<IApp>().Exit();
    }


    private void CloseBtn_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}

