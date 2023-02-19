using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Services
{
    public class MovieServices : IMovieServices
    {
        private readonly ApplicationDbContext _context;

        public MovieServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async  Task<Movie> Add(Movie movie)
        {
            await _context.AddAsync(movie);

            _context.SaveChanges();
            return movie;

        }

        public Movie Delete(Movie movie)
        {
            _context.Remove(movie); 
            _context.SaveChanges();

            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAll(byte genreId = 0)
        {
            return await _context.Movies
                .Where(m=>m.GenreId== genreId || genreId==0)
                .OrderByDescending(m => m.Rate)
                .Include(m => m.genre)
                .ToListAsync();
        }

        public  async Task<Movie> GetById(int id)
        {
            return await _context.Movies.Include(m => m.genre).SingleOrDefaultAsync(m => m.Id == id);
        }

        public Movie Update(Movie movie)
        {
            _context.Update(movie);
            _context.SaveChanges();

            return movie;
        }
    }
}
