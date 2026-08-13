using CrappyLauncher.ViewModels;
using CrappyLauncher.Views;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace CrappyLauncher
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainWindowVM vm = new MainWindowVM();
            DataContext = vm;

            UpdateIcon();
            StateChanged += WindowStateChanged;
        }

        //Sets the state of the Window? icon.
        public void WindowStateChanged(Object? s, EventArgs e)
        {
            UpdateIcon();
        }

        public void UpdateIcon()
        {
            if (WindowState != WindowState.Maximized)
            {
                WindowButton.Content = "\uE922";
            }
            else
            {
                WindowButton.Content = "\uE923";
            }
        }

        // Makes window moveable
        public void OnMouseHold(Object s, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
 

        //Library Drop down
        public void OnLibraryDrop(Object s, RoutedEventArgs e)
        {
            LibraryButton.ContextMenu.PlacementTarget = LibraryButton;
            LibraryButton.ContextMenu.IsOpen = true;
        }

        //Redirects
        private void ReportBugs(Object s, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/SaigoNoTanuki/Crappy-Launcher/issues",
                UseShellExecute = true,
            });
        }

        private void OpenPatchNotes(Object s, RoutedEventArgs e)
        {

        }

        private void OpenSupportPage(Object s, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.supportkori.com/saigonotanuki",
                UseShellExecute = true,
            });
        }

        private void OpenGithubPage(Object s, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/SaigoNoTanuki/Crappy-Launcher",
                UseShellExecute = true,
            });
        }

        //Settings
        private void SettingsButton(Object s, RoutedEventArgs e)
        {
            SettingsVV settingsWindow = new SettingsVV();
            settingsWindow.Show();
        }
    }
}