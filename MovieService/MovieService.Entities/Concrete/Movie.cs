using MovieService.Core.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace MovieService.Entities.Concrete
{
    public class Movie : BaseEntity, IEntity
    {
        public string ImdbId { get; set; }
        public string MovieTitle { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public int DirectorId { get; set; }
        public Director Director { get; set; }
        public IList<Actor> Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string PictureUrl { get; set; } = "https://yoginihead.com/wp-content/uploads/2021/02/placeholder-image-250x2501-1.jpg";
        public double ImdbRating { get; set; }
        public double RottenTomatoesRating { get; set; }
        public double MetaCriticRating { get; set; }
        //public string GenresString { get; set; }
        //public string ActorsString { get; set; }
    }
}