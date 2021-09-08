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
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;

        public ActorsController(IActorService actorService)
        {
            _actorService = actorService;
        }

        // GET: api/Actors
        [HttpGet("GetAll")]

        public async Task<IActionResult> GetAllActors()
        {
            var actors = await _actorService.GetAllAsync();
            if (actors.Success)
            {
                return Ok(actors.Data);
            }
            return NotFound(actors.Message);
        }

        [HttpGet("GetAllActive")]

        public async Task<IActionResult> GetAllActiveActors()
        {
            var activeActors = await _actorService.GetAllActiveAsync();
            if (activeActors.Success)
            {
                return Ok(activeActors.Data);
            }
            return NotFound(activeActors.Message);
        }

        //GET: api/Actors/ => Header'da "ActorId" veya "FullName" key'i ile nesne getirilebiliyor.
        [HttpGet]
        public async Task<IActionResult> GetActor()
        {
            if (Request.Headers.ContainsKey("ActorId"))
            {
                var actor = await _actorService.GetByActorIdAsync(Convert.ToInt32(Request.Headers["ActorId"]));
                if (actor.Success)
                {
                    return Ok(actor.Data.Actor);
                }
                return NotFound(actor.Message);
            }
            if (Request.Headers.ContainsKey("FullName"))
            {
                var actor = await _actorService.GetByActorNameAsync(Request.Headers["FullName"]);
                if (actor.Success)
                {
                    return Ok(actor.Data.Actor);
                }
                return NotFound(actor.Message);
            }
            return BadRequest();
        }

        //
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ActorAddDto actorAddDto)
        {
            if (ModelState.IsValid)
            {
                var actorResult = await _actorService.AddAsync(actorAddDto);
                if (actorResult.Success)
                {
                    return Ok(actorResult);
                }
            }
            return BadRequest();
        }

        // Eğer aranan isimde bir actor yok ise oluşturur ve oluşturduğu nesneyi döner.

        [HttpPost("GetOrCreate")]
        public async Task<IActionResult> GetOrCreate([FromHeader] string FullName)
        {
            if (Request.Headers.ContainsKey("FullName"))
            {
                var result = await _actorService.GetOrCreateByNameAsync(Request.Headers["FullName"]);
                if (result.Success)
                {
                    return Ok(result.Data.Actor);
                }
                return Ok(result.Message);
            }
            return BadRequest("Please enter FullName");
        }


        [HttpPost("update")]
        public async Task<IActionResult> Update(ActorUpdateDto actorUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _actorService.UpdateAsync(actorUpdateDto);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest();
        }


        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<Actor> actorPatch)
        {
            var patchResult = await _actorService.ApplyPatchAsync(id, actorPatch);
            if (!patchResult.Success)
            {
                return NotFound(patchResult.Message);
            }
            return Ok(patchResult.Message);
        }


        [HttpPost("Delete")]
        public async Task<IActionResult> Delete()
        {
            if (int.TryParse(Request.Headers["ActorId"], out int id) && Request.Headers.ContainsKey("ModifiedByName"))
            {
                string modifiedByName = Request.Headers["ModifiedByName"];
                var result = await _actorService.DeleteAsync(id, modifiedByName);
                return Ok(result);
            }
            return BadRequest("Given ActorId or ModifiedByName is not valid.");
        }


        [HttpPost("HardDelete")]
        public async Task<IActionResult> HardDelete()
        {
            if (int.TryParse(Request.Headers["ActorId"], out int id))
            {
                var result = await _actorService.HardDeleteAsync(id);
                return Ok(result);
            }
            return BadRequest("Given ActorId is not valid.");
        }
    }
}