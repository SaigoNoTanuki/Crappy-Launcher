namespace CrappyLauncher.ViewModels
{
    class GenreVM : ViewModelBase
    {
        public string GenreName { get; set; }

        public GenreVM (string genreName)
        {
            GenreName = genreName;
        }
    }
}
