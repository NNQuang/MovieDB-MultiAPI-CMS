using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using MovieService.Business.Abstract;
using MovieService.Core.Results.Abstract;
using MovieService.Core.Results.Concrete;
using MovieService.Data.UnitOfWork.Abstract;
using MovieService.Entities.Concrete;
using MovieService.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieService.Business.Concrete
{
    public class GenreManager : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenreManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult> AddAsync(GenreAddDto genreDto)
        {
            var genre = _mapper.Map<Genre>(genreDto);
            genre.Movies = new List<Movie>();
            if (genreDto.MovieIdArray != null)
            {
                foreach (int item in genreDto.MovieIdArray)
                {
                    var movie = await _unitOfWork.Movies.GetAsync(m => m.Id == item);
                    genre.Movies.Add(movie);
                }
            }
            await _unitOfWork.Genres.AddAsync(genre);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                return new Result(true, $"{genreDto.GenreName} added.");
            }
            return new Result(false, "Something went wrong when create process.");
        }

        public async Task<IResult> DeleteAsync(int genreId, string modifiedByName)
        {
            var genre = await _unitOfWork.Genres.GetAsync(g => g.Id == genreId);
            if (genre != null)
            {
                genre.IsDeleted = true;
                genre.IsActive = false;
                genre.ModifiedByName = modifiedByName;
                genre.ModifiedDate = DateTime.Now;
                await _unitOfWork.Genres.UpdateAsync(genre);
                await _unitOfWork.SaveAsync();
                return new Result(true, $"{genre.GenreName} deleted.");
            }
            return new Result(false, "Genre is not found");
        }

        public async Task<IDataResult<GenreListDto>> GetAllActiveAsync()
        {
            var activeGenres = await _unitOfWork.Genres.GetAllAsync(g => g.IsActive == true && g.IsDeleted == false);
            return new DataResult<GenreListDto>(new GenreListDto { Genres = activeGenres }, true);
        }

        public async Task<IDataResult<GenreListDto>> GetAllAsync()
        {
            var genres = await _unitOfWork.Genres.GetAllAsync();
            return new DataResult<GenreListDto>(new GenreListDto { Genres = genres }, true);
        }

        public async Task<IDataResult<GenreListDto>> GetAllByMovieIdAsync(int movieId)
        {
            var movie = await _unitOfWork.Movies.GetAsync(m => m.Id == movieId, m => m.Genres);
            if (movie != null)
            {
                var genresByMovieId = movie.Genres;
                return new DataResult<GenreListDto>(new GenreListDto { Genres = genresByMovieId.ToList() }, true);
            }
            return new DataResult<GenreListDto>(null, false, $"{movieId} is not found");
        }

        public async Task<IDataResult<GenreListDto>> GetAllByMovieNameAsync(string movieName)
        {
            var movie = await _unitOfWork.Movies.GetAsync(m => m.MovieTitle == movieName, m => m.Genres);
            if (movie != null)
            {
                var genresByMovieName = movie.Genres;
                return new DataResult<GenreListDto>(new GenreListDto { Genres = genresByMovieName.ToList() }, true);
            }
            return new DataResult<GenreListDto>(null, false, $"{movieName} is not found");
        }

        public async Task<IDataResult<GenreDto>> GetByGenreIdAsync(int genreId)
        {
            var genre = await _unitOfWork.Genres.GetAsync(g => g.Id == genreId, g => g.Movies);
            if (genre != null)
            {
                return new DataResult<GenreDto>(new GenreDto { Genre = genre }, true);
            }
            return new DataResult<GenreDto>(null, false, $"{genreId} is not found");
        }

        public async Task<IDataResult<GenreDto>> GetByGenreNameAsync(string genreName)
        {
            var genre = await _unitOfWork.Genres.GetAsync(g => g.GenreName == genreName, g => g.Movies);
            if (genre != null)
            {
                return new DataResult<GenreDto>(new GenreDto { Genre = genre }, true);
            }
            return new DataResult<GenreDto>(null, false, $"{genreName} is not found");
        }

        public async Task<IResult> HardDeleteAsync(int genreId)
        {
            var genre = await _unitOfWork.Genres.GetAsync(g => g.Id == genreId);
            if (genre != null)
            {
                await _unitOfWork.Genres.DeleteAsync(genre);
                await _unitOfWork.SaveAsync();
                return new Result(true, $"{genreId} is deleted");
            }
            return new Result(false, $"{genreId} is not found");
        }

        public async Task<IResult> UpdateAsync(GenreUpdateDto genreUpdateDto)
        {
            var oldGenre = await _unitOfWork.Genres.GetAsync(a => a.Id == genreUpdateDto.Id, a => a.Movies);
            var newGenre = _mapper.Map<GenreUpdateDto, Genre>(genreUpdateDto, oldGenre);
            newGenre.Movies = new List<Movie>();
            if (genreUpdateDto.MovieIdArray != null)
            {
                foreach (int item in genreUpdateDto.MovieIdArray)
                {
                    var movie = await _unitOfWork.Movies.GetAsync(m => m.Id == item);
                    newGenre.Movies.Add(movie);
                }
            }
            newGenre.ModifiedDate = DateTime.Now;
            try
            {
                await _unitOfWork.Genres.UpdateAsync(newGenre);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                return new Result(false, "Something went wrong when update process.");
            }
            return new Result(true, $"{genreUpdateDto.GenreName} is updated");
        }

        public async Task<IResult> AutoAddAsync(GenreAddDto genreAutoCreateDto)
        {
            var genre = _mapper.Map<Genre>(genreAutoCreateDto);
            await _unitOfWork.Genres.AddAsync(genre);
            await _unitOfWork.SaveAsync();
            return new Result(true, $"{genre.GenreName} added.");
        }

        public async Task<IDataResult<GenreDto>> GetOrCreateByNameAsync(string genreName)
        {
            var genre = await _unitOfWork.Genres.GetAsync(g => g.GenreName == genreName/*, g => g.Movies*/);
            if (genre != null)
            {
                return new DataResult<GenreDto>(new GenreDto { Genre = genre }, true);
            }
            var autoCreateResult = await AutoAddAsync(new GenreAddDto { CreatedByName = "AutoCreate", GenreName = genreName });
            if (autoCreateResult.Success)
            {
                return await GetByGenreNameAsync(genreName);
            }
            return new DataResult<GenreDto>(null, false, "Something went wrong.");
        }

        public async Task<IDataResult<GenreDto>> ApplyPatchAsync(int id, JsonPatchDocument<Genre> jsonPatchDocument)
        {
            var genre = await _unitOfWork.Genres.GetAsync(g => g.Id == id && g.IsActive == true && g.IsDeleted == false);
            if (genre != null)
            {
                jsonPatchDocument.ApplyTo(genre);
                await _unitOfWork.SaveAsync();
                return new DataResult<GenreDto>(new GenreDto { Genre = genre }, true, $"{genre.GenreName} is updated.");
            }
            return new DataResult<GenreDto>(null, false, "Genre is not found.");
        }

        public async Task<IDataResult<MovieAddDto>> MapGenres(MovieAddDto movieAddDto)
        {
            if (string.IsNullOrEmpty(movieAddDto.GenresString))
            {
                return new DataResult<MovieAddDto>(movieAddDto, false, "Genre string is empty.");
            }
            movieAddDto.Genres = new List<Genre>(); //api ile null yollama riski olduğu için instance aldık.
            string[] genres = movieAddDto.GenresString.Split(",");
            foreach (string item in genres)
            {
                var result = await GetOrCreateByNameAsync(item.Trim());
                movieAddDto.Genres.Add(result.Data.Genre);
            }
            if (movieAddDto.Genres == null)
            {
                return new DataResult<MovieAddDto>(movieAddDto, false, "Something went wrong when adding genres.");
            }
            return new DataResult<MovieAddDto>(movieAddDto, true, "Genres Added.");
        }
    }
}