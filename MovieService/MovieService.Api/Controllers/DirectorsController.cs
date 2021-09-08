using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MovieService.Business.Abstract;
using MovieService.Entities.Concrete;
using MovieService.Entities.Dtos;
using System.Threading.Tasks;

namespace MovieService.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        private readonly IDirectorService _directorService;

        public DirectorsController(IDirectorService directorService)
        {
            _directorService = directorService;
        }

        // GET: api/Directors
        [HttpGet("GetAll")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDirectors()
        {
            var directors = await _directorService.GetAllAsync();
            if (directors.Success)
            {
                return Ok(directors.Data);
            }
            return NotFound(directors.Message);
        }

        [HttpGet("GetAllActive")]
        public async Task<IActionResult> GetAllActiveDirectors()
        {
            var activeDirectors = await _directorService.GetAllActiveAsync();
            if (activeDirectors.Success)
            {
                return Ok(activeDirectors.Data);
            }
            return NotFound(activeDirectors.Message);
        }

        //GET: api/Directors/ => Header'da "DirectorId" veya "FullName" key'i ile nesne getirilebiliyor.
        [HttpGet]
        public async Task<IActionResult> GetDirector()
        {
            if (Request.Headers.ContainsKey("DirectorId"))
            {
                if (int.TryParse(Request.Headers["DirectorId"], out int id))
                {
                    var director = await _directorService.GetByDirectorIdAsync(id);
                    if (director.Success)
                    {
                        return Ok(director.Data.Director);
                    }
                    return NotFound(director.Message);
                }
                return BadRequest("Given DirectorId is invalid.");
            }
            if (Request.Headers.ContainsKey("FullName"))
            {
                var director = await _directorService.GetByDirectorNameAsync(Request.Headers["FullName"]);
                if (director.Success)
                {
                    return Ok(director.Data.Director);
                }
                return NotFound(director.Message);
            }
            return BadRequest("DirectorId or FullName key is invalid.");
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(DirectorAddDto directorAddDto)
        {
            if (ModelState.IsValid)
            {
                var directorResult = await _directorService.AddAsync(directorAddDto);
                if (directorResult.Success)
                {
                    return Ok(directorResult);
                }
            }
            return BadRequest();
        }

        // Eğer aranan isimde bir director yok ise oluşturur ve oluşturduğu nesneyi döner.
        //[Authorize(Roles = "Admin")]
        [HttpPost("GetOrCreate")]
        public async Task<IActionResult> GetOrCreate()
        {
            if (Request.Headers.ContainsKey("FullName"))
            {
                var result = await _directorService.GetOrCreateByNameAsync(Request.Headers["FullName"]);
                if (result.Success)
                {
                    return Ok(result.Data.Director);
                }
                return NotFound(result.Message);
            }
            return BadRequest("Please enter FullName");
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("update")]
        public async Task<IActionResult> Update(DirectorUpdateDto directorUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _directorService.UpdateAsync(directorUpdateDto);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<Director> directorPatch)
        {
            var patchResult = await _directorService.ApplyPatchAsync(id, directorPatch);
            if (!patchResult.Success)
            {
                return NotFound(patchResult.Message);
            }
            return Ok(patchResult.Message);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete()
        {
            if (int.TryParse(Request.Headers["DirectorId"], out int id) && Request.Headers.ContainsKey("ModifiedByName"))
            {
                string modifiedByName = Request.Headers["ModifiedByName"];
                var result = await _directorService.DeleteAsync(id, modifiedByName);
                return Ok(result);
            }
            return BadRequest("Given DirectorId or ModifiedByName is not valid.");
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("HardDelete")]
        public async Task<IActionResult> HardDelete()
        {
            if (int.TryParse(Request.Headers["DirectorId"], out int id))
            {
                var result = await _directorService.HardDeleteAsync(id);
                return Ok(result);
            }
            return BadRequest("Given DirectorId is not valid.");
        }
    }
}