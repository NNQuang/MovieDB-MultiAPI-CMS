using ClientService.Areas.Movie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Helpers.Comment
{
    public interface ICommentHelper
    {
        Task<CommentListModel> GetComments(string movie_title);
        CommentModel MapJsonDataToCommentModel(dynamic item);
        Task<bool> AddComment(string movie_title, string username, string date, string content);
        Task<dynamic> GetAllComments();
        Task<bool> ApproveComment(CommentModel commentModel);
        Task<bool> DisableComment(CommentModel commentModel);
        Task<bool> DeleteComment(CommentModel commentModel);
    }
}
