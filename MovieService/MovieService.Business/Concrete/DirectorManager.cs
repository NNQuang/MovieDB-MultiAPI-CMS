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
using System.Threading.Tasks;

namespace MovieService.Business.Concrete
{
    public class DirectorManager : IDirectorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DirectorManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult> AddAsync(DirectorAddDto directorDto)
        {
            var director = _mapper.Map<Director>(directorDto);
            director.Movies = new List<Movie>();
            await _unitOfWork.Directors.AddAsync(director);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                return new Result(true, $"{directorDto.FullName} added.");
            }
            return new Result(false, "Something went wrong when create process.");
        }

        public async Task<IResult> DeleteAsync(int directorId, string modifiedByName)
        {
            var director = await _unitOfWork.Directors.GetAsync(a => a.Id == directorId);
            if (director != null)
            {
                director.IsDeleted = true;
                director.IsActive = false;
                director.ModifiedByName = modifiedByName;
                director.ModifiedDate = DateTime.Now;
                await _unitOfWork.Directors.UpdateAsync(director);
                await _unitOfWork.SaveAsync();
                return new Result(true, $"{director.FullName} deleted.");
            }
            return new Result(false, "Director is not found");
        }

        public async Task<IDataResult<DirectorListDto>> GetAllActiveAsync()
        {
            var activeDirectors = await _unitOfWork.Directors.GetAllAsync(a => a.IsActive == true && a.IsDeleted == false);
            return new DataResult<DirectorListDto>(new DirectorListDto { Directors = activeDirectors }, true);
        }

        public async Task<IDataResult<DirectorListDto>> GetAllAsync()
        {
            var directors = await _unitOfWork.Directors.GetAllAsync();
            return new DataResult<DirectorListDto>(new DirectorListDto { Directors = directors }, true);
        }

        public async Task<IDataResult<DirectorDto>> GetByMovieIdAsync(int movieId)
        {
            var movie = await _unitOfWork.Movies.GetAsync(m => m.Id == movieId && m.IsActive == true && m.IsDeleted == false, m => m.Director);
            if (movie != null)
            {
                var directorByMovieId = movie.Director;
                return new DataResult<DirectorDto>(new DirectorDto { Director = directorByMovieId }, true);
            }
            return new DataResult<DirectorDto>(null, false, $"{movieId} is not found");
        }

        public async Task<IDataResult<DirectorDto>> GetByMovieNameAsync(string movieName)
        {
            var movie = await _unitOfWork.Movies.GetAsync(m => m.MovieTitle == movieName, m => m.Director);
            if (movie != null)
            {
                var directorByMovieName = movie.Director;
                return new DataResult<DirectorDto>(new DirectorDto { Director = directorByMovieName }, true);
            }
            return new DataResult<DirectorDto>(null, false, $"{movieName} is not found");
        }

        public async Task<IDataResult<DirectorDto>> GetByDirectorIdAsync(int directorId)
        {
            var director = await _unitOfWork.Directors.GetAsync(a => a.Id == directorId, a => a.Movies);
            if (director != null)
            {
                return new DataResult<DirectorDto>(new DirectorDto { Director = director }, true);
            }
            return new DataResult<DirectorDto>(null, false, $"{directorId} is not found");
        }

        public async Task<IDataResult<DirectorDto>> GetByDirectorNameAsync(string directorName)
        {
            var director = await _unitOfWork.Directors.GetAsync(a => a.FullName == directorName && a.IsActive == true && a.IsDeleted == false, a => a.Movies);
            if (director != null)
            {
                return new DataResult<DirectorDto>(new DirectorDto { Director = director }, true);
            }
            return new DataResult<DirectorDto>(null, false, $"{directorName} is not found");
        }

        public async Task<IResult> HardDeleteAsync(int directorId)
        {
            var director = await _unitOfWork.Directors.GetAsync(a => a.Id == directorId);
            if (director != null)
            {
                await _unitOfWork.Directors.DeleteAsync(director);
                await _unitOfWork.SaveAsync();
                return new Result(true, $"{directorId} is deleted");
            }
            return new Result(false, $"{directorId} is not found");
        }

        public async Task<IResult> UpdateAsync(DirectorUpdateDto directorUpdateDto)
        {
            var oldDirector = await _unitOfWork.Directors.GetAsync(a => a.Id == directorUpdateDto.Id, a => a.Movies);
            var newDirector = _mapper.Map<DirectorUpdateDto, Director>(directorUpdateDto, oldDirector);
            newDirector.ModifiedDate = DateTime.Now;
            try
            {
                await _unitOfWork.Directors.UpdateAsync(newDirector);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                return new Result(false, "Something went wrong when update process.");
            }

            return new Result(true, $"{directorUpdateDto.FullName} is updated");
        }

        public async Task<IDataResult<DirectorDto>> ApplyPatchAsync(int id, JsonPatchDocument<Director> jsonPatchDocument)
        {
            var director = await _unitOfWork.Directors.GetAsync(d => d.Id == id && d.IsActive == true && d.IsDeleted == false);
            if (director != null)
            {
                jsonPatchDocument.ApplyTo(director);
                await _unitOfWork.SaveAsync();
                return new DataResult<DirectorDto>(new DirectorDto { Director = director }, true, $"{director.FullName} is updated.");
            }
            return new DataResult<DirectorDto>(null, false, "Director is not found");
        }

        public async Task<IDataResult<DirectorDto>> GetOrCreateByNameAsync(string directorName)
        {
            var director = await _unitOfWork.Directors.GetAsync(a => a.FullName == directorName, a => a.Movies);
            if (director != null)
            {
                return new DataResult<DirectorDto>(new DirectorDto { Director = director }, true);
            }
            var autoCreateResult = await AutoAddAsync(new DirectorAutoCreateDto { CreatedByName = "AutoCreate", FullName = directorName });
            if (autoCreateResult.Success)
            {
                return await GetByDirectorNameAsync(directorName);
            }
            return new DataResult<DirectorDto>(null, false, "Something went wrong.");
        }

        public async Task<IResult> AutoAddAsync(DirectorAutoCreateDto directorAutoCreateDto)
        {
            var director = _mapper.Map<Director>(directorAutoCreateDto);
            await _unitOfWork.Directors.AddAsync(director);
            await _unitOfWork.SaveAsync();
            return new Result(true, $"{director.FullName} added.");
        }

        public async Task<IDataResult<MovieAddDto>> MapDirector(MovieAddDto movieAddDto)
        {
            if (string.IsNullOrEmpty(movieAddDto.DirectorString))
            {
                return new DataResult<MovieAddDto>(movieAddDto, false, "Director string is empty.");
            }
            var result = await GetOrCreateByNameAsync(movieAddDto.DirectorString);
            if (result.Success)
            {
                movieAddDto.DirectorId = result.Data.Director.Id;
                return new DataResult<MovieAddDto>(movieAddDto, true, "Director is mapped.");
            }
            return new DataResult<MovieAddDto>(movieAddDto, false, "Something went wrong when adding Director.");
        }
    }
}