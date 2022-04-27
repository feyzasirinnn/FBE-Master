﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FBE.Models
{
    public class Announce
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleEng { get; set; }
        public string Description { get; set; }
        public string DescriptionEng { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public bool Enable { get; set; }
        public bool Deleted { get; set; }
        public bool IsImportant { get; set; }
    }
}
