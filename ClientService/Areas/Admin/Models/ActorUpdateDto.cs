using Microsoft.AspNetCore.Http;
using ClientService.Areas.Movie.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClientService.Areas.Admin.Models
{
    public class ActorUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Actor Name")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(4, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(100, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string FullName { get; set; }

        [DisplayName("Movies")]
        public IList<MovieModel> Movies { get; set; }

        [DisplayName("Actor Picture")]
        public IFormFile PictureFile { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        [Required(ErrorMessage = "{0} can't be left empty.")]
        [MinLength(3, ErrorMessage = "Minimum character length is {1} for {0}")]
        [MaxLength(50, ErrorMessage = "Maximum character length is {1} for {0}")]
        public string ModifiedByName { get; set; }

        [DisplayName("Is Deleted ?")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public bool IsDeleted { get; set; }

        [DisplayName("Is Active ?")]
        [Required(ErrorMessage = "{0} can't be left empty.")]
        public bool IsActive { get; set; }

        public int[] MovieIdArray { get; set; }
    }
}