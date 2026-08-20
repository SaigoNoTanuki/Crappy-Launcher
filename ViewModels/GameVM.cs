using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CrappyLauncher.ViewModels
{
    class GameVM : ViewModelBase
    {
        public string Name { get; set; }
        public string _banner;
        public string Location { get; set; }
        public bool SteamGame { get; set; }
        public string AppID { get; set; }
        public ObservableCollection<string> _genre = new();

        public string Banner
        {
            get => _banner;
            set
            {
                if (_banner == value)
                    return;

                _banner = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Genre
        {
            get => _genre;
            set => _genre = value ?? new();
        }

        public GameVM(string name, string? location = null, string? banner = null, bool steamGame = false, string? appID = null, ObservableCollection<string>? genre = null)
        {
            Name = name;
            Location = location ?? "";
            Banner = banner ?? "./Resources/Images/DefaultBanner.png";
            SteamGame = steamGame;
            AppID = appID ?? "";
            Genre = genre;
        }
    }
}