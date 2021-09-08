using MovieService.Core.Entities.Abstract;
using MovieService.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieService.Entities.Dtos
{
    public class MovieAddDto : IDto
    {
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public string ImdbId { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MaxLength(300, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string MovieTitle { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(3, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(50, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string CreatedByName { get; set; }


        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        public int Duration { get; set; }

        public ICollection<Genre> Genres { get; set; }
        public string GenresString { get; set; }
        public string ActorsString { get; set; }
        public string DirectorString { get; set; }
        public int DirectorId { get; set; }
        public Director Director { get; set; }
        public ICollection<Actor> Actors { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(3, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(10000, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string Plot { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(3, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(500, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string Language { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(3, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(1000, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string PictureUrl { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        public double ImdbRating { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        public double RottenTomatoesRating { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        public double MetaCriticRating { get; set; }

    }
}