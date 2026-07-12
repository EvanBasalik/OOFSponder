using Avalonia.Controls;
using OOFSponderMac.ViewModels;

namespace OOFSponderMac.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        // Persist settings when the user closes the window
        if (DataContext is MainViewModel vm)
        {
            vm.SaveCommand.Execute(null);
        }

        base.OnClosing(e);
    }
}
