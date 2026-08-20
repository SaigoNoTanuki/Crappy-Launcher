using CrappyLauncher.Scripts;
using System.Collections.ObjectModel;

namespace CrappyLauncher.ViewModels
{
    class GenreSelectVM : ViewModelBase
    {
        public GameVM Game { get; }
        public ObservableCollection<GenreVM> Genre { get; }
        public RelayCommand<GenreVM> AddSelectedGenreCommand { get; }

        private readonly Action _onGenreAdded;

        public GenreSelectVM(GameVM game, IEnumerable<GenreVM> allGenre, Action onGenreAdded)
        {
            Game = game;
            Genre = new ObservableCollection<GenreVM>(allGenre);
            _onGenreAdded = onGenreAdded;
            AddSelectedGenreCommand = new RelayCommand<GenreVM>(AddSelectedGenre);
        }

        private void AddSelectedGenre(GenreVM genre)
        {
            if (genre == null) return;
            if (!Game.Genre.Contains(genre.GenreName))
            {
                Game.Genre.Add(genre.GenreName);
                _onGenreAdded?.Invoke();
            }
        }
    }
}
