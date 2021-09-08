using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ClientService.Areas.Movie.Models
{
    public class ActorModel
    {
        public string FullName { get; set; }
        public IList<MovieModel> Movies { get; set; }
        public string PictureUrl { get; set; }

        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string Note { get; set; }
        public int[] MovieIdArray { get; set; }
        public IFormFile PictureFile { get; set; }
    }
}