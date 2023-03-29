using MovieStore.Models.Domain;
using MovieStore.Repositories.Abstract;

namespace MovieStore.Repositories.Implementation
{
    public class GenreService : IGenreService
    {
        private readonly DatabaseContext _context;

        public GenreService(DatabaseContext context)
        {
            _context = context;
        }


        public bool Add(Genre model)
        {
            try
            {
                _context.Genres.Add(model);
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
                var result = _context.Genres.Find(id);
                if (result == null)
                { 
                    return false;
                }
                _context.Genres.Remove(result);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public Genre GetById(int id)
        {
            return _context.Genres.Find(id);
        }


        public IQueryable<Genre> List()
        {
            var data = _context.Genres.AsQueryable();
            return data;
        }


        public bool Update(Genre model)
        {
            try
            {
                _context.Genres.Update(model);
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
