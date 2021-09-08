using ClientService.Areas.Admin.Models;
using ClientService.Areas.Movie.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Areas.Admin.ViewModels
{
    public class MovieUpdateDataViewModel
    {
        public MovieUpdateDto MovieUpdateDto { get; set; }
        [DisplayName("Actors")]
        public ActorListModel AllActors { get; set; }
        [DisplayName("Directors")]
        public DirectorListModel AllDirectors { get; set; }
        [DisplayName("Genres")]
        public GenreListModel AllGenres { get; set; }
    }
}
