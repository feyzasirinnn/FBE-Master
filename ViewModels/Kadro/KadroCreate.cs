using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FBE.ViewModels.Kadro
{
    public class KadroCreate
    {
        [Required]
        public string Ad { get; set; }
        [Required]
        public string Soyad { get; set; }
        [Required]
        public string Kullanici_Adi { get; set; }
        [Required]
        public int EABDId { get; set; }
        public Boolean EABDBaskan { get; set; }
        [Required]
        public int Unvan { get; set; }
    }
}
