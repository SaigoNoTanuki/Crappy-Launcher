using CrappyLauncher.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace CrappyLauncher.Views
{
    public partial class GenreSelectVV : Window
    {
        public GenreSelectVV()
        {
            MainWindowVM vm = new MainWindowVM();
            DataContext = vm;

            InitializeComponent();
        }

        // Window Controls
        private void OnClose(Object s, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnMouseHold(Object s, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
