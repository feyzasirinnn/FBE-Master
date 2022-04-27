using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBE.Models
{
    public class Eyk_Ara
    {
        public int id { get; set; }
        public Akademik_Kadro Akademik_Kadro  { get; set; }
        public EABD EABD { get; set; }
        public Ens_Gorevler Ens_Gorevler { get; set; }
        public Unvan Unvan { get; set; }
    }
}
