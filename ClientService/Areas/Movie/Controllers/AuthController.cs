using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ClientService.Areas.Movie.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace ClientService.Areas.Movie.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private static HttpClient client;

        public AuthController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            client = _clientFactory.CreateClient("user");
        }
        public async Task<IActionResult> Login(UserLoginModel useLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            HttpRequestMessage loginRequestMessage = new HttpRequestMessage(HttpMethod.Post, "auth/login");
            var loginRequestJson = JsonConvert.SerializeObject(useLoginModel);
            loginRequestMessage.Content = new StringContent(loginRequestJson, Encoding.UTF8, "application/json");
            HttpResponseMessage loginResponseMessage = await client.SendAsync(loginRequestMessage);
            if (loginResponseMessage.IsSuccessStatusCode)
            {
                var token = await loginResponseMessage.Content.ReadAsAsync<TokenModel>();
                HttpContext.Session.SetString("JWToken", token.Token);
                return Redirect("~/Movie/");
            }
            return NotFound("Username or password is wrong.");
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("~/Movie/");
        }

        public IActionResult Register()
        {
            return View("~/Areas/Movie/Views/Auth/_UserRegisterView.cshtml", new UserRegisterModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel userRegisterModel)
        {
            if (ModelState.IsValid)
            {
                HttpRequestMessage registerRequestMessage = new HttpRequestMessage(HttpMethod.Post, "auth/register");
                var registerRequestJson = JsonConvert.SerializeObject(userRegisterModel);
                registerRequestMessage.Content = new StringContent(registerRequestJson, Encoding.UTF8, "application/json");
                HttpResponseMessage registerResponseMessage = await client.SendAsync(registerRequestMessage);
                if (registerResponseMessage.IsSuccessStatusCode)
                {
                    //var token = await registerResponseMessage.Content.ReadAsAsync<TokenModel>();
                    //HttpContext.Session.SetString("JWToken", token.Token);
                    return Redirect("~/Movie");
                }
            }
            return NotFound();
        }
    }
}
