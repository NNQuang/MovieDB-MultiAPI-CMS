using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ClientService.Areas.Movie.Models
{
    public class GenreModel
    {
        public override string ToString()
        {
            return GenreName;
        }
        public string GenreName { get; set; }
        public ICollection<MovieModel> Movies { get; set; }

        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string Note { get; set; }
    }
}