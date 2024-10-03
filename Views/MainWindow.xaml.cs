// MainWindow.xaml.cs
using CopyChanges.ViewModels;
using System.Windows;

namespace CopyChanges.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
        }
    }
}
