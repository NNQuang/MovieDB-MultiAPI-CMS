using MovieService.Core.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace MovieService.Entities.Dtos
{
    public class GenreAddDto : IDto
    {
        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(4, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(100, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string GenreName { get; set; }

        [MaxLength(300, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string Note { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(3, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(50, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string CreatedByName { get; set; }

        public int[] MovieIdArray { get; set; }
    }
}