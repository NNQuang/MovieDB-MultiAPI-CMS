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
    public class ActorManager : IActorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ActorManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult> AddAsync(ActorAddDto actorDto)
        {
            var actor = _mapper.Map<Actor>(actorDto);
            actor.Movies = new List<Movie>();
            if (actorDto.MovieIdArray != null)
            {
                foreach (int movieId in actorDto.MovieIdArray)
                {
                    var movie = await _unitOfWork.Movies.GetAsync(m => m.Id == movieId);
                    actor.Movies.Add(movie);
                }
            }
            await _unitOfWork.Actors.AddAsync(actor);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                return new Result(true, $"{actor.FullName} added.");
            }
            return new Result(false, "Something went wrong when create process.");
        }

        public async Task<IResult> AutoAddAsync(ActorAutoCreateDto actorAutoCreateDto)
        {
            var actor = _mapper.Map<Actor>(actorAutoCreateDto);
            await _unitOfWork.Actors.AddAsync(actor);
            await _unitOfWork.SaveAsync();
            return new Result(true, $"{actor.FullName} added.");
        }

        public async Task<IResult> DeleteAsync(int actorId, string modifiedByName)
        {
            var actor = await _unitOfWork.Actors.GetAsync(a => a.Id == actorId);
            if (actor != null)
            {
                actor.IsDeleted = true;
                actor.IsActive = false;
                actor.ModifiedByName = modifiedByName;
                actor.ModifiedDate = DateTime.Now;
                await _unitOfWork.Actors.UpdateAsync(actor);
                await _unitOfWork.SaveAsync();
                return new Result(true, $"{actor.FullName} deleted.");
            }
            return new Result(false, "Actor is not found");
        }

        public async Task<IDataResult<ActorListDto>> GetAllActiveAsync()
        {
            var activeActors = await _unitOfWork.Actors.GetAllAsync(a => a.IsActive == true && a.IsDeleted == false);
            return new DataResult<ActorListDto>(new ActorListDto { Actors = activeActors }, true);
        }

        public async Task<IDataResult<ActorListDto>> GetAllAsync()
        {
            var actors = await _unitOfWork.Actors.GetAllAsync();
            return new DataResult<ActorListDto>(new ActorListDto { Actors = actors }, true);
        }

        public async Task<IDataResult<ActorListDto>> GetAllByMovieIdAsync(int movieId)
        {
            var movie = await _unitOfWork.Movies.GetAsync(m => m.Id == movieId && m.IsDeleted == false && m.IsActive == true, m => m.Actors);
            if (movie != null)
            {
                var actorsByMovieId = movie.Actors.Where(a => a.IsDeleted == false && a.IsActive == true);
                return new DataResult<ActorListDto>(new ActorListDto { Actors = actorsByMovieId.ToList() }, true);
            }
            return new DataResult<ActorListDto>(null, false, $"{movieId} is not found");
        }

        public async Task<IDataResult<ActorListDto>> GetAllByMovieNameAsync(string movieName)
        {
            var movie = await _unitOfWork.Movies.GetAsync(m => m.MovieTitle == movieName && m.IsDeleted == false && m.IsActive == true, m => m.Actors);
            if (movie != null)
            {
                var actorsByMovieName = movie.Actors.Where(a => a.IsDeleted == false && a.IsActive == true);
                return new DataResult<ActorListDto>(new ActorListDto { Actors = actorsByMovieName.ToList() }, true);
            }
            return new DataResult<ActorListDto>(null, false, $"{movieName} is not found");
        }

        public async Task<IDataResult<ActorDto>> GetByActorIdAsync(int actorId)
        {
            var actor = await _unitOfWork.Actors.GetAsync(a => a.Id == actorId, a => a.Movies);
            if (actor != null)
            {
                return new DataResult<ActorDto>(new ActorDto { Actor = actor }, true);
            }
            return new DataResult<ActorDto>(null, false, $"{actorId} is not found");
        }

        public async Task<IDataResult<ActorDto>> GetByActorNameAsync(string actorName)
        {
            var actor = await _unitOfWork.Actors.GetAsync(a => a.FullName == actorName, a => a.Movies);
            if (actor != null)
            {
                return new DataResult<ActorDto>(new ActorDto { Actor = actor }, true);
            }
            return new DataResult<ActorDto>(null, false, $"{actorName} is not found");
        }

        public async Task<IDataResult<ActorDto>> GetOrCreateByNameAsync(string actorName)
        {
            var actor = await _unitOfWork.Actors.GetAsync(a => a.FullName == actorName/*, a => a.Movies*/);
            if (actor != null)
            {
                return new DataResult<ActorDto>(new ActorDto { Actor = actor }, true);
            }
            var autoCreateResult = await AutoAddAsync(new ActorAutoCreateDto { CreatedByName = "AutoCreate", FullName = actorName });
            if (autoCreateResult.Success)
            {
                return await GetByActorNameAsync(actorName);
            }
            return new DataResult<ActorDto>(null, false, "Something went wrong.");
        }

        public async Task<IResult> HardDeleteAsync(int actorId)
        {
            var actor = await _unitOfWork.Actors.GetAsync(a => a.Id == actorId);
            if (actor != null)
            {
                await _unitOfWork.Actors.DeleteAsync(actor);
                await _unitOfWork.SaveAsync();
                return new Result(true, $"{actorId} is deleted");
            }
            return new Result(false, $"{actorId} is not found");
        }

        public async Task<IResult> UpdateAsync(ActorUpdateDto actorUpdateDto)
        {
            var oldActor = await _unitOfWork.Actors.GetAsync(a => a.Id == actorUpdateDto.Id, a => a.Movies);
            var newActor = _mapper.Map<ActorUpdateDto, Actor>(actorUpdateDto, oldActor);
            newActor.Movies = new List<Movie>();
            if (actorUpdateDto.MovieIdArray != null)
            {
                foreach (int item in actorUpdateDto.MovieIdArray)
                {
                    var movie = await _unitOfWork.Movies.GetAsync(m => m.Id == item);
                    newActor.Movies.Add(movie);
                }
            }
            newActor.ModifiedDate = DateTime.Now;
            try
            {
                await _unitOfWork.Actors.UpdateAsync(newActor);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                return new Result(false, "Something went wrong when update process.");
            }
            return new Result(true, $"{actorUpdateDto.FullName} is updated");
        }

        public async Task<IDataResult<ActorDto>> ApplyPatchAsync(int id, JsonPatchDocument<Actor> jsonPatchDocument)
        {
            var actor = await _unitOfWork.Actors.GetAsync(a => a.Id == id && a.IsActive == true && a.IsDeleted == false);
            if (actor != null)
            {
                jsonPatchDocument.ApplyTo(actor);
                await _unitOfWork.SaveAsync();
                return new DataResult<ActorDto>(new ActorDto { Actor = actor }, true, $"{actor.FullName} is updated.");
            }
            return new DataResult<ActorDto>(null, false, "Actor is not found.");
        }

        public async Task<IDataResult<MovieAddDto>> MapActors(MovieAddDto movieAddDto)
        {
            if (string.IsNullOrEmpty(movieAddDto.ActorsString))
            {
                return new DataResult<MovieAddDto>(movieAddDto, false, "Actors string is null.");
            }
            movieAddDto.Actors = new List<Actor>();
            string[] actors = movieAddDto.ActorsString.Split(",");
            foreach (string item in actors)
            {
                var result = await GetOrCreateByNameAsync(item.Trim());
                movieAddDto.Actors.Add(result.Data.Actor);
            }
            if (movieAddDto.Actors == null)
            {
                return new DataResult<MovieAddDto>(movieAddDto, false, "Something went wrong when adding actors.");
            }
            return new DataResult<MovieAddDto>(movieAddDto, true, "Actors Added.");
        }
    }
}