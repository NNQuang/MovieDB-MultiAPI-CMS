using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ClientService.Areas.Movie.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ClientService.Areas.Admin.Models;
using ClientService.Helpers.Image;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace ClientService.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DirectorController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IImageHelper _imageHelper;
        private readonly IHttpClientFactory _clientFactory;
        private static HttpClient client;

        public DirectorController(IMapper mapper, IImageHelper imageHelper, IHttpClientFactory clientFactory)
        {
            _mapper = mapper;
            _imageHelper = imageHelper;
            _clientFactory = clientFactory;
            client = _clientFactory.CreateClient("movie");
        }
        public async Task<IActionResult> Index()
        {
            HttpRequestMessage requestAllDirectorsMessage = new HttpRequestMessage(HttpMethod.Get, "directors/getall");
            requestAllDirectorsMessage.Headers.Accept.Clear();
            requestAllDirectorsMessage.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage responseAllDirectorsMessage = await client.SendAsync(requestAllDirectorsMessage);
            if (responseAllDirectorsMessage.IsSuccessStatusCode)
            {
                var data = responseAllDirectorsMessage.Content.ReadAsStringAsync().Result;
                dynamic d_data = JsonConvert.DeserializeObject<dynamic>(data).directors;
                string directorData = Convert.ToString(d_data);
                return View("_DirectorListView", directorData);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string directorId)
        {
            HttpRequestMessage requestDirector = new HttpRequestMessage(HttpMethod.Get, "directors");
            requestDirector.Headers.Add("DirectorId", directorId);
            requestDirector.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage responseDirector = await client.SendAsync(requestDirector);
            if (responseDirector.IsSuccessStatusCode)
            {
                DirectorUpdateDto directorUpdateDto = await responseDirector.Content.ReadAsAsync<DirectorUpdateDto>();
                return View("_DirectorUpdateView", directorUpdateDto); // bütün movieleri yollamadık çünkü director-movie bağlantısını movies kısmından yöneticez.
            }

            return NotFound();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(DirectorUpdateDto directorUpdateDto)
        {
            if (ModelState.IsValid)
            {
                if (directorUpdateDto.PictureFile != null)
                {
                    var imageUploadResult = await _imageHelper.UploadImage(directorUpdateDto.FullName, directorUpdateDto.PictureFile, "Director");
                    directorUpdateDto.PictureUrl = imageUploadResult.Success ? imageUploadResult.FullName : "img/Director/defaultDirector.png";
                }
                HttpRequestMessage directorUpdateRequest = new HttpRequestMessage(HttpMethod.Post, "directors/update");
                directorUpdateRequest.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
                var directorUpdateJson = JsonConvert.SerializeObject(directorUpdateDto);
                directorUpdateRequest.Content = new StringContent(directorUpdateJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.SendAsync(directorUpdateRequest);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return NotFound();

            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("_DirectorCreateView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DirectorAddDto directorAddDto)
        {
            var imageUploadResult = await _imageHelper.UploadImage(directorAddDto.FullName, directorAddDto.PictureFile, "Director");
            directorAddDto.PictureUrl = imageUploadResult.Success ? imageUploadResult.FullName : "img/Director/defaultDirector.png";
            HttpRequestMessage directorCreateRequest = new HttpRequestMessage(HttpMethod.Post, "directors/create");
            var directorCreateJson = JsonConvert.SerializeObject(directorAddDto);
            directorCreateRequest.Content = new StringContent(directorCreateJson, Encoding.UTF8, "application/json");
            directorCreateRequest.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage directorCreateResponse = await client.SendAsync(directorCreateRequest);
            if (directorCreateResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Fail");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string directorId)
        {

            HttpRequestMessage directorDeleteRequest = new HttpRequestMessage(HttpMethod.Post, "directors/delete");
            directorDeleteRequest.Headers.Add("DirectorId", directorId);
            directorDeleteRequest.Headers.Add("ModifiedByName", Request.HttpContext.User.Identity.Name);
            directorDeleteRequest.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage directorDeleteResponse = await client.SendAsync(directorDeleteRequest);
            if (directorDeleteResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}
