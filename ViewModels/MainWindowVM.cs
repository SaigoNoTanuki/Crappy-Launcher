using CrappyLauncher.Scripts;
using Microsoft.Win32;
using WinForms = System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using ValveKeyValue;
using Microsoft.VisualBasic;
using CrappyLauncher.Views;
using System.ComponentModel;
using System.Windows.Data;

namespace CrappyLauncher.ViewModels
{
    class MainWindowVM : ViewModelBase
    {
        public WindowState windowState;

        private readonly ObservableCollection<GameVM> _games;
        private readonly ObservableCollection<GenreVM> _genre;

        private readonly ICollectionView _gamesView;
        public ICollectionView GamesView => _gamesView;
        private GenreVM? _activeFilter;

        public IEnumerable<GameVM> Games => _games;
        public IEnumerable<GenreVM> Genre => _genre;

        public ICommand CloseAppCommand { get; }
        public ICommand MinimizeCommand { get; }
        public ICommand WindowCommand { get; }
        public ICommand AddFileCommand {  get; }
        public ICommand LaunchCommand {  get; }
        public ICommand RemoveGameCommand { get; }
        public ICommand RandomGameCommand {  get; }
        public ICommand AddSteamLibCommand { get; }
        public ICommand AddBannerCommand { get; }
        public ICommand FindBannerCommand { get; }
        public ICommand AddGenreCommand { get; }
        public ICommand OpenModalCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand ClearGamesCommand { get; }
        public ICommand ClearGenreCommand { get; }

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
            LaunchCommand = new RelayCommand<GameVM>(LaunchGame);
            AddSteamLibCommand = new RelayCommand(AddSteamLib);
            AddBannerCommand = new RelayCommand<GameVM>(AddBanner);
            FindBannerCommand = new RelayCommand<GameVM>(FindBanner);
            OpenModalCommand = new RelayCommand<GameVM>(OpenModal);
            AddGenreCommand = new RelayCommand(AddGenre);
            FilterCommand = new RelayCommand<GenreVM>(SetFilter);
            ClearGamesCommand = new RelayCommand(ClearGames);
            ClearGenreCommand = new RelayCommand(ClearGenre);

            _games = new ObservableCollection<GameVM>();
            _genre = new ObservableCollection<GenreVM>();
            _gamesView = CollectionViewSource.GetDefaultView(_games);
            _gamesView.Filter = FilterByGenre;

            LoadList();
            LoadGenre();
        }

        //Gets game and adds to config
        public void GetFiles()
        {
            Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
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

            if (!File.Exists(file)) return;

            string json = File.ReadAllText(file);

            var games = JsonSerializer.Deserialize<List<GameVM>>(json);

            if (games == null) return;

            foreach (var game in games)
            {
                game.Genre ??= new();
                _games.Add(game);
            }  
        }

        //Add Genre

        private void AddGenre()
        {
            string name = Interaction.InputBox("Enter Genre name", "Add Genre");

            if (string.IsNullOrWhiteSpace(name))
            {
                System.Windows.MessageBox.Show("Please Enter a valid name","Invalid Name",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _genre.Add(new GenreVM(name));

            SaveGenre();
        }

        private void SaveGenre()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string folder = System.IO.Path.Combine(appData, "Crappy Launcher");
            string file = System.IO.Path.Combine(folder, "Genre.json");

            Directory.CreateDirectory(folder);

            string json = JsonSerializer.Serialize(_genre, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(file, json);
        }

        private void LoadGenre()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string folder = System.IO.Path.Combine(appData, "Crappy Launcher");
            string file = System.IO.Path.Combine(folder, "Genre.json");

            if (!File.Exists(file)) return;

            string json = File.ReadAllText(file);

            var genre = JsonSerializer.Deserialize<List<GenreVM>>(json);

            if (genre == null) return;

            foreach (var g in genre)
                _genre.Add(g);
        }

        //Clear lists

        private void ClearGames()
        {
            _games.Clear();

            SaveList();
            RestartApp();
        }

        private void ClearGenre()
        {
            _genre.Clear();

            SaveGenre();
            RestartApp();
        }

        //Restart app

        private void RestartApp()
        {
            WinForms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }

        //Filter by genre

        private bool FilterByGenre(Object o)
        {
            if (_activeFilter == null) return true;

            if (o is not GameVM game) return false;
            return game.Genre.Contains(_activeFilter.GenreName);
        }

        private void SetFilter(GenreVM genre)
        {
            _activeFilter = (_activeFilter == genre) ? null : genre;
            _gamesView.Refresh();
        }

        //Prompt with Modal

        private void OpenModal(GameVM game)
        {
            var vm = new GenreSelectVM(game, _genre, SaveList);
            var selectionModal = new GenreSelectVV { DataContext = vm };
            selectionModal.Show();
        }

        //Adds steam Lib

        private void AddSteamLib()
        {
            WinForms.FolderBrowserDialog fd = new WinForms.FolderBrowserDialog();

            fd.Description = "Select the stamapps Directory";

            fd.ShowDialog();
            try
            {
                foreach (string file in Directory.GetFiles(fd.SelectedPath, "*.acf"))
                {
                    using var stream = File.OpenRead(file);

                    var serializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
                    var data = serializer.Deserialize(stream);

                    string name = data["name"].ToString();
                    bool steamGame = true;
                    string appID = data["appid"].ToString();

                    Console.WriteLine(data);

                    _games.Add(new GameVM(name, null, null, steamGame, appID));
                }

                SaveList();
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show($"Could'nt add Steam games: \n{e.Message}");
            }
             
            
        }

        private void LaunchGame(GameVM game)
        {
            try
            {
                if(!game.SteamGame)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = game.Location,
                        WorkingDirectory = System.IO.Path.GetDirectoryName(game.Location),
                        UseShellExecute = true,
                    });
                }
                else
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "steam://rungameid/"+game.AppID,
                        UseShellExecute = true
                    });
                }
                
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show($"Couldn't launch game:\n{e.Message}");
            }
        }

        private void LaunchRandom()
        {
            Random rnd = new Random();
            int num = rnd.Next(0, _games.Count);
            string gamePath = _games[num].Location;

            try
            {
                if (!_games[num].SteamGame)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = gamePath,
                        WorkingDirectory = System.IO.Path.GetDirectoryName(gamePath),
                        UseShellExecute = true,
                    });
                }
                else
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "steam://rungameid/"+_games[num].AppID,
                        UseShellExecute = true
                    });
                }
                
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show($"Couldn't launch game:\n{e.Message}");
            }
        }

        //Remove game

        private void RemoveGame(GameVM game)
        {
            _games.Remove(game);
            SaveList();
        }

        //Add Banner

        private void AddBanner(GameVM game)
        {
            Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();

            fd.Filter = "PNG, JPG | *.png; *.jpg;";
            fd.Title = "Please select a Banner Image";

            bool? success = fd.ShowDialog();

            if (success == true)
            {
                game.Banner = fd.FileName;

                SaveList();
            }
        }

        //Find Banner

        private void FindBanner(GameVM game)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = $"https://www.steamgriddb.com/search/grids?term={game.Name}",
                UseShellExecute = true,
            });
        }

        //Add genre to game

        private void AddGameGenre(GameVM game)
        {
            string genre = Interaction.InputBox("Enter genre", "Please make sure genre is Identical to existing genre");

            game.Genre.Add(genre);
        }

        //Window controls
        public void CloseApp()
        {
            System.Windows.Application.Current.Shutdown();
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
