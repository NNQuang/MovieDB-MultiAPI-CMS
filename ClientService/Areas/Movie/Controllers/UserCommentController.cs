using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClientService.Helpers;
using ClientService.Helpers.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Areas.Movie.Controllers
{
    public class UserCommentController : Controller
    {
        private readonly ICommentHelper _commentHelper;
        public UserCommentController(ICommentHelper commentHelper)
        {
            _commentHelper = commentHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public async Task<IActionResult> AddComment(string movie_title, string username, string content)
        {
            string date = DateTime.Now.ToShortDateString();
            await _commentHelper.AddComment(movie_title, username, date, content);
            return Redirect($"/Movie/{movie_title.EncodeSlug()}");

        }
    }
}
