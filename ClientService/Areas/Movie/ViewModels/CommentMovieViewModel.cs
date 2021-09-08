using ClientService.Areas.Movie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Areas.Movie.ViewModels
{
    public class CommentMovieViewModel
    {
        public MovieModel MovieModel { get; set; }
        public CommentListModel CommentListModel { get; set; }
    }
}
