using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MovieService.Business.Abstract;
using MovieService.Entities.Concrete;
using MovieService.Entities.Dtos;
using System;
using System.Threading.Tasks;

namespace MovieService.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        // GET: api/Genres

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _genreService.GetAllAsync();
            if (genres.Success)
            {
                return Ok(genres.Data);
            }
            return NotFound(genres.Message);
        }

        [HttpGet("GetAllActive")]
        public async Task<IActionResult> GetAllActiveGenres()
        {
            var activeGenres = await _genreService.GetAllActiveAsync();
            if (activeGenres.Success)
            {
                return Ok(activeGenres.Data);
            }
            return NotFound(activeGenres.Message);
        }

        //GET: api/Genres/ => Header'da "GenreId" veya "GenreName" key'i ile nesne getirilebiliyor.
        [HttpGet]
        public async Task<IActionResult> GetGenre()
        {
            if (Request.Headers.ContainsKey("GenreId"))
            {
                var genre = await _genreService.GetByGenreIdAsync(Convert.ToInt32(Request.Headers["GenreId"]));
                if (genre.Success)
                {
                    return Ok(genre.Data.Genre);
                }
                return NotFound(genre.Message);
            }
            if (Request.Headers.ContainsKey("GenreName"))
            {
                var genre = await _genreService.GetByGenreNameAsync(Request.Headers["GenreName"]);
                if (genre.Success)
                {
                    return Ok(genre.Data.Genre);
                }
                return NotFound(genre.Message);
            }
            return BadRequest();
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(GenreAddDto genreAddDto)
        {
            if (ModelState.IsValid)
            {
                var genreResult = await _genreService.AddAsync(genreAddDto);
                return Ok(genreResult);
            }
            return BadRequest();
        }

        // Eğer aranan isimde bir genre yok ise oluşturur ve oluşturduğu nesneyi döner.

        [HttpPost("GetOrCreate")]
        public async Task<IActionResult> GetOrCreate()
        {
            if (Request.Headers.ContainsKey("GenreName"))
            {
                var result = await _genreService.GetOrCreateByNameAsync(Request.Headers["GenreName"]);
                if (result.Success)
                {
                    return Ok(result.Data.Genre);
                }
                return NotFound(result.Message);
            }
            return BadRequest("Please enter GenreName.");
        }


        [HttpPost("Update")]
        public async Task<IActionResult> Update(GenreUpdateDto genreUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _genreService.UpdateAsync(genreUpdateDto);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }


        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<Genre> genrePatch)
        {
            var patchResult = await _genreService.ApplyPatchAsync(id, genrePatch);
            if (!patchResult.Success)
            {
                return NotFound(patchResult.Message);
            }
            return Ok(patchResult.Message);
        }


        [HttpPost("Delete")]
        public async Task<IActionResult> Delete()
        {
            if (int.TryParse(Request.Headers["GenreId"], out int id) && Request.Headers.ContainsKey("ModifiedByName"))
            {
                string modifiedByName = Request.Headers["ModifiedByName"];
                var result = await _genreService.DeleteAsync(id, modifiedByName);
                return Ok(result);
            }
            return BadRequest("Given GenreId or ModifiedByName is not valid.");
        }


        [HttpPost("HardDelete")]
        public async Task<IActionResult> HardDelete()
        {
            if (int.TryParse(Request.Headers["GenreId"], out int id))
            {
                var result = await _genreService.HardDeleteAsync(id);
                return Ok(result);
            }
            return BadRequest("Given GenreId is not valid.");
        }
    }
}