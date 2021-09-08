using Microsoft.AspNetCore.JsonPatch;
using MovieService.Core.Results.Abstract;
using MovieService.Entities.Concrete;
using MovieService.Entities.Dtos;
using System.Threading.Tasks;

namespace MovieService.Business.Abstract
{
    public interface IActorService
    {
        //Task<bool> Any(string actorName);
        Task<IDataResult<ActorDto>> GetByActorIdAsync(int actorId);

        Task<IDataResult<ActorDto>> GetByActorNameAsync(string actorName);

        Task<IDataResult<ActorListDto>> GetAllAsync();

        Task<IDataResult<ActorListDto>> GetAllActiveAsync();

        Task<IDataResult<ActorListDto>> GetAllByMovieNameAsync(string movieName);

        Task<IDataResult<ActorListDto>> GetAllByMovieIdAsync(int movieId);

        Task<IDataResult<ActorDto>> GetOrCreateByNameAsync(string actorName);

        Task<IResult> AddAsync(ActorAddDto actorDto);

        Task<IResult> AutoAddAsync(ActorAutoCreateDto actorAutoCreateDto);

        Task<IResult> UpdateAsync(ActorUpdateDto actorUpdateDto);

        Task<IResult> DeleteAsync(int actorId, string modifiedByName);

        Task<IResult> HardDeleteAsync(int actorId);

        Task<IDataResult<ActorDto>> ApplyPatchAsync(int id, JsonPatchDocument<Actor> jsonPatchDocument);

        Task<IDataResult<MovieAddDto>> MapActors(MovieAddDto movieAddDto);
    }
}