using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Areas.Movie.Models
{
    public class CommentListModel
    {
        public List<CommentModel> Comments { get; set; }
    }
}
