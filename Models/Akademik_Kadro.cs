using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FBE.Models
{
    public class Akademik_Kadro
    {
        [Key]
        public int Sicil_No { get; set; }
        [Required]
        public string Ad { get; set; }
        public string Soyad{ get; set; }
        [Required]
        public string Kullanici_Adi { get; set; }
        public EABD EABDId { get; set; }
        public Boolean EABDBaskan { get; set; }
        public Unvan Unvan { get; set; }
        public virtual ICollection<Programs_Akademik_Kadro> Programs_Akademik_Kadro { get; set; }

    }
}
