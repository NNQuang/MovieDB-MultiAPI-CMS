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
    public class MovieManager : IMovieService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActorService _actorService;
        private readonly IGenreService _genreService;
        private readonly IDirectorService _directorService;

        public MovieManager(IUnitOfWork unitOfWork, IMapper mapper, IActorService actorService, IGenreService genreService, IDirectorService directorService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _actorService = actorService;
            _genreService = genreService;
            _directorService = directorService;
        }

        public async Task<IDataResult<MovieDto>> AddAsync(MovieAddDto movieAddDto) 
        {
            //WEBAPI ==> actorString = "Actor NameOne, Actor NameTwo" => her isim için varsa getir yoksa oluştur.
            var mappingActorsResult = await _actorService.MapActors(movieAddDto);
            var mappingDirectorsResult = await _directorService.MapDirector(movieAddDto);
            var mappingGenresResult = await _genreService.MapGenres(movieAddDto);
            var movie = _mapper.Map<Movie>(movieAddDto);
            await _unitOfWork.Movies.AddAsync(movie);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                return new DataResult<MovieDto>(new MovieDto { Movie = movie }, true, $"{movieAddDto.MovieTitle} is added.");
            }
            return new DataResult<MovieDto>(null, false, $"Something went wrong for {movieAddDto.MovieTitle}. {mappingActorsResult.Message}, {mappingDirectorsResult.Message}, {mappingGenresResult.Message}");
        }

        public async Task<IResult> DeleteAsync(int movieId, string modifiedByName)
        {
            var movie = await _unitOfWork.Movies.GetAsync(m => m.Id == movieId);
            if (movie != null)
            {
                movie.IsDeleted = true;
                movie.IsActive = false;
                movie.ModifiedByName = modifiedByName;
                movie.ModifiedDate = DateTime.Now;
                movie.IsActive = false;
                await _unitOfWork.Movies.UpdateAsync(movie);
                await _unitOfWork.SaveAsync();
                return new Result(true, $"{movie.MovieTitle} is deleted. ");
            }
            return new Result(false, $"{movieId} is not found.");
        }

        public async Task<IDataResult<MovieListDto>> GetAllByActorId(int actorId)
        {
            var actor = await _unitOfWork.Actors.GetAsync(a => a.Id == actorId && a.IsActive == true && a.IsDeleted == false, a => a.Movies);
            if (actor != null)
            {
                var moviesByActorId = actor.Movies.Where(m => m.IsDeleted == false && m.IsActive == true);
                return new DataResult<MovieListDto>(new MovieListDto { Movies = moviesByActorId.ToList(), Note = actor.FullName }, true);
            }
            return new DataResult<MovieListDto>(null, false, $"{actorId} is not found.");
        }

        public async Task<IDataResult<MovieListDto>> GetAllByActorName(string actorName)
        {
            var actor = await _unitOfWork.Actors.GetAsync(a => a.FullName == actorName && a.IsActive == true && a.IsDeleted == false, a => a.Movies);
            if (actor != null)
            {
                var moviesByActorName = actor.Movies.Where(m => m.IsDeleted == false && m.IsActive == true);
                return new DataResult<MovieListDto>(new MovieListDto { Movies = moviesByActorName.ToList(), Note = actorName }, true);
            }
            return new DataResult<MovieListDto>(null, false, $"{actorName} is not found.");
        }

        public async Task<IDataResult<MovieListDto>> GetAllByDirectorId(int directorId)
        {
            var director = await _unitOfWork.Directors.GetAsync(d => d.Id == directorId && d.IsActive == true && d.IsDeleted == false, d => d.Movies);
            if (director != null)
            {
                var moviesByDirectorId = director.Movies.Where(m => m.IsDeleted == false && m.IsActive == true);
                return new DataResult<MovieListDto>(new MovieListDto { Movies = moviesByDirectorId.ToList(), Note = director.FullName }, true);
            }
            return new DataResult<MovieListDto>(null, false, $"{directorId} is not found.");
        }

        public async Task<IDataResult<MovieListDto>> GetAllByDirectorName(string directorName)
        {
            var director = await _unitOfWork.Directors.GetAsync(d => d.FullName == directorName && d.IsActive == true && d.IsDeleted == false, d => d.Movies);
            if (director != null)
            {
                var moviesByDirectorName = director.Movies.Where(m => m.IsDeleted == false && m.IsActive == true);
                return new DataResult<MovieListDto>(new MovieListDto { Movies = moviesByDirectorName.ToList(), Note = directorName }, true);
            }
            return new DataResult<MovieListDto>(null, false, $"{directorName} is not found.");
        }

        public async Task<IDataResult<MovieListDto>> GetAllByGenreId(int genreId)
        {
            var genre = await _unitOfWork.Genres.GetAsync(g => g.Id == genreId && g.IsActive == true && g.IsDeleted == false, g => g.Movies);
            if (genre != null)
            {
                var moviesByGenreId = genre.Movies.Where(m => m.IsDeleted == false && m.IsActive == true);
                return new DataResult<MovieListDto>(new MovieListDto { Movies = moviesByGenreId.ToList(), Note = genre.GenreName }, true);
            }
            return new DataResult<MovieListDto>(null, false, $"{genreId} is not found.");
        }

        public async Task<IDataResult<MovieListDto>> GetAllByGenreName(string genreName)
        {
            var genre = await _unitOfWork.Genres.GetAsync(g => g.GenreName == genreName && g.IsActive == true && g.IsDeleted == false, g => g.Movies);
            if (genre != null)
            {
                var moviesByGenreName = genre.Movies.Where(m => m.IsDeleted == false && m.IsActive == true);
                return new DataResult<MovieListDto>(new MovieListDto { Movies = moviesByGenreName.ToList(), Note = genre.GenreName }, true);
            }
            return new DataResult<MovieListDto>(null, false, $"{genreName} is not found.");
        }

        public async Task<IDataResult<MovieListDto>> GetAllByGreaterThanImdbRating(double imdbRating)
        {
            var movies = await _unitOfWork.Movies.GetAllAsync(m => m.ImdbRating >= imdbRating && m.IsDeleted == false && m.IsActive == true);
            return new DataResult<MovieListDto>(new MovieListDto { Movies = movies }, true);
        }

        public async Task<IDataResult<MovieListDto>> GetAllByGreaterThanRottenTomatoesRating(double rottenTomatoesRating)
        {
            var movies = await _unitOfWork.Movies.GetAllAsync(m => m.RottenTomatoesRating >= rottenTomatoesRating && m.IsDeleted == false && m.IsActive == true);
            return new DataResult<MovieListDto>(new MovieListDto { Movies = movies }, true);
        }

        public async Task<IDataResult<MovieListDto>> GetAllByLessGreaterMetaCriticRating(double metaCriticRating)
        {
            var movies = await _unitOfWork.Movies.GetAllAsync(m => m.MetaCriticRating <= metaCriticRating && m.IsDeleted == false && m.IsActive == true);
            return new DataResult<MovieListDto>(new MovieListDto { Movies = movies }, true);
        }

        public async Task<IDataResult<MovieListDto>> GetAllByLessThanImdbRating(double imdbRating)
        {
            var movies = await _unitOfWork.Movies.GetAllAsync(m => m.ImdbRating <= imdbRating && m.IsDeleted == false && m.IsActive == true);
            return new DataResult<MovieListDto>(new MovieListDto { Movies = movies }, true);
        }

        public async Task<IDataResult<MovieListDto>> GetAllByLessThanMetaCriticRating(double metaCriticRating)
        {
            var movies = await _unitOfWork.Movies.GetAllAsync(m => m.MetaCriticRating <= metaCriticRating && m.IsDeleted == false && m.IsActive == true);
            return new DataResult<MovieListDto>(new MovieListDto { Movies = movies }, true);
        }

        public async Task<IDataResult<MovieListDto>> GetAllByLessThanRottenTomatoesRating(double rottenTomatoesRating)
        {
            var movies = await _unitOfWork.Movies.GetAllAsync(m => m.RottenTomatoesRating <= rottenTomatoesRating && m.IsDeleted == false && m.IsActive == true);
            return new DataResult<MovieListDto>(new MovieListDto { Movies = movies }, true);
        }

        public async Task<IDataResult<MovieDto>> GetByMovieIdAsync(int movieId)
        {
            var movie = await _unitOfWork.Movies.GetAsync(m => m.Id == movieId, m => m.Actors, m => m.Director, m => m.Genres);
            if (movie != null)
            {
                return new DataResult<MovieDto>(new MovieDto { Movie = movie }, true);
            }
            return new DataResult<MovieDto>(null, false, $"{movieId} is not found.");
        }

        public async Task<IDataResult<MovieDto>> GetByMovieNameAsync(string movieName)
        {
            var movie = await _unitOfWork.Movies.GetAsync(m => m.MovieTitle == movieName, m => m.Actors, m => m.Director, m => m.Genres);
            if (movie != null)
            {
                return new DataResult<MovieDto>(new MovieDto { Movie = movie }, true);
            }
            return new DataResult<MovieDto>(null, false, $"{movieName} is not found.");
        }

        public async Task<IResult> HardDeleteAsync(int movieId)
        {
            var movie = await _unitOfWork.Movies.GetAsync(m => m.Id == movieId);
            if (movie != null)
            {
                await _unitOfWork.Movies.DeleteAsync(movie);
                await _unitOfWork.SaveAsync();
                return new Result(true, $"{movie.MovieTitle} is deleted.");
            }
            return new Result(false, $"{movie.MovieTitle} is not found.");
        }

        public async Task<IDataResult<MovieListDto>> SearchByMovieNameAsync(string movieName)
        {
            var movies = await _unitOfWork.Movies.GetAllAsync(m => m.MovieTitle.ToLower().Contains(movieName.ToLower()));
            if (movies.Count() > -1)
            {
                var moviesBySearch = movies.Where(m => m.IsActive == true && m.IsDeleted == false);
                return new DataResult<MovieListDto>(new MovieListDto { Movies = moviesBySearch.ToList(), Note = movieName }, true);
            }
            return new DataResult<MovieListDto>(null, false, $"There is no movie called {movieName}");
        }

        public async Task<IResult> UpdateAsync(MovieUpdateDto movieUpdateDto)
        {
            //AutoMapper burada fazladan bir instance oluşturuyor ve movie Id duplicate olduğu için database update işlemi yapılamıyor.
            var oldMovie = await _unitOfWork.Movies.GetAsync(m => m.Id == movieUpdateDto.Id, m => m.Actors, m => m.Genres);
            oldMovie.DirectorId = movieUpdateDto.DirectorId;
            oldMovie.Duration = movieUpdateDto.Duration;
            oldMovie.ImdbId = movieUpdateDto.ImdbId;
            oldMovie.ImdbRating = movieUpdateDto.ImdbRating;
            oldMovie.IsActive = movieUpdateDto.IsActive;
            oldMovie.IsDeleted = movieUpdateDto.IsDeleted;
            oldMovie.Language = movieUpdateDto.Language;
            oldMovie.MetaCriticRating = movieUpdateDto.MetaCriticRating;
            oldMovie.ModifiedByName = movieUpdateDto.ModifiedByName;
            oldMovie.ModifiedDate = DateTime.Now;
            oldMovie.MovieTitle = movieUpdateDto.MovieTitle;
            oldMovie.PictureUrl = movieUpdateDto.PictureUrl;
            oldMovie.Plot = movieUpdateDto.Plot;
            oldMovie.ReleaseDate = movieUpdateDto.ReleaseDate;
            oldMovie.RottenTomatoesRating = movieUpdateDto.RottenTomatoesRating;

            oldMovie.Actors = new List<Actor>();
            oldMovie.Genres = new List<Genre>();

            if (movieUpdateDto.ActorIdArray != null)
            {
                foreach (int item in movieUpdateDto.ActorIdArray)
                {
                    var actor = await _unitOfWork.Actors.GetAsync(a => a.Id == item);
                    oldMovie.Actors.Add(actor);
                }
            }


            if (movieUpdateDto.GenreIdArray != null)
            {
                foreach (int item in movieUpdateDto.GenreIdArray)
                {
                    var genre = await _unitOfWork.Genres.GetAsync(g => g.Id == item);
                    oldMovie.Genres.Add(genre);
                }
            }

            oldMovie.ModifiedDate = DateTime.Now;
            try
            {
                await _unitOfWork.Movies.UpdateAsync(oldMovie);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                return new Result(false, "Something went wrong when update process.");
            }
            return new Result(true, $"{oldMovie.MovieTitle} updated.");
        }

        public async Task<IResult> UpdateRatingsAsync(MovieRatingUpdateDto movieRatings) // Bu method yerine Aşağıdaki ApplyPatch methodu daha mantıklı.
        {
            var movie = await _unitOfWork.Movies.GetAsync(m => m.MovieTitle == movieRatings.MovieTitle);
            if (movie != null)
            {
                if (movieRatings.ImdbRating > 0)
                {
                    movie.ImdbRating = movieRatings.ImdbRating;
                }
                if (movieRatings.RottenTomatoesRating > 0)
                {
                    movie.RottenTomatoesRating = movieRatings.RottenTomatoesRating;
                }
                if (movieRatings.MetaCriticRating > 0)
                {
                    movie.MetaCriticRating = movieRatings.MetaCriticRating;
                }
                movie.ModifiedDate = DateTime.Now;
                await _unitOfWork.Movies.UpdateAsync(movie);
                await _unitOfWork.SaveAsync();
                return new Result(true, $"{movieRatings.MovieTitle} is updated with ratings.");
            }
            return new Result(false, $"{movieRatings.MovieTitle} is not found.");
        }

        public async Task<IDataResult<MovieDto>> ApplyPatchAsync(int id, JsonPatchDocument<Movie> jsonPatchDocument)
        {
            var movie = await _unitOfWork.Movies.GetAsync(m => m.Id == id && m.IsActive == true && m.IsDeleted == false);
            if (movie != null)
            {
                jsonPatchDocument.ApplyTo(movie);
                movie.ModifiedDate = DateTime.Now;
                await _unitOfWork.SaveAsync();
                return new DataResult<MovieDto>(new MovieDto { Movie = movie }, true, $"{movie.MovieTitle} is updated.");
            }
            return new DataResult<MovieDto>(null, false, $"Movie is not found.");
        }

        public async Task<IDataResult<MovieListDto>> GetAllAsync()
        {
            var movies = await _unitOfWork.Movies.GetAllAsync();
            return new DataResult<MovieListDto>(new MovieListDto { Movies = movies }, true);
        }

        public async Task<IDataResult<MovieListDto>> GetAllActiveAsync()
        {
            var movies = await _unitOfWork.Movies.GetAllAsync(m => m.IsActive == true && m.IsDeleted == false/*, m => m.Actors, m => m.Director*/);
            return new DataResult<MovieListDto>(new MovieListDto { Movies = movies }, true);
        }

    }
}