using Microsoft.AspNetCore.Http;
using ClientService.Areas.Movie.Models;
using ClientService.Helpers.ModelValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Areas.Admin.Models
{
    public class MovieAddDto
    {
        [DisplayName("Imdb Id")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public string ImdbId { get; set; }

        [DisplayName("Movie Title")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MaxLength(300, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string MovieTitle { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(3, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(50, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string CreatedByName { get; set; }


        [DisplayName("Release Date")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Duration(min)")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public int Duration { get; set; }

        [DisplayName("Genres")]
        public IList<GenreModel> Genres { get; set; }

        //[StringArrayRequired(ErrorMessage = "Please select at least 1 genre.")]
        public string[] GenreNamesArray { get; set; }

        //[StringArrayRequired(ErrorMessage = "Please select at least 1 actor.")]
        public string[] ActorNamesArray { get; set; }


        public string GenresString => GenreNamesArray != null ? string.Join(",", GenreNamesArray) : String.Empty;

        public string ActorsString => ActorNamesArray != null ? string.Join(",", ActorNamesArray) : String.Empty;

        [Required(ErrorMessage = "Please select a director.")]
        public string DirectorString { get; set; }

        public int DirectorId { get; set; }

        [DisplayName("Director")]
        public DirectorModel Director { get; set; }

        [DisplayName("Actors")]
        public IList<ActorModel> Actors { get; set; }


        [DisplayName("Plot")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(3, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(10000, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string Plot { get; set; }


        [DisplayName("Language")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(3, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(500, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string Language { get; set; }

        [MinLength(3, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(1000, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string PictureUrl { get; set; }

        [DisplayName("Movie Poster")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public IFormFile Picture { get; set; }

        [DisplayName("Imdb Rating")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public double ImdbRating { get; set; }

        [DisplayName("RottenTomatoes Rating")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public double RottenTomatoesRating { get; set; }

        [DisplayName("MetaCriticRating Rating")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public double MetaCriticRating { get; set; }
    }
}
