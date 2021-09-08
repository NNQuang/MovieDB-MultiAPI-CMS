using MovieService.Core.Entities.Abstract;
using MovieService.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieService.Entities.Dtos
{
    public class MovieUpdateDto : IDto
    {
        [Required]
        public int Id { get; set; }


        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MaxLength(300, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string MovieTitle { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(3, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(50, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string ModifiedByName { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        public string ImdbId { get; set; }

        public int DirectorId { get; set; }

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

        [Required(ErrorMessage = "{0} can't be left empty.")]
        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        public bool IsActive { get; set; }

        public int[] ActorIdArray { get; set; }

        public int[] GenreIdArray { get; set; }
    }
}