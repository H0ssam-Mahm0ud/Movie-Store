using MovieStore.Models.Domain;
using MovieStore.Models.DTO;
using MovieStore.Repositories.Abstract;

namespace MovieStore.Repositories.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly DatabaseContext _context;

        public MovieService(DatabaseContext context)
        {
            _context = context;
        }


        public bool Add(Movie model)
        {
            try
            {
                _context.Movies.Add(model);
                _context.SaveChanges();
                foreach (int genreId in model.Genres)
                {
                    var movieGenre = new MovieGenre
                    {
                        MovieId = model.Id,
                        GenreId = genreId
                    };
                    _context.MovieGenre.Add(movieGenre);
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool Delete(int id)
        {
            try
            {
                var data = GetById(id);
                if (data == null) 
                { 
                    return false;
                }
                var movieGenres = _context.MovieGenre.Where(a => a.MovieId == data.Id);

                foreach (var movieGenre in movieGenres)
                {
                    _context.MovieGenre.Remove(movieGenre);
                }
                _context.Movies.Remove(data);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public Movie GetById(int id)
        {
            return _context.Movies.Find(id);
        }


        public List<int> GetGenreByMovieId(int movieId)
        {
            var genreId = _context.MovieGenre.Where(a => a.MovieId == movieId).Select(a => a.GenreId).ToList();
            return genreId;
        }


        public MovieListVM List(string term = "", bool paging = false, int currentPage = 0)
        {
            var data = new MovieListVM();

            var list = _context.Movies.ToList();

            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                list = list.Where(a => a.Title.ToLower().StartsWith(term)).ToList();
            }

            if (paging)
            {
                int pageSize = 5;
                int count = list.Count;
                int TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                list = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                data.PageSize = pageSize;
                data.CurrentPage = currentPage;
                data.TotalPages = TotalPages;
            }

            foreach (var movie in list)
            {
                var genres = (from genre in _context.Genres
                              join mg in _context.MovieGenre
                              on genre.Id equals mg.GenreId
                              where mg.MovieId == movie.Id
                              select genre.GenreName
                              ).ToList();
                var genreNames = string.Join(',', genres);
                movie.GenreNames = genreNames;
            }
            data.MovieList = list.AsQueryable();
            return data;
        }


        public bool Update(Movie model)
        {
            try
            {
                var genresToDeleted = _context.MovieGenre.Where(a => a.MovieId == model.Id && !model.Genres.Contains(a.GenreId)).ToList();
                foreach (var mGenre in genresToDeleted)
                {
                    _context.MovieGenre.Remove(mGenre);
                }
                foreach (int genId in model.Genres)
                {
                    var movieGenre = _context.MovieGenre.FirstOrDefault(a => a.MovieId == model.Id && a.GenreId == genId);
                    if (movieGenre == null)
                    {
                        movieGenre = new MovieGenre { GenreId = genId, MovieId = model.Id };
                        _context.MovieGenre.Add(movieGenre);
                    }
                }
                _context.Movies.Update(model);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
