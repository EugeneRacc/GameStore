﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class GameGenre
    {
        public Guid GameId { get; set; }
        public Guid GenreId { get; set; }
        public Game Game { get; set; }
        public Genre Genre { get; set; }
    }
}
