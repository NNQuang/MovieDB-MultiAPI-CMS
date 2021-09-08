using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ClientService.Areas.Movie.Models;
using AutoMapper;
using ClientService.Areas.Admin.Models;
using ClientService.Helpers;
using ClientService.Helpers.Image;
using ClientService.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace ClientService.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ActorController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IImageHelper _imageHelper;
        private readonly IHttpClientFactory _clientFactory;
        private static HttpClient client;

        public ActorController(IMapper mapper, IImageHelper imageHelper, IHttpClientFactory clientFactory)
        {
            _mapper = mapper;
            _imageHelper = imageHelper;
            _clientFactory = clientFactory;
            client = _clientFactory.CreateClient("movie");
        }
        public async Task<IActionResult> Index()
        {
            HttpRequestMessage requestAllActorsMessage = new HttpRequestMessage(HttpMethod.Get, "actors/getall");
            requestAllActorsMessage.Headers.Accept.Clear();
            requestAllActorsMessage.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage responseAllActorsMessage = await client.SendAsync(requestAllActorsMessage);

            if (responseAllActorsMessage.IsSuccessStatusCode)
            {
                var data = responseAllActorsMessage.Content.ReadAsStringAsync().Result;
                dynamic d_data = JsonConvert.DeserializeObject<dynamic>(data).actors;
                string actorData = Convert.ToString(d_data);
                return View("_ActorListView", actorData);
            }
            return NotFound(responseAllActorsMessage.ReasonPhrase);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(string actorId)
        {
            HttpRequestMessage requestActor = new HttpRequestMessage(HttpMethod.Get, "actors");
            requestActor.Headers.Add("ActorId", actorId);
            requestActor.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpRequestMessage requestMovies = new HttpRequestMessage(HttpMethod.Get, "movies/getallactive");
            requestMovies.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage responseActor = await client.SendAsync(requestActor);
            HttpResponseMessage responseMovies = await client.SendAsync(requestMovies);

            if (responseActor.IsSuccessStatusCode && responseMovies.IsSuccessStatusCode)
            {
                ActorUpdateDto actorUpdateDto = await responseActor.Content.ReadAsAsync<ActorUpdateDto>();
                MovieListModel movieListModel = await responseMovies.Content.ReadAsAsync<MovieListModel>();
                ActorMovieViewModel actorMovieViewModel = new ActorMovieViewModel { ActorUpdateDto = actorUpdateDto, AllMovies = movieListModel };
                return View("_ActorUpdateView", actorMovieViewModel);
            }

            return NotFound();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ActorMovieViewModel actorMovieViewModel, int[] movieIdArray)
        {
            if (ModelState.IsValid)
            {
                if (actorMovieViewModel.ActorUpdateDto.PictureFile != null)
                {
                    var imageUploadResult = await _imageHelper.UploadImage(actorMovieViewModel.ActorUpdateDto.FullName, actorMovieViewModel.ActorUpdateDto.PictureFile, "Actor");
                    actorMovieViewModel.ActorUpdateDto.PictureUrl = imageUploadResult.Success ? imageUploadResult.FullName : "img/Actor/defaultActor.png";
                }
                actorMovieViewModel.ActorUpdateDto.MovieIdArray = movieIdArray;
                HttpRequestMessage actorUpdateRequest = new HttpRequestMessage(HttpMethod.Post, "actors/update");
                actorUpdateRequest.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
                var actorUpdateJson = JsonConvert.SerializeObject(actorMovieViewModel.ActorUpdateDto);
                actorUpdateRequest.Content = new StringContent(actorUpdateJson, Encoding.UTF8, "application/json");
                HttpResponseMessage actorUpdateResponse = await client.SendAsync(actorUpdateRequest);
                if (actorUpdateResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return NotFound();

            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            HttpRequestMessage requestAllMoviesMessage = new HttpRequestMessage(HttpMethod.Get, "movies/getallactive");
            requestAllMoviesMessage.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage responseAllMoviesMessage = await client.SendAsync(requestAllMoviesMessage);
            if (responseAllMoviesMessage.IsSuccessStatusCode)
            {
                MovieListModel allMovies = await responseAllMoviesMessage.Content.ReadAsAsync<MovieListModel>();
                ActorAddDto actorAddDto = new ActorAddDto();
                return View("_ActorCreateView", Tuple.Create(actorAddDto, allMovies));
            }

            return View("_ActorCreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix = "Item1")] ActorAddDto actorAddDto)
        {
            if (ModelState.IsValid)
            {
                var imageUploadResult = await _imageHelper.UploadImage(actorAddDto.FullName, actorAddDto.PictureFile, "Actor");
                actorAddDto.PictureUrl = imageUploadResult.Success ? imageUploadResult.FullName : "img/Actor/defaultActor.png";

                HttpRequestMessage actorCreateRequest = new HttpRequestMessage(HttpMethod.Post, "actors/create");
                var actorCreateJson = JsonConvert.SerializeObject(actorAddDto);
                actorCreateRequest.Content = new StringContent(actorCreateJson, Encoding.UTF8, "application/json");
                actorCreateRequest.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
                HttpResponseMessage actorCreateResponse = await client.SendAsync(actorCreateRequest);

                if (actorCreateResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Fail");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string actorId)
        {
            HttpRequestMessage actorDeleteRequest = new HttpRequestMessage(HttpMethod.Post, "actors/delete");
            actorDeleteRequest.Headers.Add("ActorId", actorId);
            actorDeleteRequest.Headers.Add("ModifiedByName", Request.HttpContext.User.Identity.Name);
            actorDeleteRequest.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage actorDeleteResponse = await client.SendAsync(actorDeleteRequest);
            if (actorDeleteResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
