using Microsoft.AspNetCore.Http;
using ClientService.Areas.Movie.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Areas.Admin.Models
{
    public class MovieUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Movie Title")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MaxLength(300, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string MovieTitle { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(3, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(50, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string ModifiedByName { get; set; }

        [DisplayName("Release Date")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Duration(min)")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public int Duration { get; set; }

        public IList<ActorModel> Actors { get; set; }
        public IList<GenreModel> Genres { get; set; }

        [DisplayName("Imdb Id")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public string ImdbId { get; set; }


        [DisplayName("Director")]
        [Required(ErrorMessage = "Please select a {0}")]
        public int DirectorId { get; set; }

        [DisplayName("Movie Poster")]
        public IFormFile PictureFile { get; set; }

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


        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(3, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(1000, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string PictureUrl { get; set; }

        [DisplayName("Imdb Rating")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public double ImdbRating { get; set; }

        [DisplayName("RottenTomatoes Rating")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public double RottenTomatoesRating { get; set; }

        [DisplayName("MetaCriticRating Rating")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public double MetaCriticRating { get; set; }

        [DisplayName("Is Deleted ?")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public bool IsDeleted { get; set; }

        [DisplayName("Is Active ?")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public bool IsActive { get; set; }

        public int[] ActorIdArray { get; set; }

        public int[] GenreIdArray { get; set; }
    }
}
