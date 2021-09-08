using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClientService.Areas.Movie.Models;
using ClientService.Areas.Movie.ViewModels;
using ClientService.Helpers;
using ClientService.Helpers.Comment;
using ClientService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace ClientService.Areas.Movie.Controllers
{
    [Area("Movie")]
    [Route("Movie")]
    public class MovieController : Controller
    {
        private readonly ICommentHelper _commentHelper;
        private readonly IHttpClientFactory _clientFactory;
        static HttpClient client;
        public MovieController(ICommentHelper commentHelper, IHttpClientFactory clientFactory)
        {
            _commentHelper = commentHelper;
            _clientFactory = clientFactory;
            client = _clientFactory.CreateClient("movie");
        }

        int elementCount = 15;
        public async Task<IActionResult> Index(int? pageNumber)
        {
            List<MovieModel> movies;
            HttpRequestMessage movieRequestMessage = new HttpRequestMessage(HttpMethod.Get, "movies/getallactive");
            HttpResponseMessage movieResponseMessage = await client.SendAsync(movieRequestMessage);
            if (movieResponseMessage.IsSuccessStatusCode)
            {
                MovieListModel movieList = await movieResponseMessage.Content.ReadAsAsync<MovieListModel>();
                movieList.Movies = movieList.Movies.OrderByDescending(x => x.ImdbRating).ToList();
                if (pageNumber == null)
                {
                    movies = movieList.Movies.OrderByDescending(x => x.ImdbRating).Take(elementCount).ToList();
                }
                else
                {
                    movies = movieList.Movies.OrderByDescending(x => x.ImdbRating).Skip(elementCount * pageNumber.Value + elementCount).Take(elementCount).ToList();
                }
                if (Request.Headers["x-requested-with"] == "XMLHttpRequest")
                {
                    return PartialView("_PartialMovie", movies);
                }
                return View(movies);
            }
            return View(NotFound());
        }

        [HttpGet("{movie}")]
        public async Task<IActionResult> DetailView(string movie)
        {
            //Database'i oluştururken slug kullanmak aklıma gelmediği için url'lerin seo uyumlu ve okunabilir olmması için movie title'larını dışardan bir string extension helper'ıyla slug haline getirdim.
            movie = movie.DecodeSlug();

            HttpRequestMessage movieRequestMessage = new HttpRequestMessage(HttpMethod.Get, "movies");

            // Header sadece ascii karakter kabul ettiği için bazı filmlerde problem yaşamamak adına movie title'larını html ile encode edip istek atıyoruz server side'da da decode etmek durumundayız.

            movieRequestMessage.Headers.Add("MovieTitle", HttpUtility.HtmlEncode(movie));
            HttpResponseMessage movieResponseMessage = await client.SendAsync(movieRequestMessage);
            if (movieResponseMessage.IsSuccessStatusCode)
            {
                MovieModel movieResult = await movieResponseMessage.Content.ReadAsAsync<MovieModel>();
                CommentListModel commentList = await _commentHelper.GetComments(movieResult.MovieTitle);
                return View("_DetailView",new CommentMovieViewModel { CommentListModel = commentList, MovieModel = movieResult });
            }
            return NotFound();
        }

        [HttpGet("/Genres/{id}")]
        public async Task<IActionResult> GenreMovies(int id)
        {
            HttpRequestMessage moviesByGenreRequest = new HttpRequestMessage(HttpMethod.Get, "movies/getallbygenreid");
            moviesByGenreRequest.Headers.Add("GenreId", id.ToString());
            HttpResponseMessage moviesByGenreResponse = await client.SendAsync(moviesByGenreRequest);
            if (moviesByGenreResponse.IsSuccessStatusCode)
            {
                MovieListModel genreResult = await moviesByGenreResponse.Content.ReadAsAsync<MovieListModel>();
                return View("_CommonMovieView", genreResult);
            }
            return NotFound();
        }

        [HttpGet("/Directors/{id}")]
        public async Task<IActionResult> DirectorMovies(int id)
        {
            HttpRequestMessage moviesByDirectorRequest = new HttpRequestMessage(HttpMethod.Get, "movies/GetAllByDirectorId");
            moviesByDirectorRequest.Headers.Add("DirectorId", id.ToString());
            HttpResponseMessage moviesByDirectorResponse = await client.SendAsync(moviesByDirectorRequest);
            if (moviesByDirectorResponse.IsSuccessStatusCode)
            {
                MovieListModel directorResult = await moviesByDirectorResponse.Content.ReadAsAsync<MovieListModel>();
                return View("_CommonMovieView", directorResult);
            }
            return NotFound();
        }

        [HttpGet("/Actors/{id}")]
        public async Task<IActionResult> ActorMovies(int id)
        {
            HttpRequestMessage actorsByMovieIdRequest = new HttpRequestMessage(HttpMethod.Get, "movies/GetAllByActorId");
            actorsByMovieIdRequest.Headers.Add("ActorId", id.ToString());
            HttpResponseMessage actorsByMovieIdResponse = await client.SendAsync(actorsByMovieIdRequest);
            if (actorsByMovieIdResponse.IsSuccessStatusCode)
            {
                MovieListModel actorResult = await actorsByMovieIdResponse.Content.ReadAsAsync<MovieListModel>();
                return View("_CommonMovieView", actorResult);
            }
            return NotFound();
        }

        [HttpPost("{movieName}")]
        public async Task<IActionResult> Search(string movieName)
        {
            HttpRequestMessage movieSearchRequest = new HttpRequestMessage(HttpMethod.Get, "movies/search"); // Header sadece ascii karakter kabul ettiği için bazı filmlerde problem yaşamamak için movie title'larını html ile encode edip istek atıyoruz server side'da da decode etmek durumundayız.

            movieSearchRequest.Headers.Add("Search", HttpUtility.HtmlEncode(movieName));
            HttpResponseMessage movieSearchResponse = await client.SendAsync(movieSearchRequest);
            if (movieSearchResponse.IsSuccessStatusCode)
            {
                MovieListModel movieResult = await movieSearchResponse.Content.ReadAsAsync<MovieListModel>();
                return View("_CommonMovieView", movieResult);
            }
            return NotFound();
        }


    }
}
