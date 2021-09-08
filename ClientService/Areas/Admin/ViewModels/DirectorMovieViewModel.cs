using ClientService.Areas.Admin.Models;
using ClientService.Areas.Movie.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Areas.Admin.ViewModels
{
    public class DirectorMovieViewModel
    {
        public DirectorUpdateDto DirectorUpdateDto { get; set; }

        [DisplayName("Movies")]
        public MovieListModel AllMovies { get; set; }
    }
}
