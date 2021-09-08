using Microsoft.AspNetCore.JsonPatch;
using MovieService.Core.Entities.Abstract;
using MovieService.Core.Results.Abstract;
using MovieService.Entities.Concrete;
using MovieService.Entities.Dtos;
using System.Threading.Tasks;

namespace MovieService.Business.Abstract
{
    public interface IMovieService : IDto
    {
        Task<IDataResult<MovieDto>> GetByMovieIdAsync(int movieId);

        Task<IDataResult<MovieDto>> GetByMovieNameAsync(string movieName);

        Task<IDataResult<MovieListDto>> GetAllAsync();

        Task<IDataResult<MovieListDto>> GetAllActiveAsync();

        Task<IDataResult<MovieListDto>> SearchByMovieNameAsync(string movieName);

        Task<IDataResult<MovieListDto>> GetAllByDirectorId(int directorId);

        Task<IDataResult<MovieListDto>> GetAllByDirectorName(string directorName);

        Task<IDataResult<MovieListDto>> GetAllByActorId(int actorId);

        Task<IDataResult<MovieListDto>> GetAllByActorName(string actorName);

        Task<IDataResult<MovieListDto>> GetAllByGenreId(int genreId);

        Task<IDataResult<MovieListDto>> GetAllByGenreName(string genreName);

        Task<IDataResult<MovieListDto>> GetAllByLessThanImdbRating(double imdbRating);

        Task<IDataResult<MovieListDto>> GetAllByGreaterThanImdbRating(double imdbRating);

        Task<IDataResult<MovieListDto>> GetAllByLessThanRottenTomatoesRating(double rottenTomatoesRating);

        Task<IDataResult<MovieListDto>> GetAllByGreaterThanRottenTomatoesRating(double rottenTomatoesRating);

        Task<IDataResult<MovieListDto>> GetAllByLessThanMetaCriticRating(double metaCriticRating);

        Task<IDataResult<MovieListDto>> GetAllByLessGreaterMetaCriticRating(double metaCriticRating);

        Task<IDataResult<MovieDto>> AddAsync(MovieAddDto movieAddDto);

        Task<IResult> UpdateAsync(MovieUpdateDto movieUpdateDto);

        Task<IResult> UpdateRatingsAsync(MovieRatingUpdateDto movieRatings);

        Task<IResult> DeleteAsync(int movieId, string modifiedByName);

        Task<IResult> HardDeleteAsync(int movieId);

        Task<IDataResult<MovieDto>> ApplyPatchAsync(int id, JsonPatchDocument<Movie> jsonPatchDocument);
    }
}