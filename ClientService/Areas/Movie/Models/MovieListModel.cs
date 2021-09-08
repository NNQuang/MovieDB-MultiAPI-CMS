using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Areas.Movie.Models
{
    public class MovieListModel
    {
        [DisplayName("Movies")]
        public List<MovieModel> Movies { get; set; }
        public string Note { get; set; } // server'dan gelen dataya note olarak veri ekliyoruz (director, genre, actor)
    }
}
