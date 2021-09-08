using Microsoft.AspNetCore.JsonPatch;
using MovieService.Core.Results.Abstract;
using MovieService.Entities.Concrete;
using MovieService.Entities.Dtos;
using System.Threading.Tasks;

namespace MovieService.Business.Abstract
{
    public interface IDirectorService
    {
        Task<IDataResult<DirectorDto>> GetByDirectorIdAsync(int directorId);

        Task<IDataResult<DirectorDto>> GetByDirectorNameAsync(string directorName);

        Task<IDataResult<DirectorListDto>> GetAllAsync();

        Task<IDataResult<DirectorListDto>> GetAllActiveAsync();

        Task<IDataResult<DirectorDto>> GetByMovieNameAsync(string movieName);

        Task<IDataResult<DirectorDto>> GetByMovieIdAsync(int movieId);

        Task<IDataResult<DirectorDto>> GetOrCreateByNameAsync(string directorName);

        Task<IResult> AddAsync(DirectorAddDto directorDto);

        Task<IResult> AutoAddAsync(DirectorAutoCreateDto directorAutoCreateDto);

        Task<IResult> UpdateAsync(DirectorUpdateDto directorUpdateDto);

        Task<IResult> DeleteAsync(int directorId, string modifiedByName);

        Task<IResult> HardDeleteAsync(int directorId);

        Task<IDataResult<DirectorDto>> ApplyPatchAsync(int id, JsonPatchDocument<Director> jsonPatchDocument);
        Task<IDataResult<MovieAddDto>> MapDirector(MovieAddDto movieAddDto);
    }
}