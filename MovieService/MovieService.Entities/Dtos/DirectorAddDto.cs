using Microsoft.AspNetCore.Http;
using MovieService.Core.Entities.Abstract;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieService.Entities.Dtos
{
    public class DirectorAddDto : IDto
    {
        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(4, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(100, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string FullName { get; set; }

        public string PictureUrl { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(3, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(50, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string CreatedByName { get; set; }
    }
}