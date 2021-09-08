using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Net.Http.Headers;
using ClientService.Areas.Admin.Models;
using System.Text;

namespace ClientService.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private static HttpClient client;

        public UserController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            client = _clientFactory.CreateClient("user");
        }

        public async Task<IActionResult> Index()
        {
            HttpRequestMessage requestAllUsersMessage = new HttpRequestMessage(HttpMethod.Get, "user/getall");
            requestAllUsersMessage.Headers.Accept.Clear();
            requestAllUsersMessage.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage responseAllUsersMessage = await client.SendAsync(requestAllUsersMessage);
            if (responseAllUsersMessage.IsSuccessStatusCode)
            {
                //read json response as string
                var data = responseAllUsersMessage.Content.ReadAsStringAsync().Result;
                dynamic d_data = JsonConvert.DeserializeObject<dynamic>(data).users;
                string userData = Convert.ToString(d_data);
                return View("_UserListView", userData);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int userId)
        {

            HttpRequestMessage userDeleteRequest = new HttpRequestMessage(HttpMethod.Post, $"user/delete/{userId}");
            userDeleteRequest.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage response = await client.SendAsync(userDeleteRequest);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int userId)
        {
            HttpRequestMessage requestUser = new HttpRequestMessage(HttpMethod.Get, $"user/get/{userId}");
            requestUser.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            HttpResponseMessage responseUser = await client.SendAsync(requestUser);

            if (responseUser.IsSuccessStatusCode)
            {
                UserUpdateDto userUpdateDto = await responseUser.Content.ReadAsAsync<UserUpdateDto>();
                return View("_UserUpdateView", userUpdateDto);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            HttpRequestMessage requestUserUpdateMessage = new HttpRequestMessage(HttpMethod.Post, "user/update");
            requestUserUpdateMessage.Headers.Add("Authorization", Request.Headers[HeaderNames.Authorization].ToString());
            var userUpdateJson = JsonConvert.SerializeObject(userUpdateDto);
            requestUserUpdateMessage.Content = new StringContent(userUpdateJson, Encoding.UTF8, "application/json");
            HttpResponseMessage responseUserUpdate = await client.SendAsync(requestUserUpdateMessage);

            if (responseUserUpdate.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return NotFound();
        }



    }
}
