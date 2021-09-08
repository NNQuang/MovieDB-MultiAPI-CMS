using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClientService.Areas.Movie.Models;
using ClientService.Helpers;
using ClientService.Helpers.Comment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CommentController : Controller
    {
        private readonly ICommentHelper _commentHelper;
        public CommentController(ICommentHelper commentHelper)
        {
            _commentHelper = commentHelper;
        }
        public async Task<IActionResult> Index()
        {
            //CommentListModel allComments = await _commentHelper.GetAllComments();
            var dynamicData = await _commentHelper.GetAllComments();
            string dynamicString = Convert.ToString(dynamicData);
            //string commentsString = dynamicString.
            return View("_CommentListView", dynamicString);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(string commentModel)
        {
            var comment = JsonConvert.DeserializeObject<CommentModel>(commentModel);
            if (await _commentHelper.ApproveComment(comment))
            {
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Disable(string commentModel)
        {
            var comment = JsonConvert.DeserializeObject<CommentModel>(commentModel);
            if (await _commentHelper.DisableComment(comment))
            {
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string commentModel)
        {
            var comment = JsonConvert.DeserializeObject<CommentModel>(commentModel);
            if (await _commentHelper.DeleteComment(comment))
            {
                return RedirectToAction("Index");
            }
            return NotFound();
        }

    }
}
