using FBE.Models;
using FBE.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBE.Areas.Admin.Controllers
{
    public class EnsGorevController : Controller
    {
        FBEContext _db;

        public EnsGorevController(FBEContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var model = _db.Ens_Gorevler.ToList();
            return View(model);
        }
    }
}
