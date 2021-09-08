using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ClientService.Areas.Movie.Models;
using ClientService.Areas.Admin.Models;
using ClientService.Areas.Admin.ViewModels;
using System.Text;

namespace ClientService.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GenreController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _clientFactory;
        private static HttpClient client;

        public GenreController(IMapper mapper, IHttpClientFactory clientFactory)
        {
            _mapper = mapper;
            _clientFactory = clientFactory;
            client = _clientFactory.CreateClient("movie");
        }
        public async Task<IActionResult> Index()
        {
            HttpRequestMessage requestAllGenresMessage = new HttpRequestMessage(HttpMethod.Get, "genres/getall");
            requestAllGenresMessage.Headers.Accept.Clear();
            requestAllGenresMessage.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage responseAllGenresMessage = await client.SendAsync(requestAllGenresMessage);
            if (responseAllGenresMessage.IsSuccessStatusCode)
            {
                var data = responseAllGenresMessage.Content.ReadAsStringAsync().Result;
                dynamic d_data = JsonConvert.DeserializeObject<dynamic>(data).genres;
                string genreData = Convert.ToString(d_data);
                return View("_GenreListView", genreData);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string genreId)
        {
            HttpRequestMessage requestGenre = new HttpRequestMessage(HttpMethod.Get, "genres");
            requestGenre.Headers.Add("GenreId", genreId);
            requestGenre.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpRequestMessage requestMovies = new HttpRequestMessage(HttpMethod.Get, "movies/getallactive");
            requestMovies.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage responseGenre = await client.SendAsync(requestGenre);
            HttpResponseMessage responseMovies = await client.SendAsync(requestMovies);

            if (responseGenre.IsSuccessStatusCode && responseMovies.IsSuccessStatusCode)
            {
                GenreUpdateDto genreUpdateDto = await responseGenre.Content.ReadAsAsync<GenreUpdateDto>();
                MovieListModel movieListModel = await responseMovies.Content.ReadAsAsync<MovieListModel>();
                GenreMovieViewModel GenreMovieViewModel = new GenreMovieViewModel { GenreUpdateDto = genreUpdateDto, AllMovies = movieListModel };
                return View("_GenreUpdateView", GenreMovieViewModel);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(GenreMovieViewModel genreMovieViewModel, int[] movieIdArray)
        {
            if (ModelState.IsValid)
            {
                genreMovieViewModel.GenreUpdateDto.MovieIdArray = movieIdArray;
                HttpRequestMessage genreUpdateRequest = new HttpRequestMessage(HttpMethod.Post, "genres/update");
                genreUpdateRequest.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
                var genreUpdateJson = JsonConvert.SerializeObject(genreMovieViewModel.GenreUpdateDto);
                genreUpdateRequest.Content = new StringContent(genreUpdateJson, Encoding.UTF8, "application/json");
                HttpResponseMessage genreUpdateResponse = await client.SendAsync(genreUpdateRequest);
                if (genreUpdateResponse.IsSuccessStatusCode)
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
                GenreAddDto genreAddDto = new GenreAddDto();
                return View("_GenreCreateView", Tuple.Create(genreAddDto, allMovies));
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix = "Item1")] GenreAddDto genreAddDto)
        {
            HttpRequestMessage genreCreateRequest = new HttpRequestMessage(HttpMethod.Post, "genres/create");
            var genreCreateJson = JsonConvert.SerializeObject(genreAddDto);
            genreCreateRequest.Content = new StringContent(genreCreateJson, Encoding.UTF8, "application/json");
            genreCreateRequest.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage genreCreateResponse = await client.SendAsync(genreCreateRequest);
            if (genreCreateResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Fail");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string genreId)
        {
            HttpRequestMessage genreDeleteRequest = new HttpRequestMessage(HttpMethod.Post, "genres/delete");
            genreDeleteRequest.Headers.Add("GenreId", genreId);
            genreDeleteRequest.Headers.Add("ModifiedByName", Request.HttpContext.User.Identity.Name);
            genreDeleteRequest.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage genreDeleteResponse = await client.SendAsync(genreDeleteRequest);
            if (genreDeleteResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return NotFound();
        }

    }
}
