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


    // Window Controls
        public void OnClose(Object s, RoutedEventArgs e)
        {
            this.Close();
        }

        public void OnMinimize(Object s, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        public void OnWindowMode(Object s, RoutedEventArgs e)
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

        public void OnMouseHold(Object s, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
