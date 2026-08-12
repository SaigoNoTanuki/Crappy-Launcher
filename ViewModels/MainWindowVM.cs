using CrappyLauncher.Scripts;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace CrappyLauncher.ViewModels
{
    class MainWindowVM : ViewModelBase
    {
        public WindowState windowState;

        private readonly ObservableCollection<GameVM> _games;

        public IEnumerable<GameVM> Games => _games;

        public ICommand CloseAppCommand { get; }
        public ICommand MinimizeCommand { get; }
        public ICommand WindowCommand { get; }
        public ICommand AddFileCommand {  get; }
        public ICommand LaunchCommand {  get; }
        public ICommand RemoveGameCommand { get; }
        public ICommand RandomGameCommand {  get; }

        public WindowState WindowState
        {
            get => windowState;
            set
            {
                windowState = value;
                OnPropertyChanged();
            }

        }

        public MainWindowVM()
        {
            CloseAppCommand = new RelayCommand(CloseApp);
            MinimizeCommand = new RelayCommand(MinimizeApp);
            WindowCommand = new RelayCommand(SetWindowMode);
            AddFileCommand = new RelayCommand(GetFiles);
            RemoveGameCommand = new RelayCommand<GameVM>(RemoveGame);
            RandomGameCommand = new RelayCommand(LaunchRandom);
            LaunchCommand = new RelayCommand<string>(LaunchGame);
            _games = new ObservableCollection<GameVM>();

            LoadList();
        }

        //Gets game and adds to config
        public void GetFiles()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Executables | *.exe";
            fd.Multiselect = true;
            fd.Title = "Select game Executable(s)";
            bool? success = fd.ShowDialog();

            if (success == true)
            {
                foreach (string path in fd.FileNames)
                {
                    string name = System.IO.Path.GetFileNameWithoutExtension(path);
                    _games.Add(new GameVM(name, path));
                }

                SaveList();
            }   
        }

        private void SaveList()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string folder = System.IO.Path.Combine(appData, "Crappy Launcher");
            string file = System.IO.Path.Combine(folder, "Games.json");

            Directory.CreateDirectory(folder);

            string json = JsonSerializer.Serialize(_games, new JsonSerializerOptions{WriteIndented = true});

            File.WriteAllText(file, json);
        }

        private void LoadList()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string folder = System.IO.Path.Combine(appData, "Crappy Launcher");
            string file = System.IO.Path.Combine(folder, "Games.json");

            if (!File.Exists(file))
                return;

            string json = File.ReadAllText(file);

            var games = JsonSerializer.Deserialize<List<GameVM>>(json);

            if (games == null)
                return;

            foreach (var game in games)
                _games.Add(game);
        }

        //Game Launching

        private void LaunchGame(string path)
        {
            try
            {
                
                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    WorkingDirectory = System.IO.Path.GetDirectoryName(path),
                    UseShellExecute = true,
                });
            }
            catch (Exception e)
            {
                MessageBox.Show($"Couldn't launch game:\n{e.Message}");
            }
        }

        private void LaunchRandom()
        {
            Random rnd = new Random();
            int num = rnd.Next(0, _games.Count);
            string gamePath = _games[num].Location;

            try
            {

                Process.Start(new ProcessStartInfo
                {
                    FileName = gamePath,
                    WorkingDirectory = System.IO.Path.GetDirectoryName(gamePath),
                    UseShellExecute = true,
                });
            }
            catch (Exception e)
            {
                MessageBox.Show($"Couldn't launch game:\n{e.Message}");
            }
        }

        //Remove game

        private void RemoveGame(GameVM game)
        {
            _games.Remove(game);
            SaveList();
        }


        //Window controls
        public void CloseApp()
        {
            Application.Current.Shutdown();
        }

        public void MinimizeApp()
        {
            WindowState = WindowState.Minimized;
        }

        public void SetWindowMode()
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


        //I found out that putting "unchanging" logic in Code-Behind is much more efficient.
        //...Maybe a bit too late tho.
    }
}
