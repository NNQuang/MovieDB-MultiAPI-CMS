using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Areas.Movie.Models
{
    public class MovieModel
    {
        public string ImdbId { get; set; }
        public string MovieTitle { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public ICollection<GenreModel> Genres { get; set; }
        public int DirectorId { get; set; }
        public DirectorModel Director { get; set; }
        public ICollection<ActorModel> Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string PictureUrl { get; set; }
        public double ImdbRating { get; set; }
        public double RottenTomatoesRating { get; set; }
        public double MetaCriticRating { get; set; }

        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string Note { get; set; }
        public IFormFile PictureFile { get; set; }
    }
}
