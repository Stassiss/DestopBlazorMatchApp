﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DestopBlazorMatchApp.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int SelectedPersonId { get; set; }
        public int LikedPersonId { get; set; }
    }
}
