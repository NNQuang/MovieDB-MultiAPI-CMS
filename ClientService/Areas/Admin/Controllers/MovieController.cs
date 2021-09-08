using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ClientService.Areas.Admin.Models;
using ClientService.Areas.Admin.ViewModels;
using ClientService.Areas.Movie.Models;
using ClientService.Helpers.Image;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MovieController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IImageHelper _imageHelper;
        private readonly IHttpClientFactory _clientFactory;
        private static HttpClient client;

        public MovieController(IMapper mapper, IImageHelper imageHelper, IHttpClientFactory clientFactory)
        {
            _mapper = mapper;
            _imageHelper = imageHelper;
            _clientFactory = clientFactory;
            client = _clientFactory.CreateClient("movie");
        }

        public async Task<IActionResult> Index()
        {
            HttpRequestMessage requestAllMoviesMessage = new HttpRequestMessage(HttpMethod.Get, "movies/getall");
            requestAllMoviesMessage.Headers.Accept.Clear();
            requestAllMoviesMessage.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage responseAllMoviesMessage = await client.SendAsync(requestAllMoviesMessage);
            if (responseAllMoviesMessage.IsSuccessStatusCode)
            {
                //read json response as string
                var data = responseAllMoviesMessage.Content.ReadAsStringAsync().Result;
                dynamic d_data = JsonConvert.DeserializeObject<dynamic>(data).movies;
                string movieData = Convert.ToString(d_data);
                return View("_MovieListView", movieData);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string movieId)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString();
            HttpRequestMessage requestMovie = new HttpRequestMessage(HttpMethod.Get, "movies");
            HttpRequestMessage requestActors = new HttpRequestMessage(HttpMethod.Get, "actors/getallactive");
            HttpRequestMessage requestDirectors = new HttpRequestMessage(HttpMethod.Get, "directors/getallactive");
            HttpRequestMessage requestGenres = new HttpRequestMessage(HttpMethod.Get, "genres/getallactive");

            requestMovie.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            requestMovie.Headers.Add("MovieId", movieId);
            requestActors.Headers.Add("Authorization", token);
            requestDirectors.Headers.Add("Authorization", token);
            requestGenres.Headers.Add("Authorization", token);

            HttpResponseMessage responseMovie = await client.SendAsync(requestMovie);
            HttpResponseMessage responseActors = await client.SendAsync(requestActors);
            HttpResponseMessage responseGenres = await client.SendAsync(requestGenres);
            HttpResponseMessage responseDirectors = await client.SendAsync(requestDirectors);

            if (responseMovie.IsSuccessStatusCode && responseActors.IsSuccessStatusCode && responseGenres.IsSuccessStatusCode && responseDirectors.IsSuccessStatusCode)
            {
                MovieUpdateDto movieUpdateDto = await responseMovie.Content.ReadAsAsync<MovieUpdateDto>();
                ActorListModel allActors = await responseActors.Content.ReadAsAsync<ActorListModel>();
                GenreListModel allGenres = await responseGenres.Content.ReadAsAsync<GenreListModel>();
                DirectorListModel allDirectors = await responseDirectors.Content.ReadAsAsync<DirectorListModel>();

                MovieUpdateDataViewModel movieDataViewModel = new MovieUpdateDataViewModel
                {
                    MovieUpdateDto = movieUpdateDto,
                    AllActors = allActors,
                    AllDirectors = allDirectors,
                    AllGenres = allGenres
                };

                return View("_MovieUpdateView", movieDataViewModel);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(MovieUpdateDataViewModel movieDataModel)
        {
            if (ModelState.IsValid)
            {
                if (movieDataModel.MovieUpdateDto.PictureFile != null)
                {
                    var imageUploadResult = await _imageHelper.UploadImage(movieDataModel.MovieUpdateDto.MovieTitle, movieDataModel.MovieUpdateDto.PictureFile, "Actor");
                    movieDataModel.MovieUpdateDto.PictureUrl = imageUploadResult.Success ? imageUploadResult.FullName : "img/Movie/defaultMovie.png";
                }
                HttpRequestMessage movieUpdateRequest = new HttpRequestMessage(HttpMethod.Post, "movies/update");
                movieUpdateRequest.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
                var movieUpdateJson = JsonConvert.SerializeObject(movieDataModel.MovieUpdateDto);
                movieUpdateRequest.Content = new StringContent(movieUpdateJson, Encoding.UTF8, "application/json");
                HttpResponseMessage movieUpdateResponse = await client.SendAsync(movieUpdateRequest);
                if (movieUpdateResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString();
            HttpRequestMessage requestActors = new HttpRequestMessage(HttpMethod.Get, "actors/getallactive");
            HttpRequestMessage requestGenres = new HttpRequestMessage(HttpMethod.Get, "genres/getallactive");
            HttpRequestMessage requestDirectors = new HttpRequestMessage(HttpMethod.Get, "directors/getallactive");

            requestActors.Headers.Add("Authorization", token);
            requestGenres.Headers.Add("Authorization", token);
            requestDirectors.Headers.Add("Authorization", token);

            HttpResponseMessage responseActors = await client.SendAsync(requestActors);
            HttpResponseMessage responseGenres = await client.SendAsync(requestGenres);
            HttpResponseMessage responseDirectors = await client.SendAsync(requestDirectors);

            if (responseActors.IsSuccessStatusCode && responseGenres.IsSuccessStatusCode && responseDirectors.IsSuccessStatusCode)
            {
                ActorListModel allActors = await responseActors.Content.ReadAsAsync<ActorListModel>();
                GenreListModel allGenres = await responseGenres.Content.ReadAsAsync<GenreListModel>();
                DirectorListModel allDirectors = await responseDirectors.Content.ReadAsAsync<DirectorListModel>();

                MovieAddDataViewModel movieDataAddViewModel = new MovieAddDataViewModel
                {
                    MovieAddDto = new MovieAddDto(),
                    AllActors = allActors,
                    AllDirectors = allDirectors,
                    AllGenres = allGenres
                };

                return View("_MovieCreateView", movieDataAddViewModel);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieAddDataViewModel movieAddDataViewModel)
        {
            if (ModelState.IsValid)
            {
                var imageUploadResult = await _imageHelper.UploadImage(movieAddDataViewModel.MovieAddDto.MovieTitle, movieAddDataViewModel.MovieAddDto.Picture, "Movie");
                movieAddDataViewModel.MovieAddDto.PictureUrl = imageUploadResult.Success ? imageUploadResult.FullName : "img/Movie/defaultActor.png";

                HttpRequestMessage movieCreateRequest = new HttpRequestMessage(HttpMethod.Post, "movies/create");
                var movieCreateJson = JsonConvert.SerializeObject(movieAddDataViewModel.MovieAddDto);
                movieCreateRequest.Content = new StringContent(movieCreateJson, Encoding.UTF8, "application/json");
                movieCreateRequest.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
                HttpResponseMessage movieCreateResponse = await client.SendAsync(movieCreateRequest);

                if (movieCreateResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string movieId)
        {
            HttpRequestMessage movieDeleteRequest = new HttpRequestMessage(HttpMethod.Post, "movies/delete");
            movieDeleteRequest.Headers.Add("MovieId", movieId);
            movieDeleteRequest.Headers.Add("ModifiedByName", Request.HttpContext.User.Identity.Name);
            movieDeleteRequest.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage movieDeleteResponse = await client.SendAsync(movieDeleteRequest);
            if (movieDeleteResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
