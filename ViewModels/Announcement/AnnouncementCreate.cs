using FBE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace FBE.ViewModels
{
    public class AnnouncementCreate
    {
       
        [Required]
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public string TitleEng { get; set; }
        public string DescriptionEng { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; } 
        public bool Enable { get; set; }
        public bool IsImportant { get; set; }
        public bool Deleted { get; set; } 
    }
}
