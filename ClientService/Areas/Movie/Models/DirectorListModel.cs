﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Areas.Movie.Models
{
    public class DirectorListModel
    {
        public List<DirectorModel> Directors { get; set; }
        public string Note { get; set; }
    }
}
