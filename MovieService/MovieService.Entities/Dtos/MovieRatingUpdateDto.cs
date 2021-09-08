using MovieService.Core.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace MovieService.Entities.Dtos
{
    public class MovieRatingUpdateDto : IDto
    {
        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(3, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(300, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string MovieTitle { get; set; }

        public double ImdbRating { get; set; }

        public double RottenTomatoesRating { get; set; }

        public double MetaCriticRating { get; set; }
    }
}