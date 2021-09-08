using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Areas.Movie.Models
{
    public class CommentModel
    {
        public string _id { get; set; }
        public string user { get; set; }
        public string content { get; set; }
        public string date { get; set; }
        public int like { get; set; }
        public string movieTitle { get; set; }
        public string IsActive { get; set; }
    }
}
