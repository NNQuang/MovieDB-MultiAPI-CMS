using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MovieService.Business.Abstract;
using MovieService.Entities.Concrete;
using MovieService.Entities.Dtos;
using System.Threading.Tasks;
using System.Web;

namespace MovieService.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // GET: api/Movies
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllMovies()
        {
            var movieResult = await _movieService.GetAllAsync();
            if (movieResult.Success)
            {
                return Ok(movieResult.Data);
            }
            return NotFound(movieResult.Message);
        }

        [HttpGet("GetAllActive")]
        public async Task<IActionResult> GetAllActiveMovies()
        {
            var activeMovieResult = await _movieService.GetAllActiveAsync();
            if (activeMovieResult.Success)
            {
                return Ok(activeMovieResult.Data);
            }
            return NotFound(activeMovieResult.Message);
        }

        //GET: api/Movies/ => Header'da "MovieId" veya "MovieTitle" key'i ile nesne getirilebiliyor.
        [HttpGet]
        public async Task<IActionResult> GetMovie()
        {
            if (Request.Headers.ContainsKey("MovieId"))
            {
                if (int.TryParse(Request.Headers["MovieId"], out int id))
                {
                    var movieResult = await _movieService.GetByMovieIdAsync(id);
                    if (movieResult.Success)
                    {
                        return Ok(movieResult.Data.Movie);
                    }
                    return NotFound(movieResult.Message);
                }
                return BadRequest("MovieId is not valid.");
            }
            if (Request.Headers.ContainsKey("MovieTitle"))
            {
                // Film isimlerinde ascii olmayan karakterler olduğu için isteği atarken html ile encode edip yollamak durumunda kaldım, base64 de deneyebilirdim ama risk almamak için bu yolu kullanıyorum.
                string movieTitle = HttpUtility.HtmlDecode(Request.Headers["MovieTitle"]);
                var movieResult = await _movieService.GetByMovieNameAsync(movieTitle);
                if (movieResult.Success)
                {
                    return Ok(movieResult.Data.Movie);
                }
                return NotFound(movieResult.Message);
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(MovieAddDto movieAddDto)
        {
            if (ModelState.IsValid)
            {
                var movieResult = await _movieService.AddAsync(movieAddDto);
                return Ok(movieResult.Data.Movie);
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("update")]
        public async Task<IActionResult> Update(MovieUpdateDto movieUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var movieResult = await _movieService.UpdateAsync(movieUpdateDto);
                if (movieResult.Success)
                {
                    return Ok(movieResult.Message);
                }
            }
            return BadRequest("Update data is invalid.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<Movie> moviePatch)
        {
            var patchResult = await _movieService.ApplyPatchAsync(id, moviePatch);
            if (!patchResult.Success)
            {
                return NotFound(patchResult.Message);
            }
            return Ok(patchResult.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete()
        {
            if (int.TryParse(Request.Headers["MovieId"], out int id) && Request.Headers.ContainsKey("ModifiedByName"))
            {
                string modifiedByName = Request.Headers["ModifiedByName"];
                var result = await _movieService.DeleteAsync(id, modifiedByName);
                return Ok(result);
            }
            return BadRequest("Given MovieId or ModifiedByName is not valid.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("HardDelete")]
        public async Task<IActionResult> HardDelete()
        {
            if (int.TryParse(Request.Headers["MovieId"], out int id))
            {
                var result = await _movieService.HardDeleteAsync(id);
                return Ok(result);
            }
            return BadRequest("Given MovieId is not valid.");
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search()
        {
            if (Request.Headers.ContainsKey("Search"))
            {
                string movieTitle = HttpUtility.HtmlDecode(Request.Headers["Search"]);
                var movieResult = await _movieService.SearchByMovieNameAsync(movieTitle);
                return Ok(movieResult.Data);
            }
            return BadRequest("Search key is invalid");
        }

        [HttpGet("GetAllByGenreId")]
        public async Task<IActionResult> GetByGenreId()
        {
            if (int.TryParse(Request.Headers["GenreId"], out int id))
            {
                var result = await _movieService.GetAllByGenreId(id);
                return Ok(result.Data);
            }
            return BadRequest("Given GenreId is not valid.");
        }

        [HttpGet("GetAllByActorId")]
        public async Task<IActionResult> GetByActorId()
        {
            if (int.TryParse(Request.Headers["ActorId"], out int id))
            {
                var result = await _movieService.GetAllByActorId(id);
                return Ok(result.Data);
            }
            return BadRequest("Given GenreId is not valid.");
        }

        [HttpGet("GetAllByDirectorId")]
        public async Task<IActionResult> GetByDirectorId()
        {
            if (int.TryParse(Request.Headers["DirectorId"], out int id))
            {
                var result = await _movieService.GetAllByDirectorId(id);
                return Ok(result.Data);
            }
            return BadRequest("Given DirectorId is not valid.");
        }
    }
}