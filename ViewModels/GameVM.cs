namespace CrappyLauncher.ViewModels
{
    class GameVM
    {
        public string Name { get; set; }

        public string Banner { get; set; }

        public string Location { get; set; }

        public GameVM(string name, string location, string? banner = null)
        {
            Name = name;
            Location = location;
            Banner = banner ?? "./Resources/Images/DefaultBanner.png";
        }
    }
}