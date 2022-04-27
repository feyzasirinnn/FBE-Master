using FBE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBE.ViewModels
{
    public class AnnounceIndex
    {
        public string Search { get; set; }
        public List<Announce> Announcements { get; set; }
    }
}
