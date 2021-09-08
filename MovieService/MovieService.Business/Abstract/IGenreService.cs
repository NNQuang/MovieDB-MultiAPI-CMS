using Microsoft.AspNetCore.JsonPatch;
using MovieService.Core.Entities.Abstract;
using MovieService.Core.Results.Abstract;
using MovieService.Entities.Concrete;
using MovieService.Entities.Dtos;
using System.Threading.Tasks;

namespace MovieService.Business.Abstract
{
    public interface IGenreService : IDto
    {
        Task<IDataResult<GenreDto>> GetByGenreIdAsync(int genreId);

        Task<IDataResult<GenreDto>> GetByGenreNameAsync(string genreName);

        Task<IDataResult<GenreListDto>> GetAllAsync();

        Task<IDataResult<GenreListDto>> GetAllActiveAsync();

        Task<IDataResult<GenreListDto>> GetAllByMovieNameAsync(string movieName);

        Task<IDataResult<GenreListDto>> GetAllByMovieIdAsync(int movieId);

        Task<IDataResult<GenreDto>> GetOrCreateByNameAsync(string genreName);

        Task<IResult> AddAsync(GenreAddDto genreDto);

        Task<IResult> AutoAddAsync(GenreAddDto genreAutoCreateDto);

        Task<IResult> UpdateAsync(GenreUpdateDto genreUpdateDto);

        Task<IResult> DeleteAsync(int genreId, string modifiedByName);

        Task<IResult> HardDeleteAsync(int genreId);

        Task<IDataResult<GenreDto>> ApplyPatchAsync(int id, JsonPatchDocument<Genre> jsonPatchDocument);

        Task<IDataResult<MovieAddDto>> MapGenres(MovieAddDto movieAddDto);
    }
}