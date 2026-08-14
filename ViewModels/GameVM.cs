namespace CrappyLauncher.ViewModels
{
    class GameVM
    {
        public string Name { get; set; }
        public string Banner { get; set; }
        public string Location { get; set; }
        public bool SteamGame { get; set; }
        public string AppID { get; set; }

        public GameVM(string name, string? location = null, string? banner = null, bool? steamGame = null, string? appID = null)
        {
            Name = name;
            Location = location ?? "";
            Banner = banner ?? "./Resources/Images/DefaultBanner.png";
            SteamGame = steamGame ?? false;
            AppID = appID ?? "";
        }
    }
}