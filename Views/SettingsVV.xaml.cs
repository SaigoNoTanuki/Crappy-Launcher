using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CrappyLauncher.Views
{
    public partial class SettingsVV : Window
    {
        public SettingsVV()
        {
            InitializeComponent();

            UpdateIcon();
            StateChanged += WindowStateChanged;
        }

        //Window state
        private void WindowStateChanged(Object? s, EventArgs e)
        {
            UpdateIcon();
        }

        private void UpdateIcon()
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


    // Window Controls
        private void OnClose(Object s, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnMinimize(Object s, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnWindowMode(Object s, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
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
