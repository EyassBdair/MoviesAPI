using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreServices _genreServices;

        public GenresController(IGenreServices genreServices)
        {
            _genreServices = genreServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetALLAsync() {
            var genres =await _genreServices.GetAll();

            return Ok(genres);     
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateGenreDto dto)
        {
            var genre = new Genre { Name = dto.Name };  
            await _genreServices.Add(genre);

            return Ok(genre);
        }

        [HttpPut ("{id}")]
        public async Task<IActionResult> UpdateAsync(byte id, [FromBody] CreateGenreDto dto)
        {
            var genre = await _genreServices.GetById(id);
            if(genre == null)
                return NotFound($"No Genre was found with Id : {id}");

            genre.Name = dto.Name;

            _genreServices.Update(genre);


            return Ok(genre);
        }

        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await _genreServices.GetById(id);
            if (genre == null)
                return NotFound($"No Genre was found with Id : {id}");

            _genreServices.Delete(genre);

            return Ok(genre);
        }

    }

}
