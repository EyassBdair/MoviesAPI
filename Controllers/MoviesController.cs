using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMapper _mapper; 

        private readonly IMovieServices _movieServices;
        private readonly IGenreServices _genreServices;



        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;

        public MoviesController(IMovieServices movieServices, IGenreServices genreServices, IMapper mapper)
        {
            _movieServices = movieServices;
            _genreServices = genreServices;
            _mapper = mapper;
        }

        [HttpGet]
         public async Task<IActionResult> GetAllAsync()
         {
            //   var movies = await _context.Movies.OrderBy(g => g.Id ).ToListAsync();
            //  var movies = await _context.Movies.Include(m=>m.genre).ToListAsync();
            var movies = await _movieServices.GetAll();

            //************
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);


            return Ok(data);
         }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(byte id)
        {
           /* var movie = await _movieServices.GetById(id);

            if (movie == null)
                return NotFound();

               var dot = new MovieDetailsDto
               {
                   Id = movie.Id,
                   GenreId = movie.GenreId,
                   genreName = movie.genre.Name,
                   Poster = movie.Poster,
                   Rate = movie.Rate,
                   Storeline = movie.Storeline,
                   Title = movie.Title,
                   Year = movie.Year,

               };
               */

            var movie = await _movieServices.GetById(id);

            if (movie == null)
                return NotFound();

            var dto = _mapper.Map<MovieDetailsDto>(movie);

            return Ok(dto);

        }



        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        {
            var movies = await _movieServices.GetAll(genreId);

            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);


            return Ok(data);

        }

        [HttpPost]
        public async Task<IActionResult> CerateAsync([FromForm]MovesDto dto)
        {

            if (dto.Poster == null)
                return BadRequest("Poster is required!");

            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB!");

            var isValidGenre = await _genreServices.IsvalidGenre(dto.GenreId);

            if (!isValidGenre)
                return BadRequest("Invalid genere ID!");

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            /* var movie = new Movie
             {
                 Poster = dataStream.ToArray() ,
                 Rate = dto.Rate,
                 Title = dto.Title,
                 Year = dto.Year,    
                 GenreId = dto.GenreId,
                 Storeline = dto.Storeline,

             };
            */
            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = dataStream.ToArray();

            _movieServices.Add(movie);
          
            return Ok(movie);   

        }

   

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] MovesDto dto)
        {

            if (dto.Poster == null)
                return BadRequest("Poster is required!");

            var movie = await _movieServices.GetById(id);
            if (movie == null)
                return NotFound($"No Movie was found with Id : {id}");

            var isValidGenre = await _genreServices.IsvalidGenre(dto.GenreId);

            if (!isValidGenre)
                return BadRequest("Invalid genere ID!");


            if (dto.Poster == null)
            {

                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");

                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB!");
                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }

            movie.Title = dto.Title;        
            movie.Year = dto.Year;
            movie.GenreId = dto.GenreId;
            movie.Storeline = dto.Storeline;
            movie.Rate = dto.Rate;


           _movieServices.Update(movie);

            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _movieServices.GetById(id);   
            if (movie == null)
                return NotFound($"No Movie was found with Id : {id}");

            _movieServices.Delete(movie);

            return Ok(movie);
        }

    }
}
