using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Areas.Movie.Models
{
    public class ActorListModel
    {
        public List<ActorModel> Actors { get; set; }
        public string Note { get; set; }
    }
}
